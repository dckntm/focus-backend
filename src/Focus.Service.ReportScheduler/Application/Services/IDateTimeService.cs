using System;

namespace Focus.Service.ReportScheduler.Application.Services
{
    public interface IDateTimeService
    {
        DateTime Now() => DateTime.Now.ToUniversalTime();

        bool IsEarlier(DateTime date1, DateTime date2) => DateTime.Compare(date1, date2) < 0;

        bool IsToday(DateTime date1) => DateTime.Compare(date1.ToUniversalTime(), Now()) == 0;
    }
}