using System.Collections.Generic;

namespace Focus.Service.ReportScheduler.Domain.Entities
{
    public class OrganizationAssignment
    {
        public string OrganizationId { get; set; }
        public bool IsDelegatedToCOA { get; set; }
        public ICollection<Assignment> Assignees { get; set; }
    }
}