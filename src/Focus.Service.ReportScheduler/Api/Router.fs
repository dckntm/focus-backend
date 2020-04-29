namespace Focus.Service.ReportSchduler.Api.Router

open Focus.Service.ReportScheduler.Application.Commands
open Focus.Service.ReportScheduler.Application.Queries
open Focus.Service.ReportScheduler.Application.Dto
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Api.Common.AuthHandlers
open Microsoft.AspNetCore.Http
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Giraffe.Core
open MediatR

module Router =

    let createReportSchedulerHandler (dto: ReportScheduleDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportSchedule(dto))

                match result with
                | success when result.IsSuccessfull -> return! json success.Result next ctx
                | fail ->
                    ctx.SetStatusCode 500
                    return! setBodyFromString fail.ErrorMessage next ctx
            }

    let getReportScheduleInfo: HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportScheduleInfos())

                if result.IsSuccessfull then
                    return! json result.Result next ctx
                else
                    ctx.SetStatusCode 500
                    return! setBodyFromString result.ErrorMessage next ctx
            }

    let getReportSchedule (scheduleId: string) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(GetReportSchedule(scheduleId))

                if result.IsSuccessfull then
                    return! json result.Result next ctx
                else
                    ctx.SetStatusCode 500
                    return! setBodyFromString result.ErrorMessage next ctx
            }

    let constructReportScheduleHandler (dto: ReportScheduleDto): HttpHandler =
        fun next ctx ->
            task {
                let mediator = ctx.GetService<IMediator>()

                let! _ = mediator.Send(InitializeReportConstruction(dto))

                return! setStatusCode 200 next ctx
            }

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        mustBeAdmin >=> choose
                            [ POST
                              >=> choose
                                      [ route "/api/report/schedule"
                                        >=> bindJson<ReportScheduleDto> createReportSchedulerHandler
                                        route "/api/report/schedule/construct"
                                        >=> bindJson<ReportScheduleDto> constructReportScheduleHandler ]
                              GET >=> choose
                                          [ route "/api/report/schedule/info" >=> getReportScheduleInfo
                                            routef "/api/report/schedule/%s" getReportSchedule ]
                              setStatusCode 404 >=> text "Not Found" ]
