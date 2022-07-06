namespace mazharenko.FluentAssertions.Extensions

open System
open System.Diagnostics
open System.Threading
open FluentAssertions.Execution;

module EventualAssertions =
    type EventualAssertionsBuilder(timeout: TimeSpan, delay: TimeSpan) =
        member __.Zero() = ()
        member __.Delay f =
            use scope = new AssertionScope()
            let stopwatch = Stopwatch.StartNew()            
            let rec repeat() =
                if (not <| scope.HasFailures() || stopwatch.Elapsed > timeout)
                then ()                
                else
                    Thread.Sleep delay
                    scope.Discard() |> ignore
                    f()
                    repeat()
            f()
            repeat()
            
        
    let eventually timeout delay = EventualAssertionsBuilder(timeout, delay) 
    let eventuallyMs timeout delay = eventually (TimeSpan.FromMilliseconds timeout) (TimeSpan.FromMilliseconds delay) 
