// from https://www.hackerrank.com/challenges/string-mingling

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open System.IO
open System.Text
open FunctionalProgramming.Common

type StringMinglingTask(input: TextReader, output: TextWriter) =

    interface ITask with
        member this.Execute(argv: string[]) =
            let a = input.ReadLine()
            let b = input.ReadLine()
            let storage = new StringBuilder(a.Length + b.Length)
            let storage = {0 .. a.Length - 1} |> Seq.fold (fun (acc : StringBuilder) index -> acc.Append(a.[index]).Append(b.[index])) storage
            storage.ToString() |> output.WriteLine
            0

[<TestFixture>]
type StringMinglingTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//StringMingling"

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
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecuter.Execute((fun reader writer -> new StringMinglingTask(reader, writer) :> ITask), input, expectedOutput)