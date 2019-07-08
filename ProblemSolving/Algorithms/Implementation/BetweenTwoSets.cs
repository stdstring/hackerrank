// from https://www.hackerrank.com/challenges/between-two-sets

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class BetweenTwoSetsTask : ITask
    {
        public BetweenTwoSetsTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] nmValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 nValue = nmValues[0];
            Int32 mValue = nmValues[1];
            Int32[] aValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32[] bValues = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 aMax = aValues.Max();
            Int32 bMin = bValues.Min();
            Int32 count = 0;
            if (CheckNumber(aMax, aValues, bValues))
                ++count;
            if (CheckNumber(bMin, aValues, bValues))
                ++count;
            for (Int32 number = aMax * 2; number <= bMin / 2; ++number)
            {
                if (CheckNumber(number, aValues, bValues))
                    ++count;
            }
            _outputWriter.WriteLine(count);
            return 0;
        }

        private Boolean CheckNumber(Int32 number, Int32[] aValues, Int32[] bValues)
        {
            foreach (Int32 aValue in aValues)
            {
                if (number % aValue != 0)
                    return false;
            }
            foreach (Int32 bValue in bValues)
            {
                if (bValue % number != 0)
                    return false;
            }
            return true;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class BetweenTwoSetsTests
    {
        [TestCase("2 3\r\n2 4\r\n16 32 96", "3")]
        [TestCase("10 10\r\n100 99 98 97 96 95 94 93 92 91\r\n1 2 3 4 5 6 7 8 9 10", "0")]
        [TestCase("1 3\r\n2\r\n20 30 12", "1")]
        [TestCase("3 2\r\n3 9 6\r\n36 72", "2")]
        [TestCase("1 1\r\n1\r\n100", "9")]
        [TestCase("1 1\r\n51\r\n50", "0")]
        [TestCase("3 2\r\n2 3 6\r\n42 84", "2")]
        [TestCase("1 2\r\n1\r\n72 48", "8")]
        [TestCase("2 2\r\n3 4\r\n24 48", "2")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new BetweenTwoSetsTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
