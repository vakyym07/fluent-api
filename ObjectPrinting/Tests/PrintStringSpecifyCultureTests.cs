using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintStringSpecifyCultureTests_Should
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
        public void PrintConfig_When_CurrentCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(CultureInfo.CurrentCulture);
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void PrintConfig_When_InvariantCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(CultureInfo.InvariantCulture);
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Alex\r\n\tHeight = 72.5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180.1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}