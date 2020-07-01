using PygmentSharp.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static PygmentSharp.Core.Lexing.RegexUtil;

namespace PygmentSharp.Core.Lexing
{
    [Lexer("PHP", AlternateNames = "php,php3,php4,php5")]
    [LexerFileExtension("*.php")]
    [LexerFileExtension("*.php3")]
    [LexerFileExtension("*.php4")]
    [LexerFileExtension("*.php5")]
    [LexerFileExtension("*.inc")]
    public class PhpLexer: RegexLexer
    {
        public PhpLexer()
        {

        }

        private PhpLexer(bool startInLine)
        {
            _startInLine = startInLine;
        }


        private const string identifierChar = @"[\\\w]|[^\x00-\x7f]";
        private const string indentifierBegin = @"(?:[\\_a-z]|[^\x00-\x7f])";
        private const string identifierEnd = @"(?:" + identifierChar + @")*";
        private const string identifierInner = indentifierBegin + identifierEnd;
        private readonly bool _startInLine;

        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();
            var builder = new StateRuleBuilder();
            builder.DefaultRegexOptions = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Singleline;

            rules["root"] = builder.NewRuleSet()
                .Add(@"<\?(php)?", TokenTypes.Comment.Preproc, "php")
                .Add(@"[^<]+", TokenTypes.Other)
                .Add(@"<", TokenTypes.Other)
                .Build();

            rules["magicfuncs"] = builder.NewRuleSet()
                .Add(Words(new[] {
                     "__construct", "__destruct", "__call", "__callStatic", "__get", "__set",
                    "__isset", "__unset", "__sleep", "__wakeup", "__toString", "__invoke",
                    "__set_state", "__clone", "__debugInfo"
                }, suffix: @"\b"), TokenTypes.Name.Function.Magic)
                .Build();

            rules["magicconstants"] = builder.NewRuleSet()
                .Add(Words(new[]
                {
                    "__LINE__", "__FILE__", "__DIR__", "__FUNCTION__", "__CLASS__",
                    "__TRAIT__", "__METHOD__", "__NAMESPACE__"

                }, suffix: @"\b"), TokenTypes.Name.Constant)
                .Build();

            rules["php"] = builder.NewRuleSet()
                .Add(@"\?>", TokenTypes.Comment.Preproc, "#pop")
                .ByGroups(@"(<<<)([\""]?)(" + identifierInner + @")(\2\n.*?\n\s*)(\3)(;?)(\n)",
                    TokenTypes.String, TokenTypes.String, TokenTypes.String.Delimiter, TokenTypes.String, TokenTypes.String.Delimiter,
                    TokenTypes.Punctuation, TokenTypes.Text)
                .Add(@"\s+", TokenTypes.Text)
                .Add(@"#.*?\n", TokenTypes.Comment.Single)
                .Add(@"//.*?\n", TokenTypes.Comment.Single)
                // put the empty comment here, it is otherwise seen as
                // the start of a docstring
                .Add(@"/\*\*/", TokenTypes.Comment.Multiline)
                .Add(@"/\*\*.*?\*/", TokenTypes.String.Doc)
                .Add(@"/\*.*?\*/", TokenTypes.Comment.Multiline)
                .ByGroups(@"(->|::)(\s*)(" + identifierInner+ ")",
                     TokenTypes.Operator, TokenTypes.Text, TokenTypes.Name.Attribute)
                .Add(@"[~!%^&*+=|:.<>/@-]+", TokenTypes.Operator)
                .Add(@"\?", TokenTypes.Operator) // don"t add to the charclass above!
                .Add(@"[\[\]{}();,]+", TokenTypes.Punctuation)
                .ByGroups(@"(class)(\s+)", "classname", TokenTypes.Keyword, TokenTypes.Text)
                .ByGroups(@"(function)(\s*)(?=\()", TokenTypes.Keyword, TokenTypes.Text)
                .ByGroups(@"(function)(\s+)(&?)(\s*)", "functionname",
                 TokenTypes.Keyword, TokenTypes.Text, TokenTypes.Operator, TokenTypes.Text)
                .ByGroups(@"(const)(\s+)(" + identifierInner+ ")",
                    TokenTypes.Keyword, TokenTypes.Text, TokenTypes.Name.Constant)
                .Add(@"(and|E_PARSE|old_function|E_ERROR|or|as|E_WARNING|parent|" +
                 @"eval|PHP_OS|break|exit|case|extends|PHP_VERSION|cfunction|" +
                 @"FALSE|print|for|require|continue|foreach|require_once|" +
                 @"declare|return|default|static|do|switch|die|stdClass|" +
                 @"echo|else|TRUE|elseif|var|empty|if|xor|enddeclare|include|" +
                 @"virtual|endfor|include_once|while|endforeach|global|" +
                 @"endif|list|endswitch|new|endwhile|not|" +
                 @"array|E_ALL|NULL|final|php_user_filter|interface|" +
                 @"implements|public|private|protected|abstract|clone|try|" +
                 @"catch|throw|this|use|namespace|trait|yield|" +
                 @"finally)\b", TokenTypes.Keyword)
                .Add(@"(true|false|null)\b", TokenTypes.Keyword.Constant)
                .Include(rules["magicconstants"])
                .Add(@"\$\{\$+" + identifierInner+ @"\}", TokenTypes.Name.Variable)
                .Add(@"\$+" + identifierInner, TokenTypes.Name.Variable)
                .Add(identifierInner, TokenTypes.Name.Other)
                .Add(@"(\d+\.\d*|\d*\.\d+)(e[+-]?[0-9]+)?", TokenTypes.Number.Float)
                .Add(@"\d+e[+-]?[0-9]+", TokenTypes.Number.Float)
                .Add(@"0[0-7]+", TokenTypes.Number.Oct)
                .Add(@"0x[a-f0-9]+", TokenTypes.Number.Hex)
                .Add(@"\d+", TokenTypes.Number.Integer)
                .Add(@"0b[01]+", TokenTypes.Number.Bin)
                .Add(@"'([^'\\]*(?:\\.[^'\\]*)*)'", TokenTypes.String.Single)
                .Add(@"`([^`\\]*(?:\\.[^`\\]*)*)`", TokenTypes.String.Backtick)
                .Add(@"""", TokenTypes.String.Double, "string")
                .Build();


            rules["classname"] = builder.NewRuleSet()
                .Add(identifierInner, TokenTypes.Name.Function, "#pop")
                .Build();

            rules["functionname"] = builder.NewRuleSet()
                .Include(rules["magicfuncs"])
                .Add(identifierInner, TokenTypes.Name.Function, "#pop")
                .Default("#pop")
                .Build();

            rules["string"] = builder.NewRuleSet()
                .Add(@"""", TokenTypes.String.Double, "#pop")
                .Add(@"[^{$""\\]+", TokenTypes.String.Double)
                .Add(@"\\([nrt""$\\]|[0-7]{1,3}|x[0-9a-f]{1,2})", TokenTypes.String.Escape)
                .Add(@"\$" + identifierInner + @"(\[\S+?\]|->" + identifierInner + ")?", TokenTypes.String.Interpol)
                .ByGroups(@"(\{\$\{)(.*?)(\}\})", new TokenGroupProcessor(TokenTypes.String.Interpol),
                                               new LexerGroupProcessor(new PhpLexer(startInLine: true)),
                                               new TokenGroupProcessor(TokenTypes.String.Interpol))
                .ByGroups(@"(\{)(\$.*?)(\})",
                    new TokenGroupProcessor(TokenTypes.String.Interpol),
                    new LexerGroupProcessor(new PhpLexer(startInLine: true)),
                    new TokenGroupProcessor(TokenTypes.String.Interpol))
                .ByGroups(@"(\$\{)(\S+)(\})",
                    TokenTypes.String.Interpol, TokenTypes.Name.Variable, TokenTypes.String.Interpol)
                .Add(@"[${\\]", TokenTypes.String.Double)
                .Build();

            return rules;
        }

        protected override IEnumerable<Token> GetTokensUnprocessed(string text)
        {
            var stateStack = new Stack<string>(50);
            stateStack.Push("root");

            // TODO: other options

            if (_startInLine)
            {
                stateStack.Push("php");
            }

            return base.GetTokensUnprocessed(text, stateStack);
        }
    }
}
