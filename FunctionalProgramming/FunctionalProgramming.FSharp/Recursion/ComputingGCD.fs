// from https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---gcd

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

type ComputingGCDTask(input: TextReader, output: TextWriter) =

    let rec calcGCD (a: int) (b: int) =
        match a with
        | _ when a = b -> a
        | _ when a > b -> calcGCD (a - b) b
        | _ when a < b -> calcGCD a (b - a)
        | _ -> failwith "Unexpected branch of match expression"

    interface ITask with
        member this.Execute(argv: string[]) =
            let numbers = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            let a = numbers.[0]
            let b = numbers.[1]
            (calcGCD a b) |> output.WriteLine
            0

[<TestFixture>]
type ComputingGCDTests() =

    [<TestCase("2 3", "1")>]
    [<TestCase("10000 12345", "5")>]
    [<TestCase("1000000 999995", "5")>]
    [<TestCase("1000000 999994", "2")>]
    [<TestCase("88886 474747", "7")>]
    [<TestCase("151 139", "1")>]
    [<TestCase("49 77777", "7")>]
    [<TestCase("14456 143", "13")>]
    [<TestCase("155667 54321", "57")>]
    [<TestCase("24323 53245", "1")>]
    member public this.Execute(input: string, expectedOutput: string) =
        TaskExecuter.Execute((fun reader writer -> new ComputingGCDTask(reader, writer) :> ITask), input, expectedOutput)