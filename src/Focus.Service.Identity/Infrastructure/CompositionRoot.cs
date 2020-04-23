using System.Collections.Generic;
using System.Threading.Tasks;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using Focus.Service.Identity.Infrastructure.Persistence;
using Focus.Service.Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Focus.Service.Identity.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IIdentityRepository, IdentityRepository>()
                .AddSingleton<IPasswordGenerator, PasswordGenerator>()
                .AddSingleton<ISecurityTokenGenerator, JwtSecurityTokenGenerator>();
        }

        // TODO: move 2 common infrastructure classlib
        public static void ConfigureServices(this IConfiguration configuration, IServiceCollection services)
        {
            var mongoConfig = configuration.GetMongoConfigurationFromSection("mongodb");

            services.AddTransient(_ => mongoConfig);
        }

        public static async Task SeedAdministrator(this IApplicationBuilder app, string username, string password)
        {
            var repository = app.ApplicationServices.GetRequiredService<IIdentityRepository>();

            var id = await repository.CreateNewOrganizationAsync(new Organization()
            {
                TItle = "Head Organization",
                IsHead = true,
                Members = new List<string>(new[] {
                    username
                })
            });

            await repository.CreateNewUserAsync(
                "Admin",
                "Admin",
                "Admin",
                username,
                password,
                "HOA",
                id);
        }
    }
}