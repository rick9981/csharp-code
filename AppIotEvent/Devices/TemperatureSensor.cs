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
    /// 温度传感器
    /// </summary>
    public class TemperatureSensor : IDevice
    {
        private readonly Random _random;
        private IEventBus _eventBus;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _dataGenerationTask;

        public string DeviceId { get; }
        public string DeviceName { get; }
        public bool IsConnected { get; private set; }
        public bool IsRunning { get; private set; }

        public TemperatureSensor(string deviceId, string deviceName)
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
                // 模拟连接过程
                Thread.Sleep(1000);
                IsConnected = true;
                _eventBus?.Publish(new DeviceConnectionChangedEvent(DeviceId, DeviceName, true));
                _eventBus?.Publish(new SystemLogEvent($"温度传感器 {DeviceName} 已连接", LogLevel.Info));
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Stop();
                IsConnected = false;
                _eventBus?.Publish(new DeviceConnectionChangedEvent(DeviceId, DeviceName, false));
                _eventBus?.Publish(new SystemLogEvent($"温度传感器 {DeviceName} 已断开", LogLevel.Info));
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
                _eventBus?.Publish(new SystemLogEvent($"温度传感器 {DeviceName} 开始采集数据", LogLevel.Info));
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _cancellationTokenSource?.Cancel();
                _dataGenerationTask?.Wait(5000);
                _eventBus?.Publish(new SystemLogEvent($"温度传感器 {DeviceName} 停止采集数据", LogLevel.Info));
            }
        }

        private async Task GenerateData()
        {
            double currentTemp = 25.0; // 基础温度

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // 模拟温度变化
                    currentTemp += (_random.NextDouble() - 0.5) * 2.0; 
                    currentTemp = Math.Max(-10, Math.Min(50, currentTemp)); // 限制范围

                    // 发布数据更新事件
                    _eventBus?.Publish(new DeviceDataUpdatedEvent(
                        DeviceId, DeviceName, Math.Round(currentTemp, 1), "°C", "Temperature"));

                    // 检查报警条件
                    if (currentTemp > 35)
                    {
                        _eventBus?.Publish(new DeviceAlarmEvent(
                            DeviceId, DeviceName, $"温度过高: {currentTemp:F1}°C", AlarmLevel.Warning));
                    }
                    else if (currentTemp > 40)
                    {
                        _eventBus?.Publish(new DeviceAlarmEvent(
                            DeviceId, DeviceName, $"温度严重过高: {currentTemp:F1}°C", AlarmLevel.Error));
                    }

                    await Task.Delay(2000, _cancellationTokenSource.Token); 
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _eventBus?.Publish(new SystemLogEvent($"温度传感器数据生成异常: {ex.Message}", LogLevel.Error));
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
