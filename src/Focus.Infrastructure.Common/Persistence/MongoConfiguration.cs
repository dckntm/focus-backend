using Focus.Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Focus.Infrastructure.Common.Persistence
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public static class MongoConfigurationExtensions
    {
        public static IMongoConfiguration GetMongoConfigurationFromSection(this IConfiguration config, string section)
        {
            var mongoConfig = new MongoConfiguration();

            config.Bind(section, mongoConfig);

            return mongoConfig;
        }
    }
}