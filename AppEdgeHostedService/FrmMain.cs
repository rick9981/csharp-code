using Microsoft.Extensions.Logging;
using AppEdgeHostedService.Services;
using AppEdgeHostedService.Models;
using System.Text.Json;

namespace AppEdgeHostedService.Forms
{
    public partial class FrmMain : Form
    {
        private readonly PollingManager _pollingManager;
        private readonly ILogger<FrmMain> _logger;
        private readonly ShortTermBuffer _buffer;
        private readonly IModbusClient _modbusClient;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly System.Windows.Forms.Timer _tmrUiUpdate;
        private readonly System.Windows.Forms.Timer _tmrStatusUpdate;
        private bool _isCollecting = false;
        private int _totalRecords = 0;

        public FrmMain(PollingManager pollingManager,
                      ILogger<FrmMain> logger,
                      ShortTermBuffer buffer,
                      IModbusClient modbusClient)
        {
            InitializeComponent();

            _pollingManager = pollingManager;
            _logger = logger;
            _buffer = buffer;
            _modbusClient = modbusClient;
            _cancellationTokenSource = new CancellationTokenSource();

            // UI更新定时器
            _tmrUiUpdate = new System.Windows.Forms.Timer();
            _tmrUiUpdate.Interval = 500;
            _tmrUiUpdate.Tick += TmrUiUpdate_Tick;

            // 状态更新定时器
            _tmrStatusUpdate = new System.Windows.Forms.Timer();
            _tmrStatusUpdate.Interval = 1000;
            _tmrStatusUpdate.Tick += TmrStatusUpdate_Tick;

            // 订阅事件
            _buffer.DataReceived += OnDataReceived;

            InitializeForm();
        }

        private void InitializeForm()
        {
            // 初始化状态
            UpdateConnectionStatus(false);
            UpdateCollectionStatus(false);

            // 初始化DataGridView
            InitializeDataGridView();

            // 启动状态更新定时器
            _tmrStatusUpdate.Start();

            AppendLog("系统初始化完成", LogLevel.Information);
        }

        private void InitializeDataGridView()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToDeleteRows = false;
            dgvData.ReadOnly = true;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.MultiSelect = false;
            dgvData.RowHeadersVisible = false;
            dgvData.BackgroundColor = Color.White;
            dgvData.BorderStyle = BorderStyle.Fixed3D;
            dgvData.EnableHeadersVisualStyles = false;

            // 设置列头样式
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // 设置行样式
            dgvData.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                pgbProgress.Style = ProgressBarStyle.Marquee;

                _cancellationTokenSource = new CancellationTokenSource();

                await Task.Run(() => _pollingManager.Start(_cancellationTokenSource.Token));

                _isCollecting = true;
                UpdateCollectionStatus(true);
                _tmrUiUpdate.Start();

