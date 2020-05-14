using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Focus.Service.Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private IList<ServiceConfiguration> _microservices;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CORS");

            var _microservices = new List<ServiceConfiguration>();
            _configuration.Bind("services", _microservices);

            Console.WriteLine($"Microservices are null? {_microservices is null}");

            var client = new HttpClient();

            app.Run(async (ctx) =>
            {
                var request = ctx.Request;
                Console.WriteLine($"\nMethod: {request.Method}\nPath: {request.Path}\nPathBase: {request.PathBase}\nHost: {request.Host}\nContentType: {request.ContentType}\n");

                if (request.Method == "OPTIONS")
                {
                    ctx.Response.StatusCode = 200;
                    return;
                }

                var service = _microservices
                    .FirstOrDefault(
                        s => s.Routes
                            .Any(r =>
                                request.Path.Value.StartsWith(r.Route) &&
                                r.Method == request.Method));

                if (service is null)
                {
                    Console.WriteLine("\nService is null => gateway not found error");

                    ctx.Response.StatusCode = 404;
                    await ctx.Response.WriteAsync("Not found");
                    return;
                }


                HttpRequestMessageFeature feature = new HttpRequestMessageFeature(ctx);
                var message = feature.HttpRequestMessage;

                message.RequestUri = new Uri(service.Base + request.Path);
                // var message = await CreateMessage(service, request);

                var messageDescription = $"\nURI : {message.RequestUri}\nMethod: {message.Method}\n";

                foreach (var header in message.Headers)
                {
                    messageDescription += $"{header.Key}: ";

                    foreach (var value in header.Value)
                        messageDescription += $"{value} ";

                    messageDescription += "\n";
                }

                Console.WriteLine(messageDescription);

                try
                {
                    var response = await client.SendAsync(message);

                    Console.WriteLine($"\n{service.Service} responded with {(int)response.StatusCode} status code");
                    
                    ctx.Response.StatusCode = (int)response.StatusCode;
                    await ctx.Response.WriteAsync(await response.Content.ReadAsStringAsync());
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred");
                    var body = $"Message: {e.Message}\nStackTrace: {e.StackTrace}";

                    ctx.Response.StatusCode = 500;
                    await ctx.Response.WriteAsync(body);
                    return;
                }
            });
        }
    }
}
