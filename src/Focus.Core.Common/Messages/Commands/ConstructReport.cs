using System;
using System.Collections.Generic;
using MediatR;

namespace Focus.Core.Common.Messages.Commands
{
    public class ConstructReports : IRequest
    {
        public IList<ReportConstructionDescriptor> ReportDescriptors { get; set; }
    }

    public class ReportConstructionDescriptor
    {
        public string ReportTemplateId { get; set; }
        public IList<string> AssignedOrganizationIds { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}