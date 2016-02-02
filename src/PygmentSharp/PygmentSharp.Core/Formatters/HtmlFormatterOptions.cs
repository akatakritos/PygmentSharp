using System;
using System.Collections.Generic;
using System.Linq;

namespace PygmentSharp.Core.Formatters
{
    public class HtmlFormatterOptions
    {
        public HtmlFormatterOptions()
        {
            HighlightLines = Enumerable.Empty<int>();
        }

        public bool NoWrap { get; set; }
        public bool Full { get; set; }
        public string Title { get; set; }
        public bool NoClasses { get; set; }
        public string ClassPrefix { get; set; }
        public string CssClass { get; set; } = "highlight";
        public string PreStyles { get; set; } = "";
        public string CssFile { get; set; }
        public bool NoClobberCssFile { get; set; }
        public LineNumberStype LineNumbers { get; set; } = LineNumberStype.None;
        public int LineNumberStart { get; set; } = 0;
        public int LineNumberStep { get; set; } = 1;
        public int LineNumberSpecial { get; set; } = 0;
        public bool NoBackground { get; set; } = false;
        public string LineSeparator { get; set; } = "\n";
        public string LineAnchors { get; set; } = "";
        public string LineSpans { get; set; } = "";
        public bool AnchorLineNumbers { get; set; } = false;
        public string Filename { get; set; }

        public IEnumerable<int> HighlightLines { get; set; }
        public string CssStyles { get; set; }
        public Style Style { get; set; }
    }

    public enum LineNumberStype
    {
        None,
        Table,
        Inline
    }
}