namespace Focus.Service.ReportProcessor.Api

open Focus.Service.ReportProcessor.Infrastructure
open Focus.Service.ReportProcessor.Application
open Focus.Service.ReportProcessor.Api.Router
open Focus.Infrastructure.Common.MongoDB
open Focus.Infrastructure.Common.Messaging
open Focus.Api.Common.HelperHandlers
open Focus.Api.Common.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Logging
open Focus.Api.Common.Cors
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
            .ConfigureAppConfiguration(setup(args))
            .ConfigureServices(
                fun context services ->
                    let config = context.Configuration

                    (services |>
                        AddCors)
                        .AddGiraffe()
                        .AddMongoDB(config)
                        .AddApplication()
                        .AddRabbitMQConsumers(config)
                        .AddInfrastructure()
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    (app |> UseCors)
                        .UseAuthentication()
                        .UseGiraffeErrorHandler(errorHandler)
                        .UseGiraffe Router.webApp)
            .ConfigureLogging(Action<ILoggingBuilder> Log.ConfigureLogging)
            .Build()
            .Run()

        exitCode
