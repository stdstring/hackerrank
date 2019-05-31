// from https://www.hackerrank.com/challenges/mini-max-sum

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Warmup
{
    public class MiniMaxSumTask : ITask
    {
        public MiniMaxSumTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int64[] numbers = _inputReader.ReadLine().Split(' ').Select(Int64.Parse).ToArray();
            Int64 totalSum = numbers.Sum();
            Int64 minValue = totalSum;
            Int64 maxValue = 0;
            foreach (Int64 number in numbers)
            {
                minValue = Math.Min(totalSum - number, minValue);
                maxValue = Math.Max(totalSum - number, maxValue);
            }
            _outputWriter.WriteLine("{0} {1}", minValue, maxValue);
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class MiniMaxSumTests
    {
        [TestCase("1 2 3 4 5", "10 14")]
        [TestCase("256741038 623958417 467905213 714532089 938071625", "2063136757 2744467344")]
        [TestCase("396285104 573261094 759641832 819230764 364801279", "2093989309 2548418794")]
        [TestCase("140638725 436257910 953274816 734065819 362748590", "1673711044 2486347135")]
        [TestCase("769082435 210437958 673982045 375809214 380564127", "1640793344 2199437821")]
        [TestCase("156873294 719583602 581240736 605827319 895647130", "2063524951 2802298787")]
        [TestCase("426980153 354802167 142980735 968217435 734892650", "1659655705 2484892405")]
        [TestCase("942381765 627450398 954173620 583762094 236817490", "2390411747 3107767877")]
        [TestCase("539674108 549382170 270968351 746219035 140597628", "1500622257 2106243664")]
        [TestCase("254961783 604179258 462517083 967304281 860273491", "2181931615 2894274113")]
        [TestCase("501893267 649027153 379408215 452968170 487530619", "1821800271 2091419209")]
        [TestCase("140537896 243908675 670291834 923018467 520718469", "1575456874 2357937445")]
        [TestCase("793810624 895642170 685903712 623789054 468592370", "2572095760 2999145560")]
        [TestCase("5 5 5 5 5", "20 20")]
        [TestCase("7 69 2 221 8974", "299 9271")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new MiniMaxSumTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
