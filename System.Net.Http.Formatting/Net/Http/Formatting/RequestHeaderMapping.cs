// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.RequestHeaderMapping
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> This class provides a mapping from an arbitrary HTTP request header field to a <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> used to select <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> instances for handling the entity body of an <see cref="T:System.Net.Http.HttpRequestMessage" /> or <see cref="T:System.Net.Http.HttpResponseMessage" />. &lt;remarks&gt;This class only checks header fields associated with <see cref="M:HttpRequestMessage.Headers" /> for a match. It does not check header fields associated with <see cref="M:HttpResponseMessage.Headers" /> or <see cref="M:HttpContent.Headers" /> instances.&lt;/remarks&gt;</summary>
  public class RequestHeaderMapping : MediaTypeMapping
  {
    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.RequestHeaderMapping" /> class. </summary>
    /// <param name="headerName">Name of the header to match.</param>
    /// <param name="headerValue">The header value to match.</param>
    /// <param name="valueComparison">The value comparison to use when matching headerValue.</param>
    /// <param name="isValueSubstring">if set to true then headerValue is  considered a match if it matches a substring of the actual header value.</param>
    /// <param name="mediaType">The media type to use if headerName and headerValue  is considered a match.</param>
    public RequestHeaderMapping(
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring,
      string mediaType)
      : base(mediaType)
    {
      this.Initialize(headerName, headerValue, valueComparison, isValueSubstring);
    }

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.RequestHeaderMapping" /> class. </summary>
    /// <param name="headerName">Name of the header to match.</param>
    /// <param name="headerValue">The header value to match.</param>
    /// <param name="valueComparison">The <see cref="T:System.StringComparison" /> to use when matching headerValue.</param>
    /// <param name="isValueSubstring">if set to true then headerValue is  considered a match if it matches a substring of the actual header value.</param>
    /// <param name="mediaType">The <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> to use if headerName and headerValue  is considered a match.</param>
    public RequestHeaderMapping(
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring,
      MediaTypeHeaderValue mediaType)
      : base(mediaType)
    {
      this.Initialize(headerName, headerValue, valueComparison, isValueSubstring);
    }

    /// <summary> Gets the name of the header to match. </summary>
    public string HeaderName { get; private set; }

    /// <summary> Gets the header value to match. </summary>
    public string HeaderValue { get; private set; }

    /// <summary> Gets the <see cref="T:System.StringComparison" /> to use when matching <see cref="M:HeaderValue" />. </summary>
    public StringComparison HeaderValueComparison { get; private set; }

    /// <summary> Gets a value indicating whether <see cref="M:HeaderValue" /> is  a matched as a substring of the actual header value. this instance is value substring. </summary>
    /// <returns>true<see cref="P:System.Net.Http.Formatting.RequestHeaderMapping.HeaderValue" />false</returns>
    public bool IsValueSubstring { get; private set; }

    /// <summary> Returns a value indicating whether the current <see cref="T:System.Net.Http.Formatting.RequestHeaderMapping" /> instance can return a <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> from request. </summary>
    /// <returns> The quality of the match. It must be between 0.0 and 1.0. A value of 0.0 signifies no match. A value of 1.0 signifies a complete match. </returns>
    /// <param name="request">The <see cref="T:System.Net.Http.HttpRequestMessage" /> to check.</param>
    public override double TryMatchMediaType(HttpRequestMessage request)
    {
      if (request == null)
        throw Error.ArgumentNull(nameof (request));
      return RequestHeaderMapping.MatchHeaderValue(request, this.HeaderName, this.HeaderValue, this.HeaderValueComparison, this.IsValueSubstring);
    }

    private static double MatchHeaderValue(
      HttpRequestMessage request,
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring)
    {
      IEnumerable<string> values;
      if (request.Headers.TryGetValues(headerName, out values))
      {
        foreach (string str in values)
        {
          if (isValueSubstring)
          {
            if (str.IndexOf(headerValue, valueComparison) != -1)
              return 1.0;
          }
          else if (str.Equals(headerValue, valueComparison))
            return 1.0;
        }
      }
      return 0.0;
    }

    private void Initialize(
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring)
    {
      if (string.IsNullOrWhiteSpace(headerName))
        throw Error.ArgumentNull(nameof (headerName));
      if (string.IsNullOrWhiteSpace(headerValue))
        throw Error.ArgumentNull(nameof (headerValue));
      StringComparisonHelper.Validate(valueComparison, nameof (valueComparison));
      this.HeaderName = headerName;
      this.HeaderValue = headerValue;
      this.HeaderValueComparison = valueComparison;
      this.IsValueSubstring = isValueSubstring;
    }
  }
}
