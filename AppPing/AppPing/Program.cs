namespace AppPing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var toolkit = new ProfessionalPingToolkit();

            Console.WriteLine("=== 专业级Ping工具套件演示 ===\n");

            var target = "www.bing.com";

            // 1. 路由跟踪
            Console.WriteLine("1. 执行路由跟踪...");
            var traceRoute = await toolkit.TraceRouteAsync(target, maxHops: 15);
            await toolkit.ExportToJsonAsync(traceRoute, $"traceroute_{target}_{DateTime.Now:yyyyMMdd_HHmmss}.json");

            Console.WriteLine("\n" + "=".PadRight(50, '=') + "\n");

            // 2. 网络质量分析
            Console.WriteLine("2. 执行网络质量分析...");
            var qualityReport = await toolkit.AnalyzeNetworkQualityAsync(target, testDuration: 2, intervalSeconds: 1);
            Console.WriteLine(qualityReport);
            await toolkit.ExportToJsonAsync(qualityReport, $"quality_report_{target}_{DateTime.Now:yyyyMMdd_HHmmss}.json");

            toolkit.Dispose();

            Console.WriteLine("\n测试完成，按任意键退出...");
            Console.ReadKey();
        }
    }
}
