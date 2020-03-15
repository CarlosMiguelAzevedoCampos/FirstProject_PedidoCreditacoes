using CMA.ISMAI.Logging.Interface;
using Microsoft.Extensions.Logging;

namespace CMA.ISMAI.Logging.Service
{
    public class LoggingService : ILog
    {
        private readonly ILogger<LoggingService> _logger;

        // constructor
        public LoggingService(ILogger<LoggingService> logger = null)
        {
            _logger = logger;
        }
        public void Actions(string message)
        {
            _logger.LogInformation($"An action has been made! - {message}");
        }

        public void Fatal(string message)
        {
            _logger.LogError($"Please, take care of this as soon as possible! - {message}");
        }

        public void Info(string message)
        {
            _logger.LogInformation($"Just a information.., look! - {message}");
        }

        public void Warning(string message)
        {
            _logger.LogWarning($"Just a warning.., take a look.. - {message}");
        }
    }
}
