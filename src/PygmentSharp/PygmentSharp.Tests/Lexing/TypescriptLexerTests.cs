using NFluent;
using PygmentSharp.Core.Lexing;
using PygmentSharp.Core.Tokens;
using PygmentSharp.UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PygmentSharp.Tests.Lexing
{
    public class TypescriptLexerTests
    {
        [Fact]
        public void ClassSample()
        {
            var ts = @"
            class Animal {
                constructor(public name) { }
                move(meters) {
                    alert(this.name + "" moved "" + meters + ""m."");
                }
            }";

            var tokens = new TypescriptLexer().GetTokens(ts)
                .Where(t => !string.IsNullOrWhiteSpace(t.Value));

            Check.That(tokens).ContainsExactly(
                new Token(13, TokenTypes.Keyword.Reserved, "class"),
                new Token(19, TokenTypes.Name.Other, "Animal"),
                new Token(26, TokenTypes.Punctuation, "{"),
                new Token(44, TokenTypes.Keyword.Reserved, "constructor"),
                new Token(55, TokenTypes.Punctuation, "("),
                new Token(56, TokenTypes.Keyword.Reserved, "public"),
                new Token(63, TokenTypes.Name.Other, "name"),
                new Token(67, TokenTypes.Punctuation, ")"),
                new Token(69, TokenTypes.Punctuation, "{"),
                new Token(71, TokenTypes.Punctuation, "}"),
                new Token(89, TokenTypes.Name.Other, "move"),
                new Token(93, TokenTypes.Punctuation, "("),
                new Token(94, TokenTypes.Name.Other, "meters"),
                new Token(100, TokenTypes.Punctuation, ")"),
                new Token(102, TokenTypes.Punctuation, "{"),
                new Token(124, TokenTypes.Name.Other, "alert"),
                new Token(129, TokenTypes.Punctuation, "("),
                new Token(130, TokenTypes.Keyword, "this"),
                new Token(134, TokenTypes.Punctuation, "."),
                new Token(135, TokenTypes.Name.Other, "name"),
                new Token(140, TokenTypes.Operator, "+"),
                new Token(142, TokenTypes.Literal.String.Double, "\" moved \""),
                new Token(152, TokenTypes.Operator, "+"),
                new Token(154, TokenTypes.Name.Other, "meters"),
                new Token(161, TokenTypes.Operator, "+"),
                new Token(163, TokenTypes.Literal.String.Double, "\"m.\""),
                new Token(167, TokenTypes.Punctuation, ")"),
                new Token(168, TokenTypes.Punctuation, ";"),
                new Token(186, TokenTypes.Punctuation, "}"),
                new Token(200, TokenTypes.Punctuation, "}")
            );
        }

        [Fact]
        public void TypeAnnotations()
        {
            const string code = @"
                var sam = new Snake(""Sammy the Python"")
                var tom: Animal = new Horse(""Tommy the Palomino"")

                sam.move()
                tom.move(34)";

            var tokens = new TypescriptLexer().GetTokens(code).ToList();

            var animalToken = tokens.Single(t => t.Value == "Animal");
            Check.That(animalToken.Type).IsEqualTo(TokenTypes.Keyword.Type);
        }

        [Fact]
        public void Generics()
        {
            const string code = "export type Result<T, E> = Ok<T> | Err<E>";
            var tokens = new TypescriptLexer().GetTokens(code);

            Check.That(tokens).ContainsExactly(
                new Token(0, TokenTypes.Keyword.Reserved, "export"),
                new Token(6, TokenTypes.Text, " "),
                new Token(7, TokenTypes.Name.Other, "type"),
                new Token(11, TokenTypes.Text, " "),
                new Token(12, TokenTypes.Name.Other, "Result"),
                new Token(18, TokenTypes.Operator, "<"),
                new Token(19, TokenTypes.Name.Other, "T"),
                new Token(20, TokenTypes.Punctuation, ","),
                new Token(21, TokenTypes.Text, " "),
                new Token(22, TokenTypes.Name.Other, "E"),
                new Token(23, TokenTypes.Operator, ">"),
                new Token(24, TokenTypes.Text, " "),
                new Token(25, TokenTypes.Operator, "="),
                new Token(26, TokenTypes.Text, " "),
                new Token(27, TokenTypes.Name.Other, "Ok"),
                new Token(29, TokenTypes.Operator, "<"),
                new Token(30, TokenTypes.Name.Other, "T"),
                new Token(31, TokenTypes.Operator, ">"),
                new Token(32, TokenTypes.Text, " "),
                new Token(33, TokenTypes.Operator, "|"),
                new Token(34, TokenTypes.Text, " "),
                new Token(35, TokenTypes.Name.Other, "Err"),
                new Token(38, TokenTypes.Operator, "<"),
                new Token(39, TokenTypes.Name.Other, "E"),
                new Token(40, TokenTypes.Operator, ">"));
        }

        [Fact]
        public void StartingWithJsDoc_issue_21()
        {
            var code = @"
/**
 * Converts a list of elements into a list of batches each of maximum size `batchSize`.
 * @param list - list of elements
 * @param batchSize - the size of the batch
 */
export function batch<T>(list: T[], batchSize: number): T[][] {
  const batches: T[][] = [];
  let currentBatch: T[] = [];

  list.forEach(element => {
    currentBatch.push(element);

    if (currentBatch.length === batchSize) {
      batches.push(currentBatch);
      currentBatch = [];
    }
  });

  if (currentBatch.length > 0) {
    batches.push(currentBatch);
  }

  return batches;
}
";
            var lexer = new TypescriptLexer();
            var tokens = lexer.GetTokens(code.Trim());

            Check.That(tokens).Not.HasElementThatMatches(t => t.Type == TokenTypes.Error);
        }

    }
}
