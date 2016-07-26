using System;
using System.Collections.Generic;
using System.Linq;

using PygmentSharp.Core.Extensions;

namespace PygmentSharp.Core
{
    public class LexerLocator : AttributeLocator<LexerAttribute>
    {
        private IEnumerable<Type> Lexers => Types;

        public Lexer FindByName(string name)
        {
            var type = Lexers.FirstOrDefault(l => HasLexerName(l, name));

            return type?.InstantiateAs<Lexer>();
        }

        public Lexer FindByFilename(string filename)
        {
            var type = Lexers.FirstOrDefault(l => HasMatchingWildcard(l, filename));
            return type?.InstantiateAs<Lexer>();
        }

        private static bool HasMatchingWildcard(Type lexer, string file)
        {
            return lexer.HasAttribute<LexerFileExtensionAttribute>(a => file.MatchesFileWildcard(a.Pattern));
        }

        private static bool HasLexerName(Type l, string name)
        {
           return l.HasAttribute<LexerAttribute>(a => a.Name == name || a.AlternateNames.CsvContains(name));
        }
    }
}
