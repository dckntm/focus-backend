namespace Focus.Service.ReportConstructor.Api

open Focus.Service.ReportConstructor.Infrastructure
open Focus.Service.ReportConstructor.Application
open Focus.Infrastructure.Common.Messaging
open Focus.Service.ReportConstructor.Api
open Focus.Infrastructure.Common.MongoDB
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore
open System.Reflection
open Focus.Api.Common
open System.IO
open Giraffe

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
                        .AddRabbitMQConsumers(config)
                        .AddRabbitMQPublisher(config, false)
                        .AddInfrastructure()
                        |> Jwt.AddBearerSecurity
                        |> ignore)
            .Configure(
                fun app -> 
                    (app 
                        |> Cors.UseCors
                        |> AuthAppBuilderExtensions.UseAuthentication)
                        .UseGiraffe Router.webApp
                    )
            .Build()
            .Run()
        
        exitCode
