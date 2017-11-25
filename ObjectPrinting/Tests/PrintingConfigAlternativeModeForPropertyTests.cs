using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfigAlternativeModeForPropertyTests
    {
        [SetUp]
        public void SetUp()
        {
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
        }

        private Person person;

        [Test]
        public void PrintingConfig_When_AlternativeModeForAge()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(p => p.Age).Using(age => $"Age: {age.ToString()} y.o.");
            var expectedResult = "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age: 19 y.o.\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintingConfig_When_AlternativeModeForName()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(p => p.Name).Using(name => $"Aka {name.ToString()}");
            var expectedResult = "Person\r\n\tId = Guid\r\n\tAka Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}