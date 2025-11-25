using Timer = System.Windows.Forms.Timer;

namespace AppNetworkFileTransfer
{
    partial class FrmMain
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
            components = new System.ComponentModel.Container();
            tlpMain = new TableLayoutPanel();
            grpConnection = new GroupBox();
            tlpConnection = new TableLayoutPanel();
            lblMode = new Label();
            cmbMode = new ComboBox();
            lblPort = new Label();
            nudPort = new NumericUpDown();
            lblServerIP = new Label();
            txtServerIP = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            lblStatus = new Label();
            lblStatusValue = new Label();
            grpServerSettings = new GroupBox();
            tlpServerSettings = new TableLayoutPanel();
            lblSaveDirectory = new Label();
            txtSaveDirectory = new TextBox();
            btnBrowseSaveDirectory = new Button();
            grpClientSettings = new GroupBox();
            tlpClientSettings = new TableLayoutPanel();
            lblLocalPath = new Label();
            txtLocalPath = new TextBox();
            btnBrowseLocal = new Button();
            btnSendFile = new Button();
            grpProgress = new GroupBox();
            tlpProgress = new TableLayoutPanel();
            lblCurrentFile = new Label();
            lblCurrentFileValue = new Label();
            pgbFileProgress = new ProgressBar();
            lblProgressPercent = new Label();
            lblSpeed = new Label();
            lblSpeedValue = new Label();
            lblTimeRemaining = new Label();
            lblTimeRemainingValue = new Label();
            grpLog = new GroupBox();
            rtbLog = new RichTextBox();
            cmsLog = new ContextMenuStrip(components);
            tsmiClearLog = new ToolStripMenuItem();
            tsmiSaveLog = new ToolStripMenuItem();
            ssMain = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            tsslSeparator = new ToolStripStatusLabel();
            tsslTime = new ToolStripStatusLabel();
            tmrUpdate = new Timer(components);
            ofdSelectFile = new OpenFileDialog();
            sfdSaveFile = new SaveFileDialog();
            fbdSaveDirectory = new FolderBrowserDialog();
            tlpMain.SuspendLayout();
            grpConnection.SuspendLayout();
            tlpConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            grpServerSettings.SuspendLayout();
            tlpServerSettings.SuspendLayout();
            grpClientSettings.SuspendLayout();
            tlpClientSettings.SuspendLayout();
            grpProgress.SuspendLayout();
            tlpProgress.SuspendLayout();
            grpLog.SuspendLayout();
            cmsLog.SuspendLayout();
            ssMain.SuspendLayout();
            SuspendLayout();
            //   
            // tlpMain  
            //   
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpMain.Controls.Add(grpConnection, 0, 0);
            tlpMain.Controls.Add(grpServerSettings, 1, 0);
            tlpMain.Controls.Add(grpClientSettings, 1, 0);
            tlpMain.Controls.Add(grpProgress, 0, 1);
            tlpMain.Controls.Add(grpLog, 0, 2);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(10, 10);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 3;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.Size = new Size(864, 529);
            tlpMain.TabIndex = 0;
            //   
            // grpConnection  
            //   
            grpConnection.Controls.Add(tlpConnection);
            grpConnection.Dock = DockStyle.Fill;
            grpConnection.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grpConnection.ForeColor = Color.FromArgb(64, 64, 64);
            grpConnection.Location = new Point(3, 3);
            grpConnection.Name = "grpConnection";
            grpConnection.Padding = new Padding(8);
            grpConnection.Size = new Size(426, 174);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "连接设置";
            //   
            // tlpConnection  
            //   
            tlpConnection.ColumnCount = 3;
            tlpConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpConnection.Controls.Add(lblMode, 0, 0);
            tlpConnection.Controls.Add(cmbMode, 1, 0);
            tlpConnection.Controls.Add(lblPort, 0, 1);
            tlpConnection.Controls.Add(nudPort, 1, 1);
            tlpConnection.Controls.Add(lblServerIP, 0, 2);
            tlpConnection.Controls.Add(txtServerIP, 1, 2);
            tlpConnection.Controls.Add(btnConnect, 1, 3);
            tlpConnection.Controls.Add(btnDisconnect, 2, 3);
            tlpConnection.Controls.Add(lblStatus, 0, 4);
            tlpConnection.Controls.Add(lblStatusValue, 1, 4);
            tlpConnection.Dock = DockStyle.Fill;
            tlpConnection.Location = new Point(8, 24);
            tlpConnection.Name = "tlpConnection";
            tlpConnection.RowCount = 5;
            tlpConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpConnection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpConnection.Size = new Size(410, 142);
            tlpConnection.TabIndex = 0;
            //   
            // lblMode  
            //   
            lblMode.Anchor = AnchorStyles.Left;
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Microsoft YaHei UI", 9F);
            lblMode.Location = new Point(3, 6);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(44, 17);
            lblMode.TabIndex = 0;
            lblMode.Text = "模式：";
            //   
            // cmbMode  
            //   
            cmbMode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Font = new Font("Microsoft YaHei UI", 9F);
            cmbMode.FormattingEnabled = true;
            cmbMode.Items.AddRange(new object[] { "服务器模式", "客户端模式" });
            cmbMode.Location = new Point(83, 3);
            cmbMode.Name = "cmbMode";
            cmbMode.Size = new Size(224, 25);
            cmbMode.TabIndex = 1;
            cmbMode.SelectedIndexChanged += cmbMode_SelectedIndexChanged;
            //   
            // lblPort  
            //   
            lblPort.Anchor = AnchorStyles.Left;
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Microsoft YaHei UI", 9F);
            lblPort.Location = new Point(3, 36);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(44, 17);
            lblPort.TabIndex = 2;
            lblPort.Text = "端口：";
            //   
            // nudPort  
            //   
            nudPort.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nudPort.Font = new Font("Microsoft YaHei UI", 9F);
            nudPort.Location = new Point(83, 33);
            nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Size = new Size(224, 23);
            nudPort.TabIndex = 3;
            nudPort.Value = new decimal(new int[] { 8888, 0, 0, 0 });
            //   
            // lblServerIP  
            //   
            lblServerIP.Anchor = AnchorStyles.Left;
            lblServerIP.AutoSize = true;
            lblServerIP.Font = new Font("Microsoft YaHei UI", 9F);
            lblServerIP.Location = new Point(3, 66);
            lblServerIP.Name = "lblServerIP";
            lblServerIP.Size = new Size(58, 17);
            lblServerIP.TabIndex = 4;
            lblServerIP.Text = "服务器IP:";
            //   
            // txtServerIP  
            //   
            txtServerIP.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtServerIP.Font = new Font("Microsoft YaHei UI", 9F);
            txtServerIP.Location = new Point(83, 63);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(224, 23);
            txtServerIP.TabIndex = 5;
            txtServerIP.Text = "127.0.0.1";
            //   
            // btnConnect  
            //   
            btnConnect.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnConnect.BackColor = Color.FromArgb(0, 122, 204);
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(83, 93);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(224, 28);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            //   
            // btnDisconnect  
            //   
            btnDisconnect.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnDisconnect.BackColor = Color.FromArgb(231, 76, 60);
            btnDisconnect.Enabled = false;
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.Location = new Point(313, 93);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 28);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "断开";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += btnDisconnect_Click;
            //   
            // lblStatus  
            //   
            lblStatus.Anchor = AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft YaHei UI", 9F);
            lblStatus.Location = new Point(3, 125);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(44, 17);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "状态：";
            //   
            // lblStatusValue  
            //   
            lblStatusValue.Anchor = AnchorStyles.Left;
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            lblStatusValue.ForeColor = Color.FromArgb(231, 76, 60);
            lblStatusValue.Location = new Point(83, 125);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(44, 17);
            lblStatusValue.TabIndex = 9;
            lblStatusValue.Text = "未连接";
            //   
            // grpServerSettings  
            //   
            grpServerSettings.Controls.Add(tlpServerSettings);
            grpServerSettings.Dock = DockStyle.Fill;
            grpServerSettings.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grpServerSettings.ForeColor = Color.FromArgb(64, 64, 64);
            grpServerSettings.Location = new Point(435, 3);
            grpServerSettings.Name = "grpServerSettings";
            grpServerSettings.Padding = new Padding(8);
            grpServerSettings.Size = new Size(426, 174);
            grpServerSettings.TabIndex = 1;
            grpServerSettings.TabStop = false;
            grpServerSettings.Text = "服务器设置";
            grpServerSettings.Visible = false;
            //   
            // tlpServerSettings  
            //   
            tlpServerSettings.ColumnCount = 3;
            tlpServerSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpServerSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpServerSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpServerSettings.Controls.Add(lblSaveDirectory, 0, 0);
            tlpServerSettings.Controls.Add(txtSaveDirectory, 1, 0);
            tlpServerSettings.Controls.Add(btnBrowseSaveDirectory, 2, 0);
            tlpServerSettings.Dock = DockStyle.Fill;
            tlpServerSettings.Location = new Point(8, 24);
            tlpServerSettings.Name = "tlpServerSettings";
            tlpServerSettings.RowCount = 4;
            tlpServerSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpServerSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpServerSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpServerSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpServerSettings.Size = new Size(410, 142);
            tlpServerSettings.TabIndex = 0;
            //   
            // lblSaveDirectory  
            //   
            lblSaveDirectory.Anchor = AnchorStyles.Left;
            lblSaveDirectory.AutoSize = true;
            lblSaveDirectory.Font = new Font("Microsoft YaHei UI", 9F);
            lblSaveDirectory.Location = new Point(3, 6);
            lblSaveDirectory.Name = "lblSaveDirectory";
            lblSaveDirectory.Size = new Size(68, 17);
            lblSaveDirectory.TabIndex = 0;
            lblSaveDirectory.Text = "保存目录：";
            //   
            // txtSaveDirectory  
            //   
            txtSaveDirectory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSaveDirectory.Font = new Font("Microsoft YaHei UI", 9F);
            txtSaveDirectory.Location = new Point(83, 3);
            txtSaveDirectory.Name = "txtSaveDirectory";
            txtSaveDirectory.ReadOnly = true;
            txtSaveDirectory.Size = new Size(244, 23);
            txtSaveDirectory.TabIndex = 1;
            //   
            // btnBrowseSaveDirectory  
            //   
            btnBrowseSaveDirectory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnBrowseSaveDirectory.BackColor = Color.FromArgb(149, 165, 166);
            btnBrowseSaveDirectory.FlatAppearance.BorderSize = 0;
            btnBrowseSaveDirectory.FlatStyle = FlatStyle.Flat;
            btnBrowseSaveDirectory.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
            btnBrowseSaveDirectory.ForeColor = Color.White;
            btnBrowseSaveDirectory.Location = new Point(333, 3);
            btnBrowseSaveDirectory.Name = "btnBrowseSaveDirectory";
            btnBrowseSaveDirectory.Size = new Size(74, 23);
            btnBrowseSaveDirectory.TabIndex = 2;
            btnBrowseSaveDirectory.Text = "浏览";
            btnBrowseSaveDirectory.UseVisualStyleBackColor = false;
            btnBrowseSaveDirectory.Click += btnBrowseSaveDirectory_Click;
            //   
            // grpClientSettings  
            //   
            grpClientSettings.Controls.Add(tlpClientSettings);
            grpClientSettings.Dock = DockStyle.Fill;
            grpClientSettings.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grpClientSettings.ForeColor = Color.FromArgb(64, 64, 64);
            grpClientSettings.Location = new Point(435, 3);
            grpClientSettings.Name = "grpClientSettings";
            grpClientSettings.Padding = new Padding(8);
            grpClientSettings.Size = new Size(426, 174);
            grpClientSettings.TabIndex = 2;
            grpClientSettings.TabStop = false;
            grpClientSettings.Text = "客户端设置";
            grpClientSettings.Visible = false;
            //   
            // tlpClientSettings  
            //   
            tlpClientSettings.ColumnCount = 3;
            tlpClientSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpClientSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpClientSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpClientSettings.Controls.Add(lblLocalPath, 0, 0);
            tlpClientSettings.Controls.Add(txtLocalPath, 1, 0);
            tlpClientSettings.Controls.Add(btnBrowseLocal, 2, 0);
            tlpClientSettings.Controls.Add(btnSendFile, 1, 2);
            tlpClientSettings.Dock = DockStyle.Fill;
            tlpClientSettings.Location = new Point(8, 24);
            tlpClientSettings.Name = "tlpClientSettings";
            tlpClientSettings.RowCount = 4;
            tlpClientSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpClientSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpClientSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpClientSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpClientSettings.Size = new Size(410, 142);
            tlpClientSettings.TabIndex = 0;
            //   
            // lblLocalPath  
            //   
            lblLocalPath.Anchor = AnchorStyles.Left;
            lblLocalPath.AutoSize = true;
            lblLocalPath.Font = new Font("Microsoft YaHei UI", 9F);
            lblLocalPath.Location = new Point(3, 6);
            lblLocalPath.Name = "lblLocalPath";
            lblLocalPath.Size = new Size(68, 17);
            lblLocalPath.TabIndex = 0;
            lblLocalPath.Text = "本地文件：";
            //   
            // txtLocalPath  
            //   
            txtLocalPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtLocalPath.Font = new Font("Microsoft YaHei UI", 9F);
            txtLocalPath.Location = new Point(83, 3);
            txtLocalPath.Name = "txtLocalPath";
            txtLocalPath.ReadOnly = true;
            txtLocalPath.Size = new Size(244, 23);
            txtLocalPath.TabIndex = 1;
            //   
            // btnBrowseLocal  
            //   
            btnBrowseLocal.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnBrowseLocal.BackColor = Color.FromArgb(149, 165, 166);
            btnBrowseLocal.FlatAppearance.BorderSize = 0;
            btnBrowseLocal.FlatStyle = FlatStyle.Flat;
            btnBrowseLocal.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
            btnBrowseLocal.ForeColor = Color.White;
            btnBrowseLocal.Location = new Point(333, 3);
            btnBrowseLocal.Name = "btnBrowseLocal";
            btnBrowseLocal.Size = new Size(74, 23);
            btnBrowseLocal.TabIndex = 2;
            btnBrowseLocal.Text = "浏览";
            btnBrowseLocal.UseVisualStyleBackColor = false;
            btnBrowseLocal.Click += btnBrowseLocal_Click;
            //   
            // btnSendFile  
            //   
            btnSendFile.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnSendFile.BackColor = Color.FromArgb(46, 204, 113);
            btnSendFile.Enabled = false;
            btnSendFile.FlatAppearance.BorderSize = 0;
            btnSendFile.FlatStyle = FlatStyle.Flat;
            btnSendFile.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnSendFile.ForeColor = Color.White;
            btnSendFile.Location = new Point(83, 63);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(244, 28);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "发送文件";
            btnSendFile.UseVisualStyleBackColor = false;
            btnSendFile.Click += btnSendFile_Click;
            //   
            // grpProgress  
            //   
            tlpMain.SetColumnSpan(grpProgress, 2);
            grpProgress.Controls.Add(tlpProgress);
            grpProgress.Dock = DockStyle.Fill;
            grpProgress.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grpProgress.ForeColor = Color.FromArgb(64, 64, 64);
            grpProgress.Location = new Point(3, 183);
            grpProgress.Name = "grpProgress";
            grpProgress.Padding = new Padding(8);
            grpProgress.Size = new Size(858, 114);
            grpProgress.TabIndex = 3;
            grpProgress.TabStop = false;
            grpProgress.Text = "传输进度";
            //   
            // tlpProgress  
            //   
            tlpProgress.ColumnCount = 6;
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpProgress.Controls.Add(lblCurrentFile, 0, 0);
            tlpProgress.Controls.Add(lblCurrentFileValue, 1, 0);
            tlpProgress.Controls.Add(pgbFileProgress, 0, 1);
            tlpProgress.Controls.Add(lblProgressPercent, 5, 1);
            tlpProgress.Controls.Add(lblSpeed, 2, 0);
            tlpProgress.Controls.Add(lblSpeedValue, 3, 0);
            tlpProgress.Controls.Add(lblTimeRemaining, 4, 0);
            tlpProgress.Controls.Add(lblTimeRemainingValue, 5, 0);
            tlpProgress.Dock = DockStyle.Fill;
            tlpProgress.Location = new Point(8, 24);
            tlpProgress.Name = "tlpProgress";
            tlpProgress.RowCount = 2;
            tlpProgress.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpProgress.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpProgress.Size = new Size(842, 82);
            tlpProgress.TabIndex = 0;
            //   
            // lblCurrentFile  
            //   
            lblCurrentFile.Anchor = AnchorStyles.Left;
            lblCurrentFile.AutoSize = true;
            lblCurrentFile.Font = new Font("Microsoft YaHei UI", 9F);
            lblCurrentFile.Location = new Point(3, 4);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(68, 17);
            lblCurrentFile.TabIndex = 0;
            lblCurrentFile.Text = "当前文件：";
            //   
            // lblCurrentFileValue  
            //   
            lblCurrentFileValue.Anchor = AnchorStyles.Left;
            lblCurrentFileValue.AutoSize = true;
            lblCurrentFileValue.Font = new Font("Microsoft YaHei UI", 9F);
            lblCurrentFileValue.Location = new Point(83, 4);
            lblCurrentFileValue.Name = "lblCurrentFileValue";
            lblCurrentFileValue.Size = new Size(18, 17);
            lblCurrentFileValue.TabIndex = 1;
            lblCurrentFileValue.Text = "--";
            //   
            // pgbFileProgress  
            //   
            tlpProgress.SetColumnSpan(pgbFileProgress, 5);
            pgbFileProgress.Dock = DockStyle.Fill;
            pgbFileProgress.Location = new Point(3, 28);
            pgbFileProgress.Name = "pgbFileProgress";
            pgbFileProgress.Size = new Size(648, 51);
            pgbFileProgress.Style = ProgressBarStyle.Continuous;
            pgbFileProgress.TabIndex = 2;
            //   
            // lblProgressPercent  
            //   
            lblProgressPercent.Anchor = AnchorStyles.Left;
            lblProgressPercent.AutoSize = true;
            lblProgressPercent.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            lblProgressPercent.ForeColor = Color.FromArgb(0, 122, 204);
            lblProgressPercent.Location = new Point(657, 42);
            lblProgressPercent.Name = "lblProgressPercent";
            lblProgressPercent.Size = new Size(35, 22);
            lblProgressPercent.TabIndex = 3;
            lblProgressPercent.Text = "0%";
            //   
            // lblSpeed  
            //   
            lblSpeed.Anchor = AnchorStyles.Left;
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("Microsoft YaHei UI", 9F);
            lblSpeed.Location = new Point(331, 4);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(44, 17);
            lblSpeed.TabIndex = 4;
            lblSpeed.Text = "速度：";
            //   
            // lblSpeedValue  
            //   
            lblSpeedValue.Anchor = AnchorStyles.Left;
            lblSpeedValue.AutoSize = true;
            lblSpeedValue.Font = new Font("Microsoft YaHei UI", 9F);
            lblSpeedValue.Location = new Point(391, 4);
            lblSpeedValue.Name = "lblSpeedValue";
            lblSpeedValue.Size = new Size(18, 17);
            lblSpeedValue.TabIndex = 5;
            lblSpeedValue.Text = "--";
            //   
            // lblTimeRemaining  
            //   
            lblTimeRemaining.Anchor = AnchorStyles.Left;
            lblTimeRemaining.AutoSize = true;
            lblTimeRemaining.Font = new Font("Microsoft YaHei UI", 9F);
            lblTimeRemaining.Location = new Point(577, 4);
            lblTimeRemaining.Name = "lblTimeRemaining";
            lblTimeRemaining.Size = new Size(68, 17);
            lblTimeRemaining.TabIndex = 6;
            lblTimeRemaining.Text = "剩余时间：";
            //   
            // lblTimeRemainingValue  
            //   
            lblTimeRemainingValue.Anchor = AnchorStyles.Left;
            lblTimeRemainingValue.AutoSize = true;
            lblTimeRemainingValue.Font = new Font("Microsoft YaHei UI", 9F);
            lblTimeRemainingValue.Location = new Point(657, 4);
            lblTimeRemainingValue.Name = "lblTimeRemainingValue";
            lblTimeRemainingValue.Size = new Size(18, 17);
            lblTimeRemainingValue.TabIndex = 7;
            lblTimeRemainingValue.Text = "--";
            //   
            // grpLog  
            //   
            // grpLog
            // 
            tlpMain.SetColumnSpan(grpLog, 2);
            grpLog.Controls.Add(rtbLog);
            grpLog.Dock = DockStyle.Fill;
            grpLog.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grpLog.ForeColor = Color.FromArgb(64, 64, 64);
            grpLog.Location = new Point(3, 303);
            grpLog.Name = "grpLog";
            grpLog.Padding = new Padding(8);
            grpLog.Size = new Size(858, 223);
            grpLog.TabIndex = 4;
            grpLog.TabStop = false;
            grpLog.Text = "操作日志";
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(248, 249, 250);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.ContextMenuStrip = cmsLog;
            rtbLog.Dock = DockStyle.Fill;
            rtbLog.Font = new Font("Consolas", 9F);
            rtbLog.Location = new Point(8, 24);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(842, 191);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // cmsLog
            // 
            cmsLog.Items.AddRange(new ToolStripItem[] { tsmiClearLog, tsmiSaveLog });
            cmsLog.Name = "cmsLog";
            cmsLog.Size = new Size(127, 48);
            // 
            // tsmiClearLog
            // 
            tsmiClearLog.Name = "tsmiClearLog";
            tsmiClearLog.Size = new Size(126, 22);
            tsmiClearLog.Text = "清空日志";
            tsmiClearLog.Click += tsmiClearLog_Click;
            // 
            // tsmiSaveLog
            // 
            tsmiSaveLog.Name = "tsmiSaveLog";
            tsmiSaveLog.Size = new Size(126, 22);
            tsmiSaveLog.Text = "保存日志";
            tsmiSaveLog.Click += tsmiSaveLog_Click;
            // 
            // ssMain
            // 
            ssMain.Items.AddRange(new ToolStripItem[] { tsslStatus, tsslSeparator, tsslTime });
            ssMain.Location = new Point(10, 539);
            ssMain.Name = "ssMain";
            ssMain.Size = new Size(864, 22);
            ssMain.TabIndex = 1;
            ssMain.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(33, 17);
            tsslStatus.Text = "就绪";
            // 
            // tsslSeparator
            // 
            tsslSeparator.Name = "tsslSeparator";
            tsslSeparator.Size = new Size(783, 17);
            tsslSeparator.Spring = true;
            // 
            // tsslTime
            // 
            tsslTime.Name = "tsslTime";
            tsslTime.Size = new Size(33, 17);
            tsslTime.Text = "时间";
            // 
            // tmrUpdate
            // 
            tmrUpdate.Enabled = true;
            tmrUpdate.Interval = 1000;
            tmrUpdate.Tick += tmrUpdate_Tick;
            // 
            // ofdSelectFile
            // 
            ofdSelectFile.Filter = "所有文件|*.*";
            ofdSelectFile.Title = "选择要传输的文件";
            // 
            // sfdSaveFile
            // 
            sfdSaveFile.Filter = "日志文件|*.log|文本文件|*.txt|所有文件|*.*";
            sfdSaveFile.Title = "保存日志文件";
            // 
            // fbdSaveDirectory
            // 
            fbdSaveDirectory.Description = "选择文件保存目录";
            fbdSaveDirectory.ShowNewFolderButton = true;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(884, 571);
            Controls.Add(tlpMain);
            Controls.Add(ssMain);
            Font = new Font("Microsoft YaHei UI", 9F);
            MinimumSize = new Size(900, 600);
            Name = "FrmMain";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "工业级网络文件传输工具 v1.0";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            tlpMain.ResumeLayout(false);
            grpConnection.ResumeLayout(false);
            tlpConnection.ResumeLayout(false);
            tlpConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
            grpServerSettings.ResumeLayout(false);
            tlpServerSettings.ResumeLayout(false);
            tlpServerSettings.PerformLayout();
            grpClientSettings.ResumeLayout(false);
            tlpClientSettings.ResumeLayout(false);
            tlpClientSettings.PerformLayout();
            grpProgress.ResumeLayout(false);
            tlpProgress.ResumeLayout(false);
            tlpProgress.PerformLayout();
            grpLog.ResumeLayout(false);
            cmsLog.ResumeLayout(false);
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // 主布局控件
        private TableLayoutPanel tlpMain;
        private StatusStrip ssMain;
        private ToolStripStatusLabel tsslStatus;
        private ToolStripStatusLabel tsslSeparator;
        private ToolStripStatusLabel tsslTime;
        private Timer tmrUpdate;

        // 连接设置组
        private GroupBox grpConnection;
        private TableLayoutPanel tlpConnection;
        private Label lblMode;
        private ComboBox cmbMode;
        private Label lblPort;
        private NumericUpDown nudPort;
        private Label lblServerIP;
        private TextBox txtServerIP;
        private Button btnConnect;
        private Button btnDisconnect;
        private Label lblStatus;
        private Label lblStatusValue;

        // 服务器设置组
        private GroupBox grpServerSettings;
        private TableLayoutPanel tlpServerSettings;
        private Label lblSaveDirectory;
        private TextBox txtSaveDirectory;
        private Button btnBrowseSaveDirectory;

        // 客户端设置组
        private GroupBox grpClientSettings;
        private TableLayoutPanel tlpClientSettings;
        private Label lblLocalPath;
        private TextBox txtLocalPath;
        private Button btnBrowseLocal;
        private Button btnSendFile;

        // 传输进度组
        private GroupBox grpProgress;
        private TableLayoutPanel tlpProgress;
        private Label lblCurrentFile;
        private Label lblCurrentFileValue;
        private ProgressBar pgbFileProgress;
        private Label lblProgressPercent;
        private Label lblSpeed;
        private Label lblSpeedValue;
        private Label lblTimeRemaining;
        private Label lblTimeRemainingValue;

        // 日志组
        private GroupBox grpLog;
        private RichTextBox rtbLog;
        private ContextMenuStrip cmsLog;
        private ToolStripMenuItem tsmiClearLog;
        private ToolStripMenuItem tsmiSaveLog;

        // 对话框
        private OpenFileDialog ofdSelectFile;
        private SaveFileDialog sfdSaveFile;
        private FolderBrowserDialog fbdSaveDirectory;
    }
}