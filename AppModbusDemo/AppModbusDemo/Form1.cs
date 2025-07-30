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

                // 配置超时时间
                if (master != null)
                {
                    master.Transport.ReadTimeout = (int)nudReadTimeout.Value;
                    master.Transport.WriteTimeout = (int)nudWriteTimeout.Value;
                    master.Transport.Retries = (int)nudRetries.Value;
                }

                isConnected = true;
                btnConnect.Text = "断开连接";
                btnConnect.BackColor = Color.Red;
                pnlRTU.Enabled = false;
                pnlTCP.Enabled = false;
                gbOperations.Enabled = true;
                gbWrite.Enabled = true;
                gbDataType.Enabled = true;
                rbRTU.Enabled = false;
                rbTCP.Enabled = false;
                LogMessage("连接成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"连接失败: {ex.Message}");
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
                btnConnect.Text = "连接";
                btnConnect.BackColor = Color.Green;
                pnlRTU.Enabled = true;
                pnlTCP.Enabled = true;
                rbRTU.Enabled = true;
                rbTCP.Enabled = true;
                gbOperations.Enabled = false;
                gbWrite.Enabled = false;
                gbDataType.Enabled = false; // 禁用数据类型操作
                LogMessage("连接已断开");
            }
            catch (Exception ex)
            {
                LogMessage($"断开连接时出错: {ex.Message}");
            }
        }

        // 原有的读写方法保持不变...
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

                LogMessage($"开始读取线圈 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}");

                bool[] coils = await master.ReadCoilsAsync(slaveId, startAddress, numberOfPoints);


                txtResult.Clear();
                txtResult.AppendText($"读取线圈 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}\r\n");
                txtResult.AppendText("结果:\r\n");

                for (int i = 0; i < coils.Length; i++)
                {
                    txtResult.AppendText($"地址 {startAddress + i}: {coils[i]}\r\n");
                }

                LogMessage($"成功读取 {coils.Length} 个线圈");
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取线圈失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                LogMessage($"开始读取离散输入 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}");

                bool[] inputs = null;

                for (int retry = 0; retry <= maxRetries; retry++)
                {
                    try
                    {
                        LogMessage($"尝试读取离散输入，第 {retry + 1} 次");

                        // RTU模式下重试时添加延迟
                        if (rbRTU.Checked && retry > 0)
                        {
                            await Task.Delay(100);
                        }

                        inputs = master.ReadInputs(slaveId, startAddress, numberOfPoints);
                        LogMessage($"成功读取离散输入，返回 {inputs.Length} 个值");
                        break;
                    }
                    catch (System.IO.IOException ioEx) when (ioEx.Message.Contains("Unexpected byte count"))
                    {
                        LogMessage($"字节计数错误 (尝试 {retry + 1}/{maxRetries + 1}): {ioEx.Message}");

                        if (retry == maxRetries)
                        {
                            throw new Exception($"读取离散输入失败，已重试 {maxRetries + 1} 次。可能原因：\r\n" +
                                "1. 从站设备不支持离散输入功能码(0x02)\r\n" +
                                "2. 指定的地址范围无效\r\n" +
                                "3. 从站ID不正确\r\n" +
                                "4. 通信参数设置错误\r\n" +
                                $"原始错误: {ioEx.Message}", ioEx);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"读取离散输入异常详情 (尝试 {retry + 1}): {ex.GetType().Name} - {ex.Message}");

                        if (retry == maxRetries)
                            throw;
                    }
                }

                // 检查是否成功读取到数据
                if (inputs == null)
                {
                    throw new Exception("读取离散输入失败：未能获取到有效数据");
                }

                txtResult.Clear();
                txtResult.AppendText($"读取离散输入 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}\r\n");
                txtResult.AppendText("结果:\r\n");

                for (int i = 0; i < inputs.Length; i++)
                {
                    txtResult.AppendText($"地址 {startAddress + i}: {inputs[i]}\r\n");
                }

                LogMessage($"成功读取 {inputs.Length} 个离散输入");
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取离散输入失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                LogMessage($"开始读取保持寄存器 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}");

                ushort[] registers =  await master.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfPoints);

                txtResult.Clear();
                txtResult.AppendText($"读取保持寄存器 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}\r\n");
                txtResult.AppendText("结果:\r\n");

                for (int i = 0; i < registers.Length; i++)
                {
                    txtResult.AppendText($"地址 {startAddress + i}: {registers[i]} (0x{registers[i]:X4})\r\n");
                }

                LogMessage($"成功读取 {registers.Length} 个保持寄存器");
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取保持寄存器失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                LogMessage($"开始读取输入寄存器 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}");

                ushort[] registers =await master.ReadInputRegistersAsync(slaveId, startAddress, numberOfPoints);

                txtResult.Clear();
                txtResult.AppendText($"读取输入寄存器 - 从站ID: {slaveId}, 起始地址: {startAddress}, 数量: {numberOfPoints}\r\n");
                txtResult.AppendText("结果:\r\n");

                for (int i = 0; i < registers.Length; i++)
                {
                    txtResult.AppendText($"地址 {startAddress + i}: {registers[i]} (0x{registers[i]:X4})\r\n");
                }

                LogMessage($"成功读取 {registers.Length} 个输入寄存器");
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取输入寄存器失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                LogMessage($"开始写入单个线圈 - 从站ID: {slaveId}, 地址: {address}, 值: {value}");

                await master.WriteSingleCoilAsync(slaveId, address, value);

                txtResult.Clear();
                txtResult.AppendText($"写入单个线圈 - 从站ID: {slaveId}, 地址: {address}, 值: {value}\r\n");
                txtResult.AppendText("写入成功\r\n");

                LogMessage($"成功写入单个线圈: 地址 {address} = {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"写入单个线圈失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                LogMessage($"开始写入单个寄存器 - 从站ID: {slaveId}, 地址: {address}, 值: {value}");

                await master.WriteSingleRegisterAsync(slaveId, address, value);

                txtResult.Clear();
                txtResult.AppendText($"写入单个寄存器 - 从站ID: {slaveId}, 地址: {address}, 值: {value}\r\n");
                txtResult.AppendText("写入成功\r\n");

                LogMessage($"成功写入单个寄存器: 地址 {address} = {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"写入单个寄存器失败: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\r\n内部异常: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        // 新增：数据类型相关方法
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDataType.SelectedItem is DataType dataType)
            {
                int registerCount = GetRegisterCount(dataType);
                lblRegisterCount.Text = $"寄存器数量: {registerCount}";
                lblDataTypeDescription.Text = GetDataTypeDescription(dataType);

                // 根据数据类型调整输入框的可见性和范围
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
                    lblDataTypeValue.Text = "浮点值:";
                    txtDataTypeFloatValue.Text = "0.0";
                    break;

                case DataType.Int64:
                    nudDataTypeIntValue.Visible = false;
                    txtDataTypeFloatValue.Visible = true;
                    lblDataTypeValue.Text = "64位整数值:";
                    txtDataTypeFloatValue.Text = "0";
                    break;

                case DataType.UInt64:
                    nudDataTypeIntValue.Visible = false;
                    txtDataTypeFloatValue.Visible = true;
                    lblDataTypeValue.Text = "无符号64位整数值:";
                    txtDataTypeFloatValue.Text = "0";
                    break;

                default:
                    nudDataTypeIntValue.Visible = true;
                    txtDataTypeFloatValue.Visible = false;
                    lblDataTypeValue.Text = "整数值:";

                    // 调整整数输入范围
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
                MessageBox.Show("请先连接设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataType dataType = (DataType)cmbDataType.SelectedItem;
                ByteOrder byteOrder = (ByteOrder)cmbByteOrder.SelectedItem;
                byte slaveId = (byte)nudDataTypeSlaveId.Value;
                ushort startAddress = (ushort)nudDataTypeAddress.Value;
                int registerCount = GetRegisterCount(dataType);

                LogMessage($"开始读取 {dataType} 数据 - 从站ID: {slaveId}, 地址: {startAddress}, 字节序: {byteOrder}");

                ushort[] registers = await
                    master.ReadHoldingRegistersAsync(slaveId, startAddress, (ushort)registerCount);

                object value = ConvertFromRegisters(registers, dataType, byteOrder);

                txtResult.Clear();
                txtResult.AppendText($"读取 {dataType} 数据成功:\r\n");
                txtResult.AppendText($"从站ID: {slaveId}\r\n");
                txtResult.AppendText($"起始地址: {startAddress}\r\n");
                txtResult.AppendText($"数据类型: {dataType}\r\n");
                txtResult.AppendText($"字节序: {byteOrder}\r\n");
                txtResult.AppendText($"寄存器数量: {registerCount}\r\n");
                txtResult.AppendText($"原始寄存器值: [{string.Join(", ", registers)}]\r\n");
                txtResult.AppendText($"转换后的值: {value}\r\n");

                // 根据数据类型显示额外信息
                if (dataType == DataType.Float || dataType == DataType.Double)
                {
                    txtResult.AppendText($"科学计数法: {Convert.ToDouble(value):E}\r\n");
                }
                else if (dataType != DataType.UInt16 && dataType != DataType.Int16)
                {
                    txtResult.AppendText($"十六进制: 0x{Convert.ToUInt64(value):X}\r\n");
                }

                LogMessage($"成功读取 {dataType} 数据: {value}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取 {cmbDataType.SelectedItem} 数据失败: {ex.Message}";
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnWriteDataType_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("请先连接设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataType dataType = (DataType)cmbDataType.SelectedItem;
                ByteOrder byteOrder = (ByteOrder)cmbByteOrder.SelectedItem;
                byte slaveId = (byte)nudDataTypeSlaveId.Value;
                ushort startAddress = (ushort)nudDataTypeAddress.Value;

                // 获取要写入的值
                object valueToWrite = GetValueToWrite(dataType);
                if (valueToWrite == null) return;

                LogMessage($"开始写入 {dataType} 数据 - 从站ID: {slaveId}, 地址: {startAddress}, 值: {valueToWrite}, 字节序: {byteOrder}");

                // 转换为寄存器数组
                ushort[] registers = ConvertToRegisters(valueToWrite, dataType, byteOrder);

                // 写入寄存器

                if (registers.Length == 1)
                {
                    await master.WriteSingleRegisterAsync(slaveId, startAddress, registers[0]);
                }
                else
                {
                    await master.WriteMultipleRegistersAsync(slaveId, startAddress, registers);
                }


                txtResult.Clear();
                txtResult.AppendText($"写入 {dataType} 数据成功:\r\n");
                txtResult.AppendText($"从站ID: {slaveId}\r\n");
                txtResult.AppendText($"起始地址: {startAddress}\r\n");
                txtResult.AppendText($"数据类型: {dataType}\r\n");
                txtResult.AppendText($"字节序: {byteOrder}\r\n");
                txtResult.AppendText($"写入值: {valueToWrite}\r\n");
                txtResult.AppendText($"寄存器值: [{string.Join(", ", registers)}]\r\n");

                LogMessage($"成功写入 {dataType} 数据: {valueToWrite}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"写入 {cmbDataType.SelectedItem} 数据失败: {ex.Message}";
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        throw new ArgumentException("无效的浮点数格式");
                    case DataType.Double:
                        if (double.TryParse(txtDataTypeFloatValue.Text, out double doubleValue))
                            return doubleValue;
                        throw new ArgumentException("无效的双精度浮点数格式");
                    case DataType.Int64:
                        if (long.TryParse(txtDataTypeFloatValue.Text, out long longValue))
                            return longValue;
                        throw new ArgumentException("无效的64位整数格式");
                    case DataType.UInt64:
                        if (ulong.TryParse(txtDataTypeFloatValue.Text, out ulong ulongValue))
                            return ulongValue;
                        throw new ArgumentException("无效的无符号64位整数格式");
                    default:
                        return (ushort)nudDataTypeIntValue.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输入值格式错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // 转换为寄存器
                ushort[] registers = ConvertToRegisters(valueToTest, dataType, byteOrder);

                // 再转换回来
                object convertedBack = ConvertFromRegisters(registers, dataType, byteOrder);

                txtResult.Clear();
                txtResult.AppendText("数据转换测试:\r\n");
                txtResult.AppendText($"数据类型: {dataType}\r\n");
                txtResult.AppendText($"字节序: {byteOrder}\r\n");
                txtResult.AppendText($"原始值: {valueToTest}\r\n");
                txtResult.AppendText($"寄存器值: [{string.Join(", ", registers)}]\r\n");
                txtResult.AppendText($"转换回的值: {convertedBack}\r\n");
                txtResult.AppendText($"转换是否一致: {valueToTest.Equals(convertedBack)}\r\n");

                // 显示字节级别的信息
                byte[] bytes = new byte[registers.Length * 2];
                for (int i = 0; i < registers.Length; i++)
                {
                    byte[] regBytes = BitConverter.GetBytes(registers[i]);
                    bytes[i * 2] = regBytes[0];
                    bytes[i * 2 + 1] = regBytes[1];
                }
                txtResult.AppendText($"字节数组: [{string.Join(", ", Array.ConvertAll(bytes, b => $"0x{b:X2}"))}]\r\n");

                LogMessage($"转换测试完成: {valueToTest} -> [{string.Join(", ", registers)}] -> {convertedBack}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"转换测试失败: {ex.Message}";
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage(errorMessage);
            }
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("请先连接设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                byte slaveId = (byte)nudSlaveId.Value;
                LogMessage($"开始测试连接 - 从站ID: {slaveId}");

                await master.ReadCoilsAsync(slaveId, 0, 1);

                MessageBox.Show("连接测试成功！", "测试结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMessage("连接测试成功");
            }
            catch (Exception ex)
            {
                string errorMessage = $"连接测试失败: {ex.GetType().Name} - {ex.Message}";
                MessageBox.Show(errorMessage, "测试结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
