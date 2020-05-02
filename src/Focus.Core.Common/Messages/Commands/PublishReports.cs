using System.Collections.Generic;
using Focus.Core.Common.Entities.Template;
using MediatR;

namespace Focus.Core.Common.Messages.Commands
{
    public class PublishReports : IRequest
    {
        public IList<ReportPublishDescriptor> ReportPublishConfigurations { get; set; }
    }

    public class ReportPublishDescriptor
    {
        public ReportConstructionDescriptor ConstructionConfiguration { get; set; }
        public ReportTemplate Template { get; set; }

    }
}