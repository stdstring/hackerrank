// from https://www.hackerrank.com/challenges/rotate-string

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

type RotateStringTask(input: TextReader, output: TextWriter) =

    let genRotations (str : string) =
        let length = str.Length
        let generator =
            function
            | index when index > length -> None
            | index when index = length -> Some(str, index + 1)
            | index ->
                let result = str.Substring(index) + str.Substring(0, index)
                Some(result, index + 1)
        Seq.unfold generator 1

    let outputRotations (rotations: seq<string>) =
        let outputRotation (index: int) (rotation: string) =
            match index with
            | 0 -> rotation |> output.Write
            | _ -> output.Write(" {0}", rotation)
        rotations |> Seq.iteri outputRotation
        "" |> output.WriteLine

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let source = [for _ in 1 .. count -> input.ReadLine()]
            let dest = source |> List.map genRotations
            dest |> List.iter outputRotations
            0

[<TestFixture>]
type RotateStringTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//RotateString"

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
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecuter.Execute((fun reader writer -> new RotateStringTask(reader, writer) :> ITask), input, expectedOutput)