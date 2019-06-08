// from https://www.hackerrank.com/challenges/array-left-rotation

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Arrays
{
    public class LeftRotationTask : ITask
    {
        public LeftRotationTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] inputData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 n = inputData[0];
            Int32 d = inputData[1];
            Int32[] source = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32[] dest = new Int32[n];
            for (Int32 index = 0; index < d; ++index)
            {
                dest[n - d + index] = source[index];
            }
            for (Int32 index = d; index < n; ++index)
            {
                dest[index - d] = source[index];
            }
            _outputWriter.WriteLine(String.Join(" ", dest));
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class LeftRotationTests
    {
        [TestCase("Input00.txt", "Output00.txt")]
        [TestCase("Input01.txt", "Output01.txt")]
        [TestCase("Input02.txt", "Output02.txt")]
        [TestCase("Input03.txt", "Output03.txt")]
        [TestCase("Input04.txt", "Output04.txt")]
        [TestCase("Input05.txt", "Output05.txt")]
        [TestCase("Input06.txt", "Output06.txt")]
        [TestCase("Input07.txt", "Output07.txt")]
        [TestCase("Input08.txt", "Output08.txt")]
        [TestCase("Input09.txt", "Output09.txt")]
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new LeftRotationTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Arrays//LeftRotation";
    }
}
