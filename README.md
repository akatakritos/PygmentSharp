# PygmentSharp

![Build Status](https://github.com/akatakritos/PygmentSharp/workflows/.NET%20Core/badge.svg)

A port of python's [Pygments](http://pygments.org/) syntax highlighter.

Aims to follow the overall approach (for now) while still being mostly
idiomatic c#.

Roadmap:

I need the following features from Pygment for my static blog generator.
Other features can be ported by the community as needed.

- [x] HTML Output
- [x] C# Highlighting
- [X] SQL Highlighting
- [X] HTML Highlighting
  - [X] Javascript Highlighting
  - [X] CSS Highlighting
- [X] XML Highlighting
- [X] Shell script highlighting
- [ ] PHP Highlighting
- [ ] Ruby Highlighting

## Installation

Get it from Nuget: [PygmentSharp.Core](https://www.nuget.org/packages/PygmentSharp.Core/)

## Usage

```csharp

// basic case, specify lexer and formatter
var highlighted = Pygmentize.Content("class Foo {}")
    .WithLexer(new CSharpLexer())
    .WithFormatter(new HtmlFormatter())
    .AsString();

// Common overloads get their own fluent command

var highlighted = Pygmentize.Content("class Foo { }")
    .WithLexer(new CSharpLexer())
    .ToHtml()
    .AsString();

// It will support inference for input and output types

Pygmentize.File("test.cs")  // infers language from extension
    .ToFile("output.html"); // infers output formatter from extension

```


