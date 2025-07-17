using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AppSqlserverToMySql
{
    public class DatabaseConverter
    {
        // 连接字符串存储
        private string sqlServerConnStr;
        private string mysqlConnStr;

        // 事件委托，用于向UI报告进度
        public event Action<string> OnProgress;
        public event Action<string, Exception> OnError;

        public DatabaseConverter(string sqlServerConnStr, string mysqlConnStr)
        {
            this.sqlServerConnStr = sqlServerConnStr;
            this.mysqlConnStr = mysqlConnStr;
        }

        #region 主要转换方法

        /// <summary>
        /// 一键转换主方法 - 这是整个流程的入口
        /// 同时同步表结构和数据
        /// </summary>
        /// <param name="tableName">要转换的表名</param>
        public void Convert(string tableName)
        {
            try
            {
                ReportProgress($"🚀 开始转换表：{tableName}");

                // 步骤1：获取SQL Server表结构
                DataTable schema = GetSqlServerTableSchema(tableName);
                ReportProgress("✅ 表结构获取完成");

                // 步骤2：创建MySQL表
                CreateMySqlTable(schema, tableName);
                ReportProgress("✅ MySQL表创建完成");

                // 步骤3：同步数据
                SyncData(tableName);
                ReportProgress($"🎉 表 {tableName} 转换完成！");
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ 转换过程出错：{ex.Message}";
                ReportProgress(errorMsg);
                OnError?.Invoke($"表 {tableName} 转换失败", ex);
                throw;
            }
        }

        /// <summary>
        /// 仅同步表结构
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ConvertStructureOnly(string tableName)
        {
            try
            {
                ReportProgress($"🏗️ 开始创建表结构：{tableName}");

                // 获取SQL Server表结构
                DataTable schema = GetSqlServerTableSchema(tableName);
                ReportProgress("✅ 表结构获取完成");

                // 创建MySQL表
                CreateMySqlTable(schema, tableName);
                ReportProgress($"🎉 表 {tableName} 结构创建完成！");
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ 表结构创建失败：{ex.Message}";
                ReportProgress(errorMsg);
                OnError?.Invoke($"表 {tableName} 结构创建失败", ex);
                throw;
            }
        }

        /// <summary>
        /// 仅同步数据（假设表已存在）
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ConvertDataOnly(string tableName)
        {
            try
            {
                ReportProgress($"📦 开始同步数据：{tableName}");

                // 检查目标表是否存在
                if (!CheckMySqlTableExists(tableName))
                {
                    throw new Exception($"目标表 {tableName} 不存在，请先同步表结构");
                }

                // 清空目标表数据（可选）
                // ClearMySqlTable(tableName);

                // 同步数据
                SyncData(tableName);
                ReportProgress($"🎉 表 {tableName} 数据同步完成！");
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ 数据同步失败：{ex.Message}";
                ReportProgress(errorMsg);
                OnError?.Invoke($"表 {tableName} 数据同步失败", ex);
                throw;
            }
        }

        #endregion

        #region 表结构获取方法

        /// <summary>
        /// 从SQL Server获取完整表结构信息
        /// 包括：列名、数据类型、长度、是否允许NULL、是否自增、主键等
        /// </summary>
        private DataTable GetSqlServerTableSchema(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(sqlServerConnStr))
            {
                conn.Open();

                // 🔍 关键SQL：获取表的完整结构信息
                string query = @"
                    SELECT 
                        COLUMN_NAME,                    -- 列名
                        DATA_TYPE,                      -- 数据类型
                        CHARACTER_MAXIMUM_LENGTH,       -- 最大长度
                        NUMERIC_PRECISION,              -- 数值精度
                        NUMERIC_SCALE,                  -- 数值标度
                        IS_NULLABLE,                    -- 是否允许NULL
                        COLUMN_DEFAULT,                 -- 默认值
                        CASE WHEN COLUMNPROPERTY(OBJECT_ID(@TableName), COLUMN_NAME, 'IsIdentity') = 1 
                             THEN 'YES' 
                             ELSE 'NO' 
                        END AS IS_IDENTITY,             -- 是否自增
                        ORDINAL_POSITION                -- 列顺序
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = @TableName 
                    ORDER BY ORDINAL_POSITION";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // 🛡️ 使用参数化查询防止SQL注入
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    DataTable schema = new DataTable();
                    schema.Load(cmd.ExecuteReader());

                    if (schema.Rows.Count == 0)
                    {
                        throw new Exception($"表 {tableName} 不存在或无权限访问");
                    }

                    return schema;
                }
            }
        }

        /// <summary>
        /// 获取表的主键信息
        /// </summary>
        private string[] GetPrimaryKeys(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(sqlServerConnStr))
            {
                conn.Open();

                string query = @"
                    SELECT COLUMN_NAME
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                    WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA+'.'+CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                    AND TABLE_NAME = @TableName
                    ORDER BY ORDINAL_POSITION";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    var primaryKeys = new System.Collections.Generic.List<string>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            primaryKeys.Add(reader["COLUMN_NAME"].ToString());
                        }
                    }
                    return primaryKeys.ToArray();
                }
            }
        }

        #endregion

        #region MySQL表创建方法

        /// <summary>
        /// 根据SQL Server表结构创建对应的MySQL表
        /// 自动处理数据类型映射和约束转换
        /// </summary>
        private void CreateMySqlTable(DataTable schema, string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(mysqlConnStr))
            {
                conn.Open();

                // 先删除表（如果存在）
                using (MySqlCommand dropCmd = new MySqlCommand($"DROP TABLE IF EXISTS `{tableName}`", conn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                StringBuilder createTableSql = new StringBuilder();
                createTableSql.AppendLine($"CREATE TABLE `{tableName}` (");

                // 获取主键信息
                string[] primaryKeys = GetPrimaryKeys(tableName);

                // 🔄 遍历所有列，构建CREATE TABLE语句
                for (int i = 0; i < schema.Rows.Count; i++)
                {
                    DataRow row = schema.Rows[i];
                    string columnName = row["COLUMN_NAME"].ToString();
                    string dataType = row["DATA_TYPE"].ToString();
                    string maxLength = row["CHARACTER_MAXIMUM_LENGTH"]?.ToString() ?? "";
                    string precision = row["NUMERIC_PRECISION"]?.ToString() ?? "";
                    string scale = row["NUMERIC_SCALE"]?.ToString() ?? "";
                    string isNullable = row["IS_NULLABLE"].ToString();
                    string isIdentity = row["IS_IDENTITY"].ToString();
                    bool isPrimaryKey = primaryKeys.Contains(columnName);

                    // 构建列定义
                    createTableSql.Append($"    `{columnName}` {ConvertDataType(dataType, maxLength, precision, scale, isPrimaryKey)}");

                    // 🚀 处理自增属性
                    if (isIdentity == "YES")
                    {
                        createTableSql.Append(" AUTO_INCREMENT");
                    }

                    // 🔒 处理NULL约束
                    if (isNullable == "NO")
                    {
                        createTableSql.Append(" NOT NULL");
                    }

                    // 添加逗号分隔符（最后一列除外）
                    if (i < schema.Rows.Count - 1)
                    {
                        createTableSql.AppendLine(",");
                    }
                }

                // 🔑 添加主键约束
                if (primaryKeys.Length > 0)
                {
                    createTableSql.AppendLine(",");
                    string pkColumns = string.Join("`, `", primaryKeys);
                    createTableSql.AppendLine($"    PRIMARY KEY (`{pkColumns}`)");
                }

                createTableSql.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

                using (MySqlCommand cmd = new MySqlCommand(createTableSql.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 检查MySQL表是否存在
        /// </summary>
        private bool CheckMySqlTableExists(string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(mysqlConnStr))
            {
                conn.Open();

                string query = @"
                    SELECT COUNT(*) 
                    FROM information_schema.tables 
                    WHERE table_schema = DATABASE() 
                    AND table_name = @tableName";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    int count = System.Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// 清空MySQL表数据
        /// </summary>
        private void ClearMySqlTable(string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(mysqlConnStr))
            {
                conn.Open();

                // 禁用外键检查
                using (MySqlCommand cmd1 = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0", conn))
                {
                    cmd1.ExecuteNonQuery();
                }

                // 清空表数据
                using (MySqlCommand cmd2 = new MySqlCommand($"TRUNCATE TABLE `{tableName}`", conn))
                {
                    cmd2.ExecuteNonQuery();
                }

                // 重新启用外键检查
                using (MySqlCommand cmd3 = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1", conn))
                {
                    cmd3.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region 数据类型智能映射

        /// <summary>
        /// SQL Server数据类型到MySQL数据类型的智能映射
        /// 这是迁移成功的关键！
        /// </summary>
        private string ConvertDataType(string sqlServerType, string maxLength, string precision, string scale, bool isPrimaryKey = false)
        {
            switch (sqlServerType.ToLower())
            {
                // 🔢 整数类型映射
                case "int":
                    return "int(11)";
                case "bigint":
                    return "bigint(20)";
                case "smallint":
                    return "smallint(6)";
                case "tinyint":
                    return "tinyint(4)";
                case "bit":
                    return "bit(1)";

                // 💰 数值类型映射
                case "decimal":
                case "numeric":
                    if (!string.IsNullOrEmpty(precision) && !string.IsNullOrEmpty(scale))
                    {
                        return $"decimal({precision},{scale})";
                    }
                    return "decimal(18,2)";
                case "money":
                    return "decimal(19,4)";
                case "smallmoney":
                    return "decimal(10,4)";
                case "float":
                    return "double";
                case "real":
                    return "float";

                // 📅 日期时间类型映射
                case "datetime":
                case "datetime2":
                    return "datetime";
                case "smalldatetime":
                    return "datetime";
                case "date":
                    return "date";
                case "time":
                    return "time";
                case "timestamp":
                    return "timestamp";
                case "datetimeoffset":
                    return "datetime"; // MySQL没有时区偏移，转为普通datetime

                // 📝 字符串类型映射
                case "char":
                case "nchar":
                    if (!string.IsNullOrEmpty(maxLength))
                    {
                        int length = int.Parse(maxLength);
                        return $"char({Math.Min(length, 255)})";
                    }
                    return "char(1)";

                case "varchar":
                case "nvarchar":
                    if (maxLength == "-1") // MAX类型
                    {
                        return isPrimaryKey ? "varchar(255)" : "longtext";
                    }
                    else if (!string.IsNullOrEmpty(maxLength))
                    {
                        int length = int.Parse(maxLength);
                        if (isPrimaryKey && length > 255)
                        {
                            return "varchar(255)";
                        }
                        if (length > 65535)
                        {
                            return "longtext";
                        }
                        if (length > 16383)
                        {
                            return "mediumtext";
                        }
                        if (length > 255)
                        {
                            return "text";
                        }
                        return $"varchar({length})";
                    }
                    return isPrimaryKey ? "varchar(255)" : "text";

                case "text":
                case "ntext":
                    return isPrimaryKey ? "varchar(255)" : "longtext";

                // 🆔 唯一标识符类型
                case "uniqueidentifier":
                    return "char(36)";

                // 📦 二进制类型映射
                case "binary":
                    if (!string.IsNullOrEmpty(maxLength))
                    {
                        return $"binary({maxLength})";
                    }
                    return "binary(1)";
                case "varbinary":
                    if (maxLength == "-1")
                    {
                        return "longblob";
                    }
                    else if (!string.IsNullOrEmpty(maxLength))
                    {
                        int length = int.Parse(maxLength);
                        if (length > 16777215)
                        {
                            return "longblob";
                        }
                        if (length > 65535)
                        {
                            return "mediumblob";
                        }
                        if (length > 255)
                        {
                            return "blob";
                        }
                        return $"varbinary({length})";
                    }
                    return "blob";
                case "image":
                    return "longblob";

                // 🔧 XML和其他特殊类型
                case "xml":
                    return isPrimaryKey ? "varchar(255)" : "longtext";
                case "sql_variant":
                    return isPrimaryKey ? "varchar(255)" : "text";

                // 🔧 默认处理
                default:
                    ReportProgress($"⚠️ 未知数据类型 {sqlServerType}，使用默认映射");
                    return isPrimaryKey ? "varchar(255)" : "text";
            }
        }

        #endregion

        #region 数据同步核心方法

        /// <summary>
        /// 核心数据同步方法 - 处理大数据量迁移
        /// 包含字符转义、NULL值处理等关键逻辑
        /// </summary>
        private void SyncData(string tableName)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlServerConnStr))
            using (MySqlConnection mysqlConn = new MySqlConnection(mysqlConnStr))
            {
                sqlConn.Open();
                mysqlConn.Open();

                // 📖 读取源数据
                using (SqlCommand sqlCmd = new SqlCommand($"SELECT * FROM [{tableName}]", sqlConn))
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    DataTable schemaTable = reader.GetSchemaTable();

                    if (reader.HasRows)
                    {
                        int recordCount = 0;
                        int batchSize = 1000; // 批量处理大小
                        var batchCommands = new System.Collections.Generic.List<string>();

                        // 🔄 逐行处理数据
                        while (reader.Read())
                        {
                            StringBuilder insertSql = new StringBuilder();
                            insertSql.Append($"INSERT INTO `{tableName}` (");

                            // 🏷️ 构建列名部分
                            var columnNames = new System.Collections.Generic.List<string>();
                            for (int i = 0; i < schemaTable.Rows.Count; i++)
                            {
                                string columnName = schemaTable.Rows[i]["ColumnName"].ToString();
                                columnNames.Add($"`{columnName}`");
                            }
                            insertSql.Append(string.Join(",", columnNames));

                            insertSql.Append(") VALUES (");

                            // 💾 构建值部分
                            var values = new System.Collections.Generic.List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.IsDBNull(i))
                                {
                                    values.Add("NULL");
                                }
                                else
                                {
                                    // 🔍 获取列的数据类型
                                    string dataTypeName = schemaTable.Rows[i]["DataTypeName"].ToString();
                                    object value = reader.GetValue(i);

                                    values.Add(FormatValueForMySQL(value, dataTypeName));
                                }
                            }

                            insertSql.Append(string.Join(",", values));
                            insertSql.Append(")");

                            batchCommands.Add(insertSql.ToString());
                            recordCount++;

                            // 📊 批量执行和进度报告
                            if (batchCommands.Count >= batchSize)
                            {
                                ExecuteBatch(mysqlConn, batchCommands);
                                batchCommands.Clear();
                                ReportProgress($"已处理 {recordCount} 条记录...");
                            }
                        }

                        // 执行剩余的批量命令
                        if (batchCommands.Count > 0)
                        {
                            ExecuteBatch(mysqlConn, batchCommands);
                        }

                        ReportProgress($"✅ 数据同步完成，共处理 {recordCount} 条记录");
                    }
                    else
                    {
                        ReportProgress("⚠️ 源表无数据");
                    }
                }
            }
        }

        /// <summary>
        /// 批量执行SQL命令
        /// </summary>
        private void ExecuteBatch(MySqlConnection connection, System.Collections.Generic.List<string> commands)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (string sql in commands)
                    {
                        using (var cmd = new MySqlCommand(sql, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 格式化值以适应MySQL
        /// </summary>
        private string FormatValueForMySQL(object value, string dataTypeName)
        {
            if (value == null || value == DBNull.Value)
            {
                return "NULL";
            }

            switch (dataTypeName.ToLower())
            {
                // 📅 日期时间类型处理
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "date":
                    DateTime dateTime = System.Convert.ToDateTime(value);
                    return $"'{dateTime:yyyy-MM-dd HH:mm:ss}'";

                case "time":
                    if (value is TimeSpan timeSpan)
                    {
                        return $"'{timeSpan:hh\\:mm\\:ss}'";
                    }
                    return $"'{value}'";

                // 🔢 布尔和位类型
                case "bit":
                    bool bitValue = System.Convert.ToBoolean(value);
                    return bitValue ? "1" : "0";

                // 🔢 数值类型
                case "int":
                case "bigint":
                case "smallint":
                case "tinyint":
                case "decimal":
                case "numeric":
                case "float":
                case "real":
                case "money":
                case "smallmoney":
                    return value.ToString();

                // 📦 二进制类型
                case "binary":
                case "varbinary":
                case "image":
                    byte[] bytes = (byte[])value;
                    return $"0x{BitConverter.ToString(bytes).Replace("-", "")}";

                // 📝 字符串和其他类型
                default:
                    string stringValue = value.ToString();
                    // 🛡️ SQL注入防护 - 转义特殊字符
                    stringValue = EscapeStringForMySQL(stringValue);
                    return $"'{stringValue}'";
            }
        }

        /// <summary>
        /// MySQL字符串转义
        /// </summary>
        private string EscapeStringForMySQL(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Replace("\\", "\\\\")
                       .Replace("'", "\\'")
                       .Replace("\"", "\\\"")
                       .Replace("\r", "\\r")
                       .Replace("\n", "\\n")
                       .Replace("\t", "\\t")
                       .Replace("\0", "\\0");
        }

        /// <summary>
        /// 判断是否为日期时间类型
        /// </summary>
        private bool IsDateTimeType(string dataTypeName)
        {
            string[] dateTimeTypes = {
                "DateTime", "DateTime2", "Date", "Time",
                "SmallDateTime", "DateTimeOffset", "Timestamp"
            };

            return dateTimeTypes.Contains(dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 报告进度
        /// </summary>
        private void ReportProgress(string message)
        {
            Console.WriteLine(message);
            OnProgress?.Invoke(message);
        }

        /// <summary>
        /// 获取表的记录数
        /// </summary>
        public long GetTableRecordCount(string tableName, bool isSourceTable = true)
        {
            string connectionString = isSourceTable ? sqlServerConnStr : mysqlConnStr;

            if (isSourceTable)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM [{tableName}]", conn))
                    {
                        return System.Convert.ToInt64(cmd.ExecuteScalar());
                    }
                }
            }
            else
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand($"SELECT COUNT(*) FROM `{tableName}`", conn))
                    {
                        return System.Convert.ToInt64(cmd.ExecuteScalar());
                    }
                }
            }
        }

        /// <summary>
        /// 验证同步结果
        /// </summary>
        public bool ValidateSyncResult(string tableName)
        {
            try
            {
                long sourceCount = GetTableRecordCount(tableName, true);
                long targetCount = GetTableRecordCount(tableName, false);

                ReportProgress($"📊 验证结果 - 源表记录数：{sourceCount}，目标表记录数：{targetCount}");

                return sourceCount == targetCount;
            }
            catch (Exception ex)
            {
                ReportProgress($"⚠️ 验证同步结果时出错：{ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
