using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLib.Attributes;

namespace AppTest.Configurations
{
    [Configuration(FileName = "email", Section = "Email", Required = true)]
    public class EmailConfig
    {
        [ConfigProperty(Key = "host", Required = true)]
        public string Host { get; set; }

        [ConfigProperty(Key = "port", Required = true)]
        public int Port { get; set; }

        [ConfigProperty(Key = "enableSsl", Required = true)]
        public bool EnableSsl { get; set; }
        public string DefaultFromEmail { get; set; }
        public string EefaultToEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DefaultFromName { get; set; }

    }
}
