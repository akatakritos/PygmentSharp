using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class KeywordTokenType : TokenType
    {
        public KeywordTokenType(TokenType parent) : base(parent, "Keyword")
        {
            Constant = CreateChild("Constant");
            Declaration = CreateChild("Declaration");
            Namespace = CreateChild("Namespace");
            Pseudo = CreateChild("Pseudo");
            Reserved = CreateChild("Reserved");
            Type = CreateChild("Type");
        }

        public TokenType Constant { get; }
        public TokenType Declaration { get; }
        public TokenType Namespace { get; }
        public TokenType Pseudo{ get; }
        public TokenType Reserved { get; }
        public TokenType Type{ get; }

    }
}