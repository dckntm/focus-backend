namespace Focus.Infrastructure.Common.Messaging.Configuration
{
    public class RabbitMQConfiguration : IMessageBrokerConfiguration
    {
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Host { get; set; }
        public string VirtualHost { get; set; } = "/";
        public int Port { get; set; } = 5672;
    }
}