using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class NameTokenType : TokenType
    {
        public NameTokenType(TokenType parent) : base(parent, "Name")
        {
            Attribute = CreateChild("Attribute");
            Builtin = AddChild(new NameBuiltinTokenType(this));
            Class = CreateChild("Class");
            Constant = CreateChild("Constant");
            Decorator = CreateChild("Decorator");
            Entity = CreateChild("Entity");
            Exception = CreateChild("Exception");
            Function = CreateChild("Exception");
            Property = CreateChild("Property");
            Label = CreateChild("Label");
            Namespace = CreateChild("Namespace");
            Other = CreateChild("Other");
            Tag = CreateChild("Tag");
            Variable = new NameVariableTokenType(this);
        }

        public TokenType Attribute { get; }
        public NameBuiltinTokenType Builtin { get; }
        public TokenType Class { get; }
        public TokenType Constant { get; }
        public TokenType Decorator { get; }
        public TokenType Entity { get; }
        public TokenType Exception { get; }
        public TokenType Function { get; }
        public TokenType Property { get; }
        public TokenType Label { get; }
        public TokenType Namespace { get; }
        public TokenType Other { get; }
        public TokenType Tag { get; }
        public NameVariableTokenType Variable { get; }
    }
}