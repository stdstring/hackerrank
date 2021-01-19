// from https://www.hackerrank.com/challenges/functional-programming-the-sums-of-powers

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type SumsOfPowersTask(input: TextReader, output: TextWriter) =

    let rec calcPossibleWays (powers: int list) (number: int) (count: int) =
        match powers with
        | [] -> count
        | power :: _ when power > number -> count
        | power :: _ when power = number -> count + 1
        | [power] when power < number -> count
        | power :: powersRest -> count |> calcPossibleWays powersRest (number - power) |> calcPossibleWays powersRest number

    interface ITask with
        member this.Execute(argv: string[]) =
            let x = input.ReadLine() |> int
            let n = input.ReadLine() |> int
            let powers = (fun index -> pown (index + 1) n) |> Seq.initInfinite |> Seq.takeWhile (fun power -> power <= x) |> Seq.toList
            0 |> calcPossibleWays powers x |> output.WriteLine
            0

[<TestFixture>]
type SumsOfPowersTests() =

    [<TestCase([|"10"; "2"|], [|"1"|])>]
    [<TestCase([|"100"; "2"|], [|"3"|])>]
    [<TestCase([|"100"; "3"|], [|"1"|])>]
    [<TestCase([|"800"; "2"|], [|"561"|])>]
    [<TestCase([|"1000"; "10"|], [|"0"|])>]
    [<TestCase([|"400"; "2"|], [|"55"|])>]
    member public this.Execute(input: string[], expectedOutput: string[]) =
        TaskExecutor.Execute((fun reader writer -> new SumsOfPowersTask(reader, writer) :> ITask), input, expectedOutput)