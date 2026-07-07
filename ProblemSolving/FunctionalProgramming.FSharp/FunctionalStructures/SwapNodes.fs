// https://www.hackerrank.com/challenges/swap-nodes/

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open ProblemSolving.Common
open System.IO
open System.Text

module SwapNodes =

    type Node = {Value: int; LeftChild: Node option; RightChild: Node option}

    let rec createTreeImpl (number: int) (source: string[]) =
        let index = number - 1
        match source.[index].Split(' ') with
        | [|left; right|] ->
            let leftChild = match left with
                            | "-1" -> None
                            | _ -> source |> createTreeImpl (left |> int)
            let rightChild = match right with
                             | "-1" -> None
                             | _ -> source |> createTreeImpl (right |> int)
            {Node.Value = number; Node.LeftChild = leftChild; Node.RightChild = rightChild} |> Some
        | _ -> failwith "Bad node data"

    let createTree (source: string[]) =
        match source |> createTreeImpl 1 with
        | Some root -> root
        | None -> failwith "Bad node data"

    let rec inorderTraverseImpl (node: Node) (dest: StringBuilder) =
        match node.LeftChild with
        | Some leftChild -> dest |> inorderTraverseImpl leftChild
        | None -> ()
        if dest.Length > 0 then
            " " |> dest.Append |> ignore
        node.Value |> dest.Append |> ignore
        match node.RightChild with
        | Some rightChild -> dest |> inorderTraverseImpl rightChild
        | None -> ()

    let inorderTraverse (tree: Node) =
        let dest = new StringBuilder()
        dest |> inorderTraverseImpl tree
        dest.ToString()

    let rec swapNodes (height: int) (k: int) (current: Node) =
        let number = current.Value
        let leftChild = match current.LeftChild with
                        | None -> None
                        | Some child -> child |> swapNodes (height + 1) (k) |> Some
        let rightChild = match current.RightChild with
                         | None -> None
                         | Some child -> child |> swapNodes (height + 1) (k) |> Some
        match height % k with
        | 0 -> {Node.Value = number; Node.LeftChild = rightChild; Node.RightChild = leftChild}
        | _ -> {Node.Value = number; Node.LeftChild = leftChild; Node.RightChild = rightChild}

    type SwapNodesTask(input: TextReader, output: TextWriter) =

        interface ITask with
            member _.Execute(_: string[]) =
                let nodesCount = input.ReadLine() |> int
                let source = nodesCount |> Array.zeroCreate
                for index in seq{0 .. nodesCount - 1} do
                    source.[index] <- input.ReadLine()
                let mutable tree = source |> createTree
                let swapCount = input.ReadLine() |> int
                for _ in seq{0 .. swapCount - 1} do
                    let k = input.ReadLine() |> int
                    tree <- tree |> swapNodes 1 k
                    tree |> inorderTraverse |> output.WriteLine
                0

open SwapNodes

[<TestFixture>]
type SwapNodesTests() =

    [<Literal>]
    let RootDirectory = __SOURCE_DIRECTORY__ + "//..//TestCases//FunctionalStructures//SwapNodes"

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
    member public _.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new SwapNodesTask(reader, writer) :> ITask), input, expectedOutput)