using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProblemSolving.Common;

namespace DataStructures.Stacks
{
    public class MaximumElementTask : ITask
    {
        public MaximumElementTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            const Int32 pushQuery = 1;
            const Int32 deleteQuery = 2;
            const Int32 printMaxQuery = 3;
            Stack<Int32> maxStack = new Stack<Int32>();
            Int32 n = Int32.Parse(_inputReader.ReadLine());
            for (Int32 queryIndex = 0; queryIndex < n; ++queryIndex)
            {
                Int32[] query = _inputReader.ReadLine().Split().Select(Int32.Parse).ToArray();
                switch (query[0])
                {
                    case pushQuery:
                        maxStack.Push(maxStack.Count == 0 || query[1] > maxStack.Peek() ? query[1] : maxStack.Peek());
                        break;
                    case deleteQuery:
                        maxStack.Pop();
                        break;
                    case printMaxQuery:
                        _outputWriter.WriteLine(maxStack.Peek());
                        break;
                }
            }
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class MaximumElementTests
    {
        [TestCase("Input00.txt", "Output00.txt")]
        [TestCase("Input01.txt", "Output01.txt")]
        [TestCase("Input02.txt", null)]
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
        [TestCase("Input13.txt", "Output13.txt")]
        [TestCase("Input14.txt", "Output14.txt")]
        [TestCase("Input15.txt", "Output15.txt")]
        [TestCase("Input16.txt", "Output16.txt")]
        [TestCase("Input17.txt", "Output17.txt")]
        [TestCase("Input18.txt", "Output18.txt")]
        [TestCase("Input19.txt", "Output19.txt")]
        [TestCase("Input20.txt", "Output20.txt")]
        [TestCase("Input21.txt", "Output21.txt")]
        [TestCase("Input22.txt", "Output22.txt")]
        [TestCase("Input23.txt", "Output23.txt")]
        [TestCase("Input24.txt", "Output24.txt")]
        [TestCase("Input25.txt", "Output25.txt")]
        [TestCase("Input26.txt", "Output26.txt")]
        [TestCase("Input27.txt", "Output27.txt")]
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String[] expectedOutput = expectedOutputFile != null ? File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile)).Split("\r\n") : new String[0];
            TaskExecutor.Execute((inputReader, outputWriter) => new MaximumElementTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Stacks//MaximumElement";
    }
}
