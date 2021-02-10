using Microsoft.CodeAnalysis;
using System;
using System.CodeDom.Compiler;
using System.IO;

namespace InlineMapping.Configuration
{
  internal sealed class ConfigurationValues
  {
    public ConfigurationValues(GeneratorExecutionContext context, SyntaxTree tree)
    {
      var options = context.AnalyzerConfigOptions.GetOptions(tree);

      this.IndentStyle = options.TryGetValue(IndentStyleKey, out var indentStyle) ?
          (Enum.TryParse<IndentStyle>(indentStyle, out var indentStyleValue) ? indentStyleValue : IndentStyleDefaultValue) :
          IndentStyleDefaultValue;
      this.IndentSize = options.TryGetValue(IndentSizeKey, out var indentSize) ?
          (uint.TryParse(indentSize, out var indentSizeValue) ? indentSizeValue : IndentSizeDefaultValue) :
          IndentSizeDefaultValue;
      this.EolStyle = options.TryGetValue(EolStyleKey, out var eol) ?
          (Enum.TryParse<EolStyle>(eol, ignoreCase: true, out var eolValue) ? eolValue : EolDefaultValue) :
          EolDefaultValue;
    }

    public IndentedTextWriter BuildIndentedTextWriter(StringWriter writer)
      => new IndentedTextWriter(writer, this.IndentStyle == IndentStyle.Tab ? "\t" : new string(' ', (int)this.IndentSize))
      {
        NewLine = this.EolStyle switch
        {
          EolStyle.LF => "\n",
          EolStyle.CR => "\r",
          EolStyle.CRLF => "\r\n",
          _ => throw new NotSupportedException($"Invalid EolStyle {this.EolStyle}"),
        }
      };

    internal EolStyle EolStyle { get; }
    internal uint IndentSize { get; }
    internal IndentStyle IndentStyle { get; }

    private const EolStyle EolDefaultValue = EolStyle.LF;
    private const string EolStyleKey = "end_of_line";
    private const uint IndentSizeDefaultValue = 2u;
    private const string IndentSizeKey = "indent_size";
    private const IndentStyle IndentStyleDefaultValue = IndentStyle.Space;
    private const string IndentStyleKey = "indent_style";
  }
}