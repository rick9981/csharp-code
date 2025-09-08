using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLib.Attributes;

namespace AppTest.Configurations
{
    public class FeatureFlags
    {
        public bool EnableNewFeature { get; set; }
        public bool EnableBetaFeatures { get; set; }
        public DateTime? FeatureExpiryDate { get; set; }
    }

    [Configuration(FileName = "features", Required = false)]
    public class FeatureConfig
    {
        [ConfigProperty(Key = "flags")]
        public FeatureFlags? Flags { get; set; }

        [ConfigProperty(Key = "enableExperimentalFeatures", DefaultValue = false)]
        public bool EnableExperimentalFeatures { get; set; }

        [ConfigProperty(Key = "experimentalFeaturesList")]
        public string[]? ExperimentalFeaturesList { get; set; }
    }
}
