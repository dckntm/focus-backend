using System;
using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class Period : ValueObject
    {
        public int Days { get; private set; }
        public int Months { get; private set; }
        public int Years { get; private set; }

        public Period(int days = 0, int months = 0, int years = 0)
        {
            if (days < 0 || months < 0 || years < 0)
                throw new ArgumentException($"DOMAIN EXCEPTION: Can't create Period with {days}-{months}-{years}");

            (Days, Months, Years) = (days, months, years);
        }

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

        public static Period operator +(Period left, Period right)
            => new Period(
                left.Days + right.Days,
                left.Months + right.Months,
                left.Years + right.Years);

        public static Period operator -(Period left, Period right)
            => new Period(
                left.Days - right.Days,
                left.Months - right.Months,
                left.Years - right.Years);
    }
}