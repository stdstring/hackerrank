// from https://www.hackerrank.com/challenges/super-digit

module SuperDigitModule

open System

let to_digit =
    function
    | '0' -> 0UL
    | '1' -> 1UL
    | '2' -> 2UL
    | '3' -> 3UL
    | '4' -> 4UL
    | '5' -> 5UL
    | '6' -> 6UL
    | '7' -> 7UL
    | '8' -> 8UL
    | '9' -> 9UL
    | _ -> failwith "bad character"
    
let get_digits (number : uint64) =
    let rec get_digits_impl number storage =
        match number with
        | _ when number < 10UL -> number :: storage
        | _ ->
            let rest = number / 10UL
            let digit = number % 10UL
            get_digits_impl rest (digit :: storage)
    get_digits_impl number []

let rec superdigit (number : uint64) =
    match number with
    | _ when number < 10UL -> number
    | _ -> number |> get_digits |> List.sum |> superdigit

[<EntryPoint>]
let main argv = 
    let sourceData = Console.ReadLine().Split(' ')
    let sourceNumber = sourceData.[0]
    let repeatNumber = sourceData.[1] |> uint64
    let initSum = (sourceNumber |> Seq.fold(fun acc digit -> acc + (to_digit digit)) 0UL) * repeatNumber
    let result = superdigit initSum
    printfn "%d" result
    0 // return an integer exit code