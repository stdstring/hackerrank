// from https://www.hackerrank.com/challenges/tree-manager

namespace FunctionalProgramming.FSharp.FunctionalStructures

open NUnit.Framework
open System.Collections.Generic
open System.IO
open FunctionalProgramming.Common

[<ReferenceEquality>]
type Node = {mutable value: int; parent: Node option; children: IList<Node>}

type Command =
    | ChangeValue of int
    | Print
    | VisitLeft
    | VisitRight
    | VisitParent
    | VisitChild of int
    | InsertLeft of int
    | InsertRight of int
    | InsertChild of int
    | Delete

type TreeManagerTask(input: TextReader, output: TextWriter) =

    let parseCommand (rawCommand : string) =
        match rawCommand.Split(' ') with
        | [|"change"; value|] -> ChangeValue(value |> int)
        | [|"print"|] -> Print
        | [|"visit"; "left"|] -> VisitLeft
        | [|"visit"; "right"|] -> VisitRight
        | [|"visit"; "parent"|] -> VisitParent
        | [|"visit"; "child"; value|] -> VisitChild(value |> int)
        | [|"insert"; "left"; value|] -> InsertLeft(value |> int)
        | [|"insert"; "right"; value|] -> InsertRight(value |> int)
        | [|"insert"; "child"; value|] -> InsertChild(value |> int)
        | [|"delete"|] -> Delete
        | _ -> failwith "Unknown command"

    let getParent (node : Node) =
        match node.parent with
        | Some(parent) -> parent
        | _ -> failwith "Try get parent for root"

    let processCommand (node : Node) (command : Command) =
        match command with
        | ChangeValue(value) ->
            node.value <- value
            node
        | Print ->
            node.value |> output.WriteLine
            node
        | VisitLeft ->
            let parent = node |> getParent
            let nodeIndex = parent.children.IndexOf(node)
            parent.children.[nodeIndex - 1]
        | VisitRight ->
            let parent = node |> getParent
            let nodeIndex = parent.children.IndexOf(node)
            parent.children.[nodeIndex + 1]
        | VisitParent -> node |> getParent
        | VisitChild(n) -> node.children.[n - 1]
        | InsertLeft(value) ->
            let parent = node |> getParent
            let nodeIndex = parent.children.IndexOf(node)
            let newNode = {Node.value = value; Node.parent = Some(parent); Node.children = new List<Node>()}
            parent.children.Insert(nodeIndex, newNode)
            node
        | InsertRight(value) ->
            let parent = node |> getParent
            let nodeIndex = parent.children.IndexOf(node)
            let newNode = {Node.value = value; Node.parent = Some(parent); Node.children = new List<Node>()}
            parent.children.Insert(nodeIndex + 1, newNode)
            node
        | InsertChild(value) ->
            let newNode = {Node.value = value; Node.parent = Some(node); Node.children = new List<Node>()}
            node.children.Insert(0, newNode)
            node
        | Delete ->
            let parent = node |> getParent
            parent.children.Remove(node) |> ignore
            parent

    interface ITask with
        member this.Execute(argv: string[]) =
            let opCount =  input.ReadLine() |> int
            let commands = [for _ in 1 .. opCount do yield input.ReadLine() |> parseCommand]
            let root = {Node.value = 0; Node.parent = None; Node.children = new List<Node>()}
            commands |> List.fold processCommand root |> ignore
            0

[<TestFixture>]
type TreeManagerTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//FunctionalStructures//TreeManager"

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
    [<TestCase("Input18.txt", "Output18.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecuter.Execute((fun reader writer -> new TreeManagerTask(reader, writer) :> ITask), input, expectedOutput)