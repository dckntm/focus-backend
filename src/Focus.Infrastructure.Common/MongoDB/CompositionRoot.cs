using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Infrastructure.Common.MongoDB
{
    public static class MongoDBCompositionRoot
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfiguration = new MongoConfiguration();
            configuration.Bind("mongodb", mongoConfiguration);

            return services
                .AddSingleton<IMongoConfiguration>(_ => mongoConfiguration);
        }
    }
}