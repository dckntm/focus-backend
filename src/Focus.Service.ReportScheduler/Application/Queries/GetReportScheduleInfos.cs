using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Application.Common.Dto.Extensions;
using Focus.Service.ReportScheduler.Application.Common.Interface;

namespace Focus.Service.ReportScheduler.Application.Queries
{
    public class GetReportScheduleInfos : IQuery<IEnumerable<ReportScheduleInfoDto>>
    {
        public class GetReportScheduleInfosHandler
            : IQueryHandler<GetReportScheduleInfos, IEnumerable<ReportScheduleInfoDto>>
        {
            private IReportScheduleRepository _repository;

            public GetReportScheduleInfosHandler(IReportScheduleRepository repository)
                => _repository = repository;

            public async Task<IEnumerable<ReportScheduleInfoDto>> HandleAsync(GetReportScheduleInfos query)
            {
                var schedules = await _repository.GetReportSchedulesAsync();

                return schedules
                    .Select(s => s.AsInfoDto())
                    .AsEnumerable();
            }
        }
    }
}