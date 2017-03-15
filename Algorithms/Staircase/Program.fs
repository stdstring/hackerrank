// from https://www.hackerrank.com/challenges/staircase

module StaircaseModule

open System

[<EntryPoint>]
let main argv =
    let n = Console.ReadLine() |> int
    let iterFun = fun row ->
        let leftPart = new String(' ', n - row)
        let rightPart = new String('#', row)
        printfn "%s%s" leftPart rightPart
    seq { 1 .. n } |> Seq.iter iterFun
    0
