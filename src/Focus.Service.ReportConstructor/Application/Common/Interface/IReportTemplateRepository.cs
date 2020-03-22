using System.Linq;
using System.Threading.Tasks;
using Focus.Service.ReportConstructor.Domain.Entities;

namespace Focus.Service.ReportConstructor.Application.Common.Interface
{
    public interface IReportTemplateRepository
    {
        Task<string> CreateReportTemplateAsync(ReportTemplate reportTemplate);
        Task<IQueryable<ReportTemplate>> GetReportTemplatesAsync();
        Task<ReportTemplate> GetReportTemplateAsync(string id);
    }
}