using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public static class TokenTypes
    {
        public static readonly TokenType Token = new TokenType(null, "Token");
        public static readonly TokenType Text = Token.CreateChild("Text");
        public static readonly TokenType Whitespace = Text.CreateChild("Whitespace");
        public static readonly TokenType Escape = Token.CreateChild("Escape");
        public static readonly TokenType Error = Token.CreateChild("Error");
        public static readonly TokenType Other = Token.CreateChild("Other");
        public static readonly KeywordTokenType Keyword = Token.AddChild(new KeywordTokenType(Token));
        public static readonly NameTokenType Name = Token.AddChild(new NameTokenType(Token));
        public static readonly LiteralTokenType Literal = Token.AddChild(new LiteralTokenType(Token));
        public static readonly StringTokenType String = Literal.String; //alias
        public static readonly NumberTokenType Number = Literal.Number; //alias
        public static readonly TokenType Punctuation = Token.CreateChild("Punctuation");
        public static readonly OperatorTokenType Operator = Token.AddChild(new OperatorTokenType(Token));
        public static readonly CommentTokenType Comment = Token.AddChild(new CommentTokenType(Token));
        public static readonly GenericTokenType Generic = Token.AddChild(new GenericTokenType(Token));
    }
}