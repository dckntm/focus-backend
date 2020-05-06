namespace Focus.Api.Common

open Giraffe
open Focus.Application.Common.Abstract
open System
open Microsoft.Extensions.Logging

module HelperHandlers =

    // Result type inheritence tree:
    //
    // Result [abstract]
    // ---- Successful<T>
    // ---- Successful
    // ---- Failed
    //
    // This tree illustrates why the following function works
    // We firstly checck if type is generic and if it is, we try to get value and return it as json
    // If it is not we do straightforward conversion based on Successful and Failed types
    let handleResult (result:Result) :HttpHandler =
        fun next ctx -> 
            let t = result.GetType()

            if t.IsGenericType then
                let value = t.GetProperty("Value").GetValue(result)
                json value next ctx
            else 
                match result with
                | :? Successful -> setStatusCode 200 next ctx
                | :? Failed as fail -> 
                    ctx.SetStatusCode 500
                    text fail.Message next ctx
                | _ -> text "API Failed to handle result" next ctx

    // TODO [blocked] add exception layer tag to response
    // Simple Giraffe error handler
    let errorHandler (ex : Exception) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message