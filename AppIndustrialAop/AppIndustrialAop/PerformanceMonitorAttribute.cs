using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace AppIndustrialAop
{
    // 性能监控属性定义
    [AttributeUsage(AttributeTargets.Method)]
    public class PerformanceMonitorAttribute : Attribute
    {
        public string Description { get; set; }
        public PerformanceMonitorAttribute(string description = "")
        {
            Description = description;
        }
    }

    // 性能监控拦截器
    public class PerformanceInterceptor : IInterceptor
    {
        private readonly IPerformanceLogger _logger;

        public PerformanceInterceptor(IPerformanceLogger logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var stopwatch = Stopwatch.StartNew();
            var methodName = $"{invocation.TargetType.Name}.{invocation.Method.Name}";

            try
            {
                // 执行目标方法
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                _logger.LogError(methodName, ex);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMs = stopwatch.ElapsedMilliseconds;

                // 记录性能数据
                _logger.LogPerformance(methodName, elapsedMs);

                // 如果执行时间超过阈值，记录警告
                if (elapsedMs > 500)
                {
                    _logger.LogWarning(methodName, elapsedMs);
                }
            }
        }
    }

}