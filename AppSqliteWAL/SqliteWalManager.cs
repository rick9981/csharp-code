using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSqliteWAL
{
    public class SqliteWalManager
    {
        private string _connectionString;

        public SqliteWalManager(string dbPath)
        {
            // 注意：连接字符串中不要设置journal mode
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        /// <summary>
        /// 启用WAL模式的标准方法
        /// </summary>
        public bool EnableWalMode()
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                // 核心代码：启用WAL模式
                using var command = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection);
                string result = command.ExecuteScalar()?.ToString();

                // 验证是否成功启用
                bool success = result?.Equals("wal", StringComparison.OrdinalIgnoreCase) == true;

                if (success)
                {
                    Console.WriteLine("✅ WAL模式启用成功！");
                    // 可选：配置WAL相关参数优化性能
                    ConfigureWalParameters(connection);
                }
                else
                {
                    Console.WriteLine("❌ WAL模式启用失败！");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"启用WAL模式异常：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 配置WAL模式相关参数
        /// </summary>
        private void ConfigureWalParameters(SQLiteConnection connection)
        {
            using var command = connection.CreateCommand();

            // 设置WAL自动检查点大小（默认1000页，可根据实际情况调整）
            command.CommandText = "PRAGMA wal_autocheckpoint=2000;";
            command.ExecuteNonQuery();

            // 设置同步模式为NORMAL（平衡性能和安全性）
            command.CommandText = "PRAGMA synchronous=NORMAL;";
            command.ExecuteNonQuery();

            // 设置缓存大小（提高查询性能）
            command.CommandText = "PRAGMA cache_size=10000;";
            command.ExecuteNonQuery();

            Console.WriteLine("🔧 WAL参数配置完成");
        }
    }
}
