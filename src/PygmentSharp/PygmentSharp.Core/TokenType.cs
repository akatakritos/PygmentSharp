using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PygmentSharp.Core
{
    public class TokenType
    {
        public TokenType Parent { get; set; }
        public string Name { get; set; }

        private List<TokenType> _subtypes;

        public TokenType(TokenType parent, string name)
        {
            Parent = parent;
            Name = name;
            _subtypes = new List<TokenType>();

            Depth = CalculateDepth();
        }

        private int CalculateDepth()
        {
            return YieldAncestors().Count();
        }

        public int Depth { get; }

        public TokenType Create(string name)
        {
            var newTokenType = new TokenType(this, name);
            _subtypes.Add(newTokenType);
            return newTokenType;
        }

        public IEnumerable<TokenType> Split()
        {
            return YieldAncestors().Reverse();
        }

        private IEnumerable<TokenType> YieldAncestors()
        {
            var current = this;

            do
            {
                yield return current;
                current = current.Parent;

            } while (current != null);
        }

        public override string ToString()
        {
            return string.Join(".", Split().Select(t => t.Name));
        }

    }

    public class NameTokenType : TokenType
    {
        public NameTokenType(TokenType parent) : base(parent, "Name")
        {
            Attribute = Create("Attribute");
            Builtin = new NameBuiltinTokenType(this);
            Class = Create("Class");
            Constant = Create("Constant");
            Decorator = Create("Decorator");
            Entity = Create("Entity");
            Exception = Create("Exception");
            Function = Create("Exception");
            Property = Create("Property");
            Label = Create("Label");
            Namespace = Create("Namespace");
            Other = Create("Other");
            Tag = Create("Tag");
            Variable = new NameVariableTokenType(this);
        }

        public TokenType Attribute { get; }
        public NameBuiltinTokenType Builtin { get; }
        public TokenType Class { get; }
        public TokenType Constant { get; }
        public TokenType Decorator { get; }
        public TokenType Entity { get; }
        public TokenType Exception { get; }
        public TokenType Function { get; }
        public TokenType Property { get; }
        public TokenType Label { get; }
        public TokenType Namespace { get; }
        public TokenType Other { get; }
        public TokenType Tag { get; }
        public NameVariableTokenType Variable { get; }
    }

    public class NameVariableTokenType : TokenType
    {
        public NameVariableTokenType(TokenType parent) : base(parent, "Variable")
        {
            Class = Create("Class");
            Global = Create("Global");
            Instance = Create("Instance");
        }

        public TokenType Class { get; }
        public TokenType Global { get; }
        public TokenType Instance { get; }
    }

    public class NameBuiltinTokenType : TokenType
    {
        public NameBuiltinTokenType(TokenType parent) : base(parent, "Name")
        {
            Pseudo = Create("Pseudo");
        }

        public TokenType Pseudo { get; }
    }

    public class KeywordTokenType : TokenType
    {
        public KeywordTokenType(TokenType parent) : base(parent, "Keyword")
        {
            Constant = Create("Constant");
            Declaration = Create("Declaration");
            Namespace = Create("Namespace");
            Pseudo = Create("Pseudo");
            Reserved = Create("Reserved");
            Type = Create("Type");
        }

        public TokenType Constant { get; }
        public TokenType Declaration { get; }
        public TokenType Namespace { get; }
        public TokenType Pseudo{ get; }
        public TokenType Reserved { get; }
        public TokenType Type{ get; }

    }

    public class LiteralTokenType : TokenType
    {
        public LiteralTokenType(TokenType parent) : base(parent, "Literal")
        {
            Date = Create("Date");
            String = new StringTokenType(this);
            Number = new NumberTokenType(this);
        }

        public TokenType Date { get; }
        public StringTokenType String { get; }
        public NumberTokenType Number { get; }
    }

    public class StringTokenType : TokenType
    {
        public StringTokenType(TokenType parent) : base(parent, "String")
        {
            Backtick = Create("Backtick");
            Char = Create("Char");
            Doc = Create("Doc");
            Double = Create("Double");
            Escape = Create("Escape");
            Heredoc = Create("Heredoc");
            Interpol = Create("Interpol");
            Other = Create("Other");
            Regex = Create("Regex");
            Single = Create("Single");
            Symbol = Create("Symbol");

        }

        public TokenType Backtick { get; }
        public TokenType Char { get; }
        public TokenType Doc { get; }
        public TokenType Double { get; }
        public TokenType Escape { get; }
        public TokenType Heredoc { get; }
        public TokenType Interpol { get; }
        public TokenType Other { get; }
        public TokenType Regex { get; }
        public TokenType Single { get; }
        public TokenType Symbol { get; }
    }

    public class NumberTokenType : TokenType
    {
        public NumberTokenType(TokenType parent) : base(parent, "Number")
        {

            Bin = Create("Bin");
            Float = Create("Float");
            Hex = Create("Hex");
            Integer = new IntegerTokenType(this);
            Oct = Create("Oct");
        }

        public TokenType Bin { get; }
        public TokenType Float { get; }
        public TokenType Hex { get; }
        public IntegerTokenType Integer { get; }
        public TokenType Oct { get; }
    }

    public class IntegerTokenType : TokenType
    {
        public IntegerTokenType(TokenType parent) : base(parent, "Integer")
        {
            Long = Create("Long");
        }

        public TokenType Long { get; }
    }

    public class OperatorTokenType : TokenType
    {
        public OperatorTokenType(TokenType parent) : base(parent, "Operator")
        {
            Word = Create("Word");
        }

        public TokenType Word { get; }

    }

    public class CommentTokenType : TokenType
    {
        public CommentTokenType(TokenType parent) : base(parent, "Comment")
        {
            Hashbang = Create("Hashbang");
            Multiline = Create("Multiline");
            Preproc = Create("Preproc");
            PreprocFile = Create("PreprocFile");
            Single = Create("Single");
            Special = Create("Special");
        }

        public TokenType Hashbang { get; }
        public TokenType Multiline { get; }
        public TokenType Preproc { get; }
        public TokenType PreprocFile { get; }
        public TokenType Single { get; }
        public TokenType Special { get; }
    }

    public class GenericTokenType : TokenType
    {
        public GenericTokenType(TokenType parent) : base(parent, "Generic")
        {
            Deleted = Create("Deleted");
            Emph = Create("Emph");
            Error = Create("Error");
            Heading = Create("Heading");
            Inserted = Create("Inserted");
            Output = Create("Output");
            Prompt = Create(nameof(Prompt));
            Strong = Create(nameof(Strong));
            Subheading = Create(nameof(Subheading));
            Traceback = Create(nameof(Traceback));

        }

        public TokenType Deleted { get; }
        public TokenType Emph { get; }
        public TokenType Error { get; }
        public TokenType Heading { get; }
        public TokenType Inserted { get; }
        public TokenType Output { get; }
        public TokenType Prompt { get; }
        public TokenType Strong { get; }
        public TokenType Subheading { get; }
        public TokenType Traceback { get; }
    }

    public static class TokenTypes
    {
        public static readonly TokenType Token = new TokenType(null, "Token");
        public static readonly TokenType Text = Token.Create("Text");
        public static readonly TokenType Whitespace = Text.Create("Whitespace");
        public static readonly TokenType Escape = Token.Create("Escape");
        public static readonly TokenType Error = Token.Create("Error");
        public static readonly TokenType Other = Token.Create("Other");
        public static readonly KeywordTokenType Keyword = new KeywordTokenType(Token);
        public static readonly NameTokenType Name = new NameTokenType(Token);
        public static readonly LiteralTokenType Literal = new LiteralTokenType(Token);
        public static readonly StringTokenType String = Literal.String; //alias
        public static readonly NumberTokenType Number = Literal.Number; //alias
        public static readonly TokenType Punctuation = Token.Create("Punctuation");
        public static readonly OperatorTokenType Operator = new OperatorTokenType(Token);
        public static readonly CommentTokenType Comment = new CommentTokenType(Token);
        public static readonly GenericTokenType Generic = new GenericTokenType(Token);
    }
}
