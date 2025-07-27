using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIoTDeviceMonitor
{
    // IoT设备相关类
    public enum DeviceType
    {
        TemperatureSensor,
        HumiditySensor,
        PressureSensor,
        Motor,
        Valve,
        Pump
    }

    public enum DeviceStatus
    {
        Online,
        Offline,
        Error,
        Maintenance
    }

}
