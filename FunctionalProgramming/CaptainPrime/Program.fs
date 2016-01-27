// from https://www.hackerrank.com/challenges/captain-prime

module CaptainPrimeModule

open System
open System.Collections

[<Literal>]
let MaxNumber = 1000000

[<Literal>]
let MaxStartNumber = 999

let create_sieve () =
    let sieve = new BitArray(MaxNumber - 1, true)
    let rec fill_sieve number delta =
        match number with
        | _ when number > MaxNumber -> ()
        | _ ->
            sieve.[number - 2] <- false
            fill_sieve (number + delta) delta
    fill_sieve 4 2
    seq {3..2..MaxStartNumber} |> Seq.filter (fun number -> sieve.[number - 2]) |> Seq.iter (fun number -> fill_sieve (number * number) number)
    sieve

let find_divider number =
    let rec find_divider_impl number divider =
        let newDivider = 10 * divider
        match newDivider with
        | _ when newDivider > number -> divider
        | _ -> find_divider_impl number newDivider
    find_divider_impl number 1

let check_left (sieve : BitArray) number =
    let divider = find_divider number
    let rec check_left_impl number divider =
        match number with
        | 0 -> true
        | 1 -> false
        | _ when sieve.[number - 2] ->
            let digit = number / divider
            match digit with
            | 0 -> false
            | _ -> check_left_impl (number % divider) (divider / 10)
        | _ -> false
    check_left_impl number divider
    
let rec check_right (sieve : BitArray) number =
    match number with
    | 0 -> true
    | 1 -> false
    | _ when sieve.[number - 2] ->
        let digit = number % 10
        match digit with
        | 0 -> false
        | _ -> check_right sieve (number / 10)
    | _ -> false

[<EntryPoint>]
let main argv = 
    let count = Console.ReadLine() |> int
    let numbers = [for _ in 1 .. count -> Console.ReadLine() |> int]
    let sieve = create_sieve ()
    let mapFun number =
        let left = check_left sieve number
        let right = check_right sieve number
        match (left, right) with
        | (true, true) -> "CENTRAL"
        | (true, false) -> "LEFT"
        | (false, true) -> "RIGHT"
        | (false, false) -> "DEAD"
    let result = numbers |> List.map mapFun
    result |> List.iter (fun value -> printfn "%s" value)
    0 // return an integer exit code