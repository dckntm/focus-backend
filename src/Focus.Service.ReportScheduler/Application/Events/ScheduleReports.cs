using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Client;
using Focus.Application.Common.Services.Logging;
using Focus.Core.Common.Messages.Commands;
using Focus.Core.Common.Messages.Events;
using Focus.Service.ReportScheduler.Application.Services;
using Focus.Service.ReportScheduler.Core.Entities;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Events
{
    public class ScheduleReports : INotificationHandler<NewDay>
    {
        public readonly IServiceClient _service;
        private readonly IDateTimeService _date;
        private readonly IReportScheduleRepository _repository;
        private readonly ILog _logger;

        public ScheduleReports(
            IServiceClient service,
            IDateTimeService date,
            IReportScheduleRepository repository,
            ILog logger)
        {
            _service = service;
            _date = date;
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(NewDay notification, CancellationToken cancellationToken)
        {
            var today = _date.Now();

            try
            {
                _logger.LogApplication("Received new Day notification => starting reports scheduling");

                var schedules = await _repository.GetReportSchedulesAsync();

                var reportConstructConfigurations = schedules
                    .Where(x => _date.IsEarlier(today, x.EmissionEnd) && _date.IsEarlier(x.EmissionStart, today))
                    .Where(x => ShouldBeEmittedToday(x))
                    .Select(x => new ReportConstructionDescriptor()
                    {
                        ReportTemplateId = x.ReportTemplate,
                        AssignedOrganizationIds = x.Organizations
                            .Select(o => o.Organization)
                            .ToList(),
                        DeadlineDate = today + x.DeadlinePeriod
                    })
                    .ToList();

                var command = new ConstructReports()
                {
                    ReportDescriptors = reportConstructConfigurations
                };

                await _service.CommandAsync(command, "constructor", "api/cs/report/construct");

                _logger.LogApplication("Send construct reports command");
            }
            catch (Exception e)
            {
                _logger.LogApplication("Failed to schedule reports");
                throw e;
            }
        }

        private bool ShouldBeEmittedToday(ReportSchedule schedule)
        {
            for (int n = 0;
                _date.IsEarlier(schedule.EmissionStart + n * schedule.EmissionPeriod, schedule.EmissionEnd);
                n++)
            {
                if (_date.IsToday(schedule.EmissionStart + n * schedule.EmissionPeriod))
                {
                    return true;
                }
            }

            return false;
        }
    }
}