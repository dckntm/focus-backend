using System;
using System.Collections.Generic;
using System.Linq;
using Focus.Infrastructure.Common.MongoDB;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using Focus.Service.Identity.Infrastructure.Persistence;
using Focus.Service.Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Focus.Service.Identity.Infrastructure
{
    public static class CompositionRoot
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IIdentityRepository, IdentityRepository>()
                .AddSingleton<IPasswordGenerator, PasswordGenerator>()
                .AddSingleton<ISecurityTokenGenerator, JwtSecurityTokenGenerator>();
        }

        public static async void SeedAdministrator(this IApplicationBuilder app, string username, string password)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<object>>();
            var config = app.ApplicationServices.GetRequiredService<IMongoConfiguration>();

            logger.LogInformation($"database name : {config.Database}");
            var repository = app.ApplicationServices.GetRequiredService<IIdentityRepository>();

            try
            {
                var orgs = await repository.GetOrganizationsAsync();

                if (orgs.Any(o => o.IsHead))
                    return;

                var id = await repository.CreateNewOrganizationAsync(new Organization()
                {
                    Title = "Head Organization",
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}