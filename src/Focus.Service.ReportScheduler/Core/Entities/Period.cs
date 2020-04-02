using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class Period : ValueObject
    {
        public int Days { get; set; }
        public int Months { get; set; }
        public int Years { get; set; }

        public Period(int days = 0, int months = 0, int years = 0)
            => (Days, Months, Years) = (days, months, years);

        public override string ToString()
        {
            return $"{Days}.{Months}.{Years}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Days;
            yield return Months;
            yield return Years;
        }
    }
}