using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;
using Focus.Service.ReportConstructor.Application.Common.Interface;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplateInfos : IQuery<IEnumerable<ReportTemplateInfoDto>>
    {
        // handler
        public class GetReportTemplateInfosHandler
            : IQueryHandler<GetReportTemplateInfos, IEnumerable<ReportTemplateInfoDto>>
        {
            // fields
            private IReportTemplateRepository _repository;

            // ctors
            public GetReportTemplateInfosHandler(IReportTemplateRepository repository)
                => _repository = repository;

            // behavior
            public Task<IEnumerable<ReportTemplateInfoDto>> HandleAsync(GetReportTemplateInfos query)
                => Task.Run(async () =>
                {
                    var reports = await _repository
                                            .GetReportTemplatesAsync();

                    return reports
                        .Select(x => x.AsInfoDto())
                        .AsEnumerable();
                });
        }
    }
}