using System;
using System.Collections.Generic;
using System.Linq;

using PygmentSharp.Core.Tokens;

namespace PygmentSharp.Core.Lexers
{
    /// <summary>
    /// Lexes text as a single big ol' <see cref="Token"/>
    /// </summary>
    [Lexer("Plain", AlternateNames = "Text,Plain Text")]
    [LexerFileExtension("*.txt")]
    public class PlainLexer : Lexer
    {
        protected override IEnumerable<Token> GetTokensUnprocessed(string text)
        {
            yield return new Token(0, TokenTypes.Text, text);
        }

    }
}