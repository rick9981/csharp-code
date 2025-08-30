using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSqliteBatchInsert
{
    public static class DbHelper
    {
        // 连接字符串可以来自配置文件，并加上合适的PRAGMA来调优。
        private static string GetConnectionString(string dbFile)
        {
            return new SQLiteConnectionStringBuilder
            {
                DataSource = dbFile,
                JournalMode = SQLiteJournalModeEnum.Wal, // WAL 模式通常能提高并发与写入性能
                SyncMode = SynchronizationModes.Normal, // 根据需要调整：Off/Fast/Normal/Full
                CacheSize = 2000, // 调整缓存页数（可影响内存占用）
            }.ToString();
        }

        public static void CreateDatabaseIfNotExists(string dbFile)
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }
        }

        public static void CreateSampleTable(string dbFile)
        {
            using (var conn = new SQLiteConnection(GetConnectionString(dbFile)))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // 提供某种传感器数据表
                    cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS sensor_data (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        device_id TEXT NOT NULL,
                        timestamp INTEGER NOT NULL, -- unix epoch ms
                        temperature REAL,
                        pressure REAL,
                        status INTEGER
                    );
                    CREATE INDEX IF NOT EXISTS idx_sensor_timestamp ON sensor_data(timestamp);
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void TruncateTable(string dbFile)
        {
            using (var conn = new SQLiteConnection(GetConnectionString(dbFile)))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // 使用 DELETE FROM table; 然后执行 VACUUM 如果需要释放磁盘空间
                    cmd.CommandText = "DELETE FROM sensor_data;";
                    cmd.ExecuteNonQuery();
                    // 如果确实需要回收空间，可执行 VACUUM，但会阻塞且可能慢
                    cmd.CommandText = "VACUUM;";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 批量插入：使用事务 + 预编译参数化 SQL + 分批提交
        // progressCallback 可用于 UI 更新
        public static void BulkInsert(string dbFile, int totalRows, int batchSize, Action<int> progressCallback = null)
        {
            if (totalRows <= 0) return;
            if (batchSize <= 0) batchSize = 1000;

            using (var conn = new SQLiteConnection(GetConnectionString(dbFile)))
            {
                conn.Open();

                // 提高插入速度的建议 PRAGMA
                using (var pragmaCmd = conn.CreateCommand())
                {
                    pragmaCmd.CommandText = @"
                    PRAGMA synchronous = OFF;
                    PRAGMA journal_mode = WAL;
                    PRAGMA temp_store = MEMORY;
                    PRAGMA locking_mode = EXCLUSIVE;
                    ";
                    pragmaCmd.ExecuteNonQuery();
                }

                // 预编译插入语句
                using (var insertCmd = conn.CreateCommand())
                {
                    insertCmd.CommandText = @"
                    INSERT INTO sensor_data (device_id, timestamp, temperature, pressure, status)
                    VALUES (@device_id, @timestamp, @temperature, @pressure, @status);
                    ";
                    // 创建参数并重用
                    insertCmd.Parameters.Add(new SQLiteParameter("@device_id", DbType.String));
                    insertCmd.Parameters.Add(new SQLiteParameter("@timestamp", DbType.Int64));
                    insertCmd.Parameters.Add(new SQLiteParameter("@temperature", DbType.Double));
                    insertCmd.Parameters.Add(new SQLiteParameter("@pressure", DbType.Double));
                    insertCmd.Parameters.Add(new SQLiteParameter("@status", DbType.Int32));

                    int inserted = 0;
                    // 分批次开始事务，防止单次事务过大导致内存或 WAL 文件过大
                    while (inserted < totalRows)
                    {
                        int currentBatch = Math.Min(batchSize, totalRows - inserted);

                        using (var tran = conn.BeginTransaction())
                        {
                            insertCmd.Transaction = tran;

                            for (int i = 0; i < currentBatch; i++)
                            {
                                string deviceId = $"DEV-{(inserted + i) % 1000:D4}";
                                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                double temperature = 20.0 + ((inserted + i) % 100) * 0.01;
                                double pressure = 1000.0 + ((inserted + i) % 50) * 0.1;
                                int status = ((inserted + i) % 5 == 0) ? 1 : 0;

                                // 设置参数值
                                insertCmd.Parameters["@device_id"].Value = deviceId;
                                insertCmd.Parameters["@timestamp"].Value = timestamp;
                                insertCmd.Parameters["@temperature"].Value = temperature;
                                insertCmd.Parameters["@pressure"].Value = pressure;
                                insertCmd.Parameters["@status"].Value = status;

                                insertCmd.ExecuteNonQuery();
                            }

                            // 提交当前批次事务
                            tran.Commit();
                        }

                        inserted += currentBatch;
                        progressCallback?.Invoke(inserted);
                    }
                }

                // 建议在大批量写入后可以恢复一些 PRAGMA
                using (var pragmaCmd2 = conn.CreateCommand())
                {
                    pragmaCmd2.CommandText = "PRAGMA synchronous = NORMAL;";
                    pragmaCmd2.ExecuteNonQuery();
                }
            }
        }
    }
}
