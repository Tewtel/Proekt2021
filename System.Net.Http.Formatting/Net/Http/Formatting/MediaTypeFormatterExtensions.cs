// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeFormatterExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.ComponentModel;
using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class MediaTypeFormatterExtensions
  {
    public static void AddQueryStringMapping(
      this MediaTypeFormatter formatter,
      string queryStringParameterName,
      string queryStringParameterValue,
      MediaTypeHeaderValue mediaType)
    {
      if (formatter == null)
        throw Error.ArgumentNull(nameof (formatter));
      QueryStringMapping queryStringMapping = new QueryStringMapping(queryStringParameterName, queryStringParameterValue, mediaType);
      formatter.MediaTypeMappings.Add((MediaTypeMapping) queryStringMapping);
    }

    public static void AddQueryStringMapping(
      this MediaTypeFormatter formatter,
      string queryStringParameterName,
      string queryStringParameterValue,
      string mediaType)
    {
      if (formatter == null)
        throw Error.ArgumentNull(nameof (formatter));
      QueryStringMapping queryStringMapping = new QueryStringMapping(queryStringParameterName, queryStringParameterValue, mediaType);
      formatter.MediaTypeMappings.Add((MediaTypeMapping) queryStringMapping);
    }

    public static void AddRequestHeaderMapping(
      this MediaTypeFormatter formatter,
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring,
      MediaTypeHeaderValue mediaType)
    {
      if (formatter == null)
        throw Error.ArgumentNull(nameof (formatter));
      RequestHeaderMapping requestHeaderMapping = new RequestHeaderMapping(headerName, headerValue, valueComparison, isValueSubstring, mediaType);
      formatter.MediaTypeMappings.Add((MediaTypeMapping) requestHeaderMapping);
    }

    public static void AddRequestHeaderMapping(
      this MediaTypeFormatter formatter,
      string headerName,
      string headerValue,
      StringComparison valueComparison,
      bool isValueSubstring,
      string mediaType)
    {
      if (formatter == null)
        throw Error.ArgumentNull(nameof (formatter));
      RequestHeaderMapping requestHeaderMapping = new RequestHeaderMapping(headerName, headerValue, valueComparison, isValueSubstring, mediaType);
      formatter.MediaTypeMappings.Add((MediaTypeMapping) requestHeaderMapping);
    }
  }
}
