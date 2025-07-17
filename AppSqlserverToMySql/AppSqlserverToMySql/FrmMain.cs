using System.ComponentModel;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AppSqlserverToMySql
{
    public partial class FrmMain : Form
    {
        private DatabaseConverter converter;
        private BackgroundWorker syncWorker;
        private bool isSyncing = false;

        public FrmMain()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeWorker();
            UpdateUI();
        }

        #region 🎯 初始化方法

        /// <summary>
        /// 初始化事件绑定
        /// </summary>
        private void InitializeEvents()
        {
            // 连接测试按钮事件
            btnTestSQLServer.Click += BtnTestSQLServer_Click;
            btnTestMySQL.Click += BtnTestMySQL_Click;

            // 认证方式切换事件
            rbWindowsAuth.CheckedChanged += RbWindowsAuth_CheckedChanged;
            rbSQLAuth.CheckedChanged += RbSQLAuth_CheckedChanged;

            // 同步相关按钮事件
            btnRefreshTables.Click += BtnRefreshTables_Click;
            btnSelectAll.Click += BtnSelectAll_Click;
            btnSelectNone.Click += BtnSelectNone_Click;
            btnStartSync.Click += BtnStartSync_Click;

            // Tab切换事件
            tcMain.SelectedIndexChanged += TcMain_SelectedIndexChanged;
        }

        /// <summary>
        /// 初始化后台工作器
        /// </summary>
        private void InitializeWorker()
        {
            syncWorker = new BackgroundWorker();
            syncWorker.WorkerReportsProgress = true;
            syncWorker.WorkerSupportsCancellation = true;
            syncWorker.DoWork += SyncWorker_DoWork;
            syncWorker.ProgressChanged += SyncWorker_ProgressChanged;
            syncWorker.RunWorkerCompleted += SyncWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        private void UpdateUI()
        {
            // 初始化认证方式UI
            RbWindowsAuth_CheckedChanged(null, null);

            // 设置默认值
            txtMySQLServer.Text = "localhost";
            txtMySQLPort.Text = "3306";
            txtMySQLUsername.Text = "root";
            txtSQLServer.Text = "localhost";

            // 初始状态
            tsslStatus.Text = "准备就绪";
            tsslConnectionStatus.Text = "未连接";
            lblProgressText.Text = "准备就绪...";
            lblCurrentTable.Text = "当前表：无";
        }

        #endregion

        #region 🔗 数据库连接功能

        /// <summary>
        /// SQL Server认证方式切换
        /// </summary>
        private void RbWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            bool useWindowsAuth = rbWindowsAuth.Checked;
            txtSQLUsername.Enabled = !useWindowsAuth;
            txtSQLPassword.Enabled = !useWindowsAuth;
            lblSQLUsername.Enabled = !useWindowsAuth;
            lblSQLPassword.Enabled = !useWindowsAuth;

            if (useWindowsAuth)
            {
                txtSQLUsername.Text = "";
                txtSQLPassword.Text = "";
            }
        }

        private void RbSQLAuth_CheckedChanged(object sender, EventArgs e)
        {
            // 与Windows认证相反
            RbWindowsAuth_CheckedChanged(sender, e);
        }

        /// <summary>
        /// 测试SQL Server连接
        /// </summary>
        private async void BtnTestSQLServer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQLServer.Text))
            {
                ShowError("请输入SQL Server服务器名！");
                return;
            }

            btnTestSQLServer.Enabled = false;
            tsslStatus.Text = "正在测试SQL Server连接...";

            try
            {
                string connStr = BuildSqlServerConnectionString();
                await Task.Run(() => TestSqlServerConnection(connStr));

                ShowSuccess("✅ SQL Server连接成功！");
                tsslConnectionStatus.Text = "SQL Server：已连接";
            }
            catch (Exception ex)
            {
                ShowError($"❌ SQL Server连接失败：{ex.Message}");
                tsslConnectionStatus.Text = "SQL Server：连接失败";
            }
            finally
            {
                btnTestSQLServer.Enabled = true;
                tsslStatus.Text = "准备就绪";
            }
        }

        /// <summary>
        /// 测试MySQL连接
        /// </summary>
        private async void BtnTestMySQL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMySQLServer.Text) ||
                string.IsNullOrWhiteSpace(txtMySQLUsername.Text))
            {
                ShowError("请输入MySQL服务器和用户名！");
                return;
            }

            btnTestMySQL.Enabled = false;
            tsslStatus.Text = "正在测试MySQL连接...";

            try
            {
                string connStr = BuildMySqlConnectionString();
                await Task.Run(() => TestMySqlConnection(connStr));

                ShowSuccess("✅ MySQL连接成功！");
                UpdateConnectionStatus();
            }
            catch (Exception ex)
            {
                ShowError($"❌ MySQL连接失败：{ex.Message}");
                tsslConnectionStatus.Text = tsslConnectionStatus.Text.Replace("MySQL：已连接", "MySQL：连接失败");
            }
            finally
            {
                btnTestMySQL.Enabled = true;
                tsslStatus.Text = "准备就绪";
            }
        }

        /// <summary>
        /// 构建SQL Server连接字符串
        /// </summary>
        private string BuildSqlServerConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = txtSQLServer.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtSQLDatabase.Text))
            {
                builder.InitialCatalog = txtSQLDatabase.Text.Trim();
            }

            if (rbWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = txtSQLUsername.Text.Trim();
                builder.Password = txtSQLPassword.Text;
            }

            builder.ConnectTimeout = 30;
            return builder.ConnectionString;
        }

        /// <summary>
        /// 构建MySQL连接字符串
        /// </summary>
        private string BuildMySqlConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = txtMySQLServer.Text.Trim();
            builder.Port = uint.Parse(txtMySQLPort.Text.Trim());
            builder.UserID = txtMySQLUsername.Text.Trim();
            builder.Password = txtMySQLPassword.Text;

            if (!string.IsNullOrWhiteSpace(txtMySQLDatabase.Text))
            {
                builder.Database = txtMySQLDatabase.Text.Trim();
            }

            builder.ConnectionTimeout = 30;
            builder.CharacterSet = "utf8mb4";
            return builder.ConnectionString;
        }

        /// <summary>
        /// 测试SQL Server连接
        /// </summary>
        private void TestSqlServerConnection(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT @@VERSION", conn))
                {
                    cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 测试MySQL连接
        /// </summary>
        private void TestMySqlConnection(string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT VERSION()", conn))
                {
                    cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 更新连接状态显示
        /// </summary>
        private void UpdateConnectionStatus()
        {
            string status = "";
            if (tsslConnectionStatus.Text.Contains("SQL Server：已连接"))
            {
                status += "SQL Server：已连接 ";
            }
            status += "MySQL：已连接";
            tsslConnectionStatus.Text = status;
        }

        #endregion

        #region 📋 表管理功能

        /// <summary>
        /// 刷新表列表
        /// </summary>
        private async void BtnRefreshTables_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQLServer.Text) || string.IsNullOrWhiteSpace(txtSQLDatabase.Text))
            {
                ShowError("请先配置SQL Server连接信息！");
                return;
            }

            btnRefreshTables.Enabled = false;
            clbTables.Items.Clear();
            tsslStatus.Text = "正在获取表列表...";

            try
            {
                string connStr = BuildSqlServerConnectionString();
                var tables = await Task.Run(() => GetTableList(connStr));

                clbTables.Items.AddRange(tables);
                ShowInfo($"✅ 成功获取 {tables.Length} 个表");

                // 自动切换到同步页面
                tcMain.SelectedTab = tpSync;
            }
            catch (Exception ex)
            {
                ShowError($"❌ 获取表列表失败：{ex.Message}");
            }
            finally
            {
                btnRefreshTables.Enabled = true;
                tsslStatus.Text = "准备就绪";
            }
        }

        /// <summary>
        /// 获取SQL Server表列表
        /// </summary>
        private string[] GetTableList(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT TABLE_NAME 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE = 'BASE TABLE' 
                    ORDER BY TABLE_NAME";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var tables = new System.Collections.Generic.List<string>();
                    while (reader.Read())
                    {
                        tables.Add(reader["TABLE_NAME"].ToString());
                    }
                    return tables.ToArray();
                }
            }
        }

        /// <summary>
        /// 全选表
        /// </summary>
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                clbTables.SetItemChecked(i, true);
            }
            ShowInfo($"已选择 {clbTables.Items.Count} 个表");
        }

        /// <summary>
        /// 全不选
        /// </summary>
        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                clbTables.SetItemChecked(i, false);
            }
            ShowInfo("已取消选择所有表");
        }

        #endregion

        #region 🔄 数据同步功能

        /// <summary>
        /// 开始同步
        /// </summary>
        private void BtnStartSync_Click(object sender, EventArgs e)
        {
            if (isSyncing)
            {
                // 取消同步
                syncWorker.CancelAsync();
                return;
            }

            // 验证选择
            var selectedTables = clbTables.CheckedItems.Cast<string>().ToArray();
            if (selectedTables.Length == 0)
            {
                ShowError("请至少选择一个表进行同步！");
                return;
            }

            if (!chkSyncStructure.Checked && !chkSyncData.Checked)
            {
                ShowError("请至少选择一种同步方式！");
                return;
            }

            // 验证连接配置
            if (!ValidateConnections())
            {
                return;
            }

            // 开始同步
            StartSync(selectedTables);
        }

        /// <summary>
        /// 验证数据库连接配置
        /// </summary>
        private bool ValidateConnections()
        {
            if (string.IsNullOrWhiteSpace(txtSQLServer.Text) || string.IsNullOrWhiteSpace(txtSQLDatabase.Text))
            {
                ShowError("请完善SQL Server连接配置！");
                tcMain.SelectedTab = tpConnection;
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMySQLServer.Text) ||
                string.IsNullOrWhiteSpace(txtMySQLUsername.Text) ||
                string.IsNullOrWhiteSpace(txtMySQLDatabase.Text))
            {
                ShowError("请完善MySQL连接配置！");
                tcMain.SelectedTab = tpConnection;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 启动同步任务
        /// </summary>
        private void StartSync(string[] selectedTables)
        {
            try
            {
                // 创建转换器
                string sqlConnStr = BuildSqlServerConnectionString();
                string mysqlConnStr = BuildMySqlConnectionString();
                converter = new DatabaseConverter(sqlConnStr, mysqlConnStr);

                // 更新UI状态
                isSyncing = true;
                btnStartSync.Text = "⏹ 停止同步";
                btnStartSync.BackColor = Color.Red;

                pbSyncProgress.Value = 0;
                pbSyncProgress.Maximum = selectedTables.Length;

                txtSyncLog.Clear();
                LogMessage("🚀 开始数据同步任务...", Color.Cyan);
                LogMessage($"📊 共选择 {selectedTables.Length} 个表进行同步", Color.White);
                LogMessage($"⚙️ 同步选项：结构[{(chkSyncStructure.Checked ? "✓" : "✗")}] 数据[{(chkSyncData.Checked ? "✓" : "✗")}]", Color.Yellow);

                // 启动后台任务
                var syncOptions = new SyncOptions
                {
                    Tables = selectedTables,
                    SyncStructure = chkSyncStructure.Checked,
                    SyncData = chkSyncData.Checked
                };

                syncWorker.RunWorkerAsync(syncOptions);
            }
            catch (Exception ex)
            {
                ShowError($"启动同步失败：{ex.Message}");
                ResetSyncUI();
            }
        }

        #endregion

        #region 🔄 后台同步工作器

        /// <summary>
        /// 后台同步工作
        /// </summary>
        private void SyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = (SyncOptions)e.Argument;
            var worker = sender as BackgroundWorker;

            try
            {
                for (int i = 0; i < options.Tables.Length; i++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    string tableName = options.Tables[i];

                    // 报告进度
                    worker.ReportProgress(i, new SyncProgress
                    {
                        CurrentTable = tableName,
                        Message = $"正在处理表：{tableName}",
                        Phase = "开始"
                    });

                    try
                    {
                        // 执行同步
                        if (options.SyncStructure && options.SyncData)
                        {
                            converter.Convert(tableName);
                        }
                        else if (options.SyncStructure)
                        {
                            converter.ConvertStructureOnly(tableName);
                        }
                        else if (options.SyncData)
                        {
                            converter.ConvertDataOnly(tableName);
                        }

                        worker.ReportProgress(i, new SyncProgress
                        {
                            CurrentTable = tableName,
                            Message = $"✅ 表 {tableName} 同步完成",
                            Phase = "完成",
                            Success = true
                        });
                    }
                    catch (Exception ex)
                    {
                        worker.ReportProgress(i, new SyncProgress
                        {
                            CurrentTable = tableName,
                            Message = $"❌ 表 {tableName} 同步失败：{ex.Message}",
                            Phase = "错误",
                            Success = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"同步过程发生致命错误：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 同步进度更新
        /// </summary>
        private void SyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var progress = (SyncProgress)e.UserState;

            // 更新进度条
            pbSyncProgress.Value = e.ProgressPercentage + 1;

            // 更新当前表信息
            lblCurrentTable.Text = $"当前表：{progress.CurrentTable}";
            lblProgressText.Text = $"进度：{pbSyncProgress.Value}/{pbSyncProgress.Maximum} - {progress.Phase}";

            // 更新日志
            Color logColor = progress.Success ? Color.LimeGreen : Color.Red;
            if (progress.Phase == "开始")
                logColor = Color.Cyan;

            LogMessage(progress.Message, logColor);

            // 更新状态栏
            tsslStatus.Text = progress.Message;
        }

        /// <summary>
        /// 同步完成
        /// </summary>
        private void SyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetSyncUI();

            if (e.Cancelled)
            {
                LogMessage("⚠️ 同步任务已被用户取消", Color.Yellow);
                ShowWarning("同步任务已取消！");
            }
            else if (e.Error != null)
            {
                LogMessage($"💥 同步任务异常终止：{e.Error.Message}", Color.Red);
                ShowError($"同步失败：{e.Error.Message}");
            }
            else
            {
                LogMessage("🎉 所有表同步任务完成！", Color.LimeGreen);
                ShowSuccess("🎉 数据同步完成！");
            }

            tsslStatus.Text = "同步任务结束";
        }

        /// <summary>
        /// 重置同步UI状态
        /// </summary>
        private void ResetSyncUI()
        {
            isSyncing = false;
            btnStartSync.Text = "🚀 开始同步";
            btnStartSync.BackColor = Color.Green;
            lblCurrentTable.Text = "当前表：无";
            lblProgressText.Text = "准备就绪...";
        }

        #endregion

        #region 🎨 UI辅助方法

        /// <summary>
        /// Tab切换事件
        /// </summary>
        private void TcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpSync && clbTables.Items.Count == 0)
            {
                ShowInfo("请先在连接页面配置数据库连接并获取表列表");
            }
        }

        /// <summary>
        /// 记录同步日志
        /// </summary>
        private void LogMessage(string message, Color color)
        {
            if (txtSyncLog.InvokeRequired)
            {
                txtSyncLog.Invoke(new Action<string, Color>(LogMessage), message, color);
                return;
            }

            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
            string logText = $"[{timeStamp}] {message}\n";

            txtSyncLog.SelectionStart = txtSyncLog.TextLength;
            txtSyncLog.SelectionLength = 0;
            txtSyncLog.SelectionColor = color;
            txtSyncLog.AppendText(logText);
            txtSyncLog.SelectionColor = txtSyncLog.ForeColor;
            txtSyncLog.ScrollToCaret();
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示警告消息
        /// </summary>
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示信息消息
        /// </summary>
        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
