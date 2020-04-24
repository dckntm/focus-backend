namespace Focus.Api.Common

open MediatR
open Giraffe

module HelperHandlers =

    let injectMediator (f: IMediator -> HttpHandler): HttpHandler =
        fun next ctx ->
            let mediator = ctx.GetService<IMediator>()
            f mediator next ctx
