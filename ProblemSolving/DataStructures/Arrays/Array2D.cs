// from https://www.hackerrank.com/challenges/2d-array

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Arrays
{
    public class Array2DTask : ITask
    {
        public Array2DTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            const Int32 rowCount = 6;
            const Int32 columnCount = 6;
            Int32[,] matrix = ReadMatrix(rowCount, columnCount);
            // set of the all possible centers is the following: 1 <= row <= rowCount - 2, 1 <= column <= columnCount - 2
            Int32 maxHourglassValue = Int32.MinValue;
            for (Int32 row = 1; row <= rowCount - 2; ++row)
            {
                for (Int32 column = 1; column <= columnCount - 2; ++column)
                {
                    Int32 hourglassValue = matrix[row, column] +
                                           matrix[row - 1, column - 1] +
                                           matrix[row - 1, column] +
                                           matrix[row - 1, column + 1] +
                                           matrix[row + 1, column - 1] +
                                           matrix[row + 1, column] +
                                           matrix[row + 1, column + 1];
                    if (hourglassValue > maxHourglassValue)
                        maxHourglassValue = hourglassValue;
                }
            }
            _outputWriter.WriteLine(maxHourglassValue);
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

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class Array2DTests
    {
        [TestCase("1 1 1 0 0 0\r\n0 1 0 0 0 0\r\n1 1 1 0 0 0\r\n0 0 2 4 4 0\r\n0 0 0 2 0 0\r\n0 0 1 2 4 0", "19")]
        [TestCase("1 1 1 0 0 0\r\n0 1 0 0 0 0\r\n1 1 1 0 0 0\r\n0 9 2 -4 -4 0\r\n0 0 0 -2 0 0\r\n0 0 -1 -2 -4 0", "13")]
        [TestCase("-1 1 -1 0 0 0\r\n0 -1 0 0 0 0\r\n-1 -1 -1 0 0 0\r\n0 -9 2 -4 -4 0\r\n-7 0 0 -2 0 0\r\n0 0 -1 -2 -4 0", "0")]
        [TestCase("-1 -1 0 -9 -2 -2\r\n-2 -1 -6 -8 -2 -5\r\n-1 -1 -1 -2 -3 -4\r\n-1 -9 -2 -4 -4 -5\r\n-7 -3 -3 -2 -9 -9\r\n-1 -3 -1 -2 -4 -5", "-6")]
        [TestCase("7 6 8 2 4 3\r\n7 3 3 0 6 1\r\n3 8 7 7 2 2\r\n0 8 6 8 6 1\r\n7 1 6 0 2 4\r\n2 7 8 1 7 4", "44")]
        [TestCase("3 7 -3 0 1 8\r\n1 -4 -7 -8 -6 5\r\n-8 1 3 3 5 7\r\n-2 4 3 1 2 7\r\n2 4 -5 1 8 4\r\n5 -7 6 5 2 8", "33")]
        [TestCase("0 6 -7 1 6 3\r\n-8 2 8 3 -2 7\r\n-3 3 -6 -3 0 -6\r\n5 0 5 -1 -5 2\r\n6 2 8 1 3 0\r\n8 5 0 4 -7 4", "25")]
        [TestCase("0 -4 -6 0 -7 -6\r\n-1 -2 -6 -8 -3 -1\r\n-8 -4 -2 -8 -8 -6\r\n-3 -1 -2 -5 -7 -4\r\n-3 -5 -3 -6 -6 -6\r\n-3 -6 0 -8 -6 -7", "-19")]
        [TestCase("-9 -9 -9 1 1 1\r\n0 -9 0 4 3 2\r\n-9 -9 -9 1 2 3\r\n0 0 8 6 6 0\r\n0 0 0 -2 0 0\r\n0 0 1 2 4 0", "28")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new Array2DTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
