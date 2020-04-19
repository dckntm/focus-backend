using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.ReportScheduler.Infrastructure.Persistence;
using Focus.Service.ReportScheduler.Application.Services;
using MongoDB.Bson.Serialization;
using Focus.Service.ReportScheduler.Core.Entities;

namespace Focus.Service.ReportScheduler.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap<MemberAssignment>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(x => new MemberAssignment(x.User, x.Role));
            });

            BsonClassMap.RegisterClassMap<Assignment>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(x => new Assignment(
                    x.Organization,
                    x.IsDelegatedToCOA,
                    x.Assignees));
            });

            return services
                .AddScoped<IReportScheduleRepository, ReportScheduleRepository>();
        }

        public static void ConfigureServices(this IConfiguration configuration, IServiceCollection services)
        {
            var mongoConfig = configuration.GetMongoConfigurationFromSection("mongodb");

            services.AddTransient(_ => mongoConfig);
        }
    }
}
