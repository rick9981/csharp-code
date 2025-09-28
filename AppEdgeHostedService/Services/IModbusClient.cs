using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEdgeHostedService.Services
{

    public interface IModbusClient
    {
        Task ConnectAsync(CancellationToken cancellationToken = default);
        Task<byte[]> ReadRegistersAsync(int startAddress, int count, CancellationToken cancellationToken = default);
        Task<bool[]> ReadCoilsAsync(int startAddress, int count, CancellationToken cancellationToken = default);
        Task WriteRegisterAsync(int address, ushort value, CancellationToken cancellationToken = default);
        Task DisconnectAsync();
        bool IsConnected { get; }
        string ConnectionString { get; set; }
    }
}
