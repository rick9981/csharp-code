using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLib.SourceGenerators.Models
{
    public class ConfigurationPropertyInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ConfigKey { get; set; }
        public object? DefaultValue { get; set; }
        public bool IsRequired { get; set; }
    }
}
