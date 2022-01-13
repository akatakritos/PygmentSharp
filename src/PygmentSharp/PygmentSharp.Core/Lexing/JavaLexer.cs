using System;
using System.Collections.Generic;
using System.Linq;

using PygmentSharp.Core.Tokens;

namespace PygmentSharp.Core.Lexing
{

    /// <summary>
    /// A lexer for Java
    /// </summary>
    [Lexer("Java", AlternateNames = "java")]
    [LexerFileExtension("*.java")]
    public class JavaLexer : RegexLexer
    {
        /// <summary>
        /// Gets the state transition rules for the lexer. Each time a regex is matched,
        /// the internal state machine can be bumped to a new state which determines what
        /// regexes become valid again
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {

            System.Console.WriteLine("Using java lexer");
            /*
             * what to do about this line:
             * flags = re.MULTILINE | re.DOTALL | re.UNICODE
             */

            var rules = new Dictionary<string, StateRule[]>();
            var builder = new StateRuleBuilder();

            // SingleLine makes the . operator match \n's and MultiLine makes $ and ^ match at beginning and end of each line
            // without this, multi-line comments aren't matched correctly
            builder.DefaultRegexOptions = System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Multiline;

            rules["root"] = builder.NewRuleSet()
                .ByGroups(@"(^\s*)((?:(?:public|private|protected|static|strictfp)(?:\s+))*)(record)\b", "class",
                    new LexerGroupProcessor(this),
                    new TokenGroupProcessor(TokenTypes.Keyword.Declaration))
                .Add(@"[^\S\n]+", TokenTypes.Text)
                .Add(@"//.*?\n", TokenTypes.Comment.Single)
                .Add(@"/\*.*?\*/", TokenTypes.Comment.Multiline)
                .Add(@"(assert|break|case|catch|continue|default|do|else|finally|for|" +
                    @"if|goto|instanceof|new|return|switch|this|throw|try|while)\b", TokenTypes.Keyword)
                // method names
                .ByGroups(@"((?: (?:[^\W\d] |\$)[\w.\[\]$<>]*\s +)+?)" +  //return arguments
                    @"((?:[^\W\d]|\$)[\w$]*)" +                  // method name
                    @"(\s*)(\()",                              // signature start
                    new LexerGroupProcessor(this),
                    new TokenGroupProcessor(TokenTypes.Name.Function),
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Punctuation))
                .Add(@"@[^\W\d][\w.]*", TokenTypes.Name.Decorator)
                .Add(@"(abstract|const|enum|extends|final|implements|native|private|" +
                    @"'protected|public|sealed|static|strictfp|super|synchronized|throws|" +
                    @"transient|volatile|yield)\b", TokenTypes.Keyword.Declaration)
                .Add(@"(boolean|byte|char|double|float|int|long|short|void)\b", TokenTypes.Keyword.Type)
                .ByGroups(@"(package)(\s+)", "import",
                    new LexerGroupProcessor(this),
                    new TokenGroupProcessor(TokenTypes.Keyword.Namespace),
                    new TokenGroupProcessor(TokenTypes.Text))
                .Add(@"(true|false|null)\b", TokenTypes.Keyword.Constant)
                .Add(@"(class|interface)\b", TokenTypes.Keyword.Declaration, "class")
                .ByGroups(@"(var)(\s+)", "var",
                    new TokenGroupProcessor(TokenTypes.Keyword.Declaration),
                    new TokenGroupProcessor(TokenTypes.Text))
                .ByGroups(@"(import(?:\s+static)?)(\s+)", "import",
                    new TokenGroupProcessor(TokenTypes.Keyword.Namespace),
                    new TokenGroupProcessor(TokenTypes.Text))
                .Add(@"""", TokenTypes.String, "string")
                .Add(@"'\\.'|'[^\\]'|'\\u[0-9a-fA-F]{4}'", TokenTypes.String.Char)
                .ByGroups(@"(\.)((?:[^\W\d]|\$)[\w$]*)",
                    new TokenGroupProcessor(TokenTypes.Punctuation),
                    new TokenGroupProcessor(TokenTypes.Name.Attribute))
                .ByGroups(@"^(\s*)(default)(:)",
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Keyword),
                    new TokenGroupProcessor(TokenTypes.Punctuation))
                .ByGroups(@"^(\s*)((?:[^\W\d]|\$)[\w$]*)(:)",
                    new TokenGroupProcessor(TokenTypes.Text),
                    new TokenGroupProcessor(TokenTypes.Name.Label),
                    new TokenGroupProcessor(TokenTypes.Punctuation))
                .Add(@"([^\W\d]|\$)[\w$]*", TokenTypes.Name)
                .Add(@"([0-9][0-9_]*\.([0-9][0-9_]*)?|" +
                 @"\.[0-9][0-9_]*)" +
                 @"([eE][+\-]?[0-9][0-9_]*)?[fFdD]?|" +
                 @"[0-9][eE][+\-]?[0-9][0-9_]*[fFdD]?|" +
                 @"[0-9]([eE][+\-]?[0-9][0-9_]*)?[fFdD]|" +
                 @"0[xX]([0-9a-fA-F][0-9a-fA-F_]*\.?|" +
                 @"([0-9a-fA-F][0-9a-fA-F_]*)?\.[0-9a-fA-F][0-9a-fA-F_]*)" +
                 @"[pP][+\-]?[0-9][0-9_]*[fFdD]?", TokenTypes.Number.Float)
                .Add(@"0[xX][0-9a-fA-F][0-9a-fA-F_]*[lL]?", TokenTypes.Number.Hex)
                .Add(@"0[bB][01][01_]*[lL]?", TokenTypes.Number.Bin)
                .Add(@"0[0-7_]+[lL]?", TokenTypes.Number.Oct)
                .Add(@"0|[1-9][0-9_]*[lL]?", TokenTypes.Number.Integer)
                .Add(@"[~^*!%&\[\]<>|+=/?-]", TokenTypes.Operator)
                .Add(@"[{}();:.,]", TokenTypes.Punctuation)
                .Add(@"\n", TokenTypes.Text)
                .Build();

            rules["class"] = builder.NewRuleSet()
                .Add(@"\s+", TokenTypes.Text)
                .Add(@"([^\W\d]|\$)[\w$]*", TokenTypes.Name.Class, "#pop")
                .Build();

            rules["var"] = builder.NewRuleSet()
                .Add(@"([^\W\d]|\$)[\w$]*", TokenTypes.Name, "#pop")
                .Build();

            rules["import"] = builder.NewRuleSet()
                .Add(@"[\w.]+\*?", TokenTypes.Name.Namespace, "#pop")
                .Build();

            rules["string"] = builder.NewRuleSet()
                .Add(@"[^\\""]+", TokenTypes.String)
                .Add(@"\\\\", TokenTypes.String)  // Escaped backslash
                .Add(@"\\""", TokenTypes.String)  // Escaped quote
                .Add(@"\\", TokenTypes.String)  // Bare backslash
                .Add(@"""", TokenTypes.String, "#pop")  // Closing quote
                .Build();

            return rules;
        }
    }
}
