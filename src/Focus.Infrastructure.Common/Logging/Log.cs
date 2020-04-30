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
               $"\n------------------------ API LOG ------------------------\n\n{message}\n\n---------------------------------------------------------");
        }

        public void LogApplication(string message)
        {
            _logger.LogInformation(
                $"\n-------------------- APPLICATION LOG --------------------\n\n{message}\n\n---------------------------------------------------------");
        }

        public void LogCore(string message)
        {
            _logger.LogInformation(
                $"\n----------------------- CORE LOG ------------------------\n\n{message}\n\n---------------------------------------------------------");
        }

        public void LogInfrastructure(string message)
        {
            _logger.LogInformation(
                $"\n------------------- INFRASTRUCTURE LOG ------------------\n\n{message}\n\n---------------------------------------------------------");

        }
    }
}