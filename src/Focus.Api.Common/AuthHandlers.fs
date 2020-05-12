namespace Focus.Api.Common

open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Http
open System.Security.Claims
open Giraffe

module AuthHandlers =

    let accessDenied: HttpFunc -> HttpContext -> HttpFuncResult =
        setStatusCode 401 >=> text "Security: this user can't access service"

    let authorize: HttpFunc -> HttpContext -> HttpFuncResult =
        requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

    let mustBeAdmin: HttpFunc -> HttpContext -> HttpFuncResult =
        authorize >=> authorizeUser (fun u -> u.HasClaim("role", "HOA")) accessDenied