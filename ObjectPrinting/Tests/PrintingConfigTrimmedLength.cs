﻿using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigTrimmedLength
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
        public void PrintingConfig_TrimmedLength()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(p => p.Name).TrimmedToLength(2);
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Al\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}