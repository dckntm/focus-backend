namespace Focus.Api.Common

open Microsoft.Extensions.Logging

module Log =
    let ConfigureLogging(builder: ILoggingBuilder) = 
        builder.AddConsole() |> ignore
