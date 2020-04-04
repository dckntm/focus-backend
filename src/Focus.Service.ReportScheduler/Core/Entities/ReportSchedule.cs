using System;
using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class ReportSchedule : ValueObject
    {
        public ReportSchedule(
            string id,
            string reportTemplate,
            IList<Assignment> organizations,
            Period deadlinePeriod,
            Period emissionPeriod,
            DateTime emissionStart,
            DateTime emissionEnd)
        {
            if (organizations is null || organizations.Count < 0)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't instantiate Report Schedule with null or empty assignments");

            Id = id;
            ReportTemplate = reportTemplate;
            Organizations = organizations;
            DeadlinePeriod = deadlinePeriod;
            EmissionPeriod = emissionPeriod;
            EmissionStart = emissionStart;
            EmissionEnd = emissionEnd;
        }

        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        public IList<Assignment> Organizations { get; set; }
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
    
        // TODO: add assignment functionality
    }
}