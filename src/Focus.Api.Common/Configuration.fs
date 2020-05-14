namespace Focus.Api.Common

open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Giraffe

module Configuration =

    let setup (args: string array) =
        fun (context: WebHostBuilderContext) (config: IConfigurationBuilder) ->
            let env = context.HostingEnvironment

            config.AddJsonFile("appsettings.json", true, true)
                  .AddJsonFile((sprintf "appsettings.%s.json" env.EnvironmentName), true, true)
            |> ignore

            if isNotNull args then config.AddCommandLine(args) |> ignore
