// from https://www.hackerrank.com/challenges/super-digit

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type SuperDigitTask(input: TextReader, output: TextWriter) =

    let toDigit =
        function
        | '0' -> 0UL
        | '1' -> 1UL
        | '2' -> 2UL
        | '3' -> 3UL
        | '4' -> 4UL
        | '5' -> 5UL
        | '6' -> 6UL
        | '7' -> 7UL
        | '8' -> 8UL
        | '9' -> 9UL
        | _ -> failwith "bad character"

    let getDigits (number : uint64) =
        let rec getDigitsImpl (number: uint64) (storage: uint64 list) =
            match number with
            | _ when number < 10UL -> number :: storage
            | _ ->
                let rest = number / 10UL
                let digit = number % 10UL
                digit :: storage |> getDigitsImpl rest 
        [] |> getDigitsImpl number

    let rec superdigit (number : uint64) =
        match number with
        | _ when number < 10UL -> number
        | _ -> number |> getDigits |> List.sum |> superdigit

    interface ITask with
        member this.Execute(argv: string[]) =
            let sourceData = input.ReadLine().Split(' ')
            let sourceNumber = sourceData.[0]
            let repeatNumber = sourceData.[1] |> uint64
            let initSum = (sourceNumber |> Seq.fold(fun acc digit -> acc + (digit |> toDigit)) 0UL) * repeatNumber
            let result = superdigit initSum
            result |> output.WriteLine
            0

[<TestFixture>]
type SuperDigitTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//SuperDigit"

    [<TestCase("Input00.txt", "3")>]
    [<TestCase("Input01.txt", "8")>]
    [<TestCase("Input02.txt", "8")>]
    [<TestCase("Input03.txt", "7")>]
    [<TestCase("Input04.txt", "3")>]
    [<TestCase("Input05.txt", "5")>]
    [<TestCase("Input06.txt", "3")>]
    [<TestCase("Input07.txt", "7")>]
    [<TestCase("Input08.txt", "4")>]
    [<TestCase("Input09.txt", "9")>]
    member public this.Execute(inputFile: string, expectedOutput: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new SuperDigitTask(reader, writer) :> ITask), input, expectedOutput)