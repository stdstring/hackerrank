// from https://www.hackerrank.com/challenges/the-time-in-words

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class TimeInWordsTask : ITask
    {
        public TimeInWordsTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 hour = Int32.Parse(_inputReader.ReadLine());
            Int32 minutes = Int32.Parse(_inputReader.ReadLine());
            _outputWriter.WriteLine(CalcTimeInWords(hour, minutes));
            return 0;
        }

        public String CalcTimeInWords(Int32 hour, Int32 minutes)
        {
            if (minutes == 0)
                return $"{_numbers[hour]} o' clock";
            if (minutes == 15)
                return $"quarter past {_numbers[hour]}";
            if (minutes == 30)
                return $"half past {_numbers[hour]}";
            if (minutes == 45)
                return $"quarter to {_numbers[(hour + 1) % 24]}";
            if (minutes < 30)
                return $"{_numbers[minutes]} minute{(minutes == 1 ? "" : "s")} past {_numbers[hour]}";
            return $"{_numbers[60 - minutes]} minute{(minutes == 59 ? "" : "s")} to {_numbers[(hour + 1) % 24]}";
        }

        private readonly IList<string> _numbers = new[]{"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
                                                        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
                                                        "twenty", "twenty one", "twenty two", "twenty three", "twenty four", "twenty five", "twenty six", "twenty seven", "twenty eight", "twenty nine"};

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class TimeInWordsTests
    {
        [TestCase("5\r\n47", "thirteen minutes to six")]
        [TestCase("3\r\n00", "three o' clock")]
        [TestCase("7\r\n29", "twenty nine minutes past seven")]
        [TestCase("5\r\n30", "half past five")]
        [TestCase("5\r\n45", "quarter to six")]
        [TestCase("4\r\n15", "quarter past four")]
        [TestCase("6\r\n35", "twenty five minutes to seven")]
        [TestCase("3\r\n30", "half past three")]
        [TestCase("10\r\n57", "three minutes to eleven")]
        [TestCase("1\r\n1", "one minute past one")]
        [TestCase("7\r\n15", "quarter past seven")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new TimeInWordsTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
