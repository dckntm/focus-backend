namespace Focus.Service.ReportProcessor.Api

open Focus.Service.ReportProcessor.Infrastructure
open Focus.Service.ReportProcessor.Application
open Focus.Service.ReportProcessor.Api.Router
open Focus.Infrastructure.Common.MongoDB
open Focus.Infrastructure.Common.Client
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
open Focus.Api.Common.HelperHandlers
open Focus.Api.Common.Configuration

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
                        .AddInfrastructure()
                        |> Jwt.AddBearerSecurity 
                        |> ignore)
            .Configure(
                fun app -> 
                    (app |> Cors.UseCors)
                        .UseAuthentication()
                        .UseGiraffeErrorHandler(errorHandler)
                        .UseGiraffe Router.webApp)
            .ConfigureLogging(Action<ILoggingBuilder> Log.ConfigureLogging)
            .Build()
            .Run()

        exitCode
