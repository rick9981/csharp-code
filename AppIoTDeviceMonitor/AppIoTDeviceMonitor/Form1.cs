using System.Collections.Concurrent;
using ScottPlot;
using System.Diagnostics;
using System.Text;
using Timer = System.Windows.Forms.Timer;
using ScottPlot.WinForms;

namespace AppIoTDeviceMonitor
{
    public partial class Form1 : Form
    {
        private readonly IoTDeviceManager _deviceManager;
        private readonly DataCollectionMonitor _monitor;
        private readonly Timer _refreshTimer;
        private readonly Random _random = new Random();

        // ScottPlot 数据存储
        private readonly ConcurrentQueue<(DateTime time, double temp)> _tempData = new();
        private readonly ConcurrentQueue<(DateTime time, double humidity)> _humidityData = new();
        private readonly ConcurrentQueue<(DateTime time, double pressure)> _pressureData = new();

        // ScottPlot 绘图对象
        private ScottPlot.Plottables.Scatter _tempPlot;
        private ScottPlot.Plottables.Scatter _humidityPlot;
        private ScottPlot.Plottables.Scatter _pressurePlot;

        private const int MAX_DATA_POINTS = 100;
        public Form1()
        {
            InitializeComponent();
            _deviceManager = new IoTDeviceManager();
            _monitor = new DataCollectionMonitor();

            // 设置刷新定时器
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 1000; // 1秒刷新一次
            _refreshTimer.Tick += RefreshTimer_Tick;

            InitializeDevices();
            InitializeScottPlot();
            InitializeDataGridView();

            _refreshTimer.Start();

        }

        private void InitializeDevices()
        {
            // 添加示例设备
            _deviceManager.AddDevice(new IoTDevice("TEMP_001", "温度传感器1", DeviceType.TemperatureSensor));
            _deviceManager.AddDevice(new IoTDevice("TEMP_002", "温度传感器2", DeviceType.TemperatureSensor));
            _deviceManager.AddDevice(new IoTDevice("HUM_001", "湿度传感器1", DeviceType.HumiditySensor));
            _deviceManager.AddDevice(new IoTDevice("PRES_001", "压力传感器1", DeviceType.PressureSensor));
            _deviceManager.AddDevice(new IoTDevice("MOTOR_001", "电机1", DeviceType.Motor));

            // 启动所有设备
            _deviceManager.StartAllDevices();
        }

        private void InitializeScottPlot()
        {
            // 设置图表基本属性
            formsPlot.Plot.Title("IoT设备实时数据监控");
            formsPlot.Plot.XLabel("时间");
            formsPlot.Plot.YLabel("数值");
            formsPlot.Plot.Font.Set("SimSun");

            // 设置背景色
            formsPlot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#FAFAFA");
            formsPlot.Plot.DataBackground.Color = ScottPlot.Color.FromHex("#FAFAFA");

            // 配置坐标轴
            formsPlot.Plot.Axes.DateTimeTicksBottom();
            formsPlot.Plot.Axes.AutoScale();

            // 创建数据系列
            _tempPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _tempPlot.Color = ScottPlot.Color.FromHex("#FF0000");
            _tempPlot.LineWidth = 2;
            _tempPlot.MarkerSize = 3;
            _tempPlot.LegendText = "温度 (°C)";

            _humidityPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _humidityPlot.Color = ScottPlot.Color.FromHex("#0000FF"); ;
            _humidityPlot.LineWidth = 2;
            _humidityPlot.MarkerSize = 3;
            _humidityPlot.LegendText = "湿度 (%)";

            _pressurePlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _pressurePlot.Color = ScottPlot.Color.FromHex("#008000"); ;
            _pressurePlot.LineWidth = 2;
            _pressurePlot.MarkerSize = 3;
            _pressurePlot.LegendText = "压力 (kPa)";

            // 显示图例
            formsPlot.Plot.ShowLegend(Alignment.UpperLeft);

            // 设置网格
            formsPlot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#C8C8C8");
            formsPlot.Plot.Grid.MinorLineColor = ScottPlot.Color.FromHex("#E6E6E6");

            // 初始刷新
            formsPlot.Refresh();
        }

