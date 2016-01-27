// from https://www.hackerrank.com/challenges/fp-filter-array

module FilterArrayModule

open System

let read_input () =
    match Console.ReadLine() with
    | null -> None
    | value -> Some(value |> int, ())

[<EntryPoint>]
let main argv = 
    let topBorder = Console.ReadLine() |> int
    let input = Seq.unfold read_input ()
    let output = input |> Seq.filter (fun number -> number < topBorder) |> Seq.toList
    output |> List.iter (fun number -> printfn "%d" number)
    0 // return an integer exit code