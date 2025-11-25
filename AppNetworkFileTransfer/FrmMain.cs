using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppNetworkFileTransfer.Core;
using AppNetworkFileTransfer.Models;

namespace AppNetworkFileTransfer
{
    public partial class FrmMain : Form
    {
        private FileTransferServer _server;
        private FileTransferClient _client;
        private bool _isConnected = false;
        private bool _isServer = false;
        private DateTime _transferStartTime;
        private long _totalBytes = 0;
        private long _transferredBytes = 0;
        private bool _isServerListening = false;
        private bool _isClientConnected = false;

        public FrmMain()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            cmbMode.SelectedIndex = 0;
            UpdateConnectionControls();
            UpdateTransferControls();

            // 设置默认保存目录
            txtSaveDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            AddLog("应用程序启动", Color.Blue);
            tsslTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isConnected)
            {
                btnDisconnect_Click(null, null);
            }
            AddLog("应用程序关闭", Color.Blue);
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            tsslTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isServer = cmbMode.SelectedIndex == 0;
            txtServerIP.Enabled = !_isServer;
            lblServerIP.Text = _isServer ? "监听IP:" : "服务器IP:";

            // 显示/隐藏相关控件
            grpServerSettings.Visible = _isServer;
            grpClientSettings.Visible = !_isServer;

            if (_isServer)
            {
                txtServerIP.Text = GetLocalIPAddress();
            }
            else
            {
                txtServerIP.Text = "127.0.0.1";
            }

            UpdateTransferControls();
        }

        private void btnBrowseSaveDirectory_Click(object sender, EventArgs e)
        {
            if (fbdSaveDirectory.ShowDialog() == DialogResult.OK)
            {
                txtSaveDirectory.Text = fbdSaveDirectory.SelectedPath;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = false;
                tsslStatus.Text = "连接中...";

                if (_isServer)
                {
                    await StartServer();
                    AddLog("服务器启动成功，等待客户端连接...", Color.Green);
                    _isConnected = true;
                }
                else
                {
                    await StartClient();
                    _isConnected = true;
                    AddLog("已连接到服务器", Color.Green);
                }
            }
            catch (Exception ex)
            {
                _isConnected = false;
                _isServerListening = false;
                _isClientConnected = false;
                AddLog($"连接失败: {ex.Message}", Color.Red);
                tsslStatus.Text = "连接失败";
            }
            finally
            {
                UpdateConnectionControls();
                UpdateTransferControls();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_server != null)
                {
                    _server.Stop();
                    _server = null;
                }

                if (_client != null)
                {
                    _client.Disconnect();
                    _client = null;
                }

                _isConnected = false;
                _isServerListening = false;
                _isClientConnected = false;
                UpdateConnectionControls();
                UpdateTransferControls();
                ResetProgress();
                AddLog("连接已断开", Color.Orange);
                tsslStatus.Text = "已断开";
            }
            catch (Exception ex)
            {
                AddLog($"断开连接时出错: {ex.Message}", Color.Red);
            }
        }

        private void btnBrowseLocal_Click(object sender, EventArgs e)
        {
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {
                txtLocalPath.Text = ofdSelectFile.FileName;
            }
        }

        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLocalPath.Text) || !File.Exists(txtLocalPath.Text))
            {
                MessageBox.Show("请选择有效的本地文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSendFile.Enabled = false;

                var fileInfo = new FileInfo(txtLocalPath.Text);
                _totalBytes = fileInfo.Length;
                _transferredBytes = 0;
                _transferStartTime = DateTime.Now;

                lblCurrentFileValue.Text = Path.GetFileName(txtLocalPath.Text);
                pgbFileProgress.Value = 0;
                lblProgressPercent.Text = "0%";

                AddLog($"开始发送文件: {txtLocalPath.Text}", Color.Blue);

                if (_client != null)
                {
                    await _client.SendFileAsync(txtLocalPath.Text);
                }

                AddLog("文件发送完成", Color.Green);
            }
            catch (Exception ex)
            {
                AddLog($"发送文件失败: {ex.Message}", Color.Red);
            }
            finally
            {
                btnSendFile.Enabled = true;
            }
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        private void tsmiSaveLog_Click(object sender, EventArgs e)
        {
            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(sfdSaveFile.FileName, rtbLog.Text);
                    AddLog($"日志已保存到: {sfdSaveFile.FileName}", Color.Green);
                }
                catch (Exception ex)
                {
                    AddLog($"保存日志失败: {ex.Message}", Color.Red);
                }
            }
        }

        private async Task StartServer()
        {
            _server = new FileTransferServer();
            _server.SaveDirectory = txtSaveDirectory.Text; // 设置保存目录
            _server.ProgressChanged += OnProgressChanged;
            _server.StatusChanged += OnStatusChanged;
            _server.ClientConnected += OnClientConnected;
            _server.TransferStarted += OnTransferStarted;

            var port = (int)nudPort.Value;
            await _server.StartListening(port);
            _isServerListening = true;
        }

        private void OnTransferStarted(object sender, TransferEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, TransferEventArgs>(OnTransferStarted), sender, e);
                return;
            }

            // 重置传输相关变量
            _totalBytes = e.TotalBytes;
            _transferredBytes = 0;
            _transferStartTime = DateTime.Now; // 在这里设置开始时间

            // 更新UI
            lblCurrentFileValue.Text = Path.GetFileName(e.FileName);
            pgbFileProgress.Value = 0;
            lblProgressPercent.Text = "0%";
            lblSpeedValue.Text = "准备中...";
            lblTimeRemainingValue.Text = "准备中...";
        }

        private void OnClientConnected(object sender, TransferEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, TransferEventArgs>(OnClientConnected), sender, e);
                return;
            }

            _isClientConnected = true;
            _isConnected = true;
            UpdateConnectionControls();
            UpdateTransferControls();
            AddLog("客户端已连接，准备接收文件", Color.Green);
            tsslStatus.Text = "客户端已连接";
        }

        private async Task StartClient()
        {
            _client = new FileTransferClient();
            _client.ProgressChanged += OnProgressChanged;
            _client.StatusChanged += OnStatusChanged;
            _client.TransferStarted += OnTransferStarted;

            var serverIP = txtServerIP.Text;
            var port = (int)nudPort.Value;
            await _client.ConnectAsync(serverIP, port);
        }

        private void OnProgressChanged(object sender, TransferEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, TransferEventArgs>(OnProgressChanged), sender, e);
                return;
            }

            _transferredBytes = e.TransferredBytes;
            _totalBytes = e.TotalBytes;

            if (_totalBytes > 0)
            {
                var progress = (int)((_transferredBytes * 100) / _totalBytes);
                pgbFileProgress.Value = Math.Min(progress, 100);
                lblProgressPercent.Text = $"{progress}%";

                // 计算传输速度
                var elapsed = DateTime.Now - _transferStartTime;

                // 只有经过一定时间后才计算速度，避免除零或极小值导致的问题
                if (elapsed.TotalSeconds >= 1.0) // 至少1秒
                {
                    var speed = _transferredBytes / elapsed.TotalSeconds;
                    lblSpeedValue.Text = FormatBytes((long)speed) + "/s";

                    // 计算剩余时间
                    if (speed > 0 && _transferredBytes > 0)
                    {
                        var remainingBytes = _totalBytes - _transferredBytes;
                        var remainingSeconds = remainingBytes / speed;

                        // 限制最大剩余时间，避免 TimeSpan 溢出
                        if (remainingSeconds > 0 && remainingSeconds <= TimeSpan.MaxValue.TotalSeconds)
                        {
                            // 限制最大显示时间为999小时，避免显示过大的不合理时间
                            var maxHours = 999;
                            var maxSeconds = maxHours * 3600;

                            if (remainingSeconds > maxSeconds)
                            {
                                lblTimeRemainingValue.Text = $"> {maxHours}小时";
                            }
                            else
                            {
                                try
                                {
                                    lblTimeRemainingValue.Text = FormatTime(TimeSpan.FromSeconds(remainingSeconds));
                                }
                                catch (OverflowException)
                                {
                                    lblTimeRemainingValue.Text = "> 999小时";
                                }
                            }
                        }
                        else
                        {
                            lblTimeRemainingValue.Text = "计算中...";
                        }
                    }
                    else
                    {
                        lblTimeRemainingValue.Text = "计算中...";
                    }
                }
                else
                {
                    lblSpeedValue.Text = "计算中...";
                    lblTimeRemainingValue.Text = "计算中...";
                }
            }
        }

        private void OnStatusChanged(object sender, TransferEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, TransferEventArgs>(OnStatusChanged), sender, e);
                return;
            }

            AddLog(e.Message, e.IsError ? Color.Red : Color.Black);
            tsslStatus.Text = e.Message;
        }

        private void UpdateConnectionControls()
        {
            btnConnect.Enabled = !_isConnected;
            btnDisconnect.Enabled = _isConnected;
            cmbMode.Enabled = !_isConnected;
            nudPort.Enabled = !_isConnected;
            txtServerIP.Enabled = !_isConnected && !_isServer;
            txtSaveDirectory.Enabled = !_isConnected;
            btnBrowseSaveDirectory.Enabled = !_isConnected;

            if (_isServer)
            {
                if (_isServerListening && _isClientConnected)
                {
                    lblStatusValue.Text = "已连接(客户端已连接)";
                    lblStatusValue.ForeColor = Color.Green;
                    tsslStatus.Text = "客户端已连接";
                }
                else if (_isServerListening)
                {
                    lblStatusValue.Text = "监听中(等待客户端)";
                    lblStatusValue.ForeColor = Color.Orange;
                    tsslStatus.Text = "等待客户端连接";
                }
                else
                {
                    lblStatusValue.Text = "未连接";
                    lblStatusValue.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatusValue.Text = _isConnected ? "已连接" : "未连接";
                lblStatusValue.ForeColor = _isConnected ? Color.Green : Color.Red;
            }
        }

        private void UpdateTransferControls()
        {
            if (_isServer)
            {
                // 服务器模式：只显示接收状态，不需要操作按钮
                btnSendFile.Visible = false;
                grpClientSettings.Visible = false;
            }
            else
            {
                // 客户端模式：只显示发送相关控件
                bool canTransfer = _isConnected;
                btnSendFile.Visible = true;
                btnSendFile.Enabled = canTransfer;
                grpClientSettings.Visible = true;
            }
        }

        private void ResetProgress()
        {
            pgbFileProgress.Value = 0;
            lblProgressPercent.Text = "0%";
            lblCurrentFileValue.Text = "--";
            lblSpeedValue.Text = "--";
            lblTimeRemainingValue.Text = "--";
        }

        private void AddLog(string message, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color>(AddLog), message, color);
                return;
            }

            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = color;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            rtbLog.SelectionColor = rtbLog.ForeColor;
            rtbLog.ScrollToCaret();
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { }
            return "127.0.0.1";
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private string FormatTime(TimeSpan timeSpan)
        {
            try
            {
                // 处理负值
                if (timeSpan.TotalSeconds <= 0)
                    return "0秒";

                // 处理过大的值
                if (timeSpan.TotalDays >= 365)
                    return "> 1年";

                if (timeSpan.TotalDays >= 30)
                    return $"{(int)(timeSpan.TotalDays / 30)}个月";

                if (timeSpan.TotalDays >= 1)
                    return $"{(int)timeSpan.TotalDays}天{timeSpan.Hours}小时";

                if (timeSpan.TotalHours >= 1)
                    return $"{(int)timeSpan.TotalHours}小时{timeSpan.Minutes}分钟";

                if (timeSpan.TotalMinutes >= 1)
                    return $"{(int)timeSpan.TotalMinutes}分钟{timeSpan.Seconds}秒";

                return $"{(int)timeSpan.TotalSeconds}秒";
            }
            catch (Exception)
            {
                return "未知";
            }
        }
    }
}