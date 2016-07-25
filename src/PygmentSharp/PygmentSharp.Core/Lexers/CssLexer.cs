using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Lexers
{
    [Lexer("CSS", AlternateNames = "css")]
    [LexerFileExtension("*.css")]
    public class CssLexer : RegexLexer
    {
        protected override IDictionary<string, StateRule[]> GetStateRules()
        {
            var rules = new Dictionary<string, StateRule[]>();
            var builder = new StateRuleBuilder();

            rules["basics"] = new[]
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.Create(@"/\*(?:.|\n)*?\*/", TokenTypes.Comment),
                builder.Create(@"\{", TokenTypes.Punctuation, "content"),
                builder.Create(@"\:[\w-]+", TokenTypes.Name.Decorator),
                builder.Create(@"\.[\w-]+", TokenTypes.Name.Class),
                builder.Create(@"\#[\w-]+", TokenTypes.Name.Namespace),
                builder.Create(@"@[\w-]+", TokenTypes.Keyword, "atrule"),
                builder.Create(@"[\w-]+", TokenTypes.Name.Tag),
                builder.Create(@"[~^*!%&$\[\]()<>|+=@:;,./?-]", TokenTypes.Operator),
                builder.Create(@"""(\\\\|\\""|[^""])*""", TokenTypes.String.Double),
                builder.Create(@"'(\\\\|\\'|[^'])*'", TokenTypes.String.Single)
            };

            rules["root"] = builder.Include(rules["basics"]);

            rules["atrule"] = new[]
            {
                builder.Create(@"\{", TokenTypes.Punctuation, "atcontent"),
                builder.Create(@";", TokenTypes.Punctuation, "#pop"),
            }.Concat(rules["basics"]).ToArray();

            rules["atcontent"] = builder.Include(rules["basics"],
                builder.Create(@"\}", TokenTypes.Punctuation, "#pop", "#pop")
            );

            rules["content"] = new[]
            {
                builder.Create(@"\s+", TokenTypes.Text),
                builder.Create(@"\}", TokenTypes.Punctuation, "#pop"),
                builder.Create(@"url\(.*?\)", TokenTypes.String.Other),
                builder.Create(@"^@.*?$", TokenTypes.Comment.Preproc),
                builder.Create(RegexUtil.Words(new []
                {
                   "azimuth", "background-attachment", "background-color",
                    "background-image", "background-position", "background-repeat",
                    "background", "border-bottom-color", "border-bottom-style",
                    "border-bottom-width", "border-left-color", "border-left-style",
                    "border-left-width", "border-right", "border-right-color",
                    "border-right-style", "border-right-width", "border-top-color",
                    "border-top-style", "border-top-width", "border-bottom",
                    "border-collapse", "border-left", "border-width", "border-color",
                    "border-spacing", "border-style", "border-top", "border", "caption-side",
                    "clear", "clip", "color", "content", "counter-increment", "counter-reset",
                    "cue-after", "cue-before", "cue", "cursor", "direction", "display",
                    "elevation", "empty-cells", "float", "font-family", "font-size",
                    "font-size-adjust", "font-stretch", "font-style", "font-variant",
                    "font-weight", "font", "height", "letter-spacing", "line-height",
                    "list-style-type", "list-style-image", "list-style-position",
                    "list-style", "margin-bottom", "margin-left", "margin-right",
                    "margin-top", "margin", "marker-offset", "marks", "max-height", "max-width",
                    "min-height", "min-width", "opacity", "orphans", "outline-color",
                    "outline-style", "outline-width", "outline", "overflow", "overflow-x",
                    "overflow-y", "padding-bottom", "padding-left", "padding-right", "padding-top",
                    "padding", "page", "page-break-after", "page-break-before", "page-break-inside",
                    "pause-after", "pause-before", "pause", "pitch-range", "pitch",
                    "play-during", "position", "quotes", "richness", "right", "size",
                    "speak-header", "speak-numeral", "speak-punctuation", "speak",
                    "speech-rate", "stress", "table-layout", "text-align", "text-decoration",
                    "text-indent", "text-shadow", "text-transform", "top", "unicode-bidi",
                    "vertical-align", "visibility", "voice-family", "volume", "white-space",
                    "widows", "width", "word-spacing", "z-index", "bottom",
                    "above", "absolute", "always", "armenian", "aural", "auto", "avoid", "baseline",
                    "behind", "below", "bidi-override", "blink", "block", "bolder", "bold", "both",
                    "capitalize", "center-left", "center-right", "center", "circle",
                    "cjk-ideographic", "close-quote", "collapse", "condensed", "continuous",
                    "crop", "crosshair", "cross", "cursive", "dashed", "decimal-leading-zero",
                    "decimal", "default", "digits", "disc", "dotted", "double", "e-resize", "embed",
                    "extra-condensed", "extra-expanded", "expanded", "fantasy", "far-left",
                    "far-right", "faster", "fast", "fixed", "georgian", "groove", "hebrew", "help",
                    "hidden", "hide", "higher", "high", "hiragana-iroha", "hiragana", "icon",
                    "inherit", "inline-table", "inline", "inset", "inside", "invert", "italic",
                    "justify", "katakana-iroha", "katakana", "landscape", "larger", "large",
                    "left-side", "leftwards", "left", "level", "lighter", "line-through", "list-item",
                    "loud", "lower-alpha", "lower-greek", "lower-roman", "lowercase", "ltr",
                    "lower", "low", "medium", "message-box", "middle", "mix", "monospace",
                    "n-resize", "narrower", "ne-resize", "no-close-quote", "no-open-quote",
                    "no-repeat", "none", "normal", "nowrap", "nw-resize", "oblique", "once",
                    "open-quote", "outset", "outside", "overline", "pointer", "portrait", "px",
                    "relative", "repeat-x", "repeat-y", "repeat", "rgb", "ridge", "right-side",
                    "rightwards", "s-resize", "sans-serif", "scroll", "se-resize",
                    "semi-condensed", "semi-expanded", "separate", "serif", "show", "silent",
                    "slower", "slow", "small-caps", "small-caption", "smaller", "soft", "solid",
                    "spell-out", "square", "static", "status-bar", "super", "sw-resize",
                    "table-caption", "table-cell", "table-column", "table-column-group",
                    "table-footer-group", "table-header-group", "table-row",
                    "table-row-group", "text-bottom", "text-top", "text", "thick", "thin",
                    "transparent", "ultra-condensed", "ultra-expanded", "underline",
                    "upper-alpha", "upper-latin", "upper-roman", "uppercase", "url",
                    "visible", "w-resize", "wait", "wider", "x-fast", "x-high", "x-large", "x-loud",
                    "x-low", "x-small", "x-soft", "xx-large", "xx-small", "yes"
                }, suffix: @"\b"), TokenTypes.Name.Builtin),
                builder.Create(RegexUtil.Words(new []
                {
                    "indigo", "gold", "firebrick", "indianred", "yellow", "darkolivegreen",
                    "darkseagreen", "mediumvioletred", "mediumorchid", "chartreuse",
                    "mediumslateblue", "black", "springgreen", "crimson", "lightsalmon", "brown",
                    "turquoise", "olivedrab", "cyan", "silver", "skyblue", "gray", "darkturquoise",
                    "goldenrod", "darkgreen", "darkviolet", "darkgray", "lightpink", "teal",
                    "darkmagenta", "lightgoldenrodyellow", "lavender", "yellowgreen", "thistle",
                    "violet", "navy", "orchid", "blue", "ghostwhite", "honeydew", "cornflowerblue",
                    "darkblue", "darkkhaki", "mediumpurple", "cornsilk", "red", "bisque", "slategray",
                    "darkcyan", "khaki", "wheat", "deepskyblue", "darkred", "steelblue", "aliceblue",
                    "gainsboro", "mediumturquoise", "floralwhite", "coral", "purple", "lightgrey",
                    "lightcyan", "darksalmon", "beige", "azure", "lightsteelblue", "oldlace",
                    "greenyellow", "royalblue", "lightseagreen", "mistyrose", "sienna",
                    "lightcoral", "orangered", "navajowhite", "lime", "palegreen", "burlywood",
                    "seashell", "mediumspringgreen", "fuchsia", "papayawhip", "blanchedalmond",
                    "peru", "aquamarine", "white", "darkslategray", "ivory", "dodgerblue",
                    "lemonchiffon", "chocolate", "orange", "forestgreen", "slateblue", "olive",
                    "mintcream", "antiquewhite", "darkorange", "cadetblue", "moccasin",
                    "limegreen", "saddlebrown", "darkslateblue", "lightskyblue", "deeppink",
                    "plum", "aqua", "darkgoldenrod", "maroon", "sandybrown", "magenta", "tan",
                    "rosybrown", "pink", "lightblue", "palevioletred", "mediumseagreen",
                    "dimgray", "powderblue", "seagreen", "snow", "mediumblue", "midnightblue",
                    "paleturquoise", "palegoldenrod", "whitesmoke", "darkorchid", "salmon",
                    "lightslategray", "lawngreen", "lightgreen", "tomato", "hotpink",
                    "lightyellow", "lavenderblush", "linen", "mediumaquamarine", "green",
                    "blueviolet", "peachpuff"
                }, suffix: @"\b"), TokenTypes.Name.Builtin),
                builder.Create(@"\!important", TokenTypes.Comment.Preproc),
                builder.Create(@"/\*(?:.|\n)*?\*/", TokenTypes.Comment),
                builder.Create(@"\#[a-zA-Z0-9]{1,6}", TokenTypes.Number),
                builder.Create(@"[.-]?[0-9]*[.]?[0-9]+(em|px|pt|pc|in|mm|cm|ex|s)\b", TokenTypes.Number),
                builder.Create(@"[.-]?[0-9]*[.]?[0-9]+%", TokenTypes.Number),
                builder.Create(@"-?[0-9]+", TokenTypes.Number),
                builder.Create(@"[~^*!%&<>|+=@:,./?-]+", TokenTypes.Operator),
                builder.Create(@"[\[\]();]+", TokenTypes.Punctuation),
                builder.Create(@"""(\\\\|\\""|[^""])*""", TokenTypes.String.Double),
                builder.Create(@"'(\\\\|\\'|[^'])*'", TokenTypes.String.Single),
                builder.Create(@"a-zA-Z_]\w*", TokenTypes.Name)
            };

            return rules;
        }
    }
}