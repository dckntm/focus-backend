using Focus.Application.Common.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Application.Services;
using Focus.Service.ReportProcessor.Enums;
using MediatR;
using System.Threading;

namespace Focus.Service.ReportProcessor.Application.Queries
{
    public class GetOrganizationReports : IRequest<Result>
    {
        public GetOrganizationReports(string organizationId)
        {
            OrganizationId = organizationId;
        }
        public string OrganizationId { get; private set; }
    }

    public class GetOrganizationReportsHandler : IRequestHandler<GetOrganizationReports, Result>
    {
        private readonly IReportRepository _repository;

        public GetOrganizationReportsHandler(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetOrganizationReports request, CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _repository.GetOrganizationReportsAsync(request.OrganizationId);

                return Result
                    .Success(reports
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
                return Result.Fail(e);
            }
        }
    }
}