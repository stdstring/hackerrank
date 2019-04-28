// https://www.hackerrank.com/challenges/lambda-march-compute-the-perimeter-of-a-polygon

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open System.IO
open FunctionalProgramming.Common

type PolygonPerimeterCalculationTask(input: TextReader, output: TextWriter) =

    let calcLength (pointA : int []) (pointB : int []) =
        let xA = pointA.[0] |> float
        let yA = pointA.[1] |> float
        let xB = pointB.[0] |> float
        let yB = pointB.[1] |> float
        (pown (xA - xB) 2) + (pown (yA - yB) 2) |> sqrt

    let rec calcPerimeter (firstVertex: int[]) (prevVertex: int[]) (vertices: int[] list) (sum: float) =
        match vertices with
        | [] -> sum + calcLength prevVertex firstVertex
        | currentVertex :: verticesRest -> calcPerimeter firstVertex currentVertex verticesRest (sum + calcLength prevVertex currentVertex)

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let vertices = [for _ in 1 .. count -> input.ReadLine().Split(' ') |> Array.map (fun value -> int value)]
            let firstVertex = vertices |> List.head
            let verticesRest = vertices |> List.tail
            calcPerimeter firstVertex firstVertex verticesRest 0.0 |> output.WriteLine
            0

[<TestFixture>]
type PolygonPerimeterCalculationTests() =

    [<Literal>]
    let MaxError = 0.01

    [<Literal>]
    let RootDirectory = ".//TestCases//Introduction//PolygonPerimeterCalculation"

    [<TestCase("Input00.txt", 1556.3949033)>]
    [<TestCase("Input01.txt", 1847.48055065)>]
    [<TestCase("Input02.txt", 2013.20119806)>]
    [<TestCase("Input03.txt", 2337.96920977)>]
    [<TestCase("Input04.txt", 2121.25071955)>]
    [<TestCase("Input05.txt", 2131.83774756)>]
    [<TestCase("Input06.txt", 1983.83673681)>]
    [<TestCase("Input07.txt", 5329.01339478)>]
    [<TestCase("Input08.txt", 2940.3794891)>]
    [<TestCase("Input09.txt", 7153.14266338)>]
    [<TestCase("Input10.txt", 2721.84931945)>]
    [<TestCase("Input11.txt", 18243.3402481)>]
    [<TestCase("Input12.txt", 1887.82374566)>]
    [<TestCase("Input13.txt", 28536.2350494)>]
    [<TestCase("Input14.txt", 2802.67314226)>]
    [<TestCase("Input15.txt", 28092.908018)>]
    [<TestCase("Input16.txt", 3168.22925499)>]
    [<TestCase("Input17.txt", 14695.3576313)>]
    [<TestCase("Input18.txt", 3195.36740957)>]
    [<TestCase("Input19.txt", 39020.6763889)>]
    [<TestCase("Input20.txt", 1989.14816272)>]
    [<TestCase("Input21.txt", 34560.501081)>]
    [<TestCase("Input22.txt", 2903.23293799)>]
    [<TestCase("Input23.txt", 63046.1454973)>]
    member public this.Execute(inputFile: string, expectedValue: double) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let valueChecker (expectedValue: float) (actualValue: string) =
            let actualValue = actualValue |> System.Double.Parse
            Assert.AreEqual(expectedValue, actualValue, MaxError)
        TaskExecuter.Execute((fun reader writer -> new PolygonPerimeterCalculationTask(reader, writer) :> ITask), input, [|expectedValue|], valueChecker)