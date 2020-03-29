using System;
using System.Collections.Generic;

namespace Focus.Service.ReportScheduler.Domain.Entities
{
    public class ReportSchedule
    {
        // properties
        // unique identifier for Report Schedule
        public string Id { get; set; }
        // Report Template which is used as foundation for generated Report
        public string ReportTemplate { get; set; }
        // Organizations assigned to it & assignment settings per org
        public ICollection<OrganizationAssignment> Organizations { get; set; }
        // how much time report is accessible for editing and passing
        public Period DeadlinePeriod { get; set; }
        // how often we will create new instances of reports
        public Period EmissionPeriod { get; set; }
        // when we start emitting reports
        public DateTime EmissionStart { get; set; }
        // when we stop emitting reports
        public DateTime EmissionEnd { get; set; }
    }
}