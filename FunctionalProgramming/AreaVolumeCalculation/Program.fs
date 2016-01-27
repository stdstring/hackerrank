// from https://www.hackerrank.com/challenges/area-under-curves-and-volume-of-revolving-a-curv

module AreaVolumeCalculationModule

open System

[<Literal>]
let Interval = 0.001

let calculate_expression (aCoeffs : float[]) (bCoeffs : int[]) value =
    let foldFun result index =
        let a = aCoeffs.[index]
        let b = bCoeffs.[index]
        result + a * (pown value b)
    {0 .. Array.length aCoeffs - 1} |> Seq.fold foldFun 0.0

let calculate left right aCoeffs bCoeffs =
    let rec calculate_impl current currentExpr (area, volume) =
        match current with
        | _ when current >= right -> area, volume
        | _ ->
            let next = current + Interval
            let nextExpr = calculate_expression aCoeffs bCoeffs next
            let middleExpr = (currentExpr + nextExpr) / 2.0
            let areaDelta = middleExpr * Interval
            let volumeDelta = Math.PI * middleExpr * middleExpr * Interval
            calculate_impl next nextExpr (area + areaDelta, volume + volumeDelta)
    let leftExpr = calculate_expression aCoeffs bCoeffs left
    calculate_impl left leftExpr (0.0, 0.0)

[<EntryPoint>]
let main argv =
    let aCoeffs = Console.ReadLine().Split(' ') |> Array.map (fun value -> float value)
    let bCoeffs = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let borders = Console.ReadLine().Split(' ') |> Array.map (fun value -> float value)
    let left = borders.[0]
    let right = borders.[1]
    let area, volume = calculate left right aCoeffs bCoeffs
    printfn "%f" area
    printfn "%f" volume
    0 // return an integer exit code