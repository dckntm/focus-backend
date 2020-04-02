using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Service.ReportScheduler.Application.Services;
using MediatR;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    // command
    public class CreateReportSchedule : IRequest<RequestResult<string>>
    {
        public ReportScheduleDto Schedule { get; private set; }

        public CreateReportSchedule(ReportScheduleDto schedule)
            => Schedule = schedule;
    }

    // command handler
    public class CreateReportScheduleHandler : IRequestHandler<CreateReportSchedule, RequestResult<string>>
    {
        private readonly IReportScheduleRepository _repository;
        public CreateReportScheduleHandler(IReportScheduleRepository repository)
            => _repository = repository;

        public async Task<RequestResult<string>> Handle(CreateReportSchedule request, CancellationToken cancellationToken)
        {
            try
            {
                string id = await _repository.CreateReportScheduleAsync(request.Schedule.AsEntity());

                return RequestResult<string>
                    .Successfull(id);
            }
            catch (Exception e)
            {
                return RequestResult<string>
                    .Failed()
                    .WithException(e);
            }
        }
    }
}