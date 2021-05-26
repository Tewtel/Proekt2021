// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlElementFlag
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>Flags that describe the behavior of an Element node.</summary>
  [Flags]
  public enum HtmlElementFlag
  {
    /// <summary>The node is a CDATA node.</summary>
    CData = 1,
    /// <summary>
    /// The node is empty. META or IMG are example of such nodes.
    /// </summary>
    Empty = 2,
    /// <summary>The node will automatically be closed during parsing.</summary>
    Closed = 4,
    /// <summary>The node can overlap.</summary>
    CanOverlap = 8,
  }
}
