namespace AppModbusDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private GroupBox gbConnection;
        private RadioButton rbTCP;
        private RadioButton rbRTU;
        private Panel pnlTCP;
        private Panel pnlRTU;
        private Label lblIP;
        private TextBox txtIP;
        private Label lblPort;
        private NumericUpDown nudPort;
        private Label lblSerialPort;
        private ComboBox cmbSerialPort;
        private Label lblBaudRate;
        private NumericUpDown nudBaudRate;
        private Button btnRefreshPorts;
        private Button btnConnect;
        private GroupBox gbOperations;
        private Label lblSlaveId;
        private NumericUpDown nudSlaveId;
        private Label lblStartAddress;
        private NumericUpDown nudStartAddress;
        private Label lblNumberOfPoints;
        private NumericUpDown nudNumberOfPoints;
        private Button btnReadCoils;
        private Button btnReadInputs;
        private Button btnReadHoldingRegisters;
        private Button btnReadInputRegisters;
        private Button btnTestConnection;
        private GroupBox gbWrite;
        private Label lblWriteAddress;
        private NumericUpDown nudWriteAddress;
        private CheckBox cbCoilValue;
        private Button btnWriteSingleCoil;
        private Label lblRegisterValue;
        private NumericUpDown nudRegisterValue;
        private Button btnWriteSingleRegister;
        private GroupBox gbResult;
        private TextBox txtResult;
        private GroupBox gbLog;
        private TextBox txtLog;
        private Button btnClearLog;
        private GroupBox gbAdvancedSettings;
        private Label lblReadTimeout;
        private NumericUpDown nudReadTimeout;
        private Label lblWriteTimeout;
        private NumericUpDown nudWriteTimeout;
        private Label lblRetries;
        private NumericUpDown nudRetries;

        private GroupBox gbDataType;
        private Label lblDataType;
        private ComboBox cmbDataType;
        private Label lblByteOrder;
        private ComboBox cmbByteOrder;
        private Label lblDataTypeSlaveId;
        private NumericUpDown nudDataTypeSlaveId;
        private Label lblDataTypeAddress;
        private NumericUpDown nudDataTypeAddress;
        private Label lblDataTypeValue;
        private NumericUpDown nudDataTypeIntValue;
        private TextBox txtDataTypeFloatValue;
        private Button btnReadDataType;
        private Button btnWriteDataType;
        private Button btnTestConversion;
        private Label lblRegisterCount;
        private Label lblDataTypeDescription;
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            gbConnection = new GroupBox();
            btnConnect = new Button();
            pnlRTU = new Panel();
            btnRefreshPorts = new Button();
            nudBaudRate = new NumericUpDown();
            lblBaudRate = new Label();
            cmbSerialPort = new ComboBox();
            lblSerialPort = new Label();
            pnlTCP = new Panel();
            nudPort = new NumericUpDown();
            lblPort = new Label();
            txtIP = new TextBox();
            lblIP = new Label();
            rbRTU = new RadioButton();
            rbTCP = new RadioButton();
            gbAdvancedSettings = new GroupBox();
            nudRetries = new NumericUpDown();
            lblRetries = new Label();
            nudWriteTimeout = new NumericUpDown();
            lblWriteTimeout = new Label();
            nudReadTimeout = new NumericUpDown();
            lblReadTimeout = new Label();
            gbOperations = new GroupBox();
            btnTestConnection = new Button();
            btnReadInputRegisters = new Button();
            btnReadHoldingRegisters = new Button();
            btnReadInputs = new Button();
            btnReadCoils = new Button();
            nudNumberOfPoints = new NumericUpDown();
            lblNumberOfPoints = new Label();
            nudStartAddress = new NumericUpDown();
            lblStartAddress = new Label();
            nudSlaveId = new NumericUpDown();
            lblSlaveId = new Label();
            gbWrite = new GroupBox();
            btnWriteSingleRegister = new Button();
            nudRegisterValue = new NumericUpDown();
            lblRegisterValue = new Label();
            btnWriteSingleCoil = new Button();
            cbCoilValue = new CheckBox();
            nudWriteAddress = new NumericUpDown();
            lblWriteAddress = new Label();

            // 新增数据类型操作组  
            gbDataType = new GroupBox();
            lblDataType = new Label();
            cmbDataType = new ComboBox();
            lblByteOrder = new Label();
            cmbByteOrder = new ComboBox();
            lblDataTypeSlaveId = new Label();
            nudDataTypeSlaveId = new NumericUpDown();
            lblDataTypeAddress = new Label();
            nudDataTypeAddress = new NumericUpDown();
            lblDataTypeValue = new Label();
            nudDataTypeIntValue = new NumericUpDown();
            txtDataTypeFloatValue = new TextBox();
            btnReadDataType = new Button();
            btnWriteDataType = new Button();
            btnTestConversion = new Button();
            lblRegisterCount = new Label();
            lblDataTypeDescription = new Label();

            gbResult = new GroupBox();
            txtResult = new TextBox();
            gbLog = new GroupBox();
            btnClearLog = new Button();
            txtLog = new TextBox();

            gbConnection.SuspendLayout();
            pnlRTU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaudRate).BeginInit();
            pnlTCP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            gbAdvancedSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRetries).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudReadTimeout).BeginInit();
            gbOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudNumberOfPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStartAddress).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSlaveId).BeginInit();
            gbWrite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRegisterValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteAddress).BeginInit();
            gbDataType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeSlaveId).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeAddress).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeIntValue).BeginInit();
            gbResult.SuspendLayout();
            gbLog.SuspendLayout();
            SuspendLayout();

            // 原有控件初始化代码保持不变...  
            // gbConnection  
            gbConnection.Controls.Add(btnConnect);
            gbConnection.Controls.Add(pnlRTU);
            gbConnection.Controls.Add(pnlTCP);
            gbConnection.Controls.Add(rbRTU);
            gbConnection.Controls.Add(rbTCP);
            gbConnection.Location = new Point(12, 12);
            gbConnection.Name = "gbConnection";
            gbConnection.Size = new Size(400, 200);
            gbConnection.TabIndex = 0;
            gbConnection.TabStop = false;
            gbConnection.Text = "连接设置";

            // btnConnect  
            btnConnect.BackColor = Color.Green;
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(280, 140);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 40);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;

            // pnlRTU  
            pnlRTU.Controls.Add(btnRefreshPorts);
            pnlRTU.Controls.Add(nudBaudRate);
            pnlRTU.Controls.Add(lblBaudRate);
            pnlRTU.Controls.Add(cmbSerialPort);
            pnlRTU.Controls.Add(lblSerialPort);
            pnlRTU.Location = new Point(20, 50);
            pnlRTU.Name = "pnlRTU";
            pnlRTU.Size = new Size(360, 80);
            pnlRTU.TabIndex = 3;
            pnlRTU.Visible = false;

            // btnRefreshPorts  
            btnRefreshPorts.Location = new Point(160, 11);
            btnRefreshPorts.Name = "btnRefreshPorts";
            btnRefreshPorts.Size = new Size(60, 25);
            btnRefreshPorts.TabIndex = 4;
            btnRefreshPorts.Text = "刷新";
            btnRefreshPorts.UseVisualStyleBackColor = true;
            btnRefreshPorts.Click += btnRefreshPorts_Click;

            // nudBaudRate  
            nudBaudRate.Location = new Point(70, 42);
            nudBaudRate.Maximum = new decimal(new int[] { 115200, 0, 0, 0 });
            nudBaudRate.Minimum = new decimal(new int[] { 1200, 0, 0, 0 });
            nudBaudRate.Name = "nudBaudRate";
            nudBaudRate.Size = new Size(80, 23);
            nudBaudRate.TabIndex = 3;
            nudBaudRate.Value = new decimal(new int[] { 9600, 0, 0, 0 });

            // lblBaudRate  
            lblBaudRate.AutoSize = true;
            lblBaudRate.Location = new Point(10, 45);
            lblBaudRate.Name = "lblBaudRate";
            lblBaudRate.Size = new Size(49, 15);
            lblBaudRate.TabIndex = 2;
            lblBaudRate.Text = "波特率:";

            // cmbSerialPort  
            cmbSerialPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSerialPort.FormattingEnabled = true;
            cmbSerialPort.Location = new Point(70, 12);
            cmbSerialPort.Name = "cmbSerialPort";
            cmbSerialPort.Size = new Size(80, 23);
            cmbSerialPort.TabIndex = 1;

            // lblSerialPort  
            lblSerialPort.AutoSize = true;
            lblSerialPort.Location = new Point(10, 15);
            lblSerialPort.Name = "lblSerialPort";
            lblSerialPort.Size = new Size(36, 15);
            lblSerialPort.TabIndex = 0;
            lblSerialPort.Text = "串口:";

            // pnlTCP  
            pnlTCP.Controls.Add(nudPort);
            pnlTCP.Controls.Add(lblPort);
            pnlTCP.Controls.Add(txtIP);
            pnlTCP.Controls.Add(lblIP);
            pnlTCP.Location = new Point(20, 50);
            pnlTCP.Name = "pnlTCP";
            pnlTCP.Size = new Size(360, 80);
            pnlTCP.TabIndex = 2;

            // nudPort  
            nudPort.Location = new Point(250, 12);
            nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Size = new Size(80, 23);
            nudPort.TabIndex = 3;
            nudPort.Value = new decimal(new int[] { 502, 0, 0, 0 });

            // lblPort  
            lblPort.AutoSize = true;
            lblPort.Location = new Point(210, 15);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(36, 15);
            lblPort.TabIndex = 2;
            lblPort.Text = "端口:";

            // txtIP  
            txtIP.Location = new Point(70, 12);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(120, 23);
            txtIP.TabIndex = 1;
            txtIP.Text = "127.0.0.1";

            // lblIP  
            lblIP.AutoSize = true;
            lblIP.Location = new Point(10, 15);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(46, 15);
            lblIP.TabIndex = 0;
            lblIP.Text = "IP地址:";

            // rbRTU  
            rbRTU.AutoSize = true;
            rbRTU.Location = new Point(120, 25);
            rbRTU.Name = "rbRTU";
            rbRTU.Size = new Size(93, 19);
            rbRTU.TabIndex = 1;
            rbRTU.Text = "Modbus RTU";
            rbRTU.UseVisualStyleBackColor = true;
            rbRTU.CheckedChanged += rbRTU_CheckedChanged;

            // rbTCP  
            rbTCP.AutoSize = true;
            rbTCP.Checked = true;
            rbTCP.Location = new Point(20, 25);
            rbTCP.Name = "rbTCP";
            rbTCP.Size = new Size(93, 19);
            rbTCP.TabIndex = 0;
            rbTCP.TabStop = true;
            rbTCP.Text = "Modbus TCP";
            rbTCP.UseVisualStyleBackColor = true;
            rbTCP.CheckedChanged += rbTCP_CheckedChanged;

            // gbAdvancedSettings  
            gbAdvancedSettings.Controls.Add(nudRetries);
            gbAdvancedSettings.Controls.Add(lblRetries);
            gbAdvancedSettings.Controls.Add(nudWriteTimeout);
            gbAdvancedSettings.Controls.Add(lblWriteTimeout);
            gbAdvancedSettings.Controls.Add(nudReadTimeout);
            gbAdvancedSettings.Controls.Add(lblReadTimeout);
            gbAdvancedSettings.Location = new Point(12, 230);
            gbAdvancedSettings.Name = "gbAdvancedSettings";
            gbAdvancedSettings.Size = new Size(400, 100);
            gbAdvancedSettings.TabIndex = 5;
            gbAdvancedSettings.TabStop = false;
            gbAdvancedSettings.Text = "高级设置";

            // nudRetries  
            nudRetries.Location = new Point(90, 52);
            nudRetries.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudRetries.Name = "nudRetries";
            nudRetries.Size = new Size(80, 23);
            nudRetries.TabIndex = 5;
            nudRetries.Value = new decimal(new int[] { 3, 0, 0, 0 });

            // lblRetries  
            lblRetries.AutoSize = true;
            lblRetries.Location = new Point(20, 55);
            lblRetries.Name = "lblRetries";
            lblRetries.Size = new Size(62, 15);
            lblRetries.TabIndex = 4;
            lblRetries.Text = "重试次数:";

            // nudWriteTimeout  
            nudWriteTimeout.Location = new Point(260, 22);
            nudWriteTimeout.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            nudWriteTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            nudWriteTimeout.Name = "nudWriteTimeout";
            nudWriteTimeout.Size = new Size(80, 23);
            nudWriteTimeout.TabIndex = 3;
            nudWriteTimeout.Value = new decimal(new int[] { 5000, 0, 0, 0 });

            // lblWriteTimeout  
            lblWriteTimeout.AutoSize = true;
            lblWriteTimeout.Location = new Point(190, 25);
            lblWriteTimeout.Name = "lblWriteTimeout";
            lblWriteTimeout.Size = new Size(73, 15);
            lblWriteTimeout.TabIndex = 2;
            lblWriteTimeout.Text = "写超时(ms):";

            // nudReadTimeout  
            nudReadTimeout.Location = new Point(90, 22);
            nudReadTimeout.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            nudReadTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            nudReadTimeout.Name = "nudReadTimeout";
            nudReadTimeout.Size = new Size(80, 23);
            nudReadTimeout.TabIndex = 1;
            nudReadTimeout.Value = new decimal(new int[] { 5000, 0, 0, 0 });

            // lblReadTimeout  
            lblReadTimeout.AutoSize = true;
            lblReadTimeout.Location = new Point(20, 25);
            lblReadTimeout.Name = "lblReadTimeout";
            lblReadTimeout.Size = new Size(73, 15);
            lblReadTimeout.TabIndex = 0;
            lblReadTimeout.Text = "读超时(ms):";

            // gbOperations  
            gbOperations.Controls.Add(btnTestConnection);
            gbOperations.Controls.Add(btnReadInputRegisters);
            gbOperations.Controls.Add(btnReadHoldingRegisters);
            gbOperations.Controls.Add(btnReadInputs);
            gbOperations.Controls.Add(btnReadCoils);
            gbOperations.Controls.Add(nudNumberOfPoints);
            gbOperations.Controls.Add(lblNumberOfPoints);
            gbOperations.Controls.Add(nudStartAddress);
            gbOperations.Controls.Add(lblStartAddress);
            gbOperations.Controls.Add(nudSlaveId);
            gbOperations.Controls.Add(lblSlaveId);
            gbOperations.Enabled = false;
            gbOperations.Location = new Point(430, 12);
            gbOperations.Name = "gbOperations";
            gbOperations.Size = new Size(350, 200);
            gbOperations.TabIndex = 1;
            gbOperations.TabStop = false;
            gbOperations.Text = "基本读取操作";

            // btnTestConnection  
            btnTestConnection.BackColor = Color.LightBlue;
            btnTestConnection.Location = new Point(250, 160);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(80, 30);
            btnTestConnection.TabIndex = 10;
            btnTestConnection.Text = "测试连接";
            btnTestConnection.UseVisualStyleBackColor = false;
            btnTestConnection.Click += btnTestConnection_Click;

            // btnReadInputRegisters  
            btnReadInputRegisters.Location = new Point(130, 130);
            btnReadInputRegisters.Name = "btnReadInputRegisters";
            btnReadInputRegisters.Size = new Size(100, 30);
            btnReadInputRegisters.TabIndex = 9;
            btnReadInputRegisters.Text = "读输入寄存器";
            btnReadInputRegisters.UseVisualStyleBackColor = true;
            btnReadInputRegisters.Click += btnReadInputRegisters_Click;

            // btnReadHoldingRegisters  
            btnReadHoldingRegisters.Location = new Point(20, 130);
            btnReadHoldingRegisters.Name = "btnReadHoldingRegisters";
            btnReadHoldingRegisters.Size = new Size(100, 30);
            btnReadHoldingRegisters.TabIndex = 8;
            btnReadHoldingRegisters.Text = "读保持寄存器";
            btnReadHoldingRegisters.UseVisualStyleBackColor = true;
            btnReadHoldingRegisters.Click += btnReadHoldingRegisters_Click;

            // btnReadInputs  
            btnReadInputs.Location = new Point(100, 90);
            btnReadInputs.Name = "btnReadInputs";
            btnReadInputs.Size = new Size(80, 30);
            btnReadInputs.TabIndex = 7;
            btnReadInputs.Text = "读离散输入";
            btnReadInputs.UseVisualStyleBackColor = true;
            btnReadInputs.Click += btnReadInputs_Click;

            // btnReadCoils  
            btnReadCoils.Location = new Point(20, 90);
            btnReadCoils.Name = "btnReadCoils";
            btnReadCoils.Size = new Size(70, 30);
            btnReadCoils.TabIndex = 6;
            btnReadCoils.Text = "读线圈";
            btnReadCoils.UseVisualStyleBackColor = true;
            btnReadCoils.Click += btnReadCoils_Click;

            // nudNumberOfPoints  
            nudNumberOfPoints.Location = new Point(90, 52);
            nudNumberOfPoints.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            nudNumberOfPoints.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudNumberOfPoints.Name = "nudNumberOfPoints";
            nudNumberOfPoints.Size = new Size(60, 23);
            nudNumberOfPoints.TabIndex = 5;
            nudNumberOfPoints.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblNumberOfPoints  
            lblNumberOfPoints.AutoSize = true;
            lblNumberOfPoints.Location = new Point(20, 55);
            lblNumberOfPoints.Name = "lblNumberOfPoints";
            lblNumberOfPoints.Size = new Size(36, 15);
            lblNumberOfPoints.TabIndex = 4;
            lblNumberOfPoints.Text = "数量:";

            // nudStartAddress  
            nudStartAddress.Location = new Point(250, 22);
            nudStartAddress.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudStartAddress.Name = "nudStartAddress";
            nudStartAddress.Size = new Size(80, 23);
            nudStartAddress.TabIndex = 3;

            // lblStartAddress  
            lblStartAddress.AutoSize = true;
            lblStartAddress.Location = new Point(170, 25);
            lblStartAddress.Name = "lblStartAddress";
            lblStartAddress.Size = new Size(62, 15);
            lblStartAddress.TabIndex = 2;
            lblStartAddress.Text = "起始地址:";

            // nudSlaveId  
            nudSlaveId.Location = new Point(90, 22);
            nudSlaveId.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudSlaveId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSlaveId.Name = "nudSlaveId";
            nudSlaveId.Size = new Size(60, 23);
            nudSlaveId.TabIndex = 1;
            nudSlaveId.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblSlaveId  
            lblSlaveId.AutoSize = true;
            lblSlaveId.Location = new Point(20, 25);
            lblSlaveId.Name = "lblSlaveId";
            lblSlaveId.Size = new Size(47, 15);
            lblSlaveId.TabIndex = 0;
            lblSlaveId.Text = "从站ID:";

            // gbWrite  
            gbWrite.Controls.Add(btnWriteSingleRegister);
            gbWrite.Controls.Add(nudRegisterValue);
            gbWrite.Controls.Add(lblRegisterValue);
            gbWrite.Controls.Add(btnWriteSingleCoil);
            gbWrite.Controls.Add(cbCoilValue);
            gbWrite.Controls.Add(nudWriteAddress);
            gbWrite.Controls.Add(lblWriteAddress);
            gbWrite.Enabled = false;
            gbWrite.Location = new Point(800, 12);
            gbWrite.Name = "gbWrite";
            gbWrite.Size = new Size(280, 200);
            gbWrite.TabIndex = 2;
            gbWrite.TabStop = false;
            gbWrite.Text = "基本写入操作";

            // btnWriteSingleRegister  
            btnWriteSingleRegister.Location = new Point(20, 155);
            btnWriteSingleRegister.Name = "btnWriteSingleRegister";
            btnWriteSingleRegister.Size = new Size(120, 30);
            btnWriteSingleRegister.TabIndex = 6;
            btnWriteSingleRegister.Text = "写单个寄存器";
            btnWriteSingleRegister.UseVisualStyleBackColor = true;
            btnWriteSingleRegister.Click += btnWriteSingleRegister_Click;

            // nudRegisterValue  
            nudRegisterValue.Location = new Point(100, 122);
            nudRegisterValue.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudRegisterValue.Name = "nudRegisterValue";
            nudRegisterValue.Size = new Size(80, 23);
            nudRegisterValue.TabIndex = 5;

            // lblRegisterValue  
            lblRegisterValue.AutoSize = true;
            lblRegisterValue.Location = new Point(20, 125);
            lblRegisterValue.Name = "lblRegisterValue";
            lblRegisterValue.Size = new Size(62, 15);
            lblRegisterValue.TabIndex = 4;
            lblRegisterValue.Text = "寄存器值:";

            // btnWriteSingleCoil  
            btnWriteSingleCoil.Location = new Point(20, 85);
            btnWriteSingleCoil.Name = "btnWriteSingleCoil";
            btnWriteSingleCoil.Size = new Size(100, 30);
            btnWriteSingleCoil.TabIndex = 3;
            btnWriteSingleCoil.Text = "写单个线圈";
            btnWriteSingleCoil.UseVisualStyleBackColor = true;
            btnWriteSingleCoil.Click += btnWriteSingleCoil_Click;

            // cbCoilValue  
            cbCoilValue.AutoSize = true;
            cbCoilValue.Location = new Point(20, 55);
            cbCoilValue.Name = "cbCoilValue";
            cbCoilValue.Size = new Size(78, 19);
            cbCoilValue.TabIndex = 2;
            cbCoilValue.Text = "线圈状态";
            cbCoilValue.UseVisualStyleBackColor = true;

            // nudWriteAddress  
            nudWriteAddress.Location = new Point(80, 22);
            nudWriteAddress.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudWriteAddress.Name = "nudWriteAddress";
            nudWriteAddress.Size = new Size(80, 23);
            nudWriteAddress.TabIndex = 1;

            // lblWriteAddress  
            lblWriteAddress.AutoSize = true;
            lblWriteAddress.Location = new Point(20, 25);
            lblWriteAddress.Name = "lblWriteAddress";
            lblWriteAddress.Size = new Size(36, 15);
            lblWriteAddress.TabIndex = 0;
            lblWriteAddress.Text = "地址:";

            // 新增：gbDataType - 数据类型操作组  
            gbDataType.Controls.Add(lblDataTypeDescription);
            gbDataType.Controls.Add(lblRegisterCount);
            gbDataType.Controls.Add(btnTestConversion);
            gbDataType.Controls.Add(btnWriteDataType);
            gbDataType.Controls.Add(btnReadDataType);
            gbDataType.Controls.Add(txtDataTypeFloatValue);
            gbDataType.Controls.Add(nudDataTypeIntValue);
            gbDataType.Controls.Add(lblDataTypeValue);
            gbDataType.Controls.Add(nudDataTypeAddress);
            gbDataType.Controls.Add(lblDataTypeAddress);
            gbDataType.Controls.Add(nudDataTypeSlaveId);
            gbDataType.Controls.Add(lblDataTypeSlaveId);
            gbDataType.Controls.Add(cmbByteOrder);
            gbDataType.Controls.Add(lblByteOrder);
            gbDataType.Controls.Add(cmbDataType);
            gbDataType.Controls.Add(lblDataType);
            gbDataType.Enabled = false;
            gbDataType.Location = new Point(430, 230);
            gbDataType.Name = "gbDataType";
            gbDataType.Size = new Size(650, 200);
            gbDataType.TabIndex = 6;
            gbDataType.TabStop = false;
            gbDataType.Text = "数据类型操作 (Float, Double, Long等)";

            // lblDataType  
            lblDataType.AutoSize = true;
            lblDataType.Location = new Point(20, 25);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(62, 15);
            lblDataType.TabIndex = 0;
            lblDataType.Text = "数据类型:";

            // cmbDataType  
            cmbDataType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDataType.FormattingEnabled = true;
            cmbDataType.Location = new Point(90, 22);
            cmbDataType.Name = "cmbDataType";
            cmbDataType.Size = new Size(100, 23);
            cmbDataType.TabIndex = 1;
            cmbDataType.SelectedIndexChanged += cmbDataType_SelectedIndexChanged;

            // lblByteOrder  
            lblByteOrder.AutoSize = true;
            lblByteOrder.Location = new Point(210, 25);
            lblByteOrder.Name = "lblByteOrder";
            lblByteOrder.Size = new Size(49, 15);
            lblByteOrder.TabIndex = 2;
            lblByteOrder.Text = "字节序:";

            // cmbByteOrder  
            cmbByteOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbByteOrder.FormattingEnabled = true;
            cmbByteOrder.Location = new Point(270, 22);
            cmbByteOrder.Name = "cmbByteOrder";
            cmbByteOrder.Size = new Size(150, 23);
            cmbByteOrder.TabIndex = 3;

            // lblDataTypeSlaveId  
            lblDataTypeSlaveId.AutoSize = true;
            lblDataTypeSlaveId.Location = new Point(20, 58);
            lblDataTypeSlaveId.Name = "lblDataTypeSlaveId";
            lblDataTypeSlaveId.Size = new Size(47, 15);
            lblDataTypeSlaveId.TabIndex = 4;
            lblDataTypeSlaveId.Text = "从站ID:";

            // nudDataTypeSlaveId  
            nudDataTypeSlaveId.Location = new Point(90, 55);
            nudDataTypeSlaveId.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudDataTypeSlaveId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudDataTypeSlaveId.Name = "nudDataTypeSlaveId";
            nudDataTypeSlaveId.Size = new Size(60, 23);
            nudDataTypeSlaveId.TabIndex = 5;
            nudDataTypeSlaveId.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblDataTypeAddress  
            lblDataTypeAddress.AutoSize = true;
            lblDataTypeAddress.Location = new Point(210, 58);
            lblDataTypeAddress.Name = "lblDataTypeAddress";
            lblDataTypeAddress.Size = new Size(62, 15);
            lblDataTypeAddress.TabIndex = 6;
            lblDataTypeAddress.Text = "起始地址:";

            // nudDataTypeAddress  
            nudDataTypeAddress.Location = new Point(280, 55);
            nudDataTypeAddress.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudDataTypeAddress.Name = "nudDataTypeAddress";
            nudDataTypeAddress.Size = new Size(80, 23);
            nudDataTypeAddress.TabIndex = 7;

            // lblDataTypeValue  
            lblDataTypeValue.AutoSize = true;
            lblDataTypeValue.Location = new Point(20, 91);
            lblDataTypeValue.Name = "lblDataTypeValue";
            lblDataTypeValue.Size = new Size(49, 15);
            lblDataTypeValue.TabIndex = 8;
            lblDataTypeValue.Text = "整数值:";

            // nudDataTypeIntValue  
            nudDataTypeIntValue.Location = new Point(90, 88);
            nudDataTypeIntValue.Maximum = new decimal(new int[] { 2147483647, 0, 0, 0 });
            nudDataTypeIntValue.Minimum = new decimal(new int[] { -2147483648, 0, 0, int.MinValue });
            nudDataTypeIntValue.Name = "nudDataTypeIntValue";
            nudDataTypeIntValue.Size = new Size(120, 23);
            nudDataTypeIntValue.TabIndex = 9;

            // txtDataTypeFloatValue
            txtDataTypeFloatValue.Location = new Point(90, 88);
            txtDataTypeFloatValue.Name = "txtDataTypeFloatValue";
            txtDataTypeFloatValue.Size = new Size(120, 23);
            txtDataTypeFloatValue.TabIndex = 10;
            txtDataTypeFloatValue.Text = "0.0";
            txtDataTypeFloatValue.Visible = false;

            // btnReadDataType
            btnReadDataType.BackColor = Color.LightGreen;
            btnReadDataType.Location = new Point(20, 125);
            btnReadDataType.Name = "btnReadDataType";
            btnReadDataType.Size = new Size(100, 35);
            btnReadDataType.TabIndex = 11;
            btnReadDataType.Text = "读取数据";
            btnReadDataType.UseVisualStyleBackColor = false;
            btnReadDataType.Click += btnReadDataType_Click;

            // btnWriteDataType
            btnWriteDataType.BackColor = Color.LightCoral;
            btnWriteDataType.Location = new Point(130, 125);
            btnWriteDataType.Name = "btnWriteDataType";
            btnWriteDataType.Size = new Size(100, 35);
            btnWriteDataType.TabIndex = 12;
            btnWriteDataType.Text = "写入数据";
            btnWriteDataType.UseVisualStyleBackColor = false;
            btnWriteDataType.Click += btnWriteDataType_Click;

            // btnTestConversion
            btnTestConversion.BackColor = Color.LightBlue;
            btnTestConversion.Location = new Point(240, 125);
            btnTestConversion.Name = "btnTestConversion";
            btnTestConversion.Size = new Size(100, 35);
            btnTestConversion.TabIndex = 13;
            btnTestConversion.Text = "转换测试";
            btnTestConversion.UseVisualStyleBackColor = false;
            btnTestConversion.Click += btnTestConversion_Click;

            // lblRegisterCount
            lblRegisterCount.AutoSize = true;
            lblRegisterCount.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblRegisterCount.ForeColor = Color.Blue;
            lblRegisterCount.Location = new Point(450, 25);
            lblRegisterCount.Name = "lblRegisterCount";
            lblRegisterCount.Size = new Size(80, 13);
            lblRegisterCount.TabIndex = 14;
            lblRegisterCount.Text = "寄存器数量: 1";

            // lblDataTypeDescription
            lblDataTypeDescription.AutoSize = true;
            lblDataTypeDescription.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic);
            lblDataTypeDescription.ForeColor = Color.DarkGreen;
            lblDataTypeDescription.Location = new Point(450, 50);
            lblDataTypeDescription.Name = "lblDataTypeDescription";
            lblDataTypeDescription.Size = new Size(150, 13);
            lblDataTypeDescription.TabIndex = 15;
            lblDataTypeDescription.Text = "无符号16位整数 (1寄存器)";

            // gbResult
            gbResult.Controls.Add(txtResult);
            gbResult.Location = new Point(12, 450);
            gbResult.Name = "gbResult";
            gbResult.Size = new Size(520, 250);
            gbResult.TabIndex = 3;
            gbResult.TabStop = false;
            gbResult.Text = "操作结果";

            // txtResult
            txtResult.Dock = DockStyle.Fill;
            txtResult.Font = new Font("Consolas", 9F);
            txtResult.Location = new Point(3, 19);
            txtResult.Multiline = true;
            txtResult.Name = "txtResult";
            txtResult.ReadOnly = true;
            txtResult.ScrollBars = ScrollBars.Both;
            txtResult.Size = new Size(514, 228);
            txtResult.TabIndex = 0;

            // gbLog
            gbLog.Controls.Add(btnClearLog);
            gbLog.Controls.Add(txtLog);
            gbLog.Location = new Point(550, 450);
            gbLog.Name = "gbLog";
            gbLog.Size = new Size(530, 250);
            gbLog.TabIndex = 4;
            gbLog.TabStop = false;
            gbLog.Text = "操作日志";

            // btnClearLog
            btnClearLog.Location = new Point(440, 218);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(80, 25);
            btnClearLog.TabIndex = 1;
            btnClearLog.Text = "清空日志";
            btnClearLog.UseVisualStyleBackColor = true;
            btnClearLog.Click += btnClearLog_Click;

            // txtLog
            txtLog.Font = new Font("Consolas", 8F);
            txtLog.Location = new Point(6, 22);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(518, 190);
            txtLog.TabIndex = 0;

            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 720);
            Controls.Add(gbDataType);
            Controls.Add(gbAdvancedSettings);
            Controls.Add(gbLog);
            Controls.Add(gbResult);
            Controls.Add(gbWrite);
            Controls.Add(gbOperations);
            Controls.Add(gbConnection);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Modbus TCP/RTU 通信工具 - 支持多数据类型";

            gbConnection.ResumeLayout(false);
            gbConnection.PerformLayout();
            pnlRTU.ResumeLayout(false);
            pnlRTU.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBaudRate).EndInit();
            pnlTCP.ResumeLayout(false);
            pnlTCP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
            gbAdvancedSettings.ResumeLayout(false);
            gbAdvancedSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRetries).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudReadTimeout).EndInit();
            gbOperations.ResumeLayout(false);
            gbOperations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudNumberOfPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStartAddress).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSlaveId).EndInit();
            gbWrite.ResumeLayout(false);
            gbWrite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRegisterValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteAddress).EndInit();
            gbDataType.ResumeLayout(false);
            gbDataType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeSlaveId).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeAddress).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDataTypeIntValue).EndInit();
            gbResult.ResumeLayout(false);
            gbResult.PerformLayout();
            gbLog.ResumeLayout(false);
            gbLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
