// from https://www.hackerrank.com/challenges/eval-ex

module EvaluatingExpModule

open System

[<Literal>]
let TermCount = 10

let read_input () =
    match Console.ReadLine() with
    | null -> None
    | value -> Some(value |> float, ())

(*let calculate (value : float) =
    let foldFun (result, prev) iteration =
        let next = value * prev / (float iteration)
        (result + next, next)
    let result, _ = {1 .. TermCount - 1} |> Seq.fold foldFun (1.0, 1.0)
    result*)

let calculate (value : float) =
    let rec calculate_impl result prev iteration =
        match iteration with
        | TermCount -> result
        | _ ->
            let next = value * prev / (float iteration)
            calculate_impl (result + next) next (iteration + 1)
    calculate_impl 1.0 1.0 1

[<EntryPoint>]
let main argv = 
    let times = Console.ReadLine() |> int
    let input = Seq.unfold read_input ()
    let output = input |> Seq.map (fun value -> calculate value) |> Seq.toList
    output |> List.iter (fun result -> printfn "%f" result)
    0 // return an integer exit code