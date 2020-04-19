using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Focus.Core.Common.Messages;
using Focus.Service.ReportScheduler.Application.Services;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportScheduler.Application.Events
{
    public class EmitReportsEventHandler : INotificationHandler<NewDayEvent>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly ILogger<EmitReportsEventHandler> _logger;

        public EmitReportsEventHandler(
            IReportScheduleRepository repository,
            ILogger<EmitReportsEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(NewDayEvent notification, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Acquired new New Day Event Notification at {DateTime.Now}");

            Console.WriteLine($"Acquired new New Day Event Notification at {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}