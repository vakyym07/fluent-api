using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigExcludingTypesTests
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
        }

        private Person person;

        [Test]
        public void PrintConfig_When_ExcludeDouble()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<double>();
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tAge = 19\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeInt()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<int>();
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_ExcludeString()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<string>();
            var expectedResult = "Person\r\n\tId = Guid\r\n\tHeight = 72,5\r\n\tAge = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}