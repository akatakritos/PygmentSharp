using System;
using System.Collections.Generic;

namespace PygmentSharp.Core
{
    /// <summary>
    /// Base class for all Lexers
    /// </summary>
    /// <remarks>
    /// Lexers convert the input source to a sequence of <see cref="Token"/>s.
    /// The token types determine how they will
    /// be highlighted by supported output <see cref="Formatter"/>s.
    /// </remarks>
    public abstract class Lexer
    {
        public IEnumerable<Token> GetTokens(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            return GetTokensUnprocessed(text);
        }

        /// <summary>
        /// When overridden in a child class, gets all the <see cref="Token"/>s for the given string
        /// </summary>
        /// <param name="text">The string to tokenize</param>
        /// <returns>A sequence of <see cref="Token"/> structs</returns>
        protected abstract IEnumerable<Token> GetTokensUnprocessed(string text);
    }
}
