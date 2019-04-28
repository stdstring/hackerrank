// from https://www.hackerrank.com/challenges/prefix-compression

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open System.IO
open System.Text
open FunctionalProgramming.Common

type PrefixCompressionTask(input: TextReader, output: TextWriter) =

    let processStrings (str1 : string) (str2 : string) =
        let length1 = str1.Length
        let length2 = str2.Length
        let prefix = new StringBuilder(min length1 length2)
        let rec processStringsImpl (index: int) =
            match index with
            | _ when (index = length1) && (index = length2) -> (prefix.ToString(), "", "")
            | _ when (index = length1) && (index < length2) -> (prefix.ToString(), "", str2.Substring(index))
            | _ when (index < length1) && (index = length2) -> (prefix.ToString(), str1.Substring(index), "")
            | _ when str1.[index] <> str2.[index] -> (prefix.ToString(), str1.Substring(index), str2.Substring(index))
            | _ ->
                prefix.Append(str1.[index]) |> ignore
                (index + 1) |> processStringsImpl
        0 |> processStringsImpl

    let showResult: string -> unit =
        function
        | "" -> "0" |> output.WriteLine
        | str -> output.WriteLine("{0} {1}", str.Length, str)

    interface ITask with
        member this.Execute(argv: string[]) =
            let str1 = input.ReadLine()
            let str2 = input.ReadLine()
            let prefix, left, right = processStrings str1 str2
            showResult prefix
            showResult left
            showResult right
            0

[<TestFixture>]
type PrefixCompressionTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//PrefixCompression"

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
        TaskExecuter.Execute((fun reader writer -> new PrefixCompressionTask(reader, writer) :> ITask), input, expectedOutput)