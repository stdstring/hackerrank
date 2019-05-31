// from https://www.hackerrank.com/challenges/remove-duplicates

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.IO
open System.Text

type RemoveDuplicatesTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let AlphabetSize = 26

    [<Literal>]
    let AlphabetStart = 'a'

    interface ITask with
        member this.Execute(argv: string[]) =
            let source = input.ReadLine()
            let usageArray = Array.create AlphabetSize false
            let indexStart = (int AlphabetStart)
            let foldFun (dest : StringBuilder) (ch : char) =
                let index = (int ch) - indexStart
                match usageArray.[index] with
                | false ->
                    usageArray.[index] <- true
                    dest.Append(ch)
                | true -> dest
            let dest= source |> Seq.fold foldFun (new StringBuilder(AlphabetSize))
            dest.ToString() |> output.WriteLine
            0

[<TestFixture>]
type RemoveDuplicatesTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//RemoveDuplicates"

    [<TestCase("Input00.txt", "abc")>]
    [<TestCase("Input01.txt", "cba")>]
    [<TestCase("Input02.txt", "hskimnjwlqfdvoaegxctryupzb")>]
    [<TestCase("Input03.txt", "yhikensubgtjxrozpwmvcfladq")>]
    [<TestCase("Input04.txt", "joizycdawreptusbvgxfhnqlkm")>]
    [<TestCase("Input05.txt", "nwlrbmqhcdazokyisxjfegpuvt")>]
    [<TestCase("Input06.txt", "twjnvbckdmeouzhygqxpiraslf")>]
    [<TestCase("Input07.txt", "dnrcuzaktxjsgmboyhpeiwfqlv")>]
    [<TestCase("Input08.txt", "ksgzeljqxvmdhuwrapfiytnocb")>]
    member public this.Execute(inputFile: string, expectedOutput: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new RemoveDuplicatesTask(reader, writer) :> ITask), input, expectedOutput)