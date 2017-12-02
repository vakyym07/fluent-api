using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            person = new Person {Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1};
            personDefaultId = Guid.Empty.ToString();
            expectedSerializedObj = new Dictionary<int, string>
            {
                {1, "Person"},
                {2, $"Id = {personDefaultId}"},
                {3, "Name = Alex"},
                {4, "Height = 72,5"},
                {5, "Age = 19"},
                {6, "Growth = 180,1"}
            };
        }

        private Person person;
        private string personDefaultId;
        private Dictionary<int, string> expectedSerializedObj;

        private string GetExpectedSerializedObj()
        {
            var obgParams = expectedSerializedObj.OrderBy(p => p.Key).Select(p => p.Value);
            return string.Join("\r\n\t", obgParams) + "\r\n";
        }

        [Test]
        public void PrintConfig_When_CurrentCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(CultureInfo.CurrentCulture);
            expectedSerializedObj[4] = "Height = 72,5";
            expectedSerializedObj[6] = "Growth = 180,1";
            var expectedResult = $"Person\r\n\tId = {personDefaultId}\r\n\tName = Alex\r\n\tHeight = 72,5\r\n\t" +
                                 "Age = 19\r\n\tGrowth = 180,1\r\n";
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintConfig_When_InvariantCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(CultureInfo.InvariantCulture);
            expectedSerializedObj[4] = "Height = 72.5";
            expectedSerializedObj[6] = "Growth = 180.1";
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }
    }
}