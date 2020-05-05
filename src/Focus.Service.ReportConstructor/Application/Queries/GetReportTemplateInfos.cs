using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Application.Common.Services.Logging;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplateInfos : IRequest<RequestResult<IEnumerable<ReportTemplateInfoDto>>> { }

    public class GetReportTemplateInfosHandler
        : IRequestHandler<GetReportTemplateInfos, RequestResult<IEnumerable<ReportTemplateInfoDto>>>
    {
        private readonly IReportTemplateRepository _repository;
        private readonly ILog _logger;

        public GetReportTemplateInfosHandler(IReportTemplateRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<IEnumerable<ReportTemplateInfoDto>>> Handle(
            GetReportTemplateInfos request,
            CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<ReportTemplate> templates = await _repository.GetReportTemplatesAsync();

                _logger.LogApplication($"Successfully got info about {templates.Count()} of report templates");

                return RequestResult
                    .Successfull(templates
                        .Select(x => x.AsInfoDto())
                        .AsEnumerable());
            }
            catch (Exception e)
            {
                _logger.LogApplication($"Failed to get info about reports due to\n{e.Message}");

                return RequestResult<IEnumerable<ReportTemplateInfoDto>>
                    .Failed(e);
            }
        }
    }
}