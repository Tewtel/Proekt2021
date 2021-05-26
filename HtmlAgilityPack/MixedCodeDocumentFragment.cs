// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.MixedCodeDocumentFragment
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents a base class for fragments in a mixed code document.
  /// </summary>
  public abstract class MixedCodeDocumentFragment
  {
    internal MixedCodeDocument Doc;
    private string _fragmentText;
    internal int Index;
    internal int Length;
    private int _line;
    internal int _lineposition;
    internal MixedCodeDocumentFragmentType _type;

    internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
    {
      this.Doc = doc;
      this._type = type;
      switch (type)
      {
        case MixedCodeDocumentFragmentType.Code:
          this.Doc._codefragments.Append(this);
          break;
        case MixedCodeDocumentFragmentType.Text:
          this.Doc._textfragments.Append(this);
          break;
      }
      this.Doc._fragments.Append(this);
    }

    /// <summary>Gets the fragement text.</summary>
    public string FragmentText
    {
      get
      {
        if (this._fragmentText == null)
          this._fragmentText = this.Doc._text.Substring(this.Index, this.Length);
        return this._fragmentText;
      }
      internal set => this._fragmentText = value;
    }

    /// <summary>Gets the type of fragment.</summary>
    public MixedCodeDocumentFragmentType FragmentType => this._type;

    /// <summary>Gets the line number of the fragment.</summary>
    public int Line
    {
      get => this._line;
      internal set => this._line = value;
    }

    /// <summary>Gets the line position (column) of the fragment.</summary>
    public int LinePosition => this._lineposition;

    /// <summary>Gets the fragment position in the document's stream.</summary>
    public int StreamPosition => this.Index;
  }
}
