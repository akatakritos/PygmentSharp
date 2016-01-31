using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using PygmentSharp.Core;
using PygmentSharp.Core.Lexers;
using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexers
{
    public class ExampleFileTests
    {
        private readonly ITestOutputHelper _output;

        public ExampleFileTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData("LexerSetup")]
        public void LexerTest(Lexer subject, string exampleName)
        {
            var tokens = subject.GetTokens(SampleFile.Load(exampleName))
                .ToArray();



            foreach (var t in tokens)
            {
                _output.WriteLine(t.ToString());

                if (t.Type == TokenTypes.Error)
                {
                    throw new Exception($"Lexer reported an error at pos {t.Index} : '{t.Value}'");
                }
            }

        }

        public static IEnumerable<object[]> LexerSetup
        {
            get
            {
                yield return new object[] {new CSharpLexer(), "csharp-sample.txt"};

            }
        }
    }
}
