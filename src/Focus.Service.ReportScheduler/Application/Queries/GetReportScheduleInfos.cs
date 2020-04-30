using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportScheduleInfos : IRequest<RequestResult<IEnumerable<ReportScheduleInfoDto>>> { }

    public class GetReportScheduleInfosHandler
        : IRequestHandler<GetReportScheduleInfos, RequestResult<IEnumerable<ReportScheduleInfoDto>>>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly ILog _logger;
        public GetReportScheduleInfosHandler(IReportScheduleRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<IEnumerable<ReportScheduleInfoDto>>> Handle(GetReportScheduleInfos request, CancellationToken cancellationToken)
        {
            try
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                _logger.LogApplication($"Successfully got info about {schedules.Count()} of schedules");

                return RequestResult<IEnumerable<ReportScheduleInfoDto>>
                    .Successfull(schedules
                        .ToList()
                        .Select(x => x.AsInfoDto()));
            }
            catch (Exception e)
            {
                return RequestResult<IEnumerable<ReportScheduleInfoDto>>
                    .Failed()
                    .WithException(e);
            }


        }
    }
}