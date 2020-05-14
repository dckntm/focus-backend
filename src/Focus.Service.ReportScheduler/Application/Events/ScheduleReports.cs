using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Services.Client;
using Focus.Application.Common.Messages.Commands;
using Focus.Application.Common.Messages.Events;
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

        public ScheduleReports(
            IServiceClient service,
            IDateTimeService date,
            IReportScheduleRepository repository)
        {
            _service = service;
            _date = date;
            _repository = repository;
        }

        public async Task Handle(NewDay notification, CancellationToken cancellationToken)
        {
            var today = _date.Now();

            try
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                var reportConstructConfigurations = schedules
                    .Where(x => _date.IsEarlier(today, x.EmissionEnd) && _date.IsEarlier(x.EmissionStart, today))
                    .Where(x => ShouldBeEmittedToday(x))
                    .Select(x => new ReportConstructionDescriptor()
                    {
                        ReportTemplateId = x.ReportTemplate,
                        AssignedOrganizationIds = x.AssignedOrganizations,
                        DeadlineDate = today + x.DeadlinePeriod
                    })
                    .ToList();

                var command = new ConstructReports()
                {
                    ReportDescriptors = reportConstructConfigurations
                };

                await _service.CommandAsync(command, "constructor", "api/cs/report/construct");
            }
            catch (Exception e)
            {
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