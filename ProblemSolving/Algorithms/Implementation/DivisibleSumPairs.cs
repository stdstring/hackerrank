// from https://www.hackerrank.com/challenges/divisible-sum-pairs

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class DivisibleSumPairsTask : ITask
    {
        public DivisibleSumPairsTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] nkValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 nValue = nkValues[0];
            Int32 kValue = nkValues[1];
            Int32[] aValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 count = 0;
            for (Int32 i = 0; i < nValue; ++i)
            {
                for (Int32 j = i + 1; j < nValue; ++j)
                {
                    if ((aValues[i] + aValues[j]) % kValue == 0)
                        ++count;
                }
            }
            _outputWriter.WriteLine(count);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class DivisibleSumPairsTests
    {
        [TestCase("Input00.txt", "5")]
        [TestCase("Input01.txt", "1")]
        [TestCase("Input02.txt", "4")]
        [TestCase("Input03.txt", "3")]
        [TestCase("Input04.txt", "15")]
        [TestCase("Input05.txt", "16")]
        [TestCase("Input06.txt", "216")]
        [TestCase("Input07.txt", "69")]
        [TestCase("Input08.txt", "59")]
        [TestCase("Input09.txt", "254")]
        [TestCase("Input10.txt", "182")]
        [TestCase("Input11.txt", "51")]
        [TestCase("Input12.txt", "44")]
        [TestCase("Input13.txt", "176")]
        [TestCase("Input14.txt", "158")]
        [TestCase("Input15.txt", "70")]
        [TestCase("Input16.txt", "109")]
        [TestCase("Input17.txt", "116")]
        [TestCase("Input18.txt", "36")]
        [TestCase("Input19.txt", "65")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new DivisibleSumPairsTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Implementation//DivisibleSumPairs";
    }
}