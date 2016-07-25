using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Creates a new token with an index adjusted by <see cref="indexOffset"/>
        /// </summary>
        /// <remarks>This is useful for nested lexers that pass the inner lexer a substring of the full file, and nead to adjust the posititions accordingly</remarks>
        /// <param name="indexOffset">The number of characters to offset</param>
        /// <returns>A new token offset by the specified number of characters</returns>
        public Token Offset(int indexOffset)
        {
            return new Token(Index + indexOffset, Type, Value);
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
}