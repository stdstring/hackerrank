// from https://www.hackerrank.com/challenges/tree-manager

module TreeManagerModule

open System
open System.Collections.Generic

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

let parse_command (rawCommand : string) =
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

let get_parent (node : Node) =
    match node.parent with
    | Some(parent) -> parent
    | _ -> failwith "Try get parent for root"

let process_command (node : Node) (command : Command) =
    match command with
    | ChangeValue(value) ->
        node.value <- value
        node
    | Print ->
        node.value |> printfn "%d"
        node
    | VisitLeft ->
        let parent = node |> get_parent
        let nodeIndex = parent.children.IndexOf(node)
        parent.children.[nodeIndex - 1]
    | VisitRight ->
        let parent = node |> get_parent
        let nodeIndex = parent.children.IndexOf(node)
        parent.children.[nodeIndex + 1]
    | VisitParent -> node |> get_parent
    | VisitChild(n) -> node.children.[n - 1]
    | InsertLeft(value) ->
        let parent = node |> get_parent
        let nodeIndex = parent.children.IndexOf(node)
        let newNode = {Node.value = value; Node.parent = Some(parent); Node.children = new List<Node>()}
        parent.children.Insert(nodeIndex, newNode)
        node
    | InsertRight(value) ->
        let parent = node |> get_parent
        let nodeIndex = parent.children.IndexOf(node)
        let newNode = {Node.value = value; Node.parent = Some(parent); Node.children = new List<Node>()}
        parent.children.Insert(nodeIndex + 1, newNode)
        node
    | InsertChild(value) ->
        let newNode = {Node.value = value; Node.parent = Some(node); Node.children = new List<Node>()}
        node.children.Insert(0, newNode)
        node
    | Delete ->
        let parent = node |> get_parent
        parent.children.Remove(node) |> ignore
        parent

[<EntryPoint>]
let main argv =
    let opCount =  Console.ReadLine() |> int
    let commands = [for _ in 1 .. opCount do yield Console.ReadLine() |> parse_command]
    let root = {Node.value = 0; Node.parent = None; Node.children = new List<Node>()}
    commands |> List.fold process_command root |> ignore
    0 // return an integer exit code