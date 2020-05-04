namespace Focus.Service.ReportProcessor.Api.Router

open Focus.Service.ReportProcessor.Application.Commands
open Focus.Service.ReportProcessor.Application.Queries
open Focus.Service.ReportProcessor.Application.Dto
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Api.Common.AuthHandlers
open Microsoft.AspNetCore.Http
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Giraffe.Core
open MediatR
open Focus.Core.Common.Messages.Commands

module Router =

    let saveReport (dto: ReportUpdateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! _ = mediator.Send(UpdateReport(dto, false))

                return! setStatusCode 200 next ctx
            }

    let passReport (dto: ReportUpdateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! _ = mediator.Send(UpdateReport(dto, true))

                return! setStatusCode 200 next ctx
            }

    let getOrganizationReports (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganizationReports(id))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getReport (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReport(id))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let publishReports (command: PublishReports) : HttpHandler = 
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! _ = mediator.Send(command)

                return! setStatusCode 200 next ctx
            }

    let passOrgId (f: string -> HttpHandler): HttpHandler =
        fun next ctx ->
            let orgId = ctx.User.Claims |> Seq.find (fun claim -> claim.Type = "org")
            f orgId.Value next ctx

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
                          [ POST
                            >=> choose
                                    [ route "/api/report/save" >=> bindJson<ReportUpdateDto> saveReport
                                      >=> text "Successfully, saved"
                                      route "/api/report/pass" >=> bindJson<ReportUpdateDto> passReport
                                      >=> text "Successfully, passed"
                                      route "/api/cs/report/publish" >=> bindJson<PublishReports> publishReports ]
                            GET >=> choose
                                        [ route "/api/report/org" >=> passOrgId getOrganizationReports
                                          routef "/api/report/get/%s" getReport ] ]
