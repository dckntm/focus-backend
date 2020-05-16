namespace Focus.Service.Identity.Api.Router

open Focus.Service.Identity.Application.Commands
open Focus.Service.Identity.Application.Queries
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Service.Identity.Application.Dto
open Focus.Service.Identity.Core.Entities
open Microsoft.AspNetCore.Http
open MediatR
open Giraffe
open Focus.Api.Common.HelperHandlers
open Focus.Api.Common.AuthHandlers

module Router =

    [<CLIMutable>]
    type User = { username: string; password: string }

    let createUserHandler (user: NewUserDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateNewUser(user))

                return! handleResult result next ctx
            }

    let loginUser (login: User): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(LoginUser(login.username, login.password))

                return! handleResult result next ctx
            }

    let createOrganization (org: Organization): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateNewOrganization(org))

                return! handleResult result next ctx
            }

    let getUser (username: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetUser(username))

                return! handleResult result next ctx
            }

    let getOrganization (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganization(id))

                return! handleResult result next ctx
            }

    let getOrganizationInfos: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganizationInfos())

                return! handleResult result next ctx
            }

    let getUsers: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetUsers())

                return! handleResult result next ctx
            }

    let getClaims: HttpHandler =
        fun next ctx -> 
            let claims = ctx.User.Claims |> Seq.map (fun claim -> (claim.Type, claim.Value, claim.ValueType, ctx.User.HasClaim("role", "HOA")))
            json claims next ctx

    let getStatistics: HttpHandler = 
        fun next ctx -> 
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetStatistics())

                return! handleResult result next ctx
            }

    let wepApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST
              >=> choose
                      [ route "/api/identity/create"
                        >=> mustBeAdmin
                        >=> bindJson<NewUserDto> createUserHandler
                        route "/api/identity/login"
                        >=> bindJson<User> loginUser
                        route "/api/org/create"
                        >=> mustBeAdmin
                        >=> bindJson<Organization> createOrganization ]
              GET
              >=> mustBeAdmin
              >=> choose
                      [ route "/api/identity/claims"
                        >=> getClaims
                        route "/api/identity/stats"
                        >=> getStatistics
                        route "/api/org/info" >=> getOrganizationInfos
                        route "/api/identity/info" >=> getUsers
                        routef "/api/identity/%s" getUser
                        routef "/api/org/%s" getOrganization ]
              setStatusCode 404 >=> text "Not Found" ]
