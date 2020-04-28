using System.Collections.Generic;
using System.Threading.Tasks;
using Focus.Service.ReportProcessor.Application.Dto;
using Focus.Service.ReportProcessor.Entities;

namespace Focus.Service.ReportProcessor.Application.Services
{
    public interface IReportRepository
    {
        Task CreateReportsAsync(IEnumerable<Report> reports);
        Task<Report> GetReportAsync(string reportId);
        Task<IEnumerable<Report>> GetOrganizationReportsAsync(string organizationId);
        Task SaveReport(ReportUpdateDto report);
        Task PassReport(ReportUpdateDto report);
    }
}