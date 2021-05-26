// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlNodeType
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>Represents the type of a node.</summary>
  public enum HtmlNodeType
  {
    /// <summary>The root of a document.</summary>
    Document,
    /// <summary>An HTML element.</summary>
    Element,
    /// <summary>An HTML comment.</summary>
    Comment,
    /// <summary>
    /// A text node is always the child of an element or a document node.
    /// </summary>
    Text,
  }
}
