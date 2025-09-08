using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIotEvent.EventBus;
using AppIotEvent.Events;

namespace AppIotEvent.Devices
{
    /// <summary>
    /// 湿度传感器
    /// </summary>
    public class HumiditySensor : IDevice
    {
        private readonly Random _random;
        private IEventBus _eventBus;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _dataGenerationTask;

        public string DeviceId { get; }
        public string DeviceName { get; }
        public bool IsConnected { get; private set; }
        public bool IsRunning { get; private set; }

        public HumiditySensor(string deviceId, string deviceName)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            _random = new Random();
        }

        public void Initialize(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                Thread.Sleep(800);
                IsConnected = true;
                _eventBus?.Publish(new DeviceConnectionChangedEvent(DeviceId, DeviceName, true));
                _eventBus?.Publish(new SystemLogEvent($"湿度传感器 {DeviceName} 已连接", LogLevel.Info));
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Stop();
                IsConnected = false;
                _eventBus?.Publish(new DeviceConnectionChangedEvent(DeviceId, DeviceName, false));
                _eventBus?.Publish(new SystemLogEvent($"湿度传感器 {DeviceName} 已断开", LogLevel.Info));
            }
        }

        public void Start()
        {
            if (!IsConnected)
            {
                Connect();
            }

            if (!IsRunning)
            {
                IsRunning = true;
                _cancellationTokenSource = new CancellationTokenSource();
                _dataGenerationTask = Task.Run(GenerateData, _cancellationTokenSource.Token);
                _eventBus?.Publish(new SystemLogEvent($"湿度传感器 {DeviceName} 开始采集数据", LogLevel.Info));
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _cancellationTokenSource?.Cancel();
                _dataGenerationTask?.Wait(5000);
                _eventBus?.Publish(new SystemLogEvent($"湿度传感器 {DeviceName} 停止采集数据", LogLevel.Info));
            }
        }

        private async Task GenerateData()
        {
            double currentHumidity = 45.0; // 基础湿度

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // 模拟湿度变化
                    currentHumidity += (_random.NextDouble() - 0.5) * 5.0; // ±2.5%变化
                    currentHumidity = Math.Max(0, Math.Min(100, currentHumidity)); // 限制0-100%

                    // 发布数据更新事件
                    _eventBus?.Publish(new DeviceDataUpdatedEvent(
                        DeviceId, DeviceName, Math.Round(currentHumidity, 1), "%", "Humidity"));

                    // 检查报警条件
                    if (currentHumidity < 40 || currentHumidity > 60)
                    {
                        _eventBus?.Publish(new DeviceAlarmEvent(
                            DeviceId, DeviceName, $"湿度异常: {currentHumidity:F1}%", AlarmLevel.Warning));
                    }

                    await Task.Delay(3000, _cancellationTokenSource.Token); // 3秒间隔
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _eventBus?.Publish(new SystemLogEvent($"湿度传感器数据生成异常: {ex.Message}", LogLevel.Error));
                    await Task.Delay(5000, _cancellationTokenSource.Token);
                }
            }
        }

        public void Dispose()
        {
            Stop();
            Disconnect();
            _cancellationTokenSource?.Dispose();
        }
    }
}
