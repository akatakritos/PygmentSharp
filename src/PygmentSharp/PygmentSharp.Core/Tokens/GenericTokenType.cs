using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class GenericTokenType : TokenType
    {
        public GenericTokenType(TokenType parent) : base(parent, "Generic")
        {
            Deleted = CreateChild("Deleted");
            Emph = CreateChild("Emph");
            Error = CreateChild("Error");
            Heading = CreateChild("Heading");
            Inserted = CreateChild("Inserted");
            Output = CreateChild("Output");
            Prompt = CreateChild(nameof(Prompt));
            Strong = CreateChild(nameof(Strong));
            Subheading = CreateChild(nameof(Subheading));
            Traceback = CreateChild(nameof(Traceback));

        }

        public TokenType Deleted { get; }
        public TokenType Emph { get; }
        public TokenType Error { get; }
        public TokenType Heading { get; }
        public TokenType Inserted { get; }
        public TokenType Output { get; }
        public TokenType Prompt { get; }
        public TokenType Strong { get; }
        public TokenType Subheading { get; }
        public TokenType Traceback { get; }
    }
}