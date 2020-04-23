using Microsoft.Extensions.Configuration;

namespace Focus.Infrastructure.Common.MongoDB
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}