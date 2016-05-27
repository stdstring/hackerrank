// https://www.hackerrank.com/challenges/lambda-march-compute-the-area-of-a-polygon

module PolygonAreaCalculationModule

open System

let calc_part (pointA : int []) (pointB : int []) =
    let xA = pointA.[0] |> float
    let yA = pointA.[1] |> float
    let xB = pointB.[0] |> float
    let yB = pointB.[1] |> float
    xA * yB - xB * yA
    
let rec calc_area firstVertex prevVertex vertices sum =
    match vertices with
    | [] -> 0.5 * (sum + calc_part prevVertex firstVertex) |> abs
    | currentVertex :: verticesRest -> calc_area firstVertex currentVertex verticesRest (sum + calc_part prevVertex currentVertex)

[<EntryPoint>]
let main argv =
    let count = Console.ReadLine() |> int
    let vertices = [for _ in 1 .. count -> Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)]
    let firstVertex = vertices |> List.head
    let verticesRest = vertices |> List.tail
    calc_area firstVertex firstVertex verticesRest 0.0 |> printfn "%f"
    0 // return an integer exit code
