// from https://www.hackerrank.com/challenges/sequence-full-of-colors

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.IO

type ColorsSequenceTask(input: TextReader, output: TextWriter) =

    let checkPrefix (red: int) (green: int) (yellow: int) (blue: int) =
        match (red, green, yellow, blue) with
        | _ when (abs (red - green) > 1) -> false
        | _ when (abs (yellow - blue) > 1) -> false
        | _ -> true

    let checkResult (red: int) (green: int) (yellow: int) (blue: int) =
        match (red, green, yellow, blue) with
        | _ when red <> green -> false
        | _ when yellow <> blue -> false
        | _ -> true

    let parseColor (ch: char) (red: int) (green: int) (yellow: int) (blue: int) =
        match ch with
        | 'R' -> red + 1, green, yellow, blue
        | 'G' -> red, green + 1, yellow, blue
        | 'Y' -> red, green, yellow + 1, blue
        | 'B' -> red, green, yellow, blue + 1
        | _ -> failwith "Unknown color"

    let processColor (source : string) =
        let charSeq = source.GetEnumerator()
        let rec processColorImpl (red: int) (green: int) (yellow: int) (blue: int) =
            match charSeq.MoveNext() with
            | false -> checkResult red green yellow blue
            | true ->
                let newRed, newGreen, newYellow, newBlue = parseColor charSeq.Current red green yellow blue
                match checkPrefix newRed newGreen newYellow newBlue with
                | false -> false
                | true -> processColorImpl newRed newGreen newYellow newBlue
        processColorImpl 0 0 0 0

    let outputResult: bool -> unit =
        function
        | true -> "True" |> output.WriteLine
        | false -> "False" |> output.WriteLine

    interface ITask with
        member this.Execute(argv: string[]) =
            let count = input.ReadLine() |> int
            let source = [for _ in 1 .. count -> input.ReadLine()]
            let dest = source |> List.map (fun item -> processColor item)
            dest |> List.iter outputResult
            0

[<TestFixture>]
type ColorsSequenceTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//Recursion//ColorsSequence"

    [<TestCase("Input00.txt", [|"True"; "True"; "False"; "False"|])>]
    [<TestCase("Input01.txt", [|"False"; "False"; "False"; "True"; "False"; "True"; "True"; "True"; "True"; "False"|])>]
    [<TestCase("Input02.txt", [|"True"; "False"; "True"; "True"; "True"; "False"; "True"; "False"; "False"; "True"|])>]
    [<TestCase("Input03.txt", [|"False"; "True"; "True"; "True"; "True"; "True"; "False"; "False"; "True"; "True"|])>]
    [<TestCase("Input04.txt", [|"True"; "True"; "False"; "False"; "False"; "True"; "True"; "False"; "True"; "True"|])>]
    [<TestCase("Input05.txt", [|"True"; "False"; "False"; "False"; "True"; "True"; "True"; "False"; "False"; "False"|])>]
    [<TestCase("Input06.txt", [|"True"; "True"; "False"; "True"; "False"; "True"; "False"; "True"; "True"; "False"|])>]
    [<TestCase("Input07.txt", [|"True"; "False"; "False"; "False"; "False"; "False"; "True"; "False"; "False"; "True"|])>]
    [<TestCase("Input08.txt", [|"False"; "False"; "False"; "True"; "False"; "False"; "False"; "False"; "False"; "False"|])>]
    [<TestCase("Input09.txt", [|"True"; "False"; "True"; "True"; "True"; "True"; "False"; "True"; "True"; "False"|])>]
    [<TestCase("Input10.txt", [|"False"; "False"; "True"; "False"; "False"; "False"; "True"; "True"; "True"; "False"|])>]
    [<TestCase("Input11.txt", [|"False"; "True"; "False"; "False"; "False"; "False"; "True"; "False"; "True"; "False"|])>]
    [<TestCase("Input12.txt", [|"True"; "True"; "False"; "True"; "False"; "False"; "True"; "False"; "False"; "True"|])>]
    [<TestCase("Input13.txt", [|"True"; "True"; "True"; "False"; "False"; "False"; "False"; "False"; "True"; "False"|])>]
    [<TestCase("Input14.txt", [|"True"; "True"; "False"; "True"; "True"; "True"; "True"; "True"; "False"; "False"|])>]
    [<TestCase("Input15.txt", [|"True"; "True"; "True"; "True"; "False"; "True"; "False"; "True"; "False"; "False"|])>]
    member public this.Execute(inputFile: string, expectedOutput: string[]) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        TaskExecutor.Execute((fun reader writer -> new ColorsSequenceTask(reader, writer) :> ITask), input, expectedOutput)