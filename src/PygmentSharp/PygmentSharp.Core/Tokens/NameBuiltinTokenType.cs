using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class NameBuiltinTokenType : TokenType
    {
        public NameBuiltinTokenType(TokenType parent) : base(parent, "Builtin")
        {
            Pseudo = CreateChild("Pseudo");
        }

        public TokenType Pseudo { get; }
    }
}