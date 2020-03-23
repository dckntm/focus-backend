using System.Collections.Generic;

namespace Focus.Service.ReportScheduler.Application.Common.Dto
{
    public class ReportScheduleInfoDto
    {
        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<string> AssignedOrganizations { get; set; }
        // string of format
        // DD.MM.YYYY-DD.MM.YYYY
        public string EmissionPeriod { get; set; }
        // string of format
        // D.M.Y 
        // where D M Y - positive integers
        public string DeadlinePeriod { get; set; }
    }
}