using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Commands
{
    public class CreateReportTemplate : IRequest<RequestResult<string>>
    {
        public CreateReportTemplate(ReportTemplateDto reportTemplate)
            => ReportTemplate = reportTemplate;
        public ReportTemplateDto ReportTemplate { get; private set; }
    }

    public class CreateReportTemplateHandler : IRequestHandler<CreateReportTemplate, RequestResult<string>>
    {
        private readonly IReportTemplateRepository _repository;
        public CreateReportTemplateHandler(IReportTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<string>> Handle(CreateReportTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _repository.CreateReportTemplateAsync(request.ReportTemplate.AsEntity());

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