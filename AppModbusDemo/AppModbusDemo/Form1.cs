using System.IO.Ports;
using System.Net.Sockets;
using Modbus.Device;
using static AppModbusDemo.DataTypeHelper;

namespace AppModbusDemo
{
    public partial class Form1 : Form
    {
        private ModbusMaster master;
        private TcpClient tcpClient;
        private SerialPort serialPort;
        private bool isConnected = false;

        public bool IsConnected => isConnected;
        public ModbusMaster GetModbusMaster() => master;
        public Form1()
        {
            InitializeComponent();
            InitializeSerialPortComboBox();
            InitializeDataTypes();
            InitializeByteOrders();
        }

        private void InitializeDataTypes()
        {
            cmbDataType.Items.Clear();
            foreach (DataType dataType in Enum.GetValues<DataType>())
            {
                cmbDataType.Items.Add(dataType);
            }
            cmbDataType.SelectedIndex = 0;
        }

        private void InitializeByteOrders()
        {
            cmbByteOrder.Items.Clear();
            foreach (ByteOrder byteOrder in Enum.GetValues<ByteOrder>())
            {
                cmbByteOrder.Items.Add(byteOrder);
            }
            cmbByteOrder.SelectedIndex = 0;
        }

        private void InitializeSerialPortComboBox()
        {
            cmbSerialPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            cmbSerialPort.Items.AddRange(ports);
            if (ports.Length > 0)
                cmbSerialPort.SelectedIndex = 0;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                await ConnectModbus();
            }
            else
            {
                DisconnectModbus();
            }
        }

        private async Task ConnectModbus()
        {
            try
            {
                if (rbTCP.Checked)
                {
                    await ConnectTCP();
                }
                else
                {
                    ConnectRTU();
                }

                // ���ó�ʱʱ��
                if (master != null)
                {
                    master.Transport.ReadTimeout = (int)nudReadTimeout.Value;
                    master.Transport.WriteTimeout = (int)nudWriteTimeout.Value;
                    master.Transport.Retries = (int)nudRetries.Value;
                }

                isConnected = true;
                btnConnect.Text = "�Ͽ�����";
                btnConnect.BackColor = Color.Red;
                pnlRTU.Enabled = false;
                pnlTCP.Enabled = false;
                gbOperations.Enabled = true;
                gbWrite.Enabled = true;
                gbDataType.Enabled = true;
                rbRTU.Enabled = false;
                rbTCP.Enabled = false;
                LogMessage("���ӳɹ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ʧ��: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"����ʧ��: {ex.Message}");
            }
        }

        private async Task ConnectTCP()
        {
            string ip = txtIP.Text.Trim();
            int port = (int)nudPort.Value;

            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);
            master = ModbusIpMaster.CreateIp(tcpClient);
        }

        private void ConnectRTU()
        {
            serialPort = new SerialPort()
            {
                PortName = cmbSerialPort.Text,
                BaudRate = (int)nudBaudRate.Value,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                ReadTimeout = (int)nudReadTimeout.Value,
                WriteTimeout = (int)nudWriteTimeout.Value
            };

            serialPort.Open();
            master = ModbusSerialMaster.CreateRtu(serialPort);
        }

        private void DisconnectModbus()
        {
            try
            {
                master?.Dispose();
                tcpClient?.Close();
                serialPort?.Close();

                isConnected = false;
                btnConnect.Text = "����";
                btnConnect.BackColor = Color.Green;
                pnlRTU.Enabled = true;
                pnlTCP.Enabled = true;
                rbRTU.Enabled = true;
                rbTCP.Enabled = true;
                gbOperations.Enabled = false;
                gbWrite.Enabled = false;
                gbDataType.Enabled = false; // �����������Ͳ���
                LogMessage("�����ѶϿ�");
            }
            catch (Exception ex)
            {
                LogMessage($"�Ͽ�����ʱ����: {ex.Message}");
            }
        }

        // ԭ�еĶ�д�������ֲ���...
        private async void btnReadCoils_Click(object sender, EventArgs e)
        {
            await ReadCoils();
        }

        private async Task ReadCoils()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort startAddress = (ushort)nudStartAddress.Value;
                ushort numberOfPoints = (ushort)nudNumberOfPoints.Value;

                LogMessage($"��ʼ��ȡ��Ȧ - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}");

                bool[] coils = await master.ReadCoilsAsync(slaveId, startAddress, numberOfPoints);


