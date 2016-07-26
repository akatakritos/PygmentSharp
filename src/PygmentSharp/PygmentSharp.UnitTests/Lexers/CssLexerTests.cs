using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexers
{
    public class CssLexerTests
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _output;
        public CssLexerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Sample1()
        {
            const string css = "div, p.foo { font-style: bold; }";
            var subject = new CssLexer();

            var tokens = subject.GetTokens(css).Where(t => t.Value != "").ToArray();

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Name.Tag, "div"),
                new Token(3, TokenTypes.Operator, ","),
                new Token(4, TokenTypes.Text, " "),
                new Token(5, TokenTypes.Name.Tag, "p"),
                new Token(6, TokenTypes.Name.Class, ".foo"),
                new Token(10, TokenTypes.Text, " "),
                new Token(11, TokenTypes.Punctuation, "{"),
                new Token(12, TokenTypes.Text, " "),
                new Token(13, TokenTypes.Name.Builtin, "font-style"),
                new Token(23, TokenTypes.Operator, ":"),
                new Token(24, TokenTypes.Text, " "),
                new Token(25, TokenTypes.Name.Builtin, "bold"),
                new Token(29, TokenTypes.Punctuation, ";"),
                new Token(30, TokenTypes.Text, " "),
                new Token(31, TokenTypes.Punctuation, "}")
             );
        }
    }

    internal static class TestHelpers
    {
        public static string DumpForCode(this IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.AppendLine($"new Token({token.Index}, TokenTypes.{token.Type.ToString().Replace("Token.","")}, \"{token.Value}\"),");
            }

            return sb.ToString();
        }
    }
}
