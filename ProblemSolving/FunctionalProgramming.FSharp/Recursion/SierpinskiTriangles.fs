// from https://www.hackerrank.com/challenges/functions-and-fractals-sierpinski-triangles

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type Point = {x: float; y: float}

type Triangle = {a: Point; b: Point; c: Point}

type SierpinskiTrianglesTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let RowCount = 32

    [<Literal>]
    let ColumnCount = 63

    [<Literal>]
    let EmptyChar = '_'

    [<Literal>]
    let TriangleChar = '1'

    let scaleX = 1.0 / (ColumnCount |> float)

    let scaleY = 1.0 / (RowCount |> float)

    let centerX = 0.5 * scaleX

    let centerY = 0.5 * scaleY

    let split {Triangle.a = vertexA; Triangle.b = vertexB; Triangle.c = vertexC} =
        let middleAB = {Point.x = 0.5 * (vertexA.x + vertexB.x); Point.y = 0.5 * (vertexA.y + vertexB.y)}
        let middleBC = {Point.x = 0.5 * (vertexB.x + vertexC.x); Point.y = 0.5 * (vertexB.y + vertexC.y)}
        let middleCA = {Point.x = 0.5 * (vertexC.x + vertexA.x); Point.y = 0.5 * (vertexC.y + vertexA.y)}
        [{Triangle.a = vertexA; Triangle.b = middleAB; Triangle.c = middleCA};
         {Triangle.a = middleAB; Triangle.b = vertexB; Triangle.c = middleBC};
         {Triangle.a = middleCA; Triangle.b = middleBC; Triangle.c = vertexC}]

    let buildFractal (initTriangle: Triangle) (iterationCount: int) =
        let rec buildFractalImpl (triangles: Triangle list) (iteration: int) =
            match iteration with
            | _ when iteration = iterationCount -> triangles
            | _ ->
                let newTriangles = triangles |> List.collect split
                buildFractalImpl newTriangles (iteration + 1)
        buildFractalImpl [initTriangle] 0

    let isPointInsideTriangle {Point.x = pointX; Point.y = pointY} {Triangle.a = vertexA; Triangle.b = vertexB; Triangle.c = vertexC} =
        let value1 = (pointX - vertexA.x) * (vertexB.y - vertexA.y) - (pointY - vertexA.y) * (vertexB.x - vertexA.x)
        let value2 = (pointX - vertexB.x) * (vertexC.y - vertexB.y) - (pointY - vertexB.y) * (vertexC.x - vertexB.x)
        let value3 = (pointX - vertexC.x) * (vertexA.y - vertexC.y) - (pointY - vertexC.y) * (vertexA.x - vertexC.x)
        (value1 < 0.0) && (value2 < 0.0) && (value3 < 0.0)

    let prepareTriangle (field: char[,]) {Triangle.a = vertexA; Triangle.b = vertexB; Triangle.c = vertexC} =
        let xMin = vertexA.x
        let xMax = vertexC.x
        let yMin = vertexB.y
        let yMax = vertexA.y
        let xMinIndex = (ColumnCount |> float) * xMin  |> int
        let xMaxIndex = if (xMax = 1.0) then (ColumnCount - 1) else ((ColumnCount |> float) * xMax  |> int)
        let yMinIndex = (RowCount |> float) * yMin  |> int
        let yMaxIndex = if (yMax = 1.0) then (RowCount - 1) else ((float RowCount) * yMax  |> int)
        let cells = [for xIndex in xMinIndex .. xMaxIndex do
                     for yIndex in yMinIndex .. yMaxIndex do
                         yield (xIndex, yIndex)]
        let processFieldCell (cellXIndex: int, cellYIndex: int) =
            let cellX = (float cellXIndex) * scaleX
            let cellY = (float cellYIndex) * scaleY
            let cellCenterX = cellX + centerX
            let cellCenterY = cellY + centerY
            match isPointInsideTriangle {Point.x = cellCenterX; Point.y = cellCenterY} {Triangle.a = vertexA; Triangle.b = vertexB; Triangle.c = vertexC} with
            | true -> field.[cellYIndex, cellXIndex] <- TriangleChar
            | false -> ()
        cells |> List.iter processFieldCell

    let showResult (field: char[,]) =
        let maxColumn = ColumnCount - 1
        let showItem _ (column: int) (value: char) =
            value |> output.Write
            if (column = maxColumn) then
                "" |> output.WriteLine
        field |> Array2D.iteri showItem

    interface ITask with
        member this.Execute(argv: string[]) =
            let iterationCount = input.ReadLine() |> int
            // X axis: from left to right, Y axis: from top to down => x = 0, y = 0 - top left corner
            let initTriangle = {Triangle.a = {Point.x = 0.0; Point.y = 1.0}; Triangle.b = {Point.x = 0.5; Point.y = 0.0}; Triangle.c = {Point.x = 1.0; Point.y = 1.0}}
            let triangles = buildFractal initTriangle iterationCount
            let field = Array2D.create RowCount ColumnCount EmptyChar
            triangles |> List.iter (prepareTriangle field)
            showResult field
            0

[<TestFixture>]
type SierpinskiTrianglesTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//SierpinskiTriangles"

    [<TestCase("0", "Output00.txt")>]
    [<TestCase("1", "Output01.txt")>]
    [<TestCase("2", "Output02.txt")>]
    [<TestCase("5", "Output03.txt")>]
    [<TestCase("4", "Output04.txt")>]
    [<TestCase("3", "Output05.txt")>]
    member public this.Execute(input: string, expectedOutputFile: string) =
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new SierpinskiTrianglesTask(reader, writer) :> ITask), input, expectedOutput)