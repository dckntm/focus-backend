using Focus.Service.ReportScheduler.Application.Dto;
using Focus.Application.Common.Services.Messaging;
using System.Collections.Generic;
using Focus.Core.Common.Messages;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;
using System;
using Microsoft.Extensions.Logging;
using Focus.Application.Common.Services.Logging;
using Focus.Application.Common.Services.Client;
using Focus.Core.Common.Messages.Commands;

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
        // private readonly IPublisher _publisher;
        private readonly ILog _logger;
        private readonly IServiceClient _service;

        public InitializeReportConstructionHandler(
                // IPublisher publisher,
                ILog logger, IServiceClient service)
        {
            // _publisher = publisher;
            _logger = logger;
            _service = service;

            _logger.LogApplication($"Service Client is null: {_service is null}");
        }

        public async Task<Unit> Handle(InitializeReportConstruction request, CancellationToken cancellationToken)
        {
            var schedule = request.Schedule.AsEntity();

            // var onReportConstructing = new OnReportConstructing()
            // {
            //     NewReports = new List<ReportConfiguration>(new[] {
            //         new ReportConfiguration() {
            //             ReportTemplateId = schedule.ReportTemplate,
            //             AssignedOrganizationIds = schedule.Organizations
            //                 .Select(x => x.Organization)
            //                 .ToList(),
            //             Deadline = DateTime.Now.ToUniversalTime() + schedule.DeadlinePeriod
            //         }
            //     })
            // };

            var command = new ConstructReports()
            {
                ReportDescriptors = new List<ReportConstructionDescriptor>()
                {
                    new ReportConstructionDescriptor()
                    {
                        ReportTemplateId = schedule.ReportTemplate,
                        AssignedOrganizationIds = schedule.Organizations
                            .Select(x => x.Organization)
                            .ToList(),
                        DeadlineDate = DateTime.Now.ToUniversalTime() + schedule.DeadlinePeriod
                    }
                }
            };

            // _publisher.Publish(
            //     message: onReportConstructing,
            //     exchangeName: "focus",
            //     exchangeType: "topic",
            //     routeKey: "focus.events.report.construct");

            await _service.CommandAsync(command, "constructor", "api/cs/report/construct");

            _logger.LogApplication("On Report Constructing published from Report Scheduler");

            return Unit.Value;
        }
    }
}