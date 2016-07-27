using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFluent;

using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexing
{
    public class BashLexerTests
    {
        private readonly ITestOutputHelper _output;
        public BashLexerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Sample1()
        {
            var bashs = @"#!/bin/bash
echo ""Hello, $WORLD""
grep foo > file.txt
";
            var subject = new BashLexer();

            var tokens = subject.GetTokens(bashs).Where(t => t.Value != "").ToArray();

            _output.WriteLine(tokens.DumpForCode());

            Check.That(tokens).ContainsExactly(
               new Token(0, TokenTypes.Comment.Hashbang, "#!/bin/bash\n"),
                new Token(12, TokenTypes.Name.Builtin, "echo"),
                new Token(16, TokenTypes.Text, " "),
                new Token(17, TokenTypes.Literal.String.Double, "\""),
                new Token(18, TokenTypes.Literal.String.Double, "Hello, "),
                new Token(25, TokenTypes.Name.Variable, "$WORLD"),
                new Token(31, TokenTypes.Literal.String.Double, "\""),
                new Token(32, TokenTypes.Text, "\n"),
                new Token(33, TokenTypes.Text, "grep"),
                new Token(37, TokenTypes.Text, " "),
                new Token(38, TokenTypes.Text, "foo"),
                new Token(41, TokenTypes.Text, " "),
                new Token(42, TokenTypes.Text, ">"),
                new Token(43, TokenTypes.Text, " "),
                new Token(44, TokenTypes.Text, "file.txt"),
                new Token(52, TokenTypes.Text, "\n")
            );
        }
    }
}
