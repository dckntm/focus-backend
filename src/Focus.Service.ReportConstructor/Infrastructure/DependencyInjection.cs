using System;
using Convey;
using Convey.Persistence.MongoDB;
using Focus.Service.ReportConstructor.Application.Common.Interface;
using Focus.Service.ReportConstructor.Infrastructure.Repository;
using Focus.Service.ReportConstructor.Infrastructure.Repository.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportConstructor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services
                .AddTransient<IReportTemplateRepository, ReportTemplateRepository>();

            return builder
                .AddMongo()
                .AddMongoRepository<ReportTemplateDocument, Guid>("report-templates");
        }
    }
}
