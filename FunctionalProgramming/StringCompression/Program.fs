// from https://www.hackerrank.com/challenges/string-compression

module StringCompressionModule

open System
open System.Text

[<EntryPoint>]
let main argv = 
    let source = Console.ReadLine()
    let storage = new StringBuilder(source.Length)
    let foldFun (savedChar, savedCount) index =
        let ch = source.[index]
        match ch with
        | _ when savedChar = ch -> (savedChar, savedCount + 1)
        | _ ->
            if (savedCount > 1) then
                storage.Append(savedCount) |> ignore
            storage.Append(ch) |> ignore
            (ch, 1)
    let _, count = {0 .. source.Length - 1} |> Seq.fold foldFun (char 0, 1)
    if (count > 1) then
        storage.Append(count) |> ignore
    printfn "%s" <| storage.ToString()
    0 // return an integer exit code
