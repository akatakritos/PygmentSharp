using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;
using Xunit;

namespace PygmentSharp.UnitTests
{
    public class LexerLocatorTests
    {
        [Fact]
        public void FindByName_SearchesAttributeName()
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByName("C#");

            Check.That(lexer).IsInstanceOf<CSharpLexer>();
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

        [Fact]
        public void FindByFilename_SearchesForFileExtensions()
        {
            var subject = new LexerLocator();

            var lexer = subject.FindByFilename("file.cs");

            Check.That(lexer).IsInstanceOf<CSharpLexer>();
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
