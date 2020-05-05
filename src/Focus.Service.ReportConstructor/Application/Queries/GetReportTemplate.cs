using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using MediatR;

namespace Focus.Service.ReportConstructor.Application.Queries
{
    public class GetReportTemplate : IRequest<Result>
    {
        public GetReportTemplate(string reportId)
            => ReportId = reportId;

        public string ReportId { get; private set; }
    }

    public class GetReportTemplateHandler : IRequestHandler<GetReportTemplate, Result>
    {
        private readonly IReportTemplateRepository _repository;

        public GetReportTemplateHandler(IReportTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetReportTemplate request, CancellationToken cancellationToken)
        {
            try
            {
                var template = await _repository.GetReportTemplateAsync(request.ReportId);

                return Result.Success(template.AsDto());
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}