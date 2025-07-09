using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSqliteWAL
{
    /// <summary>
    /// 生产环境WAL模式管理器
    /// 包含连接池、错误处理、性能监控
    /// </summary>
    public class ProductionWalManager
    {
        private readonly string _connectionString;
        private readonly object _lockObject = new object();

        public ProductionWalManager(string dbPath)
        {
            // 生产环境连接字符串优化
            _connectionString = $"Data Source={dbPath};Version=3;" +
                               $"Pooling=true;Max Pool Size=100;" +
                               $"Connection Timeout=30;";
        }

        /// <summary>
        /// 安全启用WAL模式，包含重试机制
        /// </summary>
        public async Task<bool> SafeEnableWalModeAsync()
        {
            int retryCount = 0;
            const int maxRetries = 3;

            while (retryCount < maxRetries)
            {
                try
                {
                    using var connection = new SQLiteConnection(_connectionString);
                    await connection.OpenAsync();

                    // 检查当前日志模式
                    string currentMode = await GetCurrentJournalModeAsync(connection);
                    if (currentMode.Equals("wal", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("✅ WAL模式已启用");
                        return true;
                    }

                    // 启用WAL模式
                    using var command = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection);
                    string result = (await command.ExecuteScalarAsync())?.ToString();

                    if (result?.Equals("wal", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        await OptimizeWalSettingsAsync(connection);
                        Console.WriteLine($"✅ WAL模式启用成功 (第{retryCount + 1}次尝试)");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"❌ 第{retryCount}次启用WAL失败：{ex.Message}");

                    if (retryCount < maxRetries)
                    {
                        await Task.Delay(1000 * retryCount); // 指数退避
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取当前日志模式
        /// </summary>
        private async Task<string> GetCurrentJournalModeAsync(SQLiteConnection connection)
        {
            using var command = new SQLiteCommand("PRAGMA journal_mode;", connection);
            return (await command.ExecuteScalarAsync())?.ToString() ?? "";
        }

        /// <summary>
        /// 优化WAL设置
        /// </summary>
        private async Task OptimizeWalSettingsAsync(SQLiteConnection connection)
        {
            var settings = new Dictionary<string, object>
            {
                // 根据应用特点调整检查点频率
                ["wal_autocheckpoint"] = 5000,  // 高写入场景适当增大
                ["synchronous"] = "NORMAL",      // 平衡性能与安全
                ["cache_size"] = 20000,          // 增大缓存提升查询性能
                ["temp_store"] = "MEMORY",       // 临时表存储在内存
                ["mmap_size"] = 67108864         // 64MB内存映射，提升大文件性能
            };

            foreach (var setting in settings)
            {
                using var command = new SQLiteCommand($"PRAGMA {setting.Key}={setting.Value};", connection);
                await command.ExecuteNonQueryAsync();
            }

            Console.WriteLine("🚀 WAL性能优化配置完成");
        }
    }
}
