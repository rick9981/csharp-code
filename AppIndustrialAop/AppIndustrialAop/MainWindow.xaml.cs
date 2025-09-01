using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace AppIndustrialAop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            // 配置依赖注入
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            _viewModel = serviceProvider.GetRequiredService<MainViewModel>();
            DataContext = _viewModel;

            // 设置图表到UI
            SetupCharts();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // 注册性能监控日志
            services.AddSingleton<IPerformanceLogger, PerformanceLogger>();

            // 注册拦截器
            services.AddSingleton<PerformanceInterceptor>();

            // 注册代理生成器
            services.AddSingleton<ProxyGenerator>();

            // 注册业务服务（使用代理）
            services.AddSingleton<IOrderService>(provider =>
            {
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                var interceptor = provider.GetRequiredService<PerformanceInterceptor>();
                var scheduleService = provider.GetRequiredService<IProductionScheduleService>();

                var target = new OrderService(scheduleService);
                return proxyGenerator.CreateInterfaceProxyWithTarget<IOrderService>(target, interceptor);
            });

            services.AddSingleton<IProductionScheduleService>(provider =>
            {
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                var interceptor = provider.GetRequiredService<PerformanceInterceptor>();

                var target = new ProductionScheduleService();
                return proxyGenerator.CreateInterfaceProxyWithTarget<IProductionScheduleService>(target, interceptor);
            });

            // 注册ViewModel
            services.AddSingleton<MainViewModel>();
        }

        private void SetupCharts()
        {
            if (ResponseTimeChartContainer != null)
                ResponseTimeChartContainer.Child = _viewModel.ResponseTimeChart;

            if (MethodStatsChartContainer != null)
                MethodStatsChartContainer.Child = _viewModel.MethodStatsChart;

            if (RequestCountChartContainer != null)
                RequestCountChartContainer.Child = _viewModel.RequestCountChart;
        }

        private async void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.CreateOrderAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RefreshSchedulesAsync();
        }
    }
}