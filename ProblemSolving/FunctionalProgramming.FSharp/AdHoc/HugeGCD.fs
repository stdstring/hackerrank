// from https://www.hackerrank.com/challenges/huge-gcd-fp

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type HugeGCDTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let ModValue = 1000000007L

    [<Literal>]
    let MaxFactor = 10000

    let generatePrimeFactors (maxNumber: int) =
        let storage = (maxNumber + 1) |> Array.zeroCreate
        let rec generateImpl (number: int) (factor: int) =
            match (factor * factor) <= number with
            | true ->
                match number % factor with
                | 0 -> factor :: storage.[number / factor]
                | _ -> (factor + 2) |> generateImpl number
            | false -> [number]
        storage.[1] <- []
        storage.[2] <- [2]
        for number in 3 .. maxNumber do
            match number % 2 with
            | 0 -> storage.[number] <- 2 :: storage.[number / 2]
            | _ -> storage.[number] <- 3 |> generateImpl number
        storage

    let generatePrimeFactorization (maxFactor: int) (primeFactors: int list []) (numberFactors: int seq) =
        let storage = (maxFactor + 1) |> Array.zeroCreate
        for factor in numberFactors do
            primeFactors.[factor] |> Seq.iter (fun prime -> storage.[prime] <- storage.[prime] + 1)
        storage

    let calcGCD (maxFactor: int) (leftFactorization: int[]) (rightFactorization: int[]) =
        let multValues (factor1: int) (factor2: int) =
            (factor1 |> int64) * (factor2 |> int64) % ModValue |> int
        let rec multFactor (factor: int) (power: int) (gcd: int) =
            match power with
            | 0 -> gcd
            | 1 -> multValues gcd factor
            | _ -> multValues gcd factor |> multFactor factor (power - 1)
        let rec calcImpl (factor: int) (gcd: int) =
            match factor with
            | _ when factor > maxFactor -> gcd
            | _ -> gcd |> multFactor factor (min leftFactorization.[factor] rightFactorization.[factor]) |> calcImpl (factor + 2)
        1 |> multFactor 2 (min leftFactorization.[2] rightFactorization.[2]) |> calcImpl 3

    interface ITask with
        member this.Execute(argv: string[]) =
            let primeFactors = MaxFactor |> generatePrimeFactors
            // n
            input.ReadLine() |> ignore
            let factorizationA = input.ReadLine().Split(" ") |> Seq.map int |> generatePrimeFactorization MaxFactor primeFactors
            // m
            input.ReadLine() |> ignore
            let factorizationB = input.ReadLine().Split(" ") |> Seq.map int  |> generatePrimeFactorization MaxFactor primeFactors
            calcGCD MaxFactor factorizationA factorizationB |> output.WriteLine
            0

[<TestFixture>]
type HugeGCDTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//AdHoc//HugeGCD"

    [<TestCase("Input00.txt", "60")>]
    [<TestCase("Input01.txt", "1")>]
    [<TestCase("Input02.txt", "120")>]
    [<TestCase("Input03.txt", "167103973")>]
    [<TestCase("Input04.txt", "513117836")>]
    [<TestCase("Input05.txt", "427504674")>]
    [<TestCase("Input06.txt", "444396968")>]
    [<TestCase("Input07.txt", "47307555")>]
    [<TestCase("Input08.txt", "108263989")>]
    [<TestCase("Input09.txt", "297788473")>]
    [<TestCase("Input10.txt", "391944154")>]
    [<TestCase("Input11.txt", "109297038")>]
    [<TestCase("Input12.txt", "609134417")>]
    [<TestCase("Input13.txt", "809388299")>]
    [<TestCase("Input14.txt", "10")>]
    member public this.Execute(inputFile: string, expectedOutput: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new HugeGCDTask(reader, writer) :> ITask), input, expectedOutput)