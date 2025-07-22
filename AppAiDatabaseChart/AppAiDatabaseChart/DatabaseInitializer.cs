using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAiDatabaseChart
{
    public static class DatabaseInitializer
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// 初始化SCADA数据库
        /// </summary>
        public static void InitializeScadaDatabase()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string dbFilePath = "test.db";

            // 如果数据库已存在，先删除
            if (File.Exists(dbFilePath))
            {
                File.Delete(dbFilePath);
                Console.WriteLine("已删除现有数据库文件");
            }

            // 创建数据库连接
            using var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;");
            connection.Open();

            Console.WriteLine("正在创建SCADA数据库表结构...");
            CreateTables(connection);

            Console.WriteLine("正在插入1000条SCADA数据记录...");
            InsertScadaData(connection, 1000);

            Console.WriteLine($"✅ SCADA数据库初始化完成！共插入1000条记录");
            //Console.WriteLine($"📍 数据库文件位置: {Path.GetFullPath(dbFilePath)}");
        }

        /// <summary>
        /// 创建SCADA相关数据表
        /// </summary>
        private static void CreateTables(SQLiteConnection connection)
        {
            // 创建设备表
            string createDevicesTable = @"
                CREATE TABLE Devices (
                    DeviceId INTEGER PRIMARY KEY AUTOINCREMENT,
                    DeviceName TEXT NOT NULL,
                    DeviceType TEXT NOT NULL,
                    Location TEXT,
                    Status TEXT,
                    InstallDate TEXT,
                    LastMaintenance TEXT
                );";

            // 创建测点表
            string createTagsTable = @"
                CREATE TABLE Tags (
                    TagId INTEGER PRIMARY KEY AUTOINCREMENT,
                    TagName TEXT NOT NULL UNIQUE,
                    DeviceId INTEGER,
                    TagType TEXT,
                    Unit TEXT,
                    MinValue REAL,
                    MaxValue REAL,
                    AlarmHigh REAL,
                    AlarmLow REAL,
                    Description TEXT,
                    FOREIGN KEY (DeviceId) REFERENCES Devices(DeviceId)
                );";

            // 创建实时数据表
            string createRealTimeDataTable = @"
                CREATE TABLE RealTimeData (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TagId INTEGER,
                    TagName TEXT,
                    Value REAL,
                    Quality INTEGER,
                    Timestamp TEXT,
                    AlarmStatus TEXT,
                    FOREIGN KEY (TagId) REFERENCES Tags(TagId)
                );";

            // 创建历史数据表
            string createHistoricalDataTable = @"
                CREATE TABLE HistoricalData (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TagId INTEGER,
                    TagName TEXT,
                    Value REAL,
                    Quality INTEGER,
                    Timestamp TEXT,
                    FOREIGN KEY (TagId) REFERENCES Tags(TagId)
                );";

            // 创建报警表
            string createAlarmsTable = @"
                CREATE TABLE Alarms (
                    AlarmId INTEGER PRIMARY KEY AUTOINCREMENT,
                    TagId INTEGER,
                    TagName TEXT,
                    AlarmType TEXT,
                    AlarmLevel TEXT,
                    AlarmMessage TEXT,
                    AlarmTime TEXT,
                    AcknowledgeTime TEXT,
                    Status TEXT,
                    FOREIGN KEY (TagId) REFERENCES Tags(TagId)
                );";

            ExecuteNonQuery(connection, createDevicesTable);
            ExecuteNonQuery(connection, createTagsTable);
            ExecuteNonQuery(connection, createRealTimeDataTable);
            ExecuteNonQuery(connection, createHistoricalDataTable);
            ExecuteNonQuery(connection, createAlarmsTable);
        }

        /// <summary>
        /// 插入SCADA测试数据
        /// </summary>
        private static void InsertScadaData(SQLiteConnection connection, int recordCount)
        {
            // 插入设备数据
            InsertDevices(connection);

            // 插入测点数据
            InsertTags(connection);

            // 插入实时数据
            InsertRealTimeData(connection, recordCount / 4);

            // 插入历史数据
            InsertHistoricalData(connection, recordCount / 2);

            // 插入报警数据
            InsertAlarms(connection, recordCount / 4);
        }

        /// <summary>
        /// 插入设备数据
        /// </summary>
        private static void InsertDevices(SQLiteConnection connection)
        {
            string[] deviceTypes = { "PLC", "HMI", "变频器", "传感器", "阀门", "泵", "电机", "变压器" };
            string[] locations = { "车间A", "车间B", "车间C", "锅炉房", "配电室", "水泵房", "冷却塔", "办公楼" };
            string[] statuses = { "运行", "停机", "维护", "故障" };

            for (int i = 1; i <= 50; i++)
            {
                string sql = @"
                    INSERT INTO Devices (DeviceName, DeviceType, Location, Status, InstallDate, LastMaintenance)
                    VALUES (@DeviceName, @DeviceType, @Location, @Status, @InstallDate, @LastMaintenance)";

                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@DeviceName", $"设备_{i:D3}");
                command.Parameters.AddWithValue("@DeviceType", deviceTypes[random.Next(deviceTypes.Length)]);
                command.Parameters.AddWithValue("@Location", locations[random.Next(locations.Length)]);
                command.Parameters.AddWithValue("@Status", statuses[random.Next(statuses.Length)]);
                command.Parameters.AddWithValue("@InstallDate", DateTime.Now.AddDays(-random.Next(365, 1825)).ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@LastMaintenance", DateTime.Now.AddDays(-random.Next(1, 90)).ToString("yyyy-MM-dd"));

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 插入测点数据
        /// </summary>
        private static void InsertTags(SQLiteConnection connection)
        {
            string[] tagTypes = { "AI", "AO", "DI", "DO" }; // 模拟输入、模拟输出、数字输入、数字输出
            string[] units = { "℃", "MPa", "m³/h", "kW", "V", "A", "%", "rpm" };

            string[] tagPrefixes = { "温度", "压力", "流量", "功率", "电压", "电流", "湿度", "转速", "液位", "浓度" };

            for (int i = 1; i <= 100; i++)
            {
                string sql = @"
                    INSERT INTO Tags (TagName, DeviceId, TagType, Unit, MinValue, MaxValue, AlarmHigh, AlarmLow, Description)
                    VALUES (@TagName, @DeviceId, @TagType, @Unit, @MinValue, @MaxValue, @AlarmHigh, @AlarmLow, @Description)";

                using var command = new SQLiteCommand(sql, connection);

                string prefix = tagPrefixes[random.Next(tagPrefixes.Length)];
                string unit = units[random.Next(units.Length)];
                double minValue = random.NextDouble() * 50;
                double maxValue = minValue + random.NextDouble() * 200;

                command.Parameters.AddWithValue("@TagName", $"{prefix}_{i:D3}");
                command.Parameters.AddWithValue("@DeviceId", random.Next(1, 51));
                command.Parameters.AddWithValue("@TagType", tagTypes[random.Next(tagTypes.Length)]);
                command.Parameters.AddWithValue("@Unit", unit);
                command.Parameters.AddWithValue("@MinValue", Math.Round(minValue, 2));
                command.Parameters.AddWithValue("@MaxValue", Math.Round(maxValue, 2));
                command.Parameters.AddWithValue("@AlarmHigh", Math.Round(maxValue * 0.9, 2));
                command.Parameters.AddWithValue("@AlarmLow", Math.Round(minValue * 1.1, 2));
                command.Parameters.AddWithValue("@Description", $"{prefix}监测点_{i}");

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 插入实时数据
        /// </summary>
        private static void InsertRealTimeData(SQLiteConnection connection, int count)
        {
            string[] alarmStatuses = { "正常", "预警", "报警", "严重" };

            for (int i = 1; i <= count; i++)
            {
                string sql = @"
                    INSERT INTO RealTimeData (TagId, TagName, Value, Quality, Timestamp, AlarmStatus)
                    VALUES (@TagId, @TagName, @Value, @Quality, @Timestamp, @AlarmStatus)";

                using var command = new SQLiteCommand(sql, connection);

                int tagId = random.Next(1, 101);
                command.Parameters.AddWithValue("@TagId", tagId);
                command.Parameters.AddWithValue("@TagName", GetTagNameById(connection, tagId));
                command.Parameters.AddWithValue("@Value", Math.Round(random.NextDouble() * 100, 2));
                command.Parameters.AddWithValue("@Quality", random.Next(90, 101)); // 90-100的质量值
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now.AddSeconds(-random.Next(0, 3600)).ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@AlarmStatus", alarmStatuses[random.Next(alarmStatuses.Length)]);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 插入历史数据
        /// </summary>
        private static void InsertHistoricalData(SQLiteConnection connection, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                string sql = @"
                    INSERT INTO HistoricalData (TagId, TagName, Value, Quality, Timestamp)
                    VALUES (@TagId, @TagName, @Value, @Quality, @Timestamp)";

                using var command = new SQLiteCommand(sql, connection);

                int tagId = random.Next(1, 101);
                command.Parameters.AddWithValue("@TagId", tagId);
                command.Parameters.AddWithValue("@TagName", GetTagNameById(connection, tagId));
                command.Parameters.AddWithValue("@Value", Math.Round(random.NextDouble() * 100, 2));
                command.Parameters.AddWithValue("@Quality", random.Next(85, 101));
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now.AddHours(-random.Next(1, 168)).ToString("yyyy-MM-dd HH:mm:ss")); // 过去一周内的时间

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 插入报警数据
        /// </summary>
        private static void InsertAlarms(SQLiteConnection connection, int count)
        {
            string[] alarmTypes = { "高报警", "低报警", "设备故障", "通讯异常", "质量异常" };
            string[] alarmLevels = { "轻微", "一般", "严重", "紧急" };
            string[] statuses = { "活动", "已确认", "已处理", "已关闭" };

            for (int i = 1; i <= count; i++)
            {
                string sql = @"
                    INSERT INTO Alarms (TagId, TagName, AlarmType, AlarmLevel, AlarmMessage, AlarmTime, AcknowledgeTime, Status)
                    VALUES (@TagId, @TagName, @AlarmType, @AlarmLevel, @AlarmMessage, @AlarmTime, @AcknowledgeTime, @Status)";

                using var command = new SQLiteCommand(sql, connection);

                int tagId = random.Next(1, 101);
                string tagName = GetTagNameById(connection, tagId);
                string alarmType = alarmTypes[random.Next(alarmTypes.Length)];
                DateTime alarmTime = DateTime.Now.AddHours(-random.Next(1, 72));
                DateTime? ackTime = random.Next(0, 2) == 0 ? alarmTime.AddMinutes(random.Next(5, 120)) : null;

                command.Parameters.AddWithValue("@TagId", tagId);
                command.Parameters.AddWithValue("@TagName", tagName);
                command.Parameters.AddWithValue("@AlarmType", alarmType);
                command.Parameters.AddWithValue("@AlarmLevel", alarmLevels[random.Next(alarmLevels.Length)]);
                command.Parameters.AddWithValue("@AlarmMessage", $"{tagName} {alarmType}");
                command.Parameters.AddWithValue("@AlarmTime", alarmTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@AcknowledgeTime", ackTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "");
                command.Parameters.AddWithValue("@Status", statuses[random.Next(statuses.Length)]);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 根据TagId获取TagName
        /// </summary>
        private static string GetTagNameById(SQLiteConnection connection, int tagId)
        {
            string sql = "SELECT TagName FROM Tags WHERE TagId = @TagId";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@TagId", tagId);

            var result = command.ExecuteScalar();
            return result?.ToString() ?? $"Tag_{tagId}";
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 显示数据库统计信息
        /// </summary>
        public static void ShowDatabaseStatistics()
        {
            string dbFilePath = "test.db";
            if (!File.Exists(dbFilePath))
            {
                Console.WriteLine("❌ 数据库文件不存在，请先初始化数据库");
                return;
            }

            using var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;");
            connection.Open();

            Console.WriteLine("📊 SCADA数据库统计信息:");
            Console.WriteLine("========================");

            ShowTableCount(connection, "Devices", "设备");
            ShowTableCount(connection, "Tags", "测点");
            ShowTableCount(connection, "RealTimeData", "实时数据");
            ShowTableCount(connection, "HistoricalData", "历史数据");
            ShowTableCount(connection, "Alarms", "报警");
        }

        /// <summary>
        /// 显示表记录数量
        /// </summary>
        private static void ShowTableCount(SQLiteConnection connection, string tableName, string displayName)
        {
            string sql = $"SELECT COUNT(*) FROM {tableName}";
            using var command = new SQLiteCommand(sql, connection);
            var count = command.ExecuteScalar();
            Console.WriteLine($"📋 {displayName}: {count} 条记录");
        }
    }
}
