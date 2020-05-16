using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetStatistics : IRequest<Result> { }

    public class GetStatisticsHandler : IRequestHandler<GetStatistics, Result>
    {

        private readonly IReportTemplateRepository _repository;
        private readonly ILogger<GetStatisticsHandler> _logger;

        public GetStatisticsHandler(IReportTemplateRepository repository, ILogger<GetStatisticsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(GetStatistics request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting Report Constructor statistics");

            try
            {
                var templates = await _repository.GetReportTemplatesAsync();

                return Result.Success(new
                {
                    TotalTemplates = templates.Count()
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