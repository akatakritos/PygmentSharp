using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using NFluent;

using PygmentSharp.Core.Formatting;
using PygmentSharp.Core.Lexing;

using Xunit;
using Xunit.Abstractions;

namespace PygmentSharp.UnitTests.Formatting
{
    public class HtmlFormatterTests
    {
        private readonly ITestOutputHelper _output;

        public HtmlFormatterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void StrippedHtmlIsSameAsInput()
        {
            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input)
                .ToArray();

            var subject = new HtmlFormatter(new HtmlFormatterOptions()
            {
                NoWrap = true
            });

            var htmlOut = new StringWriter();
            subject.Format(tokens, htmlOut);

            File.WriteAllText("output.html", htmlOut.ToString());

            var txtOut = new StringWriter();
            new NullFormatter().Format(tokens, txtOut);

            var strippedHtml = Regex.Replace(htmlOut.ToString(),
                @"<.*?>", "")
                .Trim();
            var escapedText = HtmlFormatter.EscapeHtml(txtOut.ToString()).Trim();

            Check.That(strippedHtml).IsEqualTo(escapedText);
        }

        [Fact]
        public void TestLineNumbers()
        {
            var options = new HtmlFormatterOptions()
            {
                LineNumbers = LineNumberStyle.Table
            };
            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input);

            var subject = new HtmlFormatter(options);
            var output = new StringWriter();
            subject.Format(tokens, output);

            var html = output.ToString();

            Check.That(Regex.IsMatch(html, @"<pre>\s+1\s+2\s+3"))
                .IsTrue();
        }

        [Fact]
        public void TestLineNumbersWithStart()
        {
            var options = new HtmlFormatterOptions()
            {
                LineNumbers = LineNumberStyle.Table,
                LineNumberStart = 5
            };
            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input);

            var subject = new HtmlFormatter(options);
            var output = new StringWriter();
            subject.Format(tokens, output);

            var html = output.ToString();

            Check.That(Regex.IsMatch(html, @"<pre>\s+5\s+6\s+7"))
                .IsTrue();
        }

        [Fact]
        public void TestLineAnchors()
        {
            var options = new HtmlFormatterOptions()
            {
                LineAnchors = "foo",
                AnchorLineNumbers = true
            };
            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input);

            var subject = new HtmlFormatter(options);
            var output = new StringWriter();
            subject.Format(tokens, output);

            var html = output.ToString();

            Check.That(Regex.IsMatch(html, "<a name=\"foo-1\">"))
                .IsTrue();
        }

        [Fact]
        public void Full()
        {
            var options = new HtmlFormatterOptions()
            {
                Full = true,
                Title = "My Source Code"
            };
            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input);

            var subject = new HtmlFormatter(options);
            var output = new StringWriter();
            subject.Format(tokens, output);

            var html = output.ToString();
            File.WriteAllText("output.html", html);

            Check.That(html).Contains("<html>", "<head>", "<title>My Source Code</title>");
        }

        [Fact]
        public void StyleDefaults_SpacedToHighlight()
        {
            var fmt = new HtmlFormatter();
            var sd = fmt.GetStyleDefaults();

            _output.WriteLine(sd);

            Check.That(sd).StartsWith(".highlight");
        }

        [Fact]
        public void StyleDefaults_ClassConfiguredForEachTokenType()
        {
            var fmt = new HtmlFormatter();
            var sd = fmt.GetStyleDefaults("");

            _output.WriteLine(sd);

            Check.That(sd).Contains(".c1");
        }

        [Fact]
        public void NoClass()
        {
            var options = new HtmlFormatterOptions()
            {
                NoClasses = true,
                Title = "My Source Code",
                Full = true,
            };

            var input = SampleFile.Load("csharp-sample.txt");
            var tokens = new CSharpLexer()
                .GetTokens(input);

            var subject = new HtmlFormatter(options);
            var output = new StringWriter();
            subject.Format(tokens, output);

            var html = output.ToString();
            File.WriteAllText("output.html", html);

            Check.That(html).DoesNotContain("class=\"k\""); // no keyword class
        }
    }
}
