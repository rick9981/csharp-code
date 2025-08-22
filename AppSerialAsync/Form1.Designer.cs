namespace AppSerialAsync
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
            gbConnection = new GroupBox();
            btnRefreshPorts = new Button();
            lblStatus = new Label();
            btnConnect = new Button();
            cmbBaudRate = new ComboBox();
            cmbPorts = new ComboBox();
            lblBaudRate = new Label();
            lblPort = new Label();
            gbSend = new GroupBox();
            rbSendHex = new RadioButton();
            rbSendPacket = new RadioButton();
            rbSendText = new RadioButton();
            btnSend = new Button();
            txtSend = new TextBox();
            gbReceive = new GroupBox();
            btnClearReceived = new Button();
            txtReceived = new TextBox();
            gbLog = new GroupBox();
            btnClearLog = new Button();
            txtLog = new TextBox();
            statusStrip = new StatusStrip();
            lblTxCount = new ToolStripStatusLabel();
            lblRxCount = new ToolStripStatusLabel();
            gbConnection.SuspendLayout();
            gbSend.SuspendLayout();
            gbReceive.SuspendLayout();
            gbLog.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // gbConnection
            // 
            gbConnection.Controls.Add(btnRefreshPorts);
            gbConnection.Controls.Add(lblStatus);
            gbConnection.Controls.Add(btnConnect);
            gbConnection.Controls.Add(cmbBaudRate);
            gbConnection.Controls.Add(cmbPorts);
            gbConnection.Controls.Add(lblBaudRate);
            gbConnection.Controls.Add(lblPort);
            gbConnection.Location = new Point(14, 15);
            gbConnection.Margin = new Padding(4, 4, 4, 4);
            gbConnection.Name = "gbConnection";
            gbConnection.Padding = new Padding(4, 4, 4, 4);
            gbConnection.Size = new Size(933, 100);
            gbConnection.TabIndex = 0;
            gbConnection.TabStop = false;
            gbConnection.Text = "串口连接";
            // 
            // btnRefreshPorts
            // 
            btnRefreshPorts.Location = new Point(210, 25);
            btnRefreshPorts.Margin = new Padding(4, 4, 4, 4);
            btnRefreshPorts.Name = "btnRefreshPorts";
            btnRefreshPorts.Size = new Size(88, 29);
            btnRefreshPorts.TabIndex = 6;
            btnRefreshPorts.Text = "刷新端口";
            btnRefreshPorts.UseVisualStyleBackColor = true;
            btnRefreshPorts.Click += btnRefreshPorts_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(607, 31);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(78, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "状态: 未连接";
            // 
            // btnConnect
            // 
            btnConnect.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            btnConnect.Location = new Point(490, 25);
            btnConnect.Margin = new Padding(4, 4, 4, 4);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(105, 26);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "连接串口";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // cmbBaudRate
            // 
            cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBaudRate.FormattingEnabled = true;
            cmbBaudRate.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
            cmbBaudRate.Location = new Point(373, 28);
            cmbBaudRate.Margin = new Padding(4, 4, 4, 4);
            cmbBaudRate.Name = "cmbBaudRate";
            cmbBaudRate.Size = new Size(93, 23);
            cmbBaudRate.TabIndex = 3;
            // 
            // cmbPorts
            // 
            cmbPorts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPorts.FormattingEnabled = true;
            cmbPorts.Location = new Point(76, 28);
            cmbPorts.Margin = new Padding(4, 4, 4, 4);
            cmbPorts.Name = "cmbPorts";
            cmbPorts.Size = new Size(116, 23);
            cmbPorts.TabIndex = 2;
            // 
            // lblBaudRate
            // 
            lblBaudRate.AutoSize = true;
            lblBaudRate.Location = new Point(315, 31);
            lblBaudRate.Margin = new Padding(4, 0, 4, 0);
            lblBaudRate.Name = "lblBaudRate";
            lblBaudRate.Size = new Size(49, 15);
            lblBaudRate.TabIndex = 1;
            lblBaudRate.Text = "波特率:";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(23, 31);
            lblPort.Margin = new Padding(4, 0, 4, 0);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(49, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "端口号:";
            // 
            // gbSend
            // 
            gbSend.Controls.Add(rbSendHex);
            gbSend.Controls.Add(rbSendPacket);
            gbSend.Controls.Add(rbSendText);
            gbSend.Controls.Add(btnSend);
            gbSend.Controls.Add(txtSend);
            gbSend.Location = new Point(14, 122);
            gbSend.Margin = new Padding(4, 4, 4, 4);
            gbSend.Name = "gbSend";
            gbSend.Padding = new Padding(4, 4, 4, 4);
            gbSend.Size = new Size(933, 150);
            gbSend.TabIndex = 1;
            gbSend.TabStop = false;
            gbSend.Text = "数据发送";
            // 
            // rbSendHex
            // 
            rbSendHex.AutoSize = true;
            rbSendHex.Location = new Point(233, 112);
            rbSendHex.Margin = new Padding(4, 4, 4, 4);
            rbSendHex.Name = "rbSendHex";
            rbSendHex.Size = new Size(77, 19);
            rbSendHex.TabIndex = 4;
            rbSendHex.Text = "十六进制";
            rbSendHex.UseVisualStyleBackColor = true;
            // 
            // rbSendPacket
            // 
            rbSendPacket.AutoSize = true;
            rbSendPacket.Location = new Point(128, 112);
            rbSendPacket.Margin = new Padding(4, 4, 4, 4);
            rbSendPacket.Name = "rbSendPacket";
            rbSendPacket.Size = new Size(64, 19);
            rbSendPacket.TabIndex = 3;
            rbSendPacket.Text = "数据包";
            rbSendPacket.UseVisualStyleBackColor = true;
            // 
            // rbSendText
            // 
            rbSendText.AutoSize = true;
            rbSendText.Checked = true;
            rbSendText.Location = new Point(23, 112);
            rbSendText.Margin = new Padding(4, 4, 4, 4);
            rbSendText.Name = "rbSendText";
            rbSendText.Size = new Size(51, 19);
            rbSendText.TabIndex = 2;
            rbSendText.TabStop = true;
            rbSendText.Text = "文本";
            rbSendText.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            btnSend.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
            btnSend.Location = new Point(817, 25);
            btnSend.Margin = new Padding(4, 4, 4, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(93, 75);
            btnSend.TabIndex = 1;
            btnSend.Text = "发送";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtSend
            // 
            txtSend.Location = new Point(23, 25);
            txtSend.Margin = new Padding(4, 4, 4, 4);
            txtSend.Multiline = true;
            txtSend.Name = "txtSend";
            txtSend.ScrollBars = ScrollBars.Both;
            txtSend.Size = new Size(781, 74);
            txtSend.TabIndex = 0;
            // 
            // gbReceive
            // 
            gbReceive.Controls.Add(btnClearReceived);
            gbReceive.Controls.Add(txtReceived);
            gbReceive.Location = new Point(14, 280);
            gbReceive.Margin = new Padding(4, 4, 4, 4);
            gbReceive.Name = "gbReceive";
            gbReceive.Padding = new Padding(4, 4, 4, 4);
            gbReceive.Size = new Size(933, 250);
            gbReceive.TabIndex = 2;
            gbReceive.TabStop = false;
            gbReceive.Text = "数据接收";
            // 
            // btnClearReceived
            // 
            btnClearReceived.Location = new Point(817, 25);
            btnClearReceived.Margin = new Padding(4, 4, 4, 4);
            btnClearReceived.Name = "btnClearReceived";
            btnClearReceived.Size = new Size(93, 38);
            btnClearReceived.TabIndex = 1;
            btnClearReceived.Text = "清空";
            btnClearReceived.UseVisualStyleBackColor = true;
            btnClearReceived.Click += btnClearReceived_Click;
            // 
            // txtReceived
            // 
            txtReceived.Font = new Font("Consolas", 9F);
            txtReceived.Location = new Point(23, 25);
            txtReceived.Margin = new Padding(4, 4, 4, 4);
            txtReceived.Multiline = true;
            txtReceived.Name = "txtReceived";
            txtReceived.ReadOnly = true;
            txtReceived.ScrollBars = ScrollBars.Both;
            txtReceived.Size = new Size(781, 212);
            txtReceived.TabIndex = 0;
            // 
            // gbLog
            // 
            gbLog.Controls.Add(btnClearLog);
            gbLog.Controls.Add(txtLog);
            gbLog.Location = new Point(14, 538);
            gbLog.Margin = new Padding(4, 4, 4, 4);
            gbLog.Name = "gbLog";
            gbLog.Padding = new Padding(4, 4, 4, 4);
            gbLog.Size = new Size(933, 188);
            gbLog.TabIndex = 3;
            gbLog.TabStop = false;
            gbLog.Text = "系统日志";
            // 
            // btnClearLog
            // 
            btnClearLog.Location = new Point(817, 25);
            btnClearLog.Margin = new Padding(4, 4, 4, 4);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(93, 38);
            btnClearLog.TabIndex = 1;
            btnClearLog.Text = "清空";
            btnClearLog.UseVisualStyleBackColor = true;
            btnClearLog.Click += btnClearLog_Click;
            // 
            // txtLog
            // 
            txtLog.Font = new Font("Consolas", 8F);
            txtLog.Location = new Point(23, 25);
            txtLog.Margin = new Padding(4, 4, 4, 4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(781, 149);
            txtLog.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblTxCount, lblRxCount });
            statusStrip.Location = new Point(0, 743);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(961, 22);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip1";
            // 
            // lblTxCount
            // 
            lblTxCount.Name = "lblTxCount";
            lblTxCount.Size = new Size(45, 17);
            lblTxCount.Text = "发送: 0";
            // 
            // lblRxCount
            // 
            lblRxCount.Name = "lblRxCount";
            lblRxCount.Size = new Size(45, 17);
            lblRxCount.Text = "接收: 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 765);
            Controls.Add(statusStrip);
            Controls.Add(gbLog);
            Controls.Add(gbReceive);
            Controls.Add(gbSend);
            Controls.Add(gbConnection);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C# 高级串口通信工具";
            gbConnection.ResumeLayout(false);
            gbConnection.PerformLayout();
            gbSend.ResumeLayout(false);
            gbSend.PerformLayout();
            gbReceive.ResumeLayout(false);
            gbReceive.PerformLayout();
            gbLog.ResumeLayout(false);
            gbLog.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.ComboBox cmbPorts;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RadioButton rbSendText;
        private System.Windows.Forms.RadioButton rbSendPacket;
        private System.Windows.Forms.RadioButton rbSendHex;
        private System.Windows.Forms.GroupBox gbReceive;
        private System.Windows.Forms.TextBox txtReceived;
        private System.Windows.Forms.Button btnClearReceived;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblTxCount;
        private System.Windows.Forms.ToolStripStatusLabel lblRxCount;
    }
}
