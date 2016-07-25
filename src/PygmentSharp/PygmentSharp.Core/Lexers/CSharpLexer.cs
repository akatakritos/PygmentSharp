using System;
using System.Collections.Generic;

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
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();
            var cs_ident = CSharpLexerLevel.Full;
            var builder = new StateRuleBuilder();

            rules["root"] = new StateRule[]
            {
                builder.ByGroups(@"^([ \t]*(?:" + cs_ident + @"(?:\[\])?\s+)+?)" +  // return type
                                 @"(" + cs_ident +   @")" +                            // method name
                                 @"(\s*)(\()",                                         // signature start
                    new LexerGroupProcessor(this),
                    new TokenGroupProcessor(TokenTypes.Name.Function),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),

                builder.Create(@"^\s*\[.*?\]", TokenTypes.Name.Attribute),
                builder.Create(@"[^\S\n]+", TokenTypes.Text),
                builder.Create(@"\\\n", TokenTypes.Text), //line continuation
                builder.Create(@"//.*?\n", TokenTypes.Comment.Single),
                builder.Create(@"/[*].*?[*]/", TokenTypes.Comment.Multiline),
                builder.Create(@"\n", TokenTypes.Text),
                builder.Create(@"[~!%^&*()+=|\[\]:;,.<>/?-]", TokenTypes.Punctuation),
                builder.Create(@"[{}]", TokenTypes.Punctuation),
                builder.Create(@"@""(""""|[^""])*""", TokenTypes.String),
                builder.Create(@"""(\\\\|\\""|[^""\n])*[""\n]", TokenTypes.String),
                builder.Create(@"'\\.'|'[^\\]'", TokenTypes.String.Char),
                builder.Create(@"[0-9](\.[0-9]*)?([eE][+-][0-9]+)?" +
                                 @"[flFLdD]?|0[xX][0-9a-fA-F]+[Ll]?", TokenTypes.Number),
                builder.Create(@"#[ \t]*(if|endif|else|elif|define|undef|" +
                                 @"line|error|warning|region|endregion|pragma)\b.*?\n", TokenTypes.Comment.Preproc),
                builder.ByGroups(@"'\b(extern)(\s+)(alias)\b",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Keyword)),
                builder.Create(@"(abstract|as|async|await|base|break|case|catch|" +
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
                builder.ByGroups(@"(global)(::)",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),
                builder.Create(@"(bool|byte|char|decimal|double|dynamic|float|int|long|object|" +
                                 @"sbyte|short|string|uint|ulong|ushort|var)\b\??", TokenTypes.Keyword.Type),
                builder.ByGroups(@"(class|struct)(\s+)", "class",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text)),
                builder.ByGroups(@"(namespace|using)(\s+)", "namespace",
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Text)),
                builder.Create(cs_ident, TokenTypes.Name)
            };

            rules["class"] = new []
            {
                builder.Create(cs_ident, TokenTypes.Name.Class, "#pop"),
                builder.Default("#pop")
            };

            rules["namespace"] = new[]
            {
                builder.Create(@"(?=\()", TokenTypes.Text, "#pop"), // using resource
                builder.Create(@"(" + cs_ident + @"|\.)+", TokenTypes.Name.Namespace, "#pop")
            };

            return rules;
        }
    }
}
