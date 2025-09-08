using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIotEvent.Events
{
    /// <summary>
    /// 事件基础接口
    /// </summary>
    public interface IEvent
    {
        DateTime Timestamp { get; }
        string EventId { get; }
    }

    /// <summary>
    /// 事件基础抽象类
    /// </summary>
    public abstract class BaseEvent : IEvent
    {
        public DateTime Timestamp { get; }
        public string EventId { get; }

        protected BaseEvent()
        {
            Timestamp = DateTime.Now;
            EventId = Guid.NewGuid().ToString();
        }
    }
}
