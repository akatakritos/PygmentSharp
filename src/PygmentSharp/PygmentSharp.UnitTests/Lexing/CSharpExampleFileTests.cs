using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Formatting;
using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Lexing
{
    public class CSharpExampleFileTests
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper _output;
        private readonly Token[] _results;

        public CSharpExampleFileTests(ITestOutputHelper output)
        {
            _output = output;
            var subject = new CSharpLexer();
            _results = subject.GetTokens(SampleFile.Load("csharp-sample.txt"))
                .ToArray();
        }


        [Fact]
        public void DoesntContainErrorTokens()
        {

            foreach (var t in _results)
            {
                //_output.WriteLine(t.ToString());

                if (t.Type == TokenTypes.Error)
                {
                    throw new Exception($"Lexer reported an error at pos {t.Index} : '{t.Value}'");
                }
            }

        }

        [Fact]
        public void Contains8NamespaceOrUsings()
        {
            var usings = _results.Where(t => t.Type == TokenTypes.Name.Namespace)
                .ToArray();

            Check.That(usings).HasSize(8);
        }

        [Fact]
        public void FirstNamespaceIsDivaCore()
        {
            var nmspace = _results.First(t => t.Type == TokenTypes.Name.Namespace);

            Check.That(nmspace.Value).IsEqualTo("Diva.Core");
        }

        [Fact]
        public void NoCharactersAreLost()
        {
            var expected = SampleFile.Load("csharp-sample.txt");

            var writer = new StringWriter();
            new NullFormatter().Format(_results, writer);

            var s = writer.ToString().Replace("\n", "\r\n");

            Check.That(s).IsEqualTo(expected);
        }
    }
}
