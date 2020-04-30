using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportSchedule : IRequest<RequestResult<ReportScheduleDto>>
    {
        public GetReportSchedule(string scheduleId)
            => ScheduleId = scheduleId;

        public string ScheduleId { get; private set; }
    }

    public class GetReportScheduleHandler : IRequestHandler<GetReportSchedule, RequestResult<ReportScheduleDto>>
    {
        private readonly IReportScheduleRepository _repository;
        private readonly ILog _logger;

        public GetReportScheduleHandler(
            IReportScheduleRepository repository,
            ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<ReportScheduleDto>> Handle(GetReportSchedule request, CancellationToken cancellationToken)
        {
            try
            {
                var schedule = await _repository.GetReportScheduleAsync(request.ScheduleId);

                _logger.LogApplication($"Successfully got schedule: {schedule.Id}");

                return RequestResult<ReportScheduleDto>
                    .Successfull(schedule.AsDto());
            }
            catch (Exception e)
            {
                return RequestResult<ReportScheduleDto>
                    .Failed()
                    .WithException(e);
            }
        }
    }
}