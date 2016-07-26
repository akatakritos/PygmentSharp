using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class NumberTokenType : TokenType
    {
        public NumberTokenType(TokenType parent) : base(parent, "Number")
        {

            Bin = CreateChild("Bin");
            Float = CreateChild("Float");
            Hex = CreateChild("Hex");
            Integer = AddChild(new IntegerTokenType(this));
            Oct = CreateChild("Oct");
        }

        public TokenType Bin { get; }
        public TokenType Float { get; }
        public TokenType Hex { get; }
        public IntegerTokenType Integer { get; }
        public TokenType Oct { get; }
    }
}