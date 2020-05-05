using Focus.Application.Common.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Focus.Infrastructure.Common.Logging
{
    public class Log : ILog
    {
        private readonly ILogger<Log> _logger;

        public Log(ILogger<Log> logger)
        {
            _logger = logger;
        }

        public void LogApi(string message)
        {
            _logger.LogInformation(
               $"API LOG {message}");
        }

        public void LogApplication(string message)
        {
            _logger.LogInformation(
                $"APPLICATION LOG {message}");
        }

        public void LogCore(string message)
        {
            _logger.LogInformation(
                $"CORE LOG {message}");
        }

        public void LogInfrastructure(string message)
        {
            _logger.LogInformation(
                $"INFRASTRUCTURE LOG {message}");

        }
    }
}