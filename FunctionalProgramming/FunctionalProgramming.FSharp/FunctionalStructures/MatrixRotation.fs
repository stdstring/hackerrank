// from https://www.hackerrank.com/challenges/matrix-rotation

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

type Point = {row: int; column: int}

type MatrixRotationTask(input: TextReader, output: TextWriter) =

    let readMatrixRow (matrix : int[,]) (rowIndex: int) =
        let rowSource = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
        rowSource |> Array.iteri (fun columnIndex number -> matrix.[rowIndex, columnIndex] <- number)

    let getCircle (matrix : int[,]) (topLeftPoint: Point) (bottomRightPoint: Point) =
        [|for c in topLeftPoint.column .. (bottomRightPoint.column - 1) -> matrix.[topLeftPoint.row, c]
          for r in topLeftPoint.row .. (bottomRightPoint.row - 1) -> matrix.[r, bottomRightPoint.column]
          for c in bottomRightPoint.column .. -1 .. (topLeftPoint.column + 1) -> matrix.[bottomRightPoint.row, c]
          for r in bottomRightPoint.row .. -1 .. (topLeftPoint.row + 1) -> matrix.[r, topLeftPoint.column]|]

    let rotate (rotateCount: int) (circle : int[]) =
        let circleSize = circle.Length
        match (rotateCount % circleSize) with
        | 0 -> circle
        | shift ->
            let newCircle = Array.zeroCreate circleSize
            Array.blit circle 0 newCircle (circleSize - shift) shift
            Array.blit circle shift newCircle 0 (circleSize - shift)
            newCircle

    let setCircle (matrix : int[,]) (topLeftPoint: Point) (bottomRightPoint: Point) (circle : int[]) =
        let points = seq {for c in topLeftPoint.column .. (bottomRightPoint.column - 1) -> {row = topLeftPoint.row; column = c}
                          for r in topLeftPoint.row .. (bottomRightPoint.row - 1) -> {row = r; column = bottomRightPoint.column}
                          for c in bottomRightPoint.column .. -1 .. (topLeftPoint.column + 1) -> {row = bottomRightPoint.row; column = c}
                          for r in bottomRightPoint.row .. -1 .. (topLeftPoint.row + 1) -> {row = r; column = topLeftPoint.column}}
        points |> Seq.iteri (fun index {row = r; column = c} -> matrix.[r, c] <- circle.[index])

    let (|ProcessFinish|) (bottomRightPoint : Point) (topLeftPoint : Point) =
        if (topLeftPoint.row > bottomRightPoint.row) || (topLeftPoint.column > bottomRightPoint.column) then
            true
        else
            false

    let processMatrix (matrix: int[,]) (rowCount: int) (columnCount: int) (rotateCount: int) =
        let rec processMatrixImpl (matrix: int[,]) (topLeftPoint: Point) (bottomRightPoint: Point) =
            match topLeftPoint with
            | ProcessFinish bottomRightPoint true -> ()
            | _ ->
                getCircle matrix topLeftPoint bottomRightPoint |> rotate rotateCount |> setCircle matrix topLeftPoint bottomRightPoint
                processMatrixImpl matrix {row = topLeftPoint.row + 1; column = topLeftPoint.column + 1} {row = bottomRightPoint.row - 1; column = bottomRightPoint.column - 1}
        processMatrixImpl matrix {row = 0; column = 0} {row = rowCount - 1; column = columnCount - 1}

    let showMatrixRow (matrix : int[,]) (rowIndex: int) (columnCount: int) =
        seq {0 .. columnCount - 1} |> Seq.iter (fun columnIndex -> output.Write("{0} ", matrix.[rowIndex, columnIndex]))
        "" |> output.WriteLine

    interface ITask with
        member this.Execute(argv: string[]) =
            let borders = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            let rowCount = borders.[0]
            let columnCount = borders.[1]
            let rotateCount = borders.[2]
            let matrix = Array2D.zeroCreate rowCount columnCount
            seq {0 .. rowCount - 1} |> Seq.iter (fun rowIndex -> readMatrixRow matrix rowIndex)
            processMatrix matrix rowCount columnCount rotateCount
            seq {0 .. rowCount - 1} |> Seq.iter (fun rowIndex -> showMatrixRow matrix rowIndex columnCount)
            0

[<TestFixture>]
type MatrixRotationTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//FunctionalStructures//MatrixRotation"

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
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecuter.Execute((fun reader writer -> new MatrixRotationTask(reader, writer) :> ITask), input, expectedOutput)