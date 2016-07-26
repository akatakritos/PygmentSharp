using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class IntegerTokenType : TokenType
    {
        public IntegerTokenType(TokenType parent) : base(parent, "Integer")
        {
            Long = CreateChild("Long");
        }

        public TokenType Long { get; }
    }
}