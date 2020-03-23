using System.Linq;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Common.Dto.Extensions
{
    public static class ReportScheduleInfoExtensions
    {
        public static ReportScheduleInfoDto ToInfoDto(this ReportSchedule reportSchedule)
            => new ReportScheduleInfoDto
            {
                Id = reportSchedule.Id,
                ReportTemplate = reportSchedule.ReportTemplate,
                AssignedOrganizations = reportSchedule.Organizations
                    .Select(o => o.OrganizationId)
                    .ToList(),
                EmissionPeriod = $"{reportSchedule.EmissionStart.ToString("dd.MM.yyyy")}-{reportSchedule.EmissionEnd.ToString("dd.MM.yyyy")}",
                DeadlinePeriod = $"{reportSchedule.DeadlinePeriod.Days}.{reportSchedule.DeadlinePeriod.Month}.{reportSchedule.DeadlinePeriod.Years}"
            };
    }
}