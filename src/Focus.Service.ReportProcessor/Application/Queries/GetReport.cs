using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Entities;
using Focus.Application.Common.Abstract;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetReport : IRequest<Result>
    {
        public GetReport(string reportId)
        {
            ReportId = reportId;
        }
        public string ReportId { get; private set; }
    }

    public class GetReportHandler : IRequestHandler<GetReport, Result>
    {
        private readonly IReportRepository _repository;

        public GetReportHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetReport request, CancellationToken cancellationToken)
        {
            try
            {
                Report report = await _repository.GetReportAsync(request.ReportId);

                if (report is null)
                    throw new Exception($"APPLICATION No report with {request.ReportId} id");

                return Result.Success(new ReportUpdateDto()
                    {
                        Id = report.Id,
                        QuestionnaireAnswers = report.QuestionnaireAnswers,
                        TableAnswers = report.TableAnswers
                    });
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}