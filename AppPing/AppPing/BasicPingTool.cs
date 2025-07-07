using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AppPing
{
    /// <summary>
    /// 基础Ping工具类
    /// </summary>
    public class BasicPingTool
    {
        private readonly Ping _ping;
        private readonly PingOptions _options;

        public BasicPingTool()
        {
            _ping = new Ping();
            // 设置Ping选项：TTL=64，不允许分片
            _options = new PingOptions(64, true);
        }

        /// <summary>
        /// 执行单次Ping操作
        /// </summary>
        /// <param name="hostNameOrAddress">目标主机名或IP地址</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <returns>Ping结果</returns>
        public async Task<PingResult> PingAsync(string hostNameOrAddress, int timeout = 5000)
        {
            try
            {
                // 准备发送的数据包（32字节，与Windows ping一致）
                byte[] buffer = Encoding.ASCII.GetBytes("Hello World! This is a ping test.");

                // 执行异步Ping操作
                PingReply reply = await _ping.SendPingAsync(hostNameOrAddress, timeout, buffer, _options);

                return new PingResult
                {
                    HostName = hostNameOrAddress,
                    Status = reply.Status,
                    RoundTripTime = reply.RoundtripTime,
                    IsSuccess = reply.Status == IPStatus.Success,
                    ErrorMessage = reply.Status != IPStatus.Success ? reply.Status.ToString() : null
                };
            }
            catch (Exception ex)
            {
                return new PingResult
                {
                    HostName = hostNameOrAddress,
                    Status = IPStatus.Unknown,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _ping?.Dispose();
        }
    }

    /// <summary>
    /// Ping结果数据模型
    /// </summary>
    public class PingResult
    {
        public string HostName { get; set; }
        public IPStatus Status { get; set; }
        public long RoundTripTime { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public override string ToString()
        {
            if (IsSuccess)
            {
                return $"来自 {HostName} 的回复: 时间={RoundTripTime}ms TTL=64";
            }
            else
            {
                return $"Ping {HostName} 失败: {ErrorMessage}";
            }
        }
    }
}