                btnStop.Enabled = true;
                AppendLog("采集系统启动成功", LogLevel.Information);
            }
            catch (Exception ex)
            {
                AppendLog($"启动失败: {ex.Message}", LogLevel.Error);
                btnStart.Enabled = true;
                pgbProgress.Style = ProgressBarStyle.Blocks;
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStop.Enabled = false;
                pgbProgress.Style = ProgressBarStyle.Blocks;

                _cancellationTokenSource.Cancel();
                await _pollingManager.StopAsync(CancellationToken.None);

                _tmrUiUpdate.Stop();
                _isCollecting = false;
                UpdateCollectionStatus(false);

                btnStart.Enabled = true;
                AppendLog("采集系统已停止", LogLevel.Information);
            }
            catch (Exception ex)
            {
                AppendLog($"停止失败: {ex.Message}", LogLevel.Error);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            AppendLog("日志已清空", LogLevel.Information);
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
            _totalRecords = 0;
            UpdateStatistics();
            AppendLog("数据已清空", LogLevel.Information);
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count == 0)
                {
                    MessageBox.Show("没有数据可以导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV文件 (*.csv)|*.csv|JSON文件 (*.json)|*.json";
                saveDialog.FileName = $"EdgeData_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var data = GetDataFromGrid();

                    if (saveDialog.FilterIndex == 1)
                    {
                        ExportToCsv(data, saveDialog.FileName);
                    }
                    else
                    {
                        ExportToJson(data, saveDialog.FileName);
                    }

                    AppendLog($"数据导出成功: {saveDialog.FileName}", LogLevel.Information);
                    MessageBox.Show("数据导出成功!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"导出失败: {ex.Message}", LogLevel.Error);
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                Task.Run(async () =>
                {
                    try
                    {
                        await _modbusClient.ConnectAsync();
                        Invoke(() =>
                        {
                            UpdateConnectionStatus(true);
                            btnDisconnect.Enabled = true;
                            AppendLog("连接成功", LogLevel.Information);
                        });
                    }
                    catch (Exception ex)
                    {
                        Invoke(() =>
                        {
                            AppendLog($"连接失败: {ex.Message}", LogLevel.Error);
                            btnConnect.Enabled = true;
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                AppendLog($"连接失败: {ex.Message}", LogLevel.Error);
                btnConnect.Enabled = true;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnDisconnect.Enabled = false;
                Task.Run(async () =>
                {
                    await _modbusClient.DisconnectAsync();
                    Invoke(() =>
                    {
                        UpdateConnectionStatus(false);
                        btnConnect.Enabled = true;
                        AppendLog("已断开连接", LogLevel.Information);
                    });
                });
            }
            catch (Exception ex)
            {
                AppendLog($"断开连接失败: {ex.Message}", LogLevel.Error);
            }
        }

        private void OnDataReceived(object? sender, CollectionData data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnDataReceived(sender, data)));
                return;
            }

            // 添加到DataGridView
            var row = new object[]
            {
                data.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                data.Address,
                data.Value,
                data.DataType,
                data.Status,
                data.Quality
            };

            dgvData.Rows.Insert(0, row);
            _totalRecords++;

            // 限制显示行数
            if (dgvData.Rows.Count > 2000)
            {
                dgvData.Rows.RemoveAt(dgvData.Rows.Count - 1);
            }

            UpdateStatistics();
        }

        private void TmrUiUpdate_Tick(object? sender, EventArgs e)
        {
            UpdateStatistics();
            UpdateSystemInfo();
        }

        private void TmrStatusUpdate_Tick(object? sender, EventArgs e)
        {
            UpdateConnectionStatus(_modbusClient.IsConnected);
            UpdateSystemInfo();
        }

        private void UpdateConnectionStatus(bool connected)
        {
            lblConnectionStatus.Text = connected ? "已连接" : "未连接";
            lblConnectionStatus.ForeColor = connected ? Color.Green : Color.Red;
            lblConnectionStatus.Font = new Font(lblConnectionStatus.Font, FontStyle.Bold);
        }

        private void UpdateCollectionStatus(bool collecting)
        {
            lblCollectionStatus.Text = collecting ? "采集中" : "已停止";
            lblCollectionStatus.ForeColor = collecting ? Color.Green : Color.Red;
            lblCollectionStatus.Font = new Font(lblCollectionStatus.Font, FontStyle.Bold);
        }

        private void UpdateStatistics()
        {
            lblTotalRecords.Text = $"总记录: {_totalRecords:N0}";
            lblBufferCount.Text = $"缓冲区: {_buffer.Count:N0}";
            lblDisplayRecords.Text = $"显示: {dgvData.Rows.Count:N0}";
        }

        private void UpdateSystemInfo()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            lblMemoryUsage.Text = $"内存: {process.WorkingSet64 / 1024 / 1024:N0} MB";
            lblCpuUsage.Text = $"CPU: {GetCpuUsage():F1}%";
            lblUptime.Text = $"运行: {DateTime.Now.Subtract(process.StartTime):hh\\:mm\\:ss}";
        }

        private double GetCpuUsage()
        {
            // 简化的CPU使用率计算
            return Environment.ProcessorCount > 0 ?
                   (Environment.WorkingSet / (1024.0 * 1024.0)) / Environment.ProcessorCount : 0;
        }

        private void AppendLog(string message, LogLevel level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendLog(message, level)));
                return;
            }

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var levelStr = level.ToString().ToUpper().PadRight(7);
            var logEntry = $"[{timestamp}] [{levelStr}] {message}";

            // 设置颜色
            Color logColor = level switch
            {
                LogLevel.Error => Color.Red,
                LogLevel.Warning => Color.Orange,
                LogLevel.Information => Color.Black,
                LogLevel.Debug => Color.Gray,
                _ => Color.Black
            };

            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = logColor;
            rtbLog.AppendText(logEntry + Environment.NewLine);
            rtbLog.SelectionColor = rtbLog.ForeColor;
            rtbLog.ScrollToCaret();

            // 限制日志长度
            if (rtbLog.Lines.Length > 1000)
            {
                var lines = rtbLog.Lines.Skip(100).ToArray();
                rtbLog.Lines = lines;
            }

            _logger.Log(level, message);
        }

        private List<CollectionData> GetDataFromGrid()
        {
            var data = new List<CollectionData>();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    data.Add(new CollectionData
                    {
                        Timestamp = DateTime.Parse(row.Cells[0].Value.ToString()!),
                        Address = Convert.ToInt32(row.Cells[1].Value),
                        Value = row.Cells[2].Value?.ToString() ?? "",
                        DataType = row.Cells[3].Value?.ToString() ?? "",
                        Status = row.Cells[4].Value?.ToString() ?? "",
                        Quality = row.Cells[5].Value?.ToString() ?? ""
                    });
                }
            }
            return data;
        }

        private void ExportToCsv(List<CollectionData> data, string fileName)
        {
            using var writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);

            writer.WriteLine("时间戳,地址,数值,数据类型,状态,质量");

            foreach (var item in data)
            {
                writer.WriteLine($"{item.Timestamp:yyyy-MM-dd HH:mm:ss.fff},{item.Address},{item.Value},{item.DataType},{item.Status},{item.Quality}");
            }
        }

        private void ExportToJson(List<CollectionData> data, string fileName)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json, System.Text.Encoding.UTF8);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _tmrUiUpdate?.Stop();
            _tmrStatusUpdate?.Stop();
            _cancellationTokenSource?.Cancel();

            if (_isCollecting)
            {
                _pollingManager?.StopAsync(CancellationToken.None).Wait(5000);
            }

            base.OnFormClosing(e);
        }
    }
}