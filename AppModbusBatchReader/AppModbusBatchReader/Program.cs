namespace AppModbusBatchReader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // 创建批量读取器
                var batchReader = new ModbusBatchReader("127.0.0.1", 502);

                // 配置数据映射
                batchReader.AddDataMapping("温度", 100, ModbusDataType.Float);
                batchReader.AddDataMapping("压力", 105, ModbusDataType.Int32);
                batchReader.AddDataMapping("状态", 110, ModbusDataType.UInt16);
                batchReader.AddDataMapping("设备名称", 80, ModbusDataType.String, 8); // 8个寄存器的字符串

                // 批量读取并转换
                var results = await batchReader.ReadAndConvertData(1); // 从站ID = 1

                // 显示结果
                foreach (var kvp in results)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value} ({kvp.Value.GetType().Name})");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
        }
    }
}
