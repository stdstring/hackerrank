// from https://www.hackerrank.com/challenges/tutorial-intro

module SortingIntroModule

open System

let findIndex value (numbers : int[]) =
    let calcMiddleIndex start finish =
        let middle = (start + finish) / 2
        match middle with
        | _ when middle = start -> finish
        | _ when middle = finish -> start
        | _ -> middle
    let rec findIndexImpl start finish =
        let middle = calcMiddleIndex start finish
        let middleValue = numbers.[middle]
        match middleValue with
        | _ when (middleValue <> value) && (start = finish) -> failwith "Precondition error"
        | _ when middleValue = value -> middle
        | _ when middleValue > value -> findIndexImpl start middle
        | _ when middleValue < value -> findIndexImpl middle finish
        | _ -> failwith "Unreachable place"
    findIndexImpl 0 (Array.length numbers - 1)

[<EntryPoint>]
let main argv = 
    let value = Console.ReadLine() |> int
    let n = Console.ReadLine() |> int
    Console.ReadLine().Split(' ') |> Array.map (fun value -> value |> int) |> findIndex value |> printfn "%d"
    0