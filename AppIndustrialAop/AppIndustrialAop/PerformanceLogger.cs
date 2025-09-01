using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppIndustrialAop
{
    public class PerformanceLogger : IPerformanceLogger, INotifyPropertyChanged
    {
        private readonly ObservableCollection<PerformanceRecord> _performanceRecords;
        private readonly Dictionary<string, List<long>> _methodPerformanceHistory;
        private readonly object _lock = new object();

        public PerformanceLogger()
        {
            _performanceRecords = new ObservableCollection<PerformanceRecord>();
            _methodPerformanceHistory = new Dictionary<string, List<long>>();
        }

        public void LogPerformance(string methodName, long elapsedMs)
        {
            lock (_lock)
            {
                // 记录到历史数据
                if (!_methodPerformanceHistory.ContainsKey(methodName))
                {
                    _methodPerformanceHistory[methodName] = new List<long>();
                }
                _methodPerformanceHistory[methodName].Add(elapsedMs);

                // 保持最近500条记录
                if (_methodPerformanceHistory[methodName].Count > 500)
                {
                    _methodPerformanceHistory[methodName].RemoveAt(0);
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                _performanceRecords.Add(new PerformanceRecord
                {
                    MethodName = methodName,
                    ElapsedMilliseconds = elapsedMs,
                    Timestamp = DateTime.Now,
                    Status = elapsedMs > 500 ? "警告" : "正常"
                });

                // 保持最近1000条记录
                if (_performanceRecords.Count > 1000)
                {
                    _performanceRecords.RemoveAt(0);
                }
            });
        }

        public void LogWarning(string methodName, long elapsedMs)
        {
            Console.WriteLine($"⚠️ 性能警告: {methodName} 执行时间过长 ({elapsedMs}ms)");
        }

        public void LogError(string methodName, Exception ex)
        {
            Console.WriteLine($"❌ 错误: {methodName} 执行异常 - {ex.Message}");
        }

        public ObservableCollection<PerformanceRecord> GetPerformanceRecords()
        {
            return _performanceRecords;
        }

        public List<PerformanceMetrics> GetPerformanceMetrics()
        {
            lock (_lock)
            {
                var metrics = new List<PerformanceMetrics>();
                var now = DateTime.Now;

                for (int i = 0; i < 60; i++)
                {
                    var timePoint = now.AddSeconds(-i);
                    var recordsAtTime = _performanceRecords
                        .Where(r => r.Timestamp >= timePoint.AddSeconds(-1) && r.Timestamp < timePoint)
                        .ToList();

                    metrics.Add(new PerformanceMetrics
                    {
                        Timestamp = timePoint,
                        AverageResponseTime = recordsAtTime.Any() ? recordsAtTime.Average(r => r.ElapsedMilliseconds) : 0,
                        RequestCount = recordsAtTime.Count,
                        ErrorCount = recordsAtTime.Count(r => r.Status == "错误")
                    });
                }

                return metrics.OrderBy(m => m.Timestamp).ToList();
            }
        }

        public List<MethodPerformanceStats> GetMethodStats()
        {
            lock (_lock)
            {
                var stats = new List<MethodPerformanceStats>();

                foreach (var kvp in _methodPerformanceHistory)
                {
                    var values = kvp.Value;
                    if (values.Any())
                    {
                        stats.Add(new MethodPerformanceStats
                        {
                            MethodName = kvp.Key,
                            AverageTime = values.Average(),
                            MinTime = values.Min(),
                            MaxTime = values.Max(),
                            CallCount = values.Count,
                            P95Time = CalculatePercentile(values, 95),
                            P99Time = CalculatePercentile(values, 99)
                        });
                    }
                }

                return stats.OrderByDescending(s => s.AverageTime).ToList();
            }
        }

        private double CalculatePercentile(List<long> values, int percentile)
        {
            var sorted = values.OrderBy(x => x).ToList();
            int index = (int)Math.Ceiling(percentile / 100.0 * sorted.Count) - 1;
            return sorted[Math.Max(0, Math.Min(index, sorted.Count - 1))];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 4. 数据模型定义
    public class PerformanceRecord
    {
        public string MethodName { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }

    public class PerformanceMetrics
    {
        public DateTime Timestamp { get; set; }
        public double AverageResponseTime { get; set; }
        public int RequestCount { get; set; }
        public int ErrorCount { get; set; }
    }

    public class MethodPerformanceStats
    {
        public string MethodName { get; set; }
        public double AverageTime { get; set; }
        public long MinTime { get; set; }
        public long MaxTime { get; set; }
        public int CallCount { get; set; }
        public double P95Time { get; set; }
        public double P99Time { get; set; }
    }

    public class Order : INotifyPropertyChanged
    {
        private string _orderId;
        private string _customerName;
        private int _quantity;
        private string _product;
        private DateTime _orderTime;
        private string _status;

        public string OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public string Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(); }
        }

        public DateTime OrderTime
        {
            get => _orderTime;
            set { _orderTime = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProductionSchedule
    {
        public string ScheduleId { get; set; }
        public string OrderId { get; set; }
        public string Equipment { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EstimatedDuration { get; set; }
    }

    // 5. 业务服务接口和实现
    public interface IOrderService
    {
        Task<bool> ProcessOrderAsync(Order order);
        Task<List<Order>> GetOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(string orderId, string status);
    }

    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders;
        private readonly IProductionScheduleService _scheduleService;
        private readonly Random _random;

        public OrderService(IProductionScheduleService scheduleService)
        {
            _orders = new List<Order>();
            _scheduleService = scheduleService;
            _random = new Random();
        }

        [PerformanceMonitor("订单处理")]
        public virtual async Task<bool> ProcessOrderAsync(Order order)
        {
            // 模拟复杂的订单处理逻辑
            await Task.Delay(_random.Next(100, 800)); // 模拟网络延迟和数据库操作

            // 计算生产时间
            var productionTime = await CalculateProductionTimeAsync(order);

            // 创建生产调度
            var schedule = new ProductionSchedule
            {
                ScheduleId = Guid.NewGuid().ToString(),
                OrderId = order.OrderId,
                Equipment = GetAvailableEquipment(order.Product),
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(1 + productionTime),
                EstimatedDuration = productionTime
            };

            // 保存到生产调度
            await _scheduleService.CreateScheduleAsync(schedule);

            // 保存订单到数据库
            await SaveOrderToDatabaseAsync(order);

            order.Status = "已调度";
            return true;
        }

        [PerformanceMonitor("生产时间计算")]
        private async Task<int> CalculateProductionTimeAsync(Order order)
        {
            // 模拟复杂的算法计算
            await Task.Delay(_random.Next(50, 300));

            // 根据产品类型和数量计算生产时间
            var baseTime = order.Product switch
            {
                "电机" => 2.0,
                "齿轮" => 1.5,
                "轴承" => 3.0,
                _ => 2.0
            };

            return (int)(order.Quantity * baseTime);
        }

        [PerformanceMonitor("数据库保存")]
        private async Task SaveOrderToDatabaseAsync(Order order)
        {
            // 模拟数据库保存操作
            await Task.Delay(_random.Next(100, 400));

            _orders.Add(order);
        }

        private string GetAvailableEquipment(string product)
        {
            var equipmentOptions = product switch
            {
                "电机" => new[] { "电机生产线A", "电机生产线B" },
                "齿轮" => new[] { "齿轮加工中心1", "齿轮加工中心2" },
                "轴承" => new[] { "轴承装配线1", "轴承装配线2" },
                _ => new[] { "通用生产线1", "通用生产线2" }
            };

            return equipmentOptions[_random.Next(equipmentOptions.Length)];
        }

        public virtual async Task<List<Order>> GetOrdersAsync()
        {
            await Task.Delay(50); // 模拟数据库查询
            return _orders.ToList();
        }

        public virtual async Task<bool> UpdateOrderStatusAsync(string orderId, string status)
        {
            await Task.Delay(30);
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = status;
                return true;
            }
            return false;
        }
    }

    // 6. 生产调度服务
    public interface IProductionScheduleService
    {
        Task<bool> CreateScheduleAsync(ProductionSchedule schedule);
        Task<List<ProductionSchedule>> GetSchedulesAsync();
    }

    public class ProductionScheduleService : IProductionScheduleService
    {
        private readonly List<ProductionSchedule> _schedules;

        public ProductionScheduleService()
        {
            _schedules = new List<ProductionSchedule>();
        }

        [PerformanceMonitor("创建生产调度")]
        public virtual async Task<bool> CreateScheduleAsync(ProductionSchedule schedule)
        {
            await Task.Delay(100); // 模拟调度算法计算
            _schedules.Add(schedule);
            return true;
        }

        [PerformanceMonitor("获取调度列表")]
        public virtual async Task<List<ProductionSchedule>> GetSchedulesAsync()
        {
            await Task.Delay(50);
            return _schedules.ToList();
        }
    }
}
