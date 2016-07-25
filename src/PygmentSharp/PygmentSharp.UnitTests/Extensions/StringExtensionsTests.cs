using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Extensions;

using Xunit;

namespace PygmentSharp.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("", "foo", "")]
        [InlineData(null, "foo", null)]
        [InlineData("cat", "dog", "dog")]
        public void PythonAnd(string left, string right, string expectedResult)
        {
            var result = left.PythonAnd(right);

            Check.That(result).IsEqualTo(expectedResult);
        }

        [Theory]
        [InlineData("", "foo", "foo")]
        [InlineData(null, "foo", "foo")]
        [InlineData("cat", "dog", "cat")]
        public void PythonOr(string left, string right, string expectedResult)
        {
            var result = left.PythonOr(right);

            Check.That(result).IsEqualTo(expectedResult);
        }

        [Theory]
        [InlineData("a", "b", "c", "b")]
        [InlineData(null, "b", "c", "c")]
        [InlineData("", "b","c","c")]
        public void PythonAndOr(string a, string b, string c, string expectedResult)
        {
            var result = a.PythonAnd(b).PythonOr("c");

            Check.That(result).IsEqualTo(expectedResult);
        }

        [Theory]
        [InlineData("foobar", "raboof")]
        [InlineData("racecar", "racecar")]
        public void Backwards(string input, string expectedOutput)
        {
            var result = input.Backwards();
            Check.That(result).IsEqualTo(expectedOutput);
        }
    }
}
