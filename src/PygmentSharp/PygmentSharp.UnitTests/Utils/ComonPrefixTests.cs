using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Utils;

using Xunit;

namespace PygmentSharp.UnitTests.Utils
{
    public class ComonPrefixTests
    {
        [Theory]
        [InlineData("foo", "foo,foobar,foobaz")]
        [InlineData("", "squid,foobar,foobaz")]
        [InlineData("bla", "blah,blahgit,blapper,bland")]
        public void FindsPrefixes(string expectedPrefix, string inputs)
        {
            var strings = inputs.Split(',');

            var result = CommonPrefix.Of(strings);

            Check.That(result).IsEqualTo(expectedPrefix);
        }
    }
}
