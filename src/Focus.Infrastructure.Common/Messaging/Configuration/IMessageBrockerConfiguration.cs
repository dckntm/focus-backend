namespace Focus.Infrastructure.Common.Messaging.Configuration
{
    public interface IMessageBrokerConfiguration
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        string VirtualHost { get; set; }
        int Port { get; set; }
    }
}