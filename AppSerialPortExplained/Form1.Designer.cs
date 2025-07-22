namespace AppSerialPortExplained
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

        private void InitializeComponent()
        {
            this.groupBoxConfig = new System.Windows.Forms.GroupBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelWriteTimeout = new System.Windows.Forms.Label();
            this.numericUpDownWriteTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelReadTimeout = new System.Windows.Forms.Label();
            this.numericUpDownReadTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelHandshake = new System.Windows.Forms.Label();
            this.comboBoxHandshake = new System.Windows.Forms.ComboBox();
            this.labelStopBits = new System.Windows.Forms.Label();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.labelDataBits = new System.Windows.Forms.Label();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.labelParity = new System.Windows.Forms.Label();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.labelBaud = new System.Windows.Forms.Label();
            this.comboBoxBaud = new System.Windows.Forms.ComboBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.groupBoxReceive = new System.Windows.Forms.GroupBox();
            this.buttonClearReceive = new System.Windows.Forms.Button();
            this.textBoxReceive = new System.Windows.Forms.RichTextBox();
            this.groupBoxSend = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoClear = new System.Windows.Forms.CheckBox();
            this.checkBoxShowSent = new System.Windows.Forms.CheckBox();
            this.checkBoxNewLine = new System.Windows.Forms.CheckBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.labelCdStatus = new System.Windows.Forms.Label();
            this.labelCtsStatus = new System.Windows.Forms.Label();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBoxConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadTimeout)).BeginInit();
            this.groupBoxReceive.SuspendLayout();
            this.groupBoxSend.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxConfig
            // 
            this.groupBoxConfig.Controls.Add(this.buttonRefresh);
            this.groupBoxConfig.Controls.Add(this.labelWriteTimeout);
            this.groupBoxConfig.Controls.Add(this.numericUpDownWriteTimeout);
            this.groupBoxConfig.Controls.Add(this.labelReadTimeout);
            this.groupBoxConfig.Controls.Add(this.numericUpDownReadTimeout);
            this.groupBoxConfig.Controls.Add(this.labelHandshake);
            this.groupBoxConfig.Controls.Add(this.comboBoxHandshake);
            this.groupBoxConfig.Controls.Add(this.labelStopBits);
            this.groupBoxConfig.Controls.Add(this.comboBoxStopBits);
            this.groupBoxConfig.Controls.Add(this.labelDataBits);
            this.groupBoxConfig.Controls.Add(this.comboBoxDataBits);
            this.groupBoxConfig.Controls.Add(this.labelParity);
            this.groupBoxConfig.Controls.Add(this.comboBoxParity);
            this.groupBoxConfig.Controls.Add(this.labelBaud);
            this.groupBoxConfig.Controls.Add(this.comboBoxBaud);
            this.groupBoxConfig.Controls.Add(this.labelPort);
            this.groupBoxConfig.Controls.Add(this.comboBoxPort);
            this.groupBoxConfig.Location = new System.Drawing.Point(12, 12);
            this.groupBoxConfig.Name = "groupBoxConfig";
            this.groupBoxConfig.Size = new System.Drawing.Size(760, 120);
            this.groupBoxConfig.TabIndex = 0;
            this.groupBoxConfig.TabStop = false;
            this.groupBoxConfig.Text = "串口配置";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(165, 19);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(60, 23);
            this.buttonRefresh.TabIndex = 16;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelWriteTimeout
            // 
            this.labelWriteTimeout.AutoSize = true;
            this.labelWriteTimeout.Location = new System.Drawing.Point(550, 52);
            this.labelWriteTimeout.Name = "labelWriteTimeout";
            this.labelWriteTimeout.Size = new System.Drawing.Size(77, 12);
            this.labelWriteTimeout.TabIndex = 15;
            this.labelWriteTimeout.Text = "写超时(ms)：";
            // 
            // numericUpDownWriteTimeout
            // 
            this.numericUpDownWriteTimeout.Location = new System.Drawing.Point(630, 48);
            this.numericUpDownWriteTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownWriteTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWriteTimeout.Name = "numericUpDownWriteTimeout";
            this.numericUpDownWriteTimeout.Size = new System.Drawing.Size(100, 21);
            this.numericUpDownWriteTimeout.TabIndex = 14;
            this.numericUpDownWriteTimeout.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // labelReadTimeout
            // 
            this.labelReadTimeout.AutoSize = true;
            this.labelReadTimeout.Location = new System.Drawing.Point(550, 23);
            this.labelReadTimeout.Name = "labelReadTimeout";
            this.labelReadTimeout.Size = new System.Drawing.Size(77, 12);
            this.labelReadTimeout.TabIndex = 13;
            this.labelReadTimeout.Text = "读超时(ms)：";
            // 
            // numericUpDownReadTimeout
            // 
            this.numericUpDownReadTimeout.Location = new System.Drawing.Point(630, 19);
            this.numericUpDownReadTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownReadTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownReadTimeout.Name = "numericUpDownReadTimeout";
            this.numericUpDownReadTimeout.Size = new System.Drawing.Size(100, 21);
            this.numericUpDownReadTimeout.TabIndex = 12;
            this.numericUpDownReadTimeout.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // labelHandshake
            // 
            this.labelHandshake.AutoSize = true;
            this.labelHandshake.Location = new System.Drawing.Point(375, 52);
            this.labelHandshake.Name = "labelHandshake";
            this.labelHandshake.Size = new System.Drawing.Size(59, 12);
            this.labelHandshake.TabIndex = 11;
            this.labelHandshake.Text = "流控制：";
            // 
            // comboBoxHandshake
            // 
            this.comboBoxHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHandshake.FormattingEnabled = true;
            this.comboBoxHandshake.Location = new System.Drawing.Point(440, 48);
            this.comboBoxHandshake.Name = "comboBoxHandshake";
            this.comboBoxHandshake.Size = new System.Drawing.Size(100, 20);
            this.comboBoxHandshake.TabIndex = 10;
            // 
            // labelStopBits
            // 
            this.labelStopBits.AutoSize = true;
            this.labelStopBits.Location = new System.Drawing.Point(375, 23);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(59, 12);
            this.labelStopBits.TabIndex = 9;
            this.labelStopBits.Text = "停止位：";
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(440, 19);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(100, 20);
            this.comboBoxStopBits.TabIndex = 8;
            // 
            // labelDataBits
            // 
            this.labelDataBits.AutoSize = true;
            this.labelDataBits.Location = new System.Drawing.Point(250, 52);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(59, 12);
            this.labelDataBits.TabIndex = 7;
            this.labelDataBits.Text = "数据位：";
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(315, 48);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(50, 20);
            this.comboBoxDataBits.TabIndex = 6;
            // 
            // labelParity
            // 
            this.labelParity.AutoSize = true;
            this.labelParity.Location = new System.Drawing.Point(250, 23);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(59, 12);
            this.labelParity.TabIndex = 5;
            this.labelParity.Text = "校验位：";
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Location = new System.Drawing.Point(315, 19);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(50, 20);
            this.comboBoxParity.TabIndex = 4;
            // 
            // labelBaud
            // 
            this.labelBaud.AutoSize = true;
            this.labelBaud.Location = new System.Drawing.Point(15, 52);
            this.labelBaud.Name = "labelBaud";
            this.labelBaud.Size = new System.Drawing.Size(59, 12);
            this.labelBaud.TabIndex = 3;
            this.labelBaud.Text = "波特率：";
            // 
            // comboBoxBaud
            // 
            this.comboBoxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaud.FormattingEnabled = true;
            this.comboBoxBaud.Location = new System.Drawing.Point(80, 48);
            this.comboBoxBaud.Name = "comboBoxBaud";
            this.comboBoxBaud.Size = new System.Drawing.Size(80, 20);
            this.comboBoxBaud.TabIndex = 2;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(15, 23);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(59, 12);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "串口号：";
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(80, 19);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(80, 20);
            this.comboBoxPort.TabIndex = 0;
            // 
            // buttonOpen
            // 
            this.buttonOpen.BackColor = System.Drawing.Color.Green;
            this.buttonOpen.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOpen.ForeColor = System.Drawing.Color.White;
            this.buttonOpen.Location = new System.Drawing.Point(790, 25);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(80, 40);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "打开串口";
            this.buttonOpen.UseVisualStyleBackColor = false;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // groupBoxReceive
            // 
            this.groupBoxReceive.Controls.Add(this.buttonClearReceive);
            this.groupBoxReceive.Controls.Add(this.textBoxReceive);
            this.groupBoxReceive.Location = new System.Drawing.Point(12, 140);
            this.groupBoxReceive.Name = "groupBoxReceive";
            this.groupBoxReceive.Size = new System.Drawing.Size(600, 200);
            this.groupBoxReceive.TabIndex = 2;
            this.groupBoxReceive.TabStop = false;
            this.groupBoxReceive.Text = "接收区域";
            // 
            // buttonClearReceive
            // 
            this.buttonClearReceive.Location = new System.Drawing.Point(510, 170);
            this.buttonClearReceive.Name = "buttonClearReceive";
            this.buttonClearReceive.Size = new System.Drawing.Size(75, 23);
            this.buttonClearReceive.TabIndex = 1;
            this.buttonClearReceive.Text = "清空";
            this.buttonClearReceive.UseVisualStyleBackColor = true;
            this.buttonClearReceive.Click += new System.EventHandler(this.buttonClearReceive_Click);
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Location = new System.Drawing.Point(15, 20);
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.Size = new System.Drawing.Size(570, 145);
            this.textBoxReceive.TabIndex = 0;
            this.textBoxReceive.Text = "";
            // 
            // groupBoxSend
            // 
            this.groupBoxSend.Controls.Add(this.checkBoxAutoClear);
            this.groupBoxSend.Controls.Add(this.checkBoxShowSent);
            this.groupBoxSend.Controls.Add(this.checkBoxNewLine);
            this.groupBoxSend.Controls.Add(this.buttonSend);
            this.groupBoxSend.Controls.Add(this.textBoxSend);
            this.groupBoxSend.Enabled = false;
            this.groupBoxSend.Location = new System.Drawing.Point(12, 350);
            this.groupBoxSend.Name = "groupBoxSend";
            this.groupBoxSend.Size = new System.Drawing.Size(600, 80);
            this.groupBoxSend.TabIndex = 3;
            this.groupBoxSend.TabStop = false;
            this.groupBoxSend.Text = "发送区域";
            // 
            // checkBoxAutoClear
            // 
            this.checkBoxAutoClear.AutoSize = true;
            this.checkBoxAutoClear.Location = new System.Drawing.Point(270, 50);
            this.checkBoxAutoClear.Name = "checkBoxAutoClear";
            this.checkBoxAutoClear.Size = new System.Drawing.Size(72, 16);
            this.checkBoxAutoClear.TabIndex = 4;
            this.checkBoxAutoClear.Text = "自动清空";
            this.checkBoxAutoClear.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowSent
            // 
            this.checkBoxShowSent.AutoSize = true;
            this.checkBoxShowSent.Location = new System.Drawing.Point(150, 50);
            this.checkBoxShowSent.Name = "checkBoxShowSent";
            this.checkBoxShowSent.Size = new System.Drawing.Size(96, 16);
            this.checkBoxShowSent.TabIndex = 3;
            this.checkBoxShowSent.Text = "显示发送数据";
            this.checkBoxShowSent.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewLine
            // 
            this.checkBoxNewLine.AutoSize = true;
            this.checkBoxNewLine.Checked = true;
            this.checkBoxNewLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNewLine.Location = new System.Drawing.Point(15, 50);
            this.checkBoxNewLine.Name = "checkBoxNewLine";
            this.checkBoxNewLine.Size = new System.Drawing.Size(108, 16);
            this.checkBoxNewLine.TabIndex = 2;
            this.checkBoxNewLine.Text = "自动添加换行符";
            this.checkBoxNewLine.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(510, 20);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(15, 20);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(480, 21);
            this.textBoxSend.TabIndex = 0;
            this.textBoxSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSend_KeyPress);
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.labelCdStatus);
            this.groupBoxStatus.Controls.Add(this.labelCtsStatus);
            this.groupBoxStatus.Location = new System.Drawing.Point(630, 140);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(240, 80);
            this.groupBoxStatus.TabIndex = 4;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "状态监控";
            // 
            // labelCdStatus
            // 
            this.labelCdStatus.AutoSize = true;
            this.labelCdStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCdStatus.ForeColor = System.Drawing.Color.Gray;
            this.labelCdStatus.Location = new System.Drawing.Point(15, 50);
            this.labelCdStatus.Name = "labelCdStatus";
            this.labelCdStatus.Size = new System.Drawing.Size(33, 12);
            this.labelCdStatus.TabIndex = 1;
            this.labelCdStatus.Text = "CD: -";
            // 
            // labelCtsStatus
            // 
            this.labelCtsStatus.AutoSize = true;
            this.labelCtsStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCtsStatus.ForeColor = System.Drawing.Color.Gray;
            this.labelCtsStatus.Location = new System.Drawing.Point(15, 25);
            this.labelCtsStatus.Name = "labelCtsStatus";
            this.labelCtsStatus.Size = new System.Drawing.Size(40, 12);
            this.labelCtsStatus.TabIndex = 0;
            this.labelCtsStatus.Text = "CTS: -";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.buttonClearLog);
            this.groupBoxLog.Controls.Add(this.textBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(630, 230);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(240, 200);
            this.groupBoxLog.TabIndex = 5;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "日志信息";
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(150, 170);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 1;
            this.buttonClearLog.Text = "清空";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(15, 20);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(210, 145);
            this.textBoxLog.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 441);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.groupBoxSend);
            this.Controls.Add(this.groupBoxReceive);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.groupBoxConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口通信调试助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxConfig.ResumeLayout(false);
            this.groupBoxConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadTimeout)).EndInit();
            this.groupBoxReceive.ResumeLayout(false);
            this.groupBoxSend.ResumeLayout(false);
            this.groupBoxSend.PerformLayout();
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxLog.PerformLayout();
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.GroupBox groupBoxConfig;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelBaud;
        private System.Windows.Forms.ComboBox comboBoxBaud;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label labelHandshake;
        private System.Windows.Forms.ComboBox comboBoxHandshake;
        private System.Windows.Forms.Label labelReadTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownReadTimeout;
        private System.Windows.Forms.Label labelWriteTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownWriteTimeout;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.GroupBox groupBoxReceive;
        private System.Windows.Forms.RichTextBox textBoxReceive;
        private System.Windows.Forms.Button buttonClearReceive;
        private System.Windows.Forms.GroupBox groupBoxSend;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.CheckBox checkBoxNewLine;
        private System.Windows.Forms.CheckBox checkBoxShowSent;
        private System.Windows.Forms.CheckBox checkBoxAutoClear;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label labelCtsStatus;
        private System.Windows.Forms.Label labelCdStatus;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonClearLog;
    }
}
