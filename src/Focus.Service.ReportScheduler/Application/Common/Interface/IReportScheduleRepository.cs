using System.Linq;
using System.Threading.Tasks;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Common.Interface
{
    public interface IReportScheduleRepository {
        Task<ReportSchedule> GetReportScheduleAsync(string id);
        Task<IQueryable<ReportSchedule>> GetReportSchedulesAsync();
        Task<string> CreateReportScheduleAsync(ReportSchedule schedule);
    }
}