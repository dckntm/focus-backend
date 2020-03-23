using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Application.Common.Dto.Extensions;
using Focus.Service.ReportScheduler.Application.Common.Interface;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    public class CreateReportSchedule : ICommand
    {
        // properties
        public ReportScheduleDto ReportSchedule { get; }
        public string ScheduleId { get; set; }

        // ctor
        public CreateReportSchedule(ReportScheduleDto schedule)
            => ReportSchedule = schedule;

        // handler
        public class CreateReportScheduleHandler : ICommandHandler<CreateReportSchedule>
        {
            // fifelds
            private IReportScheduleRepository _repository;

            // ctor
            public CreateReportScheduleHandler(IReportScheduleRepository repository)
                => _repository = repository;

            // beahvior
            public async Task HandleAsync(CreateReportSchedule command)
                => command.ScheduleId = await _repository.CreateReportScheduleAsync(command.ReportSchedule.AsEntity());
        }
    }
}