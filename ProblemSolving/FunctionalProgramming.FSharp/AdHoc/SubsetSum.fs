// from https://www.hackerrank.com/challenges/subset-sum

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type SubsetSumTask(input: TextReader, output: TextWriter) =

    let createSumArray (numbers: int[]) =
        let sumArray = numbers.Length |> Array.zeroCreate
        sumArray.[0] <- numbers.[0] |> int64
        for index in 1 .. numbers.Length - 1 do
            sumArray.[index] <- sumArray.[index - 1] + (numbers.[index] |> int64)
        sumArray
        
    let calcMinSubsetSize (sum: int64) (sumArray: int64[]) =
        match System.Array.BinarySearch(sumArray, sum) with
        | index when index < 0 -> let index = ~~~index in if index = sumArray.Length then -1 else index + 1
        | index -> index + 1

    interface ITask with
        member this.Execute(argv: string[]) =
            // N
            input.ReadLine() |> ignore
            let numbers = input.ReadLine().Split(" ") |> Array.map int |> Array.sortDescending
            let sumArray = numbers |> createSumArray
            let testCases = input.ReadLine() |> int
            for _ in 1 .. testCases do
                let sum = input.ReadLine() |> int64
                calcMinSubsetSize sum sumArray |> output.WriteLine
            0

[<TestFixture>]
type SubsetSumTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//SubsetSum"

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
    [<TestCase("Input17.txt", "Output17.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new SubsetSumTask(reader, writer) :> ITask), input, expectedOutput)