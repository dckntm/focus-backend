using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Messages.Events;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Events
{
    // We delete schedules that finished more than a 15 days ago and were not prolonged
    public class DeleteOutdatedSchedules : INotificationHandler<NewDay>
    {
        private readonly IReportScheduleRepository _repository;

        public DeleteOutdatedSchedules(IReportScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(NewDay notification, CancellationToken cancellationToken)
        {
            try
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                var outdated = schedules
                    .Where(s => s.EmissionEnd.Date > DateTime.Now.Date.AddDays(15))
                    .Select(s => s.Id);

                await _repository.DeleteSchedulesAsync(outdated);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}