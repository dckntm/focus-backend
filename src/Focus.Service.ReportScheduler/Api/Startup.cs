using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Convey.WebApi.CQRS;
using Convey.WebApi;
using Convey;
using Focus.Service.ReportScheduler.Application;
using Focus.Service.ReportScheduler.Application.Commands;
using Focus.Service.ReportScheduler.Application.Queries;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Infrastructure;

namespace Focus.Service.ReportScheduler.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services
                .AddConvey()
                .AddWebApi()
                .AddApplication()
                .AddInfrastructure();

            builder.Build();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseDispatcherEndpoints(endpoints => endpoints
                .Post<CreateReportSchedule>("api/report/schedule",
                    afterDispatch: (command, context) =>
                        context.Response.Created($"api/report/schedule/{command.ScheduleId}", command.ScheduleId))
                .Get<GetReportScheduleInfos, IEnumerable<ReportScheduleInfoDto>>("api/report/schedule/info")
                .Get<GetReportSchedule, ReportScheduleDto>("api/report/schedule/{id}"));
        }
    }
}
