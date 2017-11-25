﻿using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigExcludingPropertiesTests
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
        }

        private Person person;

        [Test]
        public void PrintConfig_When_ExcludeAge()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude(p => p.Age);
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeId()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude(p => p.Id);
            var expectedResult = "Person\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\tAge = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeName()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude(p => p.Name);
            var expectedResult = "Person\r\n\tId = Guid\r\n\tHeight = 72,5\r\n\tAge = 19\r\n\tGrowth = 180,1\r\n";
            ;
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}