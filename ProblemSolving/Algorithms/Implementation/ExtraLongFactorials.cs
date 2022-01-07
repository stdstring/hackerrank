// from https://www.hackerrank.com/challenges/extra-long-factorials

using System;
using System.IO;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class ExtraLongFactorialsTask : ITask
    {
        public ExtraLongFactorialsTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            _outputWriter.WriteLine(CalcBigFactorial(n));
            return 0;
        }

        private BigInteger CalcBigFactorial(Int32 n)
        {
            System.Numerics.BigInteger init = 1;
            return Enumerable.Range(1, n).Aggregate(init, (dest, number) => dest * number);
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class ExtraLongFactorialsTests
    {
        [TestCase("25", "15511210043330985984000000")]
        [TestCase("10", "3628800")]
        [TestCase("4", "24")]
        [TestCase("20", "2432902008176640000")]
        [TestCase("37", "13763753091226345046315979581580902400000000")]
        [TestCase("44", "2658271574788448768043625811014615890319638528000000000")]
        [TestCase("58", "2350561331282878571829474910515074683828862318181142924420699914240000000000000")]
        [TestCase("77", "145183092028285869634070784086308284983740379224208358846781574688061991349156420080065207861248000000000000000000")]
        [TestCase("1", "1")]
        [TestCase("95", "10329978488239059262599702099394727095397746340117372869212250571234293987594703124871765375385424468563282236864226607350415360000000000000000000000")]
        [TestCase("100", "93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000")]
        [TestCase("88", "185482642257398439114796845645546284380220968949399346684421580986889562184028199319100141244804501828416633516851200000000000000000000")]
        [TestCase("45", "119622220865480194561963161495657715064383733760000000000")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new ExtraLongFactorialsTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
