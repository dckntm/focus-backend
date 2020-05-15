using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Focus.Application.Common.Messages.Events;
using Focus.Application.Common.Services.Messaging;

namespace Focus.Service.Automator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPublisher _publisher;
        private DateTime _today;

        public Worker(
            ILogger<Worker> logger,
            IPublisher publisher)
        {
            _today = DateTime.Now.ToLocalTime().Date;
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO make it publish event only once per day at night
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now.ToLocalTime().Date > _today.Date)
                {
                    _logger.LogInformation("[NO] Published New Day Event at: {time}", DateTimeOffset.Now);
                    _today = DateTime.Now.ToLocalTime().Date;
                    
                    _publisher.Publish(
                        message: new NewDay(),
                        exchangeName: "focus",
                        exchangeType: "topic",
                        routeKey: "focus.events.newday");
                }

                await Task.Delay(60 * 60 * 1000, stoppingToken);
            }
        }
    }
}
