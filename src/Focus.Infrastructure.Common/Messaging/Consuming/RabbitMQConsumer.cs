using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.Messaging.Configuration;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Focus.Infrastructure.Common.Messaging.Consuming
{
    public class RabbitMQConsumer<T> : BackgroundService where T : INotification
    {
        private readonly IMediator _mediator;
        private readonly RabbitMQConsumerConfiguration _config;
        private readonly IMessageBrokerConfiguration _connectionConfig;
        private readonly IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(
            IMessageBrokerConfiguration connectionConfig,
            RabbitMQConsumerConfiguration config,
            IMediator mediator)
        {
            _config = config;
            _connectionConfig = connectionConfig;

            var factory = new ConnectionFactory()
            {
                HostName = _connectionConfig.Host,
                UserName = _connectionConfig.UserName,
                Password = _connectionConfig.Password,
                VirtualHost = _connectionConfig.VirtualHost,
                Port = _connectionConfig.Port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            ConfigureChannel();

            _mediator = mediator;
        }

        private void ConfigureChannel()
        {
            _channel.ExchangeDeclare(
                exchange: _config.ExchangeName,
                type: _config.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: _config.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(
                queue: _config.QueueName,
                exchange: _config.ExchangeName,
                routingKey: _config.RoutingKey,
                arguments: null);
            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (_channel is null || _channel.IsClosed)
            {
                _channel = _connection.CreateModel();
                ConfigureChannel();
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (s, e) =>
            {
                var content = Encoding.UTF8.GetString(e.Body.Span);

                var notification = JsonConvert.DeserializeObject(content, typeof(T));

                if (notification is null)
                    throw new Exception($"Can't convert json to object {notification}");

                _mediator.Publish(notification, stoppingToken);

                _channel.BasicAck(e.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: _config.QueueName,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }
    }
}