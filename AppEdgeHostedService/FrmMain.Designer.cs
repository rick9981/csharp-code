namespace AppEdgeHostedService.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        // 主要控件
        private TableLayoutPanel tlpMain;
        private Panel pnlHeader;
        private Panel pnlStatus;
        private Panel pnlControls;
        private Panel pnlContent;
        private TabControl tabMain;
        private TabPage tpDataCollection;
        private TabPage tpSystemLog;
        private DataGridView dgvData;
        private RichTextBox rtbLog;

        // 状态控件
        private GroupBox grpConnectionStatus;
        private Label lblConnectionStatus;
        private Label lblCollectionStatus;

        // 系统信息控件
        private GroupBox grpSystemInfo;
        private Label lblMemoryUsage;
        private Label lblCpuUsage;
        private Label lblUptime;
        private Label lblTotalRecords;
        private Label lblBufferCount;
        private Label lblDisplayRecords;

        // 控制按钮
        private Button btnStart;
        private Button btnStop;
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnClearLog;
        private Button btnClearData;
        private Button btnExportData;
        private Button btnSettings;

        // 进度条
        private ProgressBar pgbProgress;

        // 标签
        private Label lblTitle;
        private Label lblVersion;

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
            this.SuspendLayout();

            // 主窗体设置
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1400, 900);
            this.Text = "工业边缘采集系统 v1.0";
            this.MinimumSize = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // 创建主要布局
            this.tlpMain = new TableLayoutPanel();
            this.tlpMain.Dock = DockStyle.Fill;
            this.tlpMain.RowCount = 3;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            this.tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            this.tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 标题面板
            this.pnlHeader = new Panel();
            this.pnlHeader.Dock = DockStyle.Fill;
            this.pnlHeader.BackColor = Color.FromArgb(51, 51, 51);
            this.pnlHeader.Margin = new Padding(0);

            this.lblTitle = new Label();
            this.lblTitle.Text = "工业边缘采集系统";
            this.lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.AutoSize = true;

            this.lblVersion = new Label();
            this.lblVersion.Text = "Version 1.0.0";
            this.lblVersion.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblVersion.ForeColor = Color.FromArgb(200, 200, 200);
            this.lblVersion.Location = new Point(20, 50);
            this.lblVersion.AutoSize = true;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblVersion);

            // 状态和控制面板
            this.pnlStatus = new Panel();
            this.pnlStatus.Dock = DockStyle.Fill;
            this.pnlStatus.BackColor = Color.FromArgb(250, 250, 250);
            this.pnlStatus.Margin = new Padding(5, 5, 5, 0);

            // 连接状态组
            this.grpConnectionStatus = new GroupBox();
            this.grpConnectionStatus.Text = "连接状态";
            this.grpConnectionStatus.Location = new Point(10, 10);
            this.grpConnectionStatus.Size = new Size(200, 100);
            this.grpConnectionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpConnectionStatus.ForeColor = Color.FromArgb(64, 64, 64);

            this.lblConnectionStatus = new Label();
            this.lblConnectionStatus.Text = "未连接";
            this.lblConnectionStatus.Location = new Point(15, 25);
            this.lblConnectionStatus.Size = new Size(170, 20);
            this.lblConnectionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = Color.Red;

            this.lblCollectionStatus = new Label();
            this.lblCollectionStatus.Text = "已停止";
            this.lblCollectionStatus.Location = new Point(15, 50);
            this.lblCollectionStatus.Size = new Size(170, 20);
            this.lblCollectionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCollectionStatus.ForeColor = Color.Red;

            this.grpConnectionStatus.Controls.Add(this.lblConnectionStatus);
            this.grpConnectionStatus.Controls.Add(this.lblCollectionStatus);

            // 系统信息组
            this.grpSystemInfo = new GroupBox();
            this.grpSystemInfo.Text = "系统信息";
            this.grpSystemInfo.Location = new Point(220, 10);
            this.grpSystemInfo.Size = new Size(400, 100);
            this.grpSystemInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpSystemInfo.ForeColor = Color.FromArgb(64, 64, 64);

            this.lblMemoryUsage = new Label();
            this.lblMemoryUsage.Text = "内存: 0 MB";
            this.lblMemoryUsage.Location = new Point(15, 25);
            this.lblMemoryUsage.Size = new Size(120, 20);

            this.lblCpuUsage = new Label();
            this.lblCpuUsage.Text = "CPU: 0%";
            this.lblCpuUsage.Location = new Point(140, 25);
            this.lblCpuUsage.Size = new Size(120, 20);

            this.lblUptime = new Label();
            this.lblUptime.Text = "运行: 00:00:00";
            this.lblUptime.Location = new Point(265, 25);
            this.lblUptime.Size = new Size(120, 20);

            this.lblTotalRecords = new Label();
            this.lblTotalRecords.Text = "总记录: 0";
            this.lblTotalRecords.Location = new Point(15, 50);
            this.lblTotalRecords.Size = new Size(120, 20);

            this.lblBufferCount = new Label();
            this.lblBufferCount.Text = "缓冲区: 0";
            this.lblBufferCount.Location = new Point(140, 50);
            this.lblBufferCount.Size = new Size(120, 20);

            this.lblDisplayRecords = new Label();
            this.lblDisplayRecords.Text = "显示: 0";
            this.lblDisplayRecords.Location = new Point(265, 50);
            this.lblDisplayRecords.Size = new Size(120, 20);

            this.grpSystemInfo.Controls.Add(this.lblMemoryUsage);
            this.grpSystemInfo.Controls.Add(this.lblCpuUsage);
            this.grpSystemInfo.Controls.Add(this.lblUptime);
            this.grpSystemInfo.Controls.Add(this.lblTotalRecords);
            this.grpSystemInfo.Controls.Add(this.lblBufferCount);
            this.grpSystemInfo.Controls.Add(this.lblDisplayRecords);

            // 控制按钮
            this.btnConnect = CreateStyledButton("连接设备", new Point(630, 20), Color.FromArgb(40, 167, 69));
            this.btnDisconnect = CreateStyledButton("断开连接", new Point(730, 20), Color.FromArgb(220, 53, 69));
            this.btnStart = CreateStyledButton("启动采集", new Point(630, 60), Color.FromArgb(0, 123, 255));
            this.btnStop = CreateStyledButton("停止采集", new Point(730, 60), Color.FromArgb(108, 117, 125));
            this.btnClearData = CreateStyledButton("清空数据", new Point(830, 20), Color.FromArgb(255, 193, 7));
            this.btnExportData = CreateStyledButton("导出数据", new Point(930, 20), Color.FromArgb(23, 162, 184));
            this.btnClearLog = CreateStyledButton("清空日志", new Point(830, 60), Color.FromArgb(255, 193, 7));
            this.btnSettings = CreateStyledButton("系统设置", new Point(930, 60), Color.FromArgb(108, 117, 125));

            // 进度条
            this.pgbProgress = new ProgressBar();
            this.pgbProgress.Location = new Point(1040, 45);
            this.pgbProgress.Size = new Size(300, 20);
            this.pgbProgress.Style = ProgressBarStyle.Blocks;

            // 初始按钮状态
            this.btnDisconnect.Enabled = false;
            this.btnStop.Enabled = false;

            // 添加事件处理
            this.btnConnect.Click += btnConnect_Click;
            this.btnDisconnect.Click += btnDisconnect_Click;
            this.btnStart.Click += btnStart_Click;
            this.btnStop.Click += btnStop_Click;
            this.btnClearData.Click += btnClearData_Click;
            this.btnExportData.Click += btnExportData_Click;
            this.btnClearLog.Click += btnClearLog_Click;

            this.pnlStatus.Controls.Add(this.grpConnectionStatus);
            this.pnlStatus.Controls.Add(this.grpSystemInfo);
            this.pnlStatus.Controls.Add(this.btnConnect);
            this.pnlStatus.Controls.Add(this.btnDisconnect);
            this.pnlStatus.Controls.Add(this.btnStart);
            this.pnlStatus.Controls.Add(this.btnStop);
            this.pnlStatus.Controls.Add(this.btnClearData);
            this.pnlStatus.Controls.Add(this.btnExportData);
            this.pnlStatus.Controls.Add(this.btnClearLog);
            this.pnlStatus.Controls.Add(this.btnSettings);
            this.pnlStatus.Controls.Add(this.pgbProgress);

            // 主内容区域
            this.pnlContent = new Panel();
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.Margin = new Padding(5, 0, 5, 5);

            // 选项卡控件
            this.tabMain = new TabControl();
            this.tabMain.Dock = DockStyle.Fill;
            this.tabMain.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // 数据采集选项卡
            this.tpDataCollection = new TabPage();
            this.tpDataCollection.Text = "数据采集";
            this.tpDataCollection.BackColor = Color.White;
            this.tpDataCollection.Margin = new Padding(0);

            this.dgvData = new DataGridView();
            this.dgvData.Dock = DockStyle.Fill;
            this.dgvData.BackgroundColor = Color.White;
            this.dgvData.BorderStyle = BorderStyle.None;
            this.dgvData.Margin = new Padding(10);

            // 添加列
            this.dgvData.Columns.Add("Timestamp", "时间戳");
            this.dgvData.Columns.Add("Address", "地址");
            this.dgvData.Columns.Add("Value", "数值");
            this.dgvData.Columns.Add("DataType", "数据类型");
            this.dgvData.Columns.Add("Status", "状态");
            this.dgvData.Columns.Add("Quality", "质量");

            // 设置列宽
            this.dgvData.Columns[0].Width = 180; // 时间戳
            this.dgvData.Columns[1].Width = 80;  // 地址
            this.dgvData.Columns[2].Width = 150; // 数值
            this.dgvData.Columns[3].Width = 120; // 数据类型
            this.dgvData.Columns[4].Width = 100; // 状态
            this.dgvData.Columns[5].Width = 100; // 质量

            this.tpDataCollection.Controls.Add(this.dgvData);

            // 系统日志选项卡
            this.tpSystemLog = new TabPage();
            this.tpSystemLog.Text = "系统日志";
            this.tpSystemLog.BackColor = Color.White;
            this.tpSystemLog.Margin = new Padding(0);

            this.rtbLog = new RichTextBox();
            this.rtbLog.Dock = DockStyle.Fill;
            this.rtbLog.BackColor = Color.FromArgb(250, 250, 250);
            this.rtbLog.Font = new Font("Consolas", 9F, FontStyle.Regular);
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Margin = new Padding(10);
            this.rtbLog.BorderStyle = BorderStyle.None;

            this.tpSystemLog.Controls.Add(this.rtbLog);

            this.tabMain.TabPages.Add(this.tpDataCollection);
            this.tabMain.TabPages.Add(this.tpSystemLog);
            this.pnlContent.Controls.Add(this.tabMain);

            // 将所有面板添加到主布局
            this.tlpMain.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpMain.Controls.Add(this.pnlStatus, 0, 1);
            this.tlpMain.Controls.Add(this.pnlContent, 0, 2);

            this.Controls.Add(this.tlpMain);
            this.ResumeLayout(false);
        }

        private Button CreateStyledButton(string text, Point location, Color backgroundColor)
        {
            var button = new Button();
            button.Text = text;
            button.Location = location;
            button.Size = new Size(90, 30);
            button.BackColor = backgroundColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;

            // 悬停效果
            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(backgroundColor, 0.2f);
            };
            button.MouseLeave += (s, e) =>
            {
                button.BackColor = backgroundColor;
            };

            return button;
        }
    }
}