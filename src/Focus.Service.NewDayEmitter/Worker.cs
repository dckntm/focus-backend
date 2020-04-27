using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Messaging;
using Focus.Core.Common.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Focus.Service.NewDayEmitter
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPublisher _publisher;

        public Worker(
            ILogger<Worker> logger,
            IPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Published New Day Event at: {time}", DateTimeOffset.Now);

                _publisher.Publish(
                    message: new NewDayEvent(), 
                    exchangeName: "focus", 
                    exchangeType: "topic",
                    routeKey: "focus.events.newday.schedule");

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
