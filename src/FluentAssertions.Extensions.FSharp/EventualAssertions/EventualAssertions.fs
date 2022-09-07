namespace mazharenko.FluentAssertions.Extensions

open System
open System.Diagnostics
open System.Threading
open FluentAssertions.Execution

module EventualAssertions =
    type EventualAssertionsBuilder(timeout: TimeSpan, delay: TimeSpan) =
        member _.Zero() = ()
        member _.Delay f = f

        member _.Run f =
            use scope = new AssertionScope()
            let stopwatch = Stopwatch.StartNew()

            let rec repeat () =
                if (not <| scope.HasFailures()
                    || stopwatch.Elapsed > timeout) then
                    ()
                else
                    Thread.Sleep delay
                    scope.Discard() |> ignore
                    f ()
                    repeat ()

            f ()
            repeat ()

    type EventualAssertionsBuilderAsync(timeout: TimeSpan, delay: TimeSpan) =
        member _.Zero() = async.Zero()
        member _.Delay f = f

        member _.Run f =
            async {
                use scope = new AssertionScope()
                let stopwatch = Stopwatch.StartNew()

                let rec repeat () =
                    async {
                        if (not <| scope.HasFailures()
                            || stopwatch.Elapsed > timeout) then
                            ()
                        else
                            do! Async.Sleep delay
                            scope.Discard() |> ignore
                            do! f ()
                            do! repeat ()
                    }

                do! f ()
                do! repeat ()
            }

        member _.Return f = f


    let eventually timeout delay =
        EventualAssertionsBuilder(timeout, delay)

    let eventuallyMs timeout delay =
        eventually (TimeSpan.FromMilliseconds timeout) (TimeSpan.FromMilliseconds delay)

    let eventuallyAsync timeout delay =
        EventualAssertionsBuilderAsync(timeout, delay)

    let eventuallyAsyncMs timeout delay =
        eventuallyAsync (TimeSpan.FromMilliseconds timeout) (TimeSpan.FromMilliseconds delay)
