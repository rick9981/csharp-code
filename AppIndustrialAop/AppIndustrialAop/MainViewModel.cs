using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.WPF;
using ScottPlot;
using System.Windows.Threading;
using System.Windows;

namespace AppIndustrialAop
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        private readonly IProductionScheduleService _scheduleService;
        private readonly IPerformanceLogger _performanceLogger;
        private readonly DispatcherTimer _chartUpdateTimer;

        public ObservableCollection<Order> Orders { get; }
        public ObservableCollection<ProductionSchedule> Schedules { get; }
        public ObservableCollection<PerformanceRecord> PerformanceRecords { get; }

        // ScottPlot 图表相关
        public WpfPlot ResponseTimeChart { get; private set; }
        public WpfPlot MethodStatsChart { get; private set; }
        public WpfPlot RequestCountChart { get; private set; }

        private string _newOrderId;
        private string _newCustomerName;
        private int _newQuantity;
        private string _newProduct;

        public string NewOrderId
        {
            get => _newOrderId;
            set { _newOrderId = value; OnPropertyChanged(); }
        }

        public string NewCustomerName
        {
            get => _newCustomerName;
            set { _newCustomerName = value; OnPropertyChanged(); }
        }

        public int NewQuantity
        {
            get => _newQuantity;
            set { _newQuantity = value; OnPropertyChanged(); }
        }

        public string NewProduct
        {
            get => _newProduct;
            set { _newProduct = value; OnPropertyChanged(); }
        }

        public List<string> ProductOptions { get; } = new List<string> { "电机", "齿轮", "轴承" };

        public MainViewModel(IOrderService orderService, IProductionScheduleService scheduleService, IPerformanceLogger performanceLogger)
        {
            _orderService = orderService;
            _scheduleService = scheduleService;
            _performanceLogger = performanceLogger;

            Orders = new ObservableCollection<Order>();
            Schedules = new ObservableCollection<ProductionSchedule>();
            PerformanceRecords = _performanceLogger.GetPerformanceRecords();

            // 初始化图表
            InitializeCharts();

            // 初始化默认值
            NewOrderId = GenerateOrderId();
            NewCustomerName = "客户A";
            NewQuantity = 10;
            NewProduct = "电机";

            // 启动图表更新定时器
            _chartUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _chartUpdateTimer.Tick += UpdateCharts;
            _chartUpdateTimer.Start();
        }

        private void InitializeCharts()
        {
            // 响应时间图表
            ResponseTimeChart = new WpfPlot();
            ResponseTimeChart.Plot.Title("实时响应时间监控");
            ResponseTimeChart.Plot.Axes.Left.Label.Text = "响应时间 (ms)";
            ResponseTimeChart.Plot.Axes.Bottom.Label.Text = "时间";
            ResponseTimeChart.Plot.Axes.Left.Min = 0;

            // 方法统计图表
            MethodStatsChart = new WpfPlot();
            MethodStatsChart.Plot.Title("方法性能统计");
            MethodStatsChart.Plot.Axes.Left.Label.Text = "平均响应时间 (ms)";
            MethodStatsChart.Plot.Axes.Bottom.Label.Text = "方法名";

            // 请求计数图表
            RequestCountChart = new WpfPlot();
            RequestCountChart.Plot.Title("请求计数监控");
            RequestCountChart.Plot.Axes.Left.Label.Text = "请求数量";
            RequestCountChart.Plot.Axes.Bottom.Label.Text = "时间";
        }

        private void UpdateCharts(object sender, EventArgs e)
        {
            UpdateResponseTimeChart();
            UpdateMethodStatsChart();
            UpdateRequestCountChart();
        }

        private void UpdateResponseTimeChart()
        {
            var metrics = _performanceLogger.GetPerformanceMetrics();
            if (!metrics.Any()) return;

            ResponseTimeChart.Plot.Clear();
            ResponseTimeChart.Plot.Font.Set("SimSun");

            var times = metrics.Select(m => m.Timestamp.ToOADate()).ToArray();
            var responseTimes = metrics.Select(m => m.AverageResponseTime).ToArray();

            var linePlot = ResponseTimeChart.Plot.Add.Scatter(times, responseTimes);
            linePlot.LineWidth = 2;
            linePlot.Color = Colors.Blue;
            linePlot.MarkerSize = 0;

            // 添加警告线
            var warningLine = ResponseTimeChart.Plot.Add.HorizontalLine(500);
            warningLine.Color = Colors.Red;
            warningLine.LineWidth = 2;
            warningLine.LinePattern = LinePattern.Dashed;

            ResponseTimeChart.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();
            ResponseTimeChart.Plot.Axes.AutoScale();
            ResponseTimeChart.Refresh();
        }

        private void UpdateMethodStatsChart()
        {
            var stats = _performanceLogger.GetMethodStats().Take(10).ToList();
            if (!stats.Any()) return;

            MethodStatsChart.Plot.Clear();
            MethodStatsChart.Plot.Font.Set("SimSun");

            var positions = Enumerable.Range(0, stats.Count).Select(i => (double)i).ToArray();
            var averageTimes = stats.Select(s => s.AverageTime).ToArray();
            var labels = stats.Select(s => s.MethodName.Split('.').Last()).ToArray();

            var barPlot = MethodStatsChart.Plot.Add.Bars(positions, averageTimes);
            barPlot.Color = Colors.Green;

            MethodStatsChart.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(positions, labels);
            MethodStatsChart.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            MethodStatsChart.Plot.Axes.AutoScale();
            MethodStatsChart.Refresh();
        }

        private void UpdateRequestCountChart()
        {
            var metrics = _performanceLogger.GetPerformanceMetrics();
            if (!metrics.Any()) return;

            RequestCountChart.Plot.Clear();
            RequestCountChart.Plot.Font.Set("SimSun");

            var times = metrics.Select(m => m.Timestamp.ToOADate()).ToArray();
            var requestCounts = metrics.Select(m => (double)m.RequestCount).ToArray();

            var barPlot = RequestCountChart.Plot.Add.Bars(times, requestCounts);
            barPlot.Color = Colors.Orange;

            RequestCountChart.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();
            RequestCountChart.Plot.Axes.AutoScale();
            RequestCountChart.Refresh();
        }

        public async Task CreateOrderAsync()
        {
            var order = new Order
            {
                OrderId = NewOrderId,
                CustomerName = NewCustomerName,
                Quantity = NewQuantity,
                Product = NewProduct,
                OrderTime = DateTime.Now,
                Status = "处理中"
            };

            try
            {
                var success = await _orderService.ProcessOrderAsync(order);
                if (success)
                {
                    Orders.Add(order);
                    await RefreshSchedulesAsync();

                    // 重置表单
                    NewOrderId = GenerateOrderId();
                    NewCustomerName = "客户A";
                    NewQuantity = 10;
                    NewProduct = "电机";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建订单失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task RefreshSchedulesAsync()
        {
            var schedules = await _scheduleService.GetSchedulesAsync();
            Schedules.Clear();
            foreach (var schedule in schedules)
            {
                Schedules.Add(schedule);
            }
        }

        private string GenerateOrderId()
        {
            return $"ORD{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(100, 999)}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
