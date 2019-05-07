// from https://www.hackerrank.com/challenges/diagonal-difference

using System;
using System.IO;
using System.Linq;
using Algorithm.Common;

namespace DiagonalDifference.AppExample
{
    public class DiagonalDifferenceTask : ITask
    {
        public DiagonalDifferenceTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 size = Int32.Parse(_inputReader.ReadLine());
            Int32 result = 0;
            for (Int32 index = 0; index < size; ++index)
            {
                Int32[] row = _inputReader.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
                result += row[index];
                result -= row[size - 1 - index];
            }
            _outputWriter.WriteLine(Math.Abs(result));
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    class Program
    {
        static void Main(string[] args)
        {
            new DiagonalDifferenceTask(Console.In, Console.Out).Execute(args);
        }
    }
}
