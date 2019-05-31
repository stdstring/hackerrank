// from https://www.hackerrank.com/challenges/staircase

using System;
using System.IO;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Warmup
{
    public class StaircaseTask : ITask
    {
        public StaircaseTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            for (Int32 row = 1; row <= n; ++row)
            {
                String leftPart = new String(' ', n - row);
                String rightPart = new String('#', row);
                _outputWriter.WriteLine("{0}{1}", leftPart, rightPart);
            }
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class StaircaseTests
    {
        [TestCase("95", "Output00.txt")]
        [TestCase("6", "Output01.txt")]
        [TestCase("95", "Output02.txt")]
        [TestCase("100", "Output03.txt")]
        [TestCase("8", "Output04.txt")]
        [TestCase("18", "Output05.txt")]
        [TestCase("25", "Output06.txt")]
        [TestCase("49", "Output07.txt")]
        [TestCase("66", "Output08.txt")]
        public void Execute(String input, String expectedOutputFile)
        {
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new StaircaseTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Warmup//Staircase";
    }
}
