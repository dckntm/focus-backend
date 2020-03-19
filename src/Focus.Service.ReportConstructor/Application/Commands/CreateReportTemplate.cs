using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;
using Focus.Service.ReportConstructor.Application.Common.Interface;

namespace Focus.Service.ReportConstructor.Application.Commands
{
    public class CreateReportTemplate : ICommand
    {
        // properties
        public ReportTemplateDto ReportTemplate { get; }
        public string ReportId { get; set; }

        // ctors
        public CreateReportTemplate(ReportTemplateDto report)
            => ReportTemplate = report;

        // handler
        public class CreateReportTemplateHandler : ICommandHandler<CreateReportTemplate>
        {
            // fields
            private IReportTemplateRepository _repository;

            // ctors
            public CreateReportTemplateHandler(IReportTemplateRepository repository)
                => _repository = repository;

            // behavior
            public async Task HandleAsync(CreateReportTemplate command)
            {
                command.ReportId = await _repository
                    .CreateReportTemplateAsync(command.ReportTemplate.AsEntity());
            }
        }
    }
}