using System.Diagnostics;

namespace SauceConnect.Service.Logging
{
    public class EventLogLogger : ILogger
    {
        private readonly EventLog _eventLog;

        public EventLogLogger(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void Log(string message)
        {
            _eventLog.WriteEntry(message);
        }
    }
}
