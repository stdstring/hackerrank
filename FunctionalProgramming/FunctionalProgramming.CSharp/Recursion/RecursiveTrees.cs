// from https://www.hackerrank.com/challenges/fractal-trees

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunctionalProgramming.Common;
using NUnit.Framework;

namespace FunctionalProgramming.CSharp.Recursion
{
    public class RecursiveTreesTask : ITask
    {
        public RecursiveTreesTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public int Execute(String[] args)
        {
            Show(BuildTree(Int32.Parse(_inputReader.ReadLine())));
            return 0;
        }

        private LinkedList<IList<Int32>> BuildTree(Int32 count)
        {
            return BuildTree(1, count, SegmentStartSize, new LinkedList<IList<Int32>>());
        }

        private LinkedList<IList<Int32>> BuildTree(Int32 level, Int32 count, Int32 size, LinkedList<IList<Int32>> storage)
        {
            if (level > count)
                return storage;
            return BuildTree(level + 1, count, size / 2, BuildSlantingSegments(size, BuildVerticalSegment(size, storage)));
        }

        private LinkedList<IList<Int32>> BuildVerticalSegment(Int32 size, LinkedList<IList<Int32>> storage)
        {
            return Enumerable.Range(1, size).Aggregate(storage, (acc, _) => storage.AddFirst(storage.Count > 0 ? storage.First.Value : new[] {ColumnStart}).List);
        }

        private LinkedList<IList<Int32>> BuildSlantingSegments(Int32 size, LinkedList<IList<Int32>> storage)
        {
            return BuildSlantingSegments(size, storage, storage.First.Value);
        }

        private LinkedList<IList<Int32>> BuildSlantingSegments(Int32 size, LinkedList<IList<Int32>> storage, IList<Int32> sourceValue)
        {
            return Enumerable.Range(1, size).Aggregate(storage, (acc, delta) => storage.AddFirst(GenerateSlantingSegmentsRow(sourceValue, delta)).List);
        }

        private IList<Int32> GenerateSlantingSegmentsRow(IList<Int32> sourceValue, Int32 delta)
        {
            return sourceValue.Aggregate(new List<Int32>(), (acc, value) =>
            {
                acc.AddRange(new[] {value - delta, value + delta});
                return acc;
            });
        }

        private LinkedList<IList<Int32>> ShowEmptyLines(LinkedList<IList<Int32>> storage)
        {
            // TODO (std_string) : need something such List.iter from F#
            Enumerable.Range(1, RowCount - storage.Count).Aggregate(new Object(), (acc, _) =>
            {
                _outputWriter.WriteLine(new String(EmptyChar, ColumnCount));
                return acc;
            });
            return storage;
        }

        private void ShowLine(IList<Int32> colunms)
        {
            Enumerable.Range(1, ColumnCount).Aggregate(0, (acc, index) =>
            {
                if (acc == colunms.Count)
                {
                    _outputWriter.Write(EmptyChar);
                    return acc;
                }
                _outputWriter.Write(colunms[acc] == index ? LineChar : EmptyChar);
                return colunms[acc] == index ? acc + 1 : acc;
            });
            _outputWriter.WriteLine();
        }

        private void ShowLines(LinkedList<IList<Int32>> storage)
        {
            // TODO (std_string) : need something such List.iter from F#
            storage.Aggregate(new Object(), (acc, columns) =>
            {
                ShowLine(columns);
                return acc;
            });
        }

        private void Show(LinkedList<IList<Int32>> storage)
        {
            ShowLines(ShowEmptyLines(storage));
        }

        private readonly TextReader _inputReader;

        private readonly TextWriter _outputWriter;

        private const Int32 RowCount = 63;

        private const Int32 ColumnCount = 100;

        private const Char EmptyChar = '_';

        private const Char LineChar = '1';

        private const Int32 ColumnStart = 50;

        private const Int32 SegmentStartSize = 16;
    }

    [TestFixture]
    public class RecursiveTreesTests
    {
        [TestCase("1", "Output00.txt")]
        [TestCase("2", "Output01.txt")]
        [TestCase("3", "Output02.txt")]
        [TestCase("4", "Output03.txt")]
        [TestCase("5", "Output04.txt")]
        public void Execute(String input, String expectedOutputFile)
        {
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecuter.Execute((inputReader, outputWriter) => new RecursiveTreesTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Recursion//RecursiveTrees";
    }
}
