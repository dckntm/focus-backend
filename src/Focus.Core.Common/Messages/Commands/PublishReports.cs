using System.Collections.Generic;
using Focus.Core.Common.Entities.Template;
using MediatR;

namespace Focus.Core.Common.Messages.Commands
{
    public class PublishReports : IRequest
    {
        public IList<ReportPublishDescriptor> ReportDescriptors { get; set; }
    }

    public class ReportPublishDescriptor
    {
        public ReportConstructionDescriptor ConstructionDescriptor { get; set; }
        public ReportTemplate Template { get; set; }

    }
}