using System;
using System.Configuration;
using System.ServiceProcess;
using SauceConnect.Service.Configuration;
using SauceConnect.Service.Logging;

namespace SauceConnect.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                var logger = new ConsoleLogger();
                logger.Log("Starting the service.");

                var config = (SauceConnectSectionConfiguration)ConfigurationManager.GetSection("sauceLabsConfiguration");
                var sauceLabsRestClient = new SauceLabsRestClient(config.Username, config.AccessKey);

                var sauceConnectWrapper = new SauceConnectWrapper(logger, sauceLabsRestClient, config);
                sauceConnectWrapper.Start();

                logger.Log("Press any key to quit.");
                Console.ReadLine();
            }
            else
            {
                RunService();
            }
        }

        private static void RunService()
        {
            var servicesToRun = new ServiceBase[]
                {
                    new SauceConnectService()
                };
            ServiceBase.Run(servicesToRun);
        }
    }
}
