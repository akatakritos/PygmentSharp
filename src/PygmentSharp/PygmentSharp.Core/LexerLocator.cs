using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PygmentSharp.Core
{
    public class LexerLocator
    {
        private readonly Lazy<IEnumerable<Type>> _lexers = new Lazy<IEnumerable<Type>>(GetLexers);
        private IEnumerable<Type> Lexers => _lexers.Value;

        public Lexer FindByName(string name)
        {
            var type = Lexers.FirstOrDefault(l => HasLexerName(l, name));
            if (type == null)
                return null;

            return (Lexer)Activator.CreateInstance(type);
        }

        public Lexer FindByFilename(string filename)
        {
            var type = Lexers.FirstOrDefault(l => HasMatchingWildcard(l, filename));
            if (type == null)
                return null;

            return (Lexer) Activator.CreateInstance(type);
        }

        private bool HasMatchingWildcard(Type lexer, string file)
        {
            var attrs = lexer.GetCustomAttributes<LexerFileExtensionAttribute>();
            return attrs.Any(a => MatchesPattern(file, a.Pattern));
        }

        private static bool MatchesPattern(string file, string pattern)
        {
            var regex = new Regex(pattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", "."));
            return regex.IsMatch(file);
        }

        private static bool HasLexerName(Type l, string name)
        {
            var attr = l.GetCustomAttribute<LexerAttribute>();
            Debug.Assert(attr != null, "All elements of the Lexers list should have the Lexer attribute applied");

            return attr.Name == name ||
                   (attr.AlternateNames?
                       .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                       .Contains(name) ?? false);
        }

        private static IEnumerable<Type> GetLexers()
        {
            return GetTypesWithAttribute<LexerAttribute>().ToArray();
        }

        private static IEnumerable<Type> GetTypesWithAttribute<T>() where T:Attribute
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes().Where(t => t.GetCustomAttributes<T>(true).Any());
        }
    }
}
