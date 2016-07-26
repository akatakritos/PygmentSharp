using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Tokens
{
    public class CommentTokenType : TokenType
    {
        public CommentTokenType(TokenType parent) : base(parent, "Comment")
        {
            Hashbang = CreateChild("Hashbang");
            Multiline = CreateChild("Multiline");
            Preproc = CreateChild("Preproc");
            PreprocFile = CreateChild("PreprocFile");
            Single = CreateChild("Single");
            Special = CreateChild("Special");
        }

        public TokenType Hashbang { get; }
        public TokenType Multiline { get; }
        public TokenType Preproc { get; }
        public TokenType PreprocFile { get; }
        public TokenType Single { get; }
        public TokenType Special { get; }
    }
}