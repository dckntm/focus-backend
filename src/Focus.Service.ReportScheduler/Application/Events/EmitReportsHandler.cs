using Focus.Service.ReportScheduler.Application.Services;
using Focus.Application.Common.Services.Messaging;
using Focus.Service.ReportScheduler.Core.Entities;
using Focus.Core.Common.Messages;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Events
{
    public class EmitReportsEventHandler : INotificationHandler<NewDayEvent>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly IPublisher _publisher;
        private readonly IDateTimeService _date;

        public EmitReportsEventHandler(
            IReportScheduleRepository repository,
            IDateTimeService date)
        {
            _repository = repository;
            _date = date;
        }

        public async Task Handle(NewDayEvent notification, CancellationToken cancellationToken)
        {
            var today = _date.Now();

            var schedules = await _repository.GetReportSchedulesAsync();

            var reportConfigs = schedules
                .Where(x => _date.IsEarlier(today, x.EmissionEnd) && _date.IsEarlier(x.EmissionStart, today))
                .Where(x => ShouldBeEmittedToday(x))
                .Select(x => new ReportConfiguration
                {
                    ReportTemplateId = x.Id,
                    AssignedOrganizationIds = x.Organizations
                                                .Select(o => o.Organization).ToList(),
                    Deadline = _date.Now() + x.DeadlinePeriod
                });

            _publisher.Publish(
                new OnReportConstructing()
                {
                    NewReports = reportConfigs.ToList()
                },
                exchangeName: "focus",
                exchangeType: "topic",
                routeKey: "focus.report.constructing");
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