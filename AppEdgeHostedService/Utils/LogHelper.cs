using Microsoft.Extensions.Logging;

namespace AppEdgeHostedService.Utils
{
    public class LogHelper
    {
        private readonly ILogger<LogHelper> _logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            _logger = logger;
        }

        public void LogPerformance(string operation, TimeSpan elapsed, Dictionary<string, object>? parameters = null)
        {
            var message = $"性能统计: {operation} 耗时 {elapsed.TotalMilliseconds:F2}ms";

            if (parameters != null && parameters.Count > 0)
            {
                var paramStr = string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"));
                message += $", 参数: {paramStr}";
            }

            if (elapsed.TotalMilliseconds > 1000)
            {
                _logger.LogWarning(message);
            }
            else
            {
                _logger.LogDebug(message);
            }
        }

        public void LogDataQuality(string source, int totalCount, int errorCount, double errorRate)
        {
            var message = $"数据质量统计: {source} - 总计: {totalCount}, 错误: {errorCount}, 错误率: {errorRate:P2}";

            if (errorRate > 0.05) // 错误率超过5%
            {
                _logger.LogWarning(message);
            }
            else
            {
                _logger.LogInformation(message);
            }
        }
    }
}