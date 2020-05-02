using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Entities;
using Focus.Application.Common.Abstract;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;
using Focus.Application.Common.Services.Logging;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetReport : IRequest<RequestResult<ReportUpdateDto>>
    {
        public GetReport(string reportId)
        {
            ReportId = reportId;
        }
        public string ReportId { get; private set; }
    }

    public class GetReportHandler : IRequestHandler<GetReport, RequestResult<ReportUpdateDto>>
    {
        private readonly IReportRepository _repository;
        private readonly ILog _logger;

        public GetReportHandler(IReportRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<ReportUpdateDto>> Handle(GetReport request, CancellationToken cancellationToken)
        {
            try
            {
                Report report = await _repository.GetReportAsync(request.ReportId);

                if (report is null)
                    throw new Exception($"APPLICATION No report with {request.ReportId} id");

                _logger.LogApplication($"Successfully got report {request.ReportId}");

                return RequestResult
                    .Successfull(new ReportUpdateDto()
                    {
                        Id = report.Id,
                        QuestionnaireAnswers = report.QuestionnaireAnswers,
                        TableAnswers = report.TableAnswers
                    });
            }
            catch (Exception e)
            {
                _logger.LogApplication($"Failed to get report {request.ReportId}");

                return RequestResult<ReportUpdateDto>
                    .Failed(e);
            }
        }
    }
}