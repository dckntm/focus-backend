using System;
using System.Text;
using Focus.Application.Common.Services.Messaging;
using Focus.Infrastructure.Common.Messaging.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Focus.Infrastructure.Common.Messaging.Publishing
{
    public class RabbitMQPublisher : IPublisher
    {
        private readonly IMessageBrokerConfiguration _connectionConfiguration;
        private readonly IConnection _connection;
        private IModel _channel;
        public RabbitMQPublisher(IMessageBrokerConfiguration connectionConfiguration)
        {
            _connectionConfiguration = connectionConfiguration;

            var factory = new ConnectionFactory()
            {
                HostName = _connectionConfiguration.Host,
                UserName = _connectionConfiguration.UserName,
                Password = _connectionConfiguration.Password,
                Port = _connectionConfiguration.Port,
                VirtualHost = _connectionConfiguration.VirtualHost
            };

            _connection = factory.CreateConnection();
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string routingKey) where T : class
        {
            if (message is null) return;

            if (_channel is null || _channel.IsClosed)
            {
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(
                    exchange: exchangeName,
                    type: exchangeType,
                    durable: true,
                    autoDelete: false,
                    arguments: null);
            }

            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var props = _channel.CreateBasicProperties();
                props.Persistent = true;

                _channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: props,
                    body: body);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}