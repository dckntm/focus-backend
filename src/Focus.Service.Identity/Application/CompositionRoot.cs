using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.Identity.Application
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(CompositionRoot).Assembly);
        }
    }
}