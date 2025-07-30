using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModbusDemo
{
    public static class DataTypeHelper
    {
        public enum DataType
        {
            UInt16,
            Int16,
            UInt32,
            Int32,
            Int64,
            UInt64,
            Float,
            Double
        }

        public enum ByteOrder
        {
            /// <summary>
            /// ABCD - 大端模式，高字节在前，高寄存器在前
            /// 寄存器1: AB, 寄存器2: CD
            /// 适用于大多数PLC和工业设备
            /// </summary>
            ABCD,

            /// <summary>
            /// BADC - 大端字节序，但寄存器内字节交换
            /// 寄存器1: BA, 寄存器2: DC
            /// </summary>
            BADC,

            /// <summary>
            /// CDAB - 小端寄存器序，但寄存器内为大端
            /// 寄存器1: CD, 寄存器2: AB
            /// </summary>
            CDAB,

            /// <summary>
            /// DCAB - 完全小端模式，低字节在前，低寄存器在前
            /// 寄存器1: DC, 寄存器2: BA
            /// 类似PC的字节序
            /// </summary>
            DCBA
        }

        public static int GetRegisterCount(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.UInt16:
                case DataType.Int16:
                    return 1;
                case DataType.UInt32:
                case DataType.Int32:
                case DataType.Float:
                    return 2;
                case DataType.Int64:
                case DataType.UInt64:
                case DataType.Double:
                    return 4;
                default:
                    return 1;
            }
        }

        public static object ConvertFromRegisters(ushort[] registers, DataType dataType, ByteOrder byteOrder)
        {
            if (registers == null || registers.Length == 0)
                throw new ArgumentException("寄存器数组不能为空");

            byte[] bytes = RegistersToBytes(registers, byteOrder);

            switch (dataType)
            {
                case DataType.UInt16:
                    return BitConverter.ToUInt16(bytes, 0);
                case DataType.Int16:
                    return BitConverter.ToInt16(bytes, 0);
                case DataType.UInt32:
                    return BitConverter.ToUInt32(bytes, 0);
                case DataType.Int32:
                    return BitConverter.ToInt32(bytes, 0);
                case DataType.Int64:
                    return BitConverter.ToInt64(bytes, 0);
                case DataType.UInt64:
                    return BitConverter.ToUInt64(bytes, 0);
                case DataType.Float:
                    return BitConverter.ToSingle(bytes, 0);
                case DataType.Double:
                    return BitConverter.ToDouble(bytes, 0);
                default:
                    return registers[0];
            }
        }

        public static ushort[] ConvertToRegisters(object value, DataType dataType, ByteOrder byteOrder)
        {
            byte[] bytes;

            switch (dataType)
            {
                case DataType.UInt16:
                    bytes = BitConverter.GetBytes(Convert.ToUInt16(value));
                    break;
                case DataType.Int16:
                    bytes = BitConverter.GetBytes(Convert.ToInt16(value));
                    break;
                case DataType.UInt32:
                    bytes = BitConverter.GetBytes(Convert.ToUInt32(value));
                    break;
                case DataType.Int32:
                    bytes = BitConverter.GetBytes(Convert.ToInt32(value));
                    break;
                case DataType.Int64:
                    bytes = BitConverter.GetBytes(Convert.ToInt64(value));
                    break;
                case DataType.UInt64:
                    bytes = BitConverter.GetBytes(Convert.ToUInt64(value));
                    break;
                case DataType.Float:
                    bytes = BitConverter.GetBytes(Convert.ToSingle(value));
                    break;
                case DataType.Double:
                    bytes = BitConverter.GetBytes(Convert.ToDouble(value));
                    break;
                default:
                    bytes = BitConverter.GetBytes(Convert.ToUInt16(value));
                    break;
            }

            return BytesToRegisters(bytes, byteOrder);
        }

        private static byte[] RegistersToBytes(ushort[] registers, ByteOrder byteOrder)
        {
            // 先按照寄存器顺序重新排列
            ushort[] reorderedRegisters = ReorderRegisters(registers, byteOrder);

            byte[] bytes = new byte[reorderedRegisters.Length * 2];

            // 将每个寄存器转换为字节，并按照字节序排列
            for (int i = 0; i < reorderedRegisters.Length; i++)
            {
                byte[] regBytes = BitConverter.GetBytes(reorderedRegisters[i]);

                switch (byteOrder)
                {
                    case ByteOrder.ABCD:
                        // AB：高字节在前
                        bytes[i * 2] = regBytes[1];     // 高字节
                        bytes[i * 2 + 1] = regBytes[0]; // 低字节
                        break;
                    case ByteOrder.BADC:
                        // BA：低字节在前
                        bytes[i * 2] = regBytes[0];     // 低字节
                        bytes[i * 2 + 1] = regBytes[1]; // 高字节
                        break;
                    case ByteOrder.CDAB:
                        // CD：高字节在前
                        bytes[i * 2] = regBytes[1];     // 高字节
                        bytes[i * 2 + 1] = regBytes[0]; // 低字节
                        break;
                    case ByteOrder.DCBA:
                        // DC：低字节在前
                        bytes[i * 2] = regBytes[0];     // 低字节
                        bytes[i * 2 + 1] = regBytes[1]; // 高字节
                        break;
                }
            }

            return bytes;
        }

        private static ushort[] BytesToRegisters(byte[] bytes, ByteOrder byteOrder)
        {
            int regCount = (bytes.Length + 1) / 2;

            // 按照字节序将字节转换为寄存器
            ushort[] registers = new ushort[regCount];

            for (int i = 0; i < regCount; i++)
            {
                byte byte1 = i * 2 < bytes.Length ? bytes[i * 2] : (byte)0;
                byte byte2 = i * 2 + 1 < bytes.Length ? bytes[i * 2 + 1] : (byte)0;

                switch (byteOrder)
                {
                    case ByteOrder.ABCD:
                        // AB：第一个字节是高字节
                        registers[i] = (ushort)((byte1 << 8) | byte2);
                        break;
                    case ByteOrder.BADC:
                        // BA：第一个字节是低字节
                        registers[i] = (ushort)((byte2 << 8) | byte1);
                        break;
                    case ByteOrder.CDAB:
                        // CD：第一个字节是高字节
                        registers[i] = (ushort)((byte1 << 8) | byte2);
                        break;
                    case ByteOrder.DCBA:
                        // DC：第一个字节是低字节
                        registers[i] = (ushort)((byte2 << 8) | byte1);
                        break;
                }
            }

            // 按照寄存器顺序重新排列回原始顺序
            return ReorderRegisters(registers, byteOrder);
        }

        /// <summary>
        /// 根据字节序对寄存器进行重新排序
        /// </summary>
        private static ushort[] ReorderRegisters(ushort[] registers, ByteOrder byteOrder)
        {
            if (registers.Length <= 1)
                return registers;

            ushort[] result = new ushort[registers.Length];

            switch (byteOrder)
            {
                case ByteOrder.ABCD:
                    // ABCD：保持原顺序，寄存器1=AB, 寄存器2=CD
                    Array.Copy(registers, result, registers.Length);
                    break;

                case ByteOrder.BADC:
                    // BADC：保持原顺序，但每个寄存器内字节会交换
                    Array.Copy(registers, result, registers.Length);
                    break;

                case ByteOrder.CDAB:
                    // CDAB：寄存器顺序交换，寄存器1=CD, 寄存器2=AB
                    for (int i = 0; i < registers.Length; i++)
                    {
                        result[i] = registers[registers.Length - 1 - i];
                    }
                    break;

                case ByteOrder.DCBA:
                    // DCBA：寄存器顺序交换，且每个寄存器内字节也会交换
                    for (int i = 0; i < registers.Length; i++)
                    {
                        result[i] = registers[registers.Length - 1 - i];
                    }
                    break;
            }

            return result;
        }

        public static string GetDataTypeDescription(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.UInt16: return "无符号16位整数 (1寄存器)";
                case DataType.Int16: return "有符号16位整数 (1寄存器)";
                case DataType.UInt32: return "无符号32位整数 (2寄存器)";
                case DataType.Int32: return "有符号32位整数 (2寄存器)";
                case DataType.Int64: return "有符号64位整数 (4寄存器)";
                case DataType.UInt64: return "无符号64位整数 (4寄存器)";
                case DataType.Float: return "32位浮点数 (2寄存器)";
                case DataType.Double: return "64位双精度浮点数 (4寄存器)";
                default: return "未知类型";
            }
        }

        /// <summary>
        /// 获取字节序的详细描述
        /// </summary>
        public static string GetByteOrderDescription(ByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case ByteOrder.ABCD:
                    return "ABCD - 大端模式 (寄存器1:AB, 寄存器2:CD)";
                case ByteOrder.BADC:
                    return "BADC - 大端寄存器序+字节交换 (寄存器1:BA, 寄存器2:DC)";
                case ByteOrder.CDAB:
                    return "CDAB - 小端寄存器序 (寄存器1:CD, 寄存器2:AB)";
                case ByteOrder.DCBA:
                    return "DCBA - 完全小端模式 (寄存器1:DC, 寄存器2:BA)";
                default:
                    return "未知字节序";
            }
        }
    }
}