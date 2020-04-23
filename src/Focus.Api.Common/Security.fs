namespace Focus.Api.Common.Security

open Giraffe
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Http
open System.Security.Claims
open System
open Microsoft.IdentityModel.Tokens
open System.Text
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection

module Security =

    let accessDenied: HttpFunc -> HttpContext -> HttpFuncResult =
        setStatusCode 401 >=> text "Security: this user can't access service"

    let authorize: HttpFunc -> HttpContext -> HttpFuncResult =
        requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

    let mustBeAdmin: HttpFunc -> HttpContext -> HttpFuncResult =
        authorize >=> authorizeUser (fun u -> u.HasClaim(ClaimTypes.Role, "HOA")) accessDenied

module SecurityCompositionRoot =

    let authenticationOptions (o : AuthenticationOptions) =
        o.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
        o.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
        o.DefaultScheme <- JwtBearerDefaults.AuthenticationScheme

    let configureApp (app : IApplicationBuilder) =
        app.UseAuthentication()

    let jwtBearerOptions (opt : JwtBearerOptions) =
        opt.TokenValidationParameters <- TokenValidationParameters (
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidAudience = "focus_audience",
            ValidIssuer = "focus_issuer",
            IssuerSigningKey = SymmetricSecurityKey (Encoding.UTF8.GetBytes("Amr273YaMvDu4X5WEvG2jmwsdaJY3ADRT6hFeZvXHhMD7nt6Bd"))
        )

    let corsOptions (opt : CorsOptions) =
        opt.AddPolicy(
            "EnableCors",
             (fun builder -> 
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod() |> ignore
                )
        )

    let configureServices (services : IServiceCollection) =
        services
            .AddCors(Action<CorsOptions> corsOptions)
            .AddAuthentication(authenticationOptions)
            .AddJwtBearer(Action<JwtBearerOptions> jwtBearerOptions) |> ignore
