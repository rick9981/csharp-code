using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppKepTool
{
    public partial class Form1 : Form
    {
        private KepwareApiClient _apiClient;


        public Form1()
        {
            InitializeComponent();
            InitializeClient();
        }

        private void InitializeClient()
        {
            // 可以从配置文件读取这些参数
            string baseUrl = "http://localhost:57412";
            string username = "administrator";
            string password = "123456";

            _apiClient = new KepwareApiClient(baseUrl, username, password);
            AppendLog($"已连接到Kepware服务器: {baseUrl}");
        }

        private void AppendLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => {
                    txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                    txtLog.ScrollToCaret();
                }));
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                txtLog.ScrollToCaret();
            }
        }

        // 导出指定channel下所有设备的所有tag
        private async void btnExport_Click(object sender, EventArgs e)
        {
            string channel = txtChannel.Text.Trim();
            if (string.IsNullOrEmpty(channel))
            {
                AppendLog("❌ 请输入Channel名!");
                return;
            }

            SaveFileDialog saveDlg = new SaveFileDialog()
            {
                Filter = "Json文件|*.json",
                FileName = $"{channel}_tags_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                btnExport.Enabled = false;
                btnImport.Enabled = false;

                try
                {
                    AppendLog($"🔄 开始导出 Channel: {channel}");

                    // 1. 检查channel是否存在
                    string channelInfo = await _apiClient.GetAsync($"/config/v1/project/channels/{channel}");
                    if (channelInfo.Contains("error"))
                    {
                        AppendLog($"❌ Channel '{channel}' 不存在或无法访问");
                        return;
                    }

                    // 2. 获取设备列表
                    string deviceListJson = await _apiClient.GetAsync($"/config/v1/project/channels/{channel}/devices");
                    if (deviceListJson.Contains("error"))
                    {
                        AppendLog($"❌ 无法获取设备列表: {deviceListJson}");
                        return;
                    }

                    JArray devices = JArray.Parse(deviceListJson);
                    AppendLog($"📋 找到 {devices.Count} 个设备");

                    JArray deviceTagsArray = new JArray();
                    int totalTags = 0;

                    foreach (var device in devices)
                    {
                        string deviceName = device["common.ALLTYPES_NAME"].ToString();
                        AppendLog($"🔄 正在处理设备: {deviceName}");

                        // 3. 获取设备信息
                        string deviceInfo = await _apiClient.GetAsync($"/config/v1/project/channels/{channel}/devices/{deviceName}");

                        // 4. 获取设备的tags
                        string tagsJson = await _apiClient.GetAsync($"/config/v1/project/channels/{channel}/devices/{deviceName}/tags");

                        if (!tagsJson.Contains("error"))
                        {
                            JArray tags = JArray.Parse(tagsJson);
                            totalTags += tags.Count;

                            deviceTagsArray.Add(new JObject
                            {
                                ["device_name"] = deviceName,
                                ["device_info"] = JObject.Parse(deviceInfo),
                                ["tags"] = tags
                            });

                            AppendLog($"✅ 设备 {deviceName}: {tags.Count} 个标签");
                        }
                        else
                        {
                            AppendLog($"⚠️ 设备 {deviceName}: 无法获取标签");
                        }
                    }

                    // 5. 构建完整导出数据
                    JObject exportData = new JObject
                    {
                        ["export_info"] = new JObject
                        {
                            ["channel_name"] = channel,
                            ["export_time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["total_devices"] = devices.Count,
                            ["total_tags"] = totalTags
                        },
                        ["channel_info"] = JObject.Parse(channelInfo),
                        ["devices"] = deviceTagsArray
                    };

                    // 6. 保存文件
                    File.WriteAllText(saveDlg.FileName, exportData.ToString(Formatting.Indented), Encoding.UTF8);
                    AppendLog($"✅ 导出完成! 文件: {saveDlg.FileName}");
                    AppendLog($"📊 统计: {devices.Count} 个设备, {totalTags} 个标签");
                }
                catch (Exception ex)
                {
                    AppendLog($"❌ 导出错误: {ex.Message}");
                }
                finally
                {
                    btnExport.Enabled = true;
                    btnImport.Enabled = true;
                }
            }
        }

        // 导入tags到指定channel
        private async void btnImport_Click(object sender, EventArgs e)
        {
            string channel = txtChannel.Text.Trim();
            if (string.IsNullOrEmpty(channel))
            {
                AppendLog("❌ 请输入Channel名!");
                return;
            }

            OpenFileDialog openDlg = new OpenFileDialog() { Filter = "Json文件|*.json" };
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                btnExport.Enabled = false;
                btnImport.Enabled = false;

                try
                {
                    AppendLog($"🔄 开始导入到 Channel: {channel}");

                    string json = File.ReadAllText(openDlg.FileName, Encoding.UTF8);
                    JObject importData = JObject.Parse(json);

                    // 验证文件格式
                    if (importData["devices"] == null)
                    {
                        AppendLog("❌ 配置文件格式不正确，缺少 'devices' 节点!");
                        return;
                    }

                    // 显示导入信息
                    if (importData["export_info"] != null)
                    {
                        var exportInfo = importData["export_info"];
                        AppendLog($"📋 文件信息: 导出时间={exportInfo["export_time"]}, " +
                                 $"设备数={exportInfo["total_devices"]}, 标签数={exportInfo["total_tags"]}");
                    }

                    var devices = importData["devices"] as JArray;
                    int successCount = 0;
                    int errorCount = 0;

                    foreach (var deviceObj in devices)
                    {
                        string deviceName = deviceObj["device_name"].ToString();
                        AppendLog($"🔄 正在导入设备: {deviceName}");

                        // 检查设备是否存在，如不存在可选择创建
                        string deviceCheck = await _apiClient.GetAsync($"/config/v1/project/channels/{channel}/devices/{deviceName}");
                        if (deviceCheck.Contains("not be found"))
                        {
                            AppendLog($"⚠️ 设备 {deviceName} 不存在，尝试创建...");

                            // 从导入数据中获取设备信息并创建设备
                            if (deviceObj["device_info"] != null)
                            {
                                var devPayload = deviceObj["device_info"].ToObject<Dictionary<string, object>>();
                                devPayload.Remove("PROJECT_ID");
                                ///config/v1/project/channels/LMES/devices
                                string createResult = await _apiClient.PostAsync(
                                    $"/config/v1/project/channels/{channel}/devices",
                                    devPayload);

                                if (!createResult.Contains("error"))
                                {
                                    AppendLog($"✅ 设备 {deviceName} 创建成功");
                                }
                                else
                                {
                                    AppendLog($"❌ 设备 {deviceName} 创建失败: {createResult}");
                                    continue;
                                }
                            }
                        }

                        // 导入标签
                        var tags = deviceObj["tags"] as JArray;
                        foreach (var tag in tags)
                        {
                            string tagName = tag["common.ALLTYPES_NAME"].ToString();

                            // 先尝试查询标签
                            string getResult = await _apiClient.GetAsync(
                                $"/config/v1/project/channels/{channel}/devices/{deviceName}/tags/{tagName}");

                            if (getResult.Contains("not be found"))
                            {
                                var tagPayload = tag.ToObject<Dictionary<string, object>>();
                                tagPayload.Remove("PROJECT_ID");

                                // 尝试POST创建
                                string postResult = await _apiClient.PostAsync(
                                    $"/config/v1/project/channels/{channel}/devices/{deviceName}/tags",
                                    tagPayload);

                                if (!postResult.Contains("error"))
                                {
                                    successCount++;
                                    AppendLog($"➕ 创建标签: {deviceName}/{tagName}");
                                }
                                else
                                {
                                    errorCount++;
                                    AppendLog($"❌ 标签创建失败: {deviceName}/{tagName} - {postResult}");
                                }
                            }
                            else
                            {
                                successCount++;
                                AppendLog($"🔄 更新标签: {deviceName}/{tagName}");
                            }
                        }
                    }

                    AppendLog($"✅ 导入完成! 成功: {successCount}, 失败: {errorCount}");
                }
                catch (Exception ex)
                {
                    AppendLog($"❌ 导入错误: {ex.Message}");
                }
                finally
                {
                    btnExport.Enabled = true;
                    btnImport.Enabled = true;
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _apiClient?.Dispose();
        }
    }
}
