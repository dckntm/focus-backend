namespace Focus.Service.Identity.Api

open System.IO
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open System.Reflection
open Giraffe
open Focus.Service.Identity.Infrastructure
open Focus.Service.Identity.Application
open Focus.Service.Identity.Api.Router
open Focus.Api.Common
open Focus.Infrastructure.Common.MongoDB
open Focus.Api.Common.HelperHandlers
open Microsoft.Extensions.Logging
open Focus.Api.Common.Log
open System

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        WebHostBuilder()
            .UseKestrel()
            .UseDefaultServiceProvider(fun options -> options.ValidateScopes <- false)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration(
                fun context config ->
                    let env = context.HostingEnvironment
                    
                    config.AddJsonFile("appsettings.json", true, true)
                          .AddJsonFile((sprintf "appsettings.%s.json" env.EnvironmentName), true, true) |> ignore

                    if env.IsDevelopment() then
                        let appAssembly = Assembly.GetExecutingAssembly()

                        if isNotNull appAssembly then config.AddUserSecrets(appAssembly, true) |> ignore

                    config.AddEnvironmentVariables() |> ignore

                    if isNotNull args then config.AddCommandLine(args) |> ignore)
            .ConfigureServices(
                fun context services ->
                    let config = context.Configuration

                    (services |> Cors.AddCors)
                        .AddGiraffe()
                        .AddMongoDB(config)
                        .AddApplication()
                        .AddInfrastructure() 
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    (app |> Cors.UseCors)
                        .UseAuthentication()
                        .UseGiraffeErrorHandler(errorHandler)
                        .UseGiraffe Router.wepApp
                    // TODO inject admin username & password from ENV variables 
                    app.SeedAdministrator("admin", "password") |> ignore)
            .ConfigureLogging(Action<ILoggingBuilder> ConfigureLogging)
            .Build()
            .Run()

        exitCode
