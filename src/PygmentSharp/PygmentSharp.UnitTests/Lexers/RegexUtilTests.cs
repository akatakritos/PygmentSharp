using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFluent;

using PygmentSharp.Core.Lexers;

using Xunit;

namespace PygmentSharp.UnitTests.Lexers
{
    public class RegexUtilTests
    {
        [Theory]
        [InlineData("abc", "[abc]")]
        [InlineData("ab]", @"[ab\]]")]
        [InlineData("^-\\]", @"[\^\-\\\]]")]
        public void MakeCharset(string letters, string expectedRegex)
        {
            var result = RegexUtil.MakeCharset(letters);

            Check.That(result).IsEqualTo(expectedRegex);
        }

    }
}
