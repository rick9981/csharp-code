using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;

namespace AppModbusBatchReader
{
    /// <summary>
    /// Modbus数据读取类，就是基本功能
    /// </summary>
    public class ModbusDataReader
    {
        private IModbusMaster master;
        private TcpClient tcpClient;
        private bool disposed = false;

        public ModbusDataReader(string ipAddress, int port)
        {
            tcpClient = new TcpClient(ipAddress, port);
            master = ModbusIpMaster.CreateIp(tcpClient);
        }

        // 批量读取保持寄存器（Holding Registers）
        public async Task<ushort[]> ReadHoldingRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            return await master.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfPoints);
        }

        // 批量读取输入寄存器（Input Registers）
        public async Task<ushort[]> ReadInputRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            return await master.ReadInputRegistersAsync(slaveId, startAddress, numberOfPoints);
        }
    }
}
