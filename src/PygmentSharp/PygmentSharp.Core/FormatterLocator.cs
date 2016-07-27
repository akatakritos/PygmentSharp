using System;
using System.Collections.Generic;
using System.Linq;

using PygmentSharp.Core.Extensions;

namespace PygmentSharp.Core
{
    internal class FormatterLocator : AttributeLocator<FormatterAttribute>
    {
        private IEnumerable<Type> Formatters => Types;

        public Formatter FindByName(string name)
        {
            var type = Formatters.FirstOrDefault(l => HasFormatter(l, name));
            return type?.InstantiateAs<Formatter>();
        }

        public Formatter FindByFilename(string filename)
        {
            var type = Formatters.FirstOrDefault(l => HasMatchingWildcard(l, filename));
            return type?.InstantiateAs<Formatter>();
        }

        private static bool HasMatchingWildcard(Type lexer, string file)
        {
             return lexer.HasAttribute<FormatterFileExtensionAttribute>(a =>
                file.MatchesFileWildcard(a.Pattern));
        }

        private static bool HasFormatter(Type l, string name)
        {
            return l.HasAttribute<FormatterAttribute>(a =>
                a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                a.AlternateNames.CsvContains(name, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}