using Serilog;

namespace LoggerService
{
    /// <summary>
    /// Implementation của ILoggerManager sử dụng Serilog
    /// </summary>
    public class LoggerManager : ILoggerManager
    {
        private readonly Serilog.ILogger _logger;

        public LoggerManager()
        {
            _logger = Log.Logger;
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogInfo(string message)
        {
            _logger.Information(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warning(message);
        }
    }
}
