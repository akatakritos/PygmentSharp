using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class NameVariableTokenType : TokenType
    {
        public NameVariableTokenType(TokenType parent) : base(parent, "Variable")
        {
            Class = CreateChild("Class");
            Global = CreateChild("Global");
            Instance = CreateChild("Instance");
        }

        public TokenType Class { get; }
        public TokenType Global { get; }
        public TokenType Instance { get; }
    }
}