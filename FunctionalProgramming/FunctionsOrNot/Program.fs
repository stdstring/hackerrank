// https://www.hackerrank.com/challenges/functions-or-not

module FunctionsOrNotModule

open System
open System.Collections.Generic

let read_testcase () =
    let count = Console.ReadLine() |> int
    [for _ in 1 .. count -> Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)]
    
let check_testcase testCase =
    let knownPairs = new Dictionary<int, int>()
    let existFun (pair : int []) =
        let x = pair.[0]
        let y = pair.[1]
        match knownPairs.ContainsKey(x) with
        | true -> (knownPairs.[x] <> y) 
        | false ->
            knownPairs.Add(x, y)
            false
    testCase |> List.exists existFun |> not

[<EntryPoint>]
let main argv =
    let count = Console.ReadLine() |> int
    let testCases = [for _ in 1 .. count -> read_testcase()]
    let resultMapFun = function
        | true -> "YES"
        | false -> "NO"
    testCases |> List.iter (fun testCase -> check_testcase testCase |> resultMapFun |> printfn "%s")
    0 // return an integer exit code
