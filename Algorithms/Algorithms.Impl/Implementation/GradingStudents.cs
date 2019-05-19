// from https://www.hackerrank.com/challenges/grading

using System;
using System.IO;
using Algorithm.Common;
using NUnit.Framework;

namespace Algorithms.Impl.Implementation
{
    public class GradingStudentsTask : ITask
    {
        public GradingStudentsTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 studentsCount = Int32.Parse(_inputReader.ReadLine());
            for (Int32 index = 0; index < studentsCount; ++index)
            {
                Int32 grade = Int32.Parse(_inputReader.ReadLine());
                Int32 gradeUpdate = (grade >= 38) && (grade % 5 >= 3) ? 5 - grade % 5 : 0;
                _outputWriter.WriteLine(grade + gradeUpdate);
            }
            return 0;
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class GradingStudentsTests
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
        public void Execute(String inputFile, String expectedOutputFile)
        {
            String input = File.ReadAllText(Path.Combine(RootDirectory, inputFile));
            String expectedOutput = File.ReadAllText(Path.Combine(RootDirectory, expectedOutputFile));
            TaskExecuter.Execute((inputReader, outputWriter) => new GradingStudentsTask(inputReader, outputWriter), input, expectedOutput);
        }

        private const String RootDirectory = ".//TestCases//Implementation//GradingStudents";
    }
}
