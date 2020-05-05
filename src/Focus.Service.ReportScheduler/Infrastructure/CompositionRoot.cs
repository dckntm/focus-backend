using Focus.Service.ReportScheduler.Infrastructure.Persistence;
using Focus.Service.ReportScheduler.Application.Services;
using Focus.Service.ReportScheduler.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Focus.Service.ReportScheduler.Infrastructure.Services;

namespace Focus.Service.ReportScheduler.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
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
                .AddScoped<IReportScheduleRepository, ReportScheduleRepository>()
                .AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}
