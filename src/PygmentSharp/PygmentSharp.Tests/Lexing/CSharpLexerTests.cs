using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexing
{
    public class CSharpLexerTests
    {
        private readonly ITestOutputHelper _output;
        public CSharpLexerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetsCorrectTokensWhenSourceContainsChar()
        {
            const string code = "using System; namespace Foo { class Bar { private char _baz = 'c'; } }";
            var subject = new CSharpLexer();

            var tokens = subject.GetTokens(code).ToArray();

            Check.That(tokens[0]).IsEqualTo(new Token(0, TokenTypes.Keyword, "using"));
        }

        [Fact]
        public void MethodDeclaration()
        {
            const string code = "public override void Foo()";
            var subject = new CSharpLexer();

            var tokens = subject.GetTokens(code).ToArray();

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Keyword, "public"),
                new Token(6, TokenTypes.Text, " "),
                new Token(7, TokenTypes.Keyword, "override"),
                new Token(15, TokenTypes.Text, " "),
                new Token(16, TokenTypes.Keyword, "void"),
                new Token(20, TokenTypes.Text, " "),
                new Token(21, TokenTypes.Name.Function, "Foo"),
                new Token(24, TokenTypes.Text, ""),
                new Token(24, TokenTypes.Punctuation, "("),
                new Token(25, TokenTypes.Punctuation, ")"));
        }
    }
}
