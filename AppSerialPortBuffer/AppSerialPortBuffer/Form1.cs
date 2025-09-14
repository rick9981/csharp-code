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

            // ��ʼ���˿��б�  
            RefreshPortList();

            // ��ʼ��������  
            cmbBaudRate.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 });
            cmbBaudRate.SelectedIndex = 4; // 115200  

            // ��ʼ��У��λ  
            foreach (Parity parity in Enum.GetValues(typeof(Parity)))
                cmbParity.Items.Add(parity);
            cmbParity.SelectedIndex = 0; // None  

            // ��ʼ������λ  
            cmbDataBits.Items.AddRange(new object[] { 7, 8 });
            cmbDataBits.SelectedIndex = 1; // 8  

            // ��ʼ��ֹͣλ  
            foreach (StopBits stopBits in Enum.GetValues(typeof(StopBits)))
                cmbStopBits.Items.Add(stopBits);
            cmbStopBits.SelectedIndex = 1; // One  

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;

            // ״̬��ʱ��  
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
                tsslStatus.Text = "��������...";

                // �����ڼ�������Ӱ�ť
                btnConnect.Enabled = false;

                bool success = await _serialHandler.ConnectAsync(config);

                pgbConnection.Style = ProgressBarStyle.Continuous;
                pgbConnection.Value = success ? 100 : 0;

                if (success)
                {
                    UpdateConnectionStatus(true);
                    tsslStatus.Text = "���ӳɹ�";
                }
                else
                {
                    UpdateConnectionStatus(false);
                    tsslStatus.Text = "����ʧ��";
                    MessageBox.Show("����ʧ�ܣ�����˿����ã�", "����",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus(false);
                tsslStatus.Text = "����ʧ��";
                MessageBox.Show("����ʧ�ܣ�����˿����ã�", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                // �Ƚ��ð�ť����ֹ�ظ����
                btnDisconnect.Enabled = false;

                _serialHandler.Disconnect();

                // ǿ�Ƹ���UI״̬
                UpdateConnectionStatus(false);

                tsslStatus.Text = "�ѶϿ�����";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�Ͽ�����ʱ��������: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // ��ʹ����ҲҪ����״̬
                UpdateConnectionStatus(false);
            }
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            if (isConnected)
            {
                lblStatus.Text = "״̬: ������";
                lblStatus.ForeColor = Color.FromArgb(76, 175, 80);

                pnlConfig.Enabled = false;
                grpSettings.Enabled = false;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                pgbConnection.Value = 100;
            }
            else
            {
                lblStatus.Text = "״̬: δ����";
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
                lblBytesSent.Text = $"����: {_bytesSent} �ֽ�";

                rtbReceived.SelectionColor = Color.Blue;
                rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] ����: {txtSend.Text}\n");
                rtbReceived.ScrollToCaret();

                txtSend.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ʧ��: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            rtbReceived.Clear();
            _bytesSent = 0;
            _bytesReceived = 0;
            lblBytesSent.Text = "����: 0 �ֽ�";
            lblBytesReceived.Text = "����: 0 �ֽ�";
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
            lblBytesReceived.Text = $"����: {_bytesReceived} �ֽ�";

            string text = Encoding.UTF8.GetString(data);
            rtbReceived.SelectionColor = Color.Green;
            rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] ����: {text}\n");
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
            rtbReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] ����: {error}\n");
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
