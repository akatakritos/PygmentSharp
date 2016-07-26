using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class OperatorTokenType : TokenType
    {
        public OperatorTokenType(TokenType parent) : base(parent, "Operator")
        {
            Word = CreateChild("Word");
        }

        public TokenType Word { get; }

    }
}