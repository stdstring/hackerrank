using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ProblemSolving.Common
{
    public static class TaskExecutor
    {
        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input, String expectedOutput)
        {
            Execute(taskFactory, input, expectedOutput.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None));
        }

        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String[] input, String[] expectedOutput)
        {
            Execute(taskFactory, String.Join("\n", input), expectedOutput);
        }

        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input, String[] expectedOutput)
        {
            String[] actualOutput = Execute(taskFactory, input);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }

        public static void Execute<TValue>(Func<TextReader, TextWriter, ITask> taskFactory, String[] input, TValue[] expectedOutput, Action<TValue, String> valueChecker)
        {
            Execute(taskFactory, String.Join("\n", input), expectedOutput, valueChecker);
        }

        public static void Execute<TValue>(Func<TextReader, TextWriter, ITask> taskFactory, String input, TValue[] expectedOutput, Action<TValue, String> valueChecker)
        {
            String[] actualOutput = Execute(taskFactory, input);
            Assert.That(actualOutput.Length, Is.EqualTo(expectedOutput.Length));
            for (Int32 index = 0; index < expectedOutput.Length; ++index)
            {
                valueChecker(expectedOutput[index], actualOutput[index]);
            }
        }

        private static String[] Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input)
        {
            using (TextReader inputReader = new StringReader(input))
            {
                using (TextWriter outputWriter = new StringWriter())
                {
                    ITask task = taskFactory(inputReader, outputWriter);
                    Int32 result = task.Execute(new String[0]);
                    Assert.That(result, Is.EqualTo(0));
                    return outputWriter.ToString()?
                        .Split(new[] {"\r\n", "\n"}, StringSplitOptions.None)
                        .Select(value => value.TrimEnd())
                        .Reverse()
                        .SkipWhile(String.IsNullOrWhiteSpace)
                        .Reverse()
                        .ToArray();
                }
            }
        }
    }
}
