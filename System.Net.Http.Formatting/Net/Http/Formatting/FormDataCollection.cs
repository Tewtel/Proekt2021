// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.FormDataCollection
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Formatting.Internal;
using System.Net.Http.Formatting.Parsers;
using System.Net.Http.Properties;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>Represent the collection of form data.</summary>
  public class FormDataCollection : IEnumerable<KeyValuePair<string, string>>, IEnumerable
  {
    private readonly IEnumerable<KeyValuePair<string, string>> _pairs;
    private NameValueCollection _nameValueCollection;

    /// <summary>Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection" /> class.</summary>
    /// <param name="pairs">The pairs.</param>
    public FormDataCollection(IEnumerable<KeyValuePair<string, string>> pairs) => this._pairs = pairs != null ? pairs : throw Error.ArgumentNull(nameof (pairs));

    /// <summary>Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection" /> class.</summary>
    /// <param name="uri">The URI</param>
    public FormDataCollection(Uri uri)
    {
      string query = !(uri == (Uri) null) ? uri.Query : throw Error.ArgumentNull(nameof (uri));
      if (query != null && query.Length > 0 && query[0] == '?')
        query = query.Substring(1);
      this._pairs = FormDataCollection.ParseQueryString(query);
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection" /> class.</summary>
    /// <param name="query">The query.</param>
    public FormDataCollection(string query) => this._pairs = FormDataCollection.ParseQueryString(query);

    /// <summary>Gets values associated with a given key. If there are multiple values, they're concatenated.</summary>
    /// <returns>Values associated with a given key. If there are multiple values, they're concatenated.</returns>
    public string this[string name] => this.Get(name);

    private static IEnumerable<KeyValuePair<string, string>> ParseQueryString(
      string query)
    {
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
      if (string.IsNullOrWhiteSpace(query))
        return (IEnumerable<KeyValuePair<string, string>>) keyValuePairList;
      byte[] bytes = Encoding.UTF8.GetBytes(query);
      FormUrlEncodedParser urlEncodedParser = new FormUrlEncodedParser((ICollection<KeyValuePair<string, string>>) keyValuePairList, long.MaxValue);
      int num = 0;
      byte[] buffer = bytes;
      int length = bytes.Length;
      ref int local = ref num;
      if (urlEncodedParser.ParseBuffer(buffer, length, ref local, true) != ParserState.Done)
        throw Error.InvalidOperation(Resources.FormUrlEncodedParseError, (object) num);
      return (IEnumerable<KeyValuePair<string, string>>) keyValuePairList;
    }

    /// <summary>Reads the collection of form data as a collection of name value.</summary>
    /// <returns>The collection of form data as a collection of name value.</returns>
    public NameValueCollection ReadAsNameValueCollection()
    {
      if (this._nameValueCollection == null)
        Interlocked.Exchange<NameValueCollection>(ref this._nameValueCollection, (NameValueCollection) HttpValueCollection.Create((IEnumerable<KeyValuePair<string, string>>) this));
      return this._nameValueCollection;
    }

    /// <summary>Gets the collection of form data.</summary>
    /// <returns>The collection of form data.</returns>
    /// <param name="key">The key.</param>
    public string Get(string key) => this.ReadAsNameValueCollection().Get(key);

    /// <summary>Gets the values of the collection of form data.</summary>
    /// <returns>The values of the collection of form data.</returns>
    /// <param name="key">The key.</param>
    public string[] GetValues(string key) => this.ReadAsNameValueCollection().GetValues(key);

    /// <summary>Gets an enumerable that iterates through the collection.</summary>
    /// <returns>The enumerable that iterates through the collection.</returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => this._pairs.GetEnumerator();

    /// <summary>Gets an enumerable that iterates through the collection.</summary>
    /// <returns>The enumerable that iterates through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => this._pairs.GetEnumerator();
  }
}
