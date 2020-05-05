using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplateInfos : IRequest<Result> { }

    public class GetReportTemplateInfosHandler
        : IRequestHandler<GetReportTemplateInfos, Result>
    {
        private readonly IReportTemplateRepository _repository;
        public GetReportTemplateInfosHandler(IReportTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(
            GetReportTemplateInfos request,
            CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<ReportTemplate> templates = await _repository.GetReportTemplatesAsync();

                return Result
                    .Success(templates
                        .Select(x => x.AsInfoDto())
                        .AsEnumerable());
            }
            catch (Exception e)
            {
                return Result
                    .Fail(e);
            }
        }
    }
}