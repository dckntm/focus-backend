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
open Focus.Application.Common.Messages.Commands
open Focus.Api.Common.HelperHandlers

module Router =

    let saveReport (dto: ReportUpdateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(UpdateReport(dto, false))

                return! handleResult result next ctx
            }

    let passReport (dto: ReportUpdateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(UpdateReport(dto, true))

                return! handleResult result next ctx
            }

    let getOrganizationReports (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetOrganizationReports(id))

                return! handleResult result next ctx
            }

    let getReport (id: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReport(id))

                return! handleResult result next ctx
            }

    let publishReports (command: PublishReports): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(command)

                return! handleResult result next ctx
            }

    let getActualReports: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetActualReports())

                return! handleResult result next ctx
            }

    let passOrgId (f: string -> HttpHandler): HttpHandler =
        fun next ctx ->
            let orgId =
                ctx.User.Claims
                |> Seq.find (fun claim -> claim.Type = "org")

            f orgId.Value next ctx

    let getStatistics: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetStatistics())

                return! handleResult result next ctx
            }

    let getQueriedReportsHandler(query:GetQueriedReports) : HttpHandler = 
        fun next ctx -> 
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(query)

                return! handleResult result next ctx
            }

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST
              >=> choose
                      [ route "/api/report/save"
                        >=> authorize
                        >=> bindJson<ReportUpdateDto> saveReport
                        route "/api/report/pass"
                        >=> authorize
                        >=> bindJson<ReportUpdateDto> passReport
                        route "/api/cs/report/publish"
                        >=> bindJson<PublishReports> publishReports
                        route "/api/report/query"
                        >=> authorize
                        >=> bindJson<GetQueriedReports> getQueriedReportsHandler ]
              GET
              >=> authorize
              >=> choose
                      [ mustBeAdmin
                        >=> route "/api/report/stats"
                        >=> getStatistics
                        route "/api/report/org"
                        >=> passOrgId getOrganizationReports
                        routef "/api/report/get/%s" getReport
                        route "/api/report/info" >=> getActualReports ] ]
