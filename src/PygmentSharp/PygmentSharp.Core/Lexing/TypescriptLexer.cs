using PygmentSharp.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PygmentSharp.Core.Lexing
{
    [Lexer("Typescript", AlternateNames = "typescript,ts")]
    [LexerFileExtension("*.ts")]
    [LexerFileExtension("*.tsx")]
    public class TypescriptLexer : RegexLexer
    {
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();

            var builder = new StateRuleBuilder();
            builder.DefaultRegexOptions = RegexOptions.Multiline;

            rules["commentsandwhitespace"] = builder.NewRuleSet()
                .Add(@"\s+", TokenTypes.Text)
                .Add(@"<!--", TokenTypes.Comment)
                .Add(@"//.*?\n", TokenTypes.Comment.Single)
                .Add(@"/\*.*?\*/", TokenTypes.Comment.Multiline)
                .Build();

            rules["slashstartsregex"] = builder.NewRuleSet()
                .Include(rules["commentsandwhitespace"])
                .Add(@"/(\\.|[^[/\\\n]|\[(\\.|[^\]\\\n])*])+/" + @"([gim]+\b|\B)", TokenTypes.String.Regex, "#pop")
                .Add(@"(?=/)", TokenTypes.Text, "#pop", "badregex")
                .Default("#pop")
                .Build();

            rules["badregex"] = builder.NewRuleSet()
                .Add(@"\n", TokenTypes.Text, "#pop")
                .Build();

            rules["root"] = builder.NewRuleSet()
                 .Add(@"^(?=\s|/|<!--)", TokenTypes.Text, "slashstartsregex")
                .Include(rules["commentsandwhitespace"])
                .Add(@"\+\+|--|~|&&|\?|:|\|\||\\(?=\n)|" + @"(<<|>>>?|==?|!=?|[-<>+*%&|^/])=?", TokenTypes.Operator, "slashstartsregex")
                .Add(@"[{(\[;,]", TokenTypes.Punctuation, "slashstartsregex")
                .Add(@"[})\].]", TokenTypes.Punctuation)
                .Add(@"(for|in|while|do|break|return|continue|switch|case|default|if|else|throw|try|catch|finally|new|delete|typeof|instanceof|void|of|this)\b", TokenTypes.Keyword, "slashstartsregex")
                .Add(@"(var|let|with|function)\b", TokenTypes.Keyword.Declaration, "slashstartsregex")
                .Add(@"(abstract|boolean|byte|char|class|const|debugger|double|enum|export|" +
                     @"extends|final|float|goto|implements|import|int|interface|long|native|" +
                     @"package|private|protected|public|short|static|super|synchronized|throws|" +
                     @"transient|volatile)\b", TokenTypes.Keyword.Reserved)
                .Add(@"(true|false|null|NaN|Infinity|undefined)\b", TokenTypes.Keyword.Constant)
                .Add(@"(Array|Boolean|Date|Error|Function|Math|netscape|" +
                     @"Number|Object|Packages|RegExp|String|sun|decodeURI|" +
                     @"decodeURIComponent|encodeURI|encodeURIComponent|" +
                     @"Error|eval|isFinite|isNaN|parseFloat|parseInt|document|this|" +
                     @"window)\b", TokenTypes.Name.Builtin)
                // Match stuff like: module name {...}
                .ByGroups(@"\b(module)(\s*)(\s*[\w?.$][\w?.$]*)(\s*)", "slashstartsregex",
                     TokenTypes.Keyword.Reserved, TokenTypes.Text, TokenTypes.Name.Other, TokenTypes.Text)
                // Match variable type keywords
                .Add(@"\b(string|bool|number)\b", TokenTypes.Keyword.Type)
                // Match stuff like: constructor
                .Add(@"\b(constructor|declare|interface|as|AS)\b", TokenTypes.Keyword.Reserved)
                // Match stuff like: super(argument, list)
                .ByGroups(@"(super)(\s*)(\([\w,?.$\s]+\s*\))", "slashstartsregex",
                     TokenTypes.Keyword.Reserved, TokenTypes.Text)
                // Match stuff like: function() {...}
                .Add(@"([a-zA-Z_?.$][\w?.$]*)\(\) \{", TokenTypes.Name.Other, "slashstartsregex")
                // Match stuff like: (function: return type)
                .ByGroups(@"([\w?.$][\w?.$]*)(\s*:\s*)([\w?.$][\w?.$]*)",
                    TokenTypes.Name.Other, TokenTypes.Text, TokenTypes.Keyword.Type)
                .Add(@"[$a-zA-Z_]\w*", TokenTypes.Name.Other)
                .Add(@"[0-9][0-9]*\.[0-9]+([eE][0-9]+)?[fd]?", TokenTypes.Number.Float)
                .Add(@"0x[0-9a-fA-F]+", TokenTypes.Number.Hex)
                .Add(@"[0-9]+", TokenTypes.Number.Integer)
                .Add(@"""(\\\\|\\""|[^""])*""", TokenTypes.String.Double)
                .Add(@"'(\\\\|\\'|[^'])*'", TokenTypes.String.Single)
                .Add(@"`", TokenTypes.String.Backtick, "interp")
                // Match stuff like: Decorators
                .Add(@"@\w+", TokenTypes.Keyword.Declaration)
                .Build();

            rules["interp"] = builder.NewRuleSet()
                .Add(@"`", TokenTypes.String.Backtick, "#pop")
                .Add(@"\\\\", TokenTypes.String.Backtick)
                .Add(@"\\`", TokenTypes.String.Backtick)
                .Add(@"\$\{", TokenTypes.String.Interpol, "interp-inside")
                .Add(@"\$", TokenTypes.String.Backtick)
                .Add(@"[^`\\$]+", TokenTypes.String.Backtick)
                .Build();

            rules["interp-inside"] = builder.NewRuleSet()
                .Add(@"\}", TokenTypes.String.Interpol, "#pop")
                .Include(rules["root"])
                .Build();

            return rules;
        }
    }
}
