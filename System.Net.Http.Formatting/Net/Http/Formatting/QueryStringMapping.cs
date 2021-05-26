// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.QueryStringMapping
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> Class that provides <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" />s from query strings. </summary>
  public class QueryStringMapping : MediaTypeMapping
  {
    private static readonly Type _queryStringMappingType = typeof (QueryStringMapping);

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.QueryStringMapping" /> class. </summary>
    /// <param name="queryStringParameterName">The name of the query string parameter to match, if present.</param>
    /// <param name="queryStringParameterValue">The value of the query string parameter specified by queryStringParameterName.</param>
    /// <param name="mediaType">The media type to use if the query parameter specified by queryStringParameterName is present and assigned the value specified by queryStringParameterValue.</param>
    public QueryStringMapping(
      string queryStringParameterName,
      string queryStringParameterValue,
      string mediaType)
      : base(mediaType)
    {
      this.Initialize(queryStringParameterName, queryStringParameterValue);
    }

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.QueryStringMapping" /> class. </summary>
    /// <param name="queryStringParameterName">The name of the query string parameter to match, if present.</param>
    /// <param name="queryStringParameterValue">The value of the query string parameter specified by queryStringParameterName.</param>
    /// <param name="mediaType">The <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> to use if the query parameter specified by queryStringParameterName is present and assigned the value specified by queryStringParameterValue.</param>
    public QueryStringMapping(
      string queryStringParameterName,
      string queryStringParameterValue,
      MediaTypeHeaderValue mediaType)
      : base(mediaType)
    {
      this.Initialize(queryStringParameterName, queryStringParameterValue);
    }

    /// <summary> Gets the query string parameter name. </summary>
    public string QueryStringParameterName { get; private set; }

    /// <summary> Gets the query string parameter value. </summary>
    public string QueryStringParameterValue { get; private set; }

    /// <summary> Returns a value indicating whether the current <see cref="T:System.Net.Http.Formatting.QueryStringMapping" /> instance can return a <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> from request. </summary>
    /// <returns>If this instance can produce a <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> from request it returns 1.0 otherwise 0.0.</returns>
    /// <param name="request">The <see cref="T:System.Net.Http.HttpRequestMessage" /> to check.</param>
    public override double TryMatchMediaType(HttpRequestMessage request)
    {
      if (request == null)
        throw Error.ArgumentNull(nameof (request));
      return !this.DoesQueryStringMatch(QueryStringMapping.GetQueryString(request.RequestUri)) ? 0.0 : 1.0;
    }

    private static NameValueCollection GetQueryString(Uri uri) => !(uri == (Uri) null) ? new FormDataCollection(uri).ReadAsNameValueCollection() : throw Error.InvalidOperation(Resources.NonNullUriRequiredForMediaTypeMapping, (object) QueryStringMapping._queryStringMappingType.Name);

    private void Initialize(string queryStringParameterName, string queryStringParameterValue)
    {
      if (string.IsNullOrWhiteSpace(queryStringParameterName))
        throw Error.ArgumentNull(nameof (queryStringParameterName));
      if (string.IsNullOrWhiteSpace(queryStringParameterValue))
        throw Error.ArgumentNull(nameof (queryStringParameterValue));
      this.QueryStringParameterName = queryStringParameterName.Trim();
      this.QueryStringParameterValue = queryStringParameterValue.Trim();
    }

    private bool DoesQueryStringMatch(NameValueCollection queryString)
    {
      if (queryString != null)
      {
        foreach (string allKey in queryString.AllKeys)
        {
          if (string.Equals(allKey, this.QueryStringParameterName, StringComparison.OrdinalIgnoreCase) && string.Equals(queryString[allKey], this.QueryStringParameterValue, StringComparison.OrdinalIgnoreCase))
            return true;
        }
      }
      return false;
    }
  }
}
