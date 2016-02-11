using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PygmentSharp.Core.Lexers
{
    public static class RegexUtil
    {
        private static string[] AllClasses =
        {
            "Cc",
            "Cf",
            "Cn",
            "Co",
            "Cs",
            "Ll",
            "Lm",
            "Lo",
            "Lt",
            "Lu",
            "Mc",
            "Me",
            "Mn",
            "Nd",
            "Nl",
            "No",
            "Pc",
            "Pd",
            "Pe",
            "Pf",
            "Pi",
            "Po",
            "Ps",
            "Sc",
            "Sk",
            "Sm",
            "So",
            "Zl",
            "Zp",
            "Zs"
        };

        public static string Combine(params string[] classes)
        {
            return string.Join("",
                classes.Select(c => $@"\p{{{c}}}"));
        }

        public static string AllExcept(params string[] classes)
        {
            var passed = AllClasses.Where(c => !classes.Contains(c)).ToArray();
            return Combine(passed);
        }

        private const string CharsetEscaper = @"[\^\\\-\]]";

        /// <summary>
        /// Creates a character set that could match any of the characters in the string
        /// </summary>
        /// <remarks>
        /// For example: <c>"abc"</c> would become <c>"[abc]"</c>
        /// </remarks>
        /// <param name="letters">THe letters to be included in the character set</param>
        /// <returns>A regex string for the character set</returns>
        public static string MakeCharset(string letters)
        {
            return "[" +
                   Regex.Replace(letters, CharsetEscaper, m => "\\" + m.Value) +
                   "]";
        }

        /// <summary>
        /// Creates a character set that could match any of the characters in the list
        /// </summary>
        /// <remarks>
        /// For example: <c>{ "a","b","c" }</c> would become <c>"[abc]"</c>
        /// </remarks>
        /// <param name="letters">THe letters to be included in the character set</param>
        /// <returns>A regex string for the character set</returns>
        public static string MakeCharset(params char[] letters)
        {
            return MakeCharset(new string(letters));
        }

        public static string Words(params string[] words)
        {
            throw new NotImplementedException();
        }
    }
}