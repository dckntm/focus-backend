namespace Focus.Service.ReportConstructor.Api

open Focus.Service.ReportConstructor.Infrastructure
open Focus.Service.ReportConstructor.Application
open Focus.Infrastructure.Common.Messaging
open Focus.Service.ReportConstructor.Api
open Focus.Infrastructure.Common.MongoDB
open Focus.Infrastructure.Common.Logging
open Focus.Infrastructure.Common.Client
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Focus.Api.Common.Log
open Microsoft.AspNetCore
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

                    (services |> Cors.AddCors)
                        .AddGiraffe()
                        .AddMongoDB(config)
                        .AddApplication()
                        .AddServiceClient(config)
                        // .AddRabbitMQConsumers(config)
                        // .AddRabbitMQPublisher(config, false)
                        .AddInfrastructure()
                        .AddLogging()
                        |> Jwt.AddBearerSecurity
                        |> ignore)
            .Configure(
                fun app -> 
                    (app 
                        |> Cors.UseCors
                        |> AuthAppBuilderExtensions.UseAuthentication)
                        .UseGiraffe Router.webApp
                    )
            .ConfigureLogging(Action<ILoggingBuilder> ConfigureLogging)
            .Build()
            .Run()
        
        exitCode
