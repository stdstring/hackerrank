// from https://www.hackerrank.com/challenges/captain-prime

namespace FunctionalProgramming.FSharp.AdHoc

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type CommonDivisorsTask(input: TextReader, output: TextWriter) =

    let calcNumber (index: int) = 2 * index + 3

    let calcIndex (number: int) = (number - 3) / 2

    let calcStorageSize (maxNumber: int) =
        match maxNumber % 2 with
        | 0 -> (maxNumber / 2) - 1
        | _ -> maxNumber / 2

    let calcSmallestPrimeFactors (maxNumber: int) =
        let erasePrimeFactors (prime: int64) (maxNumber: int64) (data: int[]) =
            let mutable number = prime * prime
            let delta =  2L * prime
            while number <= maxNumber do
                let index = number |> int |> calcIndex
                if data.[index] = 0 then
                    data.[index] <- prime |> int
                number <- number + delta
        let generate (maxNumber: int64) (data: int[]) =
            for index = 0 to data.Length - 1 do
                if data.[index] = 0 then
                    let prime = index |> calcNumber
                    data.[index] <- prime
                    data |> erasePrimeFactors (prime |> int64) maxNumber
        let data = maxNumber |> calcStorageSize |> Array.zeroCreate
        data |> generate (maxNumber |> int64)
        data

    let calcPrimeFactors (smallestPrimeFactors: int[]) (number: int) =
        let extractFactorPower (factor: int) (number: int) =
            let mutable power = 0
            let mutable rest = number
            while rest % factor = 0 do
                power <- power + 1
                rest <- rest / factor
            rest, power
        let rec calcImpl (number: int) (dest: (int * int) list) =
            let index = number |> calcIndex
            match smallestPrimeFactors.[index] with
            | factor when factor = number -> (factor, 1) :: dest
            | factor ->
                match number |> extractFactorPower factor with
                | 1, power -> (factor, power) :: dest
                | rest, power -> (factor, power) :: dest |> calcImpl rest
        match number with
        | 1 -> []
        | _ ->
            match number |> extractFactorPower 2 with
            | 1, power2 -> [2, power2]
            | rest, power2 -> (if power2 > 0 then [2, power2] else []) |> calcImpl rest |> List.sortBy (fun (factor, _) -> factor)

    let calcCommonPrimeFactors (left: (int * int) list) (right: (int * int) list) =
        let rec calcImpl (left: (int * int) list) (right: (int * int) list) (dest: (int * int) list) =
            match left, right with
            | [], _ -> dest |> List.rev
            | _, [] -> dest |> List.rev
            | (factor1, pow1) :: leftRest, (factor2, pow2) :: rightRest when factor1 = factor2 -> (factor1, min pow1 pow2) :: dest |> calcImpl leftRest rightRest
            | (factor1, _) :: leftRest, (factor2, _) :: _ when factor1 < factor2 -> dest |> calcImpl leftRest right
            | (factor1, _) :: _, (factor2, _) :: rightRest when factor1 > factor2 -> dest |> calcImpl left rightRest
            | _ -> failwith "Unexpected branch of match expression"
        [] |> calcImpl left right

    interface ITask with
        member this.Execute(argv: string[]) =
            let testCasesCount = input.ReadLine() |> int
            let testCases = seq {1 .. testCasesCount} |> Seq.map (fun _ -> input.ReadLine().Split(" ") |> Seq.map int |> Seq.toList) |> Seq.toList
            let maxNumber = testCases |> Seq.map (fun [n1; n2] -> max n1 n2) |> Seq.max
            let smallestPrimeFactors = maxNumber |> calcSmallestPrimeFactors
            for [number1; number2] in testCases do
                let primeFactor1 = number1 |> calcPrimeFactors smallestPrimeFactors
                let primeFactor2 = number2 |> calcPrimeFactors smallestPrimeFactors
                let commonPrimeFactors = calcCommonPrimeFactors primeFactor1 primeFactor2
                let commonDivisorsCount = commonPrimeFactors |> Seq.fold (fun result (_, power) -> result * (power + 1)) 1
                commonDivisorsCount |> output.WriteLine
            0

[<TestFixture>]
type CommonDivisorsTests() =

    [<TestCase("3,10 4,1 100,288 240", "2 1 10")>]
    [<TestCase("7,9 7,8 6,6 6,10 9,6 7,5 9,6 5", "1 2 4 1 1 1 1")>]
    [<TestCase("8,27 76,16 21,11 10,52 68,80 55,80 71,56 16,39 55", "1 1 1 3 2 1 4 1")>]
    [<TestCase("9,801 352,632 418,378 402,854 765,709 209,982 725,623 233,869 951,274 970", "1 2 4 1 1 1 1 1 2")>]
    [<TestCase("10,5991 5525,9098 4194,7203 7591,5975 3121,2929 5909,8822 7280,7953 4783,2352 5843,8002 9688,6731 8284",
               "1 2 1 1 1 2 1 1 2 1")>]
    [<TestCase("10,65836 50318,68387 67487,33081 35348,47085 90799,80035 28801,22827 79218,45253 49052,57576 51670,35552 95210,18257 40644",
               "2 1 1 1 1 2 1 2 2 1")>]
    [<TestCase("10,139474 262647,812895 341222,774230 720536,210624 356737,581513 466650,482598 459482,884257 748847,933146 156066,448595 325659,131311 712290",
               "1 1 2 1 2 2 2 2 1 1")>]
    [<TestCase("10,9195895 3521983,8545664 8724135,9145226 2327563,9359239 5515545,4469319 5478823,6869417 9859844,1689260 3468651,9987023 7598541,2266411 3459064,8963102 3653609",
               "1 1 1 1 1 1 1 1 2 1")>]
    [<TestCase("9,89308634 12272446,84597941 70051010,47474737 48610391,38256962 28437455,64191252 71362795,93328972 49514663,17903673 88339034,78026662 25957369,16118728 90363523",
               "2 1 1 1 1 2 1 1 1")>]
    [<TestCase("8,99999992 99999995,99999994 100000000,99999998 99999999,99999991 99999998,99999993 99999990,99999991 99999993,99999995 100000000,99999997 99999991",
               "1 2 1 2 2 1 2 1")>]
    [<TestCase("10,9699690 510510,9699690 9699690,5336100 9261000,100000000 100000000,40465425 26976950,67442375 80930850,87318000 43659000,62370000 44550000,1299827 1299821,93250113 93250113",
               "128 256 81 81 72 72 480 250 1 20")>]
    member public this.Execute(input: string, expectedOutput: string) =
        TaskExecutor.Execute((fun reader writer -> new CommonDivisorsTask(reader, writer) :> ITask), input.Split(","), expectedOutput.Split(' '))