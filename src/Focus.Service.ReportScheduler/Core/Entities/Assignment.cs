using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportScheduler.Core.Enums;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class Assignment : ValueObject
    {
        public string Organization { get; set; }
        public bool IsDelegatedToCOA { get; set; }
        public ICollection<MemberAssignment> Assignees { get; set; }

        private Assignment()
        {
            IsDelegatedToCOA = false;
            Assignees = new List<MemberAssignment>();
        }

        public static Assignment For(string organizationId)
        {
            return new Assignment()
            {
                Organization = organizationId
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Organization;
            yield return IsDelegatedToCOA;

            foreach (var assignee in Assignees)
                yield return assignee;
        }
    }
}