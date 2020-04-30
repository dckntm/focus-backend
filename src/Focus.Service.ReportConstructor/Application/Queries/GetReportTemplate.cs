using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplate : IRequest<RequestResult<ReportTemplateDto>>
    {
        public GetReportTemplate(string reportId)
            => ReportId = reportId;

        public string ReportId { get; private set; }
    }

    public class GetReportTemplateHandler : IRequestHandler<GetReportTemplate, RequestResult<ReportTemplateDto>>
    {
        private readonly IReportTemplateRepository _repository;
        private readonly ILog _logger;

        public GetReportTemplateHandler(IReportTemplateRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<ReportTemplateDto>> Handle(GetReportTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                var template = await _repository.GetReportTemplateAsync(request.ReportId);

                _logger.LogApplication($"Successfully got report template: {template.Id}");

                return RequestResult
                    .Successfull(template.AsDto());
            }
            catch (Exception e)
            {
                _logger.LogApplication($"Failed to get report template {request.ReportId} due to:\n {e.Message}");

                return RequestResult<ReportTemplateDto>
                    .Failed(e);
            }
        }
    }
}