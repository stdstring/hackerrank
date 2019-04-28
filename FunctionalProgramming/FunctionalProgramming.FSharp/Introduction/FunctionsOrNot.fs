// https://www.hackerrank.com/challenges/functions-or-not

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open System.Collections.Generic
open System.IO
open FunctionalProgramming.Common

type FunctionsOrNotTask(input: TextReader, output: TextWriter) =

    let readTestcase () =
        let count = input.ReadLine() |> int
        [for _ in 1 .. count -> input.ReadLine().Split(' ') |> Array.map (fun value -> int value)]

    let checkTestcase (testCase: int[] list) =
        let knownPairs = new Dictionary<int, int>()
        let existFun (pair : int []) =
            let x = pair.[0]
            let y = pair.[1]
            match knownPairs.ContainsKey(x) with
            | true -> (knownPairs.[x] <> y) 
            | false ->
                knownPairs.Add(x, y)
                false
        testCase |> List.exists existFun |> not

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let testCases = [for _ in 1 .. count -> readTestcase ()]
            let resultMapFun = function
                | true -> "YES"
                | false -> "NO"
            testCases |> List.iter (fun testCase -> checkTestcase testCase |> resultMapFun |> output.WriteLine)
            0

[<TestFixture>]
type FunctionsOrNotTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Introduction//FunctionsOrNot"

    [<TestCase("Input00.txt", [|"YES"; "YES"|])>]
    [<TestCase("Input01.txt", [|"YES"; "NO"|])>]
    [<TestCase("Input02.txt", [|"YES"; "YES"; "NO"|])>]
    [<TestCase("Input03.txt", [|"YES"; "YES"; "NO"|])>]
    [<TestCase("Input04.txt", [|"YES"; "NO"; "YES"|])>]
    [<TestCase("Input05.txt", [|"YES"; "YES"; "NO"; "NO"; "NO"|])>]
    member public this.Execute(inputFile: string, expectedOutput: string[]) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecuter.Execute((fun reader writer -> new FunctionsOrNotTask(reader, writer) :> ITask), input, expectedOutput)