using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace SauceConnect.Service
{
    public partial class SauceConnectService : ServiceBase
    {
        private SauceConnectWrapper _sauceConnectWrapper;
        private Timer _timer;

        public SauceConnectService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Starting the service.");

            var userName = ConfigurationManager.AppSettings["sauceConnectUserName"];
            var accessKey = ConfigurationManager.AppSettings["sauceConnectAccessKey"];
            var tunnelIdentifier = ConfigurationManager.AppSettings["sauceConnectIdentifier"];

            int pollInterval;
            if (!int.TryParse(ConfigurationManager.AppSettings["sauceConnectPollInterval"], out pollInterval))
            {
                throw new ArgumentException("sauceConnectPollInterval is not valid");
            }

            _sauceConnectWrapper = new SauceConnectWrapper(this, new SauceLabsRestClient(userName, accessKey));
            _sauceConnectWrapper.Start(userName, accessKey, tunnelIdentifier);

            _timer = new Timer();
            _timer.Elapsed += (sender, eventArgs) => CheckTunnel();
            _timer.Start();
        }

        private void CheckTunnel()
        {
            if (_sauceConnectWrapper.TunnelIsLive()) return;

            EventLog.WriteEntry("Tunnel is not live, attempting to restart");
            OnStart(new string[] {});
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Stopping the service.");
            _sauceConnectWrapper.Stop();
        }
    }
}
