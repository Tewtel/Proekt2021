// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.MixedCodeDocumentTextFragment
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents a fragment of text in a mixed code document.
  /// </summary>
  public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
  {
    internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
      : base(doc, MixedCodeDocumentFragmentType.Text)
    {
    }

    /// <summary>Gets the fragment text.</summary>
    public string Text
    {
      get => this.FragmentText;
      set => this.FragmentText = value;
    }
  }
}
