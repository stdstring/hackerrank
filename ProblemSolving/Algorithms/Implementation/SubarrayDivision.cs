// from https://www.hackerrank.com/challenges/the-birthday-bar

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class SubarrayDivisionTask : ITask
    {
        public SubarrayDivisionTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            IList<Int32> source = _inputReader.ReadLine().Split().Select(Int32.Parse).ToList();
            Int32[] dayMonthValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 dayValue = dayMonthValues[0];
            Int32 monthValue = dayMonthValues[1];
            _outputWriter.WriteLine(CalcDivisionWaysCount(source, dayValue, monthValue));
            return 0;
        }

        public Int32 CalcDivisionWaysCount(IList<Int32> source, Int32 day, Int32 month)
        {
            if (source.Count < month)
                return 0;
            Int32 waysCount = 0;
            Int32 segmentSum = source.Take(month).Sum();
            if (segmentSum == day)
                ++waysCount;
            for (Int32 index = month; index < source.Count; ++index)
            {
                segmentSum += (source[index] - source[index - month]);
                if (segmentSum == day)
                    ++waysCount;
            }
            return waysCount;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class SubarrayDivisionTests
    {
        [TestCase("5\r\n1 2 1 3 2\r\n3 2", "2")]
        [TestCase("6\r\n1 1 1 1 1 1\r\n3 2", "0")]
        [TestCase("1\r\n4\r\n4 1", "1")]
        [TestCase("19\r\n2 5 1 3 4 4 3 5 1 1 2 1 4 1 3 3 4 2 1\r\n18 7", "3")]
        [TestCase("13\r\n4 5 4 2 4 5 2 3 2 1 1 5 4\r\n15 4", "3")]
        [TestCase("7\r\n5 2 2 1 5 3 2\r\n9 3", "2")]
        [TestCase("31\r\n2 2 2 1 3 2 2 3 3 1 4 1 3 2 2 1 2 2 4 2 2 3 5 3 4 3 2 1 4 5 4\r\n10 4", "7")]
        [TestCase("34\r\n4 5 4 5 1 2 1 4 3 2 4 4 3 5 2 2 5 4 3 2 3 5 2 1 5 2 3 1 2 3 3 1 2 5\r\n18 6", "6")]
        [TestCase("21\r\n5 1 4 1 5 4 5 1 3 5 1 1 5 1 4 2 1 1 1 2 5\r\n15 7", "3")]
        [TestCase("16\r\n5 5 3 2 2 2 1 2 5 3 5 5 4 3 3 5\r\n13 3", "3")]
        [TestCase("55\r\n3 5 4 1 2 5 3 4 3 2 1 1 2 4 2 3 4 5 3 1 2 5 4 5 4 1 1 5 3 1 4 5 2 3 2 5 2 5 2 2 1 5 3 2 5 1 2 4 3 1 5 1 3 3 5\r\n18 6", "10")]
        [TestCase("90\r\n4 1 4 3 3 5 1 2 4 2 5 1 5 1 4 1 3 1 5 2 2 2 1 1 3 2 5 3 1 5 4 5 2 2 1 1 2 2 4 5 4 1 5 2 1 1 2 2 1 3 2 4 4 1 3 2 2 3 1 5 4 4 1 4 2 1 2 1 5 1 3 3 4 2 1 5 5 4 2 2 3 3 4 3 1 2 1 2 4 3\r\n16 7", "13")]
        [TestCase("20\r\n5 1 2 4 4 2 4 2 2 5 1 4 3 1 1 1 2 1 4 1\r\n18 6", "4")]
        [TestCase("82\r\n2 3 4 4 2 1 2 5 3 4 4 3 4 1 3 5 4 5 3 1 1 5 4 3 5 3 5 3 4 4 2 4 5 2 3 2 5 3 4 2 4 3 3 4 3 5 2 5 1 3 1 4 2 2 4 3 3 3 3 4 1 1 4 3 1 5 2 5 1 3 5 4 3 3 1 5 3 3 3 4 5 2\r\n26 8", "16")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new SubarrayDivisionTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
