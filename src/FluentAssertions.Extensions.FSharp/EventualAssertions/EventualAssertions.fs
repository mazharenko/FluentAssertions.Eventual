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
            let success =
                using (new AssertionScope()) (fun scope ->
                    let stopwatch = Stopwatch.StartNew()

                    let rec repeat () =
                        if (not <| scope.HasFailures()) then
                            true
                        elif (stopwatch.Elapsed > timeout) then
                            scope.Discard() |> ignore
                            false
                        else
                            scope.Discard() |> ignore
                            Thread.Sleep delay
                            f ()
                            repeat ()

                    f()
                    repeat ())

            if (not <| success) then f ()

    type EventualAssertionsBuilderAsync(timeout: TimeSpan, delay: TimeSpan) =
        member _.Zero() = async.Zero()
        member _.Delay f = f

        member _.Run f =
            async {
                let! success =
                    async {
                        use scope = new AssertionScope()
                        let stopwatch = Stopwatch.StartNew()

                        let rec repeat () =
                            async {
                                if (not <| scope.HasFailures()) then
                                    return true
                                elif (stopwatch.Elapsed > timeout) then
                                    scope.Discard() |> ignore
                                    return false
                                else
                                    scope.Discard() |> ignore
                                    do! Async.Sleep delay
                                    do! f ()
                                    return! repeat ()
                            }
                        do! f()
                        return! repeat ()
                    }
                if (not <| success) then
                    do! f ()
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
