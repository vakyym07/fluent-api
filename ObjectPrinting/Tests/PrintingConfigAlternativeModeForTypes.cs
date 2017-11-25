using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigAlternativeModeForTypes
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
        }

        private Person person;

        [Test]
        public void PrintingConfig_When_AlternativeModeForInt()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<int>().Using(i => $"{i.ToString()}, type: int");
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\tAge = 19, type: int" +
                                 "\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintingConfig_When_AlternativeModeForString()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<string>().Using(i => $"{i} Ivanov");
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex Ivanov\r\n\tHeight = 72,5\r\n\tAge = 19" +
                                 "\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}