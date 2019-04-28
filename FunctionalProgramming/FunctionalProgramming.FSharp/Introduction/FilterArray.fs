// from https://www.hackerrank.com/challenges/fp-filter-array

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

type FilterArrayTask(input: TextReader, output: TextWriter) =

    let readInput () =
        match input.ReadLine() with
        | null -> None
        | value -> Some(value |> int, ())

    interface ITask with
        member this.Execute(argv: string[]) =
            let topBorder = input.ReadLine() |> int
            let inputData = Seq.unfold readInput ()
            let outputData = inputData |> Seq.filter (fun number -> number < topBorder) |> Seq.toList
            outputData |> List.iter (fun number -> number |> output.WriteLine)
            0

[<TestFixture>]
type FilterArrayTests() =

    [<TestCase([|"25"; "-41"; "46"; "-28"; "21"; "52"; "83"; "-29"; "84"; "27"; "40"|], [|"-41"; "-28"; "21"; "-29"|])>]
    [<TestCase([|"-14"; "-6"; "-52"; "-92"; "45"; "-15"; "-38"; "75"; "-53"; "-41"; "52"; "-78"; "-56"; "54"; "53"; "-78"; "63"; "69"; "-64"; "-72"; "65"; "-12"; "56"; "-45"; "-18"; "-67"; "89"; "-75"; "30"; "39"; "-48"|],
               [|"-52"; "-92"; "-15"; "-38"; "-53"; "-41"; "-78"; "-56"; "-78"; "-64"; "-72"; "-45"; "-18"; "-67"; "-75"; "-48"|])>]
    [<TestCase([|"3"; "10"; "9"; "8"; "2"; "7"; "5"; "1"; "3"; "0"|], [|"2"; "1"; "0"|])>]
    member public this.Execute(input: string[], expectedOutput: string[]) =
        TaskExecuter.Execute((fun reader writer -> new FilterArrayTask(reader, writer) :> ITask), input, expectedOutput)