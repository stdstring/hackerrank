// from https://www.hackerrank.com/challenges/remove-duplicates

module RemoveDuplicatesModule

open System
open System.Text

[<Literal>]
let AlphabetSize = 26

[<Literal>]
let AlphabetStart = 'a'

[<EntryPoint>]
let main argv = 
    let source = Console.ReadLine()
    let usageArray = Array.create AlphabetSize false
    let indexStart = (int AlphabetStart)
    let foldFun (dest : StringBuilder) (ch : char) =
        let index = (int ch) - indexStart
        match usageArray.[index] with
        | false ->
            usageArray.[index] <- true
            dest.Append(ch)
        | true -> dest
    let dest= source |> Seq.fold foldFun (new StringBuilder(AlphabetSize))
    dest.ToString() |> printfn "%s"
    0 // return an integer exit code