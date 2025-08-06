using ScottPlot;
using ScottPlot.WinForms;
using System.Net.NetworkInformation;
using Timer = System.Windows.Forms.Timer;

namespace AppNetworkTrafficMonitor
{
    public partial class Form1 : Form
    {
        private Timer updateTimer;
        private NetworkInterface selectedInterface;
        private List<NetworkInterface> networkInterfaces;
        private List<double> downloadHistory;
        private List<double> uploadHistory;
        private List<DateTime> timeHistory;
        private long lastBytesReceived;
        private long lastBytesSent;
        private DateTime lastUpdateTime;
        private int maxHistoryPoints = 60; // 保留60个数据点

        // ScottPlot 相关
        private ScottPlot.Plottables.Scatter downloadPlot;
        private ScottPlot.Plottables.Scatter uploadPlot;

        public Form1()
        {
            InitializeComponent();
            InitializeMonitor();
        }

        private void InitializeMonitor()
        {
            downloadHistory = new List<double>();
            uploadHistory = new List<double>();
            timeHistory = new List<DateTime>();

            LoadNetworkInterfaces();
            SetupChart();

            updateTimer = new Timer();
            updateTimer.Interval = 1000; // 每秒更新一次
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            lastUpdateTime = DateTime.Now;
        }

        private void LoadNetworkInterfaces()
        {
            networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                           ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToList();

            comboBoxInterfaces.Items.Clear();
            foreach (var ni in networkInterfaces)
            {
                comboBoxInterfaces.Items.Add($"{ni.Name} ({ni.NetworkInterfaceType})");
            }

            if (comboBoxInterfaces.Items.Count > 0)
            {
                comboBoxInterfaces.SelectedIndex = 0;
                selectedInterface = networkInterfaces[0];

                // 初始化基准值
                var stats = selectedInterface.GetIPv4Statistics();
                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
            }
        }

        private void SetupChart()
        {
            // 清除所有绘图对象
            formsPlot.Plot.Clear();
            formsPlot.Plot.Legend.FontName = "SimSun";

            // 设置坐标轴
            formsPlot.Plot.Axes.Left.Label.Text = "速度 (KB/s)";
            formsPlot.Plot.Axes.Bottom.Label.Text = "时间";
            formsPlot.Plot.Axes.Left.Label.FontSize = 12;
            formsPlot.Plot.Axes.Bottom.Label.FontSize = 12;
            formsPlot.Plot.Axes.Bottom.Label.FontName = "SimSun";
            formsPlot.Plot.Axes.Left.Label.FontName = "SimSun";

            // 创建下载速度线条
            downloadPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            downloadPlot.Color = ScottPlot.Color.FromHtml("#007BFF"); // 蓝色
            downloadPlot.LineWidth = 2;
            downloadPlot.MarkerSize = 0;
            downloadPlot.LegendText = "下载速度";
 
            // 创建上传速度线条
            uploadPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            uploadPlot.Color = ScottPlot.Color.FromHtml("#DC3545"); // 红色
            uploadPlot.LineWidth = 2;
            uploadPlot.MarkerSize = 0;
            uploadPlot.LegendText = "上传速度";

            // 显示图例
            formsPlot.Plot.ShowLegend(Alignment.UpperRight);

            // 设置时间轴格式
            formsPlot.Plot.Axes.DateTimeTicksBottom();

            // 初始刷新
            formsPlot.Refresh();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (selectedInterface == null) return;

            try
            {
                var stats = selectedInterface.GetIPv4Statistics();
                var currentTime = DateTime.Now;
                var timeSpan = (currentTime - lastUpdateTime).TotalSeconds;

                if (timeSpan > 0 && lastBytesReceived > 0 && lastBytesSent > 0)
                {
                    // 计算速度 (KB/s)
                    double downloadSpeed = (stats.BytesReceived - lastBytesReceived) / timeSpan / 1024;
                    double uploadSpeed = (stats.BytesSent - lastBytesSent) / timeSpan / 1024;

                    // 确保速度不为负数
                    downloadSpeed = Math.Max(0, downloadSpeed);
                    uploadSpeed = Math.Max(0, uploadSpeed);

                    // 更新实时显示
                    UpdateRealTimeDisplay(downloadSpeed, uploadSpeed, stats);

                    // 更新历史数据
                    UpdateHistory(downloadSpeed, uploadSpeed, currentTime);

                    // 更新图表
                    UpdateChart();
                }

                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
                lastUpdateTime = currentTime;
            }
            catch (Exception ex)
            {
                labelStatus.Text = $"监控错误: {ex.Message}";
                labelStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void UpdateRealTimeDisplay(double downloadSpeed, double uploadSpeed, IPv4InterfaceStatistics stats)
        {
            // 使用Invoke确保在UI线程中更新
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateRealTimeDisplay(downloadSpeed, uploadSpeed, stats)));
                return;
            }

            labelDownloadSpeed.Text = FormatSpeed(downloadSpeed * 1024); // 转换回 bytes/s 用于格式化
            labelUploadSpeed.Text = FormatSpeed(uploadSpeed * 1024);

            labelTotalDownload.Text = $"总下载: {FormatBytes(stats.BytesReceived)}";
            labelTotalUpload.Text = $"总上传: {FormatBytes(stats.BytesSent)}";

