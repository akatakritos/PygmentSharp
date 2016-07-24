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
    public class SqlLexerTests
    {
        [Fact]
        public void GetsSomeTokens()
        {
            var subject = new SqlLexer();

            var result = subject.GetTokens("SELECT * FROM Customers");

            Check.That(result).HasSize(7);
        }

        [Fact]
        public void IgnoresCase()
        {
            var subject = new SqlLexer();

            var result = subject.GetTokens("select * from Customers");

            Check.That(result.First()).IsEqualTo(
                new Token(0, TokenTypes.Keyword, "select"));
        }
    }
}
