using System.Linq;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Common.Dto.Extensions
{
    public static class ReportScheduleInfoDtoExtensions
    {
        public static ReportScheduleInfoDto AsInfoDto(this ReportSchedule entity)
            => new ReportScheduleInfoDto
            {
                Id = entity.Id,
                ReportTemplate = entity.ReportTemplate,
                AssignedOrganizations = entity.Organizations
                    .Select(o => o.OrganizationId)
                    .ToList(),
                EmissionPeriod = 
                    $"{entity.EmissionStart.ToLocalTime().ToString("dd.MM.yyyy")}-{entity.EmissionEnd.ToLocalTime().ToString("dd.MM.yyyy")}",
                DeadlinePeriod = entity.DeadlinePeriod.ToString()
            };
    }
}