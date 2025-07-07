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

        #region �¼���  
        private void InitializeEvents()
        {
            // ���Ӳ��԰�ť�¼�  
            btnTestSourceConnection.Click += BtnTestSourceConnection_Click;
            btnTestTargetConnection.Click += BtnTestTargetConnection_Click;

            // �������ť�¼�  
            btnRefreshTables.Click += BtnRefreshTables_Click;
            btnSelectAll.Click += BtnSelectAll_Click;
            btnSelectNone.Click += BtnSelectNone_Click;

            // ͬ��������ť�¼�  
            btnStartSync.Click += BtnStartSync_Click;
            btnCancel.Click += BtnCancel_Click;

            // ���ò�����ť�¼�  
            btnSaveConfig.Click += BtnSaveConfig_Click;
            btnLoadConfig.Click += BtnLoadConfig_Click;

            // �˳���ť�¼�  
            btnExit.Click += BtnExit_Click;

            // �˵��¼�  
            saveConfigToolStripMenuItem.Click += BtnSaveConfig_Click;
            loadConfigToolStripMenuItem.Click += BtnLoadConfig_Click;
            exitToolStripMenuItem.Click += BtnExit_Click;
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

            // ����ر��¼�  
            this.FormClosing += FrmMain_FormClosing;
        }
        #endregion

        #region ��ʼ��  
        private void InitializeForm()
        {
            // ����Ĭ�������ַ���ʾ��  
            txtSourceConnection.Text = "Server=.;Database=SourceDB;Integrated Security=true;";
            txtTargetConnection.Text = "Server=.;Database=TargetDB;Integrated Security=true;";

            // ����Ĭ��ֵ  
            numericBatchSize.Value = 1000;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LogMessage("����������ɣ����������ݿ�������Ϣ��");
        }
        #endregion

        #region ���Ӳ���  
        private async void BtnTestSourceConnection_Click(object sender, EventArgs e)
        {
            await TestConnection(txtSourceConnection.Text, "Դ���ݿ�");
        }

        private async void BtnTestTargetConnection_Click(object sender, EventArgs e)
        {
            await TestConnection(txtTargetConnection.Text, "Ŀ�����ݿ�");
        }

        private async Task TestConnection(string connectionString, string dbName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show($"������{dbName}�����ַ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetControlsEnabled(false);
                toolStripStatusLabel.Text = $"���ڲ���{dbName}����...";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    MessageBox.Show($"{dbName}���Ӳ��Գɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogMessage($"{dbName}���Ӳ��Գɹ���");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{dbName}���Ӳ���ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"{dbName}���Ӳ���ʧ�ܣ�{ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
                toolStripStatusLabel.Text = "����";
            }
        }
        #endregion

        #region �����  
        private async void BtnRefreshTables_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSourceConnection.Text))
            {
                MessageBox.Show("��������Դ���ݿ������ַ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetControlsEnabled(false);
                toolStripStatusLabel.Text = "���ڻ�ȡ���б�...";
                checkedListBoxTables.Items.Clear();

                _sourceConnectionString = txtSourceConnection.Text;
                var tables = await GetTableListAsync(_sourceConnectionString);
                foreach (var table in tables)
                {
                    checkedListBoxTables.Items.Add(table);
                }

                LogMessage($"�ɹ���ȡ {tables.Count} ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡ���б�ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"��ȡ���б�ʧ�ܣ�{ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
                toolStripStatusLabel.Text = "����";
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                checkedListBoxTables.SetItemChecked(i, true);
            }
            LogMessage($"��ѡ������ {checkedListBoxTables.Items.Count} ����");
        }

        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                checkedListBoxTables.SetItemChecked(i, false);
            }
            LogMessage("��ȡ��ѡ�����б�");
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

        #region ͬ������  
        private async void BtnStartSync_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
                return;

            var selectedTables = GetSelectedTables();
            if (selectedTables.Count == 0)
            {
                MessageBox.Show("������ѡ��һ��Ҫͬ���ı�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"ȷ��Ҫͬ�� {selectedTables.Count} ������\n\n" +
                $"ͬ��ѡ�\n" +
                $"- ͬ���ṹ��{(chkSyncStructure.Checked ? "��" : "��")}\n" +
                $"- ͬ�����ݣ�{(chkSyncData.Checked ? "��" : "��")}\n" +
                $"- ����Ŀ���{(chkCreateTargetTables.Checked ? "��" : "��")}\n" +
                $"- ���Ŀ���{(chkTruncateTarget.Checked ? "��" : "��")}\n" +
                $"- ���δ�С��{numericBatchSize.Value}",
                "ȷ��ͬ��",
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
                LogMessage("�û�ȡ����ͬ��������");
            }
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(txtSourceConnection.Text))
            {
                MessageBox.Show("������Դ���ݿ������ַ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTargetConnection.Text))
            {
                MessageBox.Show("������Ŀ�����ݿ������ַ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!chkSyncStructure.Checked && !chkSyncData.Checked)
            {
                MessageBox.Show("������ѡ��һ��ͬ��ѡ�ͬ���ṹ��ͬ�����ݣ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                LogMessage("��ʼͬ������...");
                LogMessage($"����ͬ�� {tables.Count} ����");

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
                    LogMessage("ͬ��������ɣ�");
                    MessageBox.Show("ͬ��������ɣ�", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                LogMessage("ͬ��������ȡ����");
            }
            catch (Exception ex)
            {
                LogMessage($"ͬ��������������{ex.Message}");
                MessageBox.Show($"ͬ��������������{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LogMessage($"����ͬ����{tableName} ({i + 1}/{tables.Count})");

                try
                {
                    // 1. ͬ����ṹ  
                    if (options.SyncStructure && options.CreateTargetTables)
                    {
                        LogMessage($"����ͬ���� {tableName} �Ľṹ...");
                        await SynchronizeTableStructureAsync(tableName);
                    }

                    // 2. ͬ������  
                    if (options.SyncData)
                    {
                        LogMessage($"����ͬ���� {tableName} ������...");
                        await SynchronizeTableDataFullAsync(tableName);

                        // 3. ��֤ͬ�����  
                        LogMessage($"������֤�� {tableName} ��ͬ�����...");
                        bool isValid = await ValidateSyncResultAsync(tableName);
                        LogMessage($"�� {tableName} ͬ����֤�����{(isValid ? "�ɹ�" : "ʧ��")}");
                    }

                    LogMessage($"�� {tableName} ͬ����ɡ�");
                }
                catch (Exception ex)
                {
                    LogMessage($"�� {tableName} ͬ��ʧ�ܣ�{ex.Message}");
                    // ����������һ�������ж���������  
                }

                // ���½���  
                UpdateProgress(i + 1, tables.Count);
            }
        }

        // ����ͬ������ - ���ɵ�����  
        private async Task SynchronizeTableStructureAsync(string tableName)
        {
            try
            {
                LogMessage($"��ȡ�� {tableName} ������Ϣ...");
                DataTable columnsSchema = await GetTableColumnsAsync(tableName);

                if (columnsSchema == null || columnsSchema.Rows.Count == 0)
                {
                    LogMessage($"����: �� {tableName} û������Ϣ�򲻴���");
                    return;
                }

                LogMessage($"��ȡ���� {tableName} �� {columnsSchema.Rows.Count} ����Ϣ");

                List<string> primaryKeys = await GetPrimaryKeysAsync(tableName);
                LogMessage($"�� {tableName} ��������: {string.Join(", ", primaryKeys)}");

                Dictionary<string, string> defaultConstraints = await GetDefaultConstraintsAsync(tableName);
                LogMessage($"�� {tableName} ��Ĭ��ֵԼ��: {defaultConstraints.Count} ��");

                bool tableExists = await CheckTableExistsAsync(tableName);
                LogMessage($"Ŀ�����ݿ��б� {tableName} {(tableExists ? "�Ѵ���" : "������")}");

                using (SqlConnection targetConnection = new SqlConnection(_targetConnectionString))
                {
                    await targetConnection.OpenAsync();

                    using (SqlTransaction transaction = targetConnection.BeginTransaction())
                    {
                        try
                        {
                            if (tableExists)
                            {
                                LogMessage($"����ɾ��Ŀ��� {tableName}");
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
                                throw new Exception($"�� {tableName} û����Ч���ж���");
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
                            LogMessage($"�� {tableName} �ṹͬ���ɹ�");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogMessage($"�� {tableName} �ṹͬ��ʧ��: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"����� {tableName} ʱ��������: {ex.Message}");
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
                LogMessage($"��ȡ�� {tableName} ������Ϣʱ����: {ex.Message}");
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
                LogMessage($"��ʼȫ��ͬ���� {tableName} ������...");

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

                    LogMessage($"��Դ���ݿ��ȡ�� {sourceData.Rows.Count} ������");

                    using (SqlConnection targetConnection = new SqlConnection(_targetConnectionString))
                    {
                        await targetConnection.OpenAsync();

                        using (SqlTransaction transaction = targetConnection.BeginTransaction())
                        {
                            try
                            {
                                LogMessage($"�������Ŀ��� {tableName} ������...");
                                string truncateQuery = $"DELETE FROM [{tableName}]";

                                using (SqlCommand truncateCommand = new SqlCommand(truncateQuery, targetConnection, transaction))
                                {
                                    truncateCommand.CommandTimeout = 300;
                                    int deletedRows = await truncateCommand.ExecuteNonQueryAsync();
                                    LogMessage($"�����Ŀ��� {tableName}��ɾ���� {deletedRows} ������");
                                }

                                if (sourceData.Rows.Count > 0)
                                {
                                    LogMessage($"������Ŀ��� {tableName} ���� {sourceData.Rows.Count} ������...");

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
                                        LogMessage($"�ɹ���Ŀ��� {tableName} ���� {sourceData.Rows.Count} ������");
                                    }
                                }
                                else
                                {
                                    LogMessage($"Դ�� {tableName} û�����ݣ������������");
                                }

                                transaction.Commit();
                                LogMessage($"�� {tableName} ȫ������ͬ�����");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                LogMessage($"�� {tableName} ����ͬ��ʧ�ܣ��ѻع�����: {ex.Message}");
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"ȫ��ͬ���� {tableName} ����ʱ����: {ex.Message}");
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

                LogMessage($"�� {tableName} ͬ����֤: Դ�� {sourceCount} �У�Ŀ��� {targetCount} ��");

                bool isValid = sourceCount == targetCount;
                if (!isValid)
                {
                    LogMessage($"����: �� {tableName} ͬ����������һ�£�");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                LogMessage($"��֤�� {tableName} ͬ�����ʱ����: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region ���ñ���ͼ���  
        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "�����ļ�|*.xml";
                    dialog.Title = "���������ļ�";
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

                        MessageBox.Show("���ñ���ɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogMessage($"�����ѱ��浽��{dialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"��������ʧ�ܣ�{ex.Message}");
            }
        }

        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "�����ļ�|*.xml";
                    dialog.Title = "���������ļ�";
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

                        MessageBox.Show("���ü��سɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogMessage($"�����Ѵ��ļ����أ�{dialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"��������ʧ�ܣ�{ex.Message}");
            }
        }
        #endregion

        #region �����¼�  
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "SQL Server ���ݿ�ͬ������ v1.0\n\n" +
                "�������ԣ�\n" +
                "- ֧�������ı�ṹͬ���������ж��塢������Ĭ��ֵ�ȣ�\n" +
                "- ֧��ȫ������ͬ��\n" +
                "- ֧���������������ع�\n" +
                "- ֧��ͬ�������֤\n" +
                "- ֧�����ñ���ͼ���\n" +
                "- ֧��ͬ��������ʾ����ϸ��־\n\n" +
                "ʹ�÷�����\n" +
                "1. ����Դ���ݿ��Ŀ�����ݿ������ַ���\n" +
                "2. ��������ȷ����������\n" +
                "3. ˢ�²�ѡ��Ҫͬ���ı�\n" +
                "4. ����ͬ��ѡ��\n" +
                "5. ��ʼͬ������",
                "����",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isSyncing)
            {
                var result = MessageBox.Show(
                    "ͬ���������ڽ����У�ȷ��Ҫ�˳���",
                    "ȷ���˳�",
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

        #region ��������  
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
                toolStripStatusLabel.Text = syncing ? "����ͬ��..." : "����";
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

    #region ���ú�ѡ����
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