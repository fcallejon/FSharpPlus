namespace FSharpPlus

#if !FABLE_COMPILER

/// Additional operations on Task<'T>
[<RequireQualifiedAccess>]
module Task =

    open System
    open System.Threading.Tasks
    
    let private (|Canceled|Faulted|Completed|) (t: Task<'a>) =
        if t.IsCanceled then Canceled
        else if t.IsFaulted then Faulted t.Exception
        else Completed t.Result

    /// <summary>Creates a task workflow from another workflow 'x', mapping its result with 'f'.</summary>
    let map (f : 'T -> 'U) (t : Task<'T>) : Task<'U> =
        if task.Status = TaskStatus.RanToCompletion then
            try Task.FromResult (f task.Result)
            with e ->
                let tcs = TaskCompletionSource<'U> TaskCreationOptions.RunContinuationsAsynchronously
                tcs.SetException e
                tcs.Task
        else
            let tcs = TaskCompletionSource<'U> TaskCreationOptions.RunContinuationsAsynchronously
            if task.Status = TaskStatus.Faulted then
                tcs.SetException task.Exception.InnerExceptions
                tcs.Task
            elif task.Status = TaskStatus.Canceled then
                tcs.SetCanceled ()
                tcs.Task
            else
                let k = function
                    | Canceled    -> tcs.SetCanceled ()
                    | Faulted e   -> tcs.SetException e.InnerExceptions
                    | Completed r ->
                        try tcs.SetResult (f r)
                        with e -> tcs.SetException e
                task.ContinueWith k |> ignore
                tcs.Task

    /// <summary>Creates a task workflow from two workflows 'x' and 'y', mapping its results with 'f'.</summary>
    /// <remarks>Workflows are run in sequence.</remarks>
    /// <param name="f">The mapping function.</param>
    /// <param name="x">First task workflow.</param>
    /// <param name="y">Second task workflow.</param>
    let map2 (f : 'T -> 'U -> 'V) (x : Task<'T>) (y : Task<'U>) : Task<'V> =
        x.ContinueWith(fun (x' : Task<'T>) ->
            y.ContinueWith(fun (y' : Task<'U>) ->
                (f x'.Result y'.Result))).Unwrap()

    /// <summary>Creates a task workflow that is the result of applying the resulting function of a task workflow
    /// to the resulting value of another task workflow</summary>
    /// <param name="f">Task workflow returning a function</param>
    /// <param name="x">Task workflow returning a value</param>
    let apply (f : Task<'T -> 'U>) (t : Task<'T>) : Task<'U> =
        f.ContinueWith(fun (f' : Task<'T -> 'U>) ->
            t.ContinueWith(fun (t' : Task<'T>) ->
                f'.Result t'.Result)).Unwrap()

    /// <summary>Creates a task workflow from two workflows 'x' and 'y', tupling its results.</summary>
    let zip (x : Task<'T>) (y : Task<'U>) : Task<'T * 'U> =
        x.ContinueWith(fun (x' : Task<'T>) ->
            y.ContinueWith(fun (y' : Task<'U>) ->
                (x'.Result, y'.Result))).Unwrap()

    /// Flattens two nested tasks into one.
    let join (t : Task<Task<'T>>) : Task<'T> = t.Unwrap()
#endif
