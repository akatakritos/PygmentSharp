using System;
using System.Collections.Generic;
using System.Text;

namespace PygmentSharp.Core.Tokens
{
    public class FunctionTokenType : TokenType
    {
        internal FunctionTokenType(TokenType parent) : base(parent, "Function")
        {
            Magic = CreateChild("Magic");
        }

        /// <summary>
        /// A token type for a magic function
        /// </summary>
        public TokenType Magic { get; }
    }
}
