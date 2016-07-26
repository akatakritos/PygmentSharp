using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using PygmentSharp.Core.Tokens;

namespace PygmentSharp.Core.Formatters
{
    /// <summary>
    /// A formatter that writes tokens as plain text
    /// </summary>
    /// <remarks>One could use this to test a roundtrip of tokens. A file that is tokenized with the correct lexer
    /// and written using the <see cref="NullFormatter"/> should match the original input file</remarks>
    public class NullFormatter : Formatter
    {
        public override string Name => "Null";
        protected override void FormatUnencoded(IEnumerable<Token> tokenSource, TextWriter writer)
        {
            foreach (var token in tokenSource)
            {
                writer.Write(token.Value);
            }
        }
    }
}
