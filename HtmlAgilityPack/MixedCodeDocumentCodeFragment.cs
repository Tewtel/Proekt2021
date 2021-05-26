// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.MixedCodeDocumentCodeFragment
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents a fragment of code in a mixed code document.
  /// </summary>
  public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
  {
    private string _code;

    internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc)
      : base(doc, MixedCodeDocumentFragmentType.Code)
    {
    }

    /// <summary>Gets the fragment code text.</summary>
    public string Code
    {
      get
      {
        if (this._code == null)
        {
          this._code = this.FragmentText.Substring(this.Doc.TokenCodeStart.Length, this.FragmentText.Length - this.Doc.TokenCodeEnd.Length - this.Doc.TokenCodeStart.Length - 1).Trim();
          if (this._code.StartsWith("="))
            this._code = this.Doc.TokenResponseWrite + this._code.Substring(1, this._code.Length - 1);
        }
        return this._code;
      }
      set => this._code = value;
    }
  }
}
