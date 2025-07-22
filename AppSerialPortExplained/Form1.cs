using System.IO.Ports;
using Timer = System.Windows.Forms.Timer;

namespace AppSerialPortExplained
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort = new SerialPort();
        private Timer statusTimer = new Timer();
        public Form1()
        {
            InitializeComponent();
            RefreshPortList();

            comboBoxBaud.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
            comboBoxBaud.SelectedIndex = 3;
            comboBoxParity.Items.AddRange(new object[] { "None", "Even", "Odd", "Mark", "Space" });
            comboBoxParity.SelectedIndex = 0;
            comboBoxDataBits.Items.AddRange(new object[] { "5", "6", "7", "8" });
            comboBoxDataBits.SelectedIndex = 3; 
            comboBoxStopBits.Items.AddRange(new object[] { "One", "Two", "OnePointFive" });
            comboBoxStopBits.SelectedIndex = 0; 
            comboBoxHandshake.Items.AddRange(new object[] { "None", "XOnXOff", "RequestToSend", "RequestToSendXOnXOff" });
            comboBoxHandshake.SelectedIndex = 0;
            numericUpDownReadTimeout.Value = 500;
            numericUpDownWriteTimeout.Value = 500;
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.ErrorReceived += SerialPort_ErrorReceived;
            serialPort.PinChanged += SerialPort_PinChanged;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            statusTimer.Interval = 100; 
            statusTimer.Tick += StatusTimer_Tick;
        }

        private void RefreshPortList()
        {
            string selectedPort = comboBoxPort.Text;
            comboBoxPort.Items.Clear();
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());

            if (comboBoxPort.Items.Count > 0)
            {
                if (comboBoxPort.Items.Contains(selectedPort))
                    comboBoxPort.Text = selectedPort;
                else
                    comboBoxPort.SelectedIndex = 0;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshPortList();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    // 配置串口参数
                    serialPort.PortName = comboBoxPort.Text;
                    serialPort.BaudRate = int.Parse(comboBoxBaud.Text);

                    // 设置校验位
                    switch (comboBoxParity.Text)
                    {
                        case "None": serialPort.Parity = Parity.None; break;
                        case "Even": serialPort.Parity = Parity.Even; break;
                        case "Odd": serialPort.Parity = Parity.Odd; break;
                        case "Mark": serialPort.Parity = Parity.Mark; break;
                        case "Space": serialPort.Parity = Parity.Space; break;
                    }

                    serialPort.DataBits = int.Parse(comboBoxDataBits.Text);

                    // 设置停止位
                    switch (comboBoxStopBits.Text)
                    {
                        case "One": serialPort.StopBits = StopBits.One; break;
                        case "Two": serialPort.StopBits = StopBits.Two; break;
                        case "OnePointFive": serialPort.StopBits = StopBits.OnePointFive; break;
                    }

                    // 设置流控制
                    switch (comboBoxHandshake.Text)
                    {
                        case "None": serialPort.Handshake = Handshake.None; break;
                        case "XOnXOff": serialPort.Handshake = Handshake.XOnXOff; break;
                        case "RequestToSend": serialPort.Handshake = Handshake.RequestToSend; break;
                        case "RequestToSendXOnXOff": serialPort.Handshake = Handshake.RequestToSendXOnXOff; break;
                    }

                    // 设置超时时间
                    serialPort.ReadTimeout = (int)numericUpDownReadTimeout.Value;
                    serialPort.WriteTimeout = (int)numericUpDownWriteTimeout.Value;

                    // 打开串口
                    serialPort.Open();

                    // 更新UI状态
                    buttonOpen.Text = "关闭串口";
                    buttonOpen.BackColor = Color.Red;
                    groupBoxConfig.Enabled = false;
                    groupBoxSend.Enabled = true;

                    // 开始状态监控
                    statusTimer.Start();

                    AppendLog("串口已打开");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("打开串口失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    serialPort.Close();

                    // 更新UI状态
                    buttonOpen.Text = "打开串口";
                    buttonOpen.BackColor = Color.Green;
                    groupBoxConfig.Enabled = true;
                    groupBoxSend.Enabled = false;

                    // 停止状态监控
                    statusTimer.Stop();

                    // 清空状态显示
                    labelCtsStatus.Text = "CTS: -";
                    labelCdStatus.Text = "CD: -";
                    labelCtsStatus.ForeColor = Color.Gray;
                    labelCdStatus.ForeColor = Color.Gray;

                    AppendLog("串口已关闭");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("关闭串口失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen && !string.IsNullOrEmpty(textBoxSend.Text))
            {
                try
                {
                    string dataToSend = textBoxSend.Text;

                    // 根据发送模式添加结束符
                    if (checkBoxNewLine.Checked)
                        dataToSend += "\r\n";

                    serialPort.Write(dataToSend);

                    // 显示发送的数据
                    if (checkBoxShowSent.Checked)
                    {
                        AppendReceiveData($"[发送] {dataToSend}", Color.Blue);
                    }

                    // 清空发送框（如果选中了自动清空）
                    if (checkBoxAutoClear.Checked)
                        textBoxSend.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppendLog($"发送错误: {ex.Message}");
                }
            }
        }

        private void buttonClearReceive_Click(object sender, EventArgs e)
        {
            textBoxReceive.Clear();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
        }

        // 串口数据接收事件
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string received = serialPort.ReadExisting();
                AppendReceiveData(received, Color.Black);
            }
            catch (Exception ex)
            {
                AppendLog($"接收数据错误: {ex.Message}");
            }
        }

        // 串口错误事件
        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            AppendLog($"串口错误: {e.EventType}");
        }

        // 串口引脚状态变化事件
        private void SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            AppendLog($"引脚状态变化: {e.EventType}");
        }

        // 状态监控定时器
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    // 更新CTS状态
                    bool ctsHolding = serialPort.CtsHolding;
                    labelCtsStatus.Text = $"CTS: {(ctsHolding ? "ON" : "OFF")}";
                    labelCtsStatus.ForeColor = ctsHolding ? Color.Green : Color.Red;

                    // 更新CD状态
                    bool cdHolding = serialPort.CDHolding;
                    labelCdStatus.Text = $"CD: {(cdHolding ? "ON" : "OFF")}";
                    labelCdStatus.ForeColor = cdHolding ? Color.Green : Color.Red;
                }
                catch (Exception ex)
                {
                    AppendLog($"状态监控错误: {ex.Message}");
                }
            }
        }

        // 添加接收数据到显示框
        private void AppendReceiveData(string data, Color color)
        {
            if (textBoxReceive.InvokeRequired)
            {
                textBoxReceive.Invoke(new Action(() => AppendReceiveData(data, color)));
            }
            else
            {
                textBoxReceive.SelectionStart = textBoxReceive.TextLength;
                textBoxReceive.SelectionLength = 0;
                textBoxReceive.SelectionColor = color;
                textBoxReceive.AppendText(data);
                textBoxReceive.ScrollToCaret();
            }
        }

        // 添加日志信息
        private void AppendLog(string message)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new Action(() => AppendLog(message)));
            }
            else
            {
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";
                textBoxLog.AppendText(logEntry);
                textBoxLog.ScrollToCaret();
            }
        }

        // 窗体关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            statusTimer.Stop();
            statusTimer.Dispose();
        }

        // 发送框回车事件
        private void textBoxSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                buttonSend_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
