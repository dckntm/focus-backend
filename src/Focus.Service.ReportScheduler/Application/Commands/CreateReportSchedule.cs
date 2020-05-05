using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
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
        private readonly ILog _logger;
        public CreateReportScheduleHandler(
            IReportScheduleRepository repository,
            ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<string>> Handle(CreateReportSchedule request, CancellationToken cancellationToken)
        {
            try
            {
                string id = await _repository.CreateReportScheduleAsync(request.Schedule.AsEntity());

                _logger.LogApplication($"Successfully created report schedule with id: {id}");

                return RequestResult<string>
                    .Successfull(id);
            }
            catch (Exception e)
            {
                _logger.LogApplication($"Failed to create report schedule\n{e.Message}");

                return RequestResult<string>
                    .Failed()
                    .WithException(e);
            }
        }
    }
}