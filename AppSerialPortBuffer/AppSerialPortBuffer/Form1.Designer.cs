namespace AppSerialPortBuffer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            pnlMain = new Panel();
            grpConnection = new GroupBox();
            pnlConfig = new Panel();
            cmbBaudRate = new ComboBox();
            lblPort = new Label();
            cmbStopBits = new ComboBox();
            cmbPort = new ComboBox();
            lblStopBits = new Label();
            lblBaudRate = new Label();
            cmbDataBits = new ComboBox();
            lblDataBits = new Label();
            lblParity = new Label();
            cmbParity = new ComboBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            grpSettings = new GroupBox();
            lblReadBuffer = new Label();
            nudReadBuffer = new NumericUpDown();
            lblWriteBuffer = new Label();
            nudWriteBuffer = new NumericUpDown();
            lblTimeout = new Label();
            nudTimeout = new NumericUpDown();
            chkAsync = new CheckBox();
            grpData = new GroupBox();
            txtSend = new TextBox();
            btnSend = new Button();
            btnClear = new Button();
            rtbReceived = new RichTextBox();
            grpStatus = new GroupBox();
            lblStatus = new Label();
            lblBytesSent = new Label();
            lblBytesReceived = new Label();
            pgbConnection = new ProgressBar();
            ssMain = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            tsslTime = new ToolStripStatusLabel();
            pnlMain.SuspendLayout();
            grpConnection.SuspendLayout();
            pnlConfig.SuspendLayout();
            grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudReadBuffer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteBuffer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudTimeout).BeginInit();
            grpData.SuspendLayout();
            grpStatus.SuspendLayout();
            ssMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(240, 240, 240);
            pnlMain.Controls.Add(grpConnection);
            pnlMain.Controls.Add(grpSettings);
            pnlMain.Controls.Add(grpData);
            pnlMain.Controls.Add(grpStatus);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(10);
            pnlMain.Size = new Size(700, 538);
            pnlMain.TabIndex = 0;
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(pnlConfig);
            grpConnection.Controls.Add(btnConnect);
            grpConnection.Controls.Add(btnDisconnect);
            grpConnection.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            grpConnection.ForeColor = Color.FromArgb(64, 64, 64);
            grpConnection.Location = new Point(10, 10);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(380, 120);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "串口连接";
            // 
            // pnlConfig
            // 
            pnlConfig.Controls.Add(cmbBaudRate);
            pnlConfig.Controls.Add(lblPort);
            pnlConfig.Controls.Add(cmbStopBits);
            pnlConfig.Controls.Add(cmbPort);
            pnlConfig.Controls.Add(lblStopBits);
            pnlConfig.Controls.Add(lblBaudRate);
            pnlConfig.Controls.Add(cmbDataBits);
            pnlConfig.Controls.Add(lblDataBits);
            pnlConfig.Controls.Add(lblParity);
            pnlConfig.Controls.Add(cmbParity);
            pnlConfig.Location = new Point(0, 18);
            pnlConfig.Name = "pnlConfig";
            pnlConfig.Size = new Size(374, 68);
            pnlConfig.TabIndex = 12;
            // 
            // cmbBaudRate
            // 
            cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBaudRate.Font = new Font("Consolas", 9F);
            cmbBaudRate.Location = new Point(201, 3);
            cmbBaudRate.Name = "cmbBaudRate";
            cmbBaudRate.Size = new Size(80, 22);
            cmbBaudRate.TabIndex = 3;
            // 
            // lblPort
            // 
            lblPort.Location = new Point(11, 3);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(40, 23);
            lblPort.TabIndex = 0;
            lblPort.Text = "端口:";
            lblPort.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbStopBits
            // 
            cmbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStopBits.Location = new Point(291, 33);
            cmbStopBits.Name = "cmbStopBits";
            cmbStopBits.Size = new Size(70, 25);
            cmbStopBits.TabIndex = 9;
            // 
            // cmbPort
            // 
            cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPort.Font = new Font("Consolas", 9F);
            cmbPort.Location = new Point(56, 3);
            cmbPort.Name = "cmbPort";
            cmbPort.Size = new Size(80, 22);
            cmbPort.TabIndex = 1;
            // 
            // lblStopBits
            // 
            lblStopBits.Location = new Point(291, 3);
            lblStopBits.Name = "lblStopBits";
            lblStopBits.Size = new Size(50, 23);
            lblStopBits.TabIndex = 8;
            lblStopBits.Text = "停止位:";
            lblStopBits.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBaudRate
            // 
            lblBaudRate.Location = new Point(146, 3);
            lblBaudRate.Name = "lblBaudRate";
            lblBaudRate.Size = new Size(50, 23);
            lblBaudRate.TabIndex = 2;
            lblBaudRate.Text = "波特率:";
            lblBaudRate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbDataBits
            // 
            cmbDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDataBits.Location = new Point(201, 33);
            cmbDataBits.Name = "cmbDataBits";
            cmbDataBits.Size = new Size(80, 25);
            cmbDataBits.TabIndex = 7;
            // 
            // lblDataBits
            // 
            lblDataBits.Location = new Point(146, 33);
            lblDataBits.Name = "lblDataBits";
            lblDataBits.Size = new Size(50, 23);
            lblDataBits.TabIndex = 6;
            lblDataBits.Text = "数据位:";
            lblDataBits.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblParity
            // 
            lblParity.Location = new Point(11, 33);
            lblParity.Name = "lblParity";
            lblParity.Size = new Size(40, 23);
            lblParity.TabIndex = 4;
            lblParity.Text = "校验:";
            lblParity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbParity
            // 
            cmbParity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParity.Location = new Point(56, 33);
            cmbParity.Name = "cmbParity";
            cmbParity.Size = new Size(80, 25);
            cmbParity.TabIndex = 5;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(76, 175, 80);
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(6, 89);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(80, 28);
            btnConnect.TabIndex = 10;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += BtnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.FromArgb(244, 67, 54);
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.Location = new Point(96, 89);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(80, 28);
            btnDisconnect.TabIndex = 11;
            btnDisconnect.Text = "断开";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += BtnDisconnect_Click;
            // 
            // grpSettings
            // 
            grpSettings.Controls.Add(lblReadBuffer);
            grpSettings.Controls.Add(nudReadBuffer);
            grpSettings.Controls.Add(lblWriteBuffer);
            grpSettings.Controls.Add(nudWriteBuffer);
            grpSettings.Controls.Add(lblTimeout);
            grpSettings.Controls.Add(nudTimeout);
            grpSettings.Controls.Add(chkAsync);
            grpSettings.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            grpSettings.ForeColor = Color.FromArgb(64, 64, 64);
            grpSettings.Location = new Point(400, 10);
            grpSettings.Name = "grpSettings";
            grpSettings.Size = new Size(280, 120);
            grpSettings.TabIndex = 1;
            grpSettings.TabStop = false;
            grpSettings.Text = "性能设置";
            // 
            // lblReadBuffer
            // 
            lblReadBuffer.Location = new Point(15, 25);
            lblReadBuffer.Name = "lblReadBuffer";
            lblReadBuffer.Size = new Size(70, 23);
            lblReadBuffer.TabIndex = 0;
            lblReadBuffer.Text = "读缓冲区:";
            lblReadBuffer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudReadBuffer
            // 
            nudReadBuffer.Increment = new decimal(new int[] { 1024, 0, 0, 0 });
            nudReadBuffer.Location = new Point(90, 25);
            nudReadBuffer.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            nudReadBuffer.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudReadBuffer.Name = "nudReadBuffer";
            nudReadBuffer.Size = new Size(80, 23);
            nudReadBuffer.TabIndex = 1;
            nudReadBuffer.Value = new decimal(new int[] { 4096, 0, 0, 0 });
            // 
            // lblWriteBuffer
            // 
            lblWriteBuffer.Location = new Point(15, 55);
            lblWriteBuffer.Name = "lblWriteBuffer";
            lblWriteBuffer.Size = new Size(70, 23);
            lblWriteBuffer.TabIndex = 2;
            lblWriteBuffer.Text = "写缓冲区:";
            lblWriteBuffer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudWriteBuffer
            // 
            nudWriteBuffer.Increment = new decimal(new int[] { 1024, 0, 0, 0 });
            nudWriteBuffer.Location = new Point(90, 55);
            nudWriteBuffer.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            nudWriteBuffer.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudWriteBuffer.Name = "nudWriteBuffer";
            nudWriteBuffer.Size = new Size(80, 23);
            nudWriteBuffer.TabIndex = 3;
            nudWriteBuffer.Value = new decimal(new int[] { 4096, 0, 0, 0 });
            // 
            // lblTimeout
            // 
            lblTimeout.Location = new Point(15, 85);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(70, 23);
            lblTimeout.TabIndex = 4;
            lblTimeout.Text = "超时(ms):";
            lblTimeout.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudTimeout
            // 
            nudTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nudTimeout.Location = new Point(90, 85);
            nudTimeout.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            nudTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            nudTimeout.Name = "nudTimeout";
            nudTimeout.Size = new Size(80, 23);
            nudTimeout.TabIndex = 5;
            nudTimeout.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // chkAsync
            // 
            chkAsync.Checked = true;
            chkAsync.CheckState = CheckState.Checked;
            chkAsync.Location = new Point(185, 25);
            chkAsync.Name = "chkAsync";
            chkAsync.Size = new Size(85, 23);
            chkAsync.TabIndex = 6;
            chkAsync.Text = "异步通信";
            // 
            // grpData
            // 
            grpData.Controls.Add(txtSend);
            grpData.Controls.Add(btnSend);
            grpData.Controls.Add(btnClear);
            grpData.Controls.Add(rtbReceived);
            grpData.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            grpData.ForeColor = Color.FromArgb(64, 64, 64);
            grpData.Location = new Point(10, 140);
            grpData.Name = "grpData";
            grpData.Size = new Size(670, 280);
            grpData.TabIndex = 2;
            grpData.TabStop = false;
            grpData.Text = "数据收发";
            // 
            // txtSend
            // 
            txtSend.Font = new Font("Consolas", 9F);
            txtSend.Location = new Point(15, 25);
            txtSend.Name = "txtSend";
            txtSend.Size = new Size(480, 22);
            txtSend.TabIndex = 0;
            txtSend.KeyDown += TxtSend_KeyDown;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(33, 150, 243);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Microsoft YaHei", 9F);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(505, 24);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(70, 25);
            btnSend.TabIndex = 1;
            btnSend.Text = "发送";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += BtnSend_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(158, 158, 158);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Microsoft YaHei", 9F);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(585, 24);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(70, 25);
            btnClear.TabIndex = 2;
            btnClear.Text = "清空";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;
            // 
            // rtbReceived
            // 
            rtbReceived.BackColor = Color.FromArgb(250, 250, 250);
            rtbReceived.Font = new Font("Consolas", 9F);
            rtbReceived.Location = new Point(15, 55);
            rtbReceived.Name = "rtbReceived";
            rtbReceived.ReadOnly = true;
            rtbReceived.Size = new Size(640, 210);
            rtbReceived.TabIndex = 3;
            rtbReceived.Text = "";
            // 
            // grpStatus
            // 
            grpStatus.Controls.Add(lblStatus);
            grpStatus.Controls.Add(lblBytesSent);
            grpStatus.Controls.Add(lblBytesReceived);
            grpStatus.Controls.Add(pgbConnection);
            grpStatus.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            grpStatus.ForeColor = Color.FromArgb(64, 64, 64);
            grpStatus.Location = new Point(10, 430);
            grpStatus.Name = "grpStatus";
            grpStatus.Size = new Size(670, 80);
            grpStatus.TabIndex = 3;
            grpStatus.TabStop = false;
            grpStatus.Text = "状态信息";
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(244, 67, 54);
            lblStatus.Location = new Point(15, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(150, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "状态: 未连接";
            // 
            // lblBytesSent
            // 
            lblBytesSent.Location = new Point(180, 25);
            lblBytesSent.Name = "lblBytesSent";
            lblBytesSent.Size = new Size(120, 20);
            lblBytesSent.TabIndex = 1;
            lblBytesSent.Text = "发送: 0 字节";
            // 
            // lblBytesReceived
            // 
            lblBytesReceived.Location = new Point(320, 25);
            lblBytesReceived.Name = "lblBytesReceived";
            lblBytesReceived.Size = new Size(120, 20);
            lblBytesReceived.TabIndex = 2;
            lblBytesReceived.Text = "接收: 0 字节";
            // 
            // pgbConnection
            // 
            pgbConnection.Location = new Point(15, 50);
            pgbConnection.Name = "pgbConnection";
            pgbConnection.Size = new Size(640, 20);
            pgbConnection.Style = ProgressBarStyle.Continuous;
            pgbConnection.TabIndex = 3;
            // 
            // ssMain
            // 
            ssMain.Items.AddRange(new ToolStripItem[] { tsslStatus, tsslTime });
            ssMain.Location = new Point(0, 538);
            ssMain.Name = "ssMain";
            ssMain.Size = new Size(700, 22);
            ssMain.TabIndex = 1;
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(33, 17);
            tsslStatus.Text = "就绪";
            // 
            // tsslTime
            // 
            tsslTime.Name = "tsslTime";
            tsslTime.Size = new Size(110, 17);
            tsslTime.Text = "2025-09-04 21:25:20";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(700, 560);
            Controls.Add(pnlMain);
            Controls.Add(ssMain);
            Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "串口通信监控系统";
            pnlMain.ResumeLayout(false);
            grpConnection.ResumeLayout(false);
            pnlConfig.ResumeLayout(false);
            grpSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudReadBuffer).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteBuffer).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudTimeout).EndInit();
            grpData.ResumeLayout(false);
            grpData.PerformLayout();
            grpStatus.ResumeLayout(false);
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlMain;
        private GroupBox grpConnection;
        private GroupBox grpSettings;
        private GroupBox grpData;
        private GroupBox grpStatus;

        private ComboBox cmbPort;
        private ComboBox cmbBaudRate;
        private ComboBox cmbParity;
        private ComboBox cmbDataBits;
        private ComboBox cmbStopBits;
        private Button btnConnect;
        private Button btnDisconnect;

        private NumericUpDown nudReadBuffer;
        private NumericUpDown nudWriteBuffer;
        private NumericUpDown nudTimeout;
        private CheckBox chkAsync;

        private TextBox txtSend;
        private Button btnSend;
        private Button btnClear;
        private RichTextBox rtbReceived;

        private Label lblStatus;
        private Label lblBytesSent;
        private Label lblBytesReceived;
        private ProgressBar pgbConnection;

        private StatusStrip ssMain;
        private ToolStripStatusLabel tsslStatus;
        private ToolStripStatusLabel tsslTime;
        private Label lblPort;
        private Label lblBaudRate;
        private Label lblParity;
        private Label lblDataBits;
        private Label lblStopBits;
        private Label lblReadBuffer;
        private Label lblWriteBuffer;
        private Label lblTimeout;
        private Panel pnlConfig;
    }
}
