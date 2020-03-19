using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;
using Focus.Service.ReportConstructor.Application.Common.Interface;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplate : IQuery<ReportTemplateDto>
    {
        // properties
        public string ReportTemplateId { get; }

        // ctors
        public GetReportTemplate(string reportTemplateId)
            => ReportTemplateId = reportTemplateId;

        // handler
        public class GetReportTemplateHandler : IQueryHandler<GetReportTemplate, ReportTemplateDto>
        {
            // fields
            private IReportTemplateRepository _repository;

            // ctors
            public GetReportTemplateHandler(IReportTemplateRepository repository)
                => _repository = repository;

            // behavior
            public Task<ReportTemplateDto> HandleAsync(GetReportTemplate query)
                => Task.Run(async () =>
                {
                    var report = await _repository.GetReportTemplateAsync(query.ReportTemplateId);

                    return report.AsDto();
                });
        }
    }
}