// from https://www.hackerrank.com/challenges/string-mingling

module StringMinglingModule

open System
open System.Text

[<EntryPoint>]
let main argv = 
    let a = Console.ReadLine()
    let b = Console.ReadLine()
    let storage = new StringBuilder(a.Length + b.Length)
    let storage = {0 .. a.Length - 1} |> Seq.fold (fun (acc : StringBuilder) index -> acc.Append(a.[index]).Append(b.[index])) storage
    printfn "%s" <| storage.ToString()
    0 // return an integer exit code