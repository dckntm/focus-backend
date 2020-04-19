namespace Focus.Service.ReportScheduler.Api

open System.IO
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open System.Reflection
open Giraffe.Common
open Giraffe
open Focus.Service.ReportScheduler.Infrastructure
open Focus.Service.ReportScheduler.Application
open Focus.Service.ReportSchduler.Api.Router
open Focus.Infrastructure.Common.Messaging

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

                    config.ConfigureServices(services) |> ignore

                    services
                        .AddGiraffe()
                        .AddApplication()
                        .AddInfrastructure(config)
                        .AddRabbitMQConsumers(config)
                        |> ignore
                    
                    printfn "%s" (config.GetSection("rabbitmq_connection").Value)
                        )
            .Configure(
                fun app -> 
                    app.UseGiraffe Router.webApp
            )
            .Build()
            .Run()

        exitCode
