using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using PygmentSharp.Core;
using Xunit;

namespace PygmentSharp.UnitTests
{
    public class StyleTests
    {
        private static readonly TokenType Root = new TokenType(null, "Root");
        private static readonly TokenType Foo = Root.Create(nameof(Foo));
        private static readonly TokenType Bar = Root.Create(nameof(Bar));
        private static readonly TokenType Baz = Root.Create(nameof(Baz));

        [Fact]
        public void GetsStylesFromStrings()
        {
            var dict = new Dictionary<TokenType, string>()
            {
                {Root, "#012"},
                {Foo, "#123abc bold underline mono"},
            };

            var subject = new Style(dict);

            Check.That(subject.StyleForToken(Root))
                .IsEqualTo(new StyleData(color: "001122"));
            Check.That(subject[Foo])
                .IsEqualTo(new StyleData(color: "123abc", bold: true, underline: true, mono: true));
        }

        [Fact]
        public void StyleForToken_GetsNullForUnknown()
        {
            var subject = new Style();

            Check.That(subject.StyleForToken(Root)).IsNull();
            Check.That(subject[Root]).IsNull();
        }

        [Fact]
        public void IsEnumerable()
        {
            var dict = new Dictionary<TokenType, string>()
            {
                {Root, "#012"},
                {Foo, "#123abc bold underline mono"},
            };

            var subject = new Style(dict);

            Check.That(subject.Select(kvp => kvp.Key.Name))
                .ContainsExactly("Root", "Foo");
        }

    }
}
