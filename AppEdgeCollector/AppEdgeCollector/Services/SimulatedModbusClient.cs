using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEdgeCollector.Services
{
    // 模拟实现（用于开发和测试）
    public class SimulatedModbusClient : IModbusClient
    {
        private bool _connected = false;

        public Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            _connected = true;
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            _connected = false;
            return Task.CompletedTask;
        }

        public Task<bool> IsConnectedAsync()
            => Task.FromResult(_connected);

        public Task<byte[]> ReadRegistersAsync(ushort startAddress, ushort count, CancellationToken cancellationToken = default)
        {
            // 返回模拟数据
            var result = new byte[count * 2];
            var rnd = new Random();
            rnd.NextBytes(result);
            return Task.FromResult(result);
        }

        public Task WriteRegistersAsync(ushort startAddress, byte[] data, CancellationToken cancellationToken = default)
        {
            // 模拟写入
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            _connected = false;
            return ValueTask.CompletedTask;
        }
    }
}
