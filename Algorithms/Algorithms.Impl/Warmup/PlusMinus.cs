// from https://www.hackerrank.com/challenges/plus-minus

using System;
using System.IO;
using System.Linq;
using Algorithm.Common;
using NUnit.Framework;

namespace Algorithms.Impl.Warmup
{
    public class PlusMinusTask : ITask
    {
        public PlusMinusTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            Int32[] numbers = _inputReader.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
            Int32 positiveCount = 0;
            Int32 zeroCount = 0;
            Int32 negativeCount = 0;
            foreach (Int32 number in numbers)
            {
                if (number > 0)
                    positiveCount++;
                if (number == 0)
                    zeroCount++;
                if (number < 0)
                    negativeCount++;
            }
            _outputWriter.WriteLine(1.0 * positiveCount / n);
            _outputWriter.WriteLine(1.0 * negativeCount / n);
            _outputWriter.WriteLine(1.0 * zeroCount / n);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class PlusMinusTests
    {
        [TestCase("Input00.txt", new []{0.400000, 0.200000, 0.400000})]
        [TestCase("Input01.txt", new []{0.500000, 0.333333, 0.166667})]
        [TestCase("Input02.txt", new []{0.428571, 0.571429, 0.000000})]
        [TestCase("Input03.txt", new []{0.400000, 0.400000, 0.200000})]
        [TestCase("Input04.txt", new []{1.000000, 0.000000, 0.000000})]
        [TestCase("Input05.txt", new []{0.700000, 0.000000, 0.300000})]
        [TestCase("Input06.txt", new []{0.000000, 0.833333, 0.166667})]
        [TestCase("Input07.txt", new []{0.408451, 0.492958, 0.098592})]
        [TestCase("Input08.txt", new []{0.440000, 0.450000, 0.110000})]
        [TestCase("Input09.txt", new []{0.300000, 0.360000, 0.340000})]
        [TestCase("Input10.txt", new []{0.340000, 0.380000, 0.280000})]
        [TestCase("Input11.txt", new []{0.375000, 0.375000, 0.250000})]
        public void Execute(String inputFile, Double[] expectedOutput)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            TaskExecuter.Execute((inputReader, outputWriter) => new PlusMinusTask(inputReader, outputWriter), input, expectedOutput, CheckValue);

            void CheckValue(Double expectedValue, String actualValue) => Assert.AreEqual(expectedValue, Double.Parse(actualValue), MaxAbsError);
        }

        private const String RootDirectory = ".//TestCases//Warmup//PlusMinus";
        private const Double MaxAbsError = 0.0001;
    }
}
