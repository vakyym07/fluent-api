using System;
using System.Collections.Generic;
using System.Linq;
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
        public void PrintingConfig_When_AlternativeModeForAge()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(p => p.Age).Using(age => $"Age: {age.ToString()} y.o.");
            expectedSerializedObj[5] = "Age: 19 y.o.";
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintingConfig_When_AlternativeModeForName()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(p => p.Name).Using(name => $"Aka {name.ToString()}");
            expectedSerializedObj[3] = "Aka Alex";
            var expectedResult = GetExpectedSerializedObj();
            printer.PrintToString(person).Should().BeEquivalentTo(expectedResult);
        }
    }
}