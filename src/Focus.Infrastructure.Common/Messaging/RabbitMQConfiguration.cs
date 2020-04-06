namespace Focus.Infrastructure.Common.Messaging
{
    public class RabbitMQConfiguration
    {
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Localhost { get; set; }
        public string VirtualHost { get; set; } = "/";
        public int Port { get; set; } = 5672;
    }
}