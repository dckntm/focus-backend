using System;
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
    public class GetQueriedReports : IRequest<Result>
    {
        /// ids of organizations to query reports through
        /// if empty or null than we consider quering though all organizations
        public string[] ForOrganizations { get; set; }
        /// ids of templates to query reports through
        /// if empty or null than we consider quering though all templates
        public string[] ForTemplates { get; set; }
        /// which next 20 element we should pick 
        /// for (-infty, 0] - pick first 20, for N skip first 20 * Nth
        public int QueryIndex { get; set; }
        /// if null considered any
        /// in format dd.MM.yyyy
        public string DeadlineFrom { get; set; }
        /// in null considered any
        /// in format dd.MM.yyyy
        public string DeadlineTill { get; set; }
        /// for what statuses we should pick report
        public ReportStatus[] ForStatuses { get; set; }
    }

    public class GetQueriedReportsHandler : IRequestHandler<GetQueriedReports, Result>
    {

        private readonly IReportRepository _repository;
        private readonly ILogger<GetQueriedReportsHandler> _logger;

        public GetQueriedReportsHandler(ILogger<GetQueriedReportsHandler> logger, IReportRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Result> Handle(GetQueriedReports request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Quering Reports");

            try
            {
                var reports = await _repository.GetReportsAsync();

                if (request.ForOrganizations != null && request.ForOrganizations.Count() > 0)
                    reports = reports
                        .Where(r => request.ForOrganizations.Contains(r.AssignedOrganizationId));

                if (request.ForTemplates != null && request.ForTemplates.Count() > 0)
                    reports = reports
                        .Where(r => request.ForTemplates.Contains(r.ReportTemplateId));

                if (request.ForStatuses != null && request.ForStatuses.Count() > 0)
                    reports = reports
                        .Where(r => request.ForStatuses.Contains(r.Status));

                if (DateTime.TryParse(request.DeadlineFrom, out DateTime deadlineFrom))
                    reports = reports
                        .Where(r => r.Deadline.Date >= deadlineFrom.Date);

                if (DateTime.TryParse(request.DeadlineTill, out DateTime deadlineTill))
                    reports = reports
                        .Where(r => r.Deadline.Date <= deadlineTill.Date);

                return Result.Success(reports
                    .Skip(request.QueryIndex * 20)
                    .Select(r => r.AsInfoDto()));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "In Get Queried Reports Handler");
                return Result.Fail(e);
            }
        }
    }
}