// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Headers.CookieHeaderValue
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;

namespace System.Net.Http.Headers
{
  /// <summary>Provides value for the cookie header.</summary>
  public class CookieHeaderValue : ICloneable
  {
    private const string ExpiresToken = "expires";
    private const string MaxAgeToken = "max-age";
    private const string DomainToken = "domain";
    private const string PathToken = "path";
    private const string SecureToken = "secure";
    private const string HttpOnlyToken = "httponly";
    private const string DefaultPath = "/";
    private static readonly char[] segmentSeparator = new char[1]
    {
      ';'
    };
    private static readonly char[] nameValueSeparator = new char[1]
    {
      '='
    };
    private Collection<CookieState> _cookies;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieHeaderValue" /> class.</summary>
    /// <param name="name">The value of the name.</param>
    /// <param name="value">The value.</param>
    public CookieHeaderValue(string name, string value) => this.Cookies.Add(new CookieState(name, value));

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieHeaderValue" /> class.</summary>
    /// <param name="name">The value of the name.</param>
    /// <param name="values">The values.</param>
    public CookieHeaderValue(string name, NameValueCollection values) => this.Cookies.Add(new CookieState(name, values));

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieHeaderValue" /> class.</summary>
    protected CookieHeaderValue()
    {
    }

    private CookieHeaderValue(CookieHeaderValue source)
    {
      this.Expires = source != null ? source.Expires : throw System.Web.Http.Error.ArgumentNull(nameof (source));
      this.MaxAge = source.MaxAge;
      this.Domain = source.Domain;
      this.Path = source.Path;
      this.Secure = source.Secure;
      this.HttpOnly = source.HttpOnly;
      foreach (CookieState cookie in source.Cookies)
        this.Cookies.Add(cookie.Clone<CookieState>());
    }

    /// <summary>Gets a collection of cookies sent by the client.</summary>
    /// <returns>A collection object representing the client’s cookie variables.</returns>
    public Collection<CookieState> Cookies
    {
      get
      {
        if (this._cookies == null)
          this._cookies = new Collection<CookieState>();
        return this._cookies;
      }
    }

    /// <summary>Gets or sets the expiration date and time for the cookie.</summary>
    /// <returns>The time of day (on the client) at which the cookie expires.</returns>
    public DateTimeOffset? Expires { get; set; }

    /// <summary>Gets or sets the maximum age permitted for a resource.</summary>
    /// <returns>The maximum age permitted for a resource.</returns>
    public TimeSpan? MaxAge { get; set; }

    /// <summary>Gets or sets the domain to associate the cookie with.</summary>
    /// <returns>The name of the domain to associate the cookie with.</returns>
    public string Domain { get; set; }

    /// <summary>Gets or sets the virtual path to transmit with the current cookie.</summary>
    /// <returns>The virtual path to transmit with the cookie.</returns>
    public string Path { get; set; }

    /// <summary>Gets or sets a value indicating whether to transmit the cookie using Secure Sockets Layer (SSL)—that is, over HTTPS only.</summary>
    /// <returns>true to transmit the cookie over an SSL connection (HTTPS); otherwise, false.</returns>
    public bool Secure { get; set; }

    /// <summary>Gets or sets a value that specifies whether a cookie is accessible by client-side script.</summary>
    /// <returns>true if the cookie has the HttpOnly attribute and cannot be accessed through a client-side script; otherwise, false.</returns>
    public bool HttpOnly { get; set; }

    /// <summary>Gets a shortcut to the cookie property.</summary>
    /// <returns>The cookie value.</returns>
    public CookieState this[string name]
    {
      get
      {
        if (string.IsNullOrEmpty(name))
          return (CookieState) null;
        CookieState cookieState = this.Cookies.FirstOrDefault<CookieState>((Func<CookieState, bool>) (c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)));
        if (cookieState == null)
        {
          cookieState = new CookieState(name, string.Empty);
          this.Cookies.Add(cookieState);
        }
        return cookieState;
      }
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
      StringBuilder builder1 = new StringBuilder();
      bool first = true;
      foreach (CookieState cookie in this.Cookies)
        first = CookieHeaderValue.AppendSegment(builder1, first, cookie.ToString(), (string) null);
      DateTimeOffset? expires = this.Expires;
      if (expires.HasValue)
      {
        StringBuilder builder2 = builder1;
        int num = first ? 1 : 0;
        expires = this.Expires;
        string str = FormattingUtilities.DateToString(expires.Value);
        first = CookieHeaderValue.AppendSegment(builder2, num != 0, "expires", str);
      }
      TimeSpan? maxAge = this.MaxAge;
      if (maxAge.HasValue)
      {
        StringBuilder builder2 = builder1;
        int num = first ? 1 : 0;
        maxAge = this.MaxAge;
        string str = ((int) maxAge.Value.TotalSeconds).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        first = CookieHeaderValue.AppendSegment(builder2, num != 0, "max-age", str);
      }
      if (this.Domain != null)
        first = CookieHeaderValue.AppendSegment(builder1, first, "domain", this.Domain);
      if (this.Path != null)
        first = CookieHeaderValue.AppendSegment(builder1, first, "path", this.Path);
      if (this.Secure)
        first = CookieHeaderValue.AppendSegment(builder1, first, "secure", (string) null);
      if (this.HttpOnly)
        CookieHeaderValue.AppendSegment(builder1, first, "httponly", (string) null);
      return builder1.ToString();
    }

