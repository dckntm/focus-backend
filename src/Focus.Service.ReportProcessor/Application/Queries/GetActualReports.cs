using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetActualReports : IRequest<RequestResult<IEnumerable<ReportInfoDto>>>
    {

    }

    public class GetActualReportsHandler : IRequestHandler<GetActualReports, RequestResult<IEnumerable<ReportInfoDto>>>
    {
        private readonly IReportRepository _repository;
        private readonly ILogger<GetActualReportsHandler> _logger;

        public GetActualReportsHandler(
            IReportRepository repository,
            ILogger<GetActualReportsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RequestResult<IEnumerable<ReportInfoDto>>> Handle(GetActualReports request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling request for actual reports");

            try
            {
                var reports = await _repository.GetReportsAsync();

                return RequestResult
                    .Successfull(reports
                        .Where(r => DateTime.Compare(r.Deadline, DateTime.Now.ToUniversalTime()) >= 0)
                        .Select(r => new ReportInfoDto()
                        {
                            Id = r.Id,
                            AssignedOrganizationId = r.AssignedOrganizationId,
                            ReportStatus = r.Status switch {
                                ReportStatus.InProgress => "InProgress",
                                ReportStatus.Overdue => "Overdue",
                                ReportStatus.Passed => "Passed",
                                _ => ""
                            },
                            Deadline = r.Deadline.ToLocalTime().ToString()
                        })
                        .AsEnumerable());
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get actual reports");

                return RequestResult<IEnumerable<ReportInfoDto>>
                    .Failed(e);
            }
        }
    }
}