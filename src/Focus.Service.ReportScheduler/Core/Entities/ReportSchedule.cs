using System;
using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class ReportSchedule : ValueObject
    {
        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<Assignment> Organizations { get; set; }
        public Period DeadlinePeriod { get; set; }
        public Period EmissionPeriod { get; set; }
        public DateTime EmissionStart { get; set; }
        public DateTime EmissionEnd { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ReportTemplate;

            foreach (var org in Organizations)
                yield return org;

            yield return DeadlinePeriod;
            yield return EmissionPeriod;
            yield return EmissionStart;
            yield return EmissionEnd;
        }
    }
}