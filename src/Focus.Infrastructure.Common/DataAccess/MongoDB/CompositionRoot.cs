using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Infrastructure.Common.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataAccessCompositionRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDBConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfiguration = new MongoConfiguration();
            configuration.Bind("mongodb", mongoConfiguration);

            return services
                .AddSingleton<IMongoConfiguration>(_ => mongoConfiguration);
        }
    }
}