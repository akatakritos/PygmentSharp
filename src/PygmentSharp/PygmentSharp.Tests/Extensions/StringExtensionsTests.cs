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
            var result = a.PythonAnd(b).PythonOr(c);

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

        [Theory]
        [InlineData("code.cs", "*.cs")]
        [InlineData(@"C:\some\directory\code.cs", "*.cs")]
        [InlineData(@"some\relative-directory\code.cs", "*.cs")]
        [InlineData(@"App.config", "App.config")]
        [InlineData(@"C:\some\directory\Web.config", "Web.config")]
        public void MatchesWildcard(string filename, string wildcard)
        {
            Check.That(filename.MatchesFileWildcard(wildcard)).IsTrue();
        }

        [Theory]
        [InlineData(@"/home/bob/some/directory/code.cs", "*.cs")]
        [InlineData(@"~/some/directory/code.cs", "*.cs")]
        [InlineData(@"some/relative-directory/code.cs", "*.cs")]
        [InlineData(@"C:/some/directory/Web.config", "Web.config")]
        public void MatchesWildcard_SupportsUnixPathSeparator(string filename, string wildcard)
        {
            Check.That(filename.MatchesFileWildcard(wildcard)).IsTrue();
        }

        [Fact]
        public void MatchesWildcard_CaseInsensitive()
        {
            Check.That("SomeFile.CS".MatchesFileWildcard("*.cs")).IsTrue();
        }

        [Theory]
        [InlineData(@"c:\somefile\stuff.cs\myfile.pdf", "*.cs")]
        [InlineData(@"relative/directory/app.configs/foo.bar", "app.config")]
        public void DoesntMatchDirectoryNames(string filename, string wildcard)
        {
            Check.That(filename.MatchesFileWildcard(wildcard)).IsFalse();
        }
    }
}
