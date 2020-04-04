namespace Focus.Service.ReportScheduler.Api.ReportScheduler

open Giraffe.Core
open Microsoft.AspNetCore.Http
open MediatR
open FSharp.Control.Tasks.V2.ContextInsensitive
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Focus.Service.ReportConstructor.Application.Dto
open Focus.Service.ReportConstructor.Application.Commands
open Focus.Service.ReportConstructor.Application.Queries

module Router =

    let createReportTemplateHandler: HttpHandler =
        fun next ctx ->
            task {
                let! reportTemplateDto = ctx.BindJsonAsync<ReportTemplateDto>()
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportTemplate(reportTemplateDto))

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

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST >=> choose [ routex "/api/report/template(/?)" >=> createReportTemplateHandler ]
              GET >=> choose
                          [ route "/api/report/template/info" >=> getReportTemplateInfosHandler
                            routef "/api/report/template/%s" getReportTemplateHandler ]
              setStatusCode 404 >=> text "Not found" ]
