using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Focus.Application.Common.Services.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Focus.Infrastructure.Common.Client
{
    public static class ServiceClientCompositionRoot
    {
        public static IServiceCollection AddServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var token = GenerateToken();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);

            var clientConfiguration = new ClientConfiguration();
            configuration.Bind("service_client", clientConfiguration);

            foreach (var service in clientConfiguration.RequiredServices)
            {
                services.AddHttpClient(service.Service, client =>
                {
                    client.BaseAddress = new Uri(service.Host);
                    client.DefaultRequestHeaders.Authorization = authHeader;
                });
            }

            return services.AddTransient<IServiceClient, ServiceClient>();
        }

        private static string GenerateToken()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Role, "service"),
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("Amr273YaMvDu4X5WEvG2jmwsdaJY3ADRT6hFeZvXHhMD7nt6Bd"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "focus_issuer",
                audience: "focus_audience",
                claims: claims,
                signingCredentials: signingCredentials
            );

            var s_token = new JwtSecurityTokenHandler().WriteToken(token);

            Console.WriteLine(s_token); 

            return s_token;
        }
    }
}