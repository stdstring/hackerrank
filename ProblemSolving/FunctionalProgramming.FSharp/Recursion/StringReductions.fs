// from https://www.hackerrank.com/challenges/string-reductions

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO
open System.Text

type StringReductionsTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let AlphabetSize = 26

    interface ITask with
        member this.Execute(argv: string[]) =
            let source = input.ReadLine()
            let storage = new StringBuilder(AlphabetSize)
            let foldFun (acc : Set<char>) (ch : char) =
                match ch with
                | _ when acc.Contains(ch) -> acc
                | _ ->
                    storage.Append(ch) |> ignore
                    acc.Add(ch)
            source |> Seq.fold foldFun (new Set<char>(Seq.empty)) |> ignore
            storage.ToString() |> output.WriteLine
            0

[<TestFixture>]
type StringReductionsTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//StringReductions"

    [<TestCase("Input00.txt", "acb")>]
    [<TestCase("Input01.txt", "abc")>]
    [<TestCase("Input02.txt", "prq")>]
    [<TestCase("Input03.txt", "zoxpwthuv")>]
    [<TestCase("Input04.txt", "cgvfpktjnwzxrseybohiaqdl")>]
    [<TestCase("Input05.txt", "vfsxqldtiypmcuzobhneawjrk")>]
    [<TestCase("Input06.txt", "kszigovumbpdnfrhyxwlteqjac")>]
    [<TestCase("Input07.txt", "cjmqlsfbrkadpzxiwteygvunoh")>]
    [<TestCase("Input08.txt", "pvjgmxczruaibsnoktydqwefhl")>]
    [<TestCase("Input09.txt", "xcdrhzyvomasjuetwiplbqkgfn")>]
    [<TestCase("Input10.txt", "samdbguypjoerhvfnkxctlqizw")>]
    [<TestCase("Input11.txt", "fyrgopdbuwilcksjztmnxeahqv")>]
    [<TestCase("Input12.txt", "z")>]
    [<TestCase("Input13.txt", "ab")>]
    [<TestCase("Input14.txt", "qrp")>]
    [<TestCase("Input15.txt", "abcdefghijklmnopqrstuvwxyz")>]
    [<TestCase("Input16.txt", "abcdefghijklmnopqrstuvwxyz")>]
    [<TestCase("Input17.txt", "abcdefghijklmnopqrstuvwxyz")>]
    [<TestCase("Input18.txt", "abcdefghijklmnopqrstuvwxyz")>]
    [<TestCase("Input19.txt", "zyxwvutsrqponmlkjihgfedcba")>]
    [<TestCase("Input20.txt", "q")>]
    member public this.Execute(inputFile: string, expectedOutput: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new StringReductionsTask(reader, writer) :> ITask), input, expectedOutput)