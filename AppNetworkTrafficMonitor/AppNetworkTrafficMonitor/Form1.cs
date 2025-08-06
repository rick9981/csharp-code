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
        private int maxHistoryPoints = 60; // ����60�����ݵ�

        // ScottPlot ���
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
            updateTimer.Interval = 1000; // ÿ�����һ��
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

                // ��ʼ����׼ֵ
                var stats = selectedInterface.GetIPv4Statistics();
                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
            }
        }

        private void SetupChart()
        {
            // ������л�ͼ����
            formsPlot.Plot.Clear();
            formsPlot.Plot.Legend.FontName = "SimSun";

            // ����������
            formsPlot.Plot.Axes.Left.Label.Text = "�ٶ� (KB/s)";
            formsPlot.Plot.Axes.Bottom.Label.Text = "ʱ��";
            formsPlot.Plot.Axes.Left.Label.FontSize = 12;
            formsPlot.Plot.Axes.Bottom.Label.FontSize = 12;
            formsPlot.Plot.Axes.Bottom.Label.FontName = "SimSun";
            formsPlot.Plot.Axes.Left.Label.FontName = "SimSun";

            // ���������ٶ�����
            downloadPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            downloadPlot.Color = ScottPlot.Color.FromHtml("#007BFF"); // ��ɫ
            downloadPlot.LineWidth = 2;
            downloadPlot.MarkerSize = 0;
            downloadPlot.LegendText = "�����ٶ�";
 
            // �����ϴ��ٶ�����
            uploadPlot = formsPlot.Plot.Add.Scatter(new double[0], new double[0]);
            uploadPlot.Color = ScottPlot.Color.FromHtml("#DC3545"); // ��ɫ
            uploadPlot.LineWidth = 2;
            uploadPlot.MarkerSize = 0;
            uploadPlot.LegendText = "�ϴ��ٶ�";

            // ��ʾͼ��
            formsPlot.Plot.ShowLegend(Alignment.UpperRight);

            // ����ʱ�����ʽ
            formsPlot.Plot.Axes.DateTimeTicksBottom();

            // ��ʼˢ��
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
                    // �����ٶ� (KB/s)
                    double downloadSpeed = (stats.BytesReceived - lastBytesReceived) / timeSpan / 1024;
                    double uploadSpeed = (stats.BytesSent - lastBytesSent) / timeSpan / 1024;

                    // ȷ���ٶȲ�Ϊ����
                    downloadSpeed = Math.Max(0, downloadSpeed);
                    uploadSpeed = Math.Max(0, uploadSpeed);

                    // ����ʵʱ��ʾ
                    UpdateRealTimeDisplay(downloadSpeed, uploadSpeed, stats);

                    // ������ʷ����
                    UpdateHistory(downloadSpeed, uploadSpeed, currentTime);

                    // ����ͼ��
                    UpdateChart();
                }

                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
                lastUpdateTime = currentTime;
            }
            catch (Exception ex)
            {
                labelStatus.Text = $"��ش���: {ex.Message}";
                labelStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void UpdateRealTimeDisplay(double downloadSpeed, double uploadSpeed, IPv4InterfaceStatistics stats)
        {
            // ʹ��Invokeȷ����UI�߳��и���
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateRealTimeDisplay(downloadSpeed, uploadSpeed, stats)));
                return;
            }

            labelDownloadSpeed.Text = FormatSpeed(downloadSpeed * 1024); // ת���� bytes/s ���ڸ�ʽ��
            labelUploadSpeed.Text = FormatSpeed(uploadSpeed * 1024);

            labelTotalDownload.Text = $"������: {FormatBytes(stats.BytesReceived)}";
            labelTotalUpload.Text = $"���ϴ�: {FormatBytes(stats.BytesSent)}";

            // ���½����� (���100 KB/s)
            progressBarDownload.Value = Math.Min((int)downloadSpeed, 100);
            progressBarUpload.Value = Math.Min((int)uploadSpeed, 100);

            labelStatus.Text = $"�����... | ����: {FormatSpeed(downloadSpeed * 1024)} | �ϴ�: {FormatSpeed(uploadSpeed * 1024)}";
            labelStatus.ForeColor = System.Drawing.Color.Green;
        }

        private void UpdateHistory(double downloadSpeed, double uploadSpeed, DateTime currentTime)
        {
            downloadHistory.Add(downloadSpeed);
            uploadHistory.Add(uploadSpeed);
            timeHistory.Add(currentTime);

            // ������ʷ������ָ����Χ��
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
                // ת��ʱ��ΪOADate��ʽ����ScottPlot
                double[] timeArray = timeHistory.Select(t => t.ToOADate()).ToArray();
                double[] downloadArray = downloadHistory.ToArray();
                double[] uploadArray = uploadHistory.ToArray();

                // �Ƴ��ɵ�ͼ�����
                if (downloadPlot != null)
                    formsPlot.Plot.Remove(downloadPlot);
                if (uploadPlot != null)
                    formsPlot.Plot.Remove(uploadPlot);

                // �����µ�ͼ�����
                downloadPlot = formsPlot.Plot.Add.Scatter(timeArray, downloadArray);
                downloadPlot.Color = ScottPlot.Color.FromHtml("#007BFF"); // ��ɫ
                downloadPlot.LineWidth = 2;
                downloadPlot.MarkerSize = 0;
                downloadPlot.LegendText = "�����ٶ�";

                uploadPlot = formsPlot.Plot.Add.Scatter(timeArray, uploadArray);
                uploadPlot.Color = ScottPlot.Color.FromHtml("#DC3545"); // ��ɫ
                uploadPlot.LineWidth = 2;
                uploadPlot.MarkerSize = 0;
                uploadPlot.LegendText = "�ϴ��ٶ�";

                // ��ʾͼ��
                formsPlot.Plot.ShowLegend(Alignment.UpperRight);

                // �Զ����������᷶Χ
                formsPlot.Plot.Axes.AutoScale();

                // ����X�᷶ΧΪ�����ʱ�䴰��
                if (timeArray.Length > 0)
                {
                    var latestTime = timeArray[timeArray.Length - 1];
                    var earliestTime = DateTime.Now.AddSeconds(-maxHistoryPoints).ToOADate();
                    formsPlot.Plot.Axes.SetLimitsX(earliestTime, latestTime);
                }

                // ˢ��ͼ��
                formsPlot.Refresh();
            }
            catch (Exception ex)
            {
                // ͼ����³���ʱ��Ӱ������������
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

                // ����ͳ��
                var stats = selectedInterface.GetIPv4Statistics();
                lastBytesReceived = stats.BytesReceived;
                lastBytesSent = stats.BytesSent;
                lastUpdateTime = DateTime.Now;

                // �����ʷ����
                downloadHistory.Clear();
                uploadHistory.Clear();
                timeHistory.Clear();

                // ���ͼ��
                formsPlot.Plot.Clear();
                SetupChart();

                labelStatus.Text = $"���л���: {selectedInterface.Name}";
                labelStatus.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadNetworkInterfaces();
            labelStatus.Text = "����ӿ���ˢ��";
            labelStatus.ForeColor = System.Drawing.Color.Blue;
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (updateTimer.Enabled)
            {
                updateTimer.Stop();
                buttonStartStop.Text = "��ʼ���";
                buttonStartStop.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
                labelStatus.Text = "�����ֹͣ";
                labelStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                updateTimer.Start();
                buttonStartStop.Text = "ֹͣ���";
                buttonStartStop.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
                labelStatus.Text = "����ѿ�ʼ";
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

            labelStatus.Text = "��ʷ���������";
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