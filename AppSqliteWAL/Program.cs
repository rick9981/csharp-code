using System.Text;

namespace AppSqliteWAL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            WalPerformanceTest walPerformanceTest = new WalPerformanceTest("test.db");
            await walPerformanceTest.CompareModePerformanceAsync();
            Console.ReadKey();
        }
    }
}
