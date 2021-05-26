// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlParseError
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents a parsing error found during document parsing.
  /// </summary>
  public class HtmlParseError
  {
    private HtmlParseErrorCode _code;
    private int _line;
    private int _linePosition;
    private string _reason;
    private string _sourceText;
    private int _streamPosition;

    internal HtmlParseError(
      HtmlParseErrorCode code,
      int line,
      int linePosition,
      int streamPosition,
      string sourceText,
      string reason)
    {
      this._code = code;
      this._line = line;
      this._linePosition = linePosition;
      this._streamPosition = streamPosition;
      this._sourceText = sourceText;
      this._reason = reason;
    }

    /// <summary>Gets the type of error.</summary>
    public HtmlParseErrorCode Code => this._code;

    /// <summary>Gets the line number of this error in the document.</summary>
    public int Line => this._line;

    /// <summary>Gets the column number of this error in the document.</summary>
    public int LinePosition => this._linePosition;

    /// <summary>Gets a description for the error.</summary>
    public string Reason => this._reason;

    /// <summary>
    /// Gets the the full text of the line containing the error.
    /// </summary>
    public string SourceText => this._sourceText;

    /// <summary>
    /// Gets the absolute stream position of this error in the document, relative to the start of the document.
    /// </summary>
    public int StreamPosition => this._streamPosition;
  }
}
