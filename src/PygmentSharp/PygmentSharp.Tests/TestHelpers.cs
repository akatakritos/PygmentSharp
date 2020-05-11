using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PygmentSharp.Core.Tokens;

namespace PygmentSharp.UnitTests
{
    internal static class TestHelpers
    {
        /// <summary>
        /// Dumps a token sequence as a list of Token constructors so you can copy paste into your test
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static string DumpForCode(this IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.AppendLine($"new Token({token.Index}, TokenTypes.{token.Type.ToString().Replace("Token.","")}, \"{Escape(token.Value)}\"),");
            }

            return sb.ToString();
        }

        private static string Escape(string value)
        {
            return value
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\"", "\\\"");
        }
    }
}