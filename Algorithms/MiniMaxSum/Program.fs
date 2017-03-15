// from https://www.hackerrank.com/challenges/mini-max-sum

module MiniMaxSumModule

open System

[<EntryPoint>]
let main argv =
    let numbers = Console.ReadLine().Split(' ') |> Array.map (fun value -> value |> uint64)
    let sum = numbers |> Seq.sum
    let (minSum, maxSum) = numbers |> Seq.fold (fun (minSum, maxSum) number -> (min minSum (sum - number), max maxSum (sum - number))) (sum, 0UL)
    printfn "%d %d" minSum maxSum
    0
