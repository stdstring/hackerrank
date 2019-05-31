// from https://www.hackerrank.com/challenges/string-compression

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO
open System.Text

type StringCompressionTask(input: TextReader, output: TextWriter) =

    interface ITask with
        member this.Execute(argv: string[]) =
            let source = input.ReadLine()
            let storage = new StringBuilder(source.Length)
            let foldFun (savedChar, savedCount) index =
                let ch = source.[index]
                match ch with
                | _ when savedChar = ch -> (savedChar, savedCount + 1)
                | _ ->
                    if (savedCount > 1) then
                        storage.Append(savedCount) |> ignore
                    storage.Append(ch) |> ignore
                    (ch, 1)
            let _, count = {0 .. source.Length - 1} |> Seq.fold foldFun (char 0, 1)
            if (count > 1) then
                storage.Append(count) |> ignore
            storage.ToString() |> output.WriteLine
            0

[<TestFixture>]
type StringCompressionTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//StringCompression"

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
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new StringCompressionTask(reader, writer) :> ITask), input, expectedOutput)