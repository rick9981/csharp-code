using System.IO.Ports;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace AppSerialPortBuffer
{
    public partial class Form1 : Form
    {
        private OptimizedSerialPortHandler _serialHandler;
        private Timer _statusTimer;
        private long _bytesSent = 0;
        private long _bytesReceived = 0;
        public Form1()
        {
            InitializeComponent();
            InitializeApplication();
        }
        private void InitializeApplication()
        {
            _serialHandler = new OptimizedSerialPortHandler();
            _serialHandler.DataReceived += OnDataReceived;
            _serialHandler.ErrorOccurred += OnErrorOccurred;

            // 初始化端口列表  
            RefreshPortList();

            // 初始化波特率  
            cmbBaudRate.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });
            cmbBaudRate.SelectedIndex = 4; // 115200  

            // 初始化校验位  
            foreach (Parity parity in Enum.GetValues(typeof(Parity)))
                cmbParity.Items.Add(parity);
            cmbParity.SelectedIndex = 0; // None  

            // 初始化数据位  
            cmbDataBits.Items.AddRange(new object[] { 7, 8 });
            cmbDataBits.SelectedIndex = 1; // 8  

            // 初始化停止位  
            foreach (StopBits stopBits in Enum.GetValues(typeof(StopBits)))
                cmbStopBits.Items.Add(stopBits);
            cmbStopBits.SelectedIndex = 1; // One  

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;

            // 状态定时器  
            _statusTimer = new Timer();
            _statusTimer.Interval = 1000;
            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();
        }

        private void RefreshPortList()
        {
            cmbPort.Items.Clear();
            cmbPort.Items.AddRange(SerialPort.GetPortNames());
            if (cmbPort.Items.Count > 0)
                cmbPort.SelectedIndex = 0;
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            if (cmbPort.SelectedItem == null) return;

            try
            {
                var config = new SerialPortConfig
                {
                    PortName = cmbPort.SelectedItem.ToString(),
                    BaudRate = (int)cmbBaudRate.SelectedItem,
                    Parity = (Parity)cmbParity.SelectedItem,
                    DataBits = (int)cmbDataBits.SelectedItem,
                    StopBits = (StopBits)cmbStopBits.SelectedItem,
                    ReadBufferSize = (int)nudReadBuffer.Value,
                    WriteBufferSize = (int)nudWriteBuffer.Value,
                    ReadTimeout = (int)nudTimeout.Value,
                    WriteTimeout = (int)nudTimeout.Value
                };

                pgbConnection.Style = ProgressBarStyle.Marquee;
                tsslStatus.Text = "正在连接...";

                // 连接期间禁用连接按钮
                btnConnect.Enabled = false;

                bool success = await _serialHandler.ConnectAsync(config);

                pgbConnection.Style = ProgressBarStyle.Continuous;
                pgbConnection.Value = success ? 100 : 0;

                if (success)
                {
                    UpdateConnectionStatus(true);
                    tsslStatus.Text = "连接成功";
                }
                else
                {
                    UpdateConnectionStatus(false);
                    tsslStatus.Text = "连接失败";
                    MessageBox.Show("连接失败，请检查端口设置！", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus(false);
                tsslStatus.Text = "连接失败";
                MessageBox.Show("连接失败，请检查端口设置！", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 先禁用按钮，防止重复点击
                btnDisconnect.Enabled = false;

                _serialHandler.Disconnect();

                // 强制更新UI状态
                UpdateConnectionStatus(false);

                tsslStatus.Text = "已断开连接";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"断开连接时发生错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 即使出错也要更新状态
                UpdateConnectionStatus(false);
            }
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            if (isConnected)
            {
                lblStatus.Text = "状态: 已连接";
                lblStatus.ForeColor = Color.FromArgb(76, 175, 80);

                pnlConfig.Enabled = false;
                grpSettings.Enabled = false;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                pgbConnection.Value = 100;
            }
            else
            {
                lblStatus.Text = "状态: 未连接";
                lblStatus.ForeColor = Color.FromArgb(244, 67, 54);

                pnlConfig.Enabled = true;
                grpSettings.Enabled = true;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                pgbConnection.Value = 0;
            }
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (!_serialHandler.IsConnected || string.IsNullOrEmpty(txtSend.Text))
                return;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txtSend.Text);

                if (chkAsync.Checked)
                {
                    await _serialHandler.SendDataAsync(data);
                }
                else
                {
                    _serialHandler.SendData(data);
                }

                _bytesSent += data.Length;
                lblBytesSent.Text = $"发送: {_bytesSent} 字节";

                rtbReceived.SelectionColor = Color.Blue;
                rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] 发送: {txtSend.Text}\n");
                rtbReceived.ScrollToCaret();

                txtSend.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            rtbReceived.Clear();
            _bytesSent = 0;
            _bytesReceived = 0;
            lblBytesSent.Text = "发送: 0 字节";
            lblBytesReceived.Text = "接收: 0 字节";
        }

        private void TxtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSend_Click(sender, e);
                e.Handled = true;
            }
        }

        private void OnDataReceived(byte[] data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<byte[]>(OnDataReceived), data);
                return;
            }

            _bytesReceived += data.Length;
            lblBytesReceived.Text = $"接收: {_bytesReceived} 字节";

            string text = Encoding.UTF8.GetString(data);
            rtbReceived.SelectionColor = Color.Green;
            rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] 接收: {text}\n");
            rtbReceived.ScrollToCaret();
        }

        private void OnErrorOccurred(string error)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnErrorOccurred), error);
                return;
            }

            rtbReceived.SelectionColor = Color.Red;
            rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] 错误: {error}\n");
            rtbReceived.ScrollToCaret();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            tsslTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _serialHandler?.Disconnect();
            _statusTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}
