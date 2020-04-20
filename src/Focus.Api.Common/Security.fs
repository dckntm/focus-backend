namespace Focus.Api.Common.Security

open Giraffe
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Http
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open System
open Microsoft.IdentityModel.Tokens
open System.Text
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection

module Security =

    let accessDenied: HttpFunc -> HttpContext -> HttpFuncResult =
        setStatusCode 401 >=> text "Secutiry: this user can't access service"

    let authorize: HttpFunc -> HttpContext -> HttpFuncResult =
        requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

    let mustBeAdmin: HttpFunc -> HttpContext -> HttpFuncResult =
        authorize >=> authorizeUser (fun u -> u.HasClaim(ClaimTypes.Role, "admin")) accessDenied

module Token =

    let generate (role: string) =
        let claims = [ Claim(ClaimTypes.Role, role) ]

        let expires = Nullable()
        let notBefore = Nullable()
        let securityKey =
            SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("x^w7a$NX?*3b'%V1v)YiQr?)4-*jliNTE2R?<JCXDmw}*'QJATR?@4oe{!n=kc%"))
        let signingCredentials = SigningCredentials(key = securityKey, algorithm = SecurityAlgorithms.HmacSha256)

        let token =
            JwtSecurityToken
                (issuer = "focus_issuer", audience = "focus_audience", claims = claims, expires = expires,
                 notBefore = notBefore, signingCredentials = signingCredentials)

        JwtSecurityTokenHandler().WriteToken(token)

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
            IssuerSigningKey = SymmetricSecurityKey (Encoding.UTF8.GetBytes("x^w7a$NX?*3b'%V1v)YiQr?)4-*jliNTE2R?<JCXDmw}*'QJATR?@4oe{!n=kc%"))
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
