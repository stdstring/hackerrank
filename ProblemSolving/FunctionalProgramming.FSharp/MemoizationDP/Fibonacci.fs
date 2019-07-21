// from https://www.hackerrank.com/challenges/fibonacci-fp

namespace FunctionalProgramming.FSharp.MemoizationDP

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type FibonacciTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let MaxNumber = 10000

    [<Literal>]
    let ModValue = 100000007

    let rec fillNumbers (current: int) (number: int) (storage: int[]) =
        match current with
        | _ when current >= number -> ()
        | _ ->
            storage.[current]<-(storage.[current - 1] + storage.[current - 2]) % ModValue
            storage |> fillNumbers (current + 1) (number)

    let rec processTestCase (testCaseNumber: int) (testCaseCount: int) (current: int) (storage: int[]) =
        match testCaseNumber with
        | _ when testCaseNumber > testCaseCount -> ()
        | _ ->
            match input.ReadLine() |> int with
            | 0 ->
                0 |> output.WriteLine
                storage |> processTestCase (testCaseNumber + 1) testCaseCount current
            | number ->
                storage |> fillNumbers current number
                storage.[number - 1] |> output.WriteLine
                storage |> processTestCase (testCaseNumber + 1) testCaseCount (max current number)

    interface ITask with
        member this.Execute(argv: string[]) =
            let storage = Array.create MaxNumber 0
            // F0 = 0, F1 = 1, F2 = 1
            storage.[0]<-1
            storage.[1]<-1
            let testCaseCount = input.ReadLine() |> int
            storage |> processTestCase 1 testCaseCount 2
            0

[<TestFixture>]
type FibonacciTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//MemoizationDP//Fibonacci"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new FibonacciTask(reader, writer) :> ITask), input, expectedOutput)
