// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JArray
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a JSON array.</summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParseArray" title="Parsing a JSON Array from Text" />
  /// </example>
  public class JArray : 
    JContainer,
    IList<JToken>,
    ICollection<JToken>,
    IEnumerable<JToken>,
    IEnumerable
  {
    private readonly List<JToken> _values = new List<JToken>();

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" /> asynchronously.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous write operation.</returns>
    public override async Task WriteToAsync(
      JsonWriter writer,
      CancellationToken cancellationToken,
      params JsonConverter[] converters)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = writer.WriteStartArrayAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      for (int i = 0; i < this._values.Count; ++i)
      {
        configuredTaskAwaitable = this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      configuredTaskAwaitable = writer.WriteEndArrayAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    /// <summary>
    /// Asynchronously loads a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous load. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static Task<JArray> LoadAsync(
      JsonReader reader,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return JArray.LoadAsync(reader, (JsonLoadSettings) null, cancellationToken);
    }

    /// <summary>
    /// Asynchronously loads a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous load. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static async Task<JArray> LoadAsync(
      JsonReader reader,
      JsonLoadSettings? settings,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
      if (reader.TokenType == JsonToken.None)
      {
        configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        if (!await configuredTaskAwaitable)
          throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
      }
      configuredTaskAwaitable = reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
      int num = await configuredTaskAwaitable ? 1 : 0;
      if (reader.TokenType != JsonToken.StartArray)
        throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JArray a = new JArray();
      a.SetLineInfo(reader as IJsonLineInfo, settings);
      await a.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
      return a;
    }

    /// <summary>Gets the container's children tokens.</summary>
    /// <value>The container's children tokens.</value>
    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._values;

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public override JTokenType Type => JTokenType.Array;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JArray" /> class.
    /// </summary>
    public JArray()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JArray" /> class from another <see cref="T:Newtonsoft.Json.Linq.JArray" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JArray" /> object to copy from.</param>
    public JArray(JArray other)
      : base((JContainer) other)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JArray" /> class with the specified content.
    /// </summary>
    /// <param name="content">The contents of the array.</param>
    public JArray(params object[] content)
      : this((object) content)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JArray" /> class with the specified content.
    /// </summary>
    /// <param name="content">The contents of the array.</param>
    public JArray(object content) => this.Add(content);

    internal override bool DeepEquals(JToken node) => node is JArray jarray && this.ContentsEqual((JContainer) jarray);

    internal override JToken CloneToken() => (JToken) new JArray(this);

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JArray Load(JsonReader reader) => JArray.Load(reader, (JsonLoadSettings) null);

    /// <summary>
    /// Loads an <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JArray Load(JsonReader reader, JsonLoadSettings? settings)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
      reader.MoveToContent();
      if (reader.TokenType != JsonToken.StartArray)
        throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JArray jarray = new JArray();
      jarray.SetLineInfo(reader as IJsonLineInfo, settings);
      jarray.ReadTokenFrom(reader, settings);
      return jarray;
    }

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> populated from the string that contains JSON.</returns>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParseArray" title="Parsing a JSON Array from Text" />
    /// </example>
    public static JArray Parse(string json) => JArray.Parse(json, (JsonLoadSettings) null);

    /// <summary>
    /// Load a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from a string that contains JSON.
    /// </summary>
    /// <param name="json">A <see cref="T:System.String" /> that contains JSON.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> populated from the string that contains JSON.</returns>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParseArray" title="Parsing a JSON Array from Text" />
    /// </example>
    public static JArray Parse(string json, JsonLoadSettings? settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
      {
        JArray jarray = JArray.Load(reader, settings);
        do
          ;
        while (reader.Read());
        return jarray;
      }
    }

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from an object.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> with the values of the specified object.</returns>
    public static JArray FromObject(object o) => JArray.FromObject(o, JsonSerializer.CreateDefault());

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JArray" /> from an object.
    /// </summary>
    /// <param name="o">The object that will be used to create <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <param name="jsonSerializer">The <see cref="T:Newtonsoft.Json.JsonSerializer" /> that will be used to read the object.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JArray" /> with the values of the specified object.</returns>
    public static JArray FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      return jtoken.Type == JTokenType.Array ? (JArray) jtoken : throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
    }

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartArray();
      for (int index = 0; index < this._values.Count; ++index)
        this._values[index].WriteTo(writer, converters);
      writer.WriteEndArray();
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.
    /// </summary>
    /// <value>The <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.</value>
    public override JToken? this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        return key is int index ? this.GetItem(index) : throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is int index))
          throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Newtonsoft.Json.Linq.JToken" /> at the specified index.
    /// </summary>
    /// <value></value>
    public JToken this[int index]
    {
      get => this.GetItem(index);
      set => this.SetItem(index, value);
    }

    internal override int IndexOfItem(JToken? item) => item == null ? -1 : this._values.IndexOfReference<JToken>(item);

    internal override void MergeItem(object content, JsonMergeSettings? settings)
    {
      IEnumerable content1 = this.IsMultiContent(content) || content is JArray ? (IEnumerable) content : (IEnumerable) null;
      if (content1 == null)
        return;
      JContainer.MergeEnumerableContent((JContainer) this, content1, settings);
    }

    /// <summary>
    /// Determines the index of a specific item in the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <returns>
    /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
    /// </returns>
    public int IndexOf(JToken item) => this.IndexOfItem(item);

    /// <summary>
    /// Inserts an item to the <see cref="T:Newtonsoft.Json.Linq.JArray" /> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The object to insert into the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </exception>
    public void Insert(int index, JToken item) => this.InsertItem(index, item, false);

    /// <summary>
    /// Removes the <see cref="T:Newtonsoft.Json.Linq.JArray" /> item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </exception>
    public void RemoveAt(int index) => this.RemoveItemAt(index);

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<JToken> GetEnumerator() => this.Children().GetEnumerator();

    /// <summary>
    /// Adds an item to the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    public void Add(JToken item) => this.Add((object) item);

    /// <summary>
    /// Removes all items from the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </summary>
    public void Clear() => this.ClearItems();

    /// <summary>
    /// Determines whether the <see cref="T:Newtonsoft.Json.Linq.JArray" /> contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate in the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="item" /> is found in the <see cref="T:Newtonsoft.Json.Linq.JArray" />; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(JToken item) => this.ContainsItem(item);

    /// <summary>
    /// Copies the elements of the <see cref="T:Newtonsoft.Json.Linq.JArray" /> to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(JToken[] array, int arrayIndex) => this.CopyItemsTo((Array) array, arrayIndex);

    /// <summary>
    /// Gets a value indicating whether the <see cref="T:Newtonsoft.Json.Linq.JArray" /> is read-only.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="T:Newtonsoft.Json.Linq.JArray" /> is read-only; otherwise, <c>false</c>.</returns>
    public bool IsReadOnly => false;

    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="T:Newtonsoft.Json.Linq.JArray" />.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="item" /> was successfully removed from the <see cref="T:Newtonsoft.Json.Linq.JArray" />; otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="item" /> is not found in the original <see cref="T:Newtonsoft.Json.Linq.JArray" />.
    /// </returns>
    public bool Remove(JToken item) => this.RemoveItem(item);

    internal override int GetDeepHashCode() => this.ContentsHashCode();
  }
}
