namespace Focus.Service.Identity.Api.Router

open Focus.Service.Identity.Application.Commands
open Focus.Service.Identity.Application.Queries
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Service.Identity.Application.Dto
open Focus.Service.Identity.Core.Entities
open Focus.Api.Common.AuthHandlers
open Microsoft.AspNetCore.Http
open MediatR
open Giraffe

module Router =

    [<CLIMutable>]
    type User =
        { username: string
          password: string }

    let createUserHandler: HttpHandler =
        fun next ctx ->
            task {
                let! user = ctx.BindJsonAsync<NewUserDto>()

                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateNewUser(user))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let loginUser (login: User): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(LoginUser(login.username, login.password))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let createOrganization (org: Organization): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateNewOrganization(org))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getUser (username: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetUser(username))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getOrganization (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganization(id))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getOrganizationInfos: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganizationInfos())

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getUsers: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetUsers())

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let wepApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST
              >=> choose
                      [ route "/api/identity/create" >=> mustBeAdmin >=> createUserHandler
                        route "/api/identity/login" >=> bindJson<User> loginUser
                        route "/api/org/create" >=> mustBeAdmin >=> bindJson<Organization> createOrganization ]
              GET >=> mustBeAdmin >=> choose
                                                   [ route "/api/org/info" >=> getOrganizationInfos
                                                     route "/api/identity/info" >=> getUsers
                                                     routef "/api/identity/%s" getUser
                                                     routef "/api/org/%s" getOrganization ]
              setStatusCode 404 >=> text "Not Found" ]
