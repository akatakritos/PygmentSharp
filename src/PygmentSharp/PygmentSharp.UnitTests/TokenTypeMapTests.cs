using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;
using PygmentSharp.Core;
using PygmentSharp.Core.Tokens;

using Xunit;

namespace PygmentSharp.UnitTests
{
    public class TokenTypeMapTests
    {
        [Theory]
        [MemberData("MapTest")]
        public void TypesHaveCorrectCssClass(TokenType tokenType, string cssClass)
        {
            var subject = TokenTypeMap.Instance;

            var result = subject[tokenType];

            Check.That(result).IsEqualTo(cssClass);
        }

        public static IEnumerable<object[]> MapTest
        {
            get
            {
                yield return new object[] { TokenTypes.Token, "" };
                yield return new object[] { TokenTypes.Text, "" };
                yield return new object[] { TokenTypes.Whitespace, "w" };
                yield return new object[] { TokenTypes.Escape, "esc" };
                yield return new object[] { TokenTypes.Error, "err" };
                yield return new object[] { TokenTypes.Other, "x" };
                yield return new object[] { TokenTypes.Keyword, "k" };
                yield return new object[] { TokenTypes.Keyword.Constant, "kc" };
                yield return new object[] { TokenTypes.Keyword.Declaration, "kd" };
                yield return new object[] { TokenTypes.Keyword.Namespace, "kn" };
                yield return new object[] { TokenTypes.Keyword.Pseudo, "kp" };
                yield return new object[] { TokenTypes.Keyword.Reserved, "kr" };
                yield return new object[] { TokenTypes.Keyword.Type, "kt" };
                yield return new object[] { TokenTypes.Name, "n" };
                yield return new object[] { TokenTypes.Name.Attribute, "na" };
                yield return new object[] { TokenTypes.Name.Builtin, "nb" };
                yield return new object[] { TokenTypes.Name.Builtin.Pseudo, "bp" };
                yield return new object[] { TokenTypes.Name.Class, "nc" };
                yield return new object[] { TokenTypes.Name.Constant, "no" };
                yield return new object[] { TokenTypes.Name.Decorator, "nd" };
                yield return new object[] { TokenTypes.Name.Entity, "ni" };
                yield return new object[] { TokenTypes.Name.Exception, "ne" };
                yield return new object[] { TokenTypes.Name.Function, "nf" };
                yield return new object[] { TokenTypes.Name.Property, "py" };
                yield return new object[] { TokenTypes.Name.Label, "nl" };
                yield return new object[] { TokenTypes.Name.Namespace, "nn" };
                yield return new object[] { TokenTypes.Name.Other, "nx" };
                yield return new object[] { TokenTypes.Name.Tag, "nt" };
                yield return new object[] { TokenTypes.Name.Variable, "nv" };
                yield return new object[] { TokenTypes.Name.Variable.Class, "vc" };
                yield return new object[] { TokenTypes.Name.Variable.Global, "vg" };
                yield return new object[] { TokenTypes.Name.Variable.Instance, "vi" };
                yield return new object[] { TokenTypes.Literal, "l" };
                yield return new object[] { TokenTypes.Literal.Date, "ld" };
                yield return new object[] { TokenTypes.String, "s" };
                yield return new object[] { TokenTypes.String.Backtick, "sb" };
                yield return new object[] { TokenTypes.String.Char, "sc" };
                yield return new object[] { TokenTypes.String.Doc, "sd" };
                yield return new object[] { TokenTypes.String.Double, "s2" };
                yield return new object[] { TokenTypes.String.Escape, "se" };
                yield return new object[] { TokenTypes.String.Heredoc, "sh" };
                yield return new object[] { TokenTypes.String.Interpol, "si" };
                yield return new object[] { TokenTypes.String.Other, "sx" };
                yield return new object[] { TokenTypes.String.Regex, "sr" };
                yield return new object[] { TokenTypes.String.Single, "s1" };
                yield return new object[] { TokenTypes.String.Symbol, "ss" };
                yield return new object[] { TokenTypes.Number, "m" };
                yield return new object[] { TokenTypes.Number.Bin, "mb" };
                yield return new object[] { TokenTypes.Number.Float, "mf" };
                yield return new object[] { TokenTypes.Number.Hex, "mh" };
                yield return new object[] { TokenTypes.Number.Integer, "mi" };
                yield return new object[] { TokenTypes.Number.Integer.Long, "il" };
                yield return new object[] { TokenTypes.Number.Oct, "mo" };
                yield return new object[] { TokenTypes.Operator, "o" };
                yield return new object[] { TokenTypes.Operator.Word, "ow" };
                yield return new object[] { TokenTypes.Punctuation, "p" };
                yield return new object[] { TokenTypes.Comment, "c" };
                yield return new object[] { TokenTypes.Comment.Hashbang, "ch" };
                yield return new object[] { TokenTypes.Comment.Multiline, "cm" };
                yield return new object[] { TokenTypes.Comment.Preproc, "cp" };
                yield return new object[] { TokenTypes.Comment.PreprocFile, "cpf" };
                yield return new object[] { TokenTypes.Comment.Single, "c1" };
                yield return new object[] { TokenTypes.Comment.Special, "cs" };
                yield return new object[] { TokenTypes.Generic, "g" };
                yield return new object[] { TokenTypes.Generic.Deleted, "gd" };
                yield return new object[] { TokenTypes.Generic.Emph, "ge" };
                yield return new object[] { TokenTypes.Generic.Error, "gr" };
                yield return new object[] { TokenTypes.Generic.Heading, "gh" };
                yield return new object[] { TokenTypes.Generic.Inserted, "gi" };
                yield return new object[] { TokenTypes.Generic.Output, "go" };
                yield return new object[] { TokenTypes.Generic.Prompt, "gp" };
                yield return new object[] { TokenTypes.Generic.Strong, "gs" };
                yield return new object[] { TokenTypes.Generic.Subheading, "gu" };
                yield return new object[] { TokenTypes.Generic.Traceback, "gt" };



            }
        }

    }
}
