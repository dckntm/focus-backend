using System;
using Focus.Infrastructure.Common.Interfaces;
using Focus.Infrastructure.Common.Persistence;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportConstructor.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IReportTemplateRepository, ReportTemplateRepository>();
        }

        public static void ConfigureServices(this IConfiguration configuration, IServiceCollection services)
        {
            var mongoConfig = configuration.GetMongoConfigurationFromSection("mongodb");

            services.AddTransient<IMongoConfiguration>(_ => mongoConfig);
        }
    }
}
