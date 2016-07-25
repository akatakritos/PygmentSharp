using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PygmentSharp.Core
{
    public class StateRule
    {
        public Regex Regex { get; }
        public TokenType TokenType { get; }
        public StateAction Action { get; }

        internal StateRule(Regex regex, TokenType tokenType, StateAction action)
        {
            Regex = regex;
            TokenType = tokenType;
            Action = action;
        }

        public override string ToString()
        {
            return $"{Regex} -> {TokenType}";
        }
    }

    public abstract class GroupProcessor
    {
        public abstract IEnumerable<Token> GetTokens(RegexLexerContext context, string value);
    }

    public class LexerGroupProcessor : GroupProcessor
    {
        public Lexer Lexer { get; }

        public LexerGroupProcessor(Lexer lexer)
        {
            Lexer = lexer;
        }


        public override IEnumerable<Token> GetTokens(RegexLexerContext context, string value)
        {
            var tokens = Lexer.GetTokens(value);

            context.Position += value.Length;

            return tokens;
        }
    }

    public class TokenGroupProcessor : GroupProcessor
    {
        public TokenType Type { get; }

        public TokenGroupProcessor(TokenType type)
        {
            Type = type;
        }

        public override IEnumerable<Token> GetTokens(RegexLexerContext context, string value)
        {
            yield return new Token(context.Position, Type, value);
            context.Position += value.Length;
        }
    }
}