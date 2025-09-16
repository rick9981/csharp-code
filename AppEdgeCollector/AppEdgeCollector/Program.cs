using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AppEdgeCollector.Services;
using AppEdgeCollector.Utils;
using Microsoft.Extensions.Options;

namespace AppEdgeCollector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddLogging(x =>
                    {
                        x.AddSimpleConsole(option =>
                        {
                            option.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                            option.IncludeScopes = true;
                        });
                        
                        x.SetMinimumLevel(LogLevel.Information);
                    });

                    // 配置项绑定（示例）
                    services.Configure<PollingOptions>(ctx.Configuration.GetSection("Polling"));

                    // 注册 Modbus 客户端（可替换为真实实现）
                    services.AddSingleton<IModbusClient, SimulatedModbusClient>();

                    // 对象池示例（用于缓冲/复用大量对象）
                    services.AddSingleton(typeof(ObjectPool<byte[]>), sp => new ObjectPool<byte[]>(
                        () => new byte[4096],
                        maxSize: 16
                    ));

                    // 短期缓存/离线队列
                    services.AddSingleton<ShortTermBuffer>();

                    // Polling 管理器（负责并发采集、排程）
                    services.AddSingleton<PollingManager>();

                    // Worker（IHostedService）
                    services.AddHostedService<Worker>();
                })
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }
    }
}
