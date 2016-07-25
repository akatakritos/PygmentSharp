using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PygmentSharp.Core.Lexers
{
    [Lexer("Javascript", AlternateNames = "javascript,JavaScript,ECMAScript")]
    [LexerFileExtension("*.js")]
    public class JavascriptLexer : RegexLexer
    {
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();

            string JS_IDENT_START = "(?:[$_" + RegexUtil.Combine("Lu", "Ll", "Lt", "Lm", "Lo", "Nl") + "]|\\\\u[a-fA-F0-9]{4})";
            string JS_IDENT_PART = "(?:[$" + RegexUtil.Combine("Lu", "Ll", "Lt", "Lm", "Lo", "Nl", "Mn", "Mc", "Nd", "Pc") + "\u200c\u200d]|\\\\u[a-fA-F0-9]{4})";
            string JS_IDENT = JS_IDENT_START + "(?:" + JS_IDENT_PART + ")*";

            var builder = new StateRuleBuilder();
            builder.DefaultRegexOptions = RegexOptions.Multiline;

            rules["commentsandwhitespace"] = new []
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.Create(@"<!--", TokenTypes.Comment),
                builder.Create(@"//.*?\n", TokenTypes.Comment.Single),
                builder.Create(@"/\*.*?\*/", TokenTypes.Comment.Multiline)
            };

            rules["slashstartsregex"] = builder.Include(rules["commentsandwhitespace"],
                builder.Create(@"/(\\.|[^[/\\\n]|\[(\\.|[^\]\\\n])*])+/" + @"([gim]+\b|\B)", TokenTypes.String.Regex, "#pop"),
                builder.Create(@"(?=/)", TokenTypes.Text, "#pop", "badregex"),
                builder.Default("#pop")
            );

            rules["badregex"] = new []
            {
                builder.Create(@"\n", TokenTypes.Text, "#pop")
            };

            rules["root"] = new[]
            {
                builder.Create(@"\A#! ?/.*?\n", TokenTypes.Comment.Hashbang),
                builder.Create(@"^(?=\s|/|<!--)", TokenTypes.Text, "slashstartsregex"),
            }.Concat(
                builder.Include(rules["commentsandwhitespace"],
                    builder.Create(@"\+\+|--|~|&&|\?|:|\|\||\\(?=\n)|(<<|>>>?|=>|==?|!=?|[-<>+*%&|^/])=?", TokenTypes.Operator, "slashstartsregex"),
                    builder.Create(@"\.\.\.", TokenTypes.Punctuation),
                    builder.Create(@"[{(\[;,]", TokenTypes.Punctuation, "slashstartsregex"),
                    builder.Create(@"[})\].]", TokenTypes.Punctuation),
                    builder.Create(@"(for|in|while|do|break|return|continue|switch|case|default|if|else|throw|try|catch|finally|new|delete|typeof|instanceof|void|yield|this|of)\b", TokenTypes.Keyword, "slashstartsregex"),
                    builder.Create(@"(var|let|with|function)\b", TokenTypes.Keyword.Declaration, "slashstartsregex"),
                    builder.Create(@"(abstract|boolean|byte|char|class|const|debugger|double|enum|export|extends|final|float|goto|implements|import|int|interface|long|native|package|private|protected|public|short|static|super|synchronized|throws|transient|volatile)\b", TokenTypes.Keyword.Reserved),
                    builder.Create(@"(true|false|null|NaN|Infinity|undefined)\b", TokenTypes.Keyword.Constant),
                    builder.Create(@"(Array|Boolean|Date|Error|Function|Math|netscape|Number|Object|Packages|RegExp|String|Promise|Proxy|sun|decodeURI|decodeURIComponent|encodeURI|encodeURIComponent|Error|eval|isFinite|isNaN|isSafeInteger|parseFloat|parseInt|document|this|window)\b", TokenTypes.Name.Builtin),
                    builder.Create(JS_IDENT, TokenTypes.Name.Other),
                    builder.Create(@"[0-9][0-9]*\.[0-9]+([eE][0-9]+)?[fd]?", TokenTypes.Number.Float),
                    builder.Create(@"0b[01]+", TokenTypes.Number.Bin),
                    builder.Create(@"0o[0-7]+", TokenTypes.Number.Oct),
                    builder.Create(@"0x[0-9a-fA-F]+", TokenTypes.Number.Hex),
                    builder.Create(@"[0-9]+'", TokenTypes.Number.Integer),
                    builder.Create(@"""(\\\\|\\""|[^""])*""", TokenTypes.String.Double),
                    builder.Create(@"'(\\\\|\\'|[^'])*'", TokenTypes.String.Single),
                    builder.Create(@"`", TokenTypes.String.Backtick, "interp")
            )).ToArray();

            rules["interp"] = new[]
            {
                builder.Create(@"`", TokenTypes.String.Backtick, "#pop"),
                builder.Create(@"\\\\", TokenTypes.String.Backtick),
                builder.Create(@"\\`", TokenTypes.String.Backtick),
                builder.Create(@"\${", TokenTypes.String.Interpol, "interp-inside"),
                builder.Create(@"\$", TokenTypes.String.Backtick),
                builder.Create(@"[^`\\$]+'", TokenTypes.String.Backtick)
            };

            rules["interp-inside"] = new[]
            {
                builder.Create(@"}", TokenTypes.String.Interpol, "#pop")
            }.Concat(rules["root"]).ToArray();

            return rules;
        }
    }
}