using System;
using System.Diagnostics;
using System.IO;

namespace SauceConnect.Service
{
    public class SauceConnectWrapper
    {
        private readonly SauceConnectService _sauceConnectService;
        private Process _process;

        public SauceConnectWrapper(SauceConnectService sauceConnectService)
        {
            _sauceConnectService = sauceConnectService;
        }

        public void Start(string username, string accessKey, string identifier)
        {
            var args = string.Format("-u {0} -k {1}", username, accessKey);
            if(!string.IsNullOrWhiteSpace(identifier))
                args += "-i " + identifier;

            _process = new Process
                {
                    StartInfo =
                        {
                            FileName = AppDomain.CurrentDomain.BaseDirectory + "sauce_connect.exe",
                            UseShellExecute = false,
                            Arguments = args,
                            RedirectStandardOutput = true,
                            RedirectStandardInput = true
                        }
                };

            _process.Exited += ProcessOnExited;
            _process.OutputDataReceived += ProcessOnOutputDataReceived;

            try
            {
                _sauceConnectService.EventLog.WriteEntry("Starting the sauce connect app at " + _process.StartInfo.FileName);
                _process.Start();
                _process.BeginOutputReadLine();
            }
            catch (Exception e)
            {
                _sauceConnectService.EventLog.WriteEntry(e.ToString());
                throw;
            }
            
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            var logFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\sauce_connect_output_log.txt";
            using (var stream = new StreamWriter(logFilePath, true))
            {
                stream.AutoFlush = true;
                stream.WriteLine(dataReceivedEventArgs.Data);
            }
        }

        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            _sauceConnectService.EventLog.WriteEntry("Process has stopped, stopping the service");
            if(_sauceConnectService.CanStop)
                _sauceConnectService.Stop();
        }

        public void Stop()
        {
            if (null != _process && !_process.HasExited)
            {
                _sauceConnectService.EventLog.WriteEntry("Killing the sauce connect app");
                _process.Kill();
            }
                
        }
    }
}
