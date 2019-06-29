// from https://www.hackerrank.com/challenges/birthday-cake-candles

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Warmup
{
    public class BirthdayCakeCandlesTask : ITask
    {
        public BirthdayCakeCandlesTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            Int32[] heights = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 maxHeight = 0;
            Int32 maxHeightCount = 0;
            for (Int32 index = 0; index < n; ++index)
            {
                if (heights[index] == maxHeight)
                {
                    ++maxHeightCount;
                }
                if (heights[index] > maxHeight)
                {
                    maxHeight = heights[index];
                    maxHeightCount = 1;
                }
            }
            _outputWriter.WriteLine(maxHeightCount);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class BirthdayCakeCandlesTests
    {
        [TestCase("Input00.txt", "2")]
        [TestCase("Input01.txt", "5")]
        [TestCase("Input02.txt", "4")]
        [TestCase("Input03.txt", "2")]
        [TestCase("Input04.txt", "7174")]
        [TestCase("Input05.txt", "12443")]
        [TestCase("Input06.txt", "100000")]
        [TestCase("Input07.txt", "100000")]
        [TestCase("Input08.txt", "99999")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new BirthdayCakeCandlesTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Warmup//BirthdayCakeCandles";
    }
}
