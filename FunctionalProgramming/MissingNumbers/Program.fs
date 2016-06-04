// https://www.hackerrank.com/challenges/missing-numbers-fp

module MissingNumbersModule

open System
open System.Collections.Generic

[<EntryPoint>]
let main argv =
    let firstCount = Console.ReadLine() |> int
    let firstNumbers = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let secondCount = Console.ReadLine() |> int
    let secondNumbers = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let storage = new Dictionary<int, int>()
    let firstIterFun number =
        match storage.ContainsKey(number) with
        | true -> storage.[number] <- storage.[number] + 1
        | false -> storage.[number] <- 1
    let secondIterFun number =
        match storage.ContainsKey(number) with
        | true -> storage.[number] <- storage.[number] - 1
        | false -> storage.[number] <- -1
    firstNumbers |> Array.iter firstIterFun
    secondNumbers |> Array.iter secondIterFun
    storage |> Seq.filter (fun (pair : KeyValuePair<int, int>) -> pair.Value < 0) |> Seq.map (fun (pair : KeyValuePair<int, int>) -> pair.Key) |> Seq.sort |> Seq.iter (fun number -> number |> printf "%d ")
    0 // return an integer exit code