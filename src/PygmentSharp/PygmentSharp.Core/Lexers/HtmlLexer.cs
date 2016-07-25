using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PygmentSharp.Core.Lexers
{
    public class CssLexer
    {
    }

    [Lexer("HTML", AlternateNames = "html")]
    [LexerFileExtension("*.html")]
    [LexerFileExtension("*.htm")]
    [LexerFileExtension("*.xhtml")]
    public class HtmlLexer : RegexLexer
    {
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var builder = new StateRuleBuilder();
            builder.DefaultRegexOptions = RegexOptions.IgnoreCase;

            var rules = new Dictionary<string, StateRule[]>();

            rules["root"] = new[]
            {
                builder.Create(@"[^<&]+", TokenTypes.Text),
                builder.Create(@"&\S*?;", TokenTypes.Name.Entity),
                builder.Create(@"\<\!\[CDATA\[.*?\]\]\>", TokenTypes.Comment.Preproc),
                builder.Create(@"<!--", TokenTypes.Comment, "comment"),
                builder.Create(@"<\?.*?\?>", TokenTypes.Comment.Preproc),
                builder.Create(@"<![^>]*>", TokenTypes.Comment.Preproc),
                builder.ByGroups(@"(<)(\s*)(script)(\s*)", new[] { "script-content", "tag" },
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag),
                    new TokenGroupProcessor(TokenTypes.Text)),
                builder.ByGroups(@"(<)(\s*)(style)(\s*)", new[] { "style-content", "tag" },
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag),
                    new TokenGroupProcessor(TokenTypes.Text)),
                builder.ByGroups(@"(<)(\s*)([\w:.-]+)", "tag",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag)),
                builder.ByGroups(@"(<)(\s*)(/)(\s*)([\w:.-]+)(\s*)(>)",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),
            };

            rules["comment"] = new[]
            {
                builder.Create(@"[^-]+", TokenTypes.Comment, "#pop"),
                builder.Create(@"-->", TokenTypes.Comment),
                builder.Create(@"-", TokenTypes.Comment)
            };

            rules["tag"] = new[]
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.ByGroups(@"([\w:-]+\s*)(=)(\s*)", "attr",
                    new TokenGroupProcessor(TokenTypes.Name.Attribute),
                    new TokenGroupProcessor(TokenTypes.Operator),
                    new TokenGroupProcessor(TokenTypes.Text)),
                builder.Create(@"[\w:-]+", TokenTypes.Name.Attribute),
                builder.ByGroups(@"(/?)(\s*)(>)", "#pop",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation))
            };

            rules["script-content"] = new[]
            {
                builder.ByGroups(@"(<)(\s*)(/)(\s*)(script)(\s*)(>)", "#pop",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),
                builder.Using<JavascriptLexer>(@".+?(?=<\s*/\s*script\s*>)")
            };

            rules["style-content"] = new[]
            {
                builder.ByGroups(@"(<)(\s*)(/)(\s*)(style)(\s*)(>)", "#pop",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Tag),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation)),
                builder.Using<CssLexer>(@".+?(?=<\s*/\s*style\s*>)")
            };

            rules["attr"] = new[]
            {
                builder.Create(@""".*?""", TokenTypes.String, "#pop"),
                builder.Create(@"'.*?'", TokenTypes.String, "#pop"),
                builder.Create(@"[^\s>]+", TokenTypes.String, "#pop")
            };

            return rules;
        }
    }
}
