using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using PygmentSharp.Core.Formatters;

namespace PygmentSharp.Core
{
    public class HighlightingResult
    {
        public Stream OutputStream { get; }
        public string Input { get; }

        public HighlightingResult(string input, Stream output)
        {
            Input = input;
            OutputStream = output;
        }


        public string OutputAsString(Encoding encoding = null)
        {
            var outputEncoding = encoding ?? Encoding.UTF8;
            var reader = new StreamReader(OutputStream, outputEncoding);
            return reader.ReadToEnd();
        }
    }

    public static class FluentExtensions
    {
        public static IPygmentizeBuilder WithLexerFor(this IPygmentizeBuilder builder, string name)
        {
            throw new NotImplementedException();
        }

        public static IPygmentizeBuilder WithLexerForFile(this IPygmentizeBuilder builder, string filename)
        {
            throw new NotImplementedException();
        }

        public static IPygmentizeBuilder ToHtml(this IPygmentizeBuilder builder)
        {
            return builder.WithFormatter(new HtmlFormatter());
        }
    }


    public interface IPygmentizeBuilder
    {
        IPygmentizeBuilder WithInputEncoding(Encoding encoding);
        IPygmentizeBuilder WithOutputEncoding(Encoding encoding);
        IPygmentizeBuilder WithLexer(Lexer lexer);
        IPygmentizeBuilder WithFormatter(Formatter formatter);
        string AsString();
        void ToFile(string filename);
    }

    internal class PygmentizeContentBuilder : IPygmentizeBuilder
    {
        private string _input;
        public PygmentizeContentBuilder(string content)
        {
            _input = content;
        }

        private Lexer _lexer = new PlainLexer();
        private Formatter _formatter = new NullFormatter();

        public IPygmentizeBuilder WithInputEncoding(Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public IPygmentizeBuilder WithOutputEncoding(Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public IPygmentizeBuilder WithLexer(Lexer lexer)
        {
            _lexer = lexer;
            return this;
        }

        public IPygmentizeBuilder WithFormatter(Formatter formatter)
        {
            _formatter = formatter;
            return this;
        }

        public string AsString()
        {
            var tokens = _lexer.GetTokens(_input);
            var memoryStream = new StringWriter();
            _formatter.Format(tokens, memoryStream);

            return memoryStream.ToString();
        }

        public void ToFile(string filename)
        {
            var formatter = new FormatterLocator().FindByFilename(filename);
            var tokens = _lexer.GetTokens(_input);
            using (var output = new StreamWriter(File.OpenWrite(filename), Encoding.UTF8))
            {
                formatter.Format(tokens, output);
            }
        }
    }


    public static class Pygmentize
    {
        public static IPygmentizeBuilder Content(string content)
        {
            return new PygmentizeContentBuilder(content);
        }

        public static IPygmentizeBuilder File(string filename)
        {
            var lexer = new LexerLocator().FindByFilename(filename);
            return new PygmentizeContentBuilder(System.IO.File.ReadAllText(filename))
                .WithLexer(lexer);
        }

    }
}
