using System;

namespace Focus.Service.ReportScheduler.Domain.Entities
{
    public class Period
    {
        public int Days { get; set; }
        public int Months { get; set; }
        public int Years { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Period period &&
                   Days == period.Days &&
                   Months == period.Months &&
                   Years == period.Years;
        }

        public override string ToString()
        {
            return $"{Days}.{Months}.{Years}";
        }
    }
}