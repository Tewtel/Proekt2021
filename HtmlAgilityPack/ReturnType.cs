// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.ReturnType
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>
  /// Specify which part of <see cref="T:HtmlAgilityPack.HtmlNode" /> is requested.
  /// </summary>
  public enum ReturnType
  {
    /// <summary>
    /// The text between the start and end tags of the object.
    /// </summary>
    InnerText,
    /// <summary>The HTML between the start and end tags of the object</summary>
    InnerHtml,
    /// <summary>The object and its content in HTML</summary>
    OuterHtml,
  }
}
