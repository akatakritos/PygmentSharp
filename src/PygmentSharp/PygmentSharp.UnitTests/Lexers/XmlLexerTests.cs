using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Lexers;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexers
{
    public class XmlLexerTests
    {
        private readonly ITestOutputHelper _output;
        public XmlLexerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Sample1()
        {
            const string xml = "<xml><something foo=\"bar\" /><baz>zoink</baz></xml>";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).Contains(
                new Token(0, TokenTypes.Name.Tag, "<xml"),
                new Token(4, TokenTypes.Name.Tag, ">"),
                new Token(5, TokenTypes.Name.Tag, "<something"),
                new Token(15, TokenTypes.Text, " "),
                new Token(16, TokenTypes.Name.Attribute, "foo="),
                new Token(20, TokenTypes.Literal.String, "\"bar\""),
                new Token(25, TokenTypes.Text, " "),
                new Token(26, TokenTypes.Name.Tag, "/>"),
                new Token(28, TokenTypes.Name.Tag, "<baz"),
                new Token(32, TokenTypes.Name.Tag, ">"),
                new Token(33, TokenTypes.Text, "zoink"),
                new Token(38, TokenTypes.Error, "<"),
                new Token(39, TokenTypes.Text, "/baz>"),
                new Token(44, TokenTypes.Error, "<"),
                new Token(45, TokenTypes.Text, "/xml>")
            );
        }

    }
}
