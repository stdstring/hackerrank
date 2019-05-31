// from https://www.hackerrank.com/challenges/filter-elements

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.Collections.Generic
open System.IO

type FilterRequest = {n: int; k: int; a: int[]}

type FilterElementsTask(input: TextReader, output: TextWriter) =

    let readFilterRequest () =
        let nkPair = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
        let totalCount = nkPair.[0]
        let repeatCount = nkPair.[1]
        let elements = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
        {FilterRequest.n = totalCount; FilterRequest.k = repeatCount; FilterRequest.a = elements}

    let processFilterRequest (request: FilterRequest) =
        let foldFun (map : Dictionary<int, int*int>, index: int) (number: int) =
            if not (map.ContainsKey(number)) then
                map.[number] <- (index, 1)
            else
                let numberIndex, numberCount = map.[number]
                map.[number] <- (numberIndex, numberCount + 1)
            (map, index + 1)
        let numberMap, _ = request.a |> Array.fold foldFun (new Dictionary<int, int*int>(), 0)
        let rec collect (index: int) (result: int list) =
            match index with
            | _ when index = request.a.Length -> result |> List.rev
            | _ ->
                let number = request.a.[index]
                let numberIndex, numberCount = numberMap.[number]
                let newResult = if (numberCount >= request.k) && (index = numberIndex) then number :: result else result
                collect (index + 1) newResult
        collect 0 []

    let showResult: int list -> unit =
        function | [] -> "-1" |> output.WriteLine
                 | result -> result |> List.iter (fun number -> output.Write("{0} ", number)); "" |> output.WriteLine

    interface ITask with
        member this.Execute(argv: string[]) =
            let testCases = input.ReadLine() |> int
            let requests = [1 .. testCases] |> List.map (fun _ -> readFilterRequest ())
            let results = requests |> List.map processFilterRequest
            results |> List.iter showResult
            0

[<TestFixture>]
type FilterElementsTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//FilterElements"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    [<TestCase("Input04.txt", "Output04.txt")>]
    [<TestCase("Input05.txt", "Output05.txt")>]
    [<TestCase("Input06.txt", "Output06.txt")>]
    [<TestCase("Input07.txt", "Output07.txt")>]
    [<TestCase("Input08.txt", "Output08.txt")>]
    [<TestCase("Input09.txt", "Output09.txt")>]
    [<TestCase("Input10.txt", "Output10.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new FilterElementsTask(reader, writer) :> ITask), input, expectedOutput)