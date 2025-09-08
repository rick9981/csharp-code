using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppIotEvent.EventBus;

namespace AppIotEvent.Events
{
    /// <summary>
    /// 事件总线实现
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<object>> _handlers;
        private readonly object _lock = new object();

        public EventBus()
        {
            _handlers = new ConcurrentDictionary<Type, List<object>>();
        }

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(T);
                if (!_handlers.ContainsKey(eventType))
                {
                    _handlers[eventType] = new List<object>();
                }
                _handlers[eventType].Add(handler);
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(T);
                if (_handlers.ContainsKey(eventType))
                {
                    _handlers[eventType].Remove(handler);
                    if (_handlers[eventType].Count == 0)
                    {
                        _handlers.TryRemove(eventType, out _);
                    }
                }
            }
        }

        public void Publish<T>(T eventItem) where T : IEvent
        {
            var eventType = typeof(T);
            if (_handlers.ContainsKey(eventType))
            {
                var handlers = _handlers[eventType].ToList();

                // 确保在UI线程上执行
                if (Application.OpenForms.Count > 0)
                {
                    var mainForm = Application.OpenForms[0];
                    if (mainForm.InvokeRequired)
                    {
                        mainForm.Invoke(new Action(() => ExecuteHandlers(handlers, eventItem)));
                    }
                    else
                    {
                        ExecuteHandlers(handlers, eventItem);
                    }
                }
                else
                {
                    ExecuteHandlers(handlers, eventItem);
                }
            }
        }

        private void ExecuteHandlers<T>(List<object> handlers, T eventItem) where T : IEvent
        {
            foreach (var handler in handlers)
            {
                try
                {
                    ((Action<T>)handler)(eventItem);
                }
                catch (Exception ex)
                {
                    // 记录异常，但不影响其他处理器
                    System.Diagnostics.Debug.WriteLine($"事件处理器执行异常: {ex.Message}");
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _handlers.Clear();
            }
        }
    }
}
