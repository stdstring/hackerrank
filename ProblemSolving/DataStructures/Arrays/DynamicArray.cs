// from https://www.hackerrank.com/challenges/dynamic-array

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Arrays
{
    public class DynamicArrayTask : ITask
    {
        public DynamicArrayTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] inputData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 n = inputData[0];
            Int32 q = inputData[1];
            ProcessQueries(n, q, new IList<Int32>[n]);
            return 0;
        }

        private void ProcessQueries(Int32 n, Int32 q, IList<Int32>[] storage)
        {
            Int32 lastAnswer = 0;
            IDictionary<Int32, Action<Int32, Int32>> handlers = new Dictionary<Int32, Action<Int32, int>>
            {
                {1,(x, y) =>
                {
                    Int32 index = (lastAnswer ^ x) % n;
                    if (storage[index] == null)
                        storage[index] = new List<Int32>();
                    storage[index].Add(y);
                }},
                {2, (x, y) =>
                {
                    Int32 index = (lastAnswer ^ x) % n;
                    lastAnswer = storage[index][y % storage[index].Count];
                    _outputWriter.WriteLine(lastAnswer);
                }}
            };
            for (Int32 index = 0; index < q; ++index)
            {
                Int32[] queryData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
                handlers[queryData[0]](queryData[1], queryData[2]);
            }
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class DynamicArrayTests
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
        [TestCase("Input10.txt", "Output10.txt")]
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new DynamicArrayTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Arrays//DynamicArray";
    }
}
