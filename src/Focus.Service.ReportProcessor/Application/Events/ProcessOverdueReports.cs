using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Messages.Events;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Enums;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Events
{
    public class ProcessOverdueReports : INotificationHandler<NewDay>
    {
        private readonly IReportRepository _repository;
        public ProcessOverdueReports(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(NewDay notification, CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _repository.GetReportsAsync();

                var expired = reports
                    .Where(r => r.Deadline.Date < DateTime.Today.Date)
                    .Select(r => r.Id);

                await _repository.ChangeReportsStatusAsync(expired, ReportStatus.Overdue);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}