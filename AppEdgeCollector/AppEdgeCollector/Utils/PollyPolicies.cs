using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace AppEdgeCollector.Utils
{
    public static class PollyPolicies
    {
        public static RetryPolicy CreateRetryPolicy()
        {
            // 指数退避重试 3 次（可自定义）
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)),
                    onRetry: (ex, ts, attempt, context) =>
                    {
                        // 可记录日志或上报指标
                    }
                );
        }

        public static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (ex, breakDuration) => { /* log */ },
                    onReset: () => { /* log */ }
                );
        }

        public static TimeoutPolicy CreateTimeoutPolicy()
        {
            return Policy.Timeout(TimeSpan.FromSeconds(5));
        }
    }
}
