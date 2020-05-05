using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportScheduleInfos : IRequest<Result> { }

    public class GetReportScheduleInfosHandler
        : IRequestHandler<GetReportScheduleInfos, Result>
    {
        private readonly IReportScheduleRepository _repository;
        public GetReportScheduleInfosHandler(IReportScheduleRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result> Handle(GetReportScheduleInfos request, CancellationToken cancellationToken)
        {
            try
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                return Result.Success(schedules
                        .ToList()
                        .Select(x => x.AsInfoDto()));
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }


        }
    }
}