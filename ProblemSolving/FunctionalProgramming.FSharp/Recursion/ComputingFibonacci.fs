// from https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---fibonacci-numbers

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type ComputingFibonacciTask(input: TextReader, output: TextWriter) =

    let calcFib: int -> int =
        function
        | 1 -> 0
        | 2 -> 1
        | n ->
            let rec calcFibImpl (number: int) (current: int) (prev: int) =
                match number with
                | _ when number = n -> current
                | _ -> calcFibImpl (number + 1) (current + prev) current
            calcFibImpl 2 1 0

    interface ITask with
        member this.Execute(argv: string[]) =
            input.ReadLine() |> int |> calcFib |> output.WriteLine
            0

[<TestFixture>]
type ComputingFibonacciTests() =

    [<TestCase("40", "63245986")>]
    [<TestCase("35", "5702887")>]
    [<TestCase("5", "3")>]
    [<TestCase("39", "39088169")>]
    member public this.Execute(input: string, expectedOutput: string) =
        TaskExecutor.Execute((fun reader writer -> new ComputingFibonacciTask(reader, writer) :> ITask), input, expectedOutput)