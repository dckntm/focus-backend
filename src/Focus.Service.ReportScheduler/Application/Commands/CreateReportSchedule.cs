using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    public class CreateReportSchedule : IRequest<Result>
    {
        public ReportScheduleDto Schedule { get; private set; }

        public CreateReportSchedule(ReportScheduleDto schedule)
            => Schedule = schedule;
    }

    public class CreateReportScheduleHandler : IRequestHandler<CreateReportSchedule, Result>
    {
        private readonly IReportScheduleRepository _repository;
        public CreateReportScheduleHandler(IReportScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(CreateReportSchedule request, CancellationToken cancellationToken)
        {
            try
            {
                string id = await _repository.CreateReportScheduleAsync(request.Schedule.AsEntity());

                return Result.Success(id);
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}