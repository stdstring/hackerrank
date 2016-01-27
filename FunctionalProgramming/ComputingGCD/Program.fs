// from https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---gcd

module ComputingGCDModule

open System

let rec calc_gcd a b =
    match a with
    | _ when a = b -> a
    | _ when a > b -> calc_gcd (a - b) b
    | _ (*when a < b*) -> calc_gcd a (b - a)

[<EntryPoint>]
let main argv =
    let numbers = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let a = numbers.[0]
    let b = numbers.[1]
    (calc_gcd a b) |> printfn "%d"
    0 // return an integer exit code