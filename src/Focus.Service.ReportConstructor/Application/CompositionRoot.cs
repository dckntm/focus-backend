using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.ReportConstructor.Application
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // TODO: replace with application layer assembly

            return services
                .AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
