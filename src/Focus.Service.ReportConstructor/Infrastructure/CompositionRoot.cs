using Focus.Infrastructure.Common.Interfaces;
using Focus.Infrastructure.Common.Persistence;
using Focus.Service.ReportConstructor.Application.Dto;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Entities.Table;
using Focus.Service.ReportConstructor.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Focus.Service.ReportConstructor.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<ReportTemplateDto>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

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
