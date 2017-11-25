using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigGeneralTests
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
        }

        private Person person;

        [Test]
        public void PrintingConfig_When_ExtensionMethodAndPrintntingConfigDefualt()
        {
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
            person.PrintToString().Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintingConfig_When_ExtensionMethodAndPrintntingConfigWithParametrs()
        {
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Al\r\n\t" +
                                 "Age = 19\r\n";
            person.PrintToString(s => s.Exclude<double>().Printing(p => p.Name).TrimmedToLength(2))
                .Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintingConfig_When_ExcludeIntExcludeIdAlternativeModeForNameWithCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<int>()
                .Exclude<Guid>()
                .Printing(p => p.Name).Using(name => $"Aka {name.ToString()}")
                .Printing<double>().Using(CultureInfo.InvariantCulture);
            var expectedResult = "Person\r\n\tAka Alex\r\n\tHeight = 72.5\r\n\t" +
                                 "Growth = 180.1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}