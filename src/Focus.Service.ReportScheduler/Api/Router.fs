namespace Focus.Service.ReportSchduler.Api.Router

open Giraffe.Core
open Microsoft.AspNetCore.Http
open MediatR
open FSharp.Control.Tasks.V2.ContextInsensitive
open Focus.Service.ReportScheduler.Application.Commands
open Focus.Service.ReportScheduler.Application.Queries
open Giraffe.ResponseWriters
open Giraffe.ModelBinding
open Giraffe.Routing
open Focus.Service.ReportScheduler.Application.Dto

module Router =

    let createReportSchedulerHandler: HttpHandler =
        fun next ctx ->
            task {
                let! reportScheduleDto = ctx.BindJsonAsync<ReportScheduleDto>()

                let mediator = ctx.GetService<IMediator>()

                let! result = mediator.Send(CreateReportSchedule(reportScheduleDto))

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

    let webApp: HttpFunc -> HttpContext -> HttpFuncResult =
        choose
            [ POST >=> route "/report/schedule" >=> createReportSchedulerHandler
              GET >=> choose
                          [ route "/report/schedule/info" >=> getReportScheduleInfo
                            routef "/report/schedule/%s" getReportSchedule ]
              setStatusCode 404 >=> text "Not Found" ]
