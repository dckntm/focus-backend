namespace Focus.Service.ReportScheduler.Api

open Focus.Service.ReportScheduler.Infrastructure
open Focus.Service.ReportScheduler.Application
open Focus.Service.ReportSchduler.Api.Router
open Focus.Infrastructure.Common.Messaging
open Focus.Infrastructure.Common.MongoDB
open Focus.Infrastructure.Common.Client
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Focus.Api.Common.Cors
open Focus.Api.Common.Log
open Giraffe.Middleware
open System.Reflection
open Focus.Api.Common
open Giraffe.Common
open System.IO
open System

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        WebHostBuilder()
            .UseKestrel()
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

                    (services |>
                        AddCors)
                        .AddGiraffe()
                        .AddMongoDB(config)
                        .AddServiceClient(config)
                        .AddApplication()
                        // RabbitMQ DI always goes after Application as it needs IMediator to be injected
                        .AddRabbitMQConsumers(config)
                        .AddInfrastructure()
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    (app 
                        |> UseCors 
                        |> AuthAppBuilderExtensions.UseAuthentication) 
                        .UseGiraffe Router.webApp)
            .ConfigureLogging(Action<ILoggingBuilder> ConfigureLogging)
            .Build()
            .Run()

        exitCode
