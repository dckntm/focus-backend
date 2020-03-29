using System;
using System.Collections.Generic;
using Convey.Types;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Infrastructure.Repository.Documents
{
    public class ReportScheduleDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<OrganizationAssignment> Organizations { get; set; }
        public Period DeadlinePeriod { get; set; }
        public Period EmissionPeriod { get; set; }
        public DateTime EmissionStart { get; set; }
        public DateTime EmissionEnd { get; set; }
    }
}