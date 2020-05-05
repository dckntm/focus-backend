using Focus.Service.ReportScheduler.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;
using System;
using Focus.Application.Common.Services.Client;
using Focus.Core.Common.Messages.Commands;
using Focus.Application.Common.Abstract;

namespace Focus.Service.ReportScheduler.Application.Commands
{
    public class ConstructReport : IRequest<Result>
    {
        public ConstructReport(ReportScheduleDto schedule)
        {
            Schedule = schedule;
        }
        public ReportScheduleDto Schedule { get; private set; }
    }

    public class ConstructReportHandler
        : IRequestHandler<ConstructReport, Result>
    {
        private readonly IServiceClient _service;

        public ConstructReportHandler(
                IServiceClient service)
        {
            _service = service;
        }

        public async Task<Result> Handle(ConstructReport request, CancellationToken cancellationToken)
        {
            try
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

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Fail();

            }
        }
    }
}