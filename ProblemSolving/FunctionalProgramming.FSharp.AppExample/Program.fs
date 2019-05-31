// from https://www.hackerrank.com/challenges/fp-list-replication

module FunctionalProgramming.FSharp.AppExample

open ProblemSolving.Common
open System
open System.IO

type ListReplicationTask(input: TextReader, output: TextWriter) =

    let readInput () =
        match input.ReadLine() with
        | null -> None
        | value -> (value |> int, ()) |> Some

    interface ITask with
        member this.Execute(argv: string[]) =
            let times = input.ReadLine() |> int
            let inputData = Seq.unfold readInput ()
            let outputData = Seq.fold (fun dest number -> dest @ [for _ in 1 .. times -> number]) [] inputData |> Seq.toList
            outputData |> List.iter (fun number -> output.WriteLine("{0}", number))
            0

[<EntryPoint>]
let main argv =
    let task = new ListReplicationTask(Console.In, Console.Out) :> ITask
    argv |> task.Execute
