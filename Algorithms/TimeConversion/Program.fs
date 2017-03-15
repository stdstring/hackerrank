// from https://www.hackerrank.com/challenges/time-conversion

module TimeConversionModule

open System

[<EntryPoint>]
let main argv =
    let input = Console.ReadLine()
    let hours = input.Substring(0, 2) |> int
    let minutes = input.Substring(3, 2) |> int
    let seconds = input.Substring(6, 2) |> int
    match input.Substring(8) with
    | "PM" when (hours = 12) -> printfn "%02d:%02d:%02d" hours minutes seconds
    | "PM" -> printfn "%02d:%02d:%02d" (hours + 12) minutes seconds
    | "AM" when (hours = 12) -> printfn "00:%02d:%02d" minutes seconds
    | "AM" -> printfn "%02d:%02d:%02d" hours minutes seconds
    | _ -> failwith "Unknown format of input data"
    0
