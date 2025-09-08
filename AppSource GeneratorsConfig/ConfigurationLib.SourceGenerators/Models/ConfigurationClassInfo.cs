using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLib.SourceGenerators.Models
{
    public class ConfigurationClassInfo
    {
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string? FileName { get; set; }
        public string? Section { get; set; }
        public bool Required { get; set; }
        public List<ConfigurationPropertyInfo> Properties { get; set; } = new();
    }
}
