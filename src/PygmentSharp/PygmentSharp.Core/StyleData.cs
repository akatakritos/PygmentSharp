using System;
using System.Text;

namespace PygmentSharp.Core
{
    public sealed class StyleData : IEquatable<StyleData>
    {
        public string Color { get; }
        public bool Bold { get; }
        public bool Italic { get; }
        public bool Underline { get; }
        public string BackgroundColor { get; }
        public string BorderColor { get; }
        public bool Roman { get; }
        public bool Sans { get; }
        public bool Mono { get; }

        public StyleData(string color = null,
            bool bold = false,
            bool italic = false,
            bool underline = false,
            string bgColor = null,
            string borderColor = null,
            bool roman = false,
            bool sans = false,
            bool mono = false)
        {
            Color = color;
            Bold = bold;
            Italic = italic;
            Underline = underline;
            BackgroundColor = bgColor;
            BorderColor = borderColor;
            Roman = roman;
            Sans = sans;
            Mono = mono;
        }

        public static StyleData Parse(string text)
        {
            string color = null, bgColor = null, borderColor = null;
            bool bold = false, italic = false, underline = false, roman = false, sans = false, mono = false;

            foreach (var styledef in text.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries))
            {
                if (styledef == "noinherit")
                {
                    // noop
                }
                else if (styledef == "bold")
                    bold = true;
                else if (styledef == "nobold")
                    bold = false;
                else if (styledef == "italic")
                    italic = true;
                else if (styledef == "noitalic")
                    italic = false;
                else if (styledef == "underline")
                    underline = true;
                else if (styledef.StartsWith("bg:"))
                    bgColor = ColorFormat(styledef.Substring(3));
                else if (styledef.StartsWith("border:"))
                    borderColor = ColorFormat(styledef.Substring(7));
                else if (styledef == "roman")
                    roman = true;
                else if (styledef == "sans")
                    sans = true;
                else if (styledef == "mono")
                    mono = true;
                else
                    color = ColorFormat(styledef);
            }

            return new StyleData(color, bold, italic, underline, bgColor, borderColor, roman, sans, mono);
        }

        public override string ToString()
        {
            var color = string.IsNullOrEmpty(Color) ? null : $"color:#{Color};";
            var fontWeight = Bold ? "font-weight:bold;" : null;
            var fontStyle = Italic ? "font-style:italic;" : null;
            var textDecoration = Underline ? "text-decoration:underline;" : null;
            var borderColor = string.IsNullOrEmpty(BorderColor) ? null : $"border-color:#{BorderColor};";
            var backgroundColor = string.IsNullOrEmpty(BackgroundColor) ? null : $"background-color:#{BackgroundColor};";
            var fontFamily = new StringBuilder();
            if (Roman || Sans || Mono)
            {
                fontFamily.Append("font-family:");
                if (Roman) fontFamily.Append(" roman");
                if (Sans) fontFamily.Append(" sans");
                if (Mono) fontFamily.Append(" mono");
                fontFamily.Append(";");
            }

            return string.Concat(color, backgroundColor, borderColor, fontFamily, fontWeight, fontStyle, textDecoration);
        }

        private static string ColorFormat(string color)
        {
            if (color.StartsWith("#", StringComparison.InvariantCulture))
            {
                var colorPart = color.Substring(1);
                if (colorPart.Length == 6)
                    return colorPart;

                if (colorPart.Length == 3)
                {
                    var r = colorPart[0];
                    var g = colorPart[1];
                    var b = colorPart[2];
                    return string.Concat(r, r, g, g, b, b);
                }
            }
            else if (color == "")
                return "";

            throw new FormatException($"Could not understand color '{color}'.");
        }

        #region R# Equality

        public bool Equals(StyleData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Color, other.Color) && Bold == other.Bold && Italic == other.Italic && Underline == other.Underline && string.Equals(BackgroundColor, other.BackgroundColor) && string.Equals(BorderColor, other.BorderColor) && Roman == other.Roman && Sans == other.Sans && Mono == other.Mono;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StyleData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Color != null ? Color.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Bold.GetHashCode();
                hashCode = (hashCode*397) ^ Italic.GetHashCode();
                hashCode = (hashCode*397) ^ Underline.GetHashCode();
                hashCode = (hashCode*397) ^ (BackgroundColor != null ? BackgroundColor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (BorderColor != null ? BorderColor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Roman.GetHashCode();
                hashCode = (hashCode*397) ^ Sans.GetHashCode();
                hashCode = (hashCode*397) ^ Mono.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(StyleData left, StyleData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StyleData left, StyleData right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}