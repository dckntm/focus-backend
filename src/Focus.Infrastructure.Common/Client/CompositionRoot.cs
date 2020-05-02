using Focus.Application.Common.Services.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Infrastructure.Common.Client
{
    public static class ServiceClientCompositionRoot
    {
        public static IServiceCollection AddServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var clientConfiguration = new ClientConfiguration();
            configuration.Bind("serviceClient", clientConfiguration);

            return services
                .AddHttpClient()
                .AddSingleton(clientConfiguration);
        }
    }
}