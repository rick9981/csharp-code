using System.Globalization;
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
using AppVListView.Models;
using AppVListView.Services;

namespace AppVListView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VirtualEquipmentDataSource dataSource;
        private CollectionViewSource viewSource;
        private bool isRefreshing = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }
        private void InitializeData()
        {
            try
            {
                StatusTextBlock.Text = "正在初始化数据源...";

                // 创建虚拟数据源
                dataSource = new VirtualEquipmentDataSource();

                // 创建视图源用于过滤和排序
                viewSource = new CollectionViewSource { Source = dataSource };

                // 设置ListView的数据源
                EquipmentListView.ItemsSource = viewSource.View;

                // 更新状态
                UpdateStatusBar();
                StatusTextBlock.Text = "数据加载完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据时发生错误: {ex.Message}", "错误",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "初始化失败";
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRefreshing) return;

            try
            {
                isRefreshing = true;
                RefreshButton.IsEnabled = false;
                StatusTextBlock.Text = "正在刷新数据...";

                await dataSource.RefreshDataAsync();

                // 刷新视图
                viewSource.View.Refresh();

                StatusTextBlock.Text = "数据刷新完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新数据时发生错误: {ex.Message}", "错误",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "刷新失败";
            }
            finally
            {
                isRefreshing = false;
                RefreshButton.IsEnabled = true;
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (viewSource?.View == null) return;

            try
            {
                StatusTextBlock.Text = "正在应用过滤器...";

                var selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                var searchText = SearchTextBox.Text?.Trim().ToLower();

                viewSource.View.Filter = item =>
                {
                    if (item is EquipmentData equipment)
                    {
                        // 状态过滤
                        bool statusMatch = selectedStatus == "全部" || equipment.Status == selectedStatus;

                        // 搜索过滤
                        bool searchMatch = string.IsNullOrEmpty(searchText) ||
                                         equipment.EquipmentId.ToLower().Contains(searchText) ||
                                         equipment.EquipmentName.ToLower().Contains(searchText) ||
                                         equipment.Location.ToLower().Contains(searchText);

                        return statusMatch && searchMatch;
                    }
                    return false;
                };

                UpdateStatusBar();
                StatusTextBlock.Text = "过滤器应用完成";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"过滤错误: {ex.Message}";
            }
        }

        private void UpdateStatusBar()
        {
            if (viewSource?.View == null) return;

            try
            {
                var totalItems = dataSource.Count;
                var filteredItems = viewSource.View.Cast<EquipmentData>().Count();

                TotalItemsTextBlock.Text = $"总设备数: {totalItems:N0}";
                VisibleItemsTextBlock.Text = $"显示: {filteredItems:N0}";
                PerformanceTextBlock.Text = "虚拟化: 启用";
            }
            catch
            {
                // 如果计数失败，显示基本信息
                TotalItemsTextBlock.Text = "总设备数: 100,000";
                VisibleItemsTextBlock.Text = "显示: --";
            }
        }
    }

    // 状态到颜色的转换器
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "正常" => new SolidColorBrush(Color.FromRgb(46, 204, 113)),  // 绿色
                    "警告" => new SolidColorBrush(Color.FromRgb(241, 196, 15)),  // 黄色
                    "故障" => new SolidColorBrush(Color.FromRgb(231, 76, 60)),   // 红色
                    "维护中" => new SolidColorBrush(Color.FromRgb(52, 152, 219)), // 蓝色
                    "停机" => new SolidColorBrush(Color.FromRgb(149, 165, 166)), // 灰色
                    _ => new SolidColorBrush(Color.FromRgb(127, 140, 141))       // 默认灰色
                };
            }
            return new SolidColorBrush(Color.FromRgb(127, 140, 141));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}