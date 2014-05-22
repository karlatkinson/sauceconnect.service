using System.Configuration;
using System.ServiceProcess;
using SauceConnect.Service.Configuration;
using SauceConnect.Service.Logging;

namespace SauceConnect.Service
{
    public partial class SauceConnectService : ServiceBase
    {
        private SauceConnectWrapper _sauceConnectWrapper;

        public SauceConnectService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Starting the service.");

            var config = (SauceConnectSectionConfiguration)ConfigurationManager.GetSection("sauceLabsConfiguration");
            var sauceLabsRestClient = new SauceLabsRestClient(config.Username, config.AccessKey);
            var logger = new EventLogLogger(EventLog);

            _sauceConnectWrapper = new SauceConnectWrapper(logger, sauceLabsRestClient, config);
            _sauceConnectWrapper.Start();
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Stopping the service.");
            _sauceConnectWrapper.Stop();
        }
    }
}
