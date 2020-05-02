namespace Focus.Service.ReportProcessor.Api

open Focus.Service.ReportProcessor.Infrastructure
open Focus.Service.ReportProcessor.Application
open Focus.Service.ReportProcessor.Api.Router
open Focus.Infrastructure.Common.Messaging
open Focus.Infrastructure.Common.MongoDB
open Focus.Infrastructure.Common.Logging
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Focus.Api.Common.Cors
open System.Reflection
open Focus.Api.Common
open System.IO
open Giraffe
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
                        .AddApplication()
                        // RabbitMQ DI always goes after Application as it needs IMediator to be injected
                        .AddRabbitMQConsumers(config)
                        .AddInfrastructure()
                        .AddLogging()
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    app.UseGiraffe Router.webApp

                    app 
                        |> AuthAppBuilderExtensions.UseAuthentication 
                        |> UseCors 
                        |> ignore   )
            .ConfigureLogging(Action<ILoggingBuilder> Log.ConfigureLogging)
            .Build()
            .Run()

        exitCode