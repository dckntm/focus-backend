namespace Focus.Service.ReportSchduler.Api.Router

open Focus.Service.ReportScheduler.Application.Commands
open Focus.Service.ReportScheduler.Application.Queries
open Focus.Service.ReportScheduler.Application.Dto
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Api.Common.HelperHandlers
open Microsoft.AspNetCore.Http
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Giraffe.Core
open MediatR
open Focus.Api.Common.AuthHandlers

module Router =

    let createReportSchedulerHandler (dto: ReportScheduleDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportSchedule(dto))

                return! handleResult result next ctx
            }

    let getReportScheduleInfo: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportScheduleInfos())

                return! handleResult result next ctx
            }

    let getReportSchedule (scheduleId: string) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportSchedule(scheduleId))

                return! handleResult result next ctx
            }

    let constructReportScheduleHandler (dto: ReportScheduleDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(ConstructReport(dto))

                return! handleResult result next ctx
            }

    let getStatistics : HttpHandler = 
        fun next ctx -> 
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetStatistics())

                return! handleResult result next ctx
            }

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        mustBeAdmin >=> choose
            [ POST
              >=> choose
                      [ route "/api/report/schedule"
                        >=> bindJson<ReportScheduleDto> createReportSchedulerHandler
                        route "/api/report/schedule/construct"
                        >=> bindJson<ReportScheduleDto> constructReportScheduleHandler ]
              GET
              >=> choose
                      [ route "/api/report/schedule/info"
                        >=> getReportScheduleInfo
                        route "/api/report/schedule/stats" >=> getStatistics
                        routef "/api/report/schedule/%s" getReportSchedule]
              setStatusCode 404 >=> text "Not Found" ]
