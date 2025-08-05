using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModbusBatchReader
{
    public class ModbusBatchReader
    {
        private ModbusDataReader reader;
        private List<DataMapping> dataMappings;

        public ModbusBatchReader(string ipAddress, int port)
        {
            reader = new ModbusDataReader(ipAddress, port);
            dataMappings = new List<DataMapping>();
        }

        // 添加数据映射
        public void AddDataMapping(string name, ushort address, ModbusDataType dataType, int stringLength = 1)
        {
            var mapping = new DataMapping(name, address, dataType);
            if (dataType == ModbusDataType.String)
            {
                mapping.RegisterCount = stringLength; // 字符串长度（寄存器数量）
            }
            dataMappings.Add(mapping);
        }

        // 批量读取并转换数据
        public async Task<Dictionary<string, object>> ReadAndConvertData(byte slaveId)
        {
            var results = new Dictionary<string, object>();

            // 计算需要读取的总寄存器范围,重点就在这里
            if (dataMappings.Count == 0) return results;

            var sortedMappings = dataMappings.OrderBy(m => m.StartAddress).ToList();
            ushort startAddress = sortedMappings.First().StartAddress;
            ushort endAddress = (ushort)(sortedMappings.Last().StartAddress + sortedMappings.Last().RegisterCount - 1);
            ushort totalRegisters = (ushort)(endAddress - startAddress + 1);

            // 一次性读取所有需要的寄存器
            ushort[] rawData =await reader.ReadHoldingRegisters(slaveId, startAddress, totalRegisters);

            // 根据映射转换数据
            foreach (var mapping in dataMappings)
            {
                int offset = mapping.StartAddress - startAddress;
                object convertedValue = ConvertData(rawData, offset, mapping.DataType, mapping.RegisterCount);
                results[mapping.Name] = convertedValue;
            }

            return results;
        }

        private object ConvertData(ushort[] rawData, int offset, ModbusDataType dataType, int registerCount)
        {
            switch (dataType)
            {
                case ModbusDataType.Int16:
                    return ModbusDataConverter.ToInt16(rawData[offset]);

                case ModbusDataType.UInt16:
                    return rawData[offset];

                case ModbusDataType.Int32:
                    return ModbusDataConverter.ToInt32(rawData[offset], rawData[offset + 1]);

                case ModbusDataType.UInt32:
                    return ModbusDataConverter.ToUInt32(rawData[offset], rawData[offset + 1]);

                case ModbusDataType.Float:
                    return ModbusDataConverter.ToFloat(rawData[offset], rawData[offset + 1]);

                case ModbusDataType.Double:
                    return ModbusDataConverter.ToDouble(rawData[offset], rawData[offset + 1],
                                                       rawData[offset + 2], rawData[offset + 3]);

                case ModbusDataType.String:
                    ushort[] stringRegisters = new ushort[registerCount];
                    Array.Copy(rawData, offset, stringRegisters, 0, registerCount);
                    return ModbusDataConverter.ToString(stringRegisters);

                default:
                    return rawData[offset];
            }
        }
    }
}
