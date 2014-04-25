using System.Configuration;
using System.ServiceProcess;

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
            _sauceConnectWrapper = new SauceConnectWrapper(this);
            _sauceConnectWrapper.Start(ConfigurationManager.AppSettings["sauceConnectUserName"], ConfigurationManager.AppSettings["sauceConnectAccessKey"], ConfigurationManager.AppSettings["sauceConnectIdentifier"]);
        }

        protected override void OnStop()
        {
            _sauceConnectWrapper.Stop();
        }
    }
}
