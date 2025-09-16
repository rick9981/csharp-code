using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AppEdgeCollector.Services
{
    public class ShortTermBuffer
    {
        private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();
        private readonly ILogger<ShortTermBuffer> _logger;
        private readonly string _persistPath = "buffer_wal.jsonl";

        public ShortTermBuffer(ILogger<ShortTermBuffer> logger)
        {
            _logger = logger;
        }

        public Task AddAsync(byte[] data)
        {
            if (data == null)
            {
                // 占位：记录错误事件
                _logger.LogWarning("Add null data to short term buffer (placeholder).");
                return Task.CompletedTask;
            }

            _queue.Enqueue(data);

            // 简单策略：队列过大时落盘
            if (_queue.Count > 20)
            {
                _ = PersistToDiskAsync();
            }

            return Task.CompletedTask;
        }

        private async Task PersistToDiskAsync()
        {
            try
            {
                using var fs = new FileStream(_persistPath, FileMode.Append, FileAccess.Write, FileShare.None);
                while (_queue.TryDequeue(out var item))
                {
                    var line = Convert.ToBase64String(item) + Environment.NewLine;
                    var bytes = System.Text.Encoding.UTF8.GetBytes(line);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
                _logger.LogInformation("写入数据成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed persisting buffer to disk.");
            }
        }

        public async Task FlushAsync()
        {
            // 将内存与磁盘数据上报或安全落盘。这里示例为持久化所有内存数据。
            await PersistToDiskAsync();
        }
    }
}
