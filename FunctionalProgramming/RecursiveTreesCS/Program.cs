using System;
using System.Collections.Generic;
using System.Linq;

namespace RecursiveTreesCS
{
    class Program
    {
        static void Main(String[] args)
        {
            Show(BuildTree(Int32.Parse(Console.ReadLine())));
        }

        private static LinkedList<IList<Int32>> BuildTree(Int32 count)
        {
            return BuildTree(1, count, SegmentStartSize, new LinkedList<IList<Int32>>());
        }

        private static LinkedList<IList<Int32>> BuildTree(Int32 level, Int32 count, Int32 size, LinkedList<IList<Int32>> storage)
        {
            if (level > count)
                return storage;
            return BuildTree(level + 1, count, size/2, BuildSlantingSegments(size, BuildVerticalSegment(size, storage)));
        }

        private static LinkedList<IList<Int32>> BuildVerticalSegment(Int32 size, LinkedList<IList<Int32>> storage)
        {
            return Enumerable.Range(1, size).Aggregate(storage, (acc, _) => storage.AddFirst(storage.Count > 0 ? storage.First.Value : new[] {ColumnStart}).List);
        }

        private static LinkedList<IList<Int32>> BuildSlantingSegments(Int32 size, LinkedList<IList<Int32>> storage)
        {
            return BuildSlantingSegments(size, storage, storage.First.Value);
        }

        private static LinkedList<IList<Int32>> BuildSlantingSegments(Int32 size, LinkedList<IList<Int32>> storage, IList<Int32> sourceValue)
        {
            return Enumerable.Range(1, size) .Aggregate(storage, (acc, delta) => storage.AddFirst(GenerateSlantingSegmentsRow(sourceValue, delta)).List);
        }

        private static IList<Int32> GenerateSlantingSegmentsRow(IList<Int32> sourceValue, Int32 delta)
        {
            return sourceValue.Aggregate(new List<Int32>(), (acc, value) =>
            {
                acc.AddRange(new[] {value - delta, value + delta});
                return acc;
            });
        }

        private static LinkedList<IList<Int32>> ShowEmptyLines(LinkedList<IList<Int32>> storage)
        {
            // TODO (std_string) : need something such List.iter from F#
            Enumerable.Range(1, RowCount - storage.Count).Aggregate(new Object(), (acc, _) =>
            {
                Console.WriteLine(new String(EmptyChar, ColumnCount));
                return acc;
            });
            return storage;
        }

        private static void ShowLine(IList<Int32> colunms)
        {
            Enumerable.Range(1, ColumnCount).Aggregate(0, (acc, index) =>
            {
                if (acc == colunms.Count)
                {
                    Console.Write(EmptyChar);
                    return acc;
                }
                Console.Write(colunms[acc] == index ? LineChar : EmptyChar);
                return colunms[acc] == index ? acc + 1 : acc;
            });
            Console.WriteLine();
        }

        private static void ShowLines(LinkedList<IList<Int32>> storage)
        {
            // TODO (std_string) : need something such List.iter from F#
            storage.Aggregate(new Object(), (acc, columns) =>
            {
                ShowLine(columns);
                return acc;
            });
        }

        private static void Show(LinkedList<IList<Int32>> storage)
        {
            ShowLines(ShowEmptyLines(storage));
        }

        private const Int32 RowCount = 63;

        private const Int32 ColumnCount = 100;

        private const Char EmptyChar = '_';

        private const Char LineChar = '1';

        private const Int32 ColumnStart = 50;

        private const Int32 SegmentStartSize = 16;
    }
}
