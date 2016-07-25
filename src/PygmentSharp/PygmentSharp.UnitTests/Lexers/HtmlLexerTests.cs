using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexers
{
    public class HtmlLexerTests
    {
        private readonly ITestOutputHelper _output;
        public HtmlLexerTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

        [Fact]
        public void SupportsEmbeddedCss()
        {
            var html = @"<html><head><style>.foo { float: right; }</style></head><body>Hi</body></html>";
            var subject = new HtmlLexer();

            var tokens = subject.GetTokens(html).Where(t => t.Value != "").ToArray();

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Punctuation, "<"),
                new Token(1, TokenTypes.Name.Tag, "html"),
                new Token(5, TokenTypes.Punctuation, ">"),
                new Token(6, TokenTypes.Punctuation, "<"),
                new Token(7, TokenTypes.Name.Tag, "head"),
                new Token(11, TokenTypes.Punctuation, ">"),
                new Token(12, TokenTypes.Punctuation, "<"),
                new Token(13, TokenTypes.Name.Tag, "style"),
                new Token(18, TokenTypes.Punctuation, ">"),
                new Token(19, TokenTypes.Name.Class, ".foo"),
                new Token(23, TokenTypes.Text, " "),
                new Token(24, TokenTypes.Punctuation, "{"),
                new Token(25, TokenTypes.Text, " "),
                new Token(26, TokenTypes.Name.Builtin, "float"),
                new Token(31, TokenTypes.Operator, ":"),
                new Token(32, TokenTypes.Text, " "),
                new Token(33, TokenTypes.Name.Builtin, "right"),
                new Token(38, TokenTypes.Punctuation, ";"),
                new Token(39, TokenTypes.Text, " "),
                new Token(40, TokenTypes.Punctuation, "}"),
                new Token(41, TokenTypes.Punctuation, "<"),
                new Token(42, TokenTypes.Punctuation, "/"),
                new Token(43, TokenTypes.Name.Tag, "style"),
                new Token(48, TokenTypes.Punctuation, ">"),
                new Token(49, TokenTypes.Punctuation, "<"),
                new Token(50, TokenTypes.Punctuation, "/"),
                new Token(51, TokenTypes.Name.Tag, "head"),
                new Token(55, TokenTypes.Punctuation, ">"),
                new Token(56, TokenTypes.Punctuation, "<"),
                new Token(57, TokenTypes.Name.Tag, "body"),
                new Token(61, TokenTypes.Punctuation, ">"),
                new Token(62, TokenTypes.Text, "Hi"),
                new Token(64, TokenTypes.Punctuation, "<"),
                new Token(65, TokenTypes.Punctuation, "/"),
                new Token(66, TokenTypes.Name.Tag, "body"),
                new Token(70, TokenTypes.Punctuation, ">"),
                new Token(71, TokenTypes.Punctuation, "<"),
                new Token(72, TokenTypes.Punctuation, "/"),
                new Token(73, TokenTypes.Name.Tag, "html"),
                new Token(77, TokenTypes.Punctuation, ">")
            );
        }

        [Fact]
        public void SupportsEmbeddedJavascript()
        {
            const string html = @"<head><script>document.querySelector("".foo"").remove()</script></head><body>Hi</body></html>";
            var subject = new HtmlLexer();

            var tokens = subject.GetTokens(html).Where(t => t.Value != "").ToArray();
            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Punctuation, "<"),
                new Token(1, TokenTypes.Name.Tag, "head"),
                new Token(5, TokenTypes.Punctuation, ">"),
                new Token(6, TokenTypes.Punctuation, "<"),
                new Token(7, TokenTypes.Name.Tag, "script"),
                new Token(13, TokenTypes.Punctuation, ">"),
                new Token(14, TokenTypes.Name.Builtin, "document"),
                new Token(22, TokenTypes.Punctuation, "."),
                new Token(23, TokenTypes.Name.Other, "querySelector"),
                new Token(36, TokenTypes.Punctuation, "("),
                new Token(37, TokenTypes.Literal.String.Double, "\".foo\""),
                new Token(43, TokenTypes.Punctuation, ")"),
                new Token(44, TokenTypes.Punctuation, "."),
                new Token(45, TokenTypes.Name.Other, "remove"),
                new Token(51, TokenTypes.Punctuation, "("),
                new Token(52, TokenTypes.Punctuation, ")"),
                new Token(53, TokenTypes.Punctuation, "<"),
                new Token(54, TokenTypes.Punctuation, "/"),
                new Token(55, TokenTypes.Name.Tag, "script"),
                new Token(61, TokenTypes.Punctuation, ">"),
                new Token(62, TokenTypes.Punctuation, "<"),
                new Token(63, TokenTypes.Punctuation, "/"),
                new Token(64, TokenTypes.Name.Tag, "head"),
                new Token(68, TokenTypes.Punctuation, ">"),
                new Token(69, TokenTypes.Punctuation, "<"),
                new Token(70, TokenTypes.Name.Tag, "body"),
                new Token(74, TokenTypes.Punctuation, ">"),
                new Token(75, TokenTypes.Text, "Hi"),
                new Token(77, TokenTypes.Punctuation, "<"),
                new Token(78, TokenTypes.Punctuation, "/"),
                new Token(79, TokenTypes.Name.Tag, "body"),
                new Token(83, TokenTypes.Punctuation, ">"),
                new Token(84, TokenTypes.Punctuation, "<"),
                new Token(85, TokenTypes.Punctuation, "/"),
                new Token(86, TokenTypes.Name.Tag, "html"),
                new Token(90, TokenTypes.Punctuation, ">")
            );
        }
    }
}
