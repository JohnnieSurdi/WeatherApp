using log4net;

namespace WeatherApp.Logging
{
    public class Logger : IWeatherLogger
    {
        private static readonly ILog log = LogManager.GetLogger("WeatherLogger");
        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }
    }
}
