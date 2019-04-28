namespace FunctionalProgramming.Common

open System.IO
open NUnit.Framework

[<AbstractClass; Sealed>]
type TaskExecuter =

    static member public Execute(taskFactory: TextReader -> TextWriter -> ITask, input: string, expectedOutput: string) =
        TaskExecuter.Execute(taskFactory, input, expectedOutput.Split("\r\n"))

    static member public Execute(taskFactory: TextReader -> TextWriter -> ITask, input: string[], expectedOutput: string[]) =
        TaskExecuter.Execute(taskFactory, System.String.Join("\r\n", input), expectedOutput)

    static member public Execute(taskFactory: TextReader -> TextWriter -> ITask, input: string, expectedOutput: string[]) =
        let actualOutput = TaskExecuter.ExecuteImpl(taskFactory, input)
        Assert.AreEqual(expectedOutput, actualOutput)

    static member public Execute(taskFactory: TextReader -> TextWriter -> ITask, input: string[], expectedOutput: 'T[], valueChecker: 'T -> string -> unit) =
        TaskExecuter.Execute(taskFactory, System.String.Join("\r\n", input), expectedOutput, valueChecker)

    static member public Execute(taskFactory: TextReader -> TextWriter -> ITask, input: string, expectedOutput: 'T[], valueChecker: 'T -> string -> unit) =
        let actualOutput: string[] = TaskExecuter.ExecuteImpl(taskFactory, input)
        Assert.AreEqual(expectedOutput.Length, actualOutput.Length)
        Seq.iter2 valueChecker expectedOutput actualOutput

    static member private ExecuteImpl(taskFactory: TextReader -> TextWriter -> ITask, input: string) =
        use inputReader = new StringReader(input)
        use outputWriter = new StringWriter()
        let task = taskFactory inputReader outputWriter
        let result = [||] |> task.Execute
        Assert.AreEqual(0, result)
        outputWriter.ToString().Split([|"\r\n"; "\n"|], System.StringSplitOptions.None) |> Seq.map (fun value -> value.TrimEnd()) |>
                                                                                           Seq.rev |>
                                                                                           Seq.skipWhile (fun value -> value |> System.String.IsNullOrWhiteSpace) |>
                                                                                           Seq.rev |>
                                                                                           Seq.toArray