// from https://www.hackerrank.com/challenges/tutorial-intro

using System;
using System.IO;
using System.Linq;
using Algorithm.Common;
using NUnit.Framework;

namespace Algorithms.Impl.Sorting
{
    public class SortingIntroTask : ITask
    {
        public SortingIntroTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 value = Int32.Parse(_inputReader.ReadLine());
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            Int32[] numbers = _inputReader.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
            _outputWriter.WriteLine(FindIndex());
            return 0;

            Int32 FindIndex()
            {
                Int32 start = 0;
                Int32 finish = numbers.Length - 1;
                Int32 middle = CalcMiddleIndex(start, finish);
                while (numbers[middle] != value)
                {
                    start = value < numbers[middle] ? start : middle;
                    finish = value < numbers[middle] ? middle : finish;
                    middle = CalcMiddleIndex(start, finish);
                }
                return middle;
            }

            Int32 CalcMiddleIndex(Int32 start, Int32 finish)
            {
                Int32 middle = (start + finish) / 2;
                if (middle == start)
                    return finish;
                else if (middle == finish)
                    return start;
                else
                    return middle;
            }
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class SortingIntroTests
    {
        [TestCase("Input00.txt", "1")]
        [TestCase("Input01.txt", "11")]
        [TestCase("Input02.txt", "9")]
        [TestCase("Input03.txt", "6")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecuter.Execute((inputReader, outputWriter) => new SortingIntroTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Sorting//SortingIntro";
    }
}
