// from https://www.hackerrank.com/challenges/string-reductions

module StringReductionsModule

open System
open System.Text

[<Literal>]
let AlphabetSize = 26

[<EntryPoint>]
let main argv = 
    let source = Console.ReadLine()
    let storage = new StringBuilder(AlphabetSize)
    let foldFun (acc : Set<char>) (ch : char) =
        match ch with
        | _ when acc.Contains(ch) -> acc
        | _ ->
            storage.Append(ch) |> ignore
            acc.Add(ch)
    source |> Seq.fold foldFun (new Set<char>(Seq.empty)) |> ignore
    printfn "%s" <| storage.ToString()
    0 // return an integer exit code