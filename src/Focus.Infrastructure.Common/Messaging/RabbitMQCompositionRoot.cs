using System;
using System.Collections.Generic;
using Focus.Application.Common.Services.Messaging;
using Focus.Infrastructure.Common.Messaging.Configuration;
using Focus.Infrastructure.Common.Messaging.Consuming;
using Focus.Infrastructure.Common.Messaging.Publishing;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Focus.Infrastructure.Common.Messaging
{
    public static class RabbitMQCompositionRoot
    {
        public static IServiceCollection AddRabbitMQPublisher(
            this IServiceCollection services,
            IConfiguration configuration,
            bool requiresConnectionSetup = true)
        {
            if (requiresConnectionSetup)
                services.AddRabbitMQConnection(configuration);

            return services
                .AddSingleton<IPublisher, RabbitMQPublisher>();
        }

        public static IServiceCollection AddRabbitMQConsumers(
            this IServiceCollection services,
            IConfiguration configuration,
            bool requiresConnectionSetup = true)
        {
            if (requiresConnectionSetup)
                services.AddRabbitMQConnection(configuration);

            var consumers = new List<RabbitMQConsumerConfiguration>();
            configuration.Bind("rabbitmq_consumers", consumers);

            foreach (var consumer in consumers)
            {
                var type = Type.GetType(consumer.ConsumedType);

                services.AddHostedService(provider =>
                {
                    var connectionConfig = provider.GetService<IMessageBrokerConfiguration>();
                    var mediator = provider.GetService<IMediator>();

                    var nonGenericConsumerType = typeof(RabbitMQConsumer<>);
                    var genericConsumerType = nonGenericConsumerType
                        .MakeGenericType(type);

                    return (BackgroundService)Activator.CreateInstance(
                        genericConsumerType,
                        connectionConfig,
                        consumer,
                        mediator);
                });
            }

            return services;
        }

        public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionConfig = new RabbitMQConfiguration();

            configuration.Bind("rabbitmq_connection", connectionConfig);

            return services
                .AddSingleton<IMessageBrokerConfiguration, RabbitMQConfiguration>(provider => connectionConfig);
        }
    }
}