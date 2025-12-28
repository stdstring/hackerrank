// from https://www.hackerrank.com/challenges/eval-ex

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type EvaluatingExpTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let TermCount = 10

    let readInput () =
        match input.ReadLine() with
        | null -> None
        | value -> Some(value |> float, ())

    let calculate (value : float) =
        let foldFun (result, prev) iteration =
            let next = value * prev / (float iteration)
            (result + next, next)
        let result, _ = seq {1 .. TermCount - 1} |> Seq.fold foldFun (1.0, 1.0)
        result

    interface ITask with
        member this.Execute(argv: string[]) =
            input.ReadLine() |> int |> ignore
            let inputData = Seq.unfold readInput ()
            let outputData = inputData |> Seq.map (fun value -> calculate value) |> Seq.toList
            outputData |> List.iter (fun result -> output.WriteLine("{0:f}", result))
            0

[<TestFixture>]
type EvaluatingExpTests() =

    [<Literal>]
    let MaxError = 0.1

    [<TestCase([|"2"; "0.0000"; "1.0000"|], [|1.0; 2.7183|])>]
    [<TestCase([|"4"; "1.2500"; "1.5000"; "2.5000"; "5.0000"|], [|3.4903; 4.4817; 12.1791; 143.6895|])>]
    [<TestCase([|"9"; "0.0250"; "0.0350"; "0.4500"; "4.5000"; "5.5000"; "6.5000"; "-1.0000"; "-2.0000"; "-3.0000"|], [|1.0253; 1.0356; 1.5683; 88.4785; 231.533; 583.5847; 0.3679; 0.1351; 0.0371|])>]
    [<TestCase([|"18"; "10.0250"; "10.0350"; "10.4500"; "14.5000"; "15.5000"; "16.5000"; "-1.2000"; "-2.2000"; "-3.2000"; "10.2500"; "15.0000"; "14.2500"; "14.1500"; "15.5200"; "16.5200"; "-1.2300"; "-2.2400"; "-3.2500"|],
               [|10271.3676; 10346.1443; 13922.7457; 174005.6875; 297459.918; 494326.1933; 0.3012; 0.1102; 0.0168; 12078.5782; 228352.8304; 151455.0816; 143195.8854; 300577.5476; 499238.4275; 0.2923; 0.1057; 0.0109|])>]
    // TODO (std_string) : think about TestCase 4
    [<TestCase([|"4"; "20.0000"; "5.0000"; "0.5000"; "-0.5000"|], [|2423600.1887; 143.6895; 1.6487; 0.6065|])>]
    member public this.Execute(input: string[], expectedOutput: float[]) =
        let valueChecker (expectedValue: float) (actualValue: string) =
            let actualValue = actualValue |> System.Double.Parse
            let absError = (expectedValue - actualValue) |> abs
            Assert.That(absError, Is.LessThanOrEqualTo(MaxError))
        TaskExecutor.Execute((fun reader writer -> new EvaluatingExpTask(reader, writer) :> ITask), input, expectedOutput, valueChecker)