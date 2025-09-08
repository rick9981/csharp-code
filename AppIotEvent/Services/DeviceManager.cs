using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIotEvent.Devices;
using AppIotEvent.EventBus;
using AppIotEvent.Events;

namespace AppIotEvent.Services
{
    /// <summary>
    /// 设备管理器
    /// </summary>
    public class DeviceManager : IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly List<IDevice> _devices;

        public IReadOnlyList<IDevice> Devices => _devices.AsReadOnly();

        public DeviceManager(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _devices = new List<IDevice>();
        }

        public void AddDevice(IDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            device.Initialize(_eventBus);
            _devices.Add(device);
            _eventBus.Publish(new SystemLogEvent($"设备 {device.DeviceName} 已添加到管理器", LogLevel.Info));
        }

        public void RemoveDevice(string deviceId)
        {
            var device = _devices.FirstOrDefault(d => d.DeviceId == deviceId);
            if (device != null)
            {
                device.Stop();
                device.Disconnect();
                _devices.Remove(device);
                device.Dispose();
                _eventBus.Publish(new SystemLogEvent($"设备 {device.DeviceName} 已从管理器移除", LogLevel.Info));
            }
        }

        public IDevice GetDevice(string deviceId)
        {
            return _devices.FirstOrDefault(d => d.DeviceId == deviceId);
        }

        public void StartAllDevices()
        {
            foreach (var device in _devices)
            {
                try
                {
                    device.Start();
                }
                catch (Exception ex)
                {
                    _eventBus.Publish(new SystemLogEvent($"启动设备 {device.DeviceName} 失败: {ex.Message}", LogLevel.Error));
                }
            }
        }

        public void StopAllDevices()
        {
            foreach (var device in _devices)
            {
                try
                {
                    device.Stop();
                }
                catch (Exception ex)
                {
                    _eventBus.Publish(new SystemLogEvent($"停止设备 {device.DeviceName} 失败: {ex.Message}", LogLevel.Error));
                }
            }
        }

        public void Dispose()
        {
            foreach (var device in _devices)
            {
                device?.Dispose();
            }
            _devices.Clear();
        }
    }
}
