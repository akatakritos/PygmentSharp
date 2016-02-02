using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NFluent;
using PygmentSharp.Core.Formatters;
using PygmentSharp.Core.Lexers;
using Xunit;

namespace PygmentSharp.UnitTests.Formatters
{
    public class HtmlFormatterTests
    {
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
    }
}
