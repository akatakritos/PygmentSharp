using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Lexing;

using Xunit;

namespace PygmentSharp.UnitTests
{
    public class LexerLocatorTests
    {
        [Theory]
        [InlineData("C#", typeof(CSharpLexer))]
        [InlineData("SQL", typeof(SqlLexer))]
        public void FindByName_SearchesAttributeName(string name, Type lexerType)
        {
            var lexer = LexerLocator.FindByName(name);

            Check.That(lexer.GetType()).IsEqualTo(lexerType);
        }

        [Fact]
        public void FindByName_SearchesAlternateNames()
        {
            var lexer = LexerLocator.FindByName("c-sharp");

            Check.That(lexer).IsInstanceOf<CSharpLexer>();
        }

        [Fact]
        public void FindByName_ReturnsNullForNotFound()
        {
            var lexer = LexerLocator.FindByName("hotpotato");

            Check.That(lexer).IsNull();
        }

        [Theory]
        [InlineData("file.cs", typeof(CSharpLexer))]
        [InlineData("bigdata.sql", typeof(SqlLexer))]
        public void FindByFilename_SearchesForFileExtensions(string file, Type lexerType)
        {
            var lexer = LexerLocator.FindByFilename(file);

            Check.That(lexer.GetType()).IsEqualTo(lexerType);
        }

        [Fact]
        public void FindByFilename_ReturnsNullForNotFound()
        {
            var lexer = LexerLocator.FindByFilename("*.trump");

            Check.That(lexer).IsNull();
        }

        [Fact]
        public void SearchesCaseInsensitively()
        {
            var lexer = LexerLocator.FindByName("sql");
            Check.That(lexer).IsNotNull();
        }

        [Theory]
        [InlineData("js", typeof(JavascriptLexer))] // #6
        [InlineData("text", typeof(PlainLexer))] // #7
        [InlineData("json", typeof(JavascriptLexer))] // #5
        public void LexerLocationRegressions(string search, Type expectedLexer)
        {
            var lexer = LexerLocator.FindByName(search);
            Check.That(lexer.GetType()).IsEqualTo(expectedLexer);
        }
    }
}