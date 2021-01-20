// from https://www.hackerrank.com/challenges/different-ways-fp

namespace FunctionalProgramming.FSharp.MemoizationDP

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type DifferentWaysTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let ModValue = 100000007

    interface ITask with
        member this.Execute(argv: string[]) =
            let testCases = input.ReadLine() |> int
            let testCaseParams = seq {1 .. testCases} |> Seq.map (fun _ -> let [n; k] = input.ReadLine().Split(' ') |> Seq.map int |> Seq.toList in n, k) |> Seq.toList
            let maxN = testCaseParams |> Seq.map (fun (n, k) -> n) |> Seq.max
            let storage = maxN |> Array.zeroCreate
            for number in 1 .. maxN do
                storage.[number - 1] <- (number + 1 |> Array.zeroCreate)
                storage.[number - 1].[0] <- 1
                for k in 1 .. (number - 1) do
                    storage.[number - 1].[k] <- (storage.[number - 2].[k - 1] + storage.[number - 2].[k]) % ModValue
                storage.[number - 1].[number] <- 1
            testCaseParams |> Seq.iter (fun (n, k) -> storage.[n - 1].[k] |> output.WriteLine)
            0

[<TestFixture>]
type DifferentWaysTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//MemoizationDP//DifferentWays"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    [<TestCase("Input04.txt", "Output04.txt")>]
    [<TestCase("Input05.txt", "Output05.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new DifferentWaysTask(reader, writer) :> ITask), input, expectedOutput)