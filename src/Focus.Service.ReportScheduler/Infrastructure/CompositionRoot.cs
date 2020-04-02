using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Focus.Infrastructure.Common.Interfaces;
using Focus.Infrastructure.Common.Persistence;
using Focus.Service.ReportScheduler.Infrastructure.Persistence;
using Focus.Service.ReportScheduler.Application.Services;

namespace Focus.Service.ReportScheduler.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IReportScheduleRepository, ReportScheduleRepository>();
        }

        public static void ConfigureServices(this IConfiguration configuration, IServiceCollection services)
        {
            var mongoConfig = configuration.GetMongoConfigurationFromSection("mongodb");

            services.AddTransient<IMongoConfiguration>(_ => mongoConfig);
        }
    }
}
