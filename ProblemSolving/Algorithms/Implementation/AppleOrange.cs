// from https://www.hackerrank.com/challenges/apple-and-orange

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class AppleOrangeTask : ITask
    {
        public AppleOrangeTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32[] st = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 s = st[0];
            Int32 t = st[1];
            Int32[] ab = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 a = ab[0];
            Int32 b = ab[1];
            Int32[] mn = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 m = mn[0];
            Int32 n = mn[1];
            Int32[] apples = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32[] oranges = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 applesOnHouse = 0;
            for (Int32 index = 0; index < m; ++index)
            {
                Int32 apple = a + apples[index];
                if (s <= apple && apple <= t)
                    ++applesOnHouse;
            }
            Int32 orangesOnHouse = 0;
            for (Int32 index = 0; index < n; ++index)
            {
                Int32 orange = b + oranges[index];
                if (s <= orange && orange <= t)
                    ++orangesOnHouse;
            }
            _outputWriter.WriteLine(applesOnHouse);
            _outputWriter.WriteLine(orangesOnHouse);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class AppleOrangeTests
    {
        [TestCase("Input00.txt", "1\r\n1")]
        [TestCase("Input01.txt", "0\r\n0")]
        [TestCase("Input02.txt", "1\r\n1")]
        [TestCase("Input03.txt", "18409\r\n19582")]
        [TestCase("Input04.txt", "2530\r\n1149")]
        [TestCase("Input05.txt", "8032\r\n3129")]
        [TestCase("Input06.txt", "6661\r\n4075")]
        [TestCase("Input07.txt", "2082\r\n1960")]
        [TestCase("Input08.txt", "28\r\n2966")]
        [TestCase("Input09.txt", "16141\r\n3358")]
        [TestCase("Input10.txt", "37609\r\n38141")]
        [TestCase("Input11.txt", "5046\r\n5659")]
        public void Execute(String inputFile, String expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new AppleOrangeTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Implementation//AppleOrange";
    }
}
