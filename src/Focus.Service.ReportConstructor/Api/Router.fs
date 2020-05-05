namespace Focus.Service.ReportConstructor.Api

open Focus.Core.Common.Messages.Commands
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
open Focus.Application.Common.Services.Logging

module Router =

    let createReportTemplateHandler (dto:ReportTemplateDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportTemplate(dto))

                if result.IsSuccessfull then
                    return! json result.Result next ctx
                else
                    ctx.SetStatusCode 500
                    return! json result.ErrorMessage next ctx
            }

    let getReportTemplateHandler (reportId: string): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportTemplate(reportId))

                if result.IsSuccessfull then
                    return! json result.Result next ctx
                else
                    ctx.SetStatusCode 500
                    return! json result.ErrorMessage next ctx
            }

    let getReportTemplateInfosHandler: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportTemplateInfos())

                if result.IsSuccessfull then
                    return! json result.Result next ctx
                else
                    ctx.SetStatusCode 500
                    return! json result.ErrorMessage next ctx
            }

    let constructReports (command:ConstructReports) : HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()
                let logger = ctx.GetService<ILog>()

                logger.LogApi("Received construct reports request")

                let! _ = mediator.Send(command)

                return! setStatusCode 200 next ctx
            }

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST >=> choose [ route "/api/report/template" >=> bindJson<ReportTemplateDto> createReportTemplateHandler
                                route "/api/cs/report/construct" >=> bindJson<ConstructReports> constructReports ]
              GET >=> choose
                          [ route "/api/report/template/info" >=> getReportTemplateInfosHandler
                            routef "/api/report/template/%s" getReportTemplateHandler ]
              setStatusCode 404 >=> text "Not found" ]
