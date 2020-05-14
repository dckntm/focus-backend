namespace Focus.Api.Common

open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Authentication
open Microsoft.IdentityModel.Tokens
open System.Text
open System

module Jwt =

    // In order to make Bearer Auth work we should also call app.UseAuthentication()

    // TODO: moke all sensitive parameters injectable via environment variables
    // FOCUS_VALID_ISSUER
    // FOCUS_VALID_AUDIENCE
    // FOCUS_KEY
    // FOCUS_VALIDATE_ISSUER
    // FOCUS_VALIDATE_AUDIENCE
    // FOCUS_VALIDATE_KEY

    let authenticationOptions (o : AuthenticationOptions) =
        o.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
        o.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
        o.DefaultScheme <- JwtBearerDefaults.AuthenticationScheme

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

    let AddBearerSecurity (services : IServiceCollection) =
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Action<JwtBearerOptions> jwtBearerOptions)