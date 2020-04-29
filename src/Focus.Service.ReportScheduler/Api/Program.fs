namespace Focus.Service.ReportScheduler.Api

open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open System.Reflection
open Giraffe.Common
open Giraffe.Middleware
open Focus.Service.ReportScheduler.Infrastructure
open Focus.Service.ReportScheduler.Application
open Focus.Service.ReportSchduler.Api.Router
open Focus.Infrastructure.Common.Messaging
open Focus.Infrastructure.Common.MongoDB
open Focus.Api.Common
open Focus.Api.Common.Cors

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
                        .AddRabbitMQPublisher(config, false)
                        .AddInfrastructure()
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    (app 
                        |> UseCors 
                        |> AuthAppBuilderExtensions.UseAuthentication) 
                        .UseGiraffe Router.webApp
            )
            .Build()
            .Run()

        exitCode
