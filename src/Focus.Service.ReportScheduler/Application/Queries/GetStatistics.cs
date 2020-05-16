using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetStatistics : IRequest<Result> { }

    public class GetStatisticsHandler : IRequestHandler<GetStatistics, Result>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly ILogger<GetStatisticsHandler> _logger;
        public GetStatisticsHandler(IReportScheduleRepository repository, ILogger<GetStatisticsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(GetStatistics request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting statistics");

            try
            {
                var today = DateTime.Today.ToLocalTime().Date;

                var schedules = await _repository.GetReportSchedulesAsync();

                var statistics = new
                {
                    TotalSchedules = schedules.Count(),
                    ExecutingSchedules = schedules
                        .Count(s => today > s.EmissionStart.Date && s.EmissionEnd.Date > today),
                    OutdatedSchedules = schedules
                        .Count(s => today > s.EmissionEnd.Date),
                    FutureSchedules = schedules
                        .Count(s => today < s.EmissionStart.Date)
                };

                return Result.Success(statistics);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "In Get Statistics Handler");
                return Result.Fail(e);
            }
        }
    }
}