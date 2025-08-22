using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Text;

namespace AppSerialAsync
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private readonly ConcurrentQueue<byte[]> dataQueue = new();
        private readonly object lockObject = new();
        private byte[] receiveBuffer = new byte[4096];
        private int bufferPosition = 0;
        private bool isReading = false;

        public enum ParseResult
        {
            Complete,
            Partial,
            Error
        }
        public static class Packet
        {
            public const byte Soh = 0x01; 
        }

        public Form1()
        {
            InitializeComponent();
            InitializeSerialPort();
            LoadAvailablePorts();
        }

        #region Serial Port Initialization

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort
            {
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReceivedBytesThreshold = 1,
                ReadTimeout = 500,
                WriteTimeout = 500,
                ReadBufferSize = 4096,
                WriteBufferSize = 2048
            };
        }

        private void LoadAvailablePorts()
        {
            cmbPorts.Items.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                cmbPorts.Items.Add(port);
            }
            if (cmbPorts.Items.Count > 0)
                cmbPorts.SelectedIndex = 0;
        }

        #endregion

        #region Connection Handling

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    if (cmbPorts.SelectedItem == null)
                    {
                        MessageBox.Show("请选择串口端口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    serialPort.PortName = cmbPorts.SelectedItem.ToString();
                    serialPort.BaudRate = int.Parse(cmbBaudRate.Text);

                    serialPort.Open();
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();

                    await StartAsyncReading();

                    btnConnect.Text = "断开连接";
                    btnConnect.BackColor = System.Drawing.Color.Red;
                    UpdateConnectionStatus("已连接", System.Drawing.Color.Green);
                    LogMessage($"串口 {serialPort.PortName} 连接成功，波特率: {serialPort.BaudRate}");
                }
                else
                {
                    StopAsyncReading();
                    serialPort.Close();

                    btnConnect.Text = "连接串口";
                    btnConnect.BackColor = System.Drawing.SystemColors.Control;
                    UpdateConnectionStatus("未连接", System.Drawing.Color.Red);
                    LogMessage("串口连接已断开");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"串口操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"错误: {ex.Message}");
            }
        }

        #endregion

        #region Async Reading

        private async Task StartAsyncReading()
        {
            if (isReading) return;
            isReading = true;
            bufferPosition = 0;
            await Task.Run(BeginAsyncRead);
        }

        private void BeginAsyncRead()
        {
            if (!serialPort.IsOpen || !isReading) return;

            try
            {
                byte[] buffer = new byte[1024];
                serialPort.BaseStream.BeginRead(buffer, 0, buffer.Length, ar =>
                {
                    try
                    {
                        int bytesRead = serialPort.BaseStream.EndRead(ar);
                        if (bytesRead > 0)
                        {
                            byte[] receivedData = new byte[bytesRead];
                            Array.Copy(buffer, 0, receivedData, 0, bytesRead);
                            ProcessReceivedData(receivedData);
                        }
                        if (isReading && serialPort.IsOpen)
                        {
                            BeginAsyncRead();
                        }
                    }
                    catch (IOException ioEx)
                    {
                        BeginInvoke(new Action(() => LogMessage($"IO异常: {ioEx.Message}")));
                    }
                    catch (Exception ex)
                    {
                        BeginInvoke(new Action(() => LogMessage($"读取异常: {ex.Message}")));
                    }
                }, null);
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() => LogMessage($"开始异步读取失败: {ex.Message}")));
            }
        }

        private void StopAsyncReading()
        {
            isReading = false;
        }

        #endregion

        #region Data Processing

        private void ProcessReceivedData(byte[] newData)
        {
            lock (lockObject)
            {
                if (bufferPosition + newData.Length > receiveBuffer.Length)
                {
                    Array.Resize(ref receiveBuffer, receiveBuffer.Length * 2);
                }
                Array.Copy(newData, 0, receiveBuffer, bufferPosition, newData.Length);
                bufferPosition += newData.Length;

                ParseDataPackets();

                BeginInvoke(new Action(() =>
                {
                    string hexData = BitConverter.ToString(newData).Replace("-", " ");
                    LogMessage($"接收原始数据: {hexData}");
                    lblRxCount.Text = $"接收: {int.Parse(lblRxCount.Text.Split(':')[1].Trim()) + newData.Length}";
                }));

                BeginInvoke(new Action(() =>
                {
                    string hexBuffer = BitConverter.ToString(receiveBuffer, 0, bufferPosition).Replace("-", " ");
                    LogMessage($"当前缓冲区: {hexBuffer} 长度: {bufferPosition}");
                }));
            }
        }

        private void ParseDataPackets()
        {
            while (bufferPosition > 0)
            {
                ReadOnlySpan<byte> spanData = receiveBuffer.AsSpan(0, bufferPosition);
                var result = TryParsePacket(spanData, out int consumedBytes, out string command, out string data);

                if (result == ParseResult.Complete)
                {
                    BeginInvoke(new Action(() =>
                    {
                        LogMessage($"解析数据包 - 命令: {command}, 数据: {data}");
                        txtReceived.AppendText($"[{DateTime.Now:HH:mm:ss}] 命令: {command}, 数据: {data}\r\n");
                        txtReceived.ScrollToCaret();
                    }));

                    Array.Copy(receiveBuffer, consumedBytes, receiveBuffer, 0, bufferPosition - consumedBytes);
                    bufferPosition -= consumedBytes;
                }
                else if (result == ParseResult.Partial)
                {
                    break;
                }
                else
                {
                    Array.Copy(receiveBuffer, 1, receiveBuffer, 0, bufferPosition - 1);
                    bufferPosition--;
                }
            }
        }

        private ParseResult TryParsePacket(ReadOnlySpan<byte> buffer, out int consumedBytes, out string command, out string data)
        {
            consumedBytes = 0;
            command = string.Empty;
            data = string.Empty;

            int indexOfSoh = buffer.IndexOf(Packet.Soh);
            if (indexOfSoh == -1)
            {
                consumedBytes = buffer.Length;
                return ParseResult.Error;
            }
            if (indexOfSoh > 0)
            {
                consumedBytes = indexOfSoh;
                return ParseResult.Error;
            }

            buffer = buffer.Slice(1);

            if (buffer.Length < sizeof(int) * 2)
                return ParseResult.Partial;

            int commandLength = BinaryPrimitives.ReadInt32LittleEndian(buffer);
            int dataLength = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(sizeof(int)));

            buffer = buffer.Slice(sizeof(int) * 2);

            if (buffer.Length < commandLength + dataLength)
                return ParseResult.Partial;

            try
            {
                command = Encoding.UTF8.GetString(buffer.Slice(0, commandLength));
                data = Encoding.UTF8.GetString(buffer.Slice(commandLength, dataLength));
                consumedBytes = 1 + sizeof(int) * 2 + commandLength + dataLength;
                return ParseResult.Complete;
            }
            catch
            {
                consumedBytes = 1;
                return ParseResult.Error;
            }
        }

        #endregion

        #region Data Sending

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先连接串口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string dataToSend = txtSend.Text;

                if (rbSendText.Checked)
                {
                    await SendTextData(dataToSend);
                }
                else if (rbSendPacket.Checked)
                {
                    await SendDataPacket("CMD", dataToSend);
                }
                else
                {
                    await SendHexData(dataToSend);
                }

                byte[] sentData = Encoding.UTF8.GetBytes(dataToSend);
                lblTxCount.Text = $"发送: {int.Parse(lblTxCount.Text.Split(':')[1].Trim()) + sentData.Length}";
                LogMessage($"发送数据: {dataToSend}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"发送错误: {ex.Message}");
            }
        }

        private async Task SendTextData(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            await serialPort.BaseStream.WriteAsync(data, 0, data.Length);
        }

        private async Task SendDataPacket(string command, string data)
        {
            byte[] commandBytes = Encoding.UTF8.GetBytes(command);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] packet = new byte[1 + sizeof(int) * 2 + commandBytes.Length + dataBytes.Length];
            int offset = 0;

            packet[offset++] = Packet.Soh;
            BinaryPrimitives.WriteInt32LittleEndian(packet.AsSpan(offset), commandBytes.Length);
            offset += sizeof(int);
            BinaryPrimitives.WriteInt32LittleEndian(packet.AsSpan(offset), dataBytes.Length);
            offset += sizeof(int);

            Array.Copy(commandBytes, 0, packet, offset, commandBytes.Length);
            offset += commandBytes.Length;
            Array.Copy(dataBytes, 0, packet, offset, dataBytes.Length);

            await serialPort.BaseStream.WriteAsync(packet, 0, packet.Length);
        }

        private async Task SendHexData(string hexString)
        {
            try
            {
                hexString = hexString.Replace(" ", "").Replace("-", "");
                byte[] data = new byte[hexString.Length / 2];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                await serialPort.BaseStream.WriteAsync(data, 0, data.Length);
            }
            catch (FormatException)
            {
                throw new ArgumentException("无效的十六进制格式");
            }
        }

        #endregion

        #region UI & Logging

        private void btnClearReceived_Click(object sender, EventArgs e) => txtReceived.Clear();

        private void btnClearLog_Click(object sender, EventArgs e) => txtLog.Clear();

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            LoadAvailablePorts();
            LogMessage("端口列表已刷新");
        }

        private void UpdateConnectionStatus(string status, System.Drawing.Color color)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateConnectionStatus(status, color)));
                return;
            }
            lblStatus.Text = $"状态: {status}";
            lblStatus.ForeColor = color;
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => LogMessage(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\r\n");
            txtLog.ScrollToCaret();
        }

        #endregion

        #region Utility


        public static ushort CalculateCRC(byte[] data, int offset, int length)
        {
            ushort crc = 0xFFFF;
            for (int i = offset; i < offset + length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    crc = (ushort)((crc & 0x0001) != 0 ? (crc >> 1) ^ 0xA001 : crc >> 1);
                }
            }
            return crc;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                StopAsyncReading();
                serialPort.Close();
            }
            base.OnFormClosing(e);
        }

        #endregion
    }
}
