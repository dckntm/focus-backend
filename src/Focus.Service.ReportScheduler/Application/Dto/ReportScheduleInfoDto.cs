using System.Collections.Generic;
using System.Linq;
using Focus.Service.ReportScheduler.Core.Entities;

namespace Focus.Service.ReportScheduler.Application.Dto
{
    public class ReportScheduleInfoDto
    {
        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        // string with id of (id, title)
        public ICollection<string> Organizations { get; set; }
        // string of format
        // DD.MM.YYYY-DD.MM.YYYY
        public string EmissionPeriod { get; set; }
        public Period DeadlinePeriod { get; set; }
    }

    public static class ReportScheduleInfoDtoExtensions
    {
        public static ReportScheduleInfoDto AsInfoDto(this ReportSchedule entity)
            => new ReportScheduleInfoDto
            {
                Id = entity.Id,
                ReportTemplate = entity.ReportTemplate,
                Organizations = entity.AssignedOrganizations,
                EmissionPeriod =
                    $"{entity.EmissionStart.ToLocalTime():dd.MM.yyyy}-{entity.EmissionEnd.ToLocalTime():dd.MM.yyyy}",
                DeadlinePeriod = entity.DeadlinePeriod
            };
    }
}