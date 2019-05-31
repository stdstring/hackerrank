// from https://www.hackerrank.com/challenges/captain-prime

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.Collections
open System.IO

type CaptainPrimeTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let MaxNumber = 1000000

    [<Literal>]
    let MaxStartNumber = 999

    let createSieve () =
        let sieve = new BitArray(MaxNumber - 1, true)
        let rec fillSieve number delta =
            match number with
            | _ when number > MaxNumber -> ()
            | _ ->
                sieve.[number - 2] <- false
                fillSieve (number + delta) delta
        fillSieve 4 2
        seq {3..2..MaxStartNumber} |> Seq.filter (fun number -> sieve.[number - 2]) |> Seq.iter (fun number -> fillSieve (number * number) number)
        sieve

    let findDivider number =
        let rec findDividerImpl number divider =
            let newDivider = 10 * divider
            match newDivider with
            | _ when newDivider > number -> divider
            | _ -> findDividerImpl number newDivider
        findDividerImpl number 1

    let checkLeft (sieve : BitArray) number =
        let divider = findDivider number
        let rec checkLeftImpl number divider =
            match number with
            | 0 -> true
            | 1 -> false
            | _ when sieve.[number - 2] ->
                let digit = number / divider
                match digit with
                | 0 -> false
                | _ -> checkLeftImpl (number % divider) (divider / 10)
            | _ -> false
        checkLeftImpl number divider

    let rec checkRight (sieve : BitArray) number =
        match number with
        | 0 -> true
        | 1 -> false
        | _ when sieve.[number - 2] ->
            let digit = number % 10
            match digit with
            | 0 -> false
            | _ -> checkRight sieve (number / 10)
        | _ -> false

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let numbers = [for _ in 1 .. count -> input.ReadLine() |> int]
            let sieve = createSieve ()
            let mapFun number =
                let left = checkLeft sieve number
                let right = checkRight sieve number
                match (left, right) with
                | (true, true) -> "CENTRAL"
                | (true, false) -> "LEFT"
                | (false, true) -> "RIGHT"
                | (false, false) -> "DEAD"
            let result = numbers |> List.map mapFun
            result |> List.iter (fun value -> value |> output.WriteLine)
            0

[<TestFixture>]
type CaptainPrimeTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//CaptainPrime"

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
    [<TestCase("Input16.txt", "Output16.txt")>]
    [<TestCase("Input17.txt", "Output17.txt")>]
    [<TestCase("Input18.txt", "Output18.txt")>]
    [<TestCase("Input19.txt", "Output19.txt")>]
    [<TestCase("Input20.txt", "Output20.txt")>]
    [<TestCase("Input21.txt", "Output21.txt")>]
    [<TestCase("Input22.txt", "Output22.txt")>]
    [<TestCase("Input23.txt", "Output23.txt")>]
    [<TestCase("Input24.txt", "Output24.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new CaptainPrimeTask(reader, writer) :> ITask), input, expectedOutput)