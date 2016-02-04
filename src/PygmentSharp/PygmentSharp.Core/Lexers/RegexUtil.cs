using System.Linq;

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
    }
}