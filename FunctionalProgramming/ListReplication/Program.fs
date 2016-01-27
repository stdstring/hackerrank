// from https://www.hackerrank.com/challenges/fp-list-replication

module ListReplicationModule

open System

let read_input () =
    match Console.ReadLine() with
    | null -> None
    | value -> Some(value |> int, ())

[<EntryPoint>]
let main argv = 
    let times = Console.ReadLine() |> int
    let input = Seq.unfold read_input ()
    let output = Seq.fold (fun dest number -> dest @ [for _ in 1 .. times -> number]) [] input |> Seq.toList
    output |> List.iter (fun number -> printfn "%d" number)
    0 // return an integer exit code