using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportConstructor.Application.Commands
{
    public class CreateReportTemplate : IRequest<Result>
    {
        public CreateReportTemplate(ReportTemplateDto reportTemplate)
            => ReportTemplate = reportTemplate;
        public ReportTemplateDto ReportTemplate { get; private set; }
    }

    public class CreateReportTemplateHandler : IRequestHandler<CreateReportTemplate, Result>
    {
        private readonly ILogger<CreateReportTemplateHandler> _logger;
        private readonly IReportTemplateRepository _repository;
        public CreateReportTemplateHandler(IReportTemplateRepository repository, ILogger<CreateReportTemplateHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateReportTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _repository.CreateReportTemplateAsync(request.ReportTemplate.AsEntity());

                return Result.Success(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Result.Fail(e);
            }
        }
    }
}