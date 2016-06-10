// https://www.hackerrank.com/challenges/saveprincess

module BotSavesPrincessModule

open System

[<Literal>]
let PrincessChar = 'p'

[<Literal>]
let BotChar = 'm'

type Point = {x:int; y:int}

type Move =
    | Left
    | Right
    | Up
    | Down
    
let generate_moves princessPoint initBotPoint =
    let signX = (princessPoint.x - initBotPoint.x) |> sign
    let signY = (princessPoint.y - initBotPoint.y) |> sign
    let rec generate_moves_impl botPoint moves =
        let deltaX = (princessPoint.x - botPoint.x) |> abs
        let deltaY = (princessPoint.y - botPoint.y) |> abs
        match deltaX, deltaY, signX, signY with
        | 0, 0, _, _ -> moves |> List.rev
        | _, _, _, 1 when (deltaX <= deltaY) -> generate_moves_impl {botPoint with y = botPoint.y + 1} (Move.Down :: moves)
        | _, _, _, -1 when (deltaX <= deltaY) -> generate_moves_impl {botPoint with y = botPoint.y - 1} (Move.Up :: moves)
        | _, _, 1, _ when (deltaY < deltaX) -> generate_moves_impl {botPoint with x = botPoint.x + 1} (Move.Right :: moves)
        | _, _, -1, _ when (deltaY < deltaX) -> generate_moves_impl {botPoint with x = botPoint.x - 1} (Move.Left :: moves)
        | _, _, _, _ -> failwith "Unknown move"
    generate_moves_impl initBotPoint []

let show_move = function
    | Move.Left -> printfn "LEFT"
    | Move.Right -> printfn "RIGHT"
    | Move.Up -> printfn "UP"
    | Move.Down -> printfn "DOWN"
    
[<EntryPoint>]
let main argv =
    let n = Console.ReadLine() |> int
    let foldFun (princessPoint, botPoint) row =
        let line = Console.ReadLine()
        match line.IndexOf(PrincessChar), line.IndexOf(BotChar) with
        | -1, -1 -> princessPoint, botPoint
        | princessIndex, -1 -> Some({Point.x = princessIndex + 1; Point.y = row}), botPoint
        | -1, botIndex -> princessPoint, Some({Point.x = botIndex + 1; Point.y = row})
        | princessIndex, botIndex -> Some({Point.x = princessIndex + 1; Point.y = row}), Some({Point.x = botIndex + 1; Point.y = row})
    let princessPoint, botPoint = match seq {1 .. n} |> Seq.fold foldFun (None, None) with
                                  | Some(princess), Some(bot) -> princess, bot
                                  | _, _ -> failwith "Bad input data"
    generate_moves princessPoint botPoint |> List.iter show_move
    0 // return an integer exit code