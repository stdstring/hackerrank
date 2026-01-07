// https://www.hackerrank.com/challenges/valid-bst

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open ProblemSolving.Common
open System.IO

module ValidBST =

    type ValidBSTTask(input: TextReader, output: TextWriter) =

        let rec checkBST (preorderTraversal: int array) (minValue: int) (maxValue: int) =
            match preorderTraversal with
            | [||] -> true
            | _ when (preorderTraversal.[0] < minValue) || (preorderTraversal.[0] > maxValue) -> false
            | [|_|] -> true
            | _ ->
                let middleValue = preorderTraversal |> Array.head
                let leftValues = preorderTraversal |> Seq.skip 1 |> Seq.takeWhile (fun value -> value < middleValue) |> Seq.toArray
                let rightValues = preorderTraversal |> Array.skip (1 + leftValues.Length)
                let checkLeftValues = leftValues |> Seq.exists (fun value -> value < minValue) |> not
                let checkRightValues = rightValues |> Seq.exists (fun value -> (value <= middleValue) || (value > maxValue)) |> not
                match checkLeftValues && checkRightValues with
                | false -> false
                | true ->
                    (checkBST leftValues minValue (middleValue - 1)) && (checkBST rightValues (middleValue + 1) maxValue)

        interface ITask with
            member _.Execute(_: string[]) =
                let testCasesCount = input.ReadLine() |> int
                for _ in seq {0 .. testCasesCount - 1} do
                    let n = input.ReadLine() |> int
                    let values = input.ReadLine().Split(' ') |> Array.map int
                    let result = match checkBST values 1 n with
                                 | true -> "YES"
                                 | false -> "NO"
                    result |> output.WriteLine
                0

open ValidBST

[<TestFixture>]
type ValidBSTTests() =

    [<Literal>]
    let RootDirectory = __SOURCE_DIRECTORY__ + "//..//TestCases//FunctionalStructures//ValidBST"

    [<TestCase("Input00.txt", [|"YES"; "YES"; "YES"; "NO"; "NO"|])>]
    [<TestCase("Input01.txt", [|"YES"; "YES"; "NO"; "YES"; "NO"; "NO"; "YES"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input02.txt", [|"NO"; "NO"; "NO"; "NO"; "YES"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input03.txt", [|"NO"; "YES"; "NO"; "NO"; "NO"; "YES"; "YES"; "NO"|])>]
    [<TestCase("Input04.txt", [|"NO"; "YES"; "YES"; "NO"; "YES"; "YES"; "NO"; "NO"; "YES"; "NO"|])>]
    [<TestCase("Input05.txt", [|"YES"; "YES"; "NO"; "NO"; "YES"; "YES"; "YES"; "YES"; "YES"|])>]
    [<TestCase("Input06.txt", [|"NO"; "YES"; "YES"; "YES"; "NO"; "YES"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input07.txt", [|"NO"; "YES"; "YES"; "YES"; "NO"; "YES"; "YES"; "NO"|])>]
    [<TestCase("Input08.txt", [|"YES"; "YES"; "NO"; "YES"; "NO"; "YES"; "YES"; "YES"; "YES"|])>]
    [<TestCase("Input09.txt", [|"YES"; "NO"; "NO"; "NO"; "NO"; "YES"; "NO"; "YES"; "YES"|])>]
    [<TestCase("Input10.txt", [|"NO"; "NO"; "YES"; "YES"; "NO"; "YES"; "YES"; "YES"; "NO"|])>]
    [<TestCase("Input11.txt", [|"YES"; "YES"; "YES"; "NO"; "NO"|])>]
    member public _.Execute(inputFile: string, expectedOutput: string[]) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new ValidBSTTask(reader, writer) :> ITask), input, expectedOutput)