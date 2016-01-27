// from https://www.hackerrank.com/challenges/matrix-rotation

module MatrixRotationModule

open System

type Point = {row: int; column: int;}

let read_matrix_row (matrix : int[,]) rowIndex =
    let rowSource = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    rowSource |> Array.iteri (fun columnIndex number -> matrix.[rowIndex, columnIndex] <- number)

let get_circle (matrix : int[,]) topLeftPoint bottomRightPoint =
    [|for c in topLeftPoint.column .. (bottomRightPoint.column - 1) -> matrix.[topLeftPoint.row, c]
      for r in topLeftPoint.row .. (bottomRightPoint.row - 1) -> matrix.[r, bottomRightPoint.column]
      for c in bottomRightPoint.column .. -1 .. (topLeftPoint.column + 1) -> matrix.[bottomRightPoint.row, c]
      for r in bottomRightPoint.row .. -1 .. (topLeftPoint.row + 1) -> matrix.[r, topLeftPoint.column]|]

let rotate rotateCount (circle : int[]) =
    let circleSize = circle.Length
    match (rotateCount % circleSize) with
    | 0 -> circle
    | shift ->
        let newCircle = Array.zeroCreate circleSize
        Array.blit circle 0 newCircle (circleSize - shift) shift
        Array.blit circle shift newCircle 0 (circleSize - shift)
        newCircle

let set_circle (matrix : int[,]) topLeftPoint bottomRightPoint (circle : int[]) =
    let points = seq {for c in topLeftPoint.column .. (bottomRightPoint.column - 1) -> {row = topLeftPoint.row; column = c;}
                      for r in topLeftPoint.row .. (bottomRightPoint.row - 1) -> {row = r; column = bottomRightPoint.column;}
                      for c in bottomRightPoint.column .. -1 .. (topLeftPoint.column + 1) -> {row = bottomRightPoint.row; column = c;}
                      for r in bottomRightPoint.row .. -1 .. (topLeftPoint.row + 1) -> {row = r; column = topLeftPoint.column;}}
    points |> Seq.iteri (fun index {row = r; column = c} -> matrix.[r, c] <- circle.[index])

let (|ProcessFinish|) (bottomRightPoint : Point) (topLeftPoint : Point) =
    if (topLeftPoint.row > bottomRightPoint.row) || (topLeftPoint.column > bottomRightPoint.column) then
        true
    else
        false

let process_matrix matrix rowCount columnCount rotateCount =
    let rec process_matrix_impl matrix topLeftPoint bottomRightPoint =
        match topLeftPoint with
        | ProcessFinish bottomRightPoint true -> ()
        | _ ->
            get_circle matrix topLeftPoint bottomRightPoint |> rotate rotateCount |> set_circle matrix topLeftPoint bottomRightPoint
            process_matrix_impl matrix {row = topLeftPoint.row + 1; column = topLeftPoint.column + 1} {row = bottomRightPoint.row - 1; column = bottomRightPoint.column - 1}
    process_matrix_impl matrix {row = 0; column = 0;} {row = rowCount - 1; column = columnCount - 1;}

let show_matrix_row (matrix : int[,]) rowIndex columnCount =
    seq {0 .. columnCount - 1} |> Seq.iter (fun columnIndex -> matrix.[rowIndex, columnIndex] |> printf "%d ")
    printfn ""

[<EntryPoint>]
let main argv =
    let borders = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let rowCount = borders.[0]
    let columnCount = borders.[1]
    let rotateCount = borders.[2]
    let matrix = Array2D.zeroCreate rowCount columnCount
    seq {0 .. rowCount - 1} |> Seq.iter (fun rowIndex -> read_matrix_row matrix rowIndex)
    process_matrix matrix rowCount columnCount rotateCount
    seq {0 .. rowCount - 1} |> Seq.iter (fun rowIndex -> show_matrix_row matrix rowIndex columnCount)
    0 // return an integer exit code