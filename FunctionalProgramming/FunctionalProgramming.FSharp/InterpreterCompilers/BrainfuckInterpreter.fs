// from https://www.hackerrank.com/challenges/brainf-k-interpreter-fp

namespace FunctionalProgramming.FSharp.InterpreterCompilers

open NUnit.Framework
open System.Collections.Generic
open System.IO
open System.Text
open FunctionalProgramming.Common

type ExecutionContext = {
    mutable OpCount: int32;
    mutable Ip: int32;
    mutable CurrentCell: int32
    Stack: Stack<int32>;
    Input: Queue<byte>}

type BrainfuckInterpreterTask(input: TextReader, output: TextWriter) =

    [<Literal>]
    let MemorySize = 30000

    [<Literal>]
    let MaxOpCount = 100000

    let findBlockEnd (program : string) startIndex =
        let rec findBlockEndImpl index balance =
            match program.[index] with
            | '[' -> findBlockEndImpl (index + 1) (balance + 1)
            | ']' when balance > 1 -> findBlockEndImpl (index + 1) (balance - 1)
            | ']' when balance = 1 -> index
            | _ -> findBlockEndImpl (index + 1) balance
        findBlockEndImpl startIndex 0

    let processCommand (program : string) (memory : byte[]) context =
        let command = program.[context.Ip]
        match command with
        | '>' ->
            context.CurrentCell <- context.CurrentCell + 1
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '<' ->
            context.CurrentCell <- context.CurrentCell - 1
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '+' ->
            memory.[context.CurrentCell] <- memory.[context.CurrentCell] + 1uy
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '-' ->
            memory.[context.CurrentCell] <- memory.[context.CurrentCell] - 1uy
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '.' ->
            [| memory.[context.CurrentCell] |] |> Encoding.ASCII.GetString |> output.Write
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | ',' ->
            memory.[context.CurrentCell] <- context.Input.Dequeue()
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '[' ->
            match memory.[context.CurrentCell] with
            | 0uy ->
                let blockEndIp = findBlockEnd program context.Ip
                context.Ip <- blockEndIp + 1
                context.OpCount <- context.OpCount + 2
            | _ ->
                context.Ip <- context.Ip + 1
                context.Stack.Push(context.Ip)
                context.OpCount <- context.OpCount + 1
        | ']' ->
            match memory.[context.CurrentCell] with
            | 0uy ->
                context.Stack.Pop() |> ignore
                context.Ip <- context.Ip + 1
                context.OpCount <- context.OpCount + 1
            | _ ->
                context.Ip <- context.Stack.Peek()
                context.OpCount <- context.OpCount + 2
        | _ -> failwith "bad command"

    let processProgram (program : string) (input : byte[]) =
        let programSize = program.Length
        let memory = Array.create MemorySize 0uy
        let context = {ExecutionContext.OpCount = 0; ExecutionContext.Ip = 0; ExecutionContext.CurrentCell = 0; ExecutionContext.Stack = new Stack<int32>(); ExecutionContext.Input = new Queue<byte>(input)}
        let rec processProgramImpl () =
            match context with
            | context when context.Ip = programSize -> ()
            | context when context.OpCount >= MaxOpCount -> "\nPROCESS TIME OUT. KILLED!!!" |> output.WriteLine
            | _ ->
                processCommand program memory context
                processProgramImpl ()
        processProgramImpl ()

    interface ITask with
        member this.Execute(argv: string[]) =
            let nmPair = input.ReadLine().Split(' ') |> Array.map (fun value -> int value)
            let n = nmPair.[0]
            let m = nmPair.[1]
            // expect ASCII data
            let inputData = input.ReadLine().Substring(0, n) |> Encoding.ASCII.GetBytes
            let source = [for _ in 1 .. m -> input.ReadLine()]
            let knownChars = "><+-.,[]"
            let program = source |> System.String.Concat |> Seq.filter (fun ch -> knownChars.IndexOf(ch) >= 0) |> System.String.Concat
            processProgram program inputData
            0


[<TestFixture>]
type BrainfuckInterpreterTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//InterpreterCompilers//BrainfuckInterpreter"

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
    [<TestCase("Input11.txt", "Output11.txt")>]
    [<TestCase("Input12.txt", "Output12.txt")>]
    [<TestCase("Input13.txt", "Output13.txt")>]
    [<TestCase("Input14.txt", "Output14.txt")>]
    [<TestCase("Input15.txt", "Output15.txt")>]
    [<TestCase("Input16.txt", "Output16.txt")>]
    [<TestCase("Input17.txt", "Output17.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecuter.Execute((fun reader writer -> new BrainfuckInterpreterTask(reader, writer) :> ITask), input, expectedOutput)