// from https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---fibonacci-numbers

module ComputingFibonacciModule

open System

let calc_fib =
    function
    | 1 -> 0
    | 2 -> 1
    | n ->
        let rec calc_fib_impl number current prev =
            match number with
            | _ when number = n -> current
            | _ -> calc_fib_impl (number + 1) (current + prev) current
        calc_fib_impl 2 1 0

[<EntryPoint>]
let main argv =
    let n = Console.ReadLine() |> int
    (calc_fib n) |> printfn "%d"
    0 // return an integer exit code