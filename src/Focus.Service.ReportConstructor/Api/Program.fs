namespace Focus.Service.ReportConstructor.Api

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Focus.Service.ReportConstructor.Infrastructure
open Focus.Service.ReportConstructor.Application
open Giraffe
open System.Reflection
open Focus.Service.ReportScheduler.Api.ReportScheduler

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

                    services.AddGiraffe()
                    |> CompositionRoot.AddApplication
                    |> CompositionRoot.AddInfrastructure
                    |> ignore)
            .Configure(
                fun app -> app.UseGiraffe Router.webApp)
            .Build()
            .Run()
        
        exitCode
