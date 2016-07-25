using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;
using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;
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
            var subject = new LexerLocator();

            var lexer = subject.FindByName(name);

            Check.That(lexer.GetType()).IsEqualTo(lexerType);
        }

        [Fact]
        public void FindByName_SearchesAlternateNames()
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByName("c-sharp");

            Check.That(lexer).IsInstanceOf<CSharpLexer>();
        }

        [Fact]
        public void FindByName_ReturnsNullForNotFound()
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByName("hotpotato");

            Check.That(lexer).IsNull();
        }

        [Theory]
        [InlineData("file.cs", typeof(CSharpLexer))]
        [InlineData("bigdata.sql", typeof(SqlLexer))]
        public void FindByFilename_SearchesForFileExtensions(string file, Type lexerType)
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByFilename(file);

            Check.That(lexer.GetType()).IsEqualTo(lexerType);
        }

        [Fact]
        public void FindByFilename_RetusnNullForNotFound()
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByFilename("*.trump");

            Check.That(lexer).IsNull();
        }
    }
}
