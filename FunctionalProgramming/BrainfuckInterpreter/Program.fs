// from https://www.hackerrank.com/challenges/brainf-k-interpreter-fp

module BrainfuckInterpreterModule

open System
open System.Collections.Generic
open System.Text

[<Literal>]
let MemorySize = 30000

[<Literal>]
let MaxOpCount = 100000

type ExecutionContext = {
    mutable OpCount : int32;
    mutable Ip : int32;
    mutable CurrentCell : int32
    Stack : Stack<Int32>;
    Input : Queue<Byte>}
    
let find_block_end (program : string) startIndex =
    let rec find_block_end_impl index balance =
        match program.[index] with
        | '[' -> find_block_end_impl (index + 1) (balance + 1)
        | ']' when balance > 1 -> find_block_end_impl (index + 1) (balance - 1)
        | ']' when balance = 1 -> index
        | _ -> find_block_end_impl (index + 1) balance
    find_block_end_impl startIndex 0

let process_command (program : string) (memory : byte[]) context =
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
            [| memory.[context.CurrentCell] |] |> Encoding.ASCII.GetString |> printf "%s"
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | ',' ->
            memory.[context.CurrentCell] <- context.Input.Dequeue()
            context.Ip <- context.Ip + 1
            context.OpCount <- context.OpCount + 1
        | '[' ->
            match memory.[context.CurrentCell] with
            | 0uy ->
                let blockEndIp = find_block_end program context.Ip
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

let process_program (program : string) (input : byte[]) =
    let programSize = program.Length
    let memory = Array.create MemorySize 0uy
    let context = {OpCount = 0; Ip = 0; CurrentCell = 0; Stack = new Stack<Int32>(); Input = new Queue<Byte>(input)}
    let rec process_program_impl () =
        match context with
        | context when context.Ip = programSize -> ()
        | context when context.OpCount >= MaxOpCount -> printfn "\nPROCESS TIME OUT. KILLED!!!"
        | _ ->
            process_command program memory context
            process_program_impl ()
    process_program_impl ()

[<EntryPoint>]
let main argv =
    let nmPair = Console.ReadLine().Split(' ') |> Array.map (fun value -> int value)
    let n = nmPair.[0]
    let m = nmPair.[1]
    // expect ASCII data
    let input = Console.ReadLine().Substring(0, n) |> Encoding.ASCII.GetBytes
    let source = [for _ in 1 .. m -> Console.ReadLine()]
    let knownChars = "><+-.,[]"
    let program = source |> String.Concat |> Seq.filter (fun ch -> knownChars.IndexOf(ch) >= 0) |> String.Concat
    process_program program input
    0 // return an integer exit code