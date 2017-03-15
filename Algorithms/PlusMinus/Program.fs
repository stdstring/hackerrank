// from https://www.hackerrank.com/challenges/plus-minus

module PlusMinusModule

open System

[<EntryPoint>]
let main argv =
    let n = Console.ReadLine() |> int
    let numbers = Console.ReadLine().Split(' ') |> Array.map (fun value -> value |> int)
    let positiveCount = numbers |> Seq.filter (fun number -> number > 0) |> Seq.length
    let negativeCount = numbers |> Seq.filter (fun number -> number < 0) |> Seq.length
    printfn "%.6f" ((positiveCount |> float) / (n |> float))
    printfn "%.6f" ((negativeCount |> float) / (n |> float))
    printfn "%.6f" ((n - positiveCount - negativeCount |> float) / (n |> float))
    0
