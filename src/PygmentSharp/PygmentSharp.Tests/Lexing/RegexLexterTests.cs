using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;

using Xunit;

namespace PygmentSharp.UnitTests.Lexing
{
    public class RegexLexterTests
    {
        public static readonly TokenType Root = TokenTypes.Text.CreateChild(nameof(Root));
        public static readonly TokenType Beer = TokenTypes.Text.CreateChild(nameof(Beer));
        public static readonly TokenType Rag = TokenTypes.Text.CreateChild(nameof(Rag));

        private sealed class TestLexer : RegexLexer
        {
            protected override IDictionary<string, StateRule[]> GetStateRules()
            {
                var builder = new StateRuleBuilder();
                var rules = new Dictionary<string, StateRule[]>();

                rules["root"] = builder.NewRuleSet()
                    .Add("a", Root, "rag")
                    .Add("e", Root)
                    .Default("beer", "beer")
                    .Build();

                rules["beer"] = builder.NewRuleSet()
                    .Add("d", Beer, "#pop", "#pop")
                    .Build();

                rules["rag"] = builder.NewRuleSet()
                    .Add("b", Rag, "#push")
                    .Add("c", Rag, "#pop", "beer")
                    .Build();

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
