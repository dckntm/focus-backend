namespace Focus.Service.Identity.Api

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
open System.Reflection
open Giraffe
open Giraffe.Common
open Focus.Service.Identity.Infrastructure
open Focus.Service.Identity.Application
open Focus.Infrastructure.Common.Messaging
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open Focus.Service.Identity.Api.Router
open Focus.Api.Common.Security

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

                    config.ConfigureServices(services)

                    SecurityCompositionRoot.configureServices(services)

                    services
                        .AddGiraffe()
                        .AddApplication()
                        .AddInfrastructure(config)
                        |> ignore
                    )
            .Configure(
                fun app -> 
                    SecurityCompositionRoot
                        .configureApp(app)
                        .UseGiraffe Router.wepApp
                    app.SeedAdministrator("admin", "password") |> ignore)
            .Build()
            .Run()

        exitCode
