using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Lexers
{
    [Lexer("XML", AlternateNames = "xml")]
    [LexerFileExtension("*.xml")]
    [LexerFileExtension("*.xsl")]
    [LexerFileExtension("*.rss")]
    [LexerFileExtension("*.xslt")]
    [LexerFileExtension("*.xsd")]
    [LexerFileExtension("*.wsdl")]
    [LexerFileExtension("*.wsf")]
    public class XmlLexer : RegexLexer
    {
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();
            var builder = new StateRuleBuilder();

            rules["root"] = new []
            {
                builder.Create(@"[^<&]+", TokenTypes.Text),
                builder.Create(@"&\S*?;", TokenTypes.Name.Entity),
                builder.Create(@"\<\!\[CDATA\[.*?\]\]\>", TokenTypes.Comment.Preproc),
                builder.Create(@"<!--", TokenTypes.Comment, "comment"),
                builder.Create(@"<\?.*?\?>", TokenTypes.Comment.Preproc),
                builder.Create(@"<![^>]*>", TokenTypes.Comment.Preproc),
                builder.Create(@"<\s*[\w:.-]+", TokenTypes.Name.Tag, "tag"),
                builder.Create(@"<\s*/\s*[\w:.-]+\s*>'", TokenTypes.Name.Tag)
            };

            rules["comment"] = new[]
            {
                builder.Create(@"[^-]+", TokenTypes.Text),
                builder.Create(@"-->", TokenTypes.Comment, "#pop"),
                builder.Create(@"-", TokenTypes.Comment)
            };

            rules["tag"] = new[]
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.Create(@"[\w.:-]+\s*=", TokenTypes.Name.Attribute, "attr"),
                builder.Create(@"/?\s*>", TokenTypes.Name.Tag, "#pop")
            };

            rules["attr"] = new[]
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.Create(@""".*?""", TokenTypes.String, "#pop"),
                builder.Create(@".*?'", TokenTypes.String, "#pop"),
                builder.Create(@"[^\s>]+", TokenTypes.String, "#pop")
            };

            return rules;
        }
    }
}
