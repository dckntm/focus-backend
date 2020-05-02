using System.Threading.Tasks;

namespace Focus.Application.Common.Services.Client
{
    // POST method for commands
    // GET method for queries
    // Content is always send as application/json
    public interface IServiceClient
    {
        Task CommandAsync(string service, string route);
        Task CommandAsync<TRequest>(TRequest body, string service, string route)
            where TRequest : class;
        Task<TResponse> QueryAsync<TResponse>(string service, string route)
            where TResponse : class;
        Task<TResponse> QueryAsync<TRequest, TResponse>(TRequest body, string service, string route)
            where TRequest : class
            where TResponse : class;
    }
}