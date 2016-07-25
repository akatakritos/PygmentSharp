using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;
using PygmentSharp.Core;
using Xunit;

namespace PygmentSharp.UnitTests
{
    public class RegexLexterTests
    {
        public static readonly TokenType Root = TokenTypes.Text.Create(nameof(Root));
        public static readonly TokenType Beer = TokenTypes.Text.Create(nameof(Beer));
        public static readonly TokenType Rag = TokenTypes.Text.Create(nameof(Rag));

        private sealed class TestLexer : RegexLexer
        {
            protected override IDictionary<string, StateRule[]> GetStateRules()
            {
                var builder = new StateRuleBuilder();
                var rules = new Dictionary<string, StateRule[]>();

                rules["root"] = new[]
                {
                    builder.Create("a", Root, "rag"),
                    builder.Create("e", Root),
                    builder.Default("beer", "beer")
                };

                rules["beer"] = new[]
                {
                    builder.Create("d", Beer, "#pop", "#pop"),
                };

                rules["rag"] = new[]
                {
                    builder.Create("b", Rag, "#push"),
                    builder.Create("c", Rag, "#pop", "beer"),
                };

                return rules;
            }
        }

        [Fact]
        public void SampleTest()
        {
            var subject = new TestLexer();
            var result = subject.GetTokens("abcde");

            Check.That(result).ContainsExactly(
                new Token(0, Root, "a"),
                new Token(1, Rag, "b"),
                new Token(2, Rag, "c"),
                new Token(3, Beer, "d"),
                new Token(4, Root, "e")
            );
        }

        [Fact]
        public void Multiline()
        {
            var subject = new TestLexer();
            var result = subject.GetTokens("a\ne");

            Check.That(result).ContainsExactly(
                new Token(0, Root, "a"),
                new Token(1, TokenTypes.Text, "\n"),
                new Token(2, Root, "e")
            );
        }

        [Fact]
        public void Default()
        {
            var subject = new TestLexer();
            var result = subject.GetTokens("d");

            Check.That(result).ContainsExactly(
                new Token(0, Beer, "d")
            );
        }
    }
}
