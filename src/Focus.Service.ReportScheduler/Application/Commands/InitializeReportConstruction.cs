using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Application.Common.Services.Messaging;
using System.Collections.Generic;
using Focus.Core.Common.Messages;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;
using System;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    public class InitializeReportConstruction : IRequest
    {
        public InitializeReportConstruction(ReportScheduleDto schedule)
        {
            Schedule = schedule;
        }
        public ReportScheduleDto Schedule { get; private set; }

    }

    public class InitializeReportConstructionHandler
        : IRequestHandler<InitializeReportConstruction>
    {
        private readonly IPublisher _publisher;

        public InitializeReportConstructionHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task<Unit> Handle(InitializeReportConstruction request, CancellationToken cancellationToken)
        {
            var schedule = request.Schedule.AsEntity();

            var onReportConstructing = new OnReportConstructing()
            {
                NewReports = new List<ReportConfiguration>(new[] {
                    new ReportConfiguration() {
                        ReportTemplateId = schedule.ReportTemplate,
                        AssignedOrganizationIds = schedule.Organizations
                            .Select(x => x.Organization)
                            .ToList(),
                        Deadline = DateTime.Now.ToUniversalTime() + schedule.DeadlinePeriod
                    }
                })
            };

            _publisher.Publish(
                message: onReportConstructing,
                exchangeName: "focus",
                exchangeType: "topic",
                routeKey: "focus.report.constructing");

            return Unit.Task;
        }
    }
}