// from https://www.hackerrank.com/challenges/matrix-rotation-algo

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class MatrixLayerRotationTask : ITask
    {
        public MatrixLayerRotationTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(string[] args)
        {
            Int32[] taskData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
            Int32 rowCount = taskData[0];
            Int32 columnCount = taskData[1];
            Int32 rotationCount = taskData[2];
            ShowMatrix(rowCount, columnCount, ProcessMatrix(rowCount, columnCount, rotationCount, ReadMatrix(rowCount, columnCount)));
            return 0;
        }

        private Int32[,] ReadMatrix(Int32 rowCount, Int32 columnCount)
        {
            Int32[,] matrix = new Int32[rowCount, columnCount];
            for (Int32 row = 0; row < rowCount; ++row)
            {
                Int32[] rowData = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
                for (Int32 column = 0; column < columnCount; ++column)
                {
                    matrix[row, column] = rowData[column];
                }
            }
            return matrix;
        }

        private void ShowMatrix(Int32 rowCount, Int32 columnCount, Int32[,] matrix)
        {
            for (Int32 row = 0; row < rowCount; ++row)
            {
                for (Int32 column = 0; column < columnCount; ++column)
                {
                    _outputWriter.Write("{0} ", matrix[row, column]);
                }
                _outputWriter.WriteLine();
            }
        }

        private Int32[,] ProcessMatrix(Int32 rowCount, Int32 columnCount, Int32 rotationCount, Int32[,] matrix)
        {
            Point topLeftPoint = new Point(0, 0);
            Point bottomRightPoint = new Point(rowCount - 1, columnCount - 1);
            while (topLeftPoint.Row <= bottomRightPoint.Row && topLeftPoint.Column <= bottomRightPoint.Column)
            {
                SetCircle(matrix, topLeftPoint, bottomRightPoint, Rotate(rotationCount, GetCircle(matrix, topLeftPoint, bottomRightPoint)));
                topLeftPoint = new Point(topLeftPoint.Row + 1, topLeftPoint.Column + 1);
                bottomRightPoint = new Point(bottomRightPoint.Row - 1, bottomRightPoint.Column - 1);
            }
            return matrix;
        }

        private IList<Int32> GetCircle(Int32[,] matrix, Point topLeftPoint, Point bottomRightPoint)
        {
            return GetCircleImpl().ToList();

            IEnumerable<Int32> GetCircleImpl()
            {
                for (Int32 column = topLeftPoint.Column; column < bottomRightPoint.Column; ++column)
                {
                    yield return matrix[topLeftPoint.Row, column];
                }
                for (Int32 row = topLeftPoint.Row; row < bottomRightPoint.Row; ++row)
                {
                    yield return matrix[row, bottomRightPoint.Column];
                }
                for (Int32 column = bottomRightPoint.Column; column > topLeftPoint.Column; --column)
                {
                    yield return matrix[bottomRightPoint.Row, column];
                }
                for (Int32 row = bottomRightPoint.Row; row > topLeftPoint.Row; --row)
                {
                    yield return matrix[row, topLeftPoint.Column];
                }
            }
        }

        private IList<Int32> Rotate(Int32 rotationCount, IList<Int32> circle)
        {
            Int32 shift = rotationCount % circle.Count;
            IList<Int32> dest = new List<Int32>(circle.Count);
            for (Int32 index = shift; index < circle.Count; ++index)
            {
                dest.Add(circle[index]);
            }
            for (Int32 index = 0; index < shift; ++index)
            {
                dest.Add(circle[index]);
            }
            return dest;
        }

        private void SetCircle(Int32[,] matrix, Point topLeftPoint, Point bottomRightPoint, IList<Int32> circle)
        {
            Int32 circleIndex = 0;
            foreach (Point point in GetCirclePath())
            {
                matrix[point.Row, point.Column] = circle[circleIndex];
                ++circleIndex;
            }

            IEnumerable<Point> GetCirclePath()
            {
                for (Int32 column = topLeftPoint.Column; column < bottomRightPoint.Column; ++column)
                {
                    yield return new Point(topLeftPoint.Row, column);
                }
                for (Int32 row = topLeftPoint.Row; row < bottomRightPoint.Row; ++row)
                {
                    yield return new Point(row, bottomRightPoint.Column);
                }
                for (Int32 column = bottomRightPoint.Column; column > topLeftPoint.Column; --column)
                {
                    yield return new Point(bottomRightPoint.Row, column);
                }
                for (Int32 row = bottomRightPoint.Row; row > topLeftPoint.Row; --row)
                {
                    yield return new Point(row, topLeftPoint.Column);
                }
            }
        }

        private class Point
        {
            public Point(Int32 row, Int32 column)
            {
                Row = row;
                Column = column;
            }

            public Int32 Row { get; }
            public Int32 Column { get; }
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class MatrixLayerRotationTests
    {
        [TestCase("Input00.txt", "Output00.txt")]
        [TestCase("Input01.txt", "Output01.txt")]
        [TestCase("Input02.txt", "Output02.txt")]
        [TestCase("Input03.txt", "Output03.txt")]
        [TestCase("Input04.txt", "Output04.txt")]
        [TestCase("Input05.txt", "Output05.txt")]
        [TestCase("Input06.txt", "Output06.txt")]
        [TestCase("Input07.txt", "Output07.txt")]
        [TestCase("Input08.txt", "Output08.txt")]
        [TestCase("Input09.txt", "Output09.txt")]
        [TestCase("Input10.txt", "Output10.txt")]
        [TestCase("Input11.txt", "Output11.txt")]
        [TestCase("Input12.txt", "Output12.txt")]
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecutor.Execute((inputReader, outputWriter) => new MatrixLayerRotationTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Implementation//MatrixLayerRotation";
    }
}
