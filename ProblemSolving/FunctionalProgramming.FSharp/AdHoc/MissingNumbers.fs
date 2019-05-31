// https://www.hackerrank.com/challenges/missing-numbers-fp

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.Collections.Generic
open System.IO

type MissingNumbersTask(input: TextReader, output: TextWriter) =

    interface ITask with
        member this.Execute(argv: string[]) =
            input.ReadLine() |> int |> ignore // firstCount
            let firstNumbers = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            input.ReadLine() |> int |> ignore // secondCount
            let secondNumbers = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            let storage = new Dictionary<int, int>()
            let firstIterFun number =
                match storage.ContainsKey(number) with
                | true -> storage.[number] <- storage.[number] + 1
                | false -> storage.[number] <- 1
            let secondIterFun number =
                match storage.ContainsKey(number) with
                | true -> storage.[number] <- storage.[number] - 1
                | false -> storage.[number] <- -1
            firstNumbers |> Array.iter firstIterFun
            secondNumbers |> Array.iter secondIterFun
            storage |> Seq.filter (fun (pair : KeyValuePair<int, int>) -> pair.Value < 0) |> Seq.map (fun (pair : KeyValuePair<int, int>) -> pair.Key) |> Seq.sort |> Seq.iter (fun number -> output.Write("{0} ", number))
            0

[<TestFixture>]
type MissingNumbersTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//MissingNumbers"

    [<TestCase("Input00.txt", "204 205 206")>]
    [<TestCase("Input01.txt", "3670 3674 3677 3684 3685 3695 3714 3720")>]
    [<TestCase("Input02.txt", "8587 8609")>]
    [<TestCase("Input03.txt", "2437 2438 2442 2444 2447 2451 2457 2458 2466 2473 2479 2483 2488 2489 2510 2515 2517 2518")>]
    member public this.Execute(inputFile: string, expectedOutput: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new MissingNumbersTask(reader, writer) :> ITask), input, expectedOutput)