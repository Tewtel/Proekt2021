// Decompiled with JetBrains decompiler
// Type: System.Net.Http.UriExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Contains extension methods to allow strongly typed objects to be read from the query component of <see cref="T:System.Uri" /> instances. </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class UriExtensions
  {
    /// <summary>Parses the query portion of the specified URI.</summary>
    /// <returns>A  <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains the query parameters.</returns>
    /// <param name="address">The URI to parse.</param>
    public static NameValueCollection ParseQueryString(this Uri address) => !(address == (Uri) null) ? new FormDataCollection(address).ReadAsNameValueCollection() : throw Error.ArgumentNull(nameof (address));

    /// <summary>Reads HTML form URL encoded data provided in the <see cref="T:System.Uri" /> query component as a <see cref="T:Newtonsoft.Json.Linq.JObject" /> object.</summary>
    /// <returns>true if the query component can be read as <see cref="T:Newtonsoft.Json.Linq.JObject" />; otherwise false.</returns>
    /// <param name="address">The <see cref="T:System.Uri" /> instance from which to read.</param>
    /// <param name="value">An object to be initialized with this instance or null if the conversion cannot be performed.</param>
    public static bool TryReadQueryAsJson(this Uri address, out JObject value) => !(address == (Uri) null) ? FormUrlEncodedJson.TryParse((IEnumerable<KeyValuePair<string, string>>) new FormDataCollection(address), out value) : throw Error.ArgumentNull(nameof (address));

    /// <summary>Reads HTML form URL encoded data provided in the URI query string as an object of a specified type.</summary>
    /// <returns>true if the query component of the URI can be read as the specified type; otherwise, false.</returns>
    /// <param name="address">The URI to read.</param>
    /// <param name="type">The type of object to read.</param>
    /// <param name="value">When this method returns, contains an object that is initialized from the query component of the URI. This parameter is treated as uninitialized.</param>
    public static bool TryReadQueryAs(this Uri address, Type type, out object value)
    {
      if (address == (Uri) null)
        throw Error.ArgumentNull(nameof (address));
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      JObject jobject;
      if (FormUrlEncodedJson.TryParse((IEnumerable<KeyValuePair<string, string>>) new FormDataCollection(address), out jobject))
      {
        using (JTokenReader jtokenReader = new JTokenReader((JToken) jobject))
          value = new JsonSerializer().Deserialize((JsonReader) jtokenReader, type);
        return true;
      }
      value = (object) null;
      return false;
    }

    /// <summary>Reads HTML form URL encoded data provided in the URI query string as an object of a specified type.</summary>
    /// <returns>true if the query component of the URI can be read as the specified type; otherwise, false.</returns>
    /// <param name="address">The URI to read.</param>
    /// <param name="value">When this method returns, contains an object that is initialized from the query component of the URI. This parameter is treated as uninitialized.</param>
    /// <typeparam name="T">The type of object to read.</typeparam>
    public static bool TryReadQueryAs<T>(this Uri address, out T value)
    {
      if (address == (Uri) null)
        throw Error.ArgumentNull(nameof (address));
      JObject jobject;
      if (FormUrlEncodedJson.TryParse((IEnumerable<KeyValuePair<string, string>>) new FormDataCollection(address), out jobject))
      {
        value = jobject.ToObject<T>();
        return true;
      }
      value = default (T);
      return false;
    }
  }
}
