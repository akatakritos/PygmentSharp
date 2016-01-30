using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace PygmentSharp.Core
{
    public struct Token : IEquatable<Token>
    {
        public int Index { get; }
        public TokenType Type { get; }

        public string Value { get; }

        public Token(TokenType type, string value) : this(0, type, value)
        {
        }

        public Token(int index, TokenType type, string value)
        {
            Index = index;
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Index}: \"{Value}\" ({Type})";
        }

        #region R# Equality Members

        public bool Equals(Token other)
        {
            return Index == other.Index && Type.Equals(other.Type) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Token && Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Index;
                hashCode = (hashCode*397) ^ Type.GetHashCode();
                hashCode = (hashCode*397) ^ Value.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
        }

        #endregion
    }

    public abstract class Lexer
    {
        public abstract string Name { get; }

        public IEnumerable<Token> GetTokens(string text)
        {
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            return GetTokensUnprocessed(text);
        }

        protected abstract IEnumerable<Token> GetTokensUnprocessed(string text);
    }

}
