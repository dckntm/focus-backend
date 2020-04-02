using System.Linq;
using System.Threading.Tasks;
using Focus.Service.ReportConstructor.Core.Entities;

namespace Focus.Service.ReportConstructor.Application.Services
{
    public interface IReportTemplateRepository
    {
        Task<string> CreateReportTemplateAsync(ReportTemplate reportTemplate);
        Task<IQueryable<ReportTemplate>> GetReportTemplatesAsync();
        Task<ReportTemplate> GetReportTemplateAsync(string reportId);
    }
}