using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppEdgeHostedService.Services
{

    public class ModbusClient : IModbusClient
    {
        private readonly ILogger<ModbusClient> _logger;
        private readonly PollingOptions _options;
        private readonly Random _random = new Random();
        private bool _isConnected;

        public ModbusClient(ILogger<ModbusClient> logger, IOptions<PollingOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            ConnectionString = _options.ConnectionString;
        }

        public bool IsConnected => _isConnected;
        public string ConnectionString { get; set; }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("正在连接到 Modbus 设备: {ConnectionString}", ConnectionString);

                // 模拟连接过程
                await Task.Delay(2000, cancellationToken);

                // 在实际应用中，这里应该建立真实的Modbus TCP连接
                // 例如: _tcpClient.Connect(host, port);

                _isConnected = true;
                _logger.LogInformation("成功连接到 Modbus 设备");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("连接操作被取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "连接 Modbus 设备失败");
                _isConnected = false;
                throw;
            }
        }

        public async Task<byte[]> ReadRegistersAsync(int startAddress, int count, CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                throw new InvalidOperationException("设备未连接");
            }

            try
            {
                // 模拟网络延迟
                await Task.Delay(_random.Next(10, 100), cancellationToken);

                // 模拟读取保持寄存器数据
                var data = new byte[count * 2]; // 每个寄存器2字节

                // 生成模拟的传感器数据
                for (int i = 0; i < count; i++)
                {
                    var value = (ushort)(1000 + _random.Next(-100, 100)); // 模拟温度传感器
                    var bytes = BitConverter.GetBytes(value);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(bytes); // Modbus 使用大端序
                    }
                    data[i * 2] = bytes[0];
                    data[i * 2 + 1] = bytes[1];
                }

                _logger.LogDebug("从地址 {StartAddress} 读取 {Count} 个寄存器成功", startAddress, count);

                return data;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("读取寄存器操作被取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "读取寄存器失败: StartAddress={StartAddress}, Count={Count}", startAddress, count);
                throw;
            }
        }

        public async Task<bool[]> ReadCoilsAsync(int startAddress, int count, CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                throw new InvalidOperationException("设备未连接");
            }

            try
            {
                await Task.Delay(_random.Next(10, 50), cancellationToken);

                var coils = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    coils[i] = _random.Next(0, 2) == 1;
                }

                _logger.LogDebug("从地址 {StartAddress} 读取 {Count} 个线圈成功", startAddress, count);

                return coils;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "读取线圈失败: StartAddress={StartAddress}, Count={Count}", startAddress, count);
                throw;
            }
        }

        public async Task WriteRegisterAsync(int address, ushort value, CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                throw new InvalidOperationException("设备未连接");
            }

            try
            {
                await Task.Delay(_random.Next(10, 50), cancellationToken);
                _logger.LogDebug("向地址 {Address} 写入值 {Value} 成功", address, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "写入寄存器失败: Address={Address}, Value={Value}", address, value);
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_isConnected)
            {
                await Task.Delay(500); // 模拟断开延迟
                _isConnected = false;
                _logger.LogInformation("已断开 Modbus 连接");
            }
        }
    }
}
