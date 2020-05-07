namespace Focus.Service.ReportConstructor.Api

open Focus.Service.ReportConstructor.Application.Commands
open Focus.Service.ReportConstructor.Application.Queries
open Focus.Service.ReportConstructor.Application.Dto
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Api.Common.AuthHandlers
open Microsoft.AspNetCore.Http
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Giraffe.Core
open MediatR
open Focus.Api.Common.HelperHandlers
open Focus.Application.Common.Messages.Commands

module Router =

    let createReportTemplateHandler (dto: ReportTemplateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportTemplate(dto))

                return! handleResult result next ctx
            }

    let getReportTemplateHandler (reportId: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportTemplate(reportId))

                return! handleResult result next ctx
            }

    let getReportTemplateInfosHandler: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportTemplateInfos())

                return! handleResult result next ctx
            }

    let constructReports (command: ConstructReports): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(command)

                return! handleResult result next ctx
            }

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST
              >=> choose
                      [ route "/api/report/template"
                        >=> bindJson<ReportTemplateDto> createReportTemplateHandler
                        route "/api/cs/report/construct"
                        >=> bindJson<ConstructReports> constructReports ]
              GET
              >=> choose
                      [ route "/api/report/template/info"
                        >=> getReportTemplateInfosHandler
                        routef "/api/report/template/%s" getReportTemplateHandler ]
              setStatusCode 404 >=> text "Not found" ]
