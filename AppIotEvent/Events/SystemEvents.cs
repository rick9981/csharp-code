using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIotEvent.Events
{
    /// <summary>
    /// 系统日志事件
    /// </summary>
    public class SystemLogEvent : BaseEvent
    {
        public string Message { get; }
        public LogLevel Level { get; }

        public SystemLogEvent(string message, LogLevel level)
        {
            Message = message;
            Level = level;
        }
    }

    /// <summary>
    /// 用户操作事件
    /// </summary>
    public class UserActionEvent : BaseEvent
    {
        public string Action { get; }
        public string Target { get; }
        public string Details { get; }

        public UserActionEvent(string action, string target, string details = "")
        {
            Action = action;
            Target = target;
            Details = details;
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
