using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AppEdgeHostedService.Utils;
using AppEdgeHostedService.Services;

namespace AppEdgeHostedService.Services
{
    public class PollingManager
    {
        private readonly IModbusClient _modbusClient;
        private readonly ILogger<PollingManager> _logger;
        private readonly ObjectPool<byte[]> _bufferPool;
        private readonly ShortTermBuffer _shortTermBuffer;
        private readonly PollingOptions _options;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task? _backgroundTask;
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

            await _shortTermBuffer.FlushAsync();
        }

        private async Task RunAsync(CancellationToken token)
        {
            try
            {
                if (!_modbusClient.IsConnected)
                {
                    await _modbusClient.ConnectAsync(token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动时连接 Modbus 设备失败");
            }

            var consumers = new List<Task>();
            for (int i = 0; i < _options.Concurrency; i++)
            {
                consumers.Add(Task.Run(() => ConsumerLoopAsync(token), token));
            }

            var pollCounter = 0;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await _workChannel.Writer.WriteAsync(async ct =>
                    {
                        var buf = _bufferPool.Rent();
                        try
                        {
                            // 轮询不同类型的数据
                            if (pollCounter % 10 == 0) // 每10次轮询一次线圈
                            {
                                var coilData = await _modbusClient.ReadCoilsAsync(0, 8, ct);
                                await ProcessCoilData(coilData);
                            }
                            else // 读取保持寄存器
                            {
                                var registerData = await _modbusClient.ReadRegistersAsync(pollCounter % 100, 10, ct);
                                await ProcessRegisterData(registerData, pollCounter % 100);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "轮询任务错误");
                            await _shortTermBuffer.AddErrorDataAsync($"轮询错误: {ex.Message}");
                        }
                        finally
                        {
                            _bufferPool.Return(buf);
                        }
                    }, token);

                    pollCounter++;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "调度轮询工作失败");
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
                    _logger.LogError(ex, "工作线程中未处理的异常");
                }
            }
        }

        private async Task ProcessRegisterData(byte[] data, int startAddress)
        {
            for (int i = 0; i < data.Length; i += 2)
            {
                if (i + 1 < data.Length)
                {
                    var value = (ushort)((data[i] << 8) | data[i + 1]);
                    await _shortTermBuffer.AddRegisterDataAsync(startAddress + i / 2, value);
                }
            }
        }

        private async Task ProcessCoilData(bool[] coils)
        {
            for (int i = 0; i < coils.Length; i++)
            {
                await _shortTermBuffer.AddCoilDataAsync(i, coils[i]);
            }
        }
    }
}