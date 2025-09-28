using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AppEdgeHostedService.Extensions;
using AppEdgeHostedService.Forms;

namespace AppEdgeHostedService
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();

            using (var scope = host.Services.CreateScope())
            {
                var mainForm = scope.ServiceProvider.GetRequiredService<FrmMain>();
                Application.Run(mainForm);
            }
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureServices(services =>
                {
                    services.AddEdgeCollectorServices();
                    services.AddScoped<FrmMain>();
                });
        }
    }
}