using AppTest.Configurations;

namespace AppTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Configuration Management Demo ===");
            Console.WriteLine();

            try
            {
                // 创建配置管理器实例
                var configManager = new ConfigurationLib.Generated.GeneratedConfigurationManager();

                // 加载所有配置文件
                configManager.LoadConfigurations("config");

                Console.WriteLine("✅ Configurations loaded successfully!");
                Console.WriteLine();

                // 使用数据库配置
                DemonstrateUsage(configManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading configurations: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void DemonstrateUsage(ConfigurationLib.Generated.GeneratedConfigurationManager configManager)
        {
            // 获取数据库配置
            var dbConfig = configManager.GetConfiguration<DatabaseConfig>();
            if (dbConfig != null)
            {
                Console.WriteLine("📊 Database Configuration:");
                Console.WriteLine($"  Connection String: {MaskConnectionString(dbConfig.ConnectionString)}");
                Console.WriteLine($"  Command Timeout: {dbConfig.CommandTimeout}s");
                Console.WriteLine($"  Enable Retry: {dbConfig.EnableRetry}");
                Console.WriteLine($"  Max Retry Count: {dbConfig.MaxRetryCount}");
                Console.WriteLine();

                // 模拟数据库连接
                ConnectToDatabase(dbConfig);
            }

            // 获取API配置
            var apiConfig = configManager.GetConfiguration<ApiConfig>();
            if (apiConfig != null)
            {
                Console.WriteLine("🌐 API Configuration:");
                Console.WriteLine($"  Base URL: {apiConfig.BaseUrl}");
                Console.WriteLine($"  API Key: {MaskApiKey(apiConfig.ApiKey)}");
                Console.WriteLine($"  Timeout: {apiConfig.TimeoutMs}ms");
                Console.WriteLine($"  Endpoints: [{string.Join(", ", apiConfig.Endpoints ?? new string[0])}]");
                Console.WriteLine();

                // 模拟API调用
                CallApi(apiConfig);
            }

            // 获取邮箱
            var emailConfig = configManager.GetConfiguration<EmailConfig>();
            if (emailConfig != null)
            {
                Console.WriteLine("📊 Email Configuration:");
                Console.WriteLine($"  Host String: {emailConfig.Host}");
                Console.WriteLine($"  Port : {emailConfig.Port}s");
                Console.WriteLine($"  EnableSsl : {emailConfig.EnableSsl}s");
                Console.WriteLine();
            }

            // 获取日志配置
            var loggingConfig = configManager.GetConfiguration<LoggingConfig>();
            if (loggingConfig != null)
            {
                Console.WriteLine("📝 Logging Configuration:");
                Console.WriteLine($"  Log Level: {loggingConfig.LogLevel}");
                Console.WriteLine($"  File Logging: {loggingConfig.EnableFileLogging}");
                Console.WriteLine($"  Log Path: {loggingConfig.LogPath}");
                Console.WriteLine($"  Max File Size: {loggingConfig.MaxFileSizeMB}MB");
                Console.WriteLine();

                // 配置日志系统
                ConfigureLogging(loggingConfig);
            }

            // 获取功能配置
            var featureConfig = configManager.GetConfiguration<FeatureConfig>();
            if (featureConfig != null)
            {
                Console.WriteLine("🚩 Feature Configuration:");
                Console.WriteLine($"  Experimental Features: {featureConfig.EnableExperimentalFeatures}");

                if (featureConfig.Flags != null)
                {
                    Console.WriteLine($"  New Feature: {featureConfig.Flags.EnableNewFeature}");
                    Console.WriteLine($"  Beta Features: {featureConfig.Flags.EnableBetaFeatures}");
                    Console.WriteLine($"  Feature Expiry: {featureConfig.Flags.FeatureExpiryDate?.ToString("yyyy-MM-dd") ?? "Not set"}");
                }

                if (featureConfig.ExperimentalFeaturesList != null)
                {
                    Console.WriteLine($"  Experimental List: [{string.Join(", ", featureConfig.ExperimentalFeaturesList)}]");
                }
                Console.WriteLine();

                // 处理功能标志
                HandleFeatureFlags(featureConfig);
            }



            // 显示所有加载的配置
            Console.WriteLine("📋 All Loaded Configurations:");
            var allConfigs = configManager.GetAllConfigurations();
            foreach (var config in allConfigs)
            {
                Console.WriteLine($"  - {config.GetType().Name}");
            }
        }

        private static void ConnectToDatabase(DatabaseConfig config)
        {
            Console.WriteLine("🔌 Simulating database connection...");

            // 这里是模拟代码，实际应用中会创建真实的数据库连接
            if (!string.IsNullOrEmpty(config.ConnectionString))
            {
                Console.WriteLine("  ✅ Database connected successfully!");
                Console.WriteLine($"  ⏱️  Using timeout: {config.CommandTimeout}s");

                if (config.EnableRetry)
                {
                    Console.WriteLine($"  🔄 Retry enabled with max count: {config.MaxRetryCount}");
                }
            }
            else
            {
                Console.WriteLine("  ❌ Invalid connection string!");
            }
            Console.WriteLine();
        }

        private static void CallApi(ApiConfig config)
        {
            Console.WriteLine("🌍 Simulating API calls...");

            if (!string.IsNullOrEmpty(config.BaseUrl))
            {
                Console.WriteLine($"  📡 Connecting to: {config.BaseUrl}");
                Console.WriteLine($"  ⏱️  Request timeout: {config.TimeoutMs}ms");

                if (config.Endpoints != null)
                {
                    foreach (var endpoint in config.Endpoints)
                    {
                        Console.WriteLine($"  📝 Available endpoint: {config.BaseUrl}{endpoint}");
                    }
                }

                Console.WriteLine("  ✅ API connection established!");
            }
            else
            {
                Console.WriteLine("  ❌ Invalid API base URL!");
            }
            Console.WriteLine();
        }

        private static void ConfigureLogging(LoggingConfig config)
        {
            Console.WriteLine("📋 Configuring logging system...");

            Console.WriteLine($"  📊 Log level set to: {config.LogLevel}");

            if (config.EnableFileLogging)
            {
                Console.WriteLine($"  📄 File logging enabled: {config.LogPath}");
                Console.WriteLine($"  💾 Max file size: {config.MaxFileSizeMB}MB");

                // 确保日志目录存在
                var logDir = Path.GetDirectoryName(config.LogPath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                    Console.WriteLine($"  📁 Created log directory: {logDir}");
                }
            }
            else
            {
                Console.WriteLine("  📺 Console logging only");
            }

            Console.WriteLine("  ✅ Logging configured successfully!");
            Console.WriteLine();
        }

        private static void HandleFeatureFlags(FeatureConfig config)
        {
            Console.WriteLine("🎛️  Processing feature flags...");

            if (config.Flags != null)
            {
                if (config.Flags.EnableNewFeature)
                {
                    Console.WriteLine("  🆕 New feature is ENABLED");
                }

                if (config.Flags.EnableBetaFeatures)
                {
                    Console.WriteLine("  🧪 Beta features are ENABLED");
                }
                else
                {
                    Console.WriteLine("  🚫 Beta features are DISABLED");
                }

                if (config.Flags.FeatureExpiryDate.HasValue &&
                    config.Flags.FeatureExpiryDate.Value < DateTime.Now)
                {
                    Console.WriteLine("  ⚠️  Some features have expired!");
                }
            }

            if (config.EnableExperimentalFeatures && config.ExperimentalFeaturesList != null)
            {
                Console.WriteLine("  🧬 Experimental features enabled:");
                foreach (var feature in config.ExperimentalFeaturesList)
                {
                    Console.WriteLine($"    - {feature}");
                }
            }

            Console.WriteLine("  ✅ Feature flags processed!");
            Console.WriteLine();
        }

        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "[Not Set]";

            return connectionString;
        }

        private static string MaskApiKey(string? apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "[Not Set]";

            return apiKey.Length > 8
                ? apiKey.Substring(0, 4) + "****" + apiKey.Substring(apiKey.Length - 4)
                : "****";
        }
    }
}
