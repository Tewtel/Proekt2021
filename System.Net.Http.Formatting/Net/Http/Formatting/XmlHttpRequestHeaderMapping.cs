// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.XmlHttpRequestHeaderMapping
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Linq;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  /// <summary> A <see cref="T:System.Net.Http.Formatting.RequestHeaderMapping" /> that maps the X-Requested-With http header field set by AJAX XmlHttpRequest (XHR) to the media type application/json if no explicit Accept header fields are present in the request. </summary>
  public class XmlHttpRequestHeaderMapping : RequestHeaderMapping
  {
    /// <summary> Initializes a new instance of <see cref="T:System.Net.Http.Formatting.XmlHttpRequestHeaderMapping" /> class </summary>
    public XmlHttpRequestHeaderMapping()
      : base("x-requested-with", "XMLHttpRequest", StringComparison.OrdinalIgnoreCase, true, MediaTypeConstants.ApplicationJsonMediaType)
    {
    }

    /// <summary> Returns a value indicating whether the current <see cref="T:System.Net.Http.Formatting.RequestHeaderMapping" /> instance can return a <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> from request. </summary>
    /// <returns> The quality of the match. A value of 0.0 signifies no match. A value of 1.0 signifies a complete match and that the request was made using XmlHttpRequest without an Accept header. </returns>
    /// <param name="request">The <see cref="T:System.Net.Http.HttpRequestMessage" /> to check.</param>
    public override double TryMatchMediaType(HttpRequestMessage request)
    {
      if (request == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (request));
      return request.Headers.Accept.Count == 0 || request.Headers.Accept.Count == 1 && request.Headers.Accept.First<MediaTypeWithQualityHeaderValue>().MediaType.Equals("*/*", StringComparison.Ordinal) ? base.TryMatchMediaType(request) : 0.0;
    }
  }
}
