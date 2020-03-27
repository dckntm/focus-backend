using Focus.Service.ReportScheduler.Domain.Entities;
using Xunit;

namespace Focus.Service.ReportScheduler.Tests.Domain.Entity
{
    public class PeriodTests
    {
        [Fact]
        public void Period_To_String_Test()
        {
            var period = new Period
            {
                Days = 3,
                Months = 4,
                Years = 5
            };

            Assert.Equal("3.4.5", period.ToString());
        }
    }
}