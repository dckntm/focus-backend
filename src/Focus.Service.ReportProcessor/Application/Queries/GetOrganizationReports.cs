using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Entities;
using Focus.Service.ReportProcessor.Enums;
using MediatR;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetOrganizationReports : IRequest<RequestResult<IEnumerable<ReportInfoDto>>>
    {
        public string OrganizationId { get; private set; }
    }

    public class GetOrganizationReportsHandler
        : IRequestHandler<GetOrganizationReports, RequestResult<IEnumerable<ReportInfoDto>>>
    {
        private readonly IReportRepository _repository;

        public GetOrganizationReportsHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<IEnumerable<ReportInfoDto>>> Handle(GetOrganizationReports request, CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _repository.GetOrganizationReportsAsync(request.OrganizationId);

                return RequestResult
                    .Successfull(reports
                        .AsEnumerable()
                        .Select(x => new ReportInfoDto()
                        {
                            Id = x.Id,
                            AssignedOrganizationId = x.AssignedOrganizationId,
                            ReportStatus = x.Status switch
                            {
                                ReportStatus.InProgress => "InProgress",
                                ReportStatus.Overdue => "Overdue",
                                ReportStatus.Passed => "Passed",
                                _ => ""
                            },
                            Deadline = x.Deadline.ToLocalTime().ToString()
                        })
                        .AsEnumerable());
            }
            catch (Exception e)
            {
                return RequestResult<IEnumerable<ReportInfoDto>>
                    .Failed(e);
            }
        }
    }
}