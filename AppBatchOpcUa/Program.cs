using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System.Threading;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using AppBatchOpcUa;
using System.Collections.Concurrent;

namespace OpcUaBatchClient
{

    /// <summary>
    /// OPC UA节点数据结构
    /// </summary>
    public class OpcUaNode
    {
        public NodeId NodeId { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }
        public StatusCode StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 批量读写结果
    /// </summary>
    public class BatchOperationResult
    {
        public bool Success { get; set; }
        public List<OpcUaNode> Results { get; set; } = new List<OpcUaNode>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// 高效批量OPC UA客户端
    /// </summary>
    public class OpcUaBatchClient : IDisposable
    {
        private Session _session;
        private ApplicationInstance _application;
        private readonly object _lockObject = new object();
        private Dictionary<string, Subscription> _subscriptions = new Dictionary<string, Subscription>();

        public bool IsConnected => _session != null && _session.Connected;

        /// <summary>
        /// 连接到OPC UA服务器
        /// </summary>
        public async Task<bool> ConnectAsync(string endpointUrl, bool useSecurity = false,
            string applicationName = "OPC UA Batch Client")
        {
            try
            {
                _application = new ApplicationInstance
                {
                    ApplicationName = applicationName,
                    ApplicationType = ApplicationType.Client,
                    ConfigSectionName = "OpcUaBasicClient"
                };

                await _application.LoadApplicationConfiguration(false);
                await _application.CheckApplicationInstanceCertificate(false, 0);

                var selectedEndpoint = CoreClientUtils.SelectEndpoint(endpointUrl, useSecurity);
                var endpointConfiguration = EndpointConfiguration.Create(_application.ApplicationConfiguration);
                var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);

                // 创建会话
                _session = await Session.Create(
                    _application.ApplicationConfiguration,
                    endpoint,
                    false,
                    $"{applicationName}_Session",
                    60000,
                    null,
                    null);

                Console.WriteLine($"成功连接到OPC UA服务器: {endpointUrl}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败: {ex.Message}");
                return false;
            }
        }

