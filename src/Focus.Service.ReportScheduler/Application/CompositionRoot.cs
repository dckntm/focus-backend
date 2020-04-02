using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Focus.Service.ReportScheduler.Application
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
