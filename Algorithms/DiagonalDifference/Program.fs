// from https://www.hackerrank.com/challenges/diagonal-difference

module DiagonalDifferenceModule

open System

[<EntryPoint>]
let main argv = 
    let n = Console.ReadLine() |> int
    let matrix = Array.init n (fun _ -> [||])
    seq{0 .. n - 1} |> Seq.iter (fun index -> matrix.[index] <- (Console.ReadLine().Split(' ') |> Array.map (fun value -> value |> int)))
    let primarySum = seq{0 .. n - 1} |> Seq.fold (fun sum index -> sum + matrix.[index].[index]) 0
    let secondarySum = seq{0 .. n - 1} |> Seq.fold (fun sum index -> sum + matrix.[index].[n - 1 - index]) 0
    abs (primarySum - secondarySum) |> printfn "%d"
    0
