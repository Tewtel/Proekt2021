// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Internal.HttpValueCollection
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Properties;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Http;

namespace System.Net.Http.Formatting.Internal
{
  [Serializable]
  internal class HttpValueCollection : NameValueCollection
  {
    protected HttpValueCollection(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    private HttpValueCollection()
      : base((IEqualityComparer) StringComparer.OrdinalIgnoreCase)
    {
    }

    internal static HttpValueCollection Create() => new HttpValueCollection();

    internal static HttpValueCollection Create(
      IEnumerable<KeyValuePair<string, string>> pairs)
    {
      HttpValueCollection httpValueCollection = new HttpValueCollection();
      foreach (KeyValuePair<string, string> pair in pairs)
        httpValueCollection.Add(pair.Key, pair.Value);
      httpValueCollection.IsReadOnly = false;
      return httpValueCollection;
    }

    public override void Add(string name, string value)
    {
      HttpValueCollection.ThrowIfMaxHttpCollectionKeysExceeded(this.Count);
      name = name ?? string.Empty;
      value = value ?? string.Empty;
      base.Add(name, value);
    }

    public override string ToString() => this.ToString(true);

    private static void ThrowIfMaxHttpCollectionKeysExceeded(int count)
    {
      if (count >= MediaTypeFormatter.MaxHttpCollectionKeys)
        throw Error.InvalidOperation(Resources.MaxHttpCollectionKeyLimitReached, (object) MediaTypeFormatter.MaxHttpCollectionKeys, (object) typeof (MediaTypeFormatter));
    }

    private string ToString(bool urlEncode)
    {
      if (this.Count == 0)
        return string.Empty;
      StringBuilder builder = new StringBuilder();
      bool first = true;
      foreach (string name in (NameObjectCollectionBase) this)
      {
        string[] values = this.GetValues(name);
        if (values == null || values.Length == 0)
        {
          first = HttpValueCollection.AppendNameValuePair(builder, first, urlEncode, name, string.Empty);
        }
        else
        {
          foreach (string str in values)
            first = HttpValueCollection.AppendNameValuePair(builder, first, urlEncode, name, str);
        }
      }
      return builder.ToString();
    }

    private static bool AppendNameValuePair(
      StringBuilder builder,
      bool first,
      bool urlEncode,
      string name,
      string value)
    {
      string str1 = name ?? string.Empty;
      string str2 = urlEncode ? UriQueryUtility.UrlEncode(str1) : str1;
      string str3 = value ?? string.Empty;
      string str4 = urlEncode ? UriQueryUtility.UrlEncode(str3) : str3;
      if (first)
        first = false;
      else
        builder.Append("&");
      builder.Append(str2);
      if (!string.IsNullOrEmpty(str4))
      {
        builder.Append("=");
        builder.Append(str4);
      }
      return first;
    }
  }
}
