using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLib.Attributes;

namespace AppTest.Configurations
{
    [Configuration(FileName = "api", Required = true)]
    public class ApiConfig
    {
        [ConfigProperty(Key = "baseUrl", Required = true)]
        public string BaseUrl { get; set; } = string.Empty;

        [ConfigProperty(Key = "apiKey")]
        public string? ApiKey { get; set; }

        [ConfigProperty(Key = "timeout", DefaultValue = 5000)]
        public int TimeoutMs { get; set; }

        [ConfigProperty(Key = "endpoints")]
        public string[]? Endpoints { get; set; }
    }
}
