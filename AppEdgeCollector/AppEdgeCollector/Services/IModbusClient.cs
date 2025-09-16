using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEdgeCollector.Services
{
    public interface IModbusClient : IAsyncDisposable
    {
        Task ConnectAsync(CancellationToken cancellationToken = default);
        Task DisconnectAsync();
        Task<bool> IsConnectedAsync();
        // 读取寄存器或线圈，返回 raw bytes 或解析后的类型
        Task<byte[]> ReadRegistersAsync(ushort startAddress, ushort count, CancellationToken cancellationToken = default);
        // 写操作示例
        Task WriteRegistersAsync(ushort startAddress, byte[] data, CancellationToken cancellationToken = default);
    }
}
