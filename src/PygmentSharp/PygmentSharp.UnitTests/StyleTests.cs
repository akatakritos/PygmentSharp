using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;
using PygmentSharp.Core;
using Xunit;

namespace PygmentSharp.UnitTests
{
    public class StyleTests
    {
        private static readonly TokenType _root = new TokenType(null, "Root");
        private static readonly TokenType _foo = _root.CreateChild("Foo");
        // ReSharper disable once UnusedMember.Local
        private static readonly TokenType _bar = _root.CreateChild("Bar");
        // ReSharper disable once UnusedMember.Local
        private static readonly TokenType _baz = _root.CreateChild("Baz");

        [Fact]
        public void GetsStylesFromStrings()
        {
            var dict = new Dictionary<TokenType, string>()
            {
                {_root, "#012"},
                {_foo, "#123abc bold underline mono"},
            };

            var subject = new Style(dict);

            Check.That(subject.StyleForToken(_root))
                .IsEqualTo(new StyleData(color: "001122"));
            Check.That(subject[_foo])
                .IsEqualTo(new StyleData(color: "123abc", bold: true, underline: true, mono: true));
        }

        [Fact]
        public void StyleForToken_GetsNullForUnknown()
        {
            var subject = new Style();

            Check.That(subject.StyleForToken(_root)).IsNull();
            Check.That(subject[_root]).IsNull();
        }

        [Fact]
        public void IsEnumerable()
        {
            var dict = new Dictionary<TokenType, string>()
            {
                {_root, "#012"},
                {_foo, "#123abc bold underline mono"},
            };

            var subject = new Style(dict);

            // other styles from the default list are added as well
            Check.That(subject.Select(kvp => kvp.Key.Name))
                .Contains("Root", "Foo");
        }

    }
}
