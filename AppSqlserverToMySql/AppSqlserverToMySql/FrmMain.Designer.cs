namespace AppSqlserverToMySql
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            pnlMain = new Panel();
            tcMain = new TabControl();
            tpConnection = new TabPage();
            gbMySQL = new GroupBox();
            btnTestMySQL = new Button();
            txtMySQLDatabase = new TextBox();
            lblMySQLDatabase = new Label();
            txtMySQLPassword = new TextBox();
            lblMySQLPassword = new Label();
            txtMySQLUsername = new TextBox();
            lblMySQLUsername = new Label();
            txtMySQLPort = new TextBox();
            lblMySQLPort = new Label();
            txtMySQLServer = new TextBox();
            lblMySQLServer = new Label();
            gbSQLServer = new GroupBox();
            btnTestSQLServer = new Button();
            txtSQLDatabase = new TextBox();
            lblSQLDatabase = new Label();
            txtSQLPassword = new TextBox();
            lblSQLPassword = new Label();
            txtSQLUsername = new TextBox();
            lblSQLUsername = new Label();
            txtSQLServer = new TextBox();
            lblSQLServer = new Label();
            rbSQLAuth = new RadioButton();
            rbWindowsAuth = new RadioButton();
            tpSync = new TabPage();
            pnlSyncRight = new Panel();
            gbSyncLog = new GroupBox();
            txtSyncLog = new RichTextBox();
            gbSyncProgress = new GroupBox();
            lblCurrentTable = new Label();
            lblProgressText = new Label();
            pbSyncProgress = new ProgressBar();
            pnlSyncLeft = new Panel();
            gbSyncOptions = new GroupBox();
            chkSyncData = new CheckBox();
            chkSyncStructure = new CheckBox();
            btnStartSync = new Button();
            btnSelectAll = new Button();
            btnSelectNone = new Button();
            gbTableSelection = new GroupBox();
            clbTables = new CheckedListBox();
            btnRefreshTables = new Button();
            statusStrip = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            tsslConnectionStatus = new ToolStripStatusLabel();
            pnlTop = new Panel();
            lblTitle = new Label();
            pnlMain.SuspendLayout();
            tcMain.SuspendLayout();
            tpConnection.SuspendLayout();
            gbMySQL.SuspendLayout();
            gbSQLServer.SuspendLayout();
            tpSync.SuspendLayout();
            pnlSyncRight.SuspendLayout();
            gbSyncLog.SuspendLayout();
            gbSyncProgress.SuspendLayout();
            pnlSyncLeft.SuspendLayout();
            gbSyncOptions.SuspendLayout();
            gbTableSelection.SuspendLayout();
            statusStrip.SuspendLayout();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(tcMain);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 75);
            pnlMain.Margin = new Padding(4, 4, 4, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(12, 12, 12, 12);
            pnlMain.Size = new Size(1167, 631);
            pnlMain.TabIndex = 0;
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpConnection);
            tcMain.Controls.Add(tpSync);
            tcMain.Dock = DockStyle.Fill;
            tcMain.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            tcMain.Location = new Point(12, 12);
            tcMain.Margin = new Padding(4, 4, 4, 4);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.Size = new Size(1143, 607);
            tcMain.TabIndex = 0;
            // 
            // tpConnection
            // 
            tpConnection.Controls.Add(gbMySQL);
            tpConnection.Controls.Add(gbSQLServer);
            tpConnection.Location = new Point(4, 26);
            tpConnection.Margin = new Padding(4, 4, 4, 4);
            tpConnection.Name = "tpConnection";
            tpConnection.Padding = new Padding(4, 4, 4, 4);
            tpConnection.Size = new Size(1135, 577);
            tpConnection.TabIndex = 0;
            tpConnection.Text = "🔗 数据库连接";
            tpConnection.UseVisualStyleBackColor = true;
            // 
            // gbMySQL
            // 
            gbMySQL.Controls.Add(btnTestMySQL);
            gbMySQL.Controls.Add(txtMySQLDatabase);
            gbMySQL.Controls.Add(lblMySQLDatabase);
            gbMySQL.Controls.Add(txtMySQLPassword);
            gbMySQL.Controls.Add(lblMySQLPassword);
            gbMySQL.Controls.Add(txtMySQLUsername);
            gbMySQL.Controls.Add(lblMySQLUsername);
            gbMySQL.Controls.Add(txtMySQLPort);
            gbMySQL.Controls.Add(lblMySQLPort);
            gbMySQL.Controls.Add(txtMySQLServer);
            gbMySQL.Controls.Add(lblMySQLServer);
            gbMySQL.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbMySQL.ForeColor = Color.DarkOrange;
            gbMySQL.Location = new Point(567, 8);
            gbMySQL.Margin = new Padding(4, 4, 4, 4);
            gbMySQL.Name = "gbMySQL";
            gbMySQL.Padding = new Padding(4, 4, 4, 4);
            gbMySQL.Size = new Size(560, 375);
            gbMySQL.TabIndex = 1;
            gbMySQL.TabStop = false;
            gbMySQL.Text = "🐬 MySQL 目标数据库";
            // 
            // btnTestMySQL
            // 
            btnTestMySQL.BackColor = Color.DarkOrange;
            btnTestMySQL.FlatStyle = FlatStyle.Flat;
            btnTestMySQL.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnTestMySQL.ForeColor = Color.White;
            btnTestMySQL.Location = new Point(408, 312);
            btnTestMySQL.Margin = new Padding(4, 4, 4, 4);
            btnTestMySQL.Name = "btnTestMySQL";
            btnTestMySQL.Size = new Size(117, 44);
            btnTestMySQL.TabIndex = 10;
            btnTestMySQL.Text = "🔍 测试连接";
            btnTestMySQL.UseVisualStyleBackColor = false;
            // 
            // txtMySQLDatabase
            // 
            txtMySQLDatabase.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtMySQLDatabase.Location = new Point(140, 250);
            txtMySQLDatabase.Margin = new Padding(4, 4, 4, 4);
            txtMySQLDatabase.Name = "txtMySQLDatabase";
            txtMySQLDatabase.Size = new Size(384, 23);
            txtMySQLDatabase.TabIndex = 9;
            // 
            // lblMySQLDatabase
            // 
            lblMySQLDatabase.AutoSize = true;
            lblMySQLDatabase.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblMySQLDatabase.ForeColor = Color.Black;
            lblMySQLDatabase.Location = new Point(23, 254);
            lblMySQLDatabase.Margin = new Padding(4, 0, 4, 0);
            lblMySQLDatabase.Name = "lblMySQLDatabase";
            lblMySQLDatabase.Size = new Size(68, 17);
            lblMySQLDatabase.TabIndex = 8;
            lblMySQLDatabase.Text = "数据库名：";
            // 
            // txtMySQLPassword
            // 
            txtMySQLPassword.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtMySQLPassword.Location = new Point(140, 200);
            txtMySQLPassword.Margin = new Padding(4, 4, 4, 4);
            txtMySQLPassword.Name = "txtMySQLPassword";
            txtMySQLPassword.PasswordChar = '*';
            txtMySQLPassword.Size = new Size(384, 23);
            txtMySQLPassword.TabIndex = 7;
            // 
            // lblMySQLPassword
            // 
            lblMySQLPassword.AutoSize = true;
            lblMySQLPassword.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblMySQLPassword.ForeColor = Color.Black;
            lblMySQLPassword.Location = new Point(23, 204);
            lblMySQLPassword.Margin = new Padding(4, 0, 4, 0);
            lblMySQLPassword.Name = "lblMySQLPassword";
            lblMySQLPassword.Size = new Size(44, 17);
            lblMySQLPassword.TabIndex = 6;
            lblMySQLPassword.Text = "密码：";
            // 
            // txtMySQLUsername
            // 
            txtMySQLUsername.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtMySQLUsername.Location = new Point(140, 150);
            txtMySQLUsername.Margin = new Padding(4, 4, 4, 4);
            txtMySQLUsername.Name = "txtMySQLUsername";
            txtMySQLUsername.Size = new Size(384, 23);
            txtMySQLUsername.TabIndex = 5;
            txtMySQLUsername.Text = "root";
            // 
            // lblMySQLUsername
            // 
            lblMySQLUsername.AutoSize = true;
            lblMySQLUsername.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblMySQLUsername.ForeColor = Color.Black;
            lblMySQLUsername.Location = new Point(23, 154);
            lblMySQLUsername.Margin = new Padding(4, 0, 4, 0);
            lblMySQLUsername.Name = "lblMySQLUsername";
            lblMySQLUsername.Size = new Size(56, 17);
            lblMySQLUsername.TabIndex = 4;
            lblMySQLUsername.Text = "用户名：";
            // 
            // txtMySQLPort
            // 
            txtMySQLPort.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtMySQLPort.Location = new Point(408, 100);
            txtMySQLPort.Margin = new Padding(4, 4, 4, 4);
            txtMySQLPort.Name = "txtMySQLPort";
            txtMySQLPort.Size = new Size(116, 23);
            txtMySQLPort.TabIndex = 3;
            txtMySQLPort.Text = "3306";
            // 
            // lblMySQLPort
            // 
            lblMySQLPort.AutoSize = true;
            lblMySQLPort.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblMySQLPort.ForeColor = Color.Black;
            lblMySQLPort.Location = new Point(350, 104);
            lblMySQLPort.Margin = new Padding(4, 0, 4, 0);
            lblMySQLPort.Name = "lblMySQLPort";
            lblMySQLPort.Size = new Size(44, 17);
            lblMySQLPort.TabIndex = 2;
            lblMySQLPort.Text = "端口：";
            // 
            // txtMySQLServer
            // 
            txtMySQLServer.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtMySQLServer.Location = new Point(140, 100);
            txtMySQLServer.Margin = new Padding(4, 4, 4, 4);
            txtMySQLServer.Name = "txtMySQLServer";
            txtMySQLServer.Size = new Size(186, 23);
            txtMySQLServer.TabIndex = 1;
            txtMySQLServer.Text = "localhost";
            // 
            // lblMySQLServer
            // 
            lblMySQLServer.AutoSize = true;
            lblMySQLServer.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblMySQLServer.ForeColor = Color.Black;
            lblMySQLServer.Location = new Point(23, 104);
            lblMySQLServer.Margin = new Padding(4, 0, 4, 0);
            lblMySQLServer.Name = "lblMySQLServer";
            lblMySQLServer.Size = new Size(68, 17);
            lblMySQLServer.TabIndex = 0;
            lblMySQLServer.Text = "服务器名：";
            // 
            // gbSQLServer
            // 
            gbSQLServer.Controls.Add(btnTestSQLServer);
            gbSQLServer.Controls.Add(txtSQLDatabase);
            gbSQLServer.Controls.Add(lblSQLDatabase);
            gbSQLServer.Controls.Add(txtSQLPassword);
            gbSQLServer.Controls.Add(lblSQLPassword);
            gbSQLServer.Controls.Add(txtSQLUsername);
            gbSQLServer.Controls.Add(lblSQLUsername);
            gbSQLServer.Controls.Add(txtSQLServer);
            gbSQLServer.Controls.Add(lblSQLServer);
            gbSQLServer.Controls.Add(rbSQLAuth);
            gbSQLServer.Controls.Add(rbWindowsAuth);
            gbSQLServer.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbSQLServer.ForeColor = Color.DarkBlue;
            gbSQLServer.Location = new Point(7, 8);
            gbSQLServer.Margin = new Padding(4, 4, 4, 4);
            gbSQLServer.Name = "gbSQLServer";
            gbSQLServer.Padding = new Padding(4, 4, 4, 4);
            gbSQLServer.Size = new Size(553, 375);
            gbSQLServer.TabIndex = 0;
            gbSQLServer.TabStop = false;
            gbSQLServer.Text = "🗄️ SQL Server 源数据库";
            // 
            // btnTestSQLServer
            // 
            btnTestSQLServer.BackColor = Color.DarkBlue;
            btnTestSQLServer.FlatStyle = FlatStyle.Flat;
            btnTestSQLServer.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnTestSQLServer.ForeColor = Color.White;
            btnTestSQLServer.Location = new Point(408, 312);
            btnTestSQLServer.Margin = new Padding(4, 4, 4, 4);
            btnTestSQLServer.Name = "btnTestSQLServer";
            btnTestSQLServer.Size = new Size(117, 44);
            btnTestSQLServer.TabIndex = 10;
            btnTestSQLServer.Text = "🔍 测试连接";
            btnTestSQLServer.UseVisualStyleBackColor = false;
            // 
            // txtSQLDatabase
            // 
            txtSQLDatabase.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSQLDatabase.Location = new Point(140, 250);
            txtSQLDatabase.Margin = new Padding(4, 4, 4, 4);
            txtSQLDatabase.Name = "txtSQLDatabase";
            txtSQLDatabase.Size = new Size(384, 23);
            txtSQLDatabase.TabIndex = 9;
            // 
            // lblSQLDatabase
            // 
            lblSQLDatabase.AutoSize = true;
            lblSQLDatabase.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblSQLDatabase.ForeColor = Color.Black;
            lblSQLDatabase.Location = new Point(23, 254);
            lblSQLDatabase.Margin = new Padding(4, 0, 4, 0);
            lblSQLDatabase.Name = "lblSQLDatabase";
            lblSQLDatabase.Size = new Size(68, 17);
            lblSQLDatabase.TabIndex = 8;
            lblSQLDatabase.Text = "数据库名：";
            // 
            // txtSQLPassword
            // 
            txtSQLPassword.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSQLPassword.Location = new Point(140, 200);
            txtSQLPassword.Margin = new Padding(4, 4, 4, 4);
            txtSQLPassword.Name = "txtSQLPassword";
            txtSQLPassword.PasswordChar = '*';
            txtSQLPassword.Size = new Size(384, 23);
            txtSQLPassword.TabIndex = 7;
            // 
            // lblSQLPassword
            // 
            lblSQLPassword.AutoSize = true;
            lblSQLPassword.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblSQLPassword.ForeColor = Color.Black;
            lblSQLPassword.Location = new Point(23, 204);
            lblSQLPassword.Margin = new Padding(4, 0, 4, 0);
            lblSQLPassword.Name = "lblSQLPassword";
            lblSQLPassword.Size = new Size(44, 17);
            lblSQLPassword.TabIndex = 6;
            lblSQLPassword.Text = "密码：";
            // 
            // txtSQLUsername
            // 
            txtSQLUsername.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSQLUsername.Location = new Point(140, 150);
            txtSQLUsername.Margin = new Padding(4, 4, 4, 4);
            txtSQLUsername.Name = "txtSQLUsername";
            txtSQLUsername.Size = new Size(384, 23);
            txtSQLUsername.TabIndex = 5;
            // 
            // lblSQLUsername
            // 
            lblSQLUsername.AutoSize = true;
            lblSQLUsername.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblSQLUsername.ForeColor = Color.Black;
            lblSQLUsername.Location = new Point(23, 154);
            lblSQLUsername.Margin = new Padding(4, 0, 4, 0);
            lblSQLUsername.Name = "lblSQLUsername";
            lblSQLUsername.Size = new Size(56, 17);
            lblSQLUsername.TabIndex = 4;
            lblSQLUsername.Text = "用户名：";
            // 
            // txtSQLServer
            // 
            txtSQLServer.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSQLServer.Location = new Point(140, 62);
            txtSQLServer.Margin = new Padding(4, 4, 4, 4);
            txtSQLServer.Name = "txtSQLServer";
            txtSQLServer.Size = new Size(384, 23);
            txtSQLServer.TabIndex = 3;
            txtSQLServer.Text = "localhost";
            // 
            // lblSQLServer
            // 
            lblSQLServer.AutoSize = true;
            lblSQLServer.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblSQLServer.ForeColor = Color.Black;
            lblSQLServer.Location = new Point(23, 66);
            lblSQLServer.Margin = new Padding(4, 0, 4, 0);
            lblSQLServer.Name = "lblSQLServer";
            lblSQLServer.Size = new Size(68, 17);
            lblSQLServer.TabIndex = 2;
            lblSQLServer.Text = "服务器名：";
            // 
            // rbSQLAuth
            // 
            rbSQLAuth.AutoSize = true;
            rbSQLAuth.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            rbSQLAuth.ForeColor = Color.Black;
            rbSQLAuth.Location = new Point(210, 112);
            rbSQLAuth.Margin = new Padding(4, 4, 4, 4);
            rbSQLAuth.Name = "rbSQLAuth";
            rbSQLAuth.Size = new Size(114, 21);
            rbSQLAuth.TabIndex = 1;
            rbSQLAuth.Text = "SQL Server验证";
            rbSQLAuth.UseVisualStyleBackColor = true;
            // 
            // rbWindowsAuth
            // 
            rbWindowsAuth.AutoSize = true;
            rbWindowsAuth.Checked = true;
            rbWindowsAuth.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            rbWindowsAuth.ForeColor = Color.Black;
            rbWindowsAuth.Location = new Point(27, 112);
            rbWindowsAuth.Margin = new Padding(4, 4, 4, 4);
            rbWindowsAuth.Name = "rbWindowsAuth";
            rbWindowsAuth.Size = new Size(103, 21);
            rbWindowsAuth.TabIndex = 0;
            rbWindowsAuth.TabStop = true;
            rbWindowsAuth.Text = "Windows验证";
            rbWindowsAuth.UseVisualStyleBackColor = true;
            // 
            // tpSync
            // 
            tpSync.Controls.Add(pnlSyncRight);
            tpSync.Controls.Add(pnlSyncLeft);
            tpSync.Location = new Point(4, 26);
            tpSync.Margin = new Padding(4, 4, 4, 4);
            tpSync.Name = "tpSync";
            tpSync.Padding = new Padding(4, 4, 4, 4);
            tpSync.Size = new Size(1135, 577);
            tpSync.TabIndex = 1;
            tpSync.Text = "🔄 数据同步";
            tpSync.UseVisualStyleBackColor = true;
            // 
            // pnlSyncRight
            // 
            pnlSyncRight.Controls.Add(gbSyncLog);
            pnlSyncRight.Controls.Add(gbSyncProgress);
            pnlSyncRight.Dock = DockStyle.Fill;
            pnlSyncRight.Location = new Point(412, 4);
            pnlSyncRight.Margin = new Padding(4, 4, 4, 4);
            pnlSyncRight.Name = "pnlSyncRight";
            pnlSyncRight.Padding = new Padding(6, 0, 0, 0);
            pnlSyncRight.Size = new Size(719, 569);
            pnlSyncRight.TabIndex = 1;
            // 
            // gbSyncLog
            // 
            gbSyncLog.Controls.Add(txtSyncLog);
            gbSyncLog.Dock = DockStyle.Fill;
            gbSyncLog.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbSyncLog.ForeColor = Color.DarkGreen;
            gbSyncLog.Location = new Point(6, 150);
            gbSyncLog.Margin = new Padding(4, 4, 4, 4);
            gbSyncLog.Name = "gbSyncLog";
            gbSyncLog.Padding = new Padding(4, 4, 4, 4);
            gbSyncLog.Size = new Size(713, 419);
            gbSyncLog.TabIndex = 1;
            gbSyncLog.TabStop = false;
            gbSyncLog.Text = "📋 同步日志";
            // 
            // txtSyncLog
            // 
            txtSyncLog.BackColor = Color.Black;
            txtSyncLog.Dock = DockStyle.Fill;
            txtSyncLog.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSyncLog.ForeColor = Color.Lime;
            txtSyncLog.Location = new Point(4, 20);
            txtSyncLog.Margin = new Padding(4, 4, 4, 4);
            txtSyncLog.Name = "txtSyncLog";
            txtSyncLog.ReadOnly = true;
            txtSyncLog.Size = new Size(705, 395);
            txtSyncLog.TabIndex = 0;
            txtSyncLog.Text = "准备开始同步...\n请先配置数据库连接并选择要同步的表。";
            // 
            // gbSyncProgress
            // 
            gbSyncProgress.Controls.Add(lblCurrentTable);
            gbSyncProgress.Controls.Add(lblProgressText);
            gbSyncProgress.Controls.Add(pbSyncProgress);
            gbSyncProgress.Dock = DockStyle.Top;
            gbSyncProgress.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbSyncProgress.ForeColor = Color.DarkBlue;
            gbSyncProgress.Location = new Point(6, 0);
            gbSyncProgress.Margin = new Padding(4, 4, 4, 4);
            gbSyncProgress.Name = "gbSyncProgress";
            gbSyncProgress.Padding = new Padding(4, 4, 4, 4);
            gbSyncProgress.Size = new Size(713, 150);
            gbSyncProgress.TabIndex = 0;
            gbSyncProgress.TabStop = false;
            gbSyncProgress.Text = "📊 同步进度";
            // 
            // lblCurrentTable
            // 
            lblCurrentTable.AutoSize = true;
            lblCurrentTable.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblCurrentTable.ForeColor = Color.Black;
            lblCurrentTable.Location = new Point(18, 100);
            lblCurrentTable.Margin = new Padding(4, 0, 4, 0);
            lblCurrentTable.Name = "lblCurrentTable";
            lblCurrentTable.Size = new Size(68, 17);
            lblCurrentTable.TabIndex = 2;
            lblCurrentTable.Text = "当前表：无";
            // 
            // lblProgressText
            // 
            lblProgressText.AutoSize = true;
            lblProgressText.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblProgressText.ForeColor = Color.Black;
            lblProgressText.Location = new Point(18, 38);
            lblProgressText.Margin = new Padding(4, 0, 4, 0);
            lblProgressText.Name = "lblProgressText";
            lblProgressText.Size = new Size(65, 17);
            lblProgressText.TabIndex = 1;
            lblProgressText.Text = "准备就绪...";
            // 
            // pbSyncProgress
            // 
            pbSyncProgress.Location = new Point(21, 62);
            pbSyncProgress.Margin = new Padding(4, 4, 4, 4);
            pbSyncProgress.Name = "pbSyncProgress";
            pbSyncProgress.Size = new Size(671, 29);
            pbSyncProgress.TabIndex = 0;
            // 
            // pnlSyncLeft
            // 
            pnlSyncLeft.Controls.Add(gbSyncOptions);
            pnlSyncLeft.Controls.Add(gbTableSelection);
            pnlSyncLeft.Dock = DockStyle.Left;
            pnlSyncLeft.Location = new Point(4, 4);
            pnlSyncLeft.Margin = new Padding(4, 4, 4, 4);
            pnlSyncLeft.Name = "pnlSyncLeft";
            pnlSyncLeft.Size = new Size(408, 569);
            pnlSyncLeft.TabIndex = 0;
            // 
            // gbSyncOptions
            // 
            gbSyncOptions.Controls.Add(chkSyncData);
            gbSyncOptions.Controls.Add(chkSyncStructure);
            gbSyncOptions.Controls.Add(btnStartSync);
            gbSyncOptions.Controls.Add(btnSelectAll);
            gbSyncOptions.Controls.Add(btnSelectNone);
            gbSyncOptions.Dock = DockStyle.Bottom;
            gbSyncOptions.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbSyncOptions.ForeColor = Color.DarkRed;
            gbSyncOptions.Location = new Point(0, 419);
            gbSyncOptions.Margin = new Padding(4, 4, 4, 4);
            gbSyncOptions.Name = "gbSyncOptions";
            gbSyncOptions.Padding = new Padding(4, 4, 4, 4);
            gbSyncOptions.Size = new Size(408, 150);
            gbSyncOptions.TabIndex = 1;
            gbSyncOptions.TabStop = false;
            gbSyncOptions.Text = "⚙️ 同步选项";
            // 
            // chkSyncData
            // 
            chkSyncData.AutoSize = true;
            chkSyncData.Checked = true;
            chkSyncData.CheckState = CheckState.Checked;
            chkSyncData.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            chkSyncData.ForeColor = Color.Black;
            chkSyncData.Location = new Point(210, 38);
            chkSyncData.Margin = new Padding(4, 4, 4, 4);
            chkSyncData.Name = "chkSyncData";
            chkSyncData.Size = new Size(75, 21);
            chkSyncData.TabIndex = 4;
            chkSyncData.Text = "同步数据";
            chkSyncData.UseVisualStyleBackColor = true;
            // 
            // chkSyncStructure
            // 
            chkSyncStructure.AutoSize = true;
            chkSyncStructure.Checked = true;
            chkSyncStructure.CheckState = CheckState.Checked;
            chkSyncStructure.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            chkSyncStructure.ForeColor = Color.Black;
            chkSyncStructure.Location = new Point(23, 38);
            chkSyncStructure.Margin = new Padding(4, 4, 4, 4);
            chkSyncStructure.Name = "chkSyncStructure";
            chkSyncStructure.Size = new Size(87, 21);
            chkSyncStructure.TabIndex = 3;
            chkSyncStructure.Text = "同步表结构";
            chkSyncStructure.UseVisualStyleBackColor = true;
            // 
            // btnStartSync
            // 
            btnStartSync.BackColor = Color.Green;
            btnStartSync.FlatStyle = FlatStyle.Flat;
            btnStartSync.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnStartSync.ForeColor = Color.White;
            btnStartSync.Location = new Point(233, 88);
            btnStartSync.Margin = new Padding(4, 4, 4, 4);
            btnStartSync.Name = "btnStartSync";
            btnStartSync.Size = new Size(152, 44);
            btnStartSync.TabIndex = 2;
            btnStartSync.Text = "🚀 开始同步";
            btnStartSync.UseVisualStyleBackColor = false;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSelectAll.Location = new Point(23, 88);
            btnSelectAll.Margin = new Padding(4, 4, 4, 4);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(93, 44);
            btnSelectAll.TabIndex = 1;
            btnSelectAll.Text = "全选";
            btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnSelectNone
            // 
            btnSelectNone.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSelectNone.Location = new Point(128, 88);
            btnSelectNone.Margin = new Padding(4, 4, 4, 4);
            btnSelectNone.Name = "btnSelectNone";
            btnSelectNone.Size = new Size(93, 44);
            btnSelectNone.TabIndex = 0;
            btnSelectNone.Text = "全不选";
            btnSelectNone.UseVisualStyleBackColor = true;
            // 
            // gbTableSelection
            // 
            gbTableSelection.Controls.Add(clbTables);
            gbTableSelection.Controls.Add(btnRefreshTables);
            gbTableSelection.Dock = DockStyle.Fill;
            gbTableSelection.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            gbTableSelection.ForeColor = Color.DarkBlue;
            gbTableSelection.Location = new Point(0, 0);
            gbTableSelection.Margin = new Padding(4, 4, 4, 4);
            gbTableSelection.Name = "gbTableSelection";
            gbTableSelection.Padding = new Padding(4, 4, 4, 4);
            gbTableSelection.Size = new Size(408, 569);
            gbTableSelection.TabIndex = 0;
            gbTableSelection.TabStop = false;
            gbTableSelection.Text = "📋 表选择";
            // 
            // clbTables
            // 
            clbTables.CheckOnClick = true;
            clbTables.Dock = DockStyle.Fill;
            clbTables.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            clbTables.FormattingEnabled = true;
            clbTables.Location = new Point(4, 64);
            clbTables.Margin = new Padding(4, 4, 4, 4);
            clbTables.Name = "clbTables";
            clbTables.Size = new Size(400, 501);
            clbTables.TabIndex = 1;
            // 
            // btnRefreshTables
            // 
            btnRefreshTables.BackColor = Color.DarkBlue;
            btnRefreshTables.Dock = DockStyle.Top;
            btnRefreshTables.FlatStyle = FlatStyle.Flat;
            btnRefreshTables.Font = new Font("Microsoft YaHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnRefreshTables.ForeColor = Color.White;
            btnRefreshTables.Location = new Point(4, 20);
            btnRefreshTables.Margin = new Padding(4, 4, 4, 4);
            btnRefreshTables.Name = "btnRefreshTables";
            btnRefreshTables.Size = new Size(400, 44);
            btnRefreshTables.TabIndex = 0;
            btnRefreshTables.Text = "🔄 刷新表列表";
            btnRefreshTables.UseVisualStyleBackColor = false;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { tsslStatus, tsslConnectionStatus });
            statusStrip.Location = new Point(0, 706);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1167, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(59, 17);
            tsslStatus.Text = "准备就绪";
            // 
            // tsslConnectionStatus
            // 
            tsslConnectionStatus.Name = "tsslConnectionStatus";
            tsslConnectionStatus.Size = new Size(1091, 17);
            tsslConnectionStatus.Spring = true;
            tsslConnectionStatus.Text = "未连接";
            tsslConnectionStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(45, 45, 48);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 4, 4, 4);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1167, 75);
            pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei", 16F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 19);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(381, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🔄 SQL Server → MySQL 同步工具";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 728);
            Controls.Add(pnlMain);
            Controls.Add(statusStrip);
            Controls.Add(pnlTop);
            Margin = new Padding(4, 4, 4, 4);
            MinimumSize = new Size(1164, 740);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SQL Server to MySQL 数据同步工具";
            pnlMain.ResumeLayout(false);
            tcMain.ResumeLayout(false);
            tpConnection.ResumeLayout(false);
            gbMySQL.ResumeLayout(false);
            gbMySQL.PerformLayout();
            gbSQLServer.ResumeLayout(false);
            gbSQLServer.PerformLayout();
            tpSync.ResumeLayout(false);
            pnlSyncRight.ResumeLayout(false);
            gbSyncLog.ResumeLayout(false);
            gbSyncProgress.ResumeLayout(false);
            gbSyncProgress.PerformLayout();
            pnlSyncLeft.ResumeLayout(false);
            gbSyncOptions.ResumeLayout(false);
            gbSyncOptions.PerformLayout();
            gbTableSelection.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpConnection;
        private System.Windows.Forms.TabPage tpSync;
        private System.Windows.Forms.GroupBox gbSQLServer;
        private System.Windows.Forms.GroupBox gbMySQL;
        private System.Windows.Forms.TextBox txtSQLServer;
        private System.Windows.Forms.Label lblSQLServer;
        private System.Windows.Forms.RadioButton rbSQLAuth;
        private System.Windows.Forms.RadioButton rbWindowsAuth;
        private System.Windows.Forms.TextBox txtSQLDatabase;
        private System.Windows.Forms.Label lblSQLDatabase;
        private System.Windows.Forms.TextBox txtSQLPassword;
        private System.Windows.Forms.Label lblSQLPassword;
        private System.Windows.Forms.TextBox txtSQLUsername;
        private System.Windows.Forms.Label lblSQLUsername;
        private System.Windows.Forms.Button btnTestSQLServer;
        private System.Windows.Forms.Button btnTestMySQL;
        private System.Windows.Forms.TextBox txtMySQLDatabase;
        private System.Windows.Forms.Label lblMySQLDatabase;
        private System.Windows.Forms.TextBox txtMySQLPassword;
        private System.Windows.Forms.Label lblMySQLPassword;
        private System.Windows.Forms.TextBox txtMySQLUsername;
        private System.Windows.Forms.Label lblMySQLUsername;
        private System.Windows.Forms.TextBox txtMySQLPort;
        private System.Windows.Forms.Label lblMySQLPort;
        private System.Windows.Forms.TextBox txtMySQLServer;
        private System.Windows.Forms.Label lblMySQLServer;
        private System.Windows.Forms.Panel pnlSyncLeft;
        private System.Windows.Forms.Panel pnlSyncRight;
        private System.Windows.Forms.GroupBox gbTableSelection;
        private System.Windows.Forms.CheckedListBox clbTables;
        private System.Windows.Forms.Button btnRefreshTables;
        private System.Windows.Forms.GroupBox gbSyncOptions;
        private System.Windows.Forms.CheckBox chkSyncData;
        private System.Windows.Forms.CheckBox chkSyncStructure;
        private System.Windows.Forms.Button btnStartSync;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.GroupBox gbSyncProgress;
        private System.Windows.Forms.Label lblCurrentTable;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.ProgressBar pbSyncProgress;
        private System.Windows.Forms.GroupBox gbSyncLog;
        private System.Windows.Forms.RichTextBox txtSyncLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslConnectionStatus;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
    }
}
