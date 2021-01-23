// https://www.hackerrank.com/challenges/lists-and-gcd

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type ListsAndGCDTask(input: TextReader, output: TextWriter) =

    let rec calcGCD (left: int list) (right: int list) (dest: int list) =
        match left, right with
        | [], _ -> dest |> List.rev
        | _, [] -> dest |> List.rev
        | factor1 :: power1 :: leftRest, factor2 :: power2 :: rightRest when factor1 = factor2 -> (min power1 power2) :: factor1 :: dest |> calcGCD leftRest rightRest
        | factor1 :: _ :: leftRest, factor2 :: _ when factor1 < factor2 -> dest |> calcGCD leftRest right
        | factor1 :: _, factor2 :: _ :: rightRest when factor1 > factor2 -> dest |> calcGCD left rightRest
        | _ -> failwith "Unexpected branch of match expression"

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            seq {1 .. count} |>
            Seq.map (fun _ -> input.ReadLine().Split(" ") |> Seq.map int |> Seq.toList) |>
            Seq.reduce (fun left right -> [] |> calcGCD left right) |>
            Seq.iter (fun number -> output.Write("{0} ", number))
            0

[<TestFixture>]
type ListsAndGCDTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//FunctionalStructures//ListsAndGCD"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    [<TestCase("Input04.txt", "Output04.txt")>]
    [<TestCase("Input05.txt", "Output05.txt")>]
    [<TestCase("Input06.txt", "Output06.txt")>]
    [<TestCase("Input07.txt", "Output07.txt")>]
    [<TestCase("Input08.txt", "Output08.txt")>]
    [<TestCase("Input09.txt", "Output09.txt")>]
    [<TestCase("Input10.txt", "Output10.txt")>]
    [<TestCase("Input11.txt", "Output11.txt")>]
    [<TestCase("Input12.txt", "Output12.txt")>]
    [<TestCase("Input13.txt", "Output13.txt")>]
    [<TestCase("Input14.txt", "Output14.txt")>]
    [<TestCase("Input15.txt", "Output15.txt")>]
    [<TestCase("Input16.txt", "Output16.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new ListsAndGCDTask(reader, writer) :> ITask), input, expectedOutput)