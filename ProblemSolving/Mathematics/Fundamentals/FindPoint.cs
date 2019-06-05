// from https://www.hackerrank.com/challenges/find-point

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Mathematics.Fundamentals
{
    public class FindPointTask : ITask
    {
        public FindPointTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public int Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            PointPair[] pointPairs = new PointPair[n];
            for (Int32 index = 0; index < n; ++index)
            {
                Int32[] pointPairData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
                pointPairs[index] = new PointPair(pointPairData[0], pointPairData[1], pointPairData[2], pointPairData[3]);
            }
            Point[] dest = new Point[n];
            for (Int32 index = 0; index < n; ++index)
            {
                PointPair pointPair = pointPairs[index];
                Int32 reflectionX = -1 * (pointPair.P.X - pointPair.Q.X) + pointPair.Q.X;
                Int32 reflectionY = -1 * (pointPair.P.Y - pointPair.Q.Y) + pointPair.Q.Y;
                dest[index] = new Point(reflectionX, reflectionY);
            }
            for (Int32 index = 0; index < n; ++index)
            {
                _outputWriter.WriteLine("{0} {1}", dest[index].X, dest[index].Y);
            }
            return 0;
        }

        private class Point
        {
            public Point(Int32 x, Int32 y)
            {
                X = x;
                Y = y;
            }

            public Int32 X { get; }

            public Int32 Y { get; }
        }

        private class PointPair
        {
            public PointPair(Int32 px, Int32 py, Int32 qx, Int32 qy)
            {
                P = new Point(px, py);
                Q = new Point(qx, qy);
            }

            public Point P { get; }

            public Point Q { get; }
        }


        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class FindPointTests
    {
        [TestCase("2\r\n0 0 1 1\r\n1 1 2 2", "2 2\r\n3 3")]
        [TestCase("10\r\n1 1 2 2\r\n4 3 5 2\r\n2 4 5 6\r\n1 2 2 2\r\n1 1 1 1\r\n1 2 2 1\r\n1 8 7 8\r\n9 1 1 1\r\n8 4 3 2\r\n7 8 9 1",
                  "3 3\r\n6 1\r\n8 8\r\n3 2\r\n1 1\r\n3 0\r\n13 8\r\n-7 1\r\n-2 0\r\n11 -6")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new FindPointTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
