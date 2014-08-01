using System.Configuration;

namespace SauceConnect.Service.Configuration
{
    public class SauceConnectSectionConfiguration : ConfigurationSection, ISauceConnectConfiguration
    {
        [ConfigurationProperty("Username", IsRequired = true)]
        public string Username {
            get
            {
                return (string)this["Username"];
            }
        }

        [ConfigurationProperty("AccessKey", IsRequired = true)]
        public string AccessKey {
            get
            {
                return (string)this["AccessKey"];
            }
        }

        [ConfigurationProperty("TunnelIdentifier", IsRequired = true)]
        public string TunnelIdentifier
        {
            get
            {
                return (string)this["TunnelIdentifier"];
            }
        }

        [ConfigurationProperty("TunnelPollInterval", IsRequired = true)]
        public double TunnelPollInterval
        {
            get
            {
                return (double)this["TunnelPollInterval"];
            }
        }

        [ConfigurationProperty("SauceConnectPath", IsRequired = true)]
        public string SauceConnectPath
        {
            get
            {
                return (string)this["SauceConnectPath"];
            }
        }
    }
}
