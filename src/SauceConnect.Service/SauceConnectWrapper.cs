using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using SauceConnect.Service.Configuration;
using SauceConnect.Service.Logging;

namespace SauceConnect.Service
{
    public class SauceConnectWrapper
    {
        private readonly ILogger _logger;
        private readonly ISauceLabsRestClient _sauceLabsRestClient;
        private readonly ISauceConnectConfiguration _sauceConnectConfiguration;
        private Process _process;
        private static string _activeTunnelId;
        private Timer _timer;

        public SauceConnectWrapper(ILogger logger, ISauceLabsRestClient sauceLabsRestClient, ISauceConnectConfiguration sauceConnectConfiguration)
        {
            _logger = logger;
            _sauceLabsRestClient = sauceLabsRestClient;
            _sauceConnectConfiguration = sauceConnectConfiguration;
        }

        public void Start()
        {
            var args = string.Format("--user {0} --api-key {1}", _sauceConnectConfiguration.Username, _sauceConnectConfiguration.AccessKey);
            if(!string.IsNullOrWhiteSpace(_sauceConnectConfiguration.TunnelIdentifier))
                args += " --tunnel-identifier " + _sauceConnectConfiguration.TunnelIdentifier;

            _process = new Process
                {
                    StartInfo =
                        {
                            FileName = AppDomain.CurrentDomain.BaseDirectory + _sauceConnectConfiguration.SauceConnectPath + @"\sc.exe",
                            UseShellExecute = false,
                            Arguments = args,
                            RedirectStandardOutput = true,
                            RedirectStandardInput = true
                        }
                };

            _process.OutputDataReceived += ProcessOnOutputDataReceived;

            try
            {
                _logger.Log("Starting the sauce connect app at " + _process.StartInfo.FileName);
                _logger.Log("Arguments are " + _process.StartInfo.Arguments);
                _process.Start();
                _process.BeginOutputReadLine();

                _timer = new Timer(_sauceConnectConfiguration.TunnelPollInterval);
                _timer.Elapsed += (sender, eventArgs) => CheckTunnel();

                _logger.Log("Checking if tunnel is active every " + TimeSpan.FromMilliseconds(_sauceConnectConfiguration.TunnelPollInterval).Minutes + " minutes.");
                _timer.Start();
            }
            catch (Exception e)
            {
                _logger.Log(e.ToString());
                throw;
            }
            
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null && dataReceivedEventArgs.Data.Contains("Tunnel ID: "))
            {
                DetermineTunnelId(dataReceivedEventArgs);
            }

            _logger.Log(dataReceivedEventArgs.Data);
        }

        private void DetermineTunnelId(DataReceivedEventArgs dataReceivedEventArgs)
        {
            var stringArray = dataReceivedEventArgs.Data.Split(':');
            if (stringArray.Length != 4)
                throw new ArgumentOutOfRangeException("Array length expected to be 4 but was " + stringArray.Length);
            _activeTunnelId = stringArray[stringArray.Length - 1].Trim();
            _logger.Log("Tunnel ID Detected as " + _activeTunnelId);
        }

        public void Stop()
        {
            if (_process == null || _process.HasExited) return;

            _logger.Log("Killing the sauce connect app");
            _process.Kill();
        }

        private void CheckTunnel()
        {
            if (TunnelIsLive()) return;

            _logger.Log("Tunnel is not live, attempting to restart");
            _timer.Stop();
            _timer.Dispose();
            Start();
        }

        public bool TunnelIsLive()
        {
            IList<string> activeTunnels = null;

            try
            {
                activeTunnels = _sauceLabsRestClient.GetActiveTunnels();
            }
            catch (Exception e)
            {
                _logger.Log("Could not determine if tunnel active: " + e.Message);
            }

            return activeTunnels != null && activeTunnels.Contains(_activeTunnelId);
        }
    }
}