        private void InitializeDataGridView()
        {
            dgvDevices.AutoGenerateColumns = false;
            dgvDevices.Columns.Add("DeviceId", "设备ID");
            dgvDevices.Columns.Add("DeviceName", "设备名称");
            dgvDevices.Columns.Add("DeviceType", "设备类型");
            dgvDevices.Columns.Add("Status", "状态");
            dgvDevices.Columns.Add("LastValue", "最新值");
            dgvDevices.Columns.Add("LastUpdate", "最后更新");

            // 设置列宽
            dgvDevices.Columns[0].Width = 100;
            dgvDevices.Columns[1].Width = 120;
            dgvDevices.Columns[2].Width = 100;
            dgvDevices.Columns[3].Width = 80;
            dgvDevices.Columns[4].Width = 80;
            dgvDevices.Columns[5].Width = 140;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateDeviceStatus();
            UpdateScottPlot();
            UpdatePerformanceMetrics();
        }

        private void UpdateDeviceStatus()
        {
            dgvDevices.Rows.Clear();

            foreach (var device in _deviceManager.GetAllDevices())
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvDevices);

                row.Cells[0].Value = device.DeviceId;
                row.Cells[1].Value = device.DeviceName;
                row.Cells[2].Value = device.DeviceType.ToString();
                row.Cells[3].Value = device.Status.ToString();
                row.Cells[4].Value = device.LastValue?.ToString("F2") ?? "-";
                row.Cells[5].Value = device.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");

                // 根据状态设置行颜色
                if (device.Status == DeviceStatus.Online)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                else if (device.Status == DeviceStatus.Error)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                else
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

