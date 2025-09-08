using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIotEvent.Events
{
    /// <summary>
    /// 设备数据更新事件
    /// </summary>
    public class DeviceDataUpdatedEvent : BaseEvent
    {
        public string DeviceId { get; }
        public string DeviceName { get; }
        public double Value { get; }
        public string Unit { get; }
        public string DataType { get; }

        public DeviceDataUpdatedEvent(string deviceId, string deviceName, double value, string unit, string dataType)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            Value = value;
            Unit = unit;
            DataType = dataType;
        }
    }

    /// <summary>
    /// 设备连接状态变更事件
    /// </summary>
    public class DeviceConnectionChangedEvent : BaseEvent
    {
        public string DeviceId { get; }
        public string DeviceName { get; }
        public bool IsConnected { get; }

        public DeviceConnectionChangedEvent(string deviceId, string deviceName, bool isConnected)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            IsConnected = isConnected;
        }
    }

    /// <summary>
    /// 设备报警事件
    /// </summary>
    public class DeviceAlarmEvent : BaseEvent
    {
        public string DeviceId { get; }
        public string DeviceName { get; }
        public string AlarmMessage { get; }
        public AlarmLevel Level { get; }

        public DeviceAlarmEvent(string deviceId, string deviceName, string alarmMessage, AlarmLevel level)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            AlarmMessage = alarmMessage;
            Level = level;
        }
    }

    public enum AlarmLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }
}
