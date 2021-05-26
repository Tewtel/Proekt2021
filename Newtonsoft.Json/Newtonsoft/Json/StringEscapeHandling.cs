// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.StringEscapeHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how strings are escaped when writing JSON text.
  /// </summary>
  public enum StringEscapeHandling
  {
    /// <summary>Only control characters (e.g. newline) are escaped.</summary>
    Default,
    /// <summary>
    /// All non-ASCII and control characters (e.g. newline) are escaped.
    /// </summary>
    EscapeNonAscii,
    /// <summary>
    /// HTML (&lt;, &gt;, &amp;, ', ") and control characters (e.g. newline) are escaped.
    /// </summary>
    EscapeHtml,
  }
}
