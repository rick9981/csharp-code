using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AppEdgeHostedService.Services;
using AppEdgeHostedService.Utils;

namespace AppEdgeHostedService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEdgeCollectorServices(this IServiceCollection services)
        {
            // 配置选项
            services.Configure<PollingOptions>(options =>
            {
                options.PollIntervalMs = 1000;
                options.Concurrency = 4;
                options.MaxRetries = 3;
                options.ConnectionString = "192.168.1.100:502";
                options.MaxBufferSize = 10000;
                options.BatchSize = 100;
            });

            // 注册服务
            services.AddSingleton<ObjectPool<byte[]>>(provider =>
                new ObjectPool<byte[]>(() => new byte[1024], maxObjects: 50));

            services.AddSingleton<IModbusClient, ModbusClient>();
            services.AddSingleton<ShortTermBuffer>();
            services.AddSingleton<PollingManager>();
            services.AddSingleton<LogHelper>();

            return services;
        }


    }
}