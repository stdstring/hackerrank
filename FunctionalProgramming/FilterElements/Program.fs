// from https://www.hackerrank.com/challenges/filter-elements

module FilterElementsModule

open System
open System.Collections.Generic

type FilterRequest = {n: int; k: int; a: int[];}

let read_filter_request () =
    let nkPair = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let totalCount = nkPair.[0]
    let repeatCount = nkPair.[1]
    let elements = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    {n = totalCount; k = repeatCount; a = elements}
    
let process_filter_request request =
    let foldFun (map : Dictionary<int, int*int>, index) number =
        if not (map.ContainsKey(number)) then
            map.[number] <- (index, 1)
        else
            let numberIndex, numberCount = map.[number]
            map.[number] <- (numberIndex, numberCount + 1)
        (map, index + 1)
    let numberMap, _ = request.a |> Array.fold foldFun (new Dictionary<int, int*int>(), 0)
    let rec collect index result =
        match index with
        | _ when index = request.a.Length -> result |> List.rev
        | _ ->
            let number = request.a.[index]
            let numberIndex, numberCount = numberMap.[number]
            let newResult = if (numberCount >= request.k) && (index = numberIndex) then number :: result else result
            collect (index + 1) newResult
    collect 0 []
    
let show_result =
    function | [] -> printfn "-1"
             | result -> result |> List.iter (fun number -> number |> printf "%d "); printfn ""

[<EntryPoint>]
let main argv =
    let testCases = Console.ReadLine() |> int
    let requests = [1 .. testCases] |> List.map (fun _ -> read_filter_request ())
    let results = requests |> List.map process_filter_request
    results |> List.iter show_result
    0 // return an integer exit code