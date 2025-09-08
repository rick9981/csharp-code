using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIotEvent.EventBus;
using AppIotEvent.Events;

namespace AppIotEvent.Services
{
    /// <summary>
    /// 数据记录服务
    /// </summary>
    public class DataLogger : IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly string _logDirectory;
        private readonly object _fileLock = new object();

        public DataLogger(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<DeviceDataUpdatedEvent>(OnDeviceDataUpdated);
            _eventBus.Subscribe<DeviceAlarmEvent>(OnDeviceAlarm);
            _eventBus.Subscribe<SystemLogEvent>(OnSystemLog);
        }

        private void OnDeviceDataUpdated(DeviceDataUpdatedEvent eventData)
        {
            var logEntry = $"{eventData.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{eventData.DeviceId},{eventData.DeviceName},{eventData.DataType},{eventData.Value},{eventData.Unit}";
            WriteToFile("DeviceData", logEntry);
        }

        private void OnDeviceAlarm(DeviceAlarmEvent eventData)
        {
            var logEntry = $"{eventData.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{eventData.DeviceId},{eventData.DeviceName},{eventData.Level},{eventData.AlarmMessage}";
            WriteToFile("DeviceAlarms", logEntry);
        }

        private void OnSystemLog(SystemLogEvent eventData)
        {
            var logEntry = $"{eventData.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{eventData.Level},{eventData.Message}";
            WriteToFile("SystemLogs", logEntry);
        }

        private void WriteToFile(string category, string logEntry)
        {
            try
            {
                lock (_fileLock)
                {
                    var fileName = $"{category}_{DateTime.Now:yyyyMMdd}.csv";
                    var filePath = Path.Combine(_logDirectory, fileName);

                    var isNewFile = !File.Exists(filePath);

                    using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
                    {
                        if (isNewFile)
                        {
                            // 写入CSV头部
                            string header = category switch
                            {
                                "DeviceData" => "Timestamp,DeviceId,DeviceName,DataType,Value,Unit",
                                "DeviceAlarms" => "Timestamp,DeviceId,DeviceName,Level,Message",
                                "SystemLogs" => "Timestamp,Level,Message",
                                _ => "Timestamp,Data"
                            };
                            writer.WriteLine(header);
                        }

                        writer.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"写入日志文件失败: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<DeviceDataUpdatedEvent>(OnDeviceDataUpdated);
            _eventBus.Unsubscribe<DeviceAlarmEvent>(OnDeviceAlarm);
            _eventBus.Unsubscribe<SystemLogEvent>(OnSystemLog);
        }
    }
}
