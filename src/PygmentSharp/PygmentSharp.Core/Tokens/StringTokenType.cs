using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class StringTokenType : TokenType
    {
        public StringTokenType(TokenType parent) : base(parent, "String")
        {
            Backtick = CreateChild("Backtick");
            Char = CreateChild("Char");
            Doc = CreateChild("Doc");
            Double = CreateChild("Double");
            Escape = CreateChild("Escape");
            Heredoc = CreateChild("Heredoc");
            Interpol = CreateChild("Interpol");
            Other = CreateChild("Other");
            Regex = CreateChild("Regex");
            Single = CreateChild("Single");
            Symbol = CreateChild("Symbol");

        }

        public TokenType Backtick { get; }
        public TokenType Char { get; }
        public TokenType Doc { get; }
        public TokenType Double { get; }
        public TokenType Escape { get; }
        public TokenType Heredoc { get; }
        public TokenType Interpol { get; }
        public TokenType Other { get; }
        public TokenType Regex { get; }
        public TokenType Single { get; }
        public TokenType Symbol { get; }
    }
}