// https://www.hackerrank.com/challenges/fp-filter-positions-in-a-list

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open ProblemSolving.Common
open System.IO


type FilterListByPositionTask(input: TextReader, output: TextWriter) =

    let readInput () =
        match input.ReadLine() with
        | null -> None
        | value -> Some(value |> int, ())

    interface ITask with
        member this.Execute(argv: string[]) =
            let inputData = Seq.unfold readInput ()
            let outputData = inputData |> Seq.mapi (fun index number -> (index, number)) |> Seq.filter (fun (index, _) -> index % 2 <> 0) |> Seq.map (fun (_, number) -> number) |> Seq.toList
            outputData |> List.iter (fun number -> number |> output.WriteLine)
            0

[<TestFixture>]
type FilterListByPositionTests() =

    [<TestCase([|"8"; "15"; "22"; "1"; "10"; "6"; "2"; "18"; "18"; "1"|], [|"15"; "1"; "6"; "18"; "1"|])>]
    [<TestCase([|"19"; "28"; "27"; "3"; "27"; "26"; "24"; "25"; "11"; "15"; "23"; "11"; "20"; "24"; "15"|], [|"28"; "3"; "26"; "25"; "15"; "11"; "24"|])>]
    member public this.Execute(input: string[], expectedOutput: string[]) =
        TaskExecutor.Execute((fun reader writer -> new FilterListByPositionTask(reader, writer) :> ITask), input, expectedOutput)