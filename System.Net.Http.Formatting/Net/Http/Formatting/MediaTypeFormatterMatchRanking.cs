// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeFormatterMatchRanking
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

namespace System.Net.Http.Formatting
{
  /// <summary> Contains information about the degree to which a <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> matches the   explicit or implicit preferences found in an incoming request. </summary>
  public enum MediaTypeFormatterMatchRanking
  {
    /// <summary> No match was found </summary>
    None,
    /// <summary> Matched on a type, meaning that the formatter is able to serialize the type.</summary>
    MatchOnCanWriteType,
    /// <summary>Matched on an explicit literal accept header, such as “application/json”.</summary>
    MatchOnRequestAcceptHeaderLiteral,
    /// <summary>Matched on an explicit subtype range in an Accept header, such as “application/*”.</summary>
    MatchOnRequestAcceptHeaderSubtypeMediaRange,
    /// <summary>Matched on an explicit “*/*” range in the Accept header.</summary>
    MatchOnRequestAcceptHeaderAllMediaRange,
    /// <summary> Matched on <see cref="T:System.Net.Http.HttpRequestMessage" /> after having applied the various <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" />s. </summary>
    MatchOnRequestWithMediaTypeMapping,
    /// <summary> Matched on the media type of the entity body in the HTTP request message.</summary>
    MatchOnRequestMediaType,
  }
}
