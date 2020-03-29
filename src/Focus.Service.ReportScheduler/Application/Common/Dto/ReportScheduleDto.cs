using System.Collections.Generic;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Common.Dto
{
    public class ReportScheduleDto
    {
        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<OrganizationAssignment> Organizations { get; set; }
        // string in D.M.Y format
        public string DeadlinePeriod { get; set; }
        // string in D.M.Y format
        public string EmissionPeriod { get; set; }
        // string with DateTime format dd.MM.yyyy
        public string EmissionStart { get; set; }
        // string with DateTime format dd.MM.yyyy
        public string EmissionEnd { get; set; }
    }
}