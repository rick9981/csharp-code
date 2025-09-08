using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLib.Attributes;

namespace AppTest.Configurations
{
    [Configuration(FileName = "database", Section = "Database", Required = true)]
    public class DatabaseConfig
    {
        [ConfigProperty(Key = "connectionString", Required = true)]
        public string ConnectionString { get; set; } = string.Empty;

        [ConfigProperty(Key = "timeout", DefaultValue = 30)]
        public int CommandTimeout { get; set; }

        [ConfigProperty(Key = "enableRetry", DefaultValue = true)]
        public bool EnableRetry { get; set; }

        [ConfigProperty(Key = "maxRetryCount", DefaultValue = 3)]
        public int MaxRetryCount { get; set; }
    }
}
