using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class LiteralTokenType : TokenType
    {
        public LiteralTokenType(TokenType parent) : base(parent, "Literal")
        {
            Date = CreateChild("Date");
            String = AddChild(new StringTokenType(this));
            Number = AddChild(new NumberTokenType(this));
        }

        public TokenType Date { get; }
        public StringTokenType String { get; }
        public NumberTokenType Number { get; }
    }
}