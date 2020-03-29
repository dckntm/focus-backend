using System;

namespace Focus.Service.ReportScheduler.Infrastructure.Exceptions
{
    public class ReportScheduleDocumentNotFoundException : Exception
    {
        public string Id { get; }

        public ReportScheduleDocumentNotFoundException(string id)
        {
            Id = id;
        }
    }
}