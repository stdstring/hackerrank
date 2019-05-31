// from https://www.hackerrank.com/challenges/time-conversion

using System;
using System.IO;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Warmup
{
    public class TimeConversionTask : ITask
    {
        public TimeConversionTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            String input = _inputReader.ReadLine();
            Int32 hours = Int32.Parse(input.Substring(0, 2));
            Int32 minutes = Int32.Parse(input.Substring(3, 2));
            Int32 seconds = Int32.Parse(input.Substring(6, 2));
            String intervalType = input.Substring(8);
            if (String.Equals(intervalType, "PM"))
            {
                _outputWriter.WriteLine("{0:00}:{1:00}:{2:00}", hours == 12 ? hours : hours + 12, minutes, seconds);
            }
            if (String.Equals(intervalType, "AM"))
            {
                _outputWriter.WriteLine("{0:00}:{1:00}:{2:00}", hours == 12 ? 0 : hours, minutes, seconds);
            }
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class TimeConversionTests
    {
        [TestCase("07:05:45PM", "19:05:45")]
        [TestCase("12:40:22AM", "00:40:22")]
        [TestCase("06:40:03AM", "06:40:03")]
        [TestCase("12:05:39AM", "00:05:39")]
        [TestCase("12:45:54PM", "12:45:54")]
        [TestCase("02:34:50PM", "14:34:50")]
        [TestCase("04:59:59AM", "04:59:59")]
        [TestCase("04:59:59PM", "16:59:59")]
        [TestCase("12:00:00AM", "00:00:00")]
        [TestCase("11:59:59PM", "23:59:59")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new TimeConversionTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
