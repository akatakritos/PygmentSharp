using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PygmentSharp.Core.Formatters
{
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
