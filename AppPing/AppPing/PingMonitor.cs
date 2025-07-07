using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AppPing
{
    /// <summary>
    /// Ping监控工具 - 支持实时监控和报警
    /// </summary>
    public class PingMonitor : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentDictionary<string, MonitoringHost> _monitoringHosts;
        private readonly Timer _reportTimer;
        private bool _disposed = false;

        public event EventHandler<PingAlertEventArgs> OnAlert;
        public event EventHandler<PingStatusChangedEventArgs> OnStatusChanged;

        public PingMonitor()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _monitoringHosts = new ConcurrentDictionary<string, MonitoringHost>();

            // 每分钟生成一次监控报告
            _reportTimer = new Timer(GenerateReport, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// 添加监控主机
        /// </summary>
        /// <param name="hostName">主机名或IP</param>
        /// <param name="interval">检测间隔（秒）</param>
        /// <param name="alertThreshold">连续失败次数报警阈值</param>
        public void AddHost(string hostName, int interval = 30, int alertThreshold = 3)
        {
            if (_monitoringHosts.ContainsKey(hostName))
            {
                Console.WriteLine($"主机 {hostName} 已在监控列表中");
                return;
            }

            var monitoringHost = new MonitoringHost
            {
                HostName = hostName,
                Interval = TimeSpan.FromSeconds(interval),
                AlertThreshold = alertThreshold,
                Statistics = new PingStatistics()
            };

            _monitoringHosts.TryAdd(hostName, monitoringHost);

            // 启动该主机的监控任务
            Task.Run(() => MonitorHostAsync(monitoringHost, _cancellationTokenSource.Token));

            Console.WriteLine($"已添加主机 {hostName} 到监控列表，检测间隔: {interval}秒");
        }

        /// <summary>
        /// 监控单个主机
        /// </summary>
        private async Task MonitorHostAsync(MonitoringHost host, CancellationToken cancellationToken)
        {
            using var ping = new Ping();
            var options = new PingOptions(64, true);
            var buffer = new byte[32];

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var reply = await ping.SendPingAsync(host.HostName, 5000, buffer, options);
                    var isSuccess = reply.Status == IPStatus.Success;

                    // 更新统计信息
                    UpdateStatistics(host, isSuccess, reply.RoundtripTime);

                    // 检查状态变化
                    CheckStatusChange(host, isSuccess);

                    // 检查是否需要报警
                    CheckAlert(host, isSuccess);

                    await Task.Delay(host.Interval, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break; // 正常取消
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"监控 {host.HostName} 时发生异常: {ex.Message}");
                    await Task.Delay(host.Interval, cancellationToken);
                }
            }
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        private void UpdateStatistics(MonitoringHost host, bool isSuccess, long responseTime)
        {
            var stats = host.Statistics;
            stats.TotalAttempts++;

            if (isSuccess)
            {
                stats.SuccessfulAttempts++;
                stats.TotalResponseTime += responseTime;
                stats.LastSuccessTime = DateTime.Now;
                host.ConsecutiveFailures = 0; // 重置连续失败计数
            }
            else
            {
                stats.FailedAttempts++;
                host.ConsecutiveFailures++;
                stats.LastFailureTime = DateTime.Now;
            }

            stats.AvailabilityPercentage = (double)stats.SuccessfulAttempts / stats.TotalAttempts * 100;
            if (stats.SuccessfulAttempts > 0)
            {
                stats.AverageResponseTime = (double)stats.TotalResponseTime / stats.SuccessfulAttempts;
            }
        }

        /// <summary>
        /// 检查状态变化
        /// </summary>
        private void CheckStatusChange(MonitoringHost host, bool currentStatus)
        {
            if (host.LastStatus.HasValue && host.LastStatus.Value != currentStatus)
            {
                OnStatusChanged?.Invoke(this, new PingStatusChangedEventArgs
                {
                    HostName = host.HostName,
                    PreviousStatus = host.LastStatus.Value,
                    CurrentStatus = currentStatus,
                    Timestamp = DateTime.Now
                });
            }

            host.LastStatus = currentStatus;
        }

        /// <summary>
        /// 检查报警条件
        /// </summary>
        private void CheckAlert(MonitoringHost host, bool isSuccess)
        {
            if (!isSuccess && host.ConsecutiveFailures >= host.AlertThreshold)
            {
                // 避免重复报警（每次连续失败只报警一次）
                if (host.ConsecutiveFailures == host.AlertThreshold)
                {
                    OnAlert?.Invoke(this, new PingAlertEventArgs
                    {
                        HostName = host.HostName,
                        AlertType = AlertType.HostUnreachable,
                        Message = $"主机 {host.HostName} 连续 {host.ConsecutiveFailures} 次Ping失败",
                        Timestamp = DateTime.Now,
                        Statistics = host.Statistics
                    });
                }
            }
        }

        /// <summary>
        /// 生成监控报告
        /// </summary>
        private void GenerateReport(object state)
        {
            Console.WriteLine($"\n=== Ping监控报告 ({DateTime.Now:yyyy-MM-dd HH:mm:ss}) ===");

            foreach (var kvp in _monitoringHosts)
            {
                var host = kvp.Value;
                var stats = host.Statistics;

                Console.WriteLine($"主机: {host.HostName}");
                Console.WriteLine($"  可用性: {stats.AvailabilityPercentage:F2}% ({stats.SuccessfulAttempts}/{stats.TotalAttempts})");
                Console.WriteLine($"  平均响应时间: {stats.AverageResponseTime:F2}ms");
                Console.WriteLine($"  最后成功时间: {stats.LastSuccessTime:HH:mm:ss}");
                Console.WriteLine($"  连续失败次数: {host.ConsecutiveFailures}");
                Console.WriteLine();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _cancellationTokenSource?.Cancel();
            _reportTimer?.Dispose();
            _cancellationTokenSource?.Dispose();
            _disposed = true;

            Console.WriteLine("Ping监控服务已停止");
        }
    }

    #region 辅助类和事件参数

    /// <summary>
    /// 监控主机信息
    /// </summary>
    public class MonitoringHost
    {
        public string HostName { get; set; }
        public TimeSpan Interval { get; set; }
        public int AlertThreshold { get; set; }
        public int ConsecutiveFailures { get; set; }
        public bool? LastStatus { get; set; }
        public PingStatistics Statistics { get; set; }
    }

    /// <summary>
    /// Ping统计信息
    /// </summary>
    public class PingStatistics
    {
        public int TotalAttempts { get; set; }
        public int SuccessfulAttempts { get; set; }
        public int FailedAttempts { get; set; }
        public double AvailabilityPercentage { get; set; }
        public long TotalResponseTime { get; set; }
        public double AverageResponseTime { get; set; }
        public DateTime? LastSuccessTime { get; set; }
        public DateTime? LastFailureTime { get; set; }
    }

    /// <summary>
    /// 报警事件参数
    /// </summary>
    public class PingAlertEventArgs : EventArgs
    {
        public string HostName { get; set; }
        public AlertType AlertType { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public PingStatistics Statistics { get; set; }
    }

    /// <summary>
    /// 状态变化事件参数
    /// </summary>
    public class PingStatusChangedEventArgs : EventArgs
    {
        public string HostName { get; set; }
        public bool PreviousStatus { get; set; }
        public bool CurrentStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 报警类型
    /// </summary>
    public enum AlertType
    {
        HostUnreachable,
        HighLatency,
        PacketLoss
    }

    #endregion
}
