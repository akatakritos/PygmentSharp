using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NFluent;

using PygmentSharp.Core;
using PygmentSharp.Core.Formatters;
using PygmentSharp.Core.Lexers;

using Xunit;

namespace PygmentSharp.UnitTests
{
    public class FluentApiTests
    {
        [Fact]
        public void BaseCase()
        {
            var result = Pygmentize.Content("")
                .WithLexer(new PlainLexer())
                .WithFormatter(new NullFormatter())
                .AsString();


            Check.That(result).IsEqualTo("");
        }

        [Fact]
        public void ToHtml()
        {
            var result = Pygmentize.Content("class Foo { }")
                .WithLexer(new CSharpLexer())
                .ToHtml()
                .AsString();

            Check.That(result).StartsWith("<div class=\"highlight\" >");

        }

        [Fact]
        public void ToFile()
        {
            var file = Path.GetTempFileName() + ".html"; //autodetects output formatter from filename

            Pygmentize.Content("class Foo { }")
                .WithLexer(new CSharpLexer())
                .ToFile(file);

            var result = File.ReadAllText(file);

            Check.That(result).StartsWith("<div class=\"highlight\" >");
        }

        [Fact]
        public void FromFile()
        {
            var filename = Path.GetTempFileName() + ".cs"; // autodetects lexer from filename
            File.WriteAllText(filename, "class Foo { }");

            var result = Pygmentize.File(filename)
                .WithFormatter(new NullFormatter())
                .AsString();

            Check.That(result).IsEqualTo("class Foo { }");
        }
    }
}
