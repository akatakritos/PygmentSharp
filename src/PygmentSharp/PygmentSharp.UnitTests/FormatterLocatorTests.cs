using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Formatters;

using Xunit;

namespace PygmentSharp.UnitTests
{
    public class FormatterLocatorTests
    {
        [Fact]
        public void FindByName_SearchesAttributeName()
        {
            var subject = new FormatterLocator();

            var lexer = subject.FindByName("HTML");

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByName_SearchesAlternateNames()
        {
            var subject = new FormatterLocator();

            var lexer = subject.FindByName("web");

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByName_ReturnsNullForNotFound()
        {
            var subject = new FormatterLocator();

            var lexer = subject.FindByName("hotpotato");

            Check.That(lexer).IsNull();
        }

        [Theory]
        [InlineData("file.html")]
        [InlineData("file.htm")]
        public void FindByFilename_SearchesForFileExtensions(string filename)
        {
            var subject = new FormatterLocator();

            var lexer = subject.FindByFilename(filename);

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByFilename_RetusnNullForNotFound()
        {
            var subject = new FormatterLocator();

            var lexer = subject.FindByFilename("crazy.trump");

            Check.That(lexer).IsNull();
        }
    }
}