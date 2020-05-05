using Focus.Application.Common.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Infrastructure.Common.Logging
{
    public static class LoggingCompositionRoot
    {
        public static IServiceCollection AddLogging(this IServiceCollection services)
            => services
                .AddTransient<ILog, Log>();
    }
}