using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;

using Xunit;

namespace PygmentSharp.UnitTests.Lexers
{
    public class HtmlLexerTests
    {
        [Fact]
        public void Sample1()
        {
            var html = @"<html><body><div class=""foo"">Hi</div></body></html>";
            var subject = new HtmlLexer();

            var results = subject.GetTokens(html).Where(t => t.Value != "").ToArray();

            Check.That(results).ContainsExactly(
                new Token(0, TokenTypes.Punctuation, "<"),
                new Token(1, TokenTypes.Name.Tag, "html"),
                new Token(5, TokenTypes.Punctuation, ">"),
                new Token(6, TokenTypes.Punctuation, "<"),
                new Token(7, TokenTypes.Name.Tag, "body"),
                new Token(11, TokenTypes.Punctuation, ">"),
                new Token(12, TokenTypes.Punctuation, "<"),
                new Token(13, TokenTypes.Name.Tag, "div"),
                new Token(16, TokenTypes.Text, " "),
                new Token(17, TokenTypes.Name.Attribute, "class"),
                new Token(22, TokenTypes.Operator, "="),
                new Token(23, TokenTypes.Literal.String, "\"foo\""),
                new Token(28, TokenTypes.Punctuation, ">"),
                new Token(29, TokenTypes.Text, "Hi"),
                new Token(31, TokenTypes.Punctuation, "<"),
                new Token(32, TokenTypes.Punctuation, "/"),
                new Token(33, TokenTypes.Name.Tag, "div"),
                new Token(36, TokenTypes.Punctuation, ">"),
                new Token(37, TokenTypes.Punctuation, "<"),
                new Token(38, TokenTypes.Punctuation, "/"),
                new Token(39, TokenTypes.Name.Tag, "body"),
                new Token(43, TokenTypes.Punctuation, ">"),
                new Token(44, TokenTypes.Punctuation, "<"),
                new Token(45, TokenTypes.Punctuation, "/"),
                new Token(46, TokenTypes.Name.Tag, "html"),
                new Token(50, TokenTypes.Punctuation, ">"));
        }
    }
}
