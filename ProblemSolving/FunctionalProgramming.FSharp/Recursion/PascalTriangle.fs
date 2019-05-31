// from https://www.hackerrank.com/challenges/pascals-triangle

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type PascalTriangleTask(input: TextReader, output: TextWriter) =

    let createRow (triangle : int[][]) (rowIndex: int) =
        let prevRow = triangle.[rowIndex - 1]
        let row = [| for column in 0 .. rowIndex do
                        match column with
                        | 0 -> yield 1
                        | _ when column = rowIndex -> yield 1
                        | _ -> yield prevRow.[column - 1] + prevRow.[column] |]
        triangle.[rowIndex] <- row
        triangle

    let showRow (row : int[]) =
        {0 .. row.Length - 1} |> Seq.iter (fun columnIndex -> output.Write("{0} ", row.[columnIndex]))
        "" |> output.WriteLine

    interface ITask with
        member this.Execute(argv: string[]) =
            let rowCount = input.ReadLine() |> int
            let triangle = [| |] |> Array.create rowCount
            triangle.[0] <- [| 1 |]
            let triangle = {1 .. rowCount - 1} |> Seq.fold createRow triangle
            {0 .. rowCount - 1} |> Seq.iter (fun index -> triangle.[index] |> showRow)
            0

[<TestFixture>]
type PascalTriangleTests() =

    [<TestCase([|"2"|], [|"1"; "1 1"|])>]
    [<TestCase([|"4"|], [|"1"; "1 1"; "1 2 1"; "1 3 3 1"|])>]
    [<TestCase([|"6"|], [|"1"; "1 1"; "1 2 1"; "1 3 3 1"; "1 4 6 4 1"; "1 5 10 10 5 1"|])>]
    [<TestCase([|"7"|], [|"1"; "1 1"; "1 2 1"; "1 3 3 1"; "1 4 6 4 1"; "1 5 10 10 5 1"; "1 6 15 20 15 6 1"|])>]
    [<TestCase([|"10"|], [|"1"; "1 1"; "1 2 1"; "1 3 3 1"; "1 4 6 4 1"; "1 5 10 10 5 1"; "1 6 15 20 15 6 1"; "1 7 21 35 35 21 7 1"; "1 8 28 56 70 56 28 8 1"; "1 9 36 84 126 126 84 36 9 1"|])>]
    member public this.Execute(input: string[], expectedOutput: string[]) =
        TaskExecutor.Execute((fun reader writer -> new PascalTriangleTask(reader, writer) :> ITask), input, expectedOutput)