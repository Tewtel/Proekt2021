// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestHeadersExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;

namespace System.Net.Http
{
  /// <summary>Provides extension methods for the <see cref="T:System.Net.Http.Headers.HttpRequestHeaders" /> class.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpRequestHeadersExtensions
  {
    private const string Cookie = "Cookie";

    /// <summary>Gets any cookie headers present in the request.</summary>
    /// <returns>A collection of <see cref="T:System.Net.Http.Headers.CookieHeaderValue" /> instances.</returns>
    /// <param name="headers">The request headers.</param>
    public static Collection<CookieHeaderValue> GetCookies(
      this HttpRequestHeaders headers)
    {
      if (headers == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (headers));
      Collection<CookieHeaderValue> collection = new Collection<CookieHeaderValue>();
      IEnumerable<string> values;
      if (headers.TryGetValues("Cookie", out values))
      {
        foreach (string input in values)
        {
          CookieHeaderValue parsedValue;
          if (CookieHeaderValue.TryParse(input, out parsedValue))
            collection.Add(parsedValue);
        }
      }
      return collection;
    }

    /// <summary>Gets any cookie headers present in the request that contain a cookie state whose name that matches the specified value.</summary>
    /// <returns>A collection of <see cref="T:System.Net.Http.Headers.CookieHeaderValue" /> instances.</returns>
    /// <param name="headers">The request headers.</param>
    /// <param name="name">The cookie state name to match.</param>
    public static Collection<CookieHeaderValue> GetCookies(
      this HttpRequestHeaders headers,
      string name)
    {
      if (name == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (name));
      return new Collection<CookieHeaderValue>((IList<CookieHeaderValue>) headers.GetCookies().Where<CookieHeaderValue>((Func<CookieHeaderValue, bool>) (header => header.Cookies.Any<CookieState>((Func<CookieState, bool>) (state => string.Equals(state.Name, name, StringComparison.OrdinalIgnoreCase))))).ToArray<CookieHeaderValue>());
    }
  }
}
