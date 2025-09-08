using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLib.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public string? FileName { get; set; }
        public string? Section { get; set; }
        public bool Required { get; set; } = true;
    }
}
