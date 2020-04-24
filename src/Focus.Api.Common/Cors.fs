namespace Focus.Api.Common

open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open System

module Cors =

    // TODO: restrict CORS via environment variables 
    // CORS_ALLOWED_ORGIGINS

    let corsPolicy = "EnableCors"

    let corsOptions (opt : CorsOptions) =
        opt.AddPolicy(
            corsPolicy,
             (fun builder -> 
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod() |> ignore
                )
        )

    let AddCors (services : IServiceCollection) =
        services
            .AddCors(Action<CorsOptions> corsOptions)

    let UseCors (app: IApplicationBuilder) =
        app.UseCors(corsPolicy)