using Focus.Application.Common.Services.Client;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using System;

namespace Focus.Infrastructure.Common.Client
{
    public class ServiceClient : IServiceClient
    {
        private readonly ClientConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public ServiceClient(ClientConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public async Task CommandAsync(string service, string route)
        {
            var serviceConfiguration = GetService(service);
            var uri = BuildRoute(serviceConfiguration, route);
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: uri
            );

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to successfully send content-less command to {service} to {uri}");
        }

        public async Task CommandAsync<TRequest>(TRequest body, string service, string route)
            where TRequest : class
        {
            var serviceConfiguration = GetService(service);
            var uri = BuildRoute(serviceConfiguration, route);
            var client = _clientFactory.CreateClient();
            var content = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: uri);

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(content);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send command to {service} to {uri}");
        }

        public async Task<TResponse> QueryAsync<TResponse>(string service, string route)
            where TResponse : class
        {
            var serviceConfiguration = GetService(service);
            var uri = BuildRoute(serviceConfiguration, route);
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: uri
            );

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send query to {service} to {uri}");

            var jsonResponseContent = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject(jsonResponseContent);

            return content as TResponse;
        }

        public async Task<TResponse> QueryAsync<TRequest, TResponse>(TRequest body, string service, string route)
            where TRequest : class
            where TResponse : class
        {
            var serviceConfiguration = GetService(service);
            var uri = BuildRoute(serviceConfiguration, route);
            var client = _clientFactory.CreateClient();
            var content = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: uri
            );

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = new StringContent(content);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send query to {service} to {uri}");

            var jsonResponseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject(jsonResponseContent);

            return result as TResponse;
        }

        private string BuildRoute(ServiceConfiguration service, string route)
        {
            return service.Host + route;
        }

        private ServiceConfiguration GetService(string service)
        {
            return _configuration.RequiredServices
                .FirstOrDefault(s => s.Service == service) ??
                throw new Exception($"INFRASTRUCTURE No {service} was registered for Http Client");
        }
    }
}