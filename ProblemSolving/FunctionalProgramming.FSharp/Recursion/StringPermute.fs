// from https://www.hackerrank.com/challenges/string-o-permute

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO
open System.Text

type StringPermuteTask(input: TextReader, output: TextWriter) =

    let processString (source : string) =
        let sourceLength = source.Length
        let storage = new StringBuilder(sourceLength)
        let rec processStringImpl (index: int) =
            match index with
            | _ when index = sourceLength -> storage.ToString()
            | _ when index = (sourceLength - 1) -> storage.Append(source.[index]).ToString()
            | _ ->
                storage.Append(source.[index + 1]).Append(source.[index]) |> ignore
                (index + 2) |> processStringImpl
        0 |> processStringImpl

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let source = [for _ in 1 .. count -> input.ReadLine()]
            let dest = source |> Seq.map processString
            dest |> Seq.iter (fun str -> str |> output.WriteLine)
            0

[<TestFixture>]
type StringPermuteTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//StringPermute"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    [<TestCase("Input04.txt", "Output04.txt")>]
    [<TestCase("Input05.txt", "Output05.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new StringPermuteTask(reader, writer) :> ITask), input, expectedOutput)