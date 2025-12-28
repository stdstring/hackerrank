// from https://www.hackerrank.com/challenges/area-under-curves-and-volume-of-revolving-a-curv

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type AreaVolumeCalculationTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let Interval = 0.001

    let calculateExpression (aCoeffs : float[]) (bCoeffs : int[]) value =
        let foldFun result index =
            let a = aCoeffs.[index]
            let b = bCoeffs.[index]
            result + a * (pown value b)
        {0 .. Array.length aCoeffs - 1} |> Seq.fold foldFun 0.0

    let calculate left right aCoeffs bCoeffs =
        let rec calculateImpl current currentExpr (area, volume) =
            match current with
            | _ when current >= right -> area, volume
            | _ ->
                let next = current + Interval
                let nextExpr = calculateExpression aCoeffs bCoeffs next
                let middleExpr = (currentExpr + nextExpr) / 2.0
                let areaDelta = middleExpr * Interval
                let volumeDelta = System.Math.PI * middleExpr * middleExpr * Interval
                calculateImpl next nextExpr (area + areaDelta, volume + volumeDelta)
        let leftExpr = calculateExpression aCoeffs bCoeffs left
        calculateImpl left leftExpr (0.0, 0.0)

    interface ITask with
        member this.Execute(argv: string[]) =
            let aCoeffs = input.ReadLine().Split(' ') |> Array.map (fun value -> float value)
            let bCoeffs = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            let borders = input.ReadLine().Split(' ') |> Array.map (fun value -> float value)
            let left = borders.[0]
            let right = borders.[1]
            let area, volume = calculate left right aCoeffs bCoeffs
            output.WriteLine("{0:f}", area)
            output.WriteLine("{0:f}", volume)
            0

[<TestFixture>]
type AreaVolumeCalculationTests() =

    [<Literal>]
    let MaxRelativeError = 0.01

    [<TestCase([|"1 2"; "0 1"; "2 20"|], [|414.0; 36024.1|])>]
    [<TestCase([|"1 2 3 4"; "0 1 2 3"; "1 10"|], [|11108.2; 86142470.4|])>]
    [<TestCase([|"1 2 3 4 5 6 7 8"; "-1 -2 -3 -4 1 2 3 4"; "1 2"|], [|101.4; 41193.0|])>]
    [<TestCase([|"-1 2 0 2 -1 -3 -4 -1 -3 -4 -999 1 2 3 4 5"; "-15 -14 -13 -12 -11 -10 -9 -8 -7 -6 -5 -4 -3 -2 -1 0"; "1 10"|], [|-193.1; 336642.8|])>]
    [<TestCase([|"-1 2 0 2 -1 -3 -4 -1 -3 -4 -999 1 2 3 4 5 1 2 0 2 -1 -3 -4 -1 -3 -4 -999 1 2 3 4 5"; "-16 -15 -14 -13 -12 -11 -10 -9 -8 -7 -6 -5 -4 -3 -2 -1 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15"; "1 2"|],
               [|-152853.7; 196838966733.0|])>]
    member public this.Execute(input: string[], expectedOutput: float[]) =
        let valueChecker (expectedValue: float) (actualValue: string) =
            let actualValue = actualValue |> System.Double.Parse
            let relativeError = (expectedValue - actualValue) / expectedValue |> abs
            Assert.That(relativeError, Is.LessThanOrEqualTo(MaxRelativeError))
        TaskExecutor.Execute((fun reader writer -> new AreaVolumeCalculationTask(reader, writer) :> ITask), input, expectedOutput, valueChecker)
