using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            person = new Person { Name = "Alex", Age = 19, Height = 72.5, Growth = 180.1 };
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
        public void PrintingConfig_When_ExtensionMethodAndPrintntingConfigDefualt()
        {
            person.PrintToString().Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintingConfig_When_ExtensionMethodAndPrintntingConfigWithParametrs()
        {
            expectedSerializedObj.Remove(4);
            expectedSerializedObj.Remove(6);
            expectedSerializedObj[3] = "Name = Al";
            person.PrintToString(s => s.Exclude<double>().Printing(p => p.Name).TrimmedToLength(2))
                .Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintingConfig_When_ExcludeIntExcludeIdAlternativeModeForNameWithCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<int>()
                .Exclude<Guid>()
                .Printing(p => p.Name).Using(name => $"Aka {name.ToString()}")
                .Printing<double>().Using(CultureInfo.InvariantCulture);
            expectedSerializedObj.Remove(2);
            expectedSerializedObj.Remove(5);
            expectedSerializedObj[3] = "Aka Alex";
            expectedSerializedObj[4] = "Height = 72.5";
            expectedSerializedObj[6] = "Growth = 180.1";
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }
    }
}