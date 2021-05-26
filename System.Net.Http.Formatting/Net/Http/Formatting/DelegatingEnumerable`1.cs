// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.DelegatingEnumerable`1
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Net.Http.Formatting
{
  /// <summary> Helper class to serialize &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt; types by delegating them through a concrete implementation."/&amp;gt;. </summary>
  /// <typeparam name="T">The interface implementing  to proxy.</typeparam>
  public sealed class DelegatingEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private IEnumerable<T> _source;

    /// <summary> Initialize a DelegatingEnumerable. This constructor is necessary for <see cref="T:System.Runtime.Serialization.DataContractSerializer" /> to work. </summary>
    public DelegatingEnumerable() => this._source = Enumerable.Empty<T>();

    /// <summary> Initialize a DelegatingEnumerable with an &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt;. This is a helper class to proxy &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt; interfaces for <see cref="T:System.Xml.Serialization.XmlSerializer" />. </summary>
    /// <param name="source">The &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt; instance to get the enumerator from.</param>
    public DelegatingEnumerable(IEnumerable<T> source) => this._source = source != null ? source : throw System.Web.Http.Error.ArgumentNull(nameof (source));

    /// <summary> Get the enumerator of the associated &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt;. </summary>
    /// <returns>The enumerator of the &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt; source.</returns>
    public IEnumerator<T> GetEnumerator() => this._source.GetEnumerator();

    /// <summary> This method is not implemented but is required method for serialization to work. Do not use. </summary>
    /// <param name="item">The item to add. Unused.</param>
    public void Add(object item) => throw new NotImplementedException();

    /// <summary> Get the enumerator of the associated &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt;. </summary>
    /// <returns>The enumerator of the &lt;see cref="T:System.Collections.Generic.IEnumerable`1" /&gt; source.</returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._source.GetEnumerator();
  }
}
