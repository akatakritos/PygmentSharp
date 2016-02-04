using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PygmentSharp.Core.Lexers
{
    public static class CSharpLexerLevel
    {
        public static readonly string None = @"@?[_a-zA-Z]\w*";

        public static readonly string Basic = ("@?[_" + RegexUtil.Combine("Lu", "Ll", "Lt", "Lm", "Nl") + "]" +
                                        "[" + RegexUtil.Combine("Lu", "Ll", "Lt", "Lm", "Nl", "Nd", "Pc",
                                            "Cf", "Mn", "Mc") + "]*");

        public static readonly string Full = ("@?(?:_|[^" +
                                       RegexUtil.AllExcept("Lu", "Ll", "Lt", "Lm", "Lo", "Nl") + "])"
                                       + "[^" + RegexUtil.AllExcept("Lu", "Ll", "Lt", "Lm", "Lo", "Nl",
                                           "Nd", "Pc", "Cf", "Mn", "Mc") + "]*");
    }



    [Lexer("C#", AlternateNames = "csharp,c#,c-sharp,c sharp,c #")]
    [LexerFileExtension("*.cs")]
    public class CSharpLexer : RegexLexer
    {
        public override string Name => "C# Lexer";

        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();
            var cs_ident = CSharpLexerLevel.Full;

            rules["root"] = new StateRule[]
            {
                StateRule.ByGroups(@"^([ \t]*(?:" + cs_ident + @"(?:\[\])?\s+)+?)" +  // return type
                                 @"(" + cs_ident +   @")" +                            // method name
                                 @"(\s*)(\()",                                         // signature start
                    new LexerGroupProcessor(this),
                    new TokenGroupProcessor(TokenTypes.Name.Function),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),

                StateRule.Create(@"^\s*\[.*?\]", TokenTypes.Name.Attribute),
                StateRule.Create(@"[^\S\n]+", TokenTypes.Text),
                StateRule.Create(@"\\\n", TokenTypes.Text), //line continuation
                StateRule.Create(@"//.*?\n", TokenTypes.Comment.Single),
                StateRule.Create(@"/[*].*?[*]/", TokenTypes.Comment.Multiline),
                StateRule.Create(@"\n", TokenTypes.Text),
                StateRule.Create(@"[~!%^&*()+=|\[\]:;,.<>/?-]", TokenTypes.Punctuation),
                StateRule.Create(@"[{}]", TokenTypes.Punctuation),
                StateRule.Create(@"@""(""""|[^""])*""", TokenTypes.String),
                StateRule.Create(@"""(\\\\|\\""|[^""\n])*[""\n]", TokenTypes.String),
                StateRule.Create(@"'\\.'|'[^\\]'", TokenTypes.String.Char),
                StateRule.Create(@"[0-9](\.[0-9]*)?([eE][+-][0-9]+)?" +
                                 @"[flFLdD]?|0[xX][0-9a-fA-F]+[Ll]?", TokenTypes.Number),
                StateRule.Create(@"#[ \t]*(if|endif|else|elif|define|undef|" +
                                 @"line|error|warning|region|endregion|pragma)\b.*?\n", TokenTypes.Comment.Preproc),
                StateRule.ByGroups(@"'\b(extern)(\s+)(alias)\b",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Keyword)),
                StateRule.Create(@"(abstract|as|async|await|base|break|case|catch|" +
                                @"checked|const|continue|default|delegate|" +
                                @"do|else|enum|event|explicit|extern|false|finally|" +
                                @"fixed|for|foreach|goto|if|implicit|in|interface|" +
                                @"internal|is|lock|new|null|operator|" +
                                @"out|override|params|private|protected|public|readonly|" +
                                @"ref|return|sealed|sizeof|stackalloc|static|" +
                                @"switch|this|throw|true|try|typeof|" +
                                @"unchecked|unsafe|virtual|void|while|" +
                                @"get|set|new|partial|yield|add|remove|value|alias|ascending|" +
                                @"descending|from|group|into|orderby|select|where|" +
                                @"join|equals)\b", TokenTypes.Keyword),
                StateRule.ByGroups(@"(global)(::)",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),
                StateRule.Create(@"(bool|byte|char|decimal|double|dynamic|float|int|long|object|" +
                                 @"sbyte|short|string|uint|ulong|ushort|var)\b\??", TokenTypes.Keyword.Type),
                StateRule.ByGroups(@"(class|struct)(\s+)", "class",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text)),
                StateRule.ByGroups(@"(namespace|using)(\s+)", "namespace",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text)),
                StateRule.Create(cs_ident, TokenTypes.Name)
            };

            rules["class"] = new StateRule[]
            {
                StateRule.Create(cs_ident, TokenTypes.Name.Class, "#pop"),
                StateRule.Default("#pop")
            };

            rules["namespace"] = new[]
            {
                StateRule.Create(@"(?=\()", TokenTypes.Text, "#pop"), // using resource
                StateRule.Create(@"(" + cs_ident + @"|\.)+", TokenTypes.Name.Namespace, "#pop")
            };

            return rules;
        }
    }
}
