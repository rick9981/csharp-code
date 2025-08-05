using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModbusBatchReader
{
    public static class ModbusDataConverter
    {
        // 单个寄存器转换为 Int16
        public static short ToInt16(ushort register)
        {
            return (short)register;
        }

        // 两个寄存器转换为 Int32 (高位在前)
        public static int ToInt32(ushort highRegister, ushort lowRegister)
        {
            return (int)((highRegister << 16) | lowRegister);
        }

        // 两个寄存器转换为 UInt32 (高位在前)
        public static uint ToUInt32(ushort highRegister, ushort lowRegister)
        {
            return (uint)((highRegister << 16) | lowRegister);
        }

        // 两个寄存器转换为 Float
        public static float ToFloat(ushort highRegister, ushort lowRegister)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(lowRegister & 0xFF);
            bytes[1] = (byte)((lowRegister >> 8) & 0xFF);
            bytes[2] = (byte)(highRegister & 0xFF);
            bytes[3] = (byte)((highRegister >> 8) & 0xFF);
            return BitConverter.ToSingle(bytes, 0);
        }

        // 四个寄存器转换为 Double
        public static double ToDouble(ushort reg1, ushort reg2, ushort reg3, ushort reg4)
        {
            byte[] bytes = new byte[8];
            bytes[0] = (byte)(reg4 & 0xFF);
            bytes[1] = (byte)((reg4 >> 8) & 0xFF);
            bytes[2] = (byte)(reg3 & 0xFF);
            bytes[3] = (byte)((reg3 >> 8) & 0xFF);
            bytes[4] = (byte)(reg2 & 0xFF);
            bytes[5] = (byte)((reg2 >> 8) & 0xFF);
            bytes[6] = (byte)(reg1 & 0xFF);
            bytes[7] = (byte)((reg1 >> 8) & 0xFF);
            return BitConverter.ToDouble(bytes, 0);
        }

        // 寄存器转换为字符串 ASCII
        public static string ToString(ushort[] registers)
        {
            List<byte> bytes = new List<byte>();
            foreach (ushort register in registers)
            {
                bytes.Add((byte)(register >> 8));  // 高字节
                bytes.Add((byte)(register & 0xFF)); // 低字节
            }
            return System.Text.Encoding.ASCII.GetString(bytes.ToArray()).TrimEnd('\0');
        }
    }
}
