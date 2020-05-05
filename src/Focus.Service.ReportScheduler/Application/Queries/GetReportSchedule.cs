using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportSchedule : IRequest<Result>
    {
        public GetReportSchedule(string scheduleId)
            => ScheduleId = scheduleId;

        public string ScheduleId { get; private set; }
    }

    public class GetReportScheduleHandler : IRequestHandler<GetReportSchedule, Result>
    {
        private readonly IReportScheduleRepository _repository;
        public GetReportScheduleHandler(IReportScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetReportSchedule request, CancellationToken cancellationToken)
        {
            try
            {
                var schedule = await _repository.GetReportScheduleAsync(request.ScheduleId);

                return Result.Success(schedule.AsDto());
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}