using System.Linq;
using System.Threading.Tasks;
using Focus.Service.ReportScheduler.Core.Entities;

namespace Focus.Service.ReportScheduler.Application.Services
{
    public interface IReportScheduleRepository
    {
        Task<string> CreateReportScheduleAsync(ReportSchedule schedule);
        Task<IQueryable<ReportSchedule>> GetReportSchedulesAsync();
        Task<ReportSchedule> GetReportScheduleAsync(string scheduleId);
    }
}