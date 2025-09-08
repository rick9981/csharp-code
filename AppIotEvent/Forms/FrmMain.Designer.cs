namespace AppIotEvent.Forms
{
    partial class FrmMain
    {
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage deviceTabPage;
        private System.Windows.Forms.TabPage dataTabPage;
        private System.Windows.Forms.TabPage alarmTabPage;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.ListView deviceListView;
        private System.Windows.Forms.ListView dataLogListView;
        private System.Windows.Forms.ListView alarmListView;
        private System.Windows.Forms.ListView systemLogListView;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button startAllButton;
        private System.Windows.Forms.Button stopAllButton;
        private System.Windows.Forms.Button deviceControlButton;
        private System.Windows.Forms.Button clearLogsButton;
        private System.Windows.Forms.NotifyIcon notifyIcon;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));

            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.deviceTabPage = new System.Windows.Forms.TabPage();
            this.dataTabPage = new System.Windows.Forms.TabPage();
            this.alarmTabPage = new System.Windows.Forms.TabPage();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.deviceListView = new System.Windows.Forms.ListView();
            this.dataLogListView = new System.Windows.Forms.ListView();
            this.alarmListView = new System.Windows.Forms.ListView();
            this.systemLogListView = new System.Windows.Forms.ListView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.startAllButton = new System.Windows.Forms.Button();
            this.stopAllButton = new System.Windows.Forms.Button();
            this.deviceControlButton = new System.Windows.Forms.Button();
            this.clearLogsButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            this.mainTabControl.SuspendLayout();
            this.deviceTabPage.SuspendLayout();
            this.dataTabPage.SuspendLayout();
            this.alarmTabPage.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.SuspendLayout();

            // mainTabControl
            this.mainTabControl.Controls.Add(this.deviceTabPage);
            this.mainTabControl.Controls.Add(this.dataTabPage);
            this.mainTabControl.Controls.Add(this.alarmTabPage);
            this.mainTabControl.Controls.Add(this.logTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 80);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1024, 500);
            this.mainTabControl.TabIndex = 0;

            // deviceTabPage
            this.deviceTabPage.Controls.Add(this.deviceListView);
            this.deviceTabPage.Location = new System.Drawing.Point(4, 25);
            this.deviceTabPage.Name = "deviceTabPage";
            this.deviceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.deviceTabPage.Size = new System.Drawing.Size(1016, 471);
            this.deviceTabPage.TabIndex = 0;
            this.deviceTabPage.Text = "设备状态";
            this.deviceTabPage.UseVisualStyleBackColor = true;

            // deviceListView
            this.deviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceListView.FullRowSelect = true;
            this.deviceListView.GridLines = true;
            this.deviceListView.HideSelection = false;
            this.deviceListView.Location = new System.Drawing.Point(3, 3);
            this.deviceListView.MultiSelect = false;
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.Size = new System.Drawing.Size(1010, 465);
            this.deviceListView.TabIndex = 0;
            this.deviceListView.UseCompatibleStateImageBehavior = false;
            this.deviceListView.View = System.Windows.Forms.View.Details;
            this.deviceListView.Columns.Add("设备ID", 120);
            this.deviceListView.Columns.Add("设备名称", 200);
            this.deviceListView.Columns.Add("连接状态", 100);
            this.deviceListView.Columns.Add("运行状态", 100);
            this.deviceListView.Columns.Add("当前值", 150);

            // dataTabPage
            this.dataTabPage.Controls.Add(this.dataLogListView);
            this.dataTabPage.Location = new System.Drawing.Point(4, 25);
            this.dataTabPage.Name = "dataTabPage";
            this.dataTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.dataTabPage.Size = new System.Drawing.Size(1016, 471);
            this.dataTabPage.TabIndex = 1;
            this.dataTabPage.Text = "数据日志";
            this.dataTabPage.UseVisualStyleBackColor = true;

            // dataLogListView
            this.dataLogListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLogListView.FullRowSelect = true;
            this.dataLogListView.GridLines = true;
            this.dataLogListView.HideSelection = false;
            this.dataLogListView.Location = new System.Drawing.Point(3, 3);
            this.dataLogListView.Name = "dataLogListView";
            this.dataLogListView.Size = new System.Drawing.Size(1010, 465);
            this.dataLogListView.TabIndex = 0;
            this.dataLogListView.UseCompatibleStateImageBehavior = false;
            this.dataLogListView.View = System.Windows.Forms.View.Details;
            this.dataLogListView.Columns.Add("时间", 120);
            this.dataLogListView.Columns.Add("设备", 200);
            this.dataLogListView.Columns.Add("数据类型", 120);
            this.dataLogListView.Columns.Add("数值", 150);

            // alarmTabPage
            this.alarmTabPage.Controls.Add(this.alarmListView);
            this.alarmTabPage.Location = new System.Drawing.Point(4, 25);
            this.alarmTabPage.Name = "alarmTabPage";
            this.alarmTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.alarmTabPage.Size = new System.Drawing.Size(1016, 471);
            this.alarmTabPage.TabIndex = 2;
            this.alarmTabPage.Text = "报警信息";
            this.alarmTabPage.UseVisualStyleBackColor = true;

            // alarmListView
            this.alarmListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmListView.FullRowSelect = true;
            this.alarmListView.GridLines = true;
            this.alarmListView.HideSelection = false;
            this.alarmListView.Location = new System.Drawing.Point(3, 3);
            this.alarmListView.Name = "alarmListView";
            this.alarmListView.Size = new System.Drawing.Size(1010, 465);
            this.alarmListView.TabIndex = 0;
            this.alarmListView.UseCompatibleStateImageBehavior = false;
            this.alarmListView.View = System.Windows.Forms.View.Details;
            this.alarmListView.Columns.Add("时间", 120);
            this.alarmListView.Columns.Add("设备", 200);
            this.alarmListView.Columns.Add("级别", 80);
            this.alarmListView.Columns.Add("报警信息", 400);

            // logTabPage
            this.logTabPage.Controls.Add(this.systemLogListView);
            this.logTabPage.Location = new System.Drawing.Point(4, 25);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.logTabPage.Size = new System.Drawing.Size(1016, 471);
            this.logTabPage.TabIndex = 3;
            this.logTabPage.Text = "系统日志";
            this.logTabPage.UseVisualStyleBackColor = true;

            // systemLogListView
            this.systemLogListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemLogListView.FullRowSelect = true;
            this.systemLogListView.GridLines = true;
            this.systemLogListView.HideSelection = false;
            this.systemLogListView.Location = new System.Drawing.Point(3, 3);
            this.systemLogListView.Name = "systemLogListView";
            this.systemLogListView.Size = new System.Drawing.Size(1010, 465);
            this.systemLogListView.TabIndex = 0;
            this.systemLogListView.UseCompatibleStateImageBehavior = false;
            this.systemLogListView.View = System.Windows.Forms.View.Details;
            this.systemLogListView.Columns.Add("时间", 150);
            this.systemLogListView.Columns.Add("级别", 80);
            this.systemLogListView.Columns.Add("消息", 600);

            // controlPanel
            this.controlPanel.Controls.Add(this.startAllButton);
            this.controlPanel.Controls.Add(this.stopAllButton);
            this.controlPanel.Controls.Add(this.deviceControlButton);
            this.controlPanel.Controls.Add(this.clearLogsButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1024, 80);
            this.controlPanel.TabIndex = 1;

            // startAllButton
            this.startAllButton.BackColor = System.Drawing.Color.LightGreen;
            this.startAllButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.startAllButton.Location = new System.Drawing.Point(20, 20);
            this.startAllButton.Name = "startAllButton";
            this.startAllButton.Size = new System.Drawing.Size(120, 40);
            this.startAllButton.TabIndex = 0;
            this.startAllButton.Text = "启动所有设备";
            this.startAllButton.UseVisualStyleBackColor = false;
            this.startAllButton.Click += new System.EventHandler(this.startAllButton_Click);

            // stopAllButton
            this.stopAllButton.BackColor = System.Drawing.Color.LightCoral;
            this.stopAllButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.stopAllButton.Location = new System.Drawing.Point(160, 20);
            this.stopAllButton.Name = "stopAllButton";
            this.stopAllButton.Size = new System.Drawing.Size(120, 40);
            this.stopAllButton.TabIndex = 1;
            this.stopAllButton.Text = "停止所有设备";
            this.stopAllButton.UseVisualStyleBackColor = false;
            this.stopAllButton.Click += new System.EventHandler(this.stopAllButton_Click);

            // deviceControlButton
            this.deviceControlButton.BackColor = System.Drawing.Color.LightBlue;
            this.deviceControlButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.deviceControlButton.Location = new System.Drawing.Point(300, 20);
            this.deviceControlButton.Name = "deviceControlButton";
            this.deviceControlButton.Size = new System.Drawing.Size(120, 40);
            this.deviceControlButton.TabIndex = 2;
            this.deviceControlButton.Text = "设备控制";
            this.deviceControlButton.UseVisualStyleBackColor = false;
            this.deviceControlButton.Click += new System.EventHandler(this.deviceControlButton_Click);

            // clearLogsButton
            this.clearLogsButton.BackColor = System.Drawing.Color.LightYellow;
            this.clearLogsButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.clearLogsButton.Location = new System.Drawing.Point(440, 20);
            this.clearLogsButton.Name = "clearLogsButton";
            this.clearLogsButton.Size = new System.Drawing.Size(120, 40);
            this.clearLogsButton.TabIndex = 3;
            this.clearLogsButton.Text = "清空日志";
            this.clearLogsButton.UseVisualStyleBackColor = false;
            this.clearLogsButton.Click += new System.EventHandler(this.clearLogsButton_Click);

            // statusStrip
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });
            this.statusStrip.Location = new System.Drawing.Point(0, 580);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 26);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";

            // statusLabel
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 20);
            this.statusLabel.Text = "系统就绪";

            // notifyIcon
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "IoT事件驱动监控系统";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 606);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IoT事件驱动监控系统 v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);

            this.mainTabControl.ResumeLayout(false);
            this.deviceTabPage.ResumeLayout(false);
            this.dataTabPage.ResumeLayout(false);
            this.alarmTabPage.ResumeLayout(false);
            this.logTabPage.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}