// from https://www.hackerrank.com/challenges/day-of-the-programmer

using System;
using System.IO;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class DayOfProgrammerTask : ITask
    {
        public DayOfProgrammerTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            Int32 year = Int32.Parse(_inputReader.ReadLine());
            if (year < CalendarTransitionYear)
            {
                Date date = FindDate(false, IsLeapYearForJulianCalendar(year) ? _leapYear : _usualYear, DayNumber);
                _outputWriter.WriteLine($"{date.Day:D2}.{date.Month:D2}.{year}");
            }
            else if (year == CalendarTransitionYear)
            {
                Date date = FindDate(true, _transitionYear, DayNumber);
                _outputWriter.WriteLine($"{date.Day:D2}.{date.Month:D2}.{year}");
            }
            else
            {
                Date date = FindDate(false, IsLeapYearForGregorianCalendar(year) ? _leapYear : _usualYear, DayNumber);
                _outputWriter.WriteLine($"{date.Day:D2}.{date.Month:D2}.{year}");
            }
            return 0;
        }

        private Boolean IsLeapYearForJulianCalendar(Int32 year)
        {
            return year % 4 == 0;
        }

        private Boolean IsLeapYearForGregorianCalendar(Int32 year)
        {
            return (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);
        }

        private Date FindDate(Boolean isTransitionYear, Int32[] yearMonths, Int32 dayNumber)
        {
            Int32 dayCount = 0;
            for (Int32 month = 1; month <= yearMonths.Length; ++month)
            {
                if (dayCount < dayNumber && dayCount + yearMonths[month - 1] >= dayNumber)
                    return new Date(isTransitionYear && month == TransitionMonth ? dayNumber - dayCount + TransitionShift : dayNumber - dayCount, month);
                dayCount += yearMonths[month - 1];
            }
            throw new InvalidOperationException();
        }

        private struct Date
        {
            public Date(Int32 day, Int32 month)
            {
                Day = day;
                Month = month;
            }

            public Int32 Day { get; }

            public Int32 Month { get; }
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;

        private readonly Int32[] _usualYear = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        private readonly Int32[] _leapYear = {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        private readonly Int32[] _transitionYear = {31, 15, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

        private const Int32 TransitionMonth = 2;
        private const Int32 TransitionShift = 13;
        private const Int32 CalendarTransitionYear = 1918;

        private const Int32 DayNumber = 256;
    }

    [TestFixture]
    public class DayOfProgrammerTests
    {
        [TestCase("2017", "13.09.2017")]
        [TestCase("2016", "12.09.2016")]
        [TestCase("2015", "13.09.2015")]
        [TestCase("2014", "13.09.2014")]
        [TestCase("2013", "13.09.2013")]
        [TestCase("2012", "12.09.2012")]
        [TestCase("2018", "13.09.2018")]
        [TestCase("2019", "13.09.2019")]
        [TestCase("2020", "12.09.2020")]
        [TestCase("2021", "13.09.2021")]
        [TestCase("1917", "13.09.1917")]
        [TestCase("1916", "12.09.1916")]
        [TestCase("1915", "13.09.1915")]
        [TestCase("1914", "13.09.1914")]
        [TestCase("1919", "13.09.1919")]
        [TestCase("1920", "12.09.1920")]
        [TestCase("1921", "13.09.1921")]
        [TestCase("1922", "13.09.1922")]
        [TestCase("1836", "12.09.1836")]
        [TestCase("1872", "12.09.1872")]
        [TestCase("1832", "12.09.1832")]
        [TestCase("1880", "12.09.1880")]
        [TestCase("1812", "12.09.1812")]
        [TestCase("1820", "12.09.1820")]
        [TestCase("1840", "12.09.1840")]
        [TestCase("1772", "12.09.1772")]
        [TestCase("1908", "12.09.1908")]
        [TestCase("1783", "13.09.1783")]
        [TestCase("1913", "13.09.1913")]
        [TestCase("1711", "13.09.1711")]
        [TestCase("1737", "13.09.1737")]
        [TestCase("1758", "13.09.1758")]
        [TestCase("1741", "13.09.1741")]
        [TestCase("2128", "12.09.2128")]
        [TestCase("2508", "12.09.2508")]
        [TestCase("2476", "12.09.2476")]
        [TestCase("2156", "12.09.2156")]
        [TestCase("2220", "12.09.2220")]
        [TestCase("2652", "12.09.2652")]
        [TestCase("2532", "12.09.2532")]
        [TestCase("2440", "12.09.2440")]
        [TestCase("2108", "12.09.2108")]
        [TestCase("1966", "13.09.1966")]
        [TestCase("2690", "13.09.2690")]
        [TestCase("2035", "13.09.2035")]
        [TestCase("2491", "13.09.2491")]
        [TestCase("1953", "13.09.1953")]
        [TestCase("2249", "13.09.2249")]
        [TestCase("1700", "12.09.1700")]
        [TestCase("1800", "12.09.1800")]
        [TestCase("1900", "12.09.1900")]
        [TestCase("2000", "12.09.2000")]
        [TestCase("2100", "13.09.2100")]
        [TestCase("2200", "13.09.2200")]
        [TestCase("2300", "13.09.2300")]
        [TestCase("2400", "12.09.2400")]
        [TestCase("2500", "13.09.2500")]
        [TestCase("2600", "13.09.2600")]
        [TestCase("2700", "13.09.2700")]
        [TestCase("1918", "26.09.1918")]
        [TestCase("1800", "12.09.1800")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new DayOfProgrammerTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
