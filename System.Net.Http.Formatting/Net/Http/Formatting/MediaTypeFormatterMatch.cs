// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeFormatterMatch
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> This class describes how well a particular <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> matches a request. </summary>
  public class MediaTypeFormatterMatch
  {
    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatterMatch" /> class. </summary>
    /// <param name="formatter">The matching formatter.</param>
    /// <param name="mediaType">The media type. Can be null in which case the media type application/octet-stream is used.</param>
    /// <param name="quality">The quality of the match. Can be null in which case it is considered a full match with a value of 1.0</param>
    /// <param name="ranking">The kind of match.</param>
    public MediaTypeFormatterMatch(
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType,
      double? quality,
      MediaTypeFormatterMatchRanking ranking)
    {
      this.Formatter = formatter != null ? formatter : throw Error.ArgumentNull(nameof (formatter));
      this.MediaType = mediaType != null ? mediaType.Clone<MediaTypeHeaderValue>() : MediaTypeConstants.ApplicationOctetStreamMediaType;
      this.Quality = quality ?? 1.0;
      this.Ranking = ranking;
    }

    /// <summary> Gets the media type formatter. </summary>
    public MediaTypeFormatter Formatter { get; private set; }

    /// <summary> Gets the matched media type. </summary>
    public MediaTypeHeaderValue MediaType { get; private set; }

    /// <summary> Gets the quality of the match </summary>
    public double Quality { get; private set; }

    /// <summary> Gets the kind of match that occurred. </summary>
    public MediaTypeFormatterMatchRanking Ranking { get; private set; }
  }
}