        public async Task<BatchOperationResult> BatchReadAsync(List<NodeId> nodeIds, int maxBatchSize = 100)
        {
            var result = new BatchOperationResult();

            if (!IsConnected)
            {
                result.Errors.Add("未连接到OPC UA服务器");
                return result;
            }

            try
            {
                var batches = SplitIntoBatches(nodeIds, maxBatchSize);

                // 控制最大并发数
                int maxConcurrency = Environment.ProcessorCount * 2;
                using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

                // 创建任务列表
                var tasks = batches.Select(async batch =>
                {
                    await semaphore.WaitAsync(); // 等待获取信号量
                    try
                    {
                        return await Task.Run(() =>
                        {
                            var readValueIdCollection = new ReadValueIdCollection();
                            foreach (var nodeId in batch)
                            {
                                readValueIdCollection.Add(new ReadValueId
                                {
                                    NodeId = nodeId,
                                    AttributeId = Attributes.Value
                                });
                            }

                            // 这里可能需要锁，取决于OPC UA库的线程安全性
                            lock (_lockObject)
                            {
                                _session.Read(
                                    null,
                                    0,
                                    TimestampsToReturn.Both,
                                    readValueIdCollection,
                                    out DataValueCollection dataValues,
                                    out DiagnosticInfoCollection diagnosticInfos);

                                return new { batch, dataValues };
                            }
                        });
                    }
                    finally
                    {
                        semaphore.Release(); // 释放信号量
                    }
                }).ToList();

                // 等待所有任务完成
                var allResults = await Task.WhenAll(tasks);

                // 处理结果
                foreach (var res in allResults)
                {
                    for (int i = 0; i < res.batch.Count && i < res.dataValues.Count; i++)
                    {
                        var node = new OpcUaNode
                        {
                            NodeId = res.batch[i],
                            Value = res.dataValues[i].Value,
                            StatusCode = res.dataValues[i].StatusCode,
                            Timestamp = res.dataValues[i].SourceTimestamp,
                            DisplayName = res.batch[i].ToString()
                        };

                        result.Results.Add(node);

                        if (!StatusCode.IsGood(res.dataValues[i].StatusCode))
                        {
                            result.Errors.Add($"读取节点 {res.batch[i]} 失败: {res.dataValues[i].StatusCode}");
                        }
                    }
                }

                result.Success = result.Errors.Count == 0;
                Console.WriteLine($"批量读取完成，成功读取 {result.Results.Count} 个节点，最大并发: {maxConcurrency}");
            }
            catch (Exception ex)
            {
                result.Errors.Add($"批量读取异常: {ex.Message}");
                Console.WriteLine($"批量读取失败: {ex.Message}");
            }

            return result;
        }
        /// <summary>
        /// 批量订阅节点变化
        /// </summary>
        public async Task<bool> BatchSubscribeAsync(List<NodeId> nodeIds,
            Action<OpcUaNode> onValueChanged,
            int publishingInterval = 1000,
            string subscriptionName = "DefaultSubscription")
        {
            if (!IsConnected)
            {
                Console.WriteLine("未连接到OPC UA服务器");
                return false;
            }

            try
            {
                lock (_lockObject)
                {
                    // 如果订阅已存在，先删除
                    if (_subscriptions.ContainsKey(subscriptionName))
                    {
                        _subscriptions[subscriptionName].Delete(true);
                        _subscriptions.Remove(subscriptionName);
                    }

                    // 创建新订阅
                    var subscription = new Subscription(_session.DefaultSubscription)
                    {
                        PublishingInterval = publishingInterval,
                        DisplayName = subscriptionName
                    };

                    _session.AddSubscription(subscription);
                    subscription.Create();

                    // 添加监控项
                    foreach (var nodeId in nodeIds)
                    {
                        var monitoredItem = new MonitoredItem(subscription.DefaultItem)
                        {
                            DisplayName = nodeId.ToString(),
                            StartNodeId = nodeId,
                            AttributeId = Attributes.Value,
                            SamplingInterval = publishingInterval / 2
                        };

                        monitoredItem.Notification += (item, e) =>
                        {
                            foreach (var value in item.DequeueValues())
                            {
                                var node = new OpcUaNode
                                {
                                    NodeId = nodeId,
                                    DisplayName = item.DisplayName,
                                    Value = value.Value,
                                    StatusCode = value.StatusCode,
                                    Timestamp = value.SourceTimestamp
                                };

                                onValueChanged?.Invoke(node);
                            }
                        };

                        subscription.AddItem(monitoredItem);
                    }

                    subscription.ApplyChanges();
                    _subscriptions[subscriptionName] = subscription;
                }

                Console.WriteLine($"成功订阅 {nodeIds.Count} 个节点，订阅名称: {subscriptionName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"批量订阅失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (_lockObject)
            {
                foreach (var subscription in _subscriptions.Values)
                {
                    try
                    {
                        subscription.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"删除订阅失败: {ex.Message}");
                    }
                }
                _subscriptions.Clear();
            }
        }

        /// <summary>
        /// 浏览节点
        /// </summary>
        public List<ReferenceDescription> BrowseNode(NodeId nodeId)
        {
            if (!IsConnected)
                return new List<ReferenceDescription>();

            try
            {
                var browser = new Browser(_session);
                return browser.Browse(nodeId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"浏览节点失败: {ex.Message}");
                return new List<ReferenceDescription>();
            }
        }

        /// <summary>
        /// 分批处理辅助方法
        /// </summary>
        private List<List<T>> SplitIntoBatches<T>(List<T> items, int batchSize)
        {
            var batches = new List<List<T>>();
            for (int i = 0; i < items.Count; i += batchSize)
            {
                batches.Add(items.GetRange(i, Math.Min(batchSize, items.Count - i)));
            }
            return batches;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                UnsubscribeAll();
                _session?.Close();
                _session = null;
                Console.WriteLine("已断开与OPC UA服务器的连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"断开连接时出错: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }

    /// <summary>
    /// 示例程序
    /// </summary>
    internal class Program
    {
        static List<JNodeId> ExtractNodeIdsFromFile(string filePath)
        {
            var nodeIds = new List<JNodeId>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"⚠️ 文件不存在: {filePath}");
                return nodeIds;
            }

            try
            {
                // 读取并解析JSON文件
                string jsonContent = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var config = JsonSerializer.Deserialize<LmesRootConfig>(jsonContent, options);
                if (config == null)
                {
                    Console.WriteLine("❌ 无法解析JSON配置文件");
                    return nodeIds;
                }

                Console.WriteLine($"✓ 成功读取配置文件");
                Console.WriteLine($"- 通道名称: {config.ExportInfo.ChannelName}");
                Console.WriteLine($"- 设备总数: {config.ExportInfo.TotalDevices}");
                Console.WriteLine($"- 标签总数: {config.ExportInfo.TotalTags}");

                // 解析devices字符串
                var devicesDict = JsonSerializer.Deserialize<List<DeviceContainer>>(config.Devices.ToString(), options);
                if (devicesDict == null)
                {
                    Console.WriteLine("❌ 无法解析设备信息");
                    return nodeIds;
                }

                string channelName = config.ExportInfo.ChannelName;

                // 遍历所有设备和标签
                foreach (var device in devicesDict)
                {
                    string deviceName = device.DeviceName;

                    Console.WriteLine($"处理设备: {deviceName} (包含 {device.Tags.Count} 个标签)");

                    // 遍历设备中的所有标签
                    foreach (var tagEntry in device.Tags)
                    {
                        var nodeId = new JNodeId
                        {
                            Namespace = 2,
                            DisplayName = tagEntry.Name,
                            DeviceName = deviceName,
                            ChannelName = channelName,
                            TagAddress = tagEntry.Address,
                            DataType = tagEntry.DataType,
                            ReadWriteAccess = tagEntry.ReadWriteAccess
                        };

                        // 生成完整的NodeId字符串
                        nodeId.GenerateFullNodeId();
                        nodeIds.Add(nodeId);
                    }
                }

                Console.WriteLine($"✓ 总共提取到 {nodeIds.Count} 个NodeId");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 解析文件时出错: {ex.Message}");
            }

            return nodeIds;
        }

        static async Task Main(string[] args)
        {
            var client = new OpcUaBatchClient();

            try
            {
                // 连接到OPC UA服务器
                var connected = await client.ConnectAsync("opc.tcp://127.0.0.1:49320");
                if (!connected)
                {
                    Console.WriteLine("连接失败，程序退出");
                    return;
                }

                string jsonFilePath = "LMES_tags_20250727_094313.json";
                var jnodeIds = ExtractNodeIdsFromFile(jsonFilePath);
                var nodeIds = new List<NodeId>();
                foreach (var item in jnodeIds)
                {
                    nodeIds.Add(new NodeId(item.FullNodeId));
                }

                // 批量读取示例
                Console.WriteLine("\n=== 批量读取测试 ===");
                var readResult = await client.BatchReadAsync(nodeIds);
                if (readResult.Success)
                {
                    foreach (var node in readResult.Results)
                    {
                        Console.WriteLine($"节点: {node.NodeId}, 值: {node.Value}, 状态: {node.StatusCode}");
                    }
                }
                else
                {
                    Console.WriteLine("批量读取失败:");
                    readResult.Errors.ForEach(Console.WriteLine);
                }


                // 订阅示例
                Console.WriteLine("\n=== 批量订阅测试 ===");
                await client.BatchSubscribeAsync(nodeIds, (node) =>
                {
                    Console.WriteLine($"[订阅通知] 节点: {node.NodeId}, 新值: {node.Value}, 时间: {node.Timestamp:HH:mm:ss.fff}");
                }, 500);

                Console.WriteLine("\n监控节点变化中，按任意键停止...");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序异常: {ex.Message}");
            }
            finally
            {
                client.Dispose();
                Console.WriteLine("\n程序结束，按任意键退出...");
                Console.ReadKey();
            }
        }
    }
}
