using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEdgeCollector.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppEdgeCollector
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        private readonly PollingManager _pollingManager;

        public Worker(ILogger<Worker> logger, PollingManager pollingManager)
        {
            _logger = logger;
            _pollingManager = pollingManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 中文日志
            _logger.LogInformation("EdgeCollector 工作器已启动。");
            _pollingManager.Start(cancellationToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EdgeCollector 工作器停止中。");
            await _pollingManager.StopAsync(cancellationToken);
            _logger.LogInformation("EdgeCollector 工作器已停止。");
        }
    }
}
