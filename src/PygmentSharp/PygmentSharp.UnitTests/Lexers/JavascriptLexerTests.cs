using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Lexers;
using PygmentSharp.Core.Tokens;

using Xunit;

namespace PygmentSharp.UnitTests.Lexers
{
    public class JavascriptLexerTests
    {
        [Fact]
        public void Sample1()
        {
            const string js = @"(function($) { $("".foo"").hide(); })(jQuery);";

            var subject = new JavascriptLexer();

            var tokens = subject.GetTokens(js).Where(t => t.Value != "").ToArray();

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Punctuation, "("),
                new Token(1, TokenTypes.Keyword.Declaration, "function"),
                new Token(9, TokenTypes.Punctuation, "("),
                new Token(10, TokenTypes.Name.Other, "$"),
                new Token(11, TokenTypes.Punctuation, ")"),
                new Token(12, TokenTypes.Text, " "),
                new Token(13, TokenTypes.Punctuation, "{"),
                new Token(14, TokenTypes.Text, " "),
                new Token(15, TokenTypes.Name.Other, "$"),
                new Token(16, TokenTypes.Punctuation, "("),
                new Token(17, TokenTypes.Literal.String.Double, @""".foo"""),
                new Token(23, TokenTypes.Punctuation, ")"),
                new Token(24, TokenTypes.Punctuation, "."),
                new Token(25, TokenTypes.Name.Other, "hide"),
                new Token(29, TokenTypes.Punctuation, "("),
                new Token(30, TokenTypes.Punctuation, ")"),
                new Token(31, TokenTypes.Punctuation, ";"),
                new Token(32, TokenTypes.Text, " "),
                new Token(33, TokenTypes.Punctuation, "}"),
                new Token(34, TokenTypes.Punctuation, ")"),
                new Token(35, TokenTypes.Punctuation, "("),
                new Token(36, TokenTypes.Name.Other, "jQuery"),
                new Token(42, TokenTypes.Punctuation, ")"),
                new Token(43, TokenTypes.Punctuation, ";")
                );
        }
    }
}
