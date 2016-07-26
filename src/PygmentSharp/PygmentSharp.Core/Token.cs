using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core
{
    /// <summary>
    /// Represents a token from the lexing file. A Token is a type, value, and position
    /// </summary>
    /// <remarks>
    /// Lexers will emit a sequence of Tokens, each with a given type. Formatters will consume
    /// Tokens and turn them into highlighted text in whatever format they support
    /// </remarks>
    public struct Token : IEquatable<Token>
    {
        /// <summary>
        /// The 0 based index into the string being lexed
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The type of this token (Comment, Text, Keyword, etc)
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// The string value of the token
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new Token at index 0
        /// </summary>
        /// <param name="type">The type of the Token</param>
        /// <param name="value">The value of the Token</param>
        public Token(TokenType type, string value) : this(0, type, value)
        {
        }

        /// <summary>
        /// Initializes a new Token at a provided index
        /// </summary>
        /// <param name="index">The index into the string being lexer</param>
        /// <param name="type">The type of the Token</param>
        /// <param name="value">The value of the Token</param>
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

        public override string ToString() => $"{Index}: \"{Value}\" ({Type})";

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