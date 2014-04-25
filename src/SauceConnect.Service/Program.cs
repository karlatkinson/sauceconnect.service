using System.ServiceProcess;

namespace SauceConnect.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SauceConnectService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
