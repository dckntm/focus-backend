using Focus.Application.Common.Services.Client;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System;

namespace Focus.Infrastructure.Common.Client
{
    public class ServiceClient : IServiceClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<IServiceClient> _logger;

        public ServiceClient(IHttpClientFactory clientFactory, ILogger<IServiceClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task CommandAsync(string service, string route)
        {
            var client = _clientFactory.CreateClient(service);

            var request = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: route
            );

            // _logger.LogInformation($"Requesting command to AbsolutePath: {request.RequestUri}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to successfully send content-less command to {service} to {route}\nInner Exception Message: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task CommandAsync<TRequest>(TRequest body, string service, string route)
            where TRequest : class
        {
            var client = _clientFactory.CreateClient(service);
            var content = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: route)
            {
                Content = new StringContent(content)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // _logger.LogInformation($"Requesting command to AbsolutePath: {request.RequestUri}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send command to {service} to {route}\nInner Exception Message: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<TResponse> QueryAsync<TResponse>(string service, string route)
            where TResponse : class
        {
            var client = _clientFactory.CreateClient(service);

            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: route
            );

            // _logger.LogInformation($"Requesting command to AbsolutePath: {request.RequestUri}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send query to {service} to {route}\nInner Exception Message: {await response.Content.ReadAsStringAsync()}");

            var jsonResponseContent = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject(jsonResponseContent);

            return content as TResponse;
        }

        public async Task<TResponse> QueryAsync<TRequest, TResponse>(TRequest body, string service, string route)
            where TRequest : class
            where TResponse : class
        {
            var client = _clientFactory.CreateClient();
            var content = JsonConvert.SerializeObject(body);

            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: route
            )
            {
                Content = new StringContent(content)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // _logger.LogInformation($"Requesting command to AbsolutePath: {request.RequestUri}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"INFRASTRUCTURE Failed to send query to {service} to {route}\nInner Exception Message: {await response.Content.ReadAsStringAsync()}");

            var jsonResponseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject(jsonResponseContent);

            return result as TResponse;
        }
    }
}