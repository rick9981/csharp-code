using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIotEvent.EventBus;

namespace AppIotEvent.Devices
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IDevice : IDisposable
    {
        string DeviceId { get; }
        string DeviceName { get; }
        bool IsConnected { get; }
        bool IsRunning { get; }

        void Initialize(IEventBus eventBus);
        void Start();
        void Stop();
        void Connect();
        void Disconnect();
    }
}
