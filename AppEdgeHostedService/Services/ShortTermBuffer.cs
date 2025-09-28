using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AppEdgeHostedService.Models;

namespace AppEdgeHostedService.Services
{
    public class ShortTermBuffer
    {
        private readonly ConcurrentQueue<CollectionData> _buffer = new();
        private readonly ILogger<ShortTermBuffer> _logger;
        private readonly PollingOptions _options;
        private int _count = 0;

        public event EventHandler<CollectionData>? DataReceived;

        public ShortTermBuffer(ILogger<ShortTermBuffer> logger, IOptions<PollingOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public int Count => _count;

        public async Task AddRegisterDataAsync(int address, ushort value)
        {
            var data = new CollectionData
            {
                Timestamp = DateTime.Now,
                Address = address,
                Value = value.ToString(),
                DataType = "保持寄存器",
                Status = "正常",
                Quality = "良好"
            };

            await AddDataAsync(data);
        }

        public async Task AddCoilDataAsync(int address, bool value)
        {
            var data = new CollectionData
            {
                Timestamp = DateTime.Now,
                Address = address,
                Value = value ? "1" : "0",
                DataType = "线圈",
                Status = "正常",
                Quality = "良好"
            };

            await AddDataAsync(data);
        }

        public async Task AddErrorDataAsync(string errorMessage)
        {
            var data = new CollectionData
            {
                Timestamp = DateTime.Now,
                Address = -1,
                Value = errorMessage,
                DataType = "错误",
                Status = "异常",
                Quality = "差"
            };

            await AddDataAsync(data);
        }

        private async Task AddDataAsync(CollectionData data)
        {
            _buffer.Enqueue(data);
            var currentCount = Interlocked.Increment(ref _count);

            // 触发数据接收事件
            DataReceived?.Invoke(this, data);

            // 检查缓冲区是否需要清理
            if (currentCount > _options.MaxBufferSize)
            {
                await CleanupOldDataAsync();
            }

            _logger.LogDebug("添加数据到缓冲区: {Address}={Value}, 当前数量: {Count}",
                           data.Address, data.Value, currentCount);

            await Task.CompletedTask;
        }

        private async Task CleanupOldDataAsync()
        {
            var cleanupCount = _options.MaxBufferSize / 4; // 清理25%的数据
            var cleaned = 0;

            while (cleaned < cleanupCount && _buffer.TryDequeue(out var _))
            {
                Interlocked.Decrement(ref _count);
                cleaned++;
            }

            _logger.LogInformation("清理缓冲区: 移除 {Count} 条旧数据", cleaned);
            await Task.CompletedTask;
        }

        public async Task FlushAsync()
        {
            _logger.LogInformation("开始刷新缓冲区，数据量: {Count}", _count);

            var batch = new List<CollectionData>();
            var flushedCount = 0;

            while (_buffer.TryDequeue(out var data) && batch.Count < _options.BatchSize)
            {
                batch.Add(data);
                Interlocked.Decrement(ref _count);
                flushedCount++;
            }

            if (batch.Count > 0)
            {
                // 在实际应用中，这里应该将数据发送到云端或持久化存储
                await ProcessBatchAsync(batch);
            }

            _logger.LogInformation("已刷新 {Count} 条数据", flushedCount);
        }

        private async Task ProcessBatchAsync(List<CollectionData> batch)
        {
            // 模拟数据处理
            await Task.Delay(100);

            foreach (var data in batch)
            {
                _logger.LogDebug("处理数据: {Timestamp} - {Address}={Value}",
                               data.Timestamp, data.Address, data.Value);
            }
        }

        public List<CollectionData> GetAllData()
        {
            return _buffer.ToList();
        }

        public void Clear()
        {
            while (_buffer.TryDequeue(out var _))
            {
                Interlocked.Decrement(ref _count);
            }
            _logger.LogInformation("缓冲区已清空");
        }
    }
}