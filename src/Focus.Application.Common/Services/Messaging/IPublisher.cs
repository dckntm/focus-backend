namespace Focus.Application.Common.Services.Messaging
{
    public interface IPublisher
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
    }
}