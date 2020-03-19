using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Focus.Service.ReportConstructor.Application;
using Focus.Service.ReportConstructor.Application.Commands;
using Focus.Service.ReportConstructor.Application.Common.Dto;
using Focus.Service.ReportConstructor.Application.Queries;
using Focus.Service.ReportConstructor.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportConstructor.Api
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
                .Post<CreateReportTemplate>("api/report/template",
                    afterDispatch: (command, context) =>
                        context.Response.Created($"api/report/template/{command.ReportId}", command.ReportId))
                .Get<GetReportTemplateInfos, IEnumerable<ReportTemplateInfoDto>>("api/report/template/info")
                .Get<GetReportTemplate, ReportTemplateDto>("api/report/template/{reportTemplateId}"));

        }
    }
}
