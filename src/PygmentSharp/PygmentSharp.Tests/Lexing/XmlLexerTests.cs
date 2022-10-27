using System.Linq;

using NFluent;

using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexing
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

            Check.That(tokens).ContainsExactly(
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
                new Token(38, TokenTypes.Name.Tag, "</baz>"),
                new Token(44, TokenTypes.Name.Tag, "</xml>")
            );
        }

        [Fact]
        public void SupportsEntities()
        {
            const string xml = "&lt;&gt;";
            var subject = new XmlLexer();
            
            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Name.Entity, "&lt;"),
                new Token(4, TokenTypes.Name.Entity, "&gt;")
            );
        }

        [Fact]
        public void SupportsCData()
        {
            const string xml = "<xml><![CDATA[data]]></xml>";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Name.Tag, "<xml"),
                new Token(4, TokenTypes.Name.Tag, ">"),
                new Token(5, TokenTypes.Comment.Preproc, "<![CDATA[data]]>"),
                new Token(21, TokenTypes.Name.Tag, "</xml>")
            );
        }

        [Fact]
        public void SupportsMultilineComment()
        {
            const string xml = "<!-- Multiline\nComment -->";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Comment.Multiline, xml)
            );
        }

        [Fact]
        public void SupportsXmlDeclaration()
        {
            const string xml = "<?xml version=\"1.0\"?>";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Comment.Preproc, xml)
            );
        }

        [Fact]
        public void SupportsDoctype()
        {
            const string xml = "<!DOCTYPE document SYSTEM \"document.dtd\">";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Comment.Preproc, xml)
            );
        }

        [Fact]
        public void SupportsMultilineAttributes()
        {
            const string xml = "<xml foo=\"bar\nbaz\" />";
            var subject = new XmlLexer();

            var tokens = subject.GetTokens(xml).ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Name.Tag, "<xml"),
                new Token(4, TokenTypes.Text, " "),
                new Token(5, TokenTypes.Name.Attribute, "foo="),
                new Token(9, TokenTypes.Literal.String, "\"bar\nbaz\""),
                new Token(18, TokenTypes.Text, " "),
                new Token(19, TokenTypes.Name.Tag, "/>")
            );
        }
    }
}
