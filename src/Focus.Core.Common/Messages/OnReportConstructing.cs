using System.Collections.Generic;
using MediatR;
using System;

namespace Focus.Core.Common.Messages
{
    public class OnReportConstructing : INotification
    {
        public IList<ReportConfiguration> NewReports { get; set; }
    }

    public class ReportConfiguration
    {
        public string ReportTemplateId { get; set; }
        public IList<string> AssignedOrganizationIds { get; set; }
        public DateTime Deadline { get; set; }
    }
}