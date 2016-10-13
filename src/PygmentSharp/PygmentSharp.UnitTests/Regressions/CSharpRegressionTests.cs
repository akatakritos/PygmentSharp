using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Formatting;
using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;

namespace PygmentSharp.UnitTests.Regressions
{
    public class CSharpRegressionTests
    {
        [Theory]
        [InlineData("8-onmodelcreating.txt")]
        [InlineData("8-preparelayout.txt")]
        [InlineData("8-aspnet-codebehind.txt")]
        [InlineData("8-x509.txt")]
        public void IndexOutOfRange_8(string fixture)
        {
            var content = SampleFile.Load(fixture);
            var lexer = new CSharpLexer();
            Check.ThatCode(() => lexer.GetTokens(content)).DoesNotThrow();
        }
    }
}