    /// <summary>Creates a shallow copy of the cookie value.</summary>
    /// <returns>A shallow copy of the cookie value.</returns>
    public object Clone() => (object) new CookieHeaderValue(this);

    /// <summary>Indicates a value whether the string representation will be converted.</summary>
    /// <returns>true if the string representation will be converted; otherwise, false.</returns>
    /// <param name="input">The input value.</param>
    /// <param name="parsedValue">The parsed value to convert.</param>
    public static bool TryParse(string input, out CookieHeaderValue parsedValue)
    {
      parsedValue = (CookieHeaderValue) null;
      if (string.IsNullOrEmpty(input))
        return false;
      string[] strArray = input.Split(CookieHeaderValue.segmentSeparator);
      CookieHeaderValue instance = new CookieHeaderValue();
      foreach (string segment in strArray)
      {
        if (!CookieHeaderValue.ParseCookieSegment(instance, segment))
          return false;
      }
      if (instance.Cookies.Count == 0)
        return false;
      parsedValue = instance;
      return true;
    }

    private static bool AppendSegment(
      StringBuilder builder,
      bool first,
      string name,
      string value)
    {
      if (first)
        first = false;
      else
        builder.Append("; ");
      builder.Append(name);
      if (value != null)
      {
        builder.Append("=");
        builder.Append(value);
      }
      return first;
    }

    private static bool ParseCookieSegment(CookieHeaderValue instance, string segment)
    {
      if (string.IsNullOrWhiteSpace(segment))
        return true;
      string[] nameValuePair = segment.Split(CookieHeaderValue.nameValueSeparator, 2);
      if (nameValuePair.Length < 1 || string.IsNullOrWhiteSpace(nameValuePair[0]))
        return false;
      string str = nameValuePair[0].Trim();
      if (string.Equals(str, "expires", StringComparison.OrdinalIgnoreCase))
      {
        DateTimeOffset result;
        if (!FormattingUtilities.TryParseDate(CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null), out result))
          return false;
        instance.Expires = new DateTimeOffset?(result);
        return true;
      }
      if (string.Equals(str, "max-age", StringComparison.OrdinalIgnoreCase))
      {
        int result;
        if (!FormattingUtilities.TryParseInt32(CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null), out result))
          return false;
        instance.MaxAge = new TimeSpan?(new TimeSpan(0, 0, result));
        return true;
      }
      if (string.Equals(str, "domain", StringComparison.OrdinalIgnoreCase))
      {
        instance.Domain = CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null);
        return true;
      }
      if (string.Equals(str, "path", StringComparison.OrdinalIgnoreCase))
      {
        instance.Path = CookieHeaderValue.GetSegmentValue(nameValuePair, "/");
        return true;
      }
      if (string.Equals(str, "secure", StringComparison.OrdinalIgnoreCase))
      {
        if (!string.IsNullOrWhiteSpace(CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null)))
          return false;
        instance.Secure = true;
        return true;
      }
      if (string.Equals(str, "httponly", StringComparison.OrdinalIgnoreCase))
      {
        if (!string.IsNullOrWhiteSpace(CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null)))
          return false;
        instance.HttpOnly = true;
        return true;
      }
      string segmentValue = CookieHeaderValue.GetSegmentValue(nameValuePair, (string) null);
      try
      {
        NameValueCollection values = new FormDataCollection(segmentValue).ReadAsNameValueCollection();
        CookieState cookieState = new CookieState(str, values);
        instance.Cookies.Add(cookieState);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private static string GetSegmentValue(string[] nameValuePair, string defaultValue) => nameValuePair.Length <= 1 ? defaultValue : FormattingUtilities.UnquoteToken(nameValuePair[1]);
  }
}
