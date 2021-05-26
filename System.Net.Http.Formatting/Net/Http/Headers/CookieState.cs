// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Headers.CookieState
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Specialized;
using System.Net.Http.Formatting.Internal;
using System.Net.Http.Properties;
using System.Web.Http;

namespace System.Net.Http.Headers
{
  /// <summary>Contains cookie name and its associated cookie state.</summary>
  public class CookieState : ICloneable
  {
    private string _name;
    private NameValueCollection _values = (NameValueCollection) HttpValueCollection.Create();

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieState" /> class.</summary>
    /// <param name="name">The name of the cookie.</param>
    public CookieState(string name)
      : this(name, string.Empty)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieState" /> class.</summary>
    /// <param name="name">The name of the cookie.</param>
    /// <param name="value">The value of the cookie.</param>
    public CookieState(string name, string value)
    {
      CookieState.CheckNameFormat(name, nameof (name));
      this._name = name;
      CookieState.CheckValueFormat(value, nameof (value));
      this.Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.CookieState" /> class.</summary>
    /// <param name="name">The name of the cookie.</param>
    /// <param name="values">The collection of name-value pair for the cookie.</param>
    public CookieState(string name, NameValueCollection values)
    {
      CookieState.CheckNameFormat(name, nameof (name));
      this._name = name;
      if (values == null)
        throw Error.ArgumentNull(nameof (values));
      this.Values.Add(values);
    }

    private CookieState(CookieState source)
    {
      this._name = source._name;
      if (source._values == null)
        return;
      this.Values.Add(source._values);
    }

    /// <summary>Gets or sets the name of the cookie.</summary>
    /// <returns>The name of the cookie.</returns>
    public string Name
    {
      get => this._name;
      set
      {
        CookieState.CheckNameFormat(value, nameof (value));
        this._name = value;
      }
    }

    /// <summary>Gets or sets the cookie value, if cookie data is a simple string value.</summary>
    /// <returns>The value of the cookie. </returns>
    public string Value
    {
      get => this.Values.Count <= 0 ? string.Empty : this.Values.AllKeys[0];
      set
      {
        CookieState.CheckValueFormat(value, nameof (value));
        if (this.Values.Count > 0)
          this.Values.AllKeys[0] = value;
        else
          this.Values.Add(value, string.Empty);
      }
    }

    /// <summary>Gets or sets the collection of name-value pair, if the cookie data is structured.</summary>
    /// <returns>The collection of name-value pair for the cookie.</returns>
    public NameValueCollection Values => this._values;

    /// <summary>Gets or sets the cookie value with the specified cookie name, if the cookie data is structured.</summary>
    /// <returns>The cookie value with the specified cookie name.</returns>
    public string this[string subName]
    {
      get => this.Values[subName];
      set => this.Values[subName] = value;
    }

    /// <summary>Returns the string representation the current object.</summary>
    /// <returns>The string representation the current object.</returns>
    public override string ToString() => this._name + "=" + (this._values != null ? this._values.ToString() : string.Empty);

    /// <summary>Returns a new object that is a copy of the current instance.</summary>
    /// <returns>A new object that is a copy of the current instance.</returns>
    public object Clone() => (object) new CookieState(this);

    private static void CheckNameFormat(string name, string parameterName)
    {
      if (name == null)
        throw Error.ArgumentNull(nameof (name));
      if (!FormattingUtilities.ValidateHeaderToken(name))
        throw Error.Argument(parameterName, Resources.CookieInvalidName);
    }

    private static void CheckValueFormat(string value, string parameterName)
    {
      if (value == null)
        throw Error.ArgumentNull(parameterName);
    }
  }
}
