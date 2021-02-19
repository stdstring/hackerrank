// from https://www.hackerrank.com/challenges/points-on-a-line

using ProblemSolving.Common;
using System;
using System.IO;
using NUnit.Framework;

namespace Mathematics.Geometry
{
    public class PointsOnLineTask : ITask
    {
        public PointsOnLineTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public int Execute(String[] args)
        {
            int n = Convert.ToInt32(_inputReader.ReadLine());
            int x0 = 0;
            int y0 = 0;
            LineType lineType = LineType.Unspecified;
            for (int index = 0; index < n; ++index)
            {
                string[] xy = (_inputReader.ReadLine() ?? "").Split(' ');
                int x = Convert.ToInt32(xy[0]);
                int y = Convert.ToInt32(xy[1]);
                if (index == 0)
                {
                    x0 = x;
                    y0 = y;
                }
                else if (x != x0 || y != y0)
                {
                    if ((x != x0 && y != y0) || (lineType == LineType.Horizontal && x != x0) || (lineType == LineType.Vertical && y != y0))
                    {
                        _outputWriter.WriteLine("NO");
                        return 0;
                    }
                    if (lineType == LineType.Unspecified)
                        lineType = x == x0 ? LineType.Horizontal : LineType.Vertical;
                }
            }
            _outputWriter.WriteLine("YES");
            return 0;
        }

        private enum LineType { Unspecified, Horizontal, Vertical }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class PointsOnLineTests
    {
        [TestCase(new []{"5", "0 1", "0 2", "0 3", "0 4", "0 5"}, new []{"YES"})]
        [TestCase(new []{"5", "0 1", "0 2", "1 3", "0 4", "0 5"}, new []{"NO"})]
        [TestCase(new []{"2", "5 -5", "-4 -5"}, new []{"YES"})]
        [TestCase(new []{"2", "-1 5", "0 -4"}, new []{"NO"})]
        [TestCase(new []{"5", "4 4", "4 -8", "4 -4", "4 -10", "4 -3"}, new []{"YES"})]
        [TestCase(new []{"5", "9 -1", "-10 -1", "5 -1", "0 -9", "-4 -1"}, new []{"NO"})]
        [TestCase(new []{"10", "7 10", "-9 10", "8 10", "6 10", "-9 10", "6 10", "-6 10", "-1 10", "-2 10", "-5 10"}, new []{"YES"})]
        [TestCase(new []{"10", "9 -10", "9 5", "4 -4", "9 9", "9 1", "9 2", "9 0", "9 -8", "9 -8", "9 1"}, new []{"NO"})]
        [TestCase(new []{"10", "0 -1", "0 2", "0 1", "0 -2", "0 0", "0 2", "0 1", "0 0", "0 2", "0 0"}, new []{"YES"})]
        [TestCase(new []{"10", "1 -2", "1 -1", "1 2", "1 2", "1 -2", "1 0", "0 1", "1 -2", "1 1", "1 -2"}, new []{"NO"})]
        public void Execute(String[] input, String[] expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new PointsOnLineTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
