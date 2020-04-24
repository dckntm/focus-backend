using Focus.Service.ReportConstructor.Infrastructure.Persistence;
using Focus.Service.ReportConstructor.Application.Services;
using Focus.Service.ReportConstructor.Application.Dto;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

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
    }
}