                txtResult.Clear();
                txtResult.AppendText($"��ȡ��Ȧ - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}\r\n");
                txtResult.AppendText("���:\r\n");

                for (int i = 0; i < coils.Length; i++)
                {
                    txtResult.AppendText($"��ַ {startAddress + i}: {coils[i]}\r\n");
                }

                LogMessage($"�ɹ���ȡ {coils.Length} ����Ȧ");
            }
            catch (Exception ex)
            {
                string errorMessage = $"��ȡ��Ȧʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnReadInputs_Click(object sender, EventArgs e)
        {
            await ReadDiscreteInputs();
        }

        private async Task ReadDiscreteInputs()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort startAddress = (ushort)nudStartAddress.Value;
                ushort numberOfPoints = (ushort)nudNumberOfPoints.Value;
                int maxRetries = (int)nudRetries.Value;

                LogMessage($"��ʼ��ȡ��ɢ���� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}");

                bool[] inputs = null;

                for (int retry = 0; retry <= maxRetries; retry++)
                {
                    try
                    {
                        LogMessage($"���Զ�ȡ��ɢ���룬�� {retry + 1} ��");

                        // RTUģʽ������ʱ����ӳ�
                        if (rbRTU.Checked && retry > 0)
                        {
                            await Task.Delay(100);
                        }

                        inputs = master.ReadInputs(slaveId, startAddress, numberOfPoints);
                        LogMessage($"�ɹ���ȡ��ɢ���룬���� {inputs.Length} ��ֵ");
                        break;
                    }
                    catch (System.IO.IOException ioEx) when (ioEx.Message.Contains("Unexpected byte count"))
                    {
                        LogMessage($"�ֽڼ������� (���� {retry + 1}/{maxRetries + 1}): {ioEx.Message}");

                        if (retry == maxRetries)
                        {
                            throw new Exception($"��ȡ��ɢ����ʧ�ܣ������� {maxRetries + 1} �Ρ�����ԭ��\r\n" +
                                "1. ��վ�豸��֧����ɢ���빦����(0x02)\r\n" +
                                "2. ָ���ĵ�ַ��Χ��Ч\r\n" +
                                "3. ��վID����ȷ\r\n" +
                                "4. ͨ�Ų������ô���\r\n" +
                                $"ԭʼ����: {ioEx.Message}", ioEx);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"��ȡ��ɢ�����쳣���� (���� {retry + 1}): {ex.GetType().Name} - {ex.Message}");

                        if (retry == maxRetries)
                            throw;
                    }
                }

                // ����Ƿ�ɹ���ȡ������
                if (inputs == null)
                {
                    throw new Exception("��ȡ��ɢ����ʧ�ܣ�δ�ܻ�ȡ����Ч����");
                }

                txtResult.Clear();
                txtResult.AppendText($"��ȡ��ɢ���� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}\r\n");
                txtResult.AppendText("���:\r\n");

                for (int i = 0; i < inputs.Length; i++)
                {
                    txtResult.AppendText($"��ַ {startAddress + i}: {inputs[i]}\r\n");
                }

                LogMessage($"�ɹ���ȡ {inputs.Length} ����ɢ����");
            }
            catch (Exception ex)
            {
                string errorMessage = $"��ȡ��ɢ����ʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnReadHoldingRegisters_Click(object sender, EventArgs e)
        {
            await ReadHoldingRegisters();
        }

        private async Task ReadHoldingRegisters()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort startAddress = (ushort)nudStartAddress.Value;
                ushort numberOfPoints = (ushort)nudNumberOfPoints.Value;

                LogMessage($"��ʼ��ȡ���ּĴ��� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}");

                ushort[] registers =  await master.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfPoints);

                txtResult.Clear();
                txtResult.AppendText($"��ȡ���ּĴ��� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}\r\n");
                txtResult.AppendText("���:\r\n");

                for (int i = 0; i < registers.Length; i++)
                {
                    txtResult.AppendText($"��ַ {startAddress + i}: {registers[i]} (0x{registers[i]:X4})\r\n");
                }

                LogMessage($"�ɹ���ȡ {registers.Length} �����ּĴ���");
            }
            catch (Exception ex)
            {
                string errorMessage = $"��ȡ���ּĴ���ʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnReadInputRegisters_Click(object sender, EventArgs e)
        {
            await ReadInputRegisters();
        }

        private async Task ReadInputRegisters()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort startAddress = (ushort)nudStartAddress.Value;
                ushort numberOfPoints = (ushort)nudNumberOfPoints.Value;

                LogMessage($"��ʼ��ȡ����Ĵ��� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}");

                ushort[] registers =await master.ReadInputRegistersAsync(slaveId, startAddress, numberOfPoints);

                txtResult.Clear();
                txtResult.AppendText($"��ȡ����Ĵ��� - ��վID: {slaveId}, ��ʼ��ַ: {startAddress}, ����: {numberOfPoints}\r\n");
                txtResult.AppendText("���:\r\n");

                for (int i = 0; i < registers.Length; i++)
                {
                    txtResult.AppendText($"��ַ {startAddress + i}: {registers[i]} (0x{registers[i]:X4})\r\n");
                }

                LogMessage($"�ɹ���ȡ {registers.Length} ������Ĵ���");
            }
            catch (Exception ex)
            {
                string errorMessage = $"��ȡ����Ĵ���ʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnWriteSingleCoil_Click(object sender, EventArgs e)
        {
            await WriteSingleCoil();
        }

        private async Task WriteSingleCoil()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort address = (ushort)nudWriteAddress.Value;
                bool value = cbCoilValue.Checked;

                LogMessage($"��ʼд�뵥����Ȧ - ��վID: {slaveId}, ��ַ: {address}, ֵ: {value}");

                await master.WriteSingleCoilAsync(slaveId, address, value);

                txtResult.Clear();
                txtResult.AppendText($"д�뵥����Ȧ - ��վID: {slaveId}, ��ַ: {address}, ֵ: {value}\r\n");
                txtResult.AppendText("д��ɹ�\r\n");

                LogMessage($"�ɹ�д�뵥����Ȧ: ��ַ {address} = {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"д�뵥����Ȧʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnWriteSingleRegister_Click(object sender, EventArgs e)
        {
            await WriteSingleRegister();
        }

        private async Task WriteSingleRegister()
        {
            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                ushort address = (ushort)nudWriteAddress.Value;
                ushort value = (ushort)nudRegisterValue.Value;

                LogMessage($"��ʼд�뵥���Ĵ��� - ��վID: {slaveId}, ��ַ: {address}, ֵ: {value}");

                await master.WriteSingleRegisterAsync(slaveId, address, value);

                txtResult.Clear();
                txtResult.AppendText($"д�뵥���Ĵ��� - ��վID: {slaveId}, ��ַ: {address}, ֵ: {value}\r\n");
                txtResult.AppendText("д��ɹ�\r\n");

                LogMessage($"�ɹ�д�뵥���Ĵ���: ��ַ {address} = {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"д�뵥���Ĵ���ʧ��: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n�ڲ��쳣: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        // ����������������ط���
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDataType.SelectedItem is DataType dataType)
            {
                int registerCount = GetRegisterCount(dataType);
                lblRegisterCount.Text = $"�Ĵ�������: {registerCount}";
                lblDataTypeDescription.Text = GetDataTypeDescription(dataType);

                // �����������͵��������Ŀɼ��Ժͷ�Χ
                AdjustInputControls(dataType);
            }
        }

        private void AdjustInputControls(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Float:
                case DataType.Double:
                    nudDataTypeIntValue.Visible = false;
                    txtDataTypeFloatValue.Visible = true;
                    lblDataTypeValue.Text = "����ֵ:";
                    txtDataTypeFloatValue.Text = "0.0";
                    break;

                case DataType.Int64:
                    nudDataTypeIntValue.Visible = false;
                    txtDataTypeFloatValue.Visible = true;
                    lblDataTypeValue.Text = "64λ����ֵ:";
                    txtDataTypeFloatValue.Text = "0";
                    break;

                case DataType.UInt64:
                    nudDataTypeIntValue.Visible = false;
                    txtDataTypeFloatValue.Visible = true;
                    lblDataTypeValue.Text = "�޷���64λ����ֵ:";
                    txtDataTypeFloatValue.Text = "0";
                    break;

                default:
                    nudDataTypeIntValue.Visible = true;
                    txtDataTypeFloatValue.Visible = false;
                    lblDataTypeValue.Text = "����ֵ:";

                    // �����������뷶Χ
                    switch (dataType)
                    {
                        case DataType.UInt16:
                            nudDataTypeIntValue.Minimum = 0;
                            nudDataTypeIntValue.Maximum = 65535;
                            break;
                        case DataType.Int16:
                            nudDataTypeIntValue.Minimum = -32768;
                            nudDataTypeIntValue.Maximum = 32767;
                            break;
                        case DataType.UInt32:
                            nudDataTypeIntValue.Minimum = 0;
                            nudDataTypeIntValue.Maximum = 4294967295;
                            break;
                        case DataType.Int32:
                            nudDataTypeIntValue.Minimum = -2147483648;
                            nudDataTypeIntValue.Maximum = 2147483647;
                            break;
                    }
                    nudDataTypeIntValue.Value = 0;
                    break;
            }
        }

        private async void btnReadDataType_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("���������豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataType dataType = (DataType)cmbDataType.SelectedItem;
                ByteOrder byteOrder = (ByteOrder)cmbByteOrder.SelectedItem;
                byte slaveId = (byte)nudDataTypeSlaveId.Value;
                ushort startAddress = (ushort)nudDataTypeAddress.Value;
                int registerCount = GetRegisterCount(dataType);

                LogMessage($"��ʼ��ȡ {dataType} ���� - ��վID: {slaveId}, ��ַ: {startAddress}, �ֽ���: {byteOrder}");

                ushort[] registers = await
                    master.ReadHoldingRegistersAsync(slaveId, startAddress, (ushort)registerCount);

                object value = ConvertFromRegisters(registers, dataType, byteOrder);

                txtResult.Clear();
                txtResult.AppendText($"��ȡ {dataType} ���ݳɹ�:\r\n");
                txtResult.AppendText($"��վID: {slaveId}\r\n");
                txtResult.AppendText($"��ʼ��ַ: {startAddress}\r\n");
                txtResult.AppendText($"��������: {dataType}\r\n");
                txtResult.AppendText($"�ֽ���: {byteOrder}\r\n");
                txtResult.AppendText($"�Ĵ�������: {registerCount}\r\n");
                txtResult.AppendText($"ԭʼ�Ĵ���ֵ: [{string.Join(", ", registers)}]\r\n");
                txtResult.AppendText($"ת�����ֵ: {value}\r\n");

                // ��������������ʾ������Ϣ
                if (dataType == DataType.Float || dataType == DataType.Double)
                {
                    txtResult.AppendText($"��ѧ������: {Convert.ToDouble(value):E}\r\n");
                }
                else if (dataType != DataType.UInt16 && dataType != DataType.Int16)
                {
                    txtResult.AppendText($"ʮ������: 0x{Convert.ToUInt64(value):X}\r\n");
                }

                LogMessage($"�ɹ���ȡ {dataType} ����: {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"��ȡ {cmbDataType.SelectedItem} ����ʧ��: {ex.Message}";
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnWriteDataType_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("���������豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataType dataType = (DataType)cmbDataType.SelectedItem;
                ByteOrder byteOrder = (ByteOrder)cmbByteOrder.SelectedItem;
                byte slaveId = (byte)nudDataTypeSlaveId.Value;
                ushort startAddress = (ushort)nudDataTypeAddress.Value;

                // ��ȡҪд���ֵ
                object valueToWrite = GetValueToWrite(dataType);
                if (valueToWrite == null) return;

                LogMessage($"��ʼд�� {dataType} ���� - ��վID: {slaveId}, ��ַ: {startAddress}, ֵ: {valueToWrite}, �ֽ���: {byteOrder}");

                // ת��Ϊ�Ĵ�������
                ushort[] registers = ConvertToRegisters(valueToWrite, dataType, byteOrder);

                // д��Ĵ���

                if (registers.Length == 1)
                {
                    await master.WriteSingleRegisterAsync(slaveId, startAddress, registers[0]);
                }
                else
                {
                    await master.WriteMultipleRegistersAsync(slaveId, startAddress, registers);
                }


                txtResult.Clear();
                txtResult.AppendText($"д�� {dataType} ���ݳɹ�:\r\n");
                txtResult.AppendText($"��վID: {slaveId}\r\n");
                txtResult.AppendText($"��ʼ��ַ: {startAddress}\r\n");
                txtResult.AppendText($"��������: {dataType}\r\n");
                txtResult.AppendText($"�ֽ���: {byteOrder}\r\n");
                txtResult.AppendText($"д��ֵ: {valueToWrite}\r\n");
                txtResult.AppendText($"�Ĵ���ֵ: [{string.Join(", ", registers)}]\r\n");

                LogMessage($"�ɹ�д�� {dataType} ����: {valueToWrite}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"д�� {cmbDataType.SelectedItem} ����ʧ��: {ex.Message}";
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private object GetValueToWrite(DataType dataType)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.UInt16:
                        return (ushort)nudDataTypeIntValue.Value;
                    case DataType.Int16:
                        return (short)nudDataTypeIntValue.Value;
                    case DataType.UInt32:
                        return (uint)nudDataTypeIntValue.Value;
                    case DataType.Int32:
                        return (int)nudDataTypeIntValue.Value;
                    case DataType.Float:
                        if (float.TryParse(txtDataTypeFloatValue.Text, out float floatValue))
                            return floatValue;
                        throw new ArgumentException("��Ч�ĸ�������ʽ");
                    case DataType.Double:
                        if (double.TryParse(txtDataTypeFloatValue.Text, out double doubleValue))
                            return doubleValue;
                        throw new ArgumentException("��Ч��˫���ȸ�������ʽ");
                    case DataType.Int64:
                        if (long.TryParse(txtDataTypeFloatValue.Text, out long longValue))
                            return longValue;
                        throw new ArgumentException("��Ч��64λ������ʽ");
                    case DataType.UInt64:
                        if (ulong.TryParse(txtDataTypeFloatValue.Text, out ulong ulongValue))
                            return ulongValue;
                        throw new ArgumentException("��Ч���޷���64λ������ʽ");
                    default:
                        return (ushort)nudDataTypeIntValue.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ֵ��ʽ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void btnTestConversion_Click(object sender, EventArgs e)
        {
            try
            {
                DataType dataType = (DataType)cmbDataType.SelectedItem;
                ByteOrder byteOrder = (ByteOrder)cmbByteOrder.SelectedItem;

                object valueToTest = GetValueToWrite(dataType);
                if (valueToTest == null) return;

                // ת��Ϊ�Ĵ���
                ushort[] registers = ConvertToRegisters(valueToTest, dataType, byteOrder);

                // ��ת������
                object convertedBack = ConvertFromRegisters(registers, dataType, byteOrder);

                txtResult.Clear();
                txtResult.AppendText("����ת������:\r\n");
                txtResult.AppendText($"��������: {dataType}\r\n");
                txtResult.AppendText($"�ֽ���: {byteOrder}\r\n");
                txtResult.AppendText($"ԭʼֵ: {valueToTest}\r\n");
                txtResult.AppendText($"�Ĵ���ֵ: [{string.Join(", ", registers)}]\r\n");
                txtResult.AppendText($"ת���ص�ֵ: {convertedBack}\r\n");
                txtResult.AppendText($"ת���Ƿ�һ��: {valueToTest.Equals(convertedBack)}\r\n");

                // ��ʾ�ֽڼ������Ϣ
                byte[] bytes = new byte[registers.Length * 2];
                for (int i = 0; i < registers.Length; i++)
                {
                    byte[] regBytes = BitConverter.GetBytes(registers[i]);
                    bytes[i * 2] = regBytes[0];
                    bytes[i * 2 + 1] = regBytes[1];
                }
                txtResult.AppendText($"�ֽ�����: [{string.Join(", ", Array.ConvertAll(bytes, b => $"0x{b:X2}"))}]\r\n");

                LogMessage($"ת���������: {valueToTest} -> [{string.Join(", ", registers)}] -> {convertedBack}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"ת������ʧ��: {ex.Message}";
                MessageBox.Show(errorMessage, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("���������豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                LogMessage($"��ʼ�������� - ��վID: {slaveId}");

                await master.ReadCoilsAsync(slaveId, 0, 1);

                MessageBox.Show("���Ӳ��Գɹ���", "���Խ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMessage("���Ӳ��Գɹ�");
            }
            catch (Exception ex)
            {
                string errorMessage = $"���Ӳ���ʧ��: {ex.GetType().Name} - {ex.Message}";
                MessageBox.Show(errorMessage, "���Խ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            InitializeSerialPortComboBox();
        }

        private void rbTCP_CheckedChanged(object sender, EventArgs e)
        {
            pnlTCP.Visible = rbTCP.Checked;
            pnlRTU.Visible = !rbTCP.Checked;
        }

        private void rbRTU_CheckedChanged(object sender, EventArgs e)
        {
            pnlTCP.Visible = !rbRTU.Checked;
            pnlRTU.Visible = rbRTU.Checked;
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => LogMessage(message)));
                return;
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {message}\r\n");
            txtLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isConnected)
            {
                DisconnectModbus();
            }
            base.OnFormClosing(e);
        }
    }
}
