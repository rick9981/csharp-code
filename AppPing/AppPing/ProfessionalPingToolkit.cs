using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppPing
{
    /// <summary>
    /// 专业级Ping工具套件
    /// </summary>
    public class ProfessionalPingToolkit : IDisposable
    {
        private readonly List<Ping> _pingPool;
        private readonly object _lockObject = new object();

        public ProfessionalPingToolkit()
        {
            _pingPool = new List<Ping>();
        }

        /// <summary>
        /// 路由跟踪 - 类似tracert命令
        /// </summary>
        /// <param name="hostName">目标主机</param>
        /// <param name="maxHops">最大跳数</param>
        /// <returns>路由跟踪结果</returns>
        public async Task<List<TraceRouteHop>> TraceRouteAsync(string hostName, int maxHops = 30)
        {
            var results = new List<TraceRouteHop>();

            for (int ttl = 1; ttl <= maxHops; ttl++)
            {
                using var ping = new Ping();
                var options = new PingOptions(ttl, true);
                var buffer = new byte[32];

                try
                {
                    var reply = await ping.SendPingAsync(hostName, 5000, buffer, options);

                    var hop = new TraceRouteHop
                    {
                        HopNumber = ttl,
                        Address = reply.Address?.ToString(),
                        RoundTripTime = reply.RoundtripTime,
                        Status = reply.Status
                    };

                    results.Add(hop);

                    // 如果到达目标或者超过最大跳数，停止跟踪
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"跟踪完成，到达目标 {hostName}");
                        break;
                    }

                    Console.WriteLine($"第{ttl}跳: {hop}");

                    // 添加小延迟避免过于频繁的请求
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    results.Add(new TraceRouteHop
                    {
                        HopNumber = ttl,
                        Address = "超时",
                        Status = IPStatus.TimedOut,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// 网络质量测试 - 丢包率和延迟抖动分析
        /// </summary>
        /// <param name="hostName">目标主机</param>
        /// <param name="testDuration">测试持续时间（分钟）</param>
        /// <param name="intervalSeconds">测试间隔（秒）</param>
        /// <returns>网络质量报告</returns>
        public async Task<NetworkQualityReport> AnalyzeNetworkQualityAsync(
            string hostName,
            int testDuration = 5,
            int intervalSeconds = 1)
        {
            var results = new List<PingResult>();
            var endTime = DateTime.Now.AddMinutes(testDuration);

            Console.WriteLine($"开始网络质量分析: {hostName}，测试时长: {testDuration}分钟");

            using var ping = new Ping();
            var options = new PingOptions(64, true);
            var buffer = new byte[32];

            while (DateTime.Now < endTime)
            {
                try
                {
                    var reply = await ping.SendPingAsync(hostName, 5000, buffer, options);

                    results.Add(new PingResult
                    {
                        HostName = hostName,
                        Status = reply.Status,
                        RoundTripTime = reply.RoundtripTime,
                        IsSuccess = reply.Status == IPStatus.Success,
                        Timestamp = DateTime.Now
                    });

                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
                }
                catch (Exception ex)
                {
                    results.Add(new PingResult
                    {
                        HostName = hostName,
                        Status = IPStatus.Unknown,
                        IsSuccess = false,
                        ErrorMessage = ex.Message,
                        Timestamp = DateTime.Now
                    });
                }
            }

            return GenerateQualityReport(hostName, results);
        }

        /// <summary>
        /// 生成网络质量报告
        /// </summary>
        private NetworkQualityReport GenerateQualityReport(string hostName, List<PingResult> results)
        {
            var successResults = results.Where(r => r.IsSuccess).ToList();
            var responseTime = successResults.Select(r => (double)r.RoundTripTime).ToList();

            var report = new NetworkQualityReport
            {
                HostName = hostName,
                TestStartTime = results.Min(r => r.Timestamp),
                TestEndTime = results.Max(r => r.Timestamp),
                TotalPackets = results.Count,
                SuccessfulPackets = successResults.Count,
                LostPackets = results.Count - successResults.Count,
                PacketLossPercentage = (double)(results.Count - successResults.Count) / results.Count * 100
            };

            if (responseTime.Any())
            {
                report.MinLatency = responseTime.Min();
                report.MaxLatency = responseTime.Max();
                report.AverageLatency = responseTime.Average();

                // 计算延迟抖动（标准差）
                var variance = responseTime.Select(x => Math.Pow(x - report.AverageLatency, 2)).Sum() / responseTime.Count;
                report.LatencyJitter = Math.Sqrt(variance);

                // 计算百分位数
                var sorted = responseTime.OrderBy(x => x).ToList();
                report.Latency95thPercentile = sorted[(int)(sorted.Count * 0.95)];
                report.Latency99thPercentile = sorted[(int)(sorted.Count * 0.99)];
            }

            // 网络质量评级
            report.QualityGrade = EvaluateNetworkQuality(report);

            return report;
        }

        /// <summary>
        /// 评估网络质量等级
        /// </summary>
        private NetworkQualityGrade EvaluateNetworkQuality(NetworkQualityReport report)
        {
            // 综合评估：丢包率 + 平均延迟 + 延迟抖动
            var score = 100;

            // 丢包率扣分
            if (report.PacketLossPercentage > 5) score -= 30;
            else if (report.PacketLossPercentage > 1) score -= 15;
            else if (report.PacketLossPercentage > 0.1) score -= 5;

            // 平均延迟扣分
            if (report.AverageLatency > 200) score -= 25;
            else if (report.AverageLatency > 100) score -= 15;
            else if (report.AverageLatency > 50) score -= 8;

            // 延迟抖动扣分
            if (report.LatencyJitter > 50) score -= 20;
            else if (report.LatencyJitter > 20) score -= 10;
            else if (report.LatencyJitter > 10) score -= 5;

            return score switch
            {
                >= 90 => NetworkQualityGrade.Excellent,
                >= 80 => NetworkQualityGrade.Good,
                >= 70 => NetworkQualityGrade.Fair,
                >= 60 => NetworkQualityGrade.Poor,
                _ => NetworkQualityGrade.Bad
            };
        }

        /// <summary>
        /// 导出结果到JSON文件
        /// </summary>
        /// <param name="data">要导出的数据</param>
        /// <param name="fileName">文件名</param>
        public async Task ExportToJsonAsync<T>(T data, string fileName)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(fileName, json);

            Console.WriteLine($"结果已导出到: {fileName}");
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                _pingPool.ForEach(p => p?.Dispose());
                _pingPool.Clear();
            }
        }
    }

    #region 数据模型

    /// <summary>
    /// 路由跟踪跳点信息
    /// </summary>
    public class TraceRouteHop
    {
        public int HopNumber { get; set; }
        public string Address { get; set; }
        public long RoundTripTime { get; set; }
        public IPStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            if (Status == IPStatus.Success)
            {
                return $"{Address} ({RoundTripTime}ms)";
            }
            else if (Status == IPStatus.TtlExpired)
            {
                return $"{Address} ({RoundTripTime}ms) [TTL过期]";
            }
            else
            {
                return $"* 请求超时";
            }
        }
    }

    /// <summary>
    /// 网络质量报告
    /// </summary>
    public class NetworkQualityReport
    {
        public string HostName { get; set; }
        public DateTime TestStartTime { get; set; }
        public DateTime TestEndTime { get; set; }
        public int TotalPackets { get; set; }
        public int SuccessfulPackets { get; set; }
        public int LostPackets { get; set; }
        public double PacketLossPercentage { get; set; }
        public double MinLatency { get; set; }
        public double MaxLatency { get; set; }
        public double AverageLatency { get; set; }
        public double LatencyJitter { get; set; }
        public double Latency95thPercentile { get; set; }
        public double Latency99thPercentile { get; set; }
        public NetworkQualityGrade QualityGrade { get; set; }

        public override string ToString()
        {
            return $"""
            网络质量报告 - {HostName}
            测试时间: {TestStartTime:yyyy-MM-dd HH:mm:ss} - {TestEndTime:yyyy-MM-dd HH:mm:ss}
            数据包统计: {SuccessfulPackets}/{TotalPackets} (丢包率: {PacketLossPercentage:F2}%)
            延迟统计: 最小={MinLatency:F1}ms, 平均={AverageLatency:F1}ms, 最大={MaxLatency:F1}ms
            延迟抖动: {LatencyJitter:F1}ms
            95%延迟: {Latency95thPercentile:F1}ms, 99%延迟: {Latency99thPercentile:F1}ms
            网络质量: {QualityGrade} ⭐
            """;
        }
    }

    /// <summary>
    /// 网络质量等级
    /// </summary>
    public enum NetworkQualityGrade
    {
        Excellent = 5,  // 优秀
        Good = 4,       // 良好  
        Fair = 3,       // 一般
        Poor = 2,       // 较差
        Bad = 1         // 很差
    }

    #endregion
}
