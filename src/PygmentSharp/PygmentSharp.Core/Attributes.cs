using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PygmentSharp.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LexerAttribute : Attribute
    {
        public string Name { get; }

        public string AlternateNames { get; set; }

        public LexerAttribute(string name)
        {
            Name = name;
        }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LexerFileExtensionAttribute : Attribute
    {
        public string Pattern { get; }

        public LexerFileExtensionAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
