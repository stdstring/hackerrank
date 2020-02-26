// from https://www.hackerrank.com/challenges/crush

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Arrays
{
    public class ArrayManipulationTask : ITask
    {
        public ArrayManipulationTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] inputData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 n = inputData[0];
            Int32 m = inputData[1];
            Int64[] diffStorage = new Int64[n];
            for (Int32 index = 0; index < m; ++index)
            {
                Int32[] queryData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
                Int32 a = queryData[0];
                Int32 b = queryData[1];
                Int32 k = queryData[2];
                diffStorage[a - 1] += k;
                if (b < n)
                    diffStorage[b] -= k;
            }
            Int64 currentValue = diffStorage[0];
            Int64 maxValue = currentValue;
            for (Int32 index = 1; index < diffStorage.Length; ++index)
            {
                currentValue += diffStorage[index];
                if (currentValue > maxValue)
                    maxValue = currentValue;
            }
            _outputWriter.WriteLine(maxValue);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class ArrayManipulationTests
    {
        [TestCase("Input00.txt", "200")]
        [TestCase("Input01.txt", "882")]
        [TestCase("Input02.txt", "8628")]
        [TestCase("Input03.txt", "6314")]
        [TestCase("Input04.txt", "7542539201")]
        [TestCase("Input05.txt", "7496167325")]
        [TestCase("Input06.txt", "7515267971")]
        [TestCase("Input07.txt", "2497169732")]
        [TestCase("Input08.txt", "2484930878")]
        [TestCase("Input09.txt", "2501448788")]
        [TestCase("Input10.txt", "2510535321")]
        [TestCase("Input11.txt", "2506721627")]
        [TestCase("Input12.txt", "2517519438")]
        [TestCase("Input13.txt", "2490686975")]
        [TestCase("Input14.txt", "10")]
        [TestCase("Input15.txt", "31")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new ArrayManipulationTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Arrays//ArrayManipulation";
    }
}
