using System.Reflection;
using Focus.Infrastructure.Common.Messaging;
using Focus.Infrastructure.Common.Messaging.Publishing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Focus.Service.NewDayEmitter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddMediatR(Assembly.GetExecutingAssembly())
                        .AddRabbitMQPublisher(hostContext.Configuration)
                        .AddHostedService<Worker>();
                });
    }
}
