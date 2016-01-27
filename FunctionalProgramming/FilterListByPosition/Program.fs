// https://www.hackerrank.com/challenges/fp-filter-positions-in-a-list

module FilterListByPositionModule

open System

let read_input () =
    match Console.ReadLine() with
    | null -> None
    | value -> Some(value |> int, ())

[<EntryPoint>]
let main argv = 
    let input = Seq.unfold read_input ()
    let output = input |> Seq.mapi (fun index number -> (index, number)) |> Seq.filter (fun (index, _) -> index % 2 <> 0) |> Seq.map (fun (_, number) -> number) |> Seq.toList
    output |> List.iter (fun number -> printfn "%d" number)
    0 // return an integer exit code