// from https://www.hackerrank.com/challenges/crosswords-101

namespace FunctionalProgramming.FSharp.Recursion

open NUnit.Framework
open ProblemSolving.Common
open System.Collections.Generic
open System.IO

type Cell =
    | Unused
    | WithoutValue
    | WithValue of value: char

type TField = Cell[,]

type ObjectType =
    | Row
    | Column

type WordObject = {StartRow: int; StartColumn: int; Size: int; Type: ObjectType}

type Crossword101State =
    {Field: TField; Words: ResizeArray<string>; Objects: ResizeArray<WordObject>}

    member public this.Clone() =
        let field = this.Field |> Array2D.copy
        let words = new ResizeArray<string>(this.Words)
        let objects = new ResizeArray<WordObject>(this.Objects)
        {Crossword101State.Field = field;
         Crossword101State.Words = words;
         Crossword101State.Objects = objects}

type Crosswords101Task(input: TextReader, output: TextWriter) =

    [<Literal>]
    let crosswordSize = 10

    let parseField (source: string[]) =
        let dest = Cell.Unused |> Array2D.create crosswordSize crosswordSize
        for row in seq {0 .. crosswordSize - 1} do
            for column in seq{0 .. crosswordSize - 1} do
                if source.[row].[column] = '-' then
                    dest.[row, column] <- Cell.WithoutValue
        dest

    let addWordObject (row: int) (column: int) (size: int) (objectType: ObjectType) (storage: ResizeArray<WordObject>) =
        match size with
        | 1 -> ()
        | value when value > 1 ->
            {WordObject.StartRow = row;
             WordObject.StartColumn = column;
             WordObject.Size = size;
             WordObject.Type = objectType} |> storage.Add
        | _ ->
            failwith "Bad value of size"

    let parseWordObjects (source: string[]) =
        let dest = new ResizeArray<WordObject>()
        for row in seq {0 .. crosswordSize - 1} do
            let mutable currentObject = None
            for column in seq{0 .. crosswordSize - 1} do
                match source.[row].[column], currentObject with
                | '-', None ->
                    currentObject <- column |> Some
                | '+', Some start ->
                    addWordObject row start (column - start) ObjectType.Row dest
                    currentObject <- None
                | _ -> ()
            match currentObject with
            | Some start ->
                addWordObject row start (crosswordSize - start) ObjectType.Row dest
            | None -> ()
        for column in seq{0 .. crosswordSize - 1} do
            let mutable currentObject = None
            for row in seq {0 .. crosswordSize - 1} do
                match source.[row].[column], currentObject with
                | '-', None ->
                    currentObject <- row |> Some
                | '+', Some start ->
                    addWordObject start column (row - start) ObjectType.Column dest
                    currentObject <- None
                | _ -> ()
            match currentObject with
            | Some start ->
                addWordObject start column (crosswordSize - start) ObjectType.Column dest
            | None -> ()
        dest

    let canWriteLetter (cell: Cell) (letter: char) =
        match cell with
        | Unused -> false
        | WithoutValue -> true
        | WithValue value -> value = letter

    let canWriteWord (field: TField) (object: WordObject) (word: string) =
        match object.Size = word.Length with
        | false -> false
        | true ->
            let row, column = object.StartRow, object.StartColumn
            let deltaRow, deltaColumn = match object.Type with
                                        | ObjectType.Row -> 0, 1
                                        | ObjectType.Column -> 1, 0
            seq {0 .. word.Length - 1} |> Seq.exists (fun index -> word.[index] |> canWriteLetter field.[row + deltaRow * index, column + deltaColumn * index] |> not) |> not

    let createWordEntry (dest: Dictionary<string, List<WordObject>>) (word: string) =
        dest.Add(word, new List<WordObject>())
        dest

    let createObjectEntry (dest: Dictionary<WordObject, List<string>>) (object: WordObject) =
        dest.Add(object, new List<string>())
        dest

    let generatePossibleSteps (state: Crossword101State) =
        let wordsPossibleSteps = state.Words |> Seq.fold createWordEntry (new Dictionary<string, List<WordObject>>())
        let objectsPossibleSteps = state.Objects |> Seq.fold createObjectEntry (new Dictionary<WordObject, List<string>>())
        for objectStepData in objectsPossibleSteps do
            for wordStepData in wordsPossibleSteps do
                if canWriteWord state.Field objectStepData.Key wordStepData.Key then
                    objectStepData.Value.Add(wordStepData.Key) |> ignore
                    wordStepData.Value.Add(objectStepData.Key) |> ignore
        wordsPossibleSteps, objectsPossibleSteps

    let checkPossibleSteps (wordsPossibleSteps: Dictionary<string, List<WordObject>>)
                           (objectsPossibleSteps: Dictionary<WordObject, List<string>>) =
        let hasEmpty = wordsPossibleSteps |> Seq.exists (fun entry -> entry.Value.Count = 0) ||
                       objectsPossibleSteps |> Seq.exists (fun entry -> entry.Value.Count = 0)
        hasEmpty |> not

    let writeWord (state: Crossword101State) (object: WordObject) (word: string) =
        let row, column = object.StartRow, object.StartColumn
        let deltaRow, deltaColumn = match object.Type with
                                    | ObjectType.Row -> 0, 1
                                    | ObjectType.Column -> 1, 0
        seq {0 .. word.Length - 1} |> Seq.iter (fun index -> state.Field.[row + deltaRow * index, column + deltaColumn * index] <- word.[index] |> WithValue)
        word |> state.Words.Remove |> ignore
        object |> state.Objects.Remove |> ignore
        state

    let applyMandatorySteps (state: Crossword101State)
                            (wordsPossibleSteps: Dictionary<string, List<WordObject>>)
                            (objectsPossibleSteps: Dictionary<WordObject, List<string>>) =
        let wordsSteps = new Dictionary<string, List<WordObject>>(wordsPossibleSteps |> Seq.filter (fun entry -> entry.Value.Count = 1))
        let objectsSteps = new Dictionary<WordObject, List<string>>(objectsPossibleSteps |> Seq.filter (fun entry -> entry.Value.Count = 1))
        let hasMandatorySteps = (wordsSteps.Count > 0) || (objectsSteps.Count > 0)
        for wordStep in wordsSteps do
            let object = wordStep.Value.[0]
            writeWord state object wordStep.Key |> ignore
            object |> objectsSteps.Remove |> ignore
        for objectsStep in objectsSteps do
            let word = objectsStep.Value.[0]
            writeWord state objectsStep.Key word |> ignore
        hasMandatorySteps

    let cellToChar (cell: Cell) =
        match cell with
        | Cell.Unused -> '+'
        | Cell.WithoutValue -> '-'
        | Cell.WithValue value -> value

    let fieldRowToStrings (field: TField) (row: int) =
        System.String.Join("", seq {0 .. crosswordSize - 1} |> Seq.map (fun column -> field.[row, column] |> cellToChar))

    member private this.FindSolution(state: Crossword101State) =
        match state.Words.Count = 0 with
        | true -> state.Field |> Some
        | false ->
            let wordsPossibleSteps, objectsPossibleSteps = state |> generatePossibleSteps
            match checkPossibleSteps wordsPossibleSteps objectsPossibleSteps with
            | false -> None
            | true ->
                match applyMandatorySteps state wordsPossibleSteps objectsPossibleSteps with
                | true -> state |> this.FindSolution
                | false -> state |> this.GuessWord

    member private this.TryGuess(state: Crossword101State, object: WordObject, word: string) =
        match canWriteWord state.Field object word with
        | false -> None
        | true ->
            let stateClone = state.Clone()
            writeWord stateClone object word |> ignore
            stateClone |> this.FindSolution

    member private this.GuessWord(state: Crossword101State) =
        Seq.map2 (fun object word -> state, object, word) state.Objects state.Words |> Seq.tryPick this.TryGuess

    interface ITask with
        member this.Execute(argv: string[]) =
            let fieldSource = seq {0 .. crosswordSize - 1} |> Seq.map (fun _ -> input.ReadLine()) |> Seq.toArray
            let wordsSource = input.ReadLine()
            let field = fieldSource |> parseField
            let objects = fieldSource |> parseWordObjects
            let words = new ResizeArray<string>(wordsSource.Split(';'))
            let state = {Crossword101State.Field = field; Crossword101State.Words = words; Crossword101State.Objects = objects}
            let result = state |> this.FindSolution
            match result with
            | None ->
                Assert.Fail("solution isn't found")
            | Some finalState ->
                seq {0 .. crosswordSize - 1} |> Seq.iter (fun row -> row |> fieldRowToStrings finalState |> output.WriteLine)
            0

