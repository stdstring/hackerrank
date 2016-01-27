// from https://www.hackerrank.com/challenges/pascals-triangle

module PascalTriangleModule

open System

let create_row (triangle : int[][]) rowIndex =
    let prevRow = triangle.[rowIndex - 1]
    let row = [| for column in 0 .. rowIndex do
                     match column with
                     | 0 -> yield 1
                     | _ when column = rowIndex -> yield 1
                     | _ -> yield prevRow.[column - 1] + prevRow.[column] |]
    triangle.[rowIndex] <- row
    triangle

let show_row (row : int[]) =
    let show_row_impl =
        function
        | 0 -> row.[0] |> printf "%d"
        | index -> row.[index] |> printf " %d"
    {0 .. row.Length - 1} |> Seq.iter show_row_impl
    printfn ""

[<EntryPoint>]
let main argv = 
    let rowCount = Console.ReadLine() |> int
    let triangle = [| |] |> Array.create rowCount
    triangle.[0] <- [| 1 |]
    let triangle = {1 .. rowCount - 1} |> Seq.fold create_row triangle
    {0 .. rowCount - 1} |> Seq.iter (fun index -> triangle.[index] |> show_row)
    0 // return an integer exit code