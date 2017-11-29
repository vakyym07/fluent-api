﻿using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigExcludingTypesTests
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person { Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1 };
            personDefaultId = "00000000-0000-0000-0000-000000000000";
        }

        private Person person;
        private string personDefaultId;

        [Test]
        public void PrintConfig_When_ExcludeDouble()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<double>();
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Alex\r\n\tAge = 19\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeInt()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<int>();
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Alex\r\n\tHeight = 72,5" +
                                 "\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeString()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<string>();
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tHeight = 72,5" +
                                 "\r\n\tAge = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}