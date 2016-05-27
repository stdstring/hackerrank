// https://www.hackerrank.com/challenges/lambda-march-compute-the-perimeter-of-a-polygon

module PolygonPerimeterCalculationModule

open System

let calc_length (pointA : int []) (pointB : int []) =
    let xA = pointA.[0] |> float
    let yA = pointA.[1] |> float
    let xB = pointB.[0] |> float
    let yB = pointB.[1] |> float
    (pown (xA - xB) 2) + (pown (yA - yB) 2) |> sqrt
    
let rec calc_perimeter firstVertex prevVertex vertices sum =
    match vertices with
    | [] -> sum + calc_length prevVertex firstVertex
    | currentVertex :: verticesRest -> calc_perimeter firstVertex currentVertex verticesRest (sum + calc_length prevVertex currentVertex)

[<EntryPoint>]
let main argv =
    let count = Console.ReadLine() |> int
    let vertices = [for _ in 1 .. count -> Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)]
    let firstVertex = vertices |> List.head
    let verticesRest = vertices |> List.tail
    calc_perimeter firstVertex firstVertex verticesRest 0.0 |> printfn "%f"
    0 // return an integer exit code
