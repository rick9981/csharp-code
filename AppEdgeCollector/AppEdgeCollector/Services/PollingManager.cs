using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using AppEdgeCollector.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppEdgeCollector.Services
{
    public class PollingManager
    {
        private readonly IModbusClient _modbusClient;
        private readonly ILogger<PollingManager> _logger;
        private readonly ObjectPool<byte[]> _bufferPool;
        private readonly ShortTermBuffer _shortTermBuffer;
        private readonly PollingOptions _options;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _backgroundTask;
        private readonly Channel<Func<CancellationToken, Task>> _workChannel;

        public PollingManager(IModbusClient modbusClient,
                              ILogger<PollingManager> logger,
                              ObjectPool<byte[]> bufferPool,
                              ShortTermBuffer shortTermBuffer,
                              IOptions<PollingOptions> options)
        {
            _modbusClient = modbusClient;
            _logger = logger;
            _bufferPool = bufferPool;
            _shortTermBuffer = shortTermBuffer;
            _options = options.Value;
            _workChannel = Channel.CreateBounded<Func<CancellationToken, Task>>(
                new BoundedChannelOptions(_options.Concurrency * 4)
                {
                    FullMode = BoundedChannelFullMode.Wait
                });
        }

        public void Start(CancellationToken hostCancellation)
        {
            // 链接外部 host cancellation 与内部 CTS
            hostCancellation.Register(() => _cts.Cancel());

            _backgroundTask = Task.Run(() => RunAsync(_cts.Token));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            if (_backgroundTask != null)
            {
                await Task.WhenAny(_backgroundTask, Task.Delay(TimeSpan.FromSeconds(10), cancellationToken));
            }

            // 尝试将短期缓存中的数据上报
            await _shortTermBuffer.FlushAsync();
        }

        private async Task RunAsync(CancellationToken token)
        {
            // 在启动阶段建立连接
            try
            {
                await _modbusClient.ConnectAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to Modbus device at startup.");
            }

            // 启动工作消费者（并发）
            var consumers = new List<Task>();
            for (int i = 0; i < _options.Concurrency; i++)
            {
                consumers.Add(Task.Run(() => ConsumerLoopAsync(token), token));
            }

            // 主轮询循环（可替换为根据配置的多个采集任务）
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // 创建一个采集任务并写入 channel
                    await _workChannel.Writer.WriteAsync(async ct =>
                    {
                        // 取缓冲
                        var buf = _bufferPool.Rent();
                        try
                        {
                            // 使用 Polly 策略包装 Modbus 调用（示例）
                            // 这里直接调用客户端方法，但在真实场景将用 Policy.ExecuteAsync(...)
                            var data = await _modbusClient.ReadRegistersAsync(0, 10, ct);
                            String hex = BitConverter.ToString(data).Replace("-", " ");
                            _logger.LogInformation("Polled data: {data}", hex);
                            // 处理数据（解析、转换、上报或缓存）
                            await _shortTermBuffer.AddAsync(data);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error in polling task.");
                            // 离线缓存/持久化
                            await _shortTermBuffer.AddAsync(null); // 示例
                        }
                        finally
                        {
                            _bufferPool.Return(buf);
                        }
                    }, token);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed scheduling polling work.");
                }

                await Task.Delay(_options.PollIntervalMs, token);
            }

            _workChannel.Writer.Complete();
            await Task.WhenAll(consumers);
        }

        private async Task ConsumerLoopAsync(CancellationToken token)
        {
            await foreach (var work in _workChannel.Reader.ReadAllAsync(token))
            {
                try
                {
                    await work(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception in worker.");
                }
            }
        }
    }
}
