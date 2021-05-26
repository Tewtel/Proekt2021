// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlParseErrorCode
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>Represents the type of parsing error.</summary>
  public enum HtmlParseErrorCode
  {
    /// <summary>A tag was not closed.</summary>
    TagNotClosed,
    /// <summary>A tag was not opened.</summary>
    TagNotOpened,
    /// <summary>
    /// There is a charset mismatch between stream and declared (META) encoding.
    /// </summary>
    CharsetMismatch,
    /// <summary>An end tag was not required.</summary>
    EndTagNotRequired,
    /// <summary>An end tag is invalid at this position.</summary>
    EndTagInvalidHere,
  }
}
