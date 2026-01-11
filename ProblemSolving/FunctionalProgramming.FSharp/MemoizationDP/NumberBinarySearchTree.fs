// from https://www.hackerrank.com/challenges/number-of-binary-search-tree

namespace FunctionalProgramming.FSharp.MemoizationDP

open NUnit.Framework
open ProblemSolving.Common
open System.IO

module NumberBinarySearchTree =

    type NumberBinarySearchTreeTask(input: TextReader, output: TextWriter) =

        [<Literal>]
        let modValue = 100000007L

        // n = 0 -> count = 1
        // n = 1 -> count = 1
        let binarySearchTreeCountCache = new ResizeArray<int64>([1L; 1L]);

        let rec calcBinarySearchTreeCount (n: int) =
            match n < binarySearchTreeCountCache.Count with
            | true ->
                binarySearchTreeCountCache.[n] |> int
            | false ->
                let current = binarySearchTreeCountCache.Count
                0L |> binarySearchTreeCountCache.Add
                for center in seq {1 .. current} do
                    let left = center - 1
                    let right = current - center
                    let count = binarySearchTreeCountCache.[left] * binarySearchTreeCountCache.[right] % modValue
                    binarySearchTreeCountCache.[current] <- (binarySearchTreeCountCache.[current] + count) % modValue
                n |> calcBinarySearchTreeCount

        interface ITask with
            member _.Execute(_: string[]) =
                let testCaseCount = input.ReadLine() |> int
                for _ in seq {1 .. testCaseCount} do
                    let n = input.ReadLine() |> int
                    let count = n |> calcBinarySearchTreeCount
                    count |> output.WriteLine
                0

open NumberBinarySearchTree

[<TestFixture>]
type NumberBinarySearchTreeTests() =

    [<Literal>]
    let RootDirectory = __SOURCE_DIRECTORY__ + "//..//TestCases//MemoizationDP//NumberBinarySearchTree"

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
    member public _.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new NumberBinarySearchTreeTask(reader, writer) :> ITask), input, expectedOutput)
