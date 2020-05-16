using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetStatistics : IRequest<Result> { }

    public class GetStatisticsHandler : IRequestHandler<GetStatistics, Result>
    {
        private readonly IReportRepository _repository;
        private readonly ILogger<GetStatisticsHandler> _logger;

        public GetStatisticsHandler(ILogger<GetStatisticsHandler> logger, IReportRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Result> Handle(GetStatistics request, CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _repository.GetReportsAsync();

                return Result.Success(new
                {
                    InProgressReports = reports
                        .Count(r => r.Status == ReportStatus.InProgress),
                    OverdueReports = reports
                        .Count(r => r.Status == ReportStatus.Overdue),
                    PassedReports = reports
                        .Count(r => r.Status == ReportStatus.Passed)
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "In Get Statistics Handler");
                return Result.Fail(e);
            }
        }
    }
}