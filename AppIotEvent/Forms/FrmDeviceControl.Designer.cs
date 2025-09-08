namespace AppIotEvent.Forms
{
    partial class FrmDeviceControl
    {
        private System.Windows.Forms.GroupBox deviceInfoGroupBox;
        private System.Windows.Forms.Label deviceIdTitleLabel;
        private System.Windows.Forms.Label deviceIdLabel;
        private System.Windows.Forms.Label deviceNameTitleLabel;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.Label connectionStatusTitleLabel;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.Label runningStatusTitleLabel;
        private System.Windows.Forms.Label runningStatusLabel;
        private System.Windows.Forms.GroupBox controlGroupBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button closeButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deviceInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.deviceIdTitleLabel = new System.Windows.Forms.Label();
            this.deviceIdLabel = new System.Windows.Forms.Label();
            this.deviceNameTitleLabel = new System.Windows.Forms.Label();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.connectionStatusTitleLabel = new System.Windows.Forms.Label();
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.runningStatusTitleLabel = new System.Windows.Forms.Label();
            this.runningStatusLabel = new System.Windows.Forms.Label();
            this.controlGroupBox = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.deviceInfoGroupBox.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            this.controlGroupBox.SuspendLayout();
            this.SuspendLayout();

            // deviceInfoGroupBox
            this.deviceInfoGroupBox.Controls.Add(this.deviceNameLabel);
            this.deviceInfoGroupBox.Controls.Add(this.deviceNameTitleLabel);
            this.deviceInfoGroupBox.Controls.Add(this.deviceIdLabel);
            this.deviceInfoGroupBox.Controls.Add(this.deviceIdTitleLabel);
            this.deviceInfoGroupBox.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.deviceInfoGroupBox.Location = new System.Drawing.Point(20, 20);
            this.deviceInfoGroupBox.Name = "deviceInfoGroupBox";
            this.deviceInfoGroupBox.Size = new System.Drawing.Size(440, 100);
            this.deviceInfoGroupBox.TabIndex = 0;
            this.deviceInfoGroupBox.TabStop = false;
            this.deviceInfoGroupBox.Text = "设备信息";

            // deviceIdTitleLabel
            this.deviceIdTitleLabel.AutoSize = true;
            this.deviceIdTitleLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.deviceIdTitleLabel.Location = new System.Drawing.Point(20, 30);
            this.deviceIdTitleLabel.Name = "deviceIdTitleLabel";
            this.deviceIdTitleLabel.Size = new System.Drawing.Size(69, 20);
            this.deviceIdTitleLabel.TabIndex = 0;
            this.deviceIdTitleLabel.Text = "设备ID：";

            // deviceIdLabel
            this.deviceIdLabel.AutoSize = true;
            this.deviceIdLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.deviceIdLabel.Location = new System.Drawing.Point(100, 30);
            this.deviceIdLabel.Name = "deviceIdLabel";
            this.deviceIdLabel.Size = new System.Drawing.Size(0, 20);
            this.deviceIdLabel.TabIndex = 1;

            // deviceNameTitleLabel
            this.deviceNameTitleLabel.AutoSize = true;
            this.deviceNameTitleLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.deviceNameTitleLabel.Location = new System.Drawing.Point(20, 60);
            this.deviceNameTitleLabel.Name = "deviceNameTitleLabel";
            this.deviceNameTitleLabel.Size = new System.Drawing.Size(84, 20);
            this.deviceNameTitleLabel.TabIndex = 2;
            this.deviceNameTitleLabel.Text = "设备名称：";

            // deviceNameLabel
            this.deviceNameLabel.AutoSize = true;
            this.deviceNameLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.deviceNameLabel.Location = new System.Drawing.Point(120, 60);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(0, 20);
            this.deviceNameLabel.TabIndex = 3;

            // statusGroupBox
            this.statusGroupBox.Controls.Add(this.runningStatusLabel);
            this.statusGroupBox.Controls.Add(this.runningStatusTitleLabel);
            this.statusGroupBox.Controls.Add(this.connectionStatusLabel);
            this.statusGroupBox.Controls.Add(this.connectionStatusTitleLabel);
            this.statusGroupBox.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.statusGroupBox.Location = new System.Drawing.Point(20, 140);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(440, 100);
            this.statusGroupBox.TabIndex = 1;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "设备状态";

            // connectionStatusTitleLabel
            this.connectionStatusTitleLabel.AutoSize = true;
            this.connectionStatusTitleLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.connectionStatusTitleLabel.Location = new System.Drawing.Point(20, 30);
            this.connectionStatusTitleLabel.Name = "connectionStatusTitleLabel";
            this.connectionStatusTitleLabel.Size = new System.Drawing.Size(84, 20);
            this.connectionStatusTitleLabel.TabIndex = 0;
            this.connectionStatusTitleLabel.Text = "连接状态：";

            // connectionStatusLabel
            this.connectionStatusLabel.AutoSize = true;
            this.connectionStatusLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.connectionStatusLabel.Location = new System.Drawing.Point(120, 30);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(0, 20);
            this.connectionStatusLabel.TabIndex = 1;

            // runningStatusTitleLabel
            this.runningStatusTitleLabel.AutoSize = true;
            this.runningStatusTitleLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.runningStatusTitleLabel.Location = new System.Drawing.Point(20, 60);
            this.runningStatusTitleLabel.Name = "runningStatusTitleLabel";
            this.runningStatusTitleLabel.Size = new System.Drawing.Size(84, 20);
            this.runningStatusTitleLabel.TabIndex = 2;
            this.runningStatusTitleLabel.Text = "运行状态：";

            // runningStatusLabel
            this.runningStatusLabel.AutoSize = true;
            this.runningStatusLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.runningStatusLabel.Location = new System.Drawing.Point(120, 60);
            this.runningStatusLabel.Name = "runningStatusLabel";
            this.runningStatusLabel.Size = new System.Drawing.Size(0, 20);
            this.runningStatusLabel.TabIndex = 3;

            // controlGroupBox
            this.controlGroupBox.Controls.Add(this.refreshButton);
            this.controlGroupBox.Controls.Add(this.stopButton);
            this.controlGroupBox.Controls.Add(this.startButton);
            this.controlGroupBox.Controls.Add(this.disconnectButton);
            this.controlGroupBox.Controls.Add(this.connectButton);
            this.controlGroupBox.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.controlGroupBox.Location = new System.Drawing.Point(20, 260);
            this.controlGroupBox.Name = "controlGroupBox";
            this.controlGroupBox.Size = new System.Drawing.Size(440, 120);
            this.controlGroupBox.TabIndex = 2;
            this.controlGroupBox.TabStop = false;
            this.controlGroupBox.Text = "设备控制";

            // connectButton
            this.connectButton.BackColor = System.Drawing.Color.LightGreen;
            this.connectButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.connectButton.Location = new System.Drawing.Point(20, 30);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(80, 35);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "连接";
            this.connectButton.UseVisualStyleBackColor = false;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);

            // disconnectButton
            this.disconnectButton.BackColor = System.Drawing.Color.LightCoral;
            this.disconnectButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.disconnectButton.Location = new System.Drawing.Point(120, 30);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(80, 35);
            this.disconnectButton.TabIndex = 1;
            this.disconnectButton.Text = "断开";
            this.disconnectButton.UseVisualStyleBackColor = false;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);

            // startButton
            this.startButton.BackColor = System.Drawing.Color.LightBlue;
            this.startButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.startButton.Location = new System.Drawing.Point(220, 30);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 35);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "启动";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);

            // stopButton
            this.stopButton.BackColor = System.Drawing.Color.LightYellow;
            this.stopButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.stopButton.Location = new System.Drawing.Point(320, 30);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 35);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "停止";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);

            // refreshButton
            this.refreshButton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.refreshButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.refreshButton.Location = new System.Drawing.Point(20, 75);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(100, 35);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "刷新状态";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);

            // closeButton
            this.closeButton.BackColor = System.Drawing.Color.Gainsboro;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.closeButton.Location = new System.Drawing.Point(380, 400);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 35);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "关闭";
            this.closeButton.UseVisualStyleBackColor = false;

            // DeviceControlForm
            this.AcceptButton = this.refreshButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(480, 450);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.controlGroupBox);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.deviceInfoGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceControlForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设备控制";
            this.deviceInfoGroupBox.ResumeLayout(false);
            this.deviceInfoGroupBox.PerformLayout();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.controlGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}