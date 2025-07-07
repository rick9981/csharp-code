using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AppPing
{
    /// <summary>
    /// 高级Ping工具类 - 支持并发检测
    /// </summary>
    public class AdvancedPingTool : IDisposable
    {
        private readonly List<Ping> _pingInstances;
        private readonly SemaphoreSlim _semaphore;
        private readonly PingOptions _options;

        public AdvancedPingTool(int maxConcurrency = 10)
        {
            _pingInstances = new List<Ping>();
            _semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            _options = new PingOptions(64, true);
        }

        /// <summary>
        /// 批量并发Ping检测
        /// </summary>
        /// <param name="hosts">主机列表</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="pingCount">每个主机Ping次数</param>
        /// <returns>检测结果列表</returns>
        public async Task<List<BatchPingResult>> BatchPingAsync(
            IEnumerable<string> hosts,
            int timeout = 5000,
            int pingCount = 4)
        {
            var tasks = hosts.Select(host => PingHostMultipleTimesAsync(host, timeout, pingCount));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        /// <summary>
        /// 对单个主机执行多次Ping并统计结果
        /// </summary>
        private async Task<BatchPingResult> PingHostMultipleTimesAsync(string host, int timeout, int count)
        {
            await _semaphore.WaitAsync(); // 控制并发数

            try
            {
                var ping = new Ping();
                _pingInstances.Add(ping); // 记录实例用于后续释放

                var results = new List<PingResult>();
                byte[] buffer = new byte[32]; // 32字节数据包

                // 执行多次Ping
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        var reply = await ping.SendPingAsync(host, timeout, buffer, _options);
                        results.Add(new PingResult
                        {
                            HostName = host,
                            Status = reply.Status,
                            RoundTripTime = reply.RoundtripTime,
                            IsSuccess = reply.Status == IPStatus.Success
                        });
                    }
                    catch (Exception ex)
                    {
                        results.Add(new PingResult
                        {
                            HostName = host,
                            Status = IPStatus.Unknown,
                            IsSuccess = false,
                            ErrorMessage = ex.Message
                        });
                    }

                    // 避免过于频繁的请求
                    if (i < count - 1) await Task.Delay(100);
                }

                return CalculateBatchResult(host, results);
            }
            finally
            {
                _semaphore.Release(); // 释放信号量
            }
        }

        /// <summary>
        /// 计算批量Ping的统计结果
        /// </summary>
        private BatchPingResult CalculateBatchResult(string host, List<PingResult> results)
        {
            var successResults = results.Where(r => r.IsSuccess).ToList();

            return new BatchPingResult
            {
                HostName = host,
                TotalCount = results.Count,
                SuccessCount = successResults.Count,
                FailureCount = results.Count - successResults.Count,
                SuccessRate = (double)successResults.Count / results.Count * 100,
                MinRoundTripTime = successResults.Any() ? successResults.Min(r => r.RoundTripTime) : 0,
                MaxRoundTripTime = successResults.Any() ? successResults.Max(r => r.RoundTripTime) : 0,
                AverageRoundTripTime = successResults.Any() ? successResults.Average(r => r.RoundTripTime) : 0,
                IsReachable = successResults.Any()
            };
        }

        public void Dispose()
        {
            _pingInstances.ForEach(p => p?.Dispose());
            _pingInstances.Clear();
            _semaphore?.Dispose();
        }
    }

    /// <summary>
    /// 批量Ping结果统计
    /// </summary>
    public class BatchPingResult
    {
        public string HostName { get; set; }
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public double SuccessRate { get; set; }
        public long MinRoundTripTime { get; set; }
        public long MaxRoundTripTime { get; set; }
        public double AverageRoundTripTime { get; set; }
        public bool IsReachable { get; set; }

        public override string ToString()
        {
            return $"{HostName}: {SuccessCount}/{TotalCount} ({SuccessRate:F1}%) " +
                   $"最小/平均/最大 = {MinRoundTripTime}/{AverageRoundTripTime:F0}/{MaxRoundTripTime}ms";
        }
    }
}
