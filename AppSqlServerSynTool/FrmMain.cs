using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Data.SqlClient;

namespace AppSqlServerSynTool
{
    public partial class FrmMain : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isSyncing = false;
        private string _sourceConnectionString = "";
        private string _targetConnectionString = "";

        public FrmMain()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeForm();
            this.Load += FrmMain_Load;
        }

        #region 事件绑定  
        private void InitializeEvents()
        {
            // 连接测试按钮事件  
            btnTestSourceConnection.Click += BtnTestSourceConnection_Click;
            btnTestTargetConnection.Click += BtnTestTargetConnection_Click;

            // 表操作按钮事件  
            btnRefreshTables.Click += BtnRefreshTables_Click;
            btnSelectAll.Click += BtnSelectAll_Click;
            btnSelectNone.Click += BtnSelectNone_Click;

            // 同步操作按钮事件  
            btnStartSync.Click += BtnStartSync_Click;
            btnCancel.Click += BtnCancel_Click;

            // 配置操作按钮事件  
            btnSaveConfig.Click += BtnSaveConfig_Click;
            btnLoadConfig.Click += BtnLoadConfig_Click;

            // 退出按钮事件  
            btnExit.Click += BtnExit_Click;

            // 菜单事件  
            saveConfigToolStripMenuItem.Click += BtnSaveConfig_Click;
            loadConfigToolStripMenuItem.Click += BtnLoadConfig_Click;
            exitToolStripMenuItem.Click += BtnExit_Click;
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

            // 窗体关闭事件  
            this.FormClosing += FrmMain_FormClosing;
        }
        #endregion

        #region 初始化  
        private void InitializeForm()
        {
            // 设置默认连接字符串示例  
            txtSourceConnection.Text = "Server=.;Database=SourceDB;Integrated Security=true;";
            txtTargetConnection.Text = "Server=.;Database=TargetDB;Integrated Security=true;";

            // 设置默认值  
            numericBatchSize.Value = 1000;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LogMessage("程序启动完成，请配置数据库连接信息。");
        }
        #endregion

        #region 连接测试  
        private async void BtnTestSourceConnection_Click(object sender, EventArgs e)
        {
            await TestConnection(txtSourceConnection.Text, "源数据库");
        }

        private async void BtnTestTargetConnection_Click(object sender, EventArgs e)
        {
            await TestConnection(txtTargetConnection.Text, "目标数据库");
        }

        private async Task TestConnection(string connectionString, string dbName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show($"请输入{dbName}连接字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetControlsEnabled(false);
                toolStripStatusLabel.Text = $"正在测试{dbName}连接...";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    MessageBox.Show($"{dbName}连接测试成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogMessage($"{dbName}连接测试成功。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{dbName}连接测试失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"{dbName}连接测试失败：{ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
                toolStripStatusLabel.Text = "就绪";
            }
        }
        #endregion

        #region 表操作  
        private async void BtnRefreshTables_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSourceConnection.Text))
            {
                MessageBox.Show("请先配置源数据库连接字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetControlsEnabled(false);
                toolStripStatusLabel.Text = "正在获取表列表...";
                checkedListBoxTables.Items.Clear();

                _sourceConnectionString = txtSourceConnection.Text;
                var tables = await GetTableListAsync(_sourceConnectionString);
                foreach (var table in tables)
                {
                    checkedListBoxTables.Items.Add(table);
                }

                LogMessage($"成功获取 {tables.Count} 个表。");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取表列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"获取表列表失败：{ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
                toolStripStatusLabel.Text = "就绪";
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                checkedListBoxTables.SetItemChecked(i, true);
            }
            LogMessage($"已选择所有 {checkedListBoxTables.Items.Count} 个表。");
        }

        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                checkedListBoxTables.SetItemChecked(i, false);
            }
            LogMessage("已取消选择所有表。");
        }

        private async Task<List<string>> GetTableListAsync(string connectionString)
        {
            var tables = new List<string>();
            const string sql = @"  
                SELECT TABLE_NAME   
                FROM INFORMATION_SCHEMA.TABLES   
                WHERE TABLE_TYPE = 'BASE TABLE'   
                ORDER BY TABLE_NAME";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
            return tables;
        }
        #endregion

        #region 同步操作  
        private async void BtnStartSync_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
                return;

            var selectedTables = GetSelectedTables();
            if (selectedTables.Count == 0)
            {
                MessageBox.Show("请至少选择一个要同步的表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"确定要同步 {selectedTables.Count} 个表吗？\n\n" +
                $"同步选项：\n" +
                $"- 同步结构：{(chkSyncStructure.Checked ? "是" : "否")}\n" +
                $"- 同步数据：{(chkSyncData.Checked ? "是" : "否")}\n" +
                $"- 创建目标表：{(chkCreateTargetTables.Checked ? "是" : "否")}\n" +
                $"- 清空目标表：{(chkTruncateTarget.Checked ? "是" : "否")}\n" +
                $"- 批次大小：{numericBatchSize.Value}",
                "确认同步",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            await StartSyncProcess(selectedTables);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                LogMessage("用户取消了同步操作。");
            }
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(txtSourceConnection.Text))
            {
                MessageBox.Show("请配置源数据库连接字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTargetConnection.Text))
            {
                MessageBox.Show("请配置目标数据库连接字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!chkSyncStructure.Checked && !chkSyncData.Checked)
            {
                MessageBox.Show("请至少选择一种同步选项（同步结构或同步数据）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private List<string> GetSelectedTables()
        {
            var selectedTables = new List<string>();
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                if (checkedListBoxTables.GetItemChecked(i))
                {
                    selectedTables.Add(checkedListBoxTables.Items[i].ToString());
                }
            }
            return selectedTables;
        }

        private async Task StartSyncProcess(List<string> tables)
        {
            _isSyncing = true;
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                SetSyncMode(true);
                progressBarMain.Maximum = tables.Count;
                progressBarMain.Value = 0;
                labelProgress.Text = $"0/{tables.Count}";

                _sourceConnectionString = txtSourceConnection.Text;
                _targetConnectionString = txtTargetConnection.Text;

                LogMessage("开始同步操作...");
                LogMessage($"共需同步 {tables.Count} 个表。");

                var syncOptions = new SyncOptions
                {
                    SyncStructure = chkSyncStructure.Checked,
                    SyncData = chkSyncData.Checked,
                    CreateTargetTables = chkCreateTargetTables.Checked,
                    TruncateTargetTables = chkTruncateTarget.Checked,
                    BatchSize = (int)numericBatchSize.Value
                };

                await SyncTablesAsync(tables, syncOptions, _cancellationTokenSource.Token);

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    LogMessage("同步操作完成！");
                    MessageBox.Show("同步操作完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                LogMessage("同步操作已取消。");
            }
            catch (Exception ex)
            {
                LogMessage($"同步操作发生错误：{ex.Message}");
                MessageBox.Show($"同步操作发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isSyncing = false;
                SetSyncMode(false);
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private async Task SyncTablesAsync(List<string> tables, SyncOptions options, CancellationToken cancellationToken)
        {
            for (int i = 0; i < tables.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var tableName = tables[i];
                LogMessage($"正在同步表：{tableName} ({i + 1}/{tables.Count})");

                try
                {
                    // 1. 同步表结构  
                    if (options.SyncStructure && options.CreateTargetTables)
                    {
                        LogMessage($"正在同步表 {tableName} 的结构...");
                        await SynchronizeTableStructureAsync(tableName);
                    }

                    // 2. 同步数据  
                    if (options.SyncData)
                    {
                        LogMessage($"正在同步表 {tableName} 的数据...");
                        await SynchronizeTableDataFullAsync(tableName);

                        // 3. 验证同步结果  
                        LogMessage($"正在验证表 {tableName} 的同步结果...");
                        bool isValid = await ValidateSyncResultAsync(tableName);
                        LogMessage($"表 {tableName} 同步验证结果：{(isValid ? "成功" : "失败")}");
                    }

                    LogMessage($"表 {tableName} 同步完成。");
                }
                catch (Exception ex)
                {
                    LogMessage($"表 {tableName} 同步失败：{ex.Message}");
                    // 继续处理下一个表，不中断整个流程  
                }

                // 更新进度  
                UpdateProgress(i + 1, tables.Count);
            }
        }

        // 您的同步方法 - 集成到类中  
        private async Task SynchronizeTableStructureAsync(string tableName)
        {
            try
            {
                LogMessage($"获取表 {tableName} 的列信息...");
                DataTable columnsSchema = await GetTableColumnsAsync(tableName);

                if (columnsSchema == null || columnsSchema.Rows.Count == 0)
                {
                    LogMessage($"警告: 表 {tableName} 没有列信息或不存在");
                    return;
                }

                LogMessage($"获取到表 {tableName} 的 {columnsSchema.Rows.Count} 列信息");

                List<string> primaryKeys = await GetPrimaryKeysAsync(tableName);
                LogMessage($"表 {tableName} 的主键列: {string.Join(", ", primaryKeys)}");

                Dictionary<string, string> defaultConstraints = await GetDefaultConstraintsAsync(tableName);
                LogMessage($"表 {tableName} 的默认值约束: {defaultConstraints.Count} 个");

                bool tableExists = await CheckTableExistsAsync(tableName);
                LogMessage($"目标数据库中表 {tableName} {(tableExists ? "已存在" : "不存在")}");

                using (SqlConnection targetConnection = new SqlConnection(_targetConnectionString))
                {
                    await targetConnection.OpenAsync();

                    using (SqlTransaction transaction = targetConnection.BeginTransaction())
                    {
                        try
                        {
                            if (tableExists)
                            {
                                LogMessage($"正在删除目标表 {tableName}");
                                string dropTableSql = $"DROP TABLE [{tableName}]";
                                using (SqlCommand command = new SqlCommand(dropTableSql, targetConnection, transaction))
                                {
                                    await command.ExecuteNonQueryAsync();
                                }
                            }

                            StringBuilder createTableSql = new StringBuilder();
                            createTableSql.AppendLine($"CREATE TABLE [{tableName}] (");

                            List<string> columnDefinitions = new List<string>();
                            foreach (DataRow row in columnsSchema.Rows)
                            {
                                string columnName = row["COLUMN_NAME"].ToString();
                                string dataType = row["DATA_TYPE"].ToString();

                                string maxLength = row["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? row["CHARACTER_MAXIMUM_LENGTH"].ToString() : "";
                                string isNullable = row["IS_NULLABLE"] != DBNull.Value ? row["IS_NULLABLE"].ToString() : "YES";
                                string numericPrecision = row["NUMERIC_PRECISION"] != DBNull.Value ? row["NUMERIC_PRECISION"].ToString() : "";
                                string numericScale = row["NUMERIC_SCALE"] != DBNull.Value ? row["NUMERIC_SCALE"].ToString() : "";

                                StringBuilder columnDefinition = new StringBuilder();
                                columnDefinition.Append($"[{columnName}] {dataType}");

                                if (dataType.ToLower() == "varchar" || dataType.ToLower() == "nvarchar" ||
                                    dataType.ToLower() == "char" || dataType.ToLower() == "nchar")
                                {
                                    if (!string.IsNullOrEmpty(maxLength))
                                    {
                                        if (maxLength == "-1")
                                            columnDefinition.Append("(MAX)");
                                        else if (maxLength == "8000")
                                        {
                                            columnDefinition.Append($"(4000)");
                                        }
                                        else
                                        {
                                            columnDefinition.Append($"({maxLength})");
                                        }
                                    }
                                }
                                else if (dataType.ToLower() == "decimal" || dataType.ToLower() == "numeric")
                                {
                                    if (!string.IsNullOrEmpty(numericPrecision) && !string.IsNullOrEmpty(numericScale))
                                    {
                                        columnDefinition.Append($"({numericPrecision}, {numericScale})");
                                    }
                                }

                                columnDefinition.Append(isNullable == "YES" ? " NULL" : " NOT NULL");

                                if (defaultConstraints.ContainsKey(columnName))
                                {
                                    columnDefinition.Append($" DEFAULT {defaultConstraints[columnName]}");
                                }

                                columnDefinitions.Add(columnDefinition.ToString());
                            }

                            if (columnDefinitions.Count == 0)
                            {
                                throw new Exception($"表 {tableName} 没有有效的列定义");
                            }

                            createTableSql.AppendLine(string.Join("," + Environment.NewLine, columnDefinitions));

                            if (primaryKeys.Count > 0)
                            {
                                createTableSql.AppendLine($", CONSTRAINT [PK_{tableName}] PRIMARY KEY (");
                                createTableSql.AppendLine($"    [{string.Join("], [", primaryKeys)}]");
                                createTableSql.AppendLine(")");
                            }

                            createTableSql.AppendLine(")");

                            string finalSql = createTableSql.ToString();

                            using (SqlCommand command = new SqlCommand(finalSql, targetConnection, transaction))
                            {
                                await command.ExecuteNonQueryAsync();
                            }

                            transaction.Commit();
                            LogMessage($"表 {tableName} 结构同步成功");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogMessage($"表 {tableName} 结构同步失败: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"处理表 {tableName} 时发生错误: {ex.Message}");
                throw;
            }
        }

        private async Task<DataTable> GetTableColumnsAsync(string tableName)
        {
            DataTable schema = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_sourceConnectionString))
                {
                    await connection.OpenAsync();

                    string query = @"  
                    SELECT   
                        c.name AS COLUMN_NAME,  
                        t.name AS DATA_TYPE,  
                        CASE WHEN t.name IN ('nvarchar','nchar','varchar','char')   
                             THEN c.max_length  
                             ELSE NULL   
                        END AS CHARACTER_MAXIMUM_LENGTH,  
                        CASE WHEN c.is_nullable = 1 THEN 'YES' ELSE 'NO' END AS IS_NULLABLE,  
                        c.precision AS NUMERIC_PRECISION,  
                        c.scale AS NUMERIC_SCALE  
                    FROM sys.columns c  
                    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id  
                    INNER JOIN sys.tables tbl ON c.object_id = tbl.object_id  
                    WHERE tbl.name = @TableName  
                    ORDER BY c.column_id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TableName", tableName);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(schema);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"获取表 {tableName} 的列信息时出错: {ex.Message}");
                throw;
            }

            return schema;
        }

        private async Task<List<string>> GetPrimaryKeysAsync(string tableName)
        {
            List<string> primaryKeys = new List<string>();

            using (SqlConnection connection = new SqlConnection(_sourceConnectionString))
            {
                await connection.OpenAsync();

                string query = @"  
                    SELECT COLUMN_NAME  
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  
                    WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1  
                    AND TABLE_NAME = @TableName  
                    ORDER BY ORDINAL_POSITION";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            primaryKeys.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return primaryKeys;
        }

        private async Task<Dictionary<string, string>> GetDefaultConstraintsAsync(string tableName)
        {
            Dictionary<string, string> defaultConstraints = new Dictionary<string, string>();

            using (SqlConnection connection = new SqlConnection(_sourceConnectionString))
            {
                await connection.OpenAsync();

                string query = @"  
                    SELECT c.name AS ColumnName, dc.definition AS DefaultValue  
                    FROM sys.tables t  
                    INNER JOIN sys.default_constraints dc ON t.object_id = dc.parent_object_id  
                    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id  
                    WHERE t.name = @TableName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            defaultConstraints.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }

            return defaultConstraints;
        }

        private async Task<bool> CheckTableExistsAsync(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(_targetConnectionString))
            {
                await connection.OpenAsync();

                string query = @"  
                    SELECT COUNT(*)  
                    FROM INFORMATION_SCHEMA.TABLES  
                    WHERE TABLE_NAME = @TableName AND TABLE_TYPE = 'BASE TABLE'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);

                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        private async Task SynchronizeTableDataFullAsync(string tableName)
        {
            try
            {
                LogMessage($"开始全量同步表 {tableName} 的数据...");

                using (SqlConnection sourceConnection = new SqlConnection(_sourceConnectionString))
                {
                    await sourceConnection.OpenAsync();

                    DataTable sourceData = new DataTable();
                    string selectQuery = $"SELECT * FROM [{tableName}]";

                    using (SqlCommand command = new SqlCommand(selectQuery, sourceConnection))
                    {
                        command.CommandTimeout = 300;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(sourceData);
                        }
                    }

                    LogMessage($"从源数据库读取到 {sourceData.Rows.Count} 行数据");

                    using (SqlConnection targetConnection = new SqlConnection(_targetConnectionString))
                    {
                        await targetConnection.OpenAsync();

                        using (SqlTransaction transaction = targetConnection.BeginTransaction())
                        {
                            try
                            {
                                LogMessage($"正在清空目标表 {tableName} 的数据...");
                                string truncateQuery = $"DELETE FROM [{tableName}]";

                                using (SqlCommand truncateCommand = new SqlCommand(truncateQuery, targetConnection, transaction))
                                {
                                    truncateCommand.CommandTimeout = 300;
                                    int deletedRows = await truncateCommand.ExecuteNonQueryAsync();
                                    LogMessage($"已清空目标表 {tableName}，删除了 {deletedRows} 行数据");
                                }

                                if (sourceData.Rows.Count > 0)
                                {
                                    LogMessage($"正在向目标表 {tableName} 插入 {sourceData.Rows.Count} 行数据...");

                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(targetConnection, SqlBulkCopyOptions.Default, transaction))
                                    {
                                        bulkCopy.DestinationTableName = tableName;
                                        bulkCopy.BatchSize = (int)numericBatchSize.Value;
                                        bulkCopy.BulkCopyTimeout = 600;
                                        bulkCopy.EnableStreaming = true;

                                        foreach (DataColumn column in sourceData.Columns)
                                        {
                                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                                        }

                                        await bulkCopy.WriteToServerAsync(sourceData);
                                        LogMessage($"成功向目标表 {tableName} 插入 {sourceData.Rows.Count} 行数据");
                                    }
                                }
                                else
                                {
                                    LogMessage($"源表 {tableName} 没有数据，跳过插入操作");
                                }

                                transaction.Commit();
                                LogMessage($"表 {tableName} 全量数据同步完成");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                LogMessage($"表 {tableName} 数据同步失败，已回滚事务: {ex.Message}");
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"全量同步表 {tableName} 数据时出错: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> ValidateSyncResultAsync(string tableName)
        {
            try
            {
                int sourceCount = 0;
                int targetCount = 0;

                using (SqlConnection sourceConnection = new SqlConnection(_sourceConnectionString))
                {
                    await sourceConnection.OpenAsync();
                    string sourceCountQuery = $"SELECT COUNT(*) FROM [{tableName}]";

                    using (SqlCommand command = new SqlCommand(sourceCountQuery, sourceConnection))
                    {
                        sourceCount = (int)await command.ExecuteScalarAsync();
                    }
                }

                using (SqlConnection targetConnection = new SqlConnection(_targetConnectionString))
                {
                    await targetConnection.OpenAsync();
                    string targetCountQuery = $"SELECT COUNT(*) FROM [{tableName}]";

                    using (SqlCommand command = new SqlCommand(targetCountQuery, targetConnection))
                    {
                        targetCount = (int)await command.ExecuteScalarAsync();
                    }
                }

                LogMessage($"表 {tableName} 同步验证: 源表 {sourceCount} 行，目标表 {targetCount} 行");

                bool isValid = sourceCount == targetCount;
                if (!isValid)
                {
                    LogMessage($"警告: 表 {tableName} 同步后行数不一致！");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                LogMessage($"验证表 {tableName} 同步结果时出错: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region 配置保存和加载  
        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "配置文件|*.xml";
                    dialog.Title = "保存配置文件";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var config = new SyncConfig
                        {
                            SourceConnectionString = txtSourceConnection.Text,
                            TargetConnectionString = txtTargetConnection.Text,
                            SyncStructure = chkSyncStructure.Checked,
                            SyncData = chkSyncData.Checked,
                            CreateTargetTables = chkCreateTargetTables.Checked,
                            TruncateTargetTables = chkTruncateTarget.Checked,
                            BatchSize = (int)numericBatchSize.Value,
                            SelectedTables = GetSelectedTables()
                        };

                        var serializer = new XmlSerializer(typeof(SyncConfig));
                        using (var writer = new StreamWriter(dialog.FileName))
                        {
                            serializer.Serialize(writer, config);
                        }

                        MessageBox.Show("配置保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogMessage($"配置已保存到：{dialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"保存配置失败：{ex.Message}");
            }
        }

        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "配置文件|*.xml";
                    dialog.Title = "加载配置文件";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var serializer = new XmlSerializer(typeof(SyncConfig));
                        using (var reader = new StreamReader(dialog.FileName))
                        {
                            var config = (SyncConfig)serializer.Deserialize(reader);

                            txtSourceConnection.Text = config.SourceConnectionString;
                            txtTargetConnection.Text = config.TargetConnectionString;
                            chkSyncStructure.Checked = config.SyncStructure;
                            chkSyncData.Checked = config.SyncData;
                            chkCreateTargetTables.Checked = config.CreateTargetTables;
                            chkTruncateTarget.Checked = config.TruncateTargetTables;
                            numericBatchSize.Value = config.BatchSize;

                            if (config.SelectedTables != null)
                            {
                                for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
                                {
                                    var tableName = checkedListBoxTables.Items[i].ToString();
                                    checkedListBoxTables.SetItemChecked(i, config.SelectedTables.Contains(tableName));
                                }
                            }
                        }

                        MessageBox.Show("配置加载成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogMessage($"配置已从文件加载：{dialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"加载配置失败：{ex.Message}");
            }
        }
        #endregion

        #region 其他事件  
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "SQL Server 数据库同步工具 v1.0\n\n" +
                "功能特性：\n" +
                "- 支持完整的表结构同步（包括列定义、主键、默认值等）\n" +
                "- 支持全量数据同步\n" +
                "- 支持批量处理和事务回滚\n" +
                "- 支持同步结果验证\n" +
                "- 支持配置保存和加载\n" +
                "- 支持同步进度显示和详细日志\n\n" +
                "使用方法：\n" +
                "1. 配置源数据库和目标数据库连接字符串\n" +
                "2. 测试连接确保连接正常\n" +
                "3. 刷新并选择要同步的表\n" +
                "4. 配置同步选项\n" +
                "5. 开始同步操作",
                "关于",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isSyncing)
            {
                var result = MessageBox.Show(
                    "同步操作正在进行中，确定要退出吗？",
                    "确认退出",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _cancellationTokenSource?.Cancel();
            }
        }
        #endregion

        #region 辅助方法  
        private void SetControlsEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(new Action(() => SetControlsEnabledInternal(enabled)));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
                }
            }
            else
            {
                SetControlsEnabledInternal(enabled);
            }
        }

        private void SetControlsEnabledInternal(bool enabled)
        {
            if (!this.IsDisposed)
            {
                btnTestSourceConnection.Enabled = enabled;
                btnTestTargetConnection.Enabled = enabled;
                btnRefreshTables.Enabled = enabled;
                btnSelectAll.Enabled = enabled;
                btnSelectNone.Enabled = enabled;
                btnStartSync.Enabled = enabled && !_isSyncing;
                btnSaveConfig.Enabled = enabled;
                btnLoadConfig.Enabled = enabled;
            }
        }

        private void SetSyncMode(bool syncing)
        {
            if (this.InvokeRequired)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(new Action(() => SetSyncModeInternal(syncing)));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
                }
            }
            else
            {
                SetSyncModeInternal(syncing);
            }
        }

        private void SetSyncModeInternal(bool syncing)
        {
            if (!this.IsDisposed)
            {
                _isSyncing = syncing;
                btnStartSync.Enabled = !syncing;
                btnCancel.Enabled = syncing;

                txtSourceConnection.Enabled = !syncing;
                txtTargetConnection.Enabled = !syncing;
                checkedListBoxTables.Enabled = !syncing;
                groupBoxOptions.Enabled = !syncing;

                toolStripProgressBar.Visible = syncing;
                toolStripStatusLabel.Text = syncing ? "正在同步..." : "就绪";
            }
        }

        private void UpdateProgress(int current, int total)
        {
            if (this.InvokeRequired)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(new Action(() => UpdateProgressInternal(current, total)));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
                }
            }
            else
            {
                UpdateProgressInternal(current, total);
            }
        }

        private void UpdateProgressInternal(int current, int total)
        {
            if (!this.IsDisposed)
            {
                progressBarMain.Value = current;
                labelProgress.Text = $"{current}/{total}";
                toolStripProgressBar.Value = (int)((double)current / total * 100);
            }
        }

        private void LogMessage(string message)
        {
            if (this.InvokeRequired)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(new Action(() => LogMessageInternal(message)));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
                }
            }
            else
            {
                LogMessageInternal(message);
            }
        }

        private void LogMessageInternal(string message)
        {
            if (richTextBoxLog != null && !richTextBoxLog.IsDisposed)
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                richTextBoxLog.AppendText($"[{timestamp}] {message}\n");
                richTextBoxLog.ScrollToCaret();
            }
        }
        #endregion
    }

    #region 配置和选项类
    [Serializable]
    public class SyncConfig
    {
        public string SourceConnectionString { get; set; }
        public string TargetConnectionString { get; set; }
        public bool SyncStructure { get; set; }
        public bool SyncData { get; set; }
        public bool CreateTargetTables { get; set; }
        public bool TruncateTargetTables { get; set; }
        public int BatchSize { get; set; }
        public List<string> SelectedTables { get; set; }
    }

    public class SyncOptions
    {
        public bool SyncStructure { get; set; }
        public bool SyncData { get; set; }
        public bool CreateTargetTables { get; set; }
        public bool TruncateTargetTables { get; set; }
        public int BatchSize { get; set; }
    }
    #endregion
}