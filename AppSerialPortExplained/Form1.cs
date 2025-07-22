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
                    // ���ô��ڲ���
                    serialPort.PortName = comboBoxPort.Text;
                    serialPort.BaudRate = int.Parse(comboBoxBaud.Text);

                    // ����У��λ
                    switch (comboBoxParity.Text)
                    {
                        case "None": serialPort.Parity = Parity.None; break;
                        case "Even": serialPort.Parity = Parity.Even; break;
                        case "Odd": serialPort.Parity = Parity.Odd; break;
                        case "Mark": serialPort.Parity = Parity.Mark; break;
                        case "Space": serialPort.Parity = Parity.Space; break;
                    }

                    serialPort.DataBits = int.Parse(comboBoxDataBits.Text);

                    // ����ֹͣλ
                    switch (comboBoxStopBits.Text)
                    {
                        case "One": serialPort.StopBits = StopBits.One; break;
                        case "Two": serialPort.StopBits = StopBits.Two; break;
                        case "OnePointFive": serialPort.StopBits = StopBits.OnePointFive; break;
                    }

                    // ����������
                    switch (comboBoxHandshake.Text)
                    {
                        case "None": serialPort.Handshake = Handshake.None; break;
                        case "XOnXOff": serialPort.Handshake = Handshake.XOnXOff; break;
                        case "RequestToSend": serialPort.Handshake = Handshake.RequestToSend; break;
                        case "RequestToSendXOnXOff": serialPort.Handshake = Handshake.RequestToSendXOnXOff; break;
                    }

                    // ���ó�ʱʱ��
                    serialPort.ReadTimeout = (int)numericUpDownReadTimeout.Value;
                    serialPort.WriteTimeout = (int)numericUpDownWriteTimeout.Value;

                    // �򿪴���
                    serialPort.Open();

                    // ����UI״̬
                    buttonOpen.Text = "�رմ���";
                    buttonOpen.BackColor = Color.Red;
                    groupBoxConfig.Enabled = false;
                    groupBoxSend.Enabled = true;

                    // ��ʼ״̬���
                    statusTimer.Start();

                    AppendLog("�����Ѵ�");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�򿪴���ʧ��: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    serialPort.Close();

                    // ����UI״̬
                    buttonOpen.Text = "�򿪴���";
                    buttonOpen.BackColor = Color.Green;
                    groupBoxConfig.Enabled = true;
                    groupBoxSend.Enabled = false;

                    // ֹͣ״̬���
                    statusTimer.Stop();

                    // ���״̬��ʾ
                    labelCtsStatus.Text = "CTS: -";
                    labelCdStatus.Text = "CD: -";
                    labelCtsStatus.ForeColor = Color.Gray;
                    labelCdStatus.ForeColor = Color.Gray;

                    AppendLog("�����ѹر�");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�رմ���ʧ��: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    // ���ݷ���ģʽ��ӽ�����
                    if (checkBoxNewLine.Checked)
                        dataToSend += "\r\n";

                    serialPort.Write(dataToSend);

                    // ��ʾ���͵�����
                    if (checkBoxShowSent.Checked)
                    {
                        AppendReceiveData($"[����] {dataToSend}", Color.Blue);
                    }

                    // ��շ��Ϳ����ѡ�����Զ���գ�
                    if (checkBoxAutoClear.Checked)
                        textBoxSend.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("����ʧ��: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppendLog($"���ʹ���: {ex.Message}");
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

        // �������ݽ����¼�
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string received = serialPort.ReadExisting();
                AppendReceiveData(received, Color.Black);
            }
            catch (Exception ex)
            {
                AppendLog($"�������ݴ���: {ex.Message}");
            }
        }

        // ���ڴ����¼�
        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            AppendLog($"���ڴ���: {e.EventType}");
        }

        // ��������״̬�仯�¼�
        private void SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            AppendLog($"����״̬�仯: {e.EventType}");
        }

        // ״̬��ض�ʱ��
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    // ����CTS״̬
                    bool ctsHolding = serialPort.CtsHolding;
                    labelCtsStatus.Text = $"CTS: {(ctsHolding ? "ON" : "OFF")}";
                    labelCtsStatus.ForeColor = ctsHolding ? Color.Green : Color.Red;

                    // ����CD״̬
                    bool cdHolding = serialPort.CDHolding;
                    labelCdStatus.Text = $"CD: {(cdHolding ? "ON" : "OFF")}";
                    labelCdStatus.ForeColor = cdHolding ? Color.Green : Color.Red;
                }
                catch (Exception ex)
                {
                    AppendLog($"״̬��ش���: {ex.Message}");
                }
            }
        }

        // ��ӽ������ݵ���ʾ��
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

        // �����־��Ϣ
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

        // ����ر��¼�
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            statusTimer.Stop();
            statusTimer.Dispose();
        }

        // ���Ϳ�س��¼�
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