                dgvDevices.Rows.Add(row);
            }
        }

        private void UpdateScottPlot()
        {
            var now = DateTime.Now;

            // 获取并添加温度数据
            var tempDevice = _deviceManager.GetDevice("TEMP_001");
            if (tempDevice != null && tempDevice.LastValue.HasValue)
            {
                _tempData.Enqueue((now, tempDevice.LastValue.Value));

                // 限制数据点数量
                while (_tempData.Count > MAX_DATA_POINTS)
                {
                    _tempData.TryDequeue(out _);
                }
            }

            // 获取并添加湿度数据
            var humDevice = _deviceManager.GetDevice("HUM_001");
            if (humDevice != null && humDevice.LastValue.HasValue)
            {
                _humidityData.Enqueue((now, humDevice.LastValue.Value));

                while (_humidityData.Count > MAX_DATA_POINTS)
                {
                    _humidityData.TryDequeue(out _);
                }
            }

            // 获取并添加压力数据
            var presDevice = _deviceManager.GetDevice("PRES_001");
            if (presDevice != null && presDevice.LastValue.HasValue)
            {
                _pressureData.Enqueue((now, presDevice.LastValue.Value));

                while (_pressureData.Count > MAX_DATA_POINTS)
                {
                    _pressureData.TryDequeue(out _);
                }
            }

            // 更新图表数据
            UpdatePlotData();
        }

        private void UpdatePlotData()
        {
            // 更新温度数据
            if (_tempData.Count > 0)
            {
                var tempArray = _tempData.ToArray();
                var tempX = tempArray.Select(d => d.time.ToOADate()).ToArray();
                var tempY = tempArray.Select(d => d.temp).ToArray();

                // 移除旧的图表并添加新的
                formsPlot.Plot.Remove(_tempPlot);
                _tempPlot = formsPlot.Plot.Add.Scatter(tempX, tempY);
                _tempPlot.Color = ScottPlot.Color.FromHex("#FF0000");
                _tempPlot.LineWidth = 2;
                _tempPlot.MarkerSize = 3;
                _tempPlot.LegendText = "温度 (°C)";
            }

            // 更新湿度数据
            if (_humidityData.Count > 0)
            {
                var humArray = _humidityData.ToArray();
                var humX = humArray.Select(d => d.time.ToOADate()).ToArray();
                var humY = humArray.Select(d => d.humidity).ToArray();

                formsPlot.Plot.Remove(_humidityPlot);
                _humidityPlot = formsPlot.Plot.Add.Scatter(humX, humY);
                _humidityPlot.Color = ScottPlot.Color.FromHex("#0000FF");
                _humidityPlot.LineWidth = 2;
                _humidityPlot.MarkerSize = 3;
                _humidityPlot.LegendText = "湿度 (%)";
            }

            // 更新压力数据
            if (_pressureData.Count > 0)
            {
                var presArray = _pressureData.ToArray();
                var presX = presArray.Select(d => d.time.ToOADate()).ToArray();
                var presY = presArray.Select(d => d.pressure).ToArray();

                formsPlot.Plot.Remove(_pressurePlot);
                _pressurePlot = formsPlot.Plot.Add.Scatter(presX, presY);
                _pressurePlot.Color = ScottPlot.Color.FromHex("#008000");
                _pressurePlot.LineWidth = 2;
                _pressurePlot.MarkerSize = 3;
                _pressurePlot.LegendText = "压力 (kPa)";
            }

            // 重新显示图例
            formsPlot.Plot.ShowLegend(Alignment.UpperLeft);

            // 自动缩放并刷新
            formsPlot.Plot.Axes.AutoScale();
            formsPlot.Refresh();
        }

        private void UpdatePerformanceMetrics()
        {
            // 更新性能指标显示
            var totalDevices = _deviceManager.GetAllDevices().Count();
            var onlineDevices = _deviceManager.GetAllDevices().Count(d => d.Status == DeviceStatus.Online);
            var errorDevices = _deviceManager.GetAllDevices().Count(d => d.Status == DeviceStatus.Error);

            lblTotalDevices.Text = $"总设备数: {totalDevices}";
            lblOnlineDevices.Text = $"在线设备: {onlineDevices}";
            lblErrorDevices.Text = $"故障设备: {errorDevices}";

            // 更新系统性能
            var process = Process.GetCurrentProcess();
            lblMemoryUsage.Text = $"内存使用: {process.WorkingSet64 / 1024 / 1024:F1} MB";
            lblCpuUsage.Text = $"CPU使用率: {GetCpuUsage():F1}%";

            // 更新数据采集速率
            lblDataRate.Text = $"数据采集速率: {_deviceManager.GetDataRate():F0} 条/秒";
        }

        private double GetCpuUsage()
        {
            // 简单的CPU使用率计算
            return _random.NextDouble() * 20 + 10; // 模拟10-30%的CPU使用率
        }

        private void btnStartMonitoring_Click(object sender, EventArgs e)
        {
            _deviceManager.StartAllDevices();
            _refreshTimer.Start();
            btnStartMonitoring.Enabled = false;
            btnStopMonitoring.Enabled = true;
            MessageBox.Show("监控已启动", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStopMonitoring_Click(object sender, EventArgs e)
        {
            _deviceManager.StopAllDevices();
            _refreshTimer.Stop();
            btnStartMonitoring.Enabled = true;
            btnStopMonitoring.Enabled = false;
            MessageBox.Show("监控已停止", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetChart_Click(object sender, EventArgs e)
        {
            // 清空所有数据
            while (_tempData.TryDequeue(out _)) { }
            while (_humidityData.TryDequeue(out _)) { }
            while (_pressureData.TryDequeue(out _)) { }

            // 完全清空图表并重新初始化
            formsPlot.Plot.Clear();
            InitializeScottPlot();

            MessageBox.Show("图表已重置", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "导出设备数据"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportDeviceData(saveDialog.FileName);
                MessageBox.Show("数据导出成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportDeviceData(string fileName)
        {
            var csv = new StringBuilder();
            csv.AppendLine("设备ID,设备名称,设备类型,状态,最新值,最后更新时间");

            foreach (var device in _deviceManager.GetAllDevices())
            {
                csv.AppendLine($"{device.DeviceId},{device.DeviceName},{device.DeviceType}," +
                             $"{device.Status},{device.LastValue},{device.LastUpdateTime}");
            }

            System.IO.File.WriteAllText(fileName, csv.ToString(), Encoding.UTF8);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            formsPlot.Plot.Axes.Zoom(0.8, 0.8);
            formsPlot.Refresh();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            formsPlot.Plot.Axes.Zoom(1.2, 1.2);
            formsPlot.Refresh();
        }

        private void btnAutoScale_Click(object sender, EventArgs e)
        {
            formsPlot.Plot.Axes.AutoScale();
            formsPlot.Refresh();
        }

        private void btnSaveChart_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "PNG files (*.png)|*.png|JPEG files (*.jpg)|*.jpg",
                Title = "保存图表"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                formsPlot.Plot.Save(saveDialog.FileName, formsPlot.Width, formsPlot.Height);
                MessageBox.Show("图表已保存", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _refreshTimer?.Stop();
            _deviceManager?.Dispose();
            _monitor?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
