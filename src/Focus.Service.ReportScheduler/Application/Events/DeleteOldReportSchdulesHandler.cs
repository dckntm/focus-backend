using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Core.Common.Messages;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportScheduler.Application.Events
{
    // public class DeleteOldReportSchedulesHandler : INotificationHandler<NewDayEvent>
    // {
    //     private readonly ILogger<DeleteOldReportSchedulesHandler> _logger;

    //     private readonly IReportScheduleRepository _repository;

    //     public DeleteOldReportSchedulesHandler(
    //         ILogger<DeleteOldReportSchedulesHandler> logger,
    //         IReportScheduleRepository repository)
    //     {
    //         _logger = logger;
    //         _repository = repository;
    //     }

    //     public Task Handle(NewDayEvent notification, CancellationToken cancellationToken)
    //     {
    //         // _logger.LogInformation($"Ran deleting old Report Schedules at {DateTime.Now}");

    //         return Task.CompletedTask;
    //     }
    // }
}