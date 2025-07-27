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

        // ScottPlot ���ݴ洢
        private readonly ConcurrentQueue<(DateTime time, double temp)> _tempData = new();
        private readonly ConcurrentQueue<(DateTime time, double humidity)> _humidityData = new();
        private readonly ConcurrentQueue<(DateTime time, double pressure)> _pressureData = new();

        // ScottPlot ��ͼ����
        private ScottPlot.Plottables.Scatter _tempPlot;
        private ScottPlot.Plottables.Scatter _humidityPlot;
        private ScottPlot.Plottables.Scatter _pressurePlot;

        private const int MAX_DATA_POINTS = 100;
        public Form1()
        {
            InitializeComponent();
            _deviceManager = new IoTDeviceManager();
            _monitor = new DataCollectionMonitor();

            // ����ˢ�¶�ʱ��
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 1000; // 1��ˢ��һ��
            _refreshTimer.Tick += RefreshTimer_Tick;

            InitializeDevices();
            InitializeScottPlot();
            InitializeDataGridView();

            _refreshTimer.Start();

        }

        private void InitializeDevices()
        {
            // ���ʾ���豸
            _deviceManager.AddDevice(new IoTDevice("TEMP_001", "�¶ȴ�����1", DeviceType.TemperatureSensor));
            _deviceManager.AddDevice(new IoTDevice("TEMP_002", "�¶ȴ�����2", DeviceType.TemperatureSensor));
            _deviceManager.AddDevice(new IoTDevice("HUM_001", "ʪ�ȴ�����1", DeviceType.HumiditySensor));
            _deviceManager.AddDevice(new IoTDevice("PRES_001", "ѹ��������1", DeviceType.PressureSensor));
            _deviceManager.AddDevice(new IoTDevice("MOTOR_001", "���1", DeviceType.Motor));

            // ���������豸
            _deviceManager.StartAllDevices();
        }

        private void InitializeScottPlot()
        {
            // ����ͼ���������
            formsPlot.Plot.Title("IoT�豸ʵʱ���ݼ��");
            formsPlot.Plot.XLabel("ʱ��");
            formsPlot.Plot.YLabel("��ֵ");
            formsPlot.Plot.Font.Set("SimSun");

            // ���ñ���ɫ
            formsPlot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#FAFAFA");
            formsPlot.Plot.DataBackground.Color = ScottPlot.Color.FromHex("#FAFAFA");

            // ����������
            formsPlot.Plot.Axes.DateTimeTicksBottom();
            formsPlot.Plot.Axes.AutoScale();

            // ��������ϵ��
            _tempPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _tempPlot.Color = ScottPlot.Color.FromHex("#FF0000");
            _tempPlot.LineWidth = 2;
            _tempPlot.MarkerSize = 3;
            _tempPlot.LegendText = "�¶� (��C)";

            _humidityPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _humidityPlot.Color = ScottPlot.Color.FromHex("#0000FF"); ;
            _humidityPlot.LineWidth = 2;
            _humidityPlot.MarkerSize = 3;
            _humidityPlot.LegendText = "ʪ�� (%)";

            _pressurePlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            _pressurePlot.Color = ScottPlot.Color.FromHex("#008000"); ;
            _pressurePlot.LineWidth = 2;
            _pressurePlot.MarkerSize = 3;
            _pressurePlot.LegendText = "ѹ�� (kPa)";

            // ��ʾͼ��
            formsPlot.Plot.ShowLegend(Alignment.UpperLeft);

            // ��������
            formsPlot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#C8C8C8");
            formsPlot.Plot.Grid.MinorLineColor = ScottPlot.Color.FromHex("#E6E6E6");

            // ��ʼˢ��
            formsPlot.Refresh();
        }

        private void InitializeDataGridView()
        {
            dgvDevices.AutoGenerateColumns = false;
            dgvDevices.Columns.Add("DeviceId", "�豸ID");
            dgvDevices.Columns.Add("DeviceName", "�豸����");
            dgvDevices.Columns.Add("DeviceType", "�豸����");
            dgvDevices.Columns.Add("Status", "״̬");
            dgvDevices.Columns.Add("LastValue", "����ֵ");
            dgvDevices.Columns.Add("LastUpdate", "������");

            // �����п�
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

                // ����״̬��������ɫ
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

            // ��ȡ������¶�����
            var tempDevice = _deviceManager.GetDevice("TEMP_001");
            if (tempDevice != null && tempDevice.LastValue.HasValue)
            {
                _tempData.Enqueue((now, tempDevice.LastValue.Value));

                // �������ݵ�����
                while (_tempData.Count > MAX_DATA_POINTS)
                {
                    _tempData.TryDequeue(out _);
                }
            }

            // ��ȡ�����ʪ������
            var humDevice = _deviceManager.GetDevice("HUM_001");
            if (humDevice != null && humDevice.LastValue.HasValue)
            {
                _humidityData.Enqueue((now, humDevice.LastValue.Value));

                while (_humidityData.Count > MAX_DATA_POINTS)
                {
                    _humidityData.TryDequeue(out _);
                }
            }

            // ��ȡ�����ѹ������
            var presDevice = _deviceManager.GetDevice("PRES_001");
            if (presDevice != null && presDevice.LastValue.HasValue)
            {
                _pressureData.Enqueue((now, presDevice.LastValue.Value));

                while (_pressureData.Count > MAX_DATA_POINTS)
                {
                    _pressureData.TryDequeue(out _);
                }
            }

            // ����ͼ������
            UpdatePlotData();
        }

        private void UpdatePlotData()
        {
            // �����¶�����
            if (_tempData.Count > 0)
            {
                var tempArray = _tempData.ToArray();
                var tempX = tempArray.Select(d => d.time.ToOADate()).ToArray();
                var tempY = tempArray.Select(d => d.temp).ToArray();

                // �Ƴ��ɵ�ͼ������µ�
                formsPlot.Plot.Remove(_tempPlot);
                _tempPlot = formsPlot.Plot.Add.Scatter(tempX, tempY);
                _tempPlot.Color = ScottPlot.Color.FromHex("#FF0000");
                _tempPlot.LineWidth = 2;
                _tempPlot.MarkerSize = 3;
                _tempPlot.LegendText = "�¶� (��C)";
            }

            // ����ʪ������
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
                _humidityPlot.LegendText = "ʪ�� (%)";
            }

            // ����ѹ������
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
                _pressurePlot.LegendText = "ѹ�� (kPa)";
            }

            // ������ʾͼ��
            formsPlot.Plot.ShowLegend(Alignment.UpperLeft);

            // �Զ����Ų�ˢ��
            formsPlot.Plot.Axes.AutoScale();
            formsPlot.Refresh();
        }

        private void UpdatePerformanceMetrics()
        {
            // ��������ָ����ʾ
            var totalDevices = _deviceManager.GetAllDevices().Count();
            var onlineDevices = _deviceManager.GetAllDevices().Count(d => d.Status == DeviceStatus.Online);
            var errorDevices = _deviceManager.GetAllDevices().Count(d => d.Status == DeviceStatus.Error);

            lblTotalDevices.Text = $"���豸��: {totalDevices}";
            lblOnlineDevices.Text = $"�����豸: {onlineDevices}";
            lblErrorDevices.Text = $"�����豸: {errorDevices}";

            // ����ϵͳ����
            var process = Process.GetCurrentProcess();
            lblMemoryUsage.Text = $"�ڴ�ʹ��: {process.WorkingSet64 / 1024 / 1024:F1} MB";
            lblCpuUsage.Text = $"CPUʹ����: {GetCpuUsage():F1}%";

            // �������ݲɼ�����
            lblDataRate.Text = $"���ݲɼ�����: {_deviceManager.GetDataRate():F0} ��/��";
        }

        private double GetCpuUsage()
        {
            // �򵥵�CPUʹ���ʼ���
            return _random.NextDouble() * 20 + 10; // ģ��10-30%��CPUʹ����
        }

        private void btnStartMonitoring_Click(object sender, EventArgs e)
        {
            _deviceManager.StartAllDevices();
            _refreshTimer.Start();
            btnStartMonitoring.Enabled = false;
            btnStopMonitoring.Enabled = true;
            MessageBox.Show("���������", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStopMonitoring_Click(object sender, EventArgs e)
        {
            _deviceManager.StopAllDevices();
            _refreshTimer.Stop();
            btnStartMonitoring.Enabled = true;
            btnStopMonitoring.Enabled = false;
            MessageBox.Show("�����ֹͣ", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetChart_Click(object sender, EventArgs e)
        {
            // �����������
            while (_tempData.TryDequeue(out _)) { }
            while (_humidityData.TryDequeue(out _)) { }
            while (_pressureData.TryDequeue(out _)) { }

            // ��ȫ���ͼ�����³�ʼ��
            formsPlot.Plot.Clear();
            InitializeScottPlot();

            MessageBox.Show("ͼ��������", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "�����豸����"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportDeviceData(saveDialog.FileName);
                MessageBox.Show("���ݵ����ɹ�", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportDeviceData(string fileName)
        {
            var csv = new StringBuilder();
            csv.AppendLine("�豸ID,�豸����,�豸����,״̬,����ֵ,������ʱ��");

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
                Title = "����ͼ��"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                formsPlot.Plot.Save(saveDialog.FileName, formsPlot.Width, formsPlot.Height);
                MessageBox.Show("ͼ���ѱ���", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
