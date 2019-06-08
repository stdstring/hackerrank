// from https://www.hackerrank.com/challenges/sparse-arrays

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Arrays
{
    public class SparseArraysTask : ITask
    {
        public SparseArraysTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            IDictionary<String, Int32> storage = new Dictionary<String, Int32>();
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            for (Int32 index = 0; index < n; ++index)
            {
                String value = _inputReader.ReadLine();
                if (!storage.ContainsKey(value))
                    storage[value] = 0;
                storage[value] += 1;
            }
            Int32 q = Int32.Parse(_inputReader.ReadLine());
            Int32[] results = new Int32[q];
            for (Int32 index = 0; index < q; ++index)
            {
                String query = _inputReader.ReadLine();
                results[index] = storage.ContainsKey(query) ? storage[query] : 0;
            }
            foreach (Int32 result in results)
            {
                _outputWriter.WriteLine(result);
            }

            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class SparseArraysTests
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
        [TestCase("Input11.txt", "Output11.txt")]
        [TestCase("Input12.txt", "Output12.txt")]
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new SparseArraysTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Arrays//SparseArrays";
    }
}
