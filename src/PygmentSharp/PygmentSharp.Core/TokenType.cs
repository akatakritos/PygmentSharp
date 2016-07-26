using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core
{
    /// <summary>
    /// Represents a nested type for a <see cref="Token"/>
    /// </summary>
    /// <remarks>
    /// Token Types are nested. For example, a Comment will have nested types <c>Comment.Multiline</c>,
    /// <c>Comment.Single</c>, or <c>Comment.Preproc</c>. Lexers and formatters have a choice on how
    /// specific they want to get. If a formatter doesn't want to support different styles for each of
    /// those comment types, it can just implement highlighting for Comment and all the child types will
    /// fall in line.
    /// </remarks>
    public class TokenType
    {
        /// <summary>
        /// Gets the parent type
        /// </summary>
        public TokenType Parent { get; }

        /// <summary>
        /// Gets the name of this type
        /// </summary>
        public string Name { get; set; }

        private readonly List<TokenType> _subtypes;
        /// <summary>
        /// Gets the depth of this Token Type
        /// </summary>
        /// <remarks>
        /// For example, Root.Comment.Preproc has a depth of 3
        /// </remarks>
        public int Depth { get; }

        /// <summary>
        /// Gets the list of subtypes for this token type
        /// </summary>
        public IReadOnlyCollection<TokenType> Subtypes => _subtypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenType"/> class
        /// </summary>
        /// <param name="parent">The parent token type</param>
        /// <param name="name">The name of this token type</param>
        public TokenType(TokenType parent, string name)
        {
            Parent = parent;
            Name = name;

            _subtypes = new List<TokenType>();
            Depth = CalculateDepth();
        }

        /// <summary>
        /// Creates a child token type
        /// </summary>
        /// <param name="name">The name of the child token type</param>
        /// <returns></returns>
        public TokenType CreateChild(string name)
        {
            var newTokenType = new TokenType(this, name);
            _subtypes.Add(newTokenType);
            return newTokenType;
        }

        public TChild AddChild<TChild>(TChild child) where TChild : TokenType
        {
            _subtypes.Add(child);
            return child;
        }

        /// <summary>
        /// Gets a list of types for this token, starting with itself and ending at its highest level parent
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TokenType> Split()
        {
            return YieldAncestors().Reverse();
        }

        private int CalculateDepth()
        {
            return YieldAncestors().Count();
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
            Attribute = CreateChild("Attribute");
            Builtin = AddChild(new NameBuiltinTokenType(this));
            Class = CreateChild("Class");
            Constant = CreateChild("Constant");
            Decorator = CreateChild("Decorator");
            Entity = CreateChild("Entity");
            Exception = CreateChild("Exception");
            Function = CreateChild("Exception");
            Property = CreateChild("Property");
            Label = CreateChild("Label");
            Namespace = CreateChild("Namespace");
            Other = CreateChild("Other");
            Tag = CreateChild("Tag");
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
            Class = CreateChild("Class");
            Global = CreateChild("Global");
            Instance = CreateChild("Instance");
        }

        public TokenType Class { get; }
        public TokenType Global { get; }
        public TokenType Instance { get; }
    }

    public class NameBuiltinTokenType : TokenType
    {
        public NameBuiltinTokenType(TokenType parent) : base(parent, "Builtin")
        {
            Pseudo = CreateChild("Pseudo");
        }

        public TokenType Pseudo { get; }
    }

    public class KeywordTokenType : TokenType
    {
        public KeywordTokenType(TokenType parent) : base(parent, "Keyword")
        {
            Constant = CreateChild("Constant");
            Declaration = CreateChild("Declaration");
            Namespace = CreateChild("Namespace");
            Pseudo = CreateChild("Pseudo");
            Reserved = CreateChild("Reserved");
            Type = CreateChild("Type");
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
            Date = CreateChild("Date");
            String = AddChild(new StringTokenType(this));
            Number = AddChild(new NumberTokenType(this));
        }

        public TokenType Date { get; }
        public StringTokenType String { get; }
        public NumberTokenType Number { get; }
    }

    public class StringTokenType : TokenType
    {
        public StringTokenType(TokenType parent) : base(parent, "String")
        {
            Backtick = CreateChild("Backtick");
            Char = CreateChild("Char");
            Doc = CreateChild("Doc");
            Double = CreateChild("Double");
            Escape = CreateChild("Escape");
            Heredoc = CreateChild("Heredoc");
            Interpol = CreateChild("Interpol");
            Other = CreateChild("Other");
            Regex = CreateChild("Regex");
            Single = CreateChild("Single");
            Symbol = CreateChild("Symbol");

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

            Bin = CreateChild("Bin");
            Float = CreateChild("Float");
            Hex = CreateChild("Hex");
            Integer = AddChild(new IntegerTokenType(this));
            Oct = CreateChild("Oct");
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
            Long = CreateChild("Long");
        }

        public TokenType Long { get; }
    }

    public class OperatorTokenType : TokenType
    {
        public OperatorTokenType(TokenType parent) : base(parent, "Operator")
        {
            Word = CreateChild("Word");
        }

        public TokenType Word { get; }

    }

    public class CommentTokenType : TokenType
    {
        public CommentTokenType(TokenType parent) : base(parent, "Comment")
        {
            Hashbang = CreateChild("Hashbang");
            Multiline = CreateChild("Multiline");
            Preproc = CreateChild("Preproc");
            PreprocFile = CreateChild("PreprocFile");
            Single = CreateChild("Single");
            Special = CreateChild("Special");
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
            Deleted = CreateChild("Deleted");
            Emph = CreateChild("Emph");
            Error = CreateChild("Error");
            Heading = CreateChild("Heading");
            Inserted = CreateChild("Inserted");
            Output = CreateChild("Output");
            Prompt = CreateChild(nameof(Prompt));
            Strong = CreateChild(nameof(Strong));
            Subheading = CreateChild(nameof(Subheading));
            Traceback = CreateChild(nameof(Traceback));

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
        public static readonly TokenType Text = Token.CreateChild("Text");
        public static readonly TokenType Whitespace = Text.CreateChild("Whitespace");
        public static readonly TokenType Escape = Token.CreateChild("Escape");
        public static readonly TokenType Error = Token.CreateChild("Error");
        public static readonly TokenType Other = Token.CreateChild("Other");
        public static readonly KeywordTokenType Keyword = Token.AddChild(new KeywordTokenType(Token));
        public static readonly NameTokenType Name = Token.AddChild(new NameTokenType(Token));
        public static readonly LiteralTokenType Literal = Token.AddChild(new LiteralTokenType(Token));
        public static readonly StringTokenType String = Literal.String; //alias
        public static readonly NumberTokenType Number = Literal.Number; //alias
        public static readonly TokenType Punctuation = Token.CreateChild("Punctuation");
        public static readonly OperatorTokenType Operator = Token.AddChild(new OperatorTokenType(Token));
        public static readonly CommentTokenType Comment = Token.AddChild(new CommentTokenType(Token));
        public static readonly GenericTokenType Generic = Token.AddChild(new GenericTokenType(Token));
    }
}
