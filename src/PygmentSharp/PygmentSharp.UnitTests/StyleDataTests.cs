using System.Collections.Generic;
using NFluent;
using PygmentSharp.Core;
using Xunit;

namespace PygmentSharp.UnitTests
{
    public class StyleDataTests
    {
        [Theory]
        [MemberData("ParseData")]
        public void Parse(string text, StyleData expected)
        {
            var result = StyleData.Parse(text);

            Check.That(result).IsEqualTo(expected);

        }

        public static IEnumerable<object[]> ParseData
        {
            get
            {
                yield return new object[] {"#bbbbbb", new StyleData(color: "bbbbbb") };
                yield return new object[] {"#888", new StyleData(color: "888888" )};
                yield return new object[] {"bold #cc0000", new StyleData(color: "cc0000", bold: true) };
                yield return new object[] {"bg:#fff0f0", new StyleData(bgColor: "fff0f0") };
                yield return new object[] {"#04D bg:", new StyleData(color: "0044DD", bgColor: "") };
                yield return new object[] {"italic", new StyleData(italic: true)};
            }
        }
    }
}