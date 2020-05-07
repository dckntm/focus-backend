using System.Collections.Generic;
using Focus.Application.Common.Abstract;
using Focus.Core.Common.Entities.Template;
using MediatR;

namespace Focus.Application.Common.Messages.Commands
{
    public class PublishReports : IRequest<Result>
    {
        public IList<ReportPublishDescriptor> ReportDescriptors { get; set; }
    }

    public class ReportPublishDescriptor
    {
        public ReportConstructionDescriptor ConstructionDescriptor { get; set; }
        public ReportTemplateDto Template { get; set; }

    }
}