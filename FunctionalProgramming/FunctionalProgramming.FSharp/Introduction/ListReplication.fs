// from https://www.hackerrank.com/challenges/fp-list-replication

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

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

[<TestFixture>]
type ListReplicationTests() =

    [<TestCase([|"2"; "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9"; "10"|], [|"1"; "1"; "2"; "2"; "3"; "3"; "4"; "4"; "5"; "5"; "6"; "6"; "7"; "7"; "8"; "8"; "9"; "9"; "10"; "10"|])>]
    [<TestCase([|"5"; "1"; "2"; "3"; "4"; "5"|], [|"1"; "1"; "1"; "1"; "1"; "2"; "2"; "2"; "2"; "2"; "3"; "3"; "3"; "3"; "3"; "4"; "4"; "4"; "4"; "4"; "5"; "5"; "5"; "5"; "5"|])>]
    [<TestCase([|"3"; "1"; "2"; "3"; "4"|], [|"1"; "1"; "1"; "2"; "2"; "2"; "3"; "3"; "3"; "4"; "4"; "4"|])>]
    member public this.Execute(input: string[], expectedOutput: string[]) =
        TaskExecuter.Execute((fun reader writer -> new ListReplicationTask(reader, writer) :> ITask), input, expectedOutput)