[<TestFixture>]
type Crosswords101Tests() =

    [<TestCase([|"+-++++++++";
                 "+-++++++++";
                 "+-++++++++";
                 "+-----++++";
                 "+-+++-++++";
                 "+-+++-++++";
                 "+++++-++++";
                 "++------++";
                 "+++++-++++";
                 "+++++-++++";
                 "LONDON;DELHI;ICELAND;ANKARA"|],
               [|"+L++++++++";
                 "+O++++++++";
                 "+N++++++++";
                 "+DELHI++++";
                 "+O+++C++++";
                 "+N+++E++++";
                 "+++++L++++";
                 "++ANKARA++";
                 "+++++N++++";
                 "+++++D++++"|])>]
    [<TestCase([|"+-++++++++";
                 "+-++++++++";
                 "+-------++";
                 "+-++++++++";
                 "+-++++++++";
                 "+------+++";
                 "+-+++-++++";
                 "+++++-++++";
                 "+++++-++++";
                 "++++++++++";
                 "AGRA;NORWAY;ENGLAND;GWALIOR"|],
               [|"+E++++++++";
                 "+N++++++++";
                 "+GWALIOR++";
                 "+L++++++++";
                 "+A++++++++";
                 "+NORWAY+++";
                 "+D+++G++++";
                 "+++++R++++";
                 "+++++A++++";
                 "++++++++++"|])>]
    [<TestCase([|"+-++++++++";
                 "+-++-+++++";
                 "+-------++";
                 "+-++-+++++";
                 "+-++-+++++";
                 "+-++-+++++";
                 "++++-+++++";
                 "++++-+++++";
                 "++++++++++";
                 "----------";
                 "CALIFORNIA;NIGERIA;CANADA;TELAVIV"|],
               [|"+C++++++++";
                 "+A++T+++++";
                 "+NIGERIA++";
                 "+A++L+++++";
                 "+D++A+++++";
                 "+A++V+++++";
                 "++++I+++++";
                 "++++V+++++";
                 "++++++++++";
                 "CALIFORNIA"|])>]
    [<TestCase([|"+-++++++++";
                 "+-++-+++++";
                 "+-------++";
                 "+-++-++-++";
                 "+-++-++-++";
                 "+-++-++-++";
                 "++++-++-++";
                 "+--------+";
                 "++++++++++";
                 "----------";
                 "CALIFORNIA;LASVEGAS;NIGERIA;CANADA;TELAVIV;ALASKA"|],
               [|"+C++++++++";
                 "+A++T+++++";
                 "+NIGERIA++";
                 "+A++L++L++";
                 "+D++A++A++";
                 "+A++V++S++";
                 "++++I++K++";
                 "+LASVEGAS+";
                 "++++++++++";
                 "CALIFORNIA"|])>]
    [<TestCase([|"+-++++++++";
                 "+-++++++++";
                 "+-------++";
                 "+-++++++++";
                 "+-----++++";
                 "+-+++-++++";
                 "+++-----++";
                 "+++++-++++";
                 "+++++-++++";
                 "+++++-++++";
                 "SYDNEY;TURKEY;DETROIT;EGYPT;PARIS"|],
               [|"+S++++++++";
                 "+Y++++++++";
                 "+DETROIT++";
                 "+N++++++++";
                 "+EGYPT++++";
                 "+Y+++U++++";
                 "+++PARIS++";
                 "+++++K++++";
                 "+++++E++++";
                 "+++++Y++++"|])>]
    [<TestCase([|"+----+++++";
                 "++++-+++++";
                 "++++-+++++";
                 "++++------";
                 "++++-+++-+";
                 "++++-+++-+";
                 "++++-+++-+";
                 "++++-+++-+";
                 "++++-+++++";
                 "++++++++++";
                 "TREE;ELEPHANTS;PICKLE;LEMON"|],
               [|"+TREE+++++";
                 "++++L+++++";
                 "++++E+++++";
                 "++++PICKLE";
                 "++++H+++E+";
                 "++++A+++M+";
                 "++++N+++O+";
                 "++++T+++N+";
                 "++++S+++++";
                 "++++++++++"|])>]
    [<TestCase([|"+-++++++++";
                 "+-------++";
                 "+-++-+++++";
                 "+-------++";
                 "+-++-++++-";
                 "+-++-++++-";
                 "+-++------";
                 "+++++++++-";
                 "++++++++++";
                 "++++++++++";
                 "ANDAMAN;MANIPUR;ICELAND;ALLEPY;YANGON;PUNE"|],
               [|"+M++++++++";
                 "+ANDAMAN++";
                 "+N++L+++++";
                 "+ICELAND++";
                 "+P++E++++P";
                 "+U++P++++U";
                 "+R++YANGON";
                 "+++++++++E";
                 "++++++++++";
                 "++++++++++"|])>]
    member public _.Execute(input: string[], expectedOutput: string[]) =
        TaskExecutor.Execute((fun reader writer -> new Crosswords101Task(reader, writer) :> ITask), input, expectedOutput)