// https://www.hackerrank.com/challenges/kmp-fp/

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open ProblemSolving.Common
open System.IO

module SubstringSearching =

    type SubstringSearchingTask(input: TextReader, output: TextWriter) =

        let buildPrefixFunction (word: string) =
            let prefixFunction = word.Length |> Array.zeroCreate
            let mutable j = 0
            for i in seq {1 .. word.Length - 1} do
                while (j > 0) && (word[i] <> word[j]) do
                    j <- prefixFunction[j - 1]
                if word[i] = word[j] then
                    j <- j + 1
                prefixFunction[i] <- j
            prefixFunction

        let rec kmpIsSubstringImpl(text: string) (word: string) (i: int) (j: int) (prefixFunction: int array) =
            match i with
            | _ when i = text.Length -> false
            | _ ->
                let mutable nextj = j
                while (nextj > 0) && (text[i] <> word[nextj]) do
                    nextj <- prefixFunction[nextj - 1]
                if text[i] = word[nextj] then
                    nextj <- nextj + 1
                match nextj with
                | _ when nextj = word.Length -> true
                | _ -> kmpIsSubstringImpl text word (i + 1) nextj prefixFunction

        let kmpIsSubstring(text: string) (word: string) =
            let prefixFunction = word |> buildPrefixFunction
            kmpIsSubstringImpl text word 0 0 prefixFunction

        interface ITask with
            member _.Execute(_: string[]) =
                let testCasesCount = input.ReadLine() |> int
                for _ in seq {0 .. testCasesCount - 1} do
                    let text = input.ReadLine()
                    let word = input.ReadLine()
                    let result = match kmpIsSubstring text word with
                                 | true -> "YES"
                                 | false -> "NO"
                    result |> output.WriteLine
                0

open SubstringSearching

[<TestFixture>]
type SubstringSearchingTests() =

    [<Literal>]
    let RootDirectory = __SOURCE_DIRECTORY__ + "//..//TestCases//FunctionalStructures//SubstringSearching"

    [<TestCase("Input00.txt", [|"YES"; "NO"; "YES"; "YES"|])>]
    [<TestCase("Input01.txt", [|"YES"; "NO"; "YES"; "YES"; "NO"; "NO"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input02.txt", [|"YES"; "YES"; "YES"; "NO"; "NO"; "NO"; "YES"; "YES"; "YES"; "NO"|])>]
    [<TestCase("Input03.txt", [|"NO"; "YES"; "YES"; "NO"; "NO"; "YES"; "NO"|])>]
    [<TestCase("Input04.txt", [|"NO"; "NO"; "YES"; "NO"; "NO"; "NO"; "NO"; "YES"|])>]
    [<TestCase("Input05.txt", [|"YES"; "NO"; "YES"; "YES"; "NO"; "NO"; "NO"; "NO"; "YES"; "YES"|])>]
    [<TestCase("Input06.txt", [|"YES"; "NO"; "NO"; "YES"; "YES"; "NO"; "YES"; "YES"|])>]
    [<TestCase("Input07.txt", [|"NO"; "NO"; "YES"; "YES"; "YES"; "NO"; "YES"; "NO"; "NO"|])>]
    [<TestCase("Input08.txt", [|"YES"; "YES"; "YES"; "NO"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input09.txt", [|"NO"; "YES"; "YES"; "YES"; "YES"; "YES"; "YES"; "YES"|])>]
    [<TestCase("Input10.txt", [|"NO"; "NO"; "NO"; "NO"; "YES"; "NO"; "YES"; "NO"|])>]
    [<TestCase("Input11.txt", [|"NO"; "YES"; "NO"; "YES"; "NO"; "YES"; "NO"; "YES"|])>]
    [<TestCase("Input12.txt", [|"NO"; "NO"; "NO"; "YES"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"|])>]
    [<TestCase("Input13.txt", [|"NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"|])>]
    [<TestCase("Input14.txt", [|"NO"; "NO"; "NO"; "NO"; "YES"; "NO"; "NO"; "NO"; "NO"; "NO"|])>]
    [<TestCase("Input15.txt", [|"NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"; "NO"|])>]
    member public _.Execute(inputFile: string, expectedOutput: string[]) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new SubstringSearchingTask(reader, writer) :> ITask), input, expectedOutput)