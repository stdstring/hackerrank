// from https://www.hackerrank.com/challenges/rotate-string

module RotateStringModule

open System

let gen_rotations (str : string) =
    let length = str.Length
    let generator =
        function
        | index when index > length -> None
        | index when index = length -> Some(str, index + 1)
        | index ->
            let result = str.Substring(index) + str.Substring(0, index)
            Some(result, index + 1)
    Seq.unfold generator 1

let output_rotations rotations =
    let output_rotation index rotation =
        match index with
        | 0 -> printf "%s" rotation
        | _ -> printf " %s" rotation
    rotations |> Seq.iteri output_rotation
    printfn ""

[<EntryPoint>]
let main argv = 
    let count = Console.ReadLine() |> int
    let source = [for _ in 1 .. count -> Console.ReadLine()]
    let dest = source |> List.map gen_rotations
    dest |> List.iter output_rotations
    0 // return an integer exit code