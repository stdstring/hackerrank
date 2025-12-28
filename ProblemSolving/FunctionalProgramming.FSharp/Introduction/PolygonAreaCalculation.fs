// https://www.hackerrank.com/challenges/lambda-march-compute-the-area-of-a-polygon

namespace FunctionalProgramming.FSharp.Introduction

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type PolygonAreaCalculationTask(input: TextReader, output: TextWriter) =

    let calcPart (pointA : int []) (pointB : int []) =
        let xA = pointA.[0] |> float
        let yA = pointA.[1] |> float
        let xB = pointB.[0] |> float
        let yB = pointB.[1] |> float
        xA * yB - xB * yA

    let rec calcArea (firstVertex: int []) (prevVertex: int []) (vertices: int [] list) (sum: float) =
        match vertices with
        | [] -> 0.5 * (sum + calcPart prevVertex firstVertex) |> abs
        | currentVertex :: verticesRest -> calcArea firstVertex currentVertex verticesRest (sum + calcPart prevVertex currentVertex)

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let vertices = [for _ in 1 .. count -> input.ReadLine().Split(' ') |> Array.map (fun value -> int value)]
            let firstVertex = vertices |> List.head
            let verticesRest = vertices |> List.tail
            calcArea firstVertex firstVertex verticesRest 0.0 |> output.WriteLine
            0

[<TestFixture>]
type PolygonAreaCalculationTests() =

    [<Literal>]
    let MaxError = 0.01

    [<Literal>]
    let RootDirectory = ".//TestCases//Introduction//PolygonAreaCalculation"

    [<TestCase("Input00.txt", 115342.0)>]
    [<TestCase("Input01.txt", 255931.0)>]
    [<TestCase("Input02.txt", 277068.0)>]
    [<TestCase("Input03.txt", 288529.0)>]
    [<TestCase("Input04.txt", 335883.5)>]
    [<TestCase("Input05.txt", 280351.0)>]
    [<TestCase("Input06.txt", 305020.5)>]
    [<TestCase("Input07.txt", 277705.0)>]
    [<TestCase("Input08.txt", 631733.5)>]
    [<TestCase("Input09.txt", 281228.0)>]
    [<TestCase("Input10.txt", 561582.5)>]
    [<TestCase("Input11.txt", 263626.0)>]
    [<TestCase("Input12.txt", 283195.0)>]
    [<TestCase("Input13.txt", 255308.5)>]
    [<TestCase("Input14.txt", 572367.0)>]
    [<TestCase("Input15.txt", 287054.0)>]
    [<TestCase("Input16.txt", 739667.5)>]
    [<TestCase("Input17.txt", 281366.5)>]
    [<TestCase("Input18.txt", 758581.5)>]
    [<TestCase("Input19.txt", 272595.0)>]
    [<TestCase("Input20.txt", 310158.5)>]
    [<TestCase("Input21.txt", 287203.5)>]
    [<TestCase("Input22.txt", 632236.0)>]
    [<TestCase("Input23.txt", 293078.0)>]
    member public this.Execute(inputFile: string, expectedValue: double) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let valueChecker (expectedValue: float) (actualValue: string) =
            let actualValue = actualValue |> System.Double.Parse
            let absError = (expectedValue - actualValue) |> abs
            Assert.That(absError, Is.LessThanOrEqualTo(MaxError))
        TaskExecutor.Execute((fun reader writer -> new PolygonAreaCalculationTask(reader, writer) :> ITask), input, [|expectedValue|], valueChecker)