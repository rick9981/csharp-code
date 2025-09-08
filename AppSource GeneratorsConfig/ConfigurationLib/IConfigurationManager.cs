using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLib
{
    public interface IConfigurationManager
    {
        void LoadConfigurations(string configDirectory = "config");
        T GetConfiguration<T>() where T : class, new();
        IEnumerable<object> GetAllConfigurations();
    }
}
