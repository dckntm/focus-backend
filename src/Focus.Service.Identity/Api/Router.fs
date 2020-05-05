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

    let wepApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST
              >=> choose
                      [ route "/api/identity/create"
                        >=> bindJson<NewUserDto> createUserHandler
                        route "/api/identity/login"
                        >=> bindJson<User> loginUser
                        route "/api/org/create"
                        >=> bindJson<Organization> createOrganization ]
              GET
              >=> choose
                      [ route "/api/org/info" >=> getOrganizationInfos
                        route "/api/identity/info" >=> getUsers
                        routef "/api/identity/%s" getUser
                        routef "/api/org/%s" getOrganization ]
              setStatusCode 404 >=> text "Not Found" ]
