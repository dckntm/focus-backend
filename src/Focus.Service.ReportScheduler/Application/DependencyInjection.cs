using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;

namespace Focus.Service.ReportScheduler.Application
{
    public static class DependencyInjection
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddQueryHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryQueryDispatcher();
    }
}