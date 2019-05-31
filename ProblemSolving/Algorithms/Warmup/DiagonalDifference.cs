// from https://www.hackerrank.com/challenges/diagonal-difference

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Warmup
{
    public class DiagonalDifferenceTask : ITask
    {
        public DiagonalDifferenceTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 size = Int32.Parse(_inputReader.ReadLine());
            Int32 result = 0;
            for (Int32 index = 0; index < size; ++index)
            {
                Int32[] row = _inputReader.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
                result += row[index];
                result -= row[size - 1 - index];
            }
            _outputWriter.WriteLine(Math.Abs(result));
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class DiagonalDifferenceTests
    {
        [TestCase("Input00.txt", "15")]
        [TestCase("Input01.txt", "1")]
        [TestCase("Input02.txt", "52")]
        [TestCase("Input03.txt", "3")]
        [TestCase("Input04.txt", "13")]
        [TestCase("Input05.txt", "0")]
        [TestCase("Input06.txt", "327")]
        [TestCase("Input07.txt", "462")]
        [TestCase("Input08.txt", "11600")]
        [TestCase("Input09.txt", "20000")]
        [TestCase("Input10.txt", "19600")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new DiagonalDifferenceTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Warmup//DiagonalDifference";
    }
}
