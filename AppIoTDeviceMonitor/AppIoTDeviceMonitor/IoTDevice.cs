using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace AppIoTDeviceMonitor
{
    public class IoTDevice
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }
        public DeviceStatus Status { get; set; }
        public double? LastValue { get; set; }
        public DateTime LastUpdateTime { get; set; }

        private readonly Timer _dataTimer;
        private readonly Random _random = new Random();

        public IoTDevice(string deviceId, string deviceName, DeviceType deviceType)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            DeviceType = deviceType;
            Status = DeviceStatus.Offline;
            LastUpdateTime = DateTime.Now;

            _dataTimer = new Timer();
            _dataTimer.Interval = 2000 + _random.Next(0, 3000); // 2-5秒随机间隔
            _dataTimer.Tick += GenerateData;
        }

        public void Start()
        {
            Status = DeviceStatus.Online;
            _dataTimer.Start();
        }

        public void Stop()
        {
            Status = DeviceStatus.Offline;
            _dataTimer.Stop();
        }

        private void GenerateData(object sender, EventArgs e)
        {
            // 模拟设备数据生成
            switch (DeviceType)
            {
                case DeviceType.TemperatureSensor:
                    LastValue = 20 + _random.NextDouble() * 30; // 20-50°C
                    break;
                case DeviceType.HumiditySensor:
                    LastValue = 30 + _random.NextDouble() * 40; // 30-70%
                    break;
                case DeviceType.PressureSensor:
                    LastValue = 100 + _random.NextDouble() * 200; // 100-300 kPa
                    break;
                case DeviceType.Motor:
                    LastValue = 1000 + _random.NextDouble() * 2000; // 1000-3000 RPM
                    break;
                default:
                    LastValue = _random.NextDouble() * 100;
                    break;
            }

            LastUpdateTime = DateTime.Now;

            // 模拟设备偶尔出错
            if (_random.NextDouble() < 0.02) // 2%概率出错
            {
                Status = DeviceStatus.Error;
            }
            else if (Status == DeviceStatus.Error && _random.NextDouble() < 0.1) // 10%概率恢复
            {
                Status = DeviceStatus.Online;
            }
        }
    }

    public class IoTDeviceManager : IDisposable
    {
        private readonly ConcurrentDictionary<string, IoTDevice> _devices;
        private readonly Timer _statsTimer;
        private long _totalDataPoints;
        private DateTime _lastStatsTime;

        public IoTDeviceManager()
        {
            _devices = new ConcurrentDictionary<string, IoTDevice>();
            _lastStatsTime = DateTime.Now;

            _statsTimer = new Timer();
            _statsTimer.Interval = 1000;
            _statsTimer.Tick += UpdateStats;
            _statsTimer.Start();
        }

        public void AddDevice(IoTDevice device)
        {
            _devices.TryAdd(device.DeviceId, device);
        }

        public IoTDevice GetDevice(string deviceId)
        {
            _devices.TryGetValue(deviceId, out var device);
            return device;
        }

        public IEnumerable<IoTDevice> GetAllDevices()
        {
            return _devices.Values;
        }

        public void StartAllDevices()
        {
            foreach (var device in _devices.Values)
            {
                device.Start();
            }
        }

        public void StopAllDevices()
        {
            foreach (var device in _devices.Values)
            {
                device.Stop();
            }
        }

        public double GetDataRate()
        {
            var now = DateTime.Now;
            var elapsed = (now - _lastStatsTime).TotalSeconds;
            var onlineDevices = _devices.Values.Count(d => d.Status == DeviceStatus.Online);

            return elapsed > 0 ? onlineDevices / 2.5 : 0; // 平均每2.5秒一个数据点
        }

        private void UpdateStats(object sender, EventArgs e)
        {
            _totalDataPoints += _devices.Values.Count(d => d.Status == DeviceStatus.Online);
            _lastStatsTime = DateTime.Now;
        }

        public void Dispose()
        {
            _statsTimer?.Dispose();
            foreach (var device in _devices.Values)
            {
                device.Stop();
            }
        }
    }

    // 性能监控相关类（基于上传文档）
    public class DataCollectionMonitor : IDisposable
    {
        private readonly ConcurrentDictionary<string, PerformanceCounter> _counters;
        private readonly Timer _reportTimer;
        private readonly MetricsCollector _metricsCollector;

        public DataCollectionMonitor()
        {
            _counters = new ConcurrentDictionary<string, PerformanceCounter>();
            _metricsCollector = new MetricsCollector();

            _reportTimer = new Timer();
            _reportTimer.Interval = 10000; // 10秒
            _reportTimer.Tick += ReportMetrics;
            _reportTimer.Start();
        }

        public void RecordProcessing(string operation, TimeSpan duration, bool success = true)
        {
            _metricsCollector.RecordProcessing(operation, duration, success);
        }

        public void RecordDataVolume(string source, long count)
        {
            _metricsCollector.RecordDataVolume(source, count);
        }

        public void RecordError(string operation, Exception exception)
        {
            _metricsCollector.RecordError(operation, exception);
        }

        private void ReportMetrics(object sender, EventArgs e)
        {
            var report = _metricsCollector.GenerateReport();
            // 可以在这里添加日志记录或UI更新
        }

        public void Dispose()
        {
            _reportTimer?.Dispose();
            _metricsCollector?.Dispose();
            foreach (var counter in _counters.Values)
            {
                counter?.Dispose();
            }
        }
    }

    public class MetricsCollector : IDisposable
    {
        private readonly ConcurrentDictionary<string, OperationMetrics> _operationMetrics;
        private readonly ConcurrentDictionary<string, long> _dataVolumes;
        private readonly ConcurrentQueue<ErrorRecord> _errors;

        public MetricsCollector()
        {
            _operationMetrics = new ConcurrentDictionary<string, OperationMetrics>();
            _dataVolumes = new ConcurrentDictionary<string, long>();
            _errors = new ConcurrentQueue<ErrorRecord>();
        }

        public void RecordProcessing(string operation, TimeSpan duration, bool success)
        {
            _operationMetrics.AddOrUpdate(operation,
                new OperationMetrics
                {
                    Count = 1,
                    TotalDuration = duration,
                    SuccessCount = success ? 1 : 0
                },
                (key, existing) => new OperationMetrics
                {
                    Count = existing.Count + 1,
                    TotalDuration = existing.TotalDuration + duration,
                    SuccessCount = existing.SuccessCount + (success ? 1 : 0)
                });
        }

        public void RecordDataVolume(string source, long count)
        {
            _dataVolumes.AddOrUpdate(source, count, (key, existing) => existing + count);
        }

        public void RecordError(string operation, Exception exception)
        {
            _errors.Enqueue(new ErrorRecord
            {
                Operation = operation,
                Exception = exception,
                Timestamp = DateTime.UtcNow
            });
        }

        public PerformanceReport GenerateReport()
        {
            var totalProcessed = _operationMetrics.Values.Sum(m => m.Count);
            var totalDuration = _operationMetrics.Values.Sum(m => m.TotalDuration.TotalMilliseconds);
            var totalSuccess = _operationMetrics.Values.Sum(m => m.SuccessCount);

            return new PerformanceReport
            {
                TimeWindow = TimeSpan.FromSeconds(10),
                TotalProcessed = totalProcessed,
                ProcessingRate = totalProcessed / 10.0,
                AverageLatency = totalProcessed > 0 ? totalDuration / totalProcessed : 0,
                ErrorRate = totalProcessed > 0 ? (double)(totalProcessed - totalSuccess) / totalProcessed : 0,
                MemoryUsage = GC.GetTotalMemory(false) / 1024 / 1024
            };
        }

        public void Dispose()
        {
            // 清理资源
        }
    }

    // 支持类
    public class OperationMetrics
    {
        public int Count { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public int SuccessCount { get; set; }
    }

    public class ErrorRecord
    {
        public string Operation { get; set; }
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class PerformanceReport
    {
        public TimeSpan TimeWindow { get; set; }
        public long TotalProcessed { get; set; }
        public double ProcessingRate { get; set; }
        public double AverageLatency { get; set; }
        public double ErrorRate { get; set; }
        public long MemoryUsage { get; set; }
    }
}