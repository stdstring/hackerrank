// from https://www.hackerrank.com/challenges/fractal-trees

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type RecursiveTreesTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let RowCount = 63

    [<Literal>]
    let ColumnCount = 100

    [<Literal>]
    let EmptyChar = '_'

    [<Literal>]
    let LineChar = '1'

    [<Literal>]
    let ColumnStart = 50

    [<Literal>]
    let SegmentStartSize = 16

    let buildVerticalSegment (size: int) (storage: int list list) =
        match storage with
        | [] -> [ColumnStart - 1] |> List.replicate size
        | head :: _ -> storage |> List.append (head |> List.replicate size)

    let buildSlantingSegments (size: int) (storage: int list list) =
        storage |> List.append [for delta in size .. -1 .. 1 -> storage |>  List.head |> List.collect (fun column -> [column - delta; column + delta])]

    let rec buildTree (level: int) (count: int) (size: int) (storage: int list list) =
        match level with
        | _ when level > count -> storage
        | _ -> storage |> buildVerticalSegment size |> buildSlantingSegments size |> buildTree (level + 1) count (size / 2)

    let buildTreeStart (count: int) =
        buildTree 1 count SegmentStartSize []

    let showEmptyLines (storage: int list list) =
        seq {1 .. (RowCount - List.length storage)} |> Seq.iter (fun _ -> new System.String(EmptyChar, ColumnCount) |> output.WriteLine)
        storage

    let rec showLine (index: int) (columns: int list) =
        match index with
        | ColumnCount -> "" |> output.WriteLine
        | _ ->
            match columns with
            | [] -> EmptyChar |> output.Write; showLine (index + 1) columns
            | column :: rest when index = column -> LineChar |> output.Write; showLine (index + 1) rest
            | _ -> EmptyChar |> output.Write; showLine (index + 1) columns

    let showStorage (storage: int list list) =
        storage |> List.iter (fun columns -> showLine 0 columns)

    interface ITask with
        member this.Execute(argv: string[]) =
            input.ReadLine() |> int |> buildTreeStart |> showEmptyLines |> showStorage
            0

[<TestFixture>]
type RecursiveTreesTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//RecursiveTrees"

    [<TestCase("1", "Output00.txt")>]
    [<TestCase("2", "Output01.txt")>]
    [<TestCase("3", "Output02.txt")>]
    [<TestCase("4", "Output03.txt")>]
    [<TestCase("5", "Output04.txt")>]
    member public this.Execute(input: string, expectedOutputFile: string) =
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new RecursiveTreesTask(reader, writer) :> ITask), input, expectedOutput)