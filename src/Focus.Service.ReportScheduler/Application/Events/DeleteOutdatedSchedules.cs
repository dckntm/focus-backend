using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Messages.Events;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportScheduler.Application.Events
{
    // We delete schedules that finished more than a 15 days ago and were not prolonged
    public class DeleteOutdatedSchedules : INotificationHandler<NewDay>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly ILogger<DeleteOutdatedSchedules> _logger;

        public DeleteOutdatedSchedules(IReportScheduleRepository repository, ILogger<DeleteOutdatedSchedules> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(NewDay notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initialized deletion of outdated schedules");

            try
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                var outdated = schedules
                    .Where(s => s.EmissionEnd.Date < DateTime.Now.Date.AddDays(15))
                    .Select(s => s.Id);

                _logger.LogInformation($"Schedules to delete: {outdated.Count()}");

                if (outdated != null && outdated.Count() > 0)
                    await _repository.DeleteSchedulesAsync(outdated);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw e;
            }
        }
    }
}