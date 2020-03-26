using System;
using Convey;
using Convey.Persistence.MongoDB;
using Focus.Service.ReportScheduler.Application.Common.Interface;
using Focus.Service.ReportScheduler.Infrastructure.Repository;
using Focus.Service.ReportScheduler.Infrastructure.Repository.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportScheduler.Infrastructure
{
    public static class DependencyInjection
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services
                .AddTransient<IReportScheduleRepository, ReportScheduleRepository>();

            return builder
                .AddMongo()
                .AddMongoRepository<ReportScheduleDocument, Guid>("report-templates");
        }
    }
}
