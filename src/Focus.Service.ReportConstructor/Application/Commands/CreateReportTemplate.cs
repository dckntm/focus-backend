using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
using Focus.Service.ReportConstructor.Application.Dto;
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
        private readonly ILog _logger;
        public CreateReportTemplateHandler(IReportTemplateRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<string>> Handle(CreateReportTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _repository.CreateReportTemplateAsync(request.ReportTemplate.AsEntity());

                _logger.LogApplication($"Successfully create report template: {id}");

                return RequestResult<string>
                    .Successfull(id);
            }
            catch (Exception e)
            {
                _logger.LogApplication($"Failed to create report template");

                return RequestResult<string>
                    .Failed()
                    .WithException(e);
            }
        }
    }
}