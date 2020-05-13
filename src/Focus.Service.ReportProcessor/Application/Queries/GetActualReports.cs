using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Enums;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetActualReports : IRequest<Result> { }

    public class GetActualReportsHandler : IRequestHandler<GetActualReports, Result>
    {
        private readonly IReportRepository _repository;

        public GetActualReportsHandler(
            IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetActualReports request, CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _repository.GetReportsAsync();

                return Result.Success(reports
                        .Where(r => DateTime.Compare(r.Deadline, DateTime.Now.ToUniversalTime()) >= 0)
                        .Select(r => new ReportInfoDto()
                        {
                            Title = r.Title,
                            Id = r.Id,
                            AssignedOrganizationId = r.AssignedOrganizationId,
                            ReportStatus = r.Status switch {
                                ReportStatus.InProgress => "InProgress",
                                ReportStatus.Overdue => "Overdue",
                                ReportStatus.Passed => "Passed",
                                _ => ""
                            },
                            Deadline = r.Deadline.ToLocalTime().ToString("dd.MM.yyyy")
                        })
                        .AsEnumerable());
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}