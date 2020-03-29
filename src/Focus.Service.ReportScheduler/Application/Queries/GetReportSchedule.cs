using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Application.Common.Dto.Extensions;
using Focus.Service.ReportScheduler.Application.Common.Interface;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportSchedule : IQuery<ReportScheduleDto>
    {
        public string Id { get; set; }

        public GetReportSchedule(string id)
            => Id = id;

        public class GetReportScheduleHandler : IQueryHandler<GetReportSchedule, ReportScheduleDto>
        {
            private IReportScheduleRepository _repository;

            public GetReportScheduleHandler(IReportScheduleRepository repository)
                => _repository = repository;
            public async Task<ReportScheduleDto> HandleAsync(GetReportSchedule query)
            {
                var schedule = await _repository.GetReportScheduleAsync(query.Id);

                return schedule.AsDto();
            }
        }
    }
}