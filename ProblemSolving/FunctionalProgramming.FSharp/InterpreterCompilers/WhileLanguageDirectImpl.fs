// from https://www.hackerrank.com/challenges/while-language-fp

namespace FunctionalProgramming.FSharp.InterpreterCompilers

open NUnit.Framework
open ProblemSolving.Common
open System
open System.Collections.Generic
open System.IO

type Lexem =
    | Variable of string
    | Number of int64
    | OpAdd
    | OpSub
    | OpMult
    | OpDiv
    | OpAnd
    | OpOr
    | OpGreater
    | OpLess
    | OpenBracket
    | CloseBracket
    | True
    | False
    | If
    | Then
    | Else
    | While
    | Do
    | CurlyOpenBracket
    | CurlyCloseBracket
    | Semicolon
    | Assignment
    | Eof

type LexParser() =
    let knownNames = ["+", Lexem.OpAdd;
                      "-", Lexem.OpSub;
                      "*", Lexem.OpMult;
                      "/", Lexem.OpDiv;
                      "and", Lexem.OpAnd;
                      "or", Lexem.OpOr;
                      ">", Lexem.OpGreater;
                      "<", Lexem.OpLess;
                      "(", Lexem.OpenBracket;
                      ")", Lexem.CloseBracket;
                      "true", Lexem.True;
                      "false", Lexem.False;
                      "if", Lexem.If;
                      "then", Lexem.Then;
                      "else", Lexem.Else;
                      "while", Lexem.While;
                      "do", Lexem.Do;
                      "{", Lexem.CurlyOpenBracket;
                      "}", Lexem.CurlyCloseBracket;
                      ";", Lexem.Semicolon;
                      ":=", Lexem.Assignment] |> dict

    let parseLexem (lexemStr: string) =
        match lexemStr |> knownNames.ContainsKey with
        | true -> knownNames.[lexemStr]
        | false ->
            match lexemStr.[0] with
            | letter when letter |> Char.IsLower -> Lexem.Variable lexemStr
            | digit when digit |> Char.IsDigit -> Lexem.Number (lexemStr |> int64)
            | '-' -> Lexem.Number (lexemStr |> int64)
            | _ -> failwith "Unexpected branch of match expression"

    member public this.Parse(source: seq<string>) =
        [Lexem.Eof] |> Seq.append (source |> Seq.map (fun line -> line.Split([|" "; "\t"|], StringSplitOptions.RemoveEmptyEntries) |> Seq.map parseLexem) |> Seq.concat)

type Context = IDictionary<string, int64>
type Expression = Context->int64
type Condition = Context->bool
type ExecutionItem = Context->unit

type OutputGenerator() =

    member public this.GenerateForBody(bodyItems: seq<ExecutionItem>) =
        fun (context: Context) -> bodyItems |> Seq.iter (fun item -> (context |> item))

    member public this.GenerateForAssignVar(name: string, expression: Expression) =
        fun (context: Context) -> (context.[name] <- (context |> expression))

    member public this.GenerateForIf(condition: Condition, thenBodyItems: seq<ExecutionItem>, elseBodyItems: seq<ExecutionItem>) =
        let thenBody = thenBodyItems |> this.GenerateForBody
        let elseBody = elseBodyItems |> this.GenerateForBody
        fun (context: Context) -> if (context |> condition) then (context |> thenBody) else (context |> elseBody)

    member public this.GenerateForWhile(condition: Condition, bodyItems: seq<ExecutionItem>) =
        let body = bodyItems |> this.GenerateForBody
        fun (context: Context) -> while (context |> condition) do (context |> body)

    member public this.GenerateForNumberExpression(number: int64) =
        fun (context: Context) -> number

    member public this.GenerateForVariableExpression(name: string) =
        fun (context: Context) -> context.[name]

    member public this.GenerateForOperatorAdd(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) + (context |> rightOp)

    member public this.GenerateForOperatorSub(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) - (context |> rightOp)

    member public this.GenerateForOperatorMult(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) * (context |> rightOp)

    member public this.GenerateForOperatorDiv(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) / (context |> rightOp)

    member public this.GenerateForOperatorAnd(leftOp: Condition, rightOp: Condition) =
        fun (context: Context) -> (context |> leftOp) && (context |> rightOp)

    member public this.GenerateForOperatorOr(leftOp: Condition, rightOp: Condition) =
        fun (context: Context) -> (context |> leftOp) || (context |> rightOp)

    member public this.GenerateForBoolValue(value: bool) =
        fun (context: Context) -> false

    member public this.GenerateForGreater(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) > (context |> rightOp)

    member public this.GenerateForLess(leftOp: Expression, rightOp: Expression) =
        fun (context: Context) -> (context |> leftOp) < (context |> rightOp)

type ExpressionData = {Expressions: Expression list; Operators: Lexem list}

type ConditionData = {Conditions: Condition list; Operators: Lexem list}

type SyntaxParser() =

    let outputGenerator = new OutputGenerator()

    let appendExpression (expressionData: ExpressionData) (current: Expression) =
        match expressionData.Expressions with
        | [] -> {expressionData with ExpressionData.Expressions = [current]}
        | [expression] ->
            match expressionData.Operators with
            | [Lexem.OpMult] -> {ExpressionData.Expressions = [outputGenerator.GenerateForOperatorMult(expression, current)]; ExpressionData.Operators = []}
            | [Lexem.OpDiv] -> {ExpressionData.Expressions = [outputGenerator.GenerateForOperatorDiv(expression, current)]; ExpressionData.Operators = []}
            |_ -> {expressionData with ExpressionData.Expressions = [expression; current]}
        | [expression1; expression2] ->
            match expressionData.Operators with
            | [op1; Lexem.OpMult] -> {ExpressionData.Expressions = [expression1; outputGenerator.GenerateForOperatorMult(expression2, current)]; ExpressionData.Operators = [op1]}
            | [op1; Lexem.OpDiv] -> {ExpressionData.Expressions = [expression1; outputGenerator.GenerateForOperatorDiv(expression2, current)]; ExpressionData.Operators = [op1]}
            | [Lexem.OpAdd; op2] -> {ExpressionData.Expressions = [outputGenerator.GenerateForOperatorAdd(expression1, expression2); current]; ExpressionData.Operators = [op2]}
            | [Lexem.OpSub; op2] -> {ExpressionData.Expressions = [outputGenerator.GenerateForOperatorSub(expression1, expression2); current]; ExpressionData.Operators = [op2]}
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    let finishExpression (expressionData: ExpressionData) =
        match expressionData.Expressions with
        | [expression] -> expression
        | [expression1; expression2] ->
            match expressionData.Operators with
            | [Lexem.OpAdd] -> outputGenerator.GenerateForOperatorAdd(expression1, expression2)
            | [Lexem.OpSub] -> outputGenerator.GenerateForOperatorSub(expression1, expression2)
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    let appendCondition (conditionData: ConditionData) (current: Condition) =
        match conditionData.Conditions with
        | [] -> {conditionData with ConditionData.Conditions = [current]}
        | [condition] ->
            match conditionData.Operators with
            | [Lexem.OpAnd] -> {ConditionData.Conditions = [outputGenerator.GenerateForOperatorAnd(condition, current)]; ConditionData.Operators = []}
            |_ -> {conditionData with ConditionData.Conditions = [condition; current]}
        | [condition1; condition2] ->
            match conditionData.Operators with
            | [op1; Lexem.OpAnd] -> {ConditionData.Conditions = [condition1; outputGenerator.GenerateForOperatorAnd(condition2, current)]; ConditionData.Operators = [op1]}
            | [Lexem.OpOr; op2] -> {ConditionData.Conditions = [outputGenerator.GenerateForOperatorOr(condition1, condition2); current]; ConditionData.Operators = [op2]}
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    let finishCondition (conditionData: ConditionData) =
        match conditionData.Conditions with
        | [condition] -> condition
        | [condition1; condition2] ->
            match conditionData.Operators with
            | [Lexem.OpOr] -> outputGenerator.GenerateForOperatorOr(condition1, condition2)
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    member public this.Parse(lexems: seq<Lexem>) =
        let lexemsEnumerator = lexems.GetEnumerator();
        lexemsEnumerator.MoveNext() |> ignore
        let program = this.ParseStatementBlock(lexemsEnumerator, Lexem.Eof) |> outputGenerator.GenerateForBody
        match lexemsEnumerator.Current with
        | Lexem.Eof -> program
        | _ -> failwith "Unexpected branch of match expression"

    member private this.ParseStatementBlock(lexems: IEnumerator<Lexem>, blockEnd: Lexem) =
        let rec parseStatementBlockImpl (body: ExecutionItem list) =
            let statement = match lexems.Current with
                            | Lexem.Variable _ -> this.ParseAssignStatement(lexems, blockEnd)
                            | Lexem.If -> lexems |> this.ParseIfStatement
                            | Lexem.While -> lexems |> this.ParseWhileStatement
                            | _ -> failwith "Unexpected branch of match expression"
            match lexems.Current with
            | currentLexem when currentLexem = blockEnd -> statement :: body |> Seq.rev
            | Lexem.Semicolon ->
                lexems.MoveNext() |> ignore
                statement :: body |> parseStatementBlockImpl
            | _ -> failwith "Unexpected branch of match expression"
        [] |> parseStatementBlockImpl

    member private this.ParseAssignStatement(lexems: IEnumerator<Lexem>, blockEnd: Lexem) =
        match lexems.Current with
        | Lexem.Variable name ->
            lexems.MoveNext() |> ignore
            match lexems.Current with
            | Lexem.Assignment ->
                lexems.MoveNext() |> ignore
                outputGenerator.GenerateForAssignVar(name, this.ParseArithExpr(lexems, [Lexem.Semicolon; blockEnd]))
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    // if b then { S1 } else { S2 }
    member private this.ParseIfStatement(lexems: IEnumerator<Lexem>) =
        match lexems.Current with
        | Lexem.If ->
            lexems.MoveNext() |> ignore
            let condition = this.ParseBoolExpr(lexems, [Lexem.Then])
            let thenBody = lexems |> this.ParseThenPart
            let elseBody = lexems |> this.ParseElsePart
            outputGenerator.GenerateForIf(condition, thenBody, elseBody)
        | _ -> failwith "Unexpected branch of match expression"

    member private this.ParseThenPart(lexems: IEnumerator<Lexem>) =
        match lexems.Current with
        | Lexem.Then ->
            lexems.MoveNext() |> ignore
            match lexems.Current with
            | Lexem.CurlyOpenBracket ->
                lexems.MoveNext() |> ignore
                let thenBody = this.ParseStatementBlock(lexems, Lexem.CurlyCloseBracket)
                match lexems.Current with
                | Lexem.CurlyCloseBracket ->
                    lexems.MoveNext() |> ignore
                    thenBody
                | _ -> failwith "Unexpected branch of match expression"
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    member private this.ParseElsePart(lexems: IEnumerator<Lexem>) =
        match lexems.Current with
        | Lexem.Else ->
            lexems.MoveNext() |> ignore
            match lexems.Current with
            | Lexem.CurlyOpenBracket ->
                lexems.MoveNext() |> ignore
                let elseBody = this.ParseStatementBlock(lexems, Lexem.CurlyCloseBracket)
                match lexems.Current with
                | Lexem.CurlyCloseBracket ->
                    lexems.MoveNext() |> ignore
                    elseBody
                | _ -> failwith "Unexpected branch of match expression"
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    // while b do { S }
    member private this.ParseWhileStatement(lexems: IEnumerator<Lexem>) =
        match lexems.Current with
        | Lexem.While ->
            lexems.MoveNext() |> ignore
            let condition = this.ParseBoolExpr(lexems, [Lexem.Do])
            let body = lexems |> this.ParseDoPart
            outputGenerator.GenerateForWhile(condition, body)
        | _ -> failwith "Unexpected branch of match expression"

    member private this.ParseDoPart(lexems: IEnumerator<Lexem>) =
        match lexems.Current with
        | Lexem.Do ->
            lexems.MoveNext() |> ignore
            match lexems.Current with
            | Lexem.CurlyOpenBracket ->
                lexems.MoveNext() |> ignore
                let body = this.ParseStatementBlock(lexems, Lexem.CurlyCloseBracket)
                match lexems.Current with
                | Lexem.CurlyCloseBracket ->
                    lexems.MoveNext() |> ignore
                    body
                | _ -> failwith "Unexpected branch of match expression"
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

    // Arithmetic expressions a ::= x | n | a1 opa a2 | ( a )
    member private this.ParseArithExpr(lexems: IEnumerator<Lexem>, followLexems: Lexem list) =
        let checkAddAbility (expressionsOpDelta: int) (expressionData: ExpressionData) =
            match (expressionData.Expressions.Length - expressionData.Operators.Length) with
            | value when value = expressionsOpDelta -> ()
            | _ -> failwith "Unexpected branch of match expression"
        let mutable expressionData = {ExpressionData.Expressions = []; ExpressionData.Operators = []}
        while followLexems |> List.contains lexems.Current |> not do
            match lexems.Current with
            | Lexem.OpenBracket ->
                expressionData |> checkAddAbility 0
                expressionData <- this.ParseArithExprInBrackets(lexems, expressionData)
            | Lexem.Number number ->
                expressionData |> checkAddAbility 0
                expressionData <- number |> outputGenerator.GenerateForNumberExpression |> appendExpression expressionData
            | Lexem.Variable name ->
                expressionData |> checkAddAbility 0
                expressionData <- name |> outputGenerator.GenerateForVariableExpression |> appendExpression expressionData
            | Lexem.OpAdd | Lexem.OpSub | Lexem.OpMult | Lexem.OpDiv ->
                expressionData |> checkAddAbility 1
                expressionData <- {expressionData with ExpressionData.Operators = expressionData.Operators @ [lexems.Current]}
            | _ -> failwith "Unexpected branch of match expression"
            lexems.MoveNext() |> ignore
        expressionData |> finishExpression

    member private this.ParseArithExprInBrackets(lexems: IEnumerator<Lexem>, expressionData: ExpressionData) =
        lexems.MoveNext() |> ignore
        let expressionData = this.ParseArithExpr(lexems, [Lexem.CloseBracket]) |> appendExpression expressionData
        match lexems.Current with
        | Lexem.CloseBracket -> expressionData
        | _ -> failwith "Unexpected branch of match expression"

    // Boolean expressions b ::= true | false | b1 opb b2 | a1 opr a2 | ( b )
    member private this.ParseBoolExpr(lexems: IEnumerator<Lexem>, followLexems: Lexem list) =
        let checkAddAbility (conditionsOpDelta: int) (conditionData: ConditionData) =
            match (conditionData.Conditions.Length - conditionData.Operators.Length) with
            | value when value = conditionsOpDelta -> ()
            | _ -> failwith "Unexpected branch of match expression"
        let mutable conditionData = {ConditionData.Conditions = []; ConditionData.Operators = []}
        while followLexems |> List.contains lexems.Current |> not do
            match lexems.Current with
            | Lexem.OpenBracket ->
                conditionData |> checkAddAbility 0
                conditionData <- this.ParseBoolExprInBrackets(lexems, conditionData)
            | Lexem.Number _ | Lexem.Variable _ ->
                conditionData |> checkAddAbility 0
                conditionData <- this.ParseComparisonExpr(lexems, followLexems, conditionData)
            | Lexem.True | Lexem.False ->
                conditionData |> checkAddAbility 0
                conditionData <- (lexems.Current = Lexem.True) |> outputGenerator.GenerateForBoolValue |> appendCondition conditionData
                lexems.MoveNext() |> ignore
            | Lexem.OpAnd | Lexem.OpOr ->
                conditionData |> checkAddAbility 1
                conditionData <- {conditionData with ConditionData.Operators = conditionData.Operators @ [lexems.Current]}
                lexems.MoveNext() |> ignore
            | _ -> failwith "Unexpected branch of match expression"
        conditionData |> finishCondition

    member private this.ParseBoolExprInBrackets(lexems: IEnumerator<Lexem>, conditionData: ConditionData) =
        lexems.MoveNext() |> ignore
        let conditionData = this.ParseBoolExpr(lexems, [Lexem.CloseBracket]) |> appendCondition conditionData
        match lexems.Current with
        | Lexem.CloseBracket ->
            lexems.MoveNext() |> ignore
            conditionData
        | _ -> failwith "Unexpected branch of match expression"

    member private this.ParseComparisonExpr(lexems: IEnumerator<Lexem>, followLexems: Lexem list, conditionData: ConditionData) =
        let comparisionLeft = this.ParseArithExpr(lexems, [Lexem.OpGreater; Lexem.OpLess])
        match lexems.Current with
        | Lexem.OpGreater | Lexem.OpLess ->
            let comparisionOp = lexems.Current
            lexems.MoveNext() |> ignore
            let comparisionRight = this.ParseArithExpr(lexems, Lexem.OpAnd :: Lexem.OpOr :: followLexems)
            match comparisionOp with
            | Lexem.OpGreater -> outputGenerator.GenerateForGreater(comparisionLeft, comparisionRight) |> appendCondition conditionData
            | Lexem.OpLess -> outputGenerator.GenerateForLess(comparisionLeft, comparisionRight) |> appendCondition conditionData
            | _ -> failwith "Unexpected branch of match expression"
        | _ -> failwith "Unexpected branch of match expression"

type WhileLanguageDirectImplTask(input: TextReader, output: TextWriter) =

    let executeProgram (source: seq<string>) =
        let lexParser = new LexParser()
        let syntaxParser = new SyntaxParser()
        let program = source |> lexParser.Parse |> syntaxParser.Parse
        let context = new Dictionary<string, int64>()
        context |> program
        context.Keys |> Seq.sort |> Seq.iter (fun key -> output.WriteLine("{0} {1}", key, context.[key]))

    interface ITask with
        member this.Execute(argv: string[]) =
            Seq.initInfinite (fun _ -> input.ReadLine()) |> Seq.takeWhile (fun line -> line <> null) |> executeProgram
            0

[<TestFixture>]
type WhileLanguageDirectImplTests() =

    [<Literal>]
    let RootDirectory = ".//TestCases//InterpreterCompilers//WhileLanguage"

    [<TestCase("Input00.txt", "Output00.txt")>]
    [<TestCase("Input01.txt", "Output01.txt")>]
    [<TestCase("Input02.txt", "Output02.txt")>]
    [<TestCase("Input03.txt", "Output03.txt")>]
    [<TestCase("Input04.txt", "Output04.txt")>]
    [<TestCase("Input05.txt", "Output05.txt")>]
    [<TestCase("Input06.txt", "Output06.txt")>]
    [<TestCase("Input07.txt", "Output07.txt")>]
    member public this.Execute(inputFile: string, expectedOutputFile: string) =
        let input = File.ReadAllText(Path.Combine(RootDirectory, inputFile))
        let expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile))
        TaskExecutor.Execute((fun reader writer -> new WhileLanguageDirectImplTask(reader, writer) :> ITask), input, expectedOutput)