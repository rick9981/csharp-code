using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSqliteWAL
{
    /// <summary>
    /// WAL模式性能测试
    /// </summary>
    public class WalPerformanceTest
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        public WalPerformanceTest(string dbPath)
        {
            _dbPath = dbPath;
            _connectionString = $"Data Source={dbPath};Version=3;Pooling=true;Max Pool Size=100;";
        }

        /// <summary>
        /// 对比DELETE模式和WAL模式的并发性能
        /// </summary>
        public async Task CompareModePerformanceAsync()
        {
            // 准备测试数据表
            await InitializeTestTableAsync();

            Console.WriteLine("🧪 开始性能对比测试...\n");

            // 测试DELETE模式
            await SetJournalModeAsync("DELETE");
            var deleteResults = await RunConcurrentTestAsync("DELETE模式", 10, 1000);

            // 测试WAL模式  
            await SetJournalModeAsync("WAL");
            var walResults = await RunConcurrentTestAsync("WAL模式", 10, 1000);

            // 输出对比结果
            PrintComparisonResults(deleteResults, walResults);
        }

        /// <summary>
        /// 运行并发读写测试
        /// </summary>
        private async Task<TestResult> RunConcurrentTestAsync(string modeName, int threadCount, int operationsPerThread)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task>();
            var successCount = 0;
            var errorCount = 0;
            var lockObject = new object();

            // 创建并发任务
            for (int i = 0; i < threadCount; i++)
            {
                int threadId = i;
                tasks.Add(Task.Run(async () =>
                {
                    for (int j = 0; j < operationsPerThread; j++)
                    {
                        try
                        {
                            // 随机执行读或写操作
                            bool isWrite = Random.Shared.Next(0, 2) == 0;

                            if (isWrite)
                                await InsertTestDataAsync(threadId, j);
                            else
                                await ReadTestDataAsync();

                            lock (lockObject) { successCount++; }
                        }
                        catch (Exception ex)
                        {
                            lock (lockObject) { errorCount++; }
                            Console.WriteLine($"线程{threadId}操作{j}失败：{ex.Message}");
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();

            return new TestResult
            {
                ModeName = modeName,
                Duration = stopwatch.Elapsed,
                SuccessCount = successCount,
                ErrorCount = errorCount,
                TotalOperations = threadCount * operationsPerThread
            };
        }

        /// <summary>
        /// 插入测试数据
        /// </summary>
        private async Task InsertTestDataAsync(int threadId, int operationId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SQLiteCommand(
                "INSERT INTO test_table (thread_id, operation_id, data, created_time) VALUES (@thread, @op, @data, @time)",
                connection);

            command.Parameters.AddWithValue("@thread", threadId);
            command.Parameters.AddWithValue("@op", operationId);
            command.Parameters.AddWithValue("@data", $"测试数据_{threadId}_{operationId}_{Guid.NewGuid()}");
            command.Parameters.AddWithValue("@time", DateTime.Now);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 读取测试数据
        /// </summary>
        private async Task ReadTestDataAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SQLiteCommand(
                "SELECT COUNT(*) FROM test_table WHERE created_time > datetime('now', '-1 minute')",
                connection);

            await command.ExecuteScalarAsync();
        }

        // 其他辅助方法...
        private async Task InitializeTestTableAsync()
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SQLiteCommand(@"
            CREATE TABLE IF NOT EXISTS test_table (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                thread_id INTEGER,
                operation_id INTEGER,
                data TEXT,
                created_time DATETIME
            );
            DELETE FROM test_table;", connection);

            await command.ExecuteNonQueryAsync();
        }

        private async Task SetJournalModeAsync(string mode)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SQLiteCommand($"PRAGMA journal_mode={mode};", connection);
            await command.ExecuteNonQueryAsync();

            // 等待模式切换完成
            await Task.Delay(1000);
        }

        private void PrintComparisonResults(TestResult deleteResult, TestResult walResult)
        {
            Console.WriteLine("\n📊 性能对比结果");
            Console.WriteLine("================================");
            Console.WriteLine($"模式          | 总时间    | 成功数  | 失败数  | 平均耗时");
            Console.WriteLine($"{deleteResult.ModeName,-12} | {deleteResult.Duration.TotalSeconds:F2}s    | {deleteResult.SuccessCount,-6} | {deleteResult.ErrorCount,-6} | {deleteResult.Duration.TotalMilliseconds / deleteResult.TotalOperations:F2}ms");
            Console.WriteLine($"{walResult.ModeName,-12} | {walResult.Duration.TotalSeconds:F2}s    | {walResult.SuccessCount,-6} | {walResult.ErrorCount,-6} | {walResult.Duration.TotalMilliseconds / walResult.TotalOperations:F2}ms");

            double improvement = (deleteResult.Duration.TotalMilliseconds - walResult.Duration.TotalMilliseconds)
                               / deleteResult.Duration.TotalMilliseconds * 100;

            Console.WriteLine($"\n🚀 WAL模式性能提升：{improvement:F1}%");
            Console.WriteLine($"🛡️ WAL模式错误率降低：{((double)(deleteResult.ErrorCount - walResult.ErrorCount) / deleteResult.TotalOperations * 100):F1}%");
        }
    }

    // 测试结果数据结构
    public class TestResult
    {
        public string ModeName { get; set; } = "";
        public TimeSpan Duration { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public int TotalOperations { get; set; }
    }
}
