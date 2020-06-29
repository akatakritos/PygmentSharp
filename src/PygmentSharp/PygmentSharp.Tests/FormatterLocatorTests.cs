using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Formatting;

using Xunit;

namespace PygmentSharp.UnitTests
{
    public class FormatterLocatorTests
    {
        [Fact]
        public void FindByName_SearchesAttributeName()
        {
            var lexer = FormatterLocator.FindByName("HTML");

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByName_SearchesAlternateNames()
        {
            var lexer = FormatterLocator.FindByName("web");

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByName_ReturnsNullForNotFound()
        {
            var lexer = FormatterLocator.FindByName("hotpotato");

            Check.That(lexer).IsNull();
        }

        [Theory]
        [InlineData("file.html")]
        [InlineData("file.htm")]
        public void FindByFilename_SearchesForFileExtensions(string filename)
        {
            var lexer = FormatterLocator.FindByFilename(filename);

            Check.That(lexer).IsInstanceOf<HtmlFormatter>();
        }

        [Fact]
        public void FindByFilename_RetusnNullForNotFound()
        {
            var lexer = FormatterLocator.FindByFilename("crazy.trump");

            Check.That(lexer).IsNull();
        }
    }
}