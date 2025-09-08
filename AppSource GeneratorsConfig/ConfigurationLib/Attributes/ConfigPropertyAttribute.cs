using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string? Key { get; set; }
        public object? DefaultValue { get; set; }
        public bool Required { get; set; } = false;
    }
}
