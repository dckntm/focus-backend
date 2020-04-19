namespace Focus.Infrastructure.Common.Messaging.Configuration
{
    public class RabbitMQConsumerConfiguration
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string Hostname { get; set; }
        public string RoutingKey { get; set; }
        public string ConsumedType { get; set; }
        public string ExchangeType { get; set; }
    }
}