// Decompiled with JetBrains decompiler
// Type: System.Net.Http.FormattingUtilities
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Net.Http
{
  internal static class FormattingUtilities
  {
    private static readonly string[] dateFormats = new string[16]
    {
      "ddd, d MMM yyyy H:m:s 'GMT'",
      "ddd, d MMM yyyy H:m:s",
      "d MMM yyyy H:m:s 'GMT'",
      "d MMM yyyy H:m:s",
      "ddd, d MMM yy H:m:s 'GMT'",
      "ddd, d MMM yy H:m:s",
      "d MMM yy H:m:s 'GMT'",
      "d MMM yy H:m:s",
      "dddd, d'-'MMM'-'yy H:m:s 'GMT'",
      "dddd, d'-'MMM'-'yy H:m:s",
      "ddd, d'-'MMM'-'yyyy H:m:s 'GMT'",
      "ddd MMM d H:m:s yyyy",
      "ddd, d MMM yyyy H:m:s zzz",
      "ddd, d MMM yyyy H:m:s",
      "d MMM yyyy H:m:s zzz",
      "d MMM yyyy H:m:s"
    };
    private const string NonTokenChars = "()<>@,;:\\\"/[]?={}";
    public const double Match = 1.0;
    public const double NoMatch = 0.0;
    public const int DefaultMaxDepth = 256;
    public const int DefaultMinDepth = 1;
    public const string HttpRequestedWithHeader = "x-requested-with";
    public const string HttpRequestedWithHeaderValue = "XMLHttpRequest";
    public const string HttpHostHeader = "Host";
    public const string HttpVersionToken = "HTTP";
    public static readonly Type HttpRequestMessageType = typeof (HttpRequestMessage);
    public static readonly Type HttpResponseMessageType = typeof (HttpResponseMessage);
    public static readonly Type HttpContentType = typeof (HttpContent);
    public static readonly Type DelegatingEnumerableGenericType = typeof (DelegatingEnumerable<>);
    public static readonly Type EnumerableInterfaceGenericType = typeof (IEnumerable<>);
    public static readonly Type QueryableInterfaceGenericType = typeof (IQueryable<>);
    public static readonly XsdDataContractExporter XsdDataContractExporter = new XsdDataContractExporter();

    public static bool IsJTokenType(Type type) => typeof (JToken).IsAssignableFrom(type);

    public static HttpContentHeaders CreateEmptyContentHeaders()
    {
      HttpContent httpContent = (HttpContent) null;
      HttpContentHeaders httpContentHeaders = (HttpContentHeaders) null;
      try
      {
        httpContent = (HttpContent) new StringContent(string.Empty);
        httpContentHeaders = httpContent.Headers;
        httpContentHeaders.Clear();
      }
      finally
      {
        httpContent?.Dispose();
      }
      return httpContentHeaders;
    }

    public static XmlDictionaryReaderQuotas CreateDefaultReaderQuotas() => new XmlDictionaryReaderQuotas()
    {
      MaxArrayLength = int.MaxValue,
      MaxBytesPerRead = int.MaxValue,
      MaxDepth = 256,
      MaxNameTableCharCount = int.MaxValue,
      MaxStringContentLength = int.MaxValue
    };

    public static string UnquoteToken(string token) => string.IsNullOrWhiteSpace(token) || !token.StartsWith("\"", StringComparison.Ordinal) || (!token.EndsWith("\"", StringComparison.Ordinal) || token.Length <= 1) ? token : token.Substring(1, token.Length - 2);

    public static bool ValidateHeaderToken(string token)
    {
      if (token == null)
        return false;
      foreach (char ch in token)
      {
        if (ch < '!' || ch > '~' || "()<>@,;:\\\"/[]?={}".IndexOf(ch) != -1)
          return false;
      }
      return true;
    }

    public static string DateToString(DateTimeOffset dateTime) => dateTime.ToUniversalTime().ToString("r", (IFormatProvider) CultureInfo.InvariantCulture);

    public static bool TryParseDate(string input, out DateTimeOffset result) => DateTimeOffset.TryParseExact(input, FormattingUtilities.dateFormats, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out result);

    public static bool TryParseInt32(string value, out int result) => int.TryParse(value, NumberStyles.None, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result);
  }
}
