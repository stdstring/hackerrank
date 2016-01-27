// from https://www.hackerrank.com/challenges/functions-and-fractals-sierpinski-triangles

module SierpinskiTrianglesModule

open System

[<Literal>]
let RowCount = 32

[<Literal>]
let ColumnCount = 63

[<Literal>]
let EmptyChar = '_'

[<Literal>]
let TriangleChar = '1'

let scaleX = 1.0 / (float ColumnCount)

let scaleY = 1.0 / (float RowCount)

let centerX = 0.5 * scaleX

let centerY = 0.5 * scaleY

type Point = {x: float; y: float;}

type Triangle = {a: Point; b: Point; c: Point;}

let split {a = vertexA; b = vertexB; c = vertexC} =
    let middleAB = {x = 0.5 * (vertexA.x + vertexB.x); y = 0.5 * (vertexA.y + vertexB.y);}
    let middleBC = {x = 0.5 * (vertexB.x + vertexC.x); y = 0.5 * (vertexB.y + vertexC.y);}
    let middleCA = {x = 0.5 * (vertexC.x + vertexA.x); y = 0.5 * (vertexC.y + vertexA.y);}
    [{a = vertexA; b = middleAB; c = middleCA;};
     {a = middleAB; b = vertexB; c = middleBC;};
     {a = middleCA; b = middleBC; c = vertexC;}]
     
let build_fractal initTriangle iterationCount =
    let rec build_fractal_impl triangles iteration =
        match iteration with
        | _ when iteration = iterationCount -> triangles
        | _ ->
            let newTriangles = triangles |> List.collect split
            build_fractal_impl newTriangles (iteration + 1)
    build_fractal_impl [initTriangle] 0
    
let is_point_inside_triangle {x = pointX; y = pointY;} {a = vertexA; b = vertexB; c = vertexC;} =
    let value1 = (pointX - vertexA.x) * (vertexB.y - vertexA.y) - (pointY - vertexA.y) * (vertexB.x - vertexA.x)
    let value2 = (pointX - vertexB.x) * (vertexC.y - vertexB.y) - (pointY - vertexB.y) * (vertexC.x - vertexB.x)
    let value3 = (pointX - vertexC.x) * (vertexA.y - vertexC.y) - (pointY - vertexC.y) * (vertexA.x - vertexC.x)
    (value1 < 0.0) && (value2 < 0.0) && (value3 < 0.0)
    
let prepare_triangle (field: char[,]) {a = vertexA; b = vertexB; c = vertexC;} =
    let xMin = vertexA.x
    let xMax = vertexC.x
    let yMin = vertexB.y
    let yMax = vertexA.y
    let xMinIndex = (float ColumnCount) * xMin  |> int
    let xMaxIndex = if (xMax = 1.0) then (ColumnCount - 1) else ((float ColumnCount) * xMax  |> int)
    let yMinIndex = (float RowCount) * yMin  |> int
    let yMaxIndex = if (yMax = 1.0) then (RowCount - 1) else ((float RowCount) * yMax  |> int)
    let cells = [for xIndex in xMinIndex .. xMaxIndex do
                 for yIndex in yMinIndex .. yMaxIndex do
                     yield (xIndex, yIndex)]
    let process_field_cell (cellXIndex, cellYIndex) =
        let cellX = (float cellXIndex) * scaleX
        let cellY = (float cellYIndex) * scaleY
        let cellCenterX = cellX + centerX
        let cellCenterY = cellY + centerY
        match is_point_inside_triangle {x = cellCenterX; y = cellCenterY;} {a = vertexA; b = vertexB; c = vertexC;} with
        | true -> field.[cellYIndex, cellXIndex] <- TriangleChar
        | false -> ()
    cells |> List.iter process_field_cell

let show_result field =
    let maxColumn = ColumnCount - 1
    let show_item _ column value =
        printf "%c" value
        if (column = maxColumn) then
            printfn ""
    field |> Array2D.iteri show_item

[<EntryPoint>]
let main argv = 
    let iterationCount = Console.ReadLine() |> int
    // X axis: from left to right, Y axis: from top to down => x = 0, y = 0 - top left corner
    let initTriangle = {a = {x = 0.0; y = 1.0;}; b = {x = 0.5; y = 0.0;}; c = {x = 1.0; y = 1.0;}}
    let triangles = build_fractal initTriangle iterationCount
    let field = Array2D.create RowCount ColumnCount EmptyChar
    triangles |> List.iter (prepare_triangle field)
    show_result field
    0 // return an integer exit code