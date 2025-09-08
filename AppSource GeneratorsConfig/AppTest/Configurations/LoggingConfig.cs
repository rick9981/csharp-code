using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLib.Attributes;

namespace AppTest.Configurations
{
    [Configuration(Section = "Logging", Required = false)]
    public class LoggingConfig
    {
        [ConfigProperty(Key = "level", DefaultValue = "Information")]
        public string LogLevel { get; set; } = "Information";

        [ConfigProperty(Key = "enableFileLogging", DefaultValue = false)]
        public bool EnableFileLogging { get; set; }

        [ConfigProperty(Key = "logPath", DefaultValue = "logs/app.log")]
        public string LogPath { get; set; } = "logs/app.log";

        [ConfigProperty(Key = "maxFileSizeMB", DefaultValue = 100)]
        public int MaxFileSizeMB { get; set; }
    }
}
