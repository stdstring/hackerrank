// from https://www.hackerrank.com/challenges/kangaroo

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class KangarooTask : ITask
    {
        public KangarooTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] data = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 x1 = data[0];
            Int32 v1 = data[1];
            Int32 x2 = data[2];
            Int32 v2 = data[3];
            if (v1 == v2)
            {
                _outputWriter.WriteLine(x1 == x2 ? "YES" : "NO");
            }
            else if (Math.Sign(x1 - x2) == Math.Sign(v2 - v1))
            {
                _outputWriter.WriteLine((x1 - x2) % (v2 - v1) == 0 ? "YES" : "NO");
            }
            else
            {
                _outputWriter.WriteLine("NO");
            }
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class KangarooTests
    {
        [TestCase("0 3 4 2", "YES")]
        [TestCase("0 2 5 3", "NO")]
        [TestCase("14 4 98 2", "YES")]
        [TestCase("21 6 47 3", "NO")]
        [TestCase("28 8 96 2", "NO")]
        [TestCase("35 1 45 3", "NO")]
        [TestCase("42 3 94 2", "YES")]
        [TestCase("43 5 49 3", "YES")]
        [TestCase("45 7 56 2", "NO")]
        [TestCase("63 8 94 3", "NO")]
        [TestCase("43 2 70 2", "NO")]
        [TestCase("4523 8092 9419 8076", "YES")]
        [TestCase("6644 5868 8349 3477", "NO")]
        [TestCase("112 9563 8625 244", "NO")]
        [TestCase("1408 6689 6730 4028", "YES")]
        [TestCase("4181 3976 6312 988", "NO")]
        [TestCase("240 575 9179 9986", "NO")]
        [TestCase("55 8223 5803 6509", "NO")]
        [TestCase("2564 5393 5121 2836", "YES")]
        [TestCase("288 9679 9653 99", "NO")]
        [TestCase("2932 7030 9106 4840", "NO")]
        [TestCase("4602 8519 7585 8362", "YES")]
        [TestCase("23 9867 9814 5861", "NO")]
        [TestCase("1817 9931 8417 190", "NO")]
        [TestCase("3585 7317 6994 9610", "NO")]
        [TestCase("1113 612 1331 610", "YES")]
        [TestCase("2081 8403 9107 8400", "YES")]
        [TestCase("1928 4306 5763 4301", "YES")]
        [TestCase("7271 2211 7915 2050", "YES")]
        [TestCase("1571 4240 9023 4234", "YES")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new KangarooTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