            // 更新进度条 (最大100 KB/s)
            progressBarDownload.Value = Math.Min((int)downloadSpeed, 100);
            progressBarUpload.Value = Math.Min((int)uploadSpeed, 100);

            labelStatus.Text = $"监控中... | 下载: {FormatSpeed(downloadSpeed * 1024)} | 上传: {FormatSpeed(uploadSpeed * 1024)}";
            labelStatus.ForeColor = System.Drawing.Color.Green;
        }

        private void UpdateHistory(double downloadSpeed, double uploadSpeed, DateTime currentTime)
        {
            downloadHistory.Add(downloadSpeed);
            uploadHistory.Add(uploadSpeed);
            timeHistory.Add(currentTime);

            // 保持历史数据在指定范围内
            while (downloadHistory.Count > maxHistoryPoints)
            {
                downloadHistory.RemoveAt(0);
                uploadHistory.RemoveAt(0);
                timeHistory.RemoveAt(0);
            }
        }

        private void UpdateChart()
        {
            if (timeHistory.Count == 0) return;

            try
            {
                // 转换时间为OADate格式用于ScottPlot
                double[] timeArray = timeHistory.Select(t => t.ToOADate()).ToArray();
                double[] downloadArray = downloadHistory.ToArray();
                double[] uploadArray = uploadHistory.ToArray();

                // 移除旧的图表对象
                if (downloadPlot != null)
                    formsPlot.Plot.Remove(downloadPlot);
                if (uploadPlot != null)
                    formsPlot.Plot.Remove(uploadPlot);

                // 创建新的图表对象
                downloadPlot = formsPlot.Plot.Add.Scatter(timeArray, downloadArray);
                downloadPlot.Color = ScottPlot.Color.FromHtml("#007BFF"); // 蓝色
                downloadPlot.LineWidth = 2;
                downloadPlot.MarkerSize = 0;
                downloadPlot.LegendText = "下载速度";

                uploadPlot = formsPlot.Plot.Add.Scatter(timeArray, uploadArray);
                uploadPlot.Color = ScottPlot.Color.FromHtml("#DC3545"); // 红色
                uploadPlot.LineWidth = 2;
                uploadPlot.MarkerSize = 0;
                uploadPlot.LegendText = "上传速度";

                // 显示图例
                formsPlot.Plot.ShowLegend(Alignment.UpperRight);

                // 自动调整坐标轴范围
                formsPlot.Plot.Axes.AutoScale();

                // 设置X轴范围为最近的时间窗口
                if (timeArray.Length > 0)
                {
                    var latestTime = timeArray[timeArray.Length - 1];
                    var earliestTime = DateTime.Now.AddSeconds(-maxHistoryPoints).ToOADate();
                    formsPlot.Plot.Axes.SetLimitsX(earliestTime, latestTime);
                }

                // 刷新图表
                formsPlot.Refresh();
            }
            catch (Exception ex)
            {
                // 图表更新出错时不影响主程序运行
                System.Diagnostics.Debug.WriteLine($"Chart update error: {ex.Message}");
            }
        }

        private string FormatSpeed(double bytesPerSecond)
        {
            if (bytesPerSecond < 1024)
                return $"{bytesPerSecond:F1} B/s";
            else if (bytesPerSecond < 1024 * 1024)
                return $"{bytesPerSecond / 1024:F1} KB/s";
            else if (bytesPerSecond < 1024L * 1024 * 1024)
                return $"{bytesPerSecond / (1024 * 1024):F1} MB/s";
            else
                return $"{bytesPerSecond / (1024L * 1024 * 1024):F1} GB/s";
        }

        private string FormatBytes(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F1} KB";
            else if (bytes < 1024L * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024):F1} MB";
            else if (bytes < 1024L * 1024 * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
            else
                return $"{bytes / (1024.0 * 1024 * 1024 * 1024):F1} TB";
        }

        private void comboBoxInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxInterfaces.SelectedIndex >= 0)
            {
                selectedInterface = networkInterfaces[comboBoxInterfaces.SelectedIndex];

                // 重置统计
                var stats = selectedInterface.GetIPv4Statistics();
                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
                lastUpdateTime = DateTime.Now;

                // 清空历史数据
                downloadHistory.Clear();
                uploadHistory.Clear();
                timeHistory.Clear();

                // 清空图表
                formsPlot.Plot.Clear();
                SetupChart();

                labelStatus.Text = $"已切换到: {selectedInterface.Name}";
                labelStatus.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadNetworkInterfaces();
            labelStatus.Text = "网络接口已刷新";
            labelStatus.ForeColor = System.Drawing.Color.Blue;
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (updateTimer.Enabled)
            {
                updateTimer.Stop();
                buttonStartStop.Text = "开始监控";
                buttonStartStop.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
                labelStatus.Text = "监控已停止";
                labelStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                updateTimer.Start();
                buttonStartStop.Text = "停止监控";
                buttonStartStop.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
                labelStatus.Text = "监控已开始";
                labelStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void buttonClearHistory_Click(object sender, EventArgs e)
        {
            downloadHistory.Clear();
            uploadHistory.Clear();
            timeHistory.Clear();

            formsPlot.Plot.Clear();
            SetupChart();

            labelStatus.Text = "历史数据已清空";
            labelStatus.ForeColor = System.Drawing.Color.Blue;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer?.Stop();
            updateTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}