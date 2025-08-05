using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModbusBatchReader
{
    public enum ModbusDataType
    {
        Int16,
        UInt16,
        Int32,
        UInt32,
        Float,
        Double,
        String
    }

    public class DataMapping
    {
        public string Name { get; set; }
        public ushort StartAddress { get; set; }
        public ModbusDataType DataType { get; set; }
        public int RegisterCount { get; set; }

        public DataMapping(string name, ushort startAddress, ModbusDataType dataType)
        {
            Name = name;
            StartAddress = startAddress;
            DataType = dataType;
            RegisterCount = GetRegisterCount(dataType);
        }

        private int GetRegisterCount(ModbusDataType dataType)
        {
            switch (dataType)
            {
                case ModbusDataType.Int16:
                case ModbusDataType.UInt16:
                    return 1;
                case ModbusDataType.Int32:
                case ModbusDataType.UInt32:
                case ModbusDataType.Float:
                    return 2;
                case ModbusDataType.Double:
                    return 4;
                case ModbusDataType.String:
                    return 1; // 默认值，需要具体指定
                default:
                    return 1;
            }
        }
    }
}
