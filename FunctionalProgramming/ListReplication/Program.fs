// from https://www.hackerrank.com/challenges/fp-list-replication

module ListReplicationModule

open System
open System.IO

(*let read_input () =
    match Console.ReadLine() with
    | null -> None
    | value -> Some(value |> int, ())

[<EntryPoint>]
let main argv = 
    let times = Console.ReadLine() |> int
    let input = Seq.unfold read_input ()
    let output = Seq.fold (fun dest number -> dest @ [for _ in 1 .. times -> number]) [] input |> Seq.toList
    output |> List.iter (fun number -> printfn "%d" number)
    0 // return an integer exit code*)

type ListReplication(input: TextReader, output: TextWriter) =

    let readInput () =
        match input.ReadLine() with
        | null -> None
        | value -> (value |> int, ()) |> Some

    member public this.Execute(argv: string[]) =
        let times = input.ReadLine() |> int
        let inputData = Seq.unfold readInput ()
        let outputData = Seq.fold (fun dest number -> dest @ [for _ in 1 .. times -> number]) [] inputData |> Seq.toList
        outputData |> List.iter (fun number -> output.WriteLine("{0}", number))
        0

[<EntryPoint>]
let main argv = 
    let task = new ListReplication(Console.In, Console.Out)
    argv |> task.Execute