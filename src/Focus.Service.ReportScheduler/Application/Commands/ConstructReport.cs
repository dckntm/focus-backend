using Focus.Service.ReportScheduler.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;
using System;
using Focus.Application.Common.Services.Client;
using Focus.Core.Common.Messages.Commands;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    public class ConstructReport : IRequest
    {
        public ConstructReport(ReportScheduleDto schedule)
        {
            Schedule = schedule;
        }
        public ReportScheduleDto Schedule { get; private set; }
    }

    public class ConstructReportHandler
        : IRequestHandler<ConstructReport>
    {
        private readonly IServiceClient _service;

        public ConstructReportHandler(
                IServiceClient service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(ConstructReport request, CancellationToken cancellationToken)
        {
            var schedule = request.Schedule.AsEntity();

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

            await _service.CommandAsync(command, "constructor", "api/cs/report/construct");

            return Unit.Value;
        }
    }
}