// from https://www.hackerrank.com/challenges/fractal-trees

module RecursiveTreesModule

open System

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

let duplicate count element =
    List.init count (fun _ -> element)

let build_vertical_segment size storage =
    match storage with
    | [] -> [ColumnStart - 1] |> duplicate size
    | head :: _ -> storage |> List.append (head |> duplicate size)

let build_slanting_segments size storage =
    storage |> List.append [for delta in size .. -1 .. 1 -> storage |>  List.head |> List.collect (fun column -> [column - delta; column + delta])]

let rec build_tree level count size storage =
    match level with
    | _ when level > count -> storage
    | _ ->
        storage |> build_vertical_segment size |> build_slanting_segments size |> build_tree (level + 1) count (size / 2)

let build_tree_start count =
    build_tree 1 count SegmentStartSize []
    
let show_empty_lines storage =
    seq {1 .. (RowCount - List.length storage)} |> Seq.iter (fun _ -> printfn "%s" (new String(EmptyChar, ColumnCount)))
    storage

let rec show_line index columns =
    match index with
    | ColumnCount -> printfn ""
    | _ ->
        match columns with
        | [] -> printf "%c" EmptyChar; show_line (index + 1) columns
        | column :: rest when index = column -> printf "%c" LineChar; show_line (index + 1) rest
        | _ -> printf "%c" EmptyChar; show_line (index + 1) columns

let show_storage storage =
    storage |> List.iter (fun columns -> show_line 0 columns)

[<EntryPoint>]
let main argv =
    Console.ReadLine() |> int |> build_tree_start |> show_empty_lines |> show_storage
    0 // return an integer exit code