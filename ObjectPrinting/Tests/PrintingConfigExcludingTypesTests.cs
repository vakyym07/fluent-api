using System;
using System.Collections.Generic;
using System.Linq;
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
        public void PrintConfig_When_ExcludeDouble()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<double>();
            expectedSerializedObj.Remove(4);
            expectedSerializedObj.Remove(6);
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintConfig_When_ExcludeInt()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<int>();
            expectedSerializedObj.Remove(5);
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }

        [Test]
        public void PrintConfig_When_ExcludeString()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<string>();
            expectedSerializedObj.Remove(3);
            printer.PrintToString(person).Should().BeEquivalentTo(GetExpectedSerializedObj());
        }
    }
}