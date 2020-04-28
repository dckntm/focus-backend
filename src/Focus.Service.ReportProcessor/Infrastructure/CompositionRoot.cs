using Focus.Service.ReportProcessor.Infrastructure.Persistence;
using Focus.Service.ReportProcessor.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportProcessor.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddSingleton<IReportRepository, ReportRepository>();
        }
    }
}
