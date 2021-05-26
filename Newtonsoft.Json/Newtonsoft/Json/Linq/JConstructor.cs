// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JConstructor
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a JSON constructor.</summary>
  public class JConstructor : JContainer
  {
    private string? _name;
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
      await writer.WriteStartConstructorAsync(this._name ?? string.Empty, cancellationToken).ConfigureAwait(false);
      for (int i = 0; i < this._values.Count; ++i)
        await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
      await writer.WriteEndConstructorAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously loads a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous load. The <see cref="P:System.Threading.Tasks.Task`1.Result" />
    /// property returns a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static Task<JConstructor> LoadAsync(
      JsonReader reader,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return JConstructor.LoadAsync(reader, (JsonLoadSettings) null, cancellationToken);
    }

    /// <summary>
    /// Asynchronously loads a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous load. The <see cref="P:System.Threading.Tasks.Task`1.Result" />
    /// property returns a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static async Task<JConstructor> LoadAsync(
      JsonReader reader,
      JsonLoadSettings? settings,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
      if (reader.TokenType == JsonToken.None)
      {
        configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        if (!await configuredTaskAwaitable)
          throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      }
      configuredTaskAwaitable = reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
      int num = await configuredTaskAwaitable ? 1 : 0;
      JConstructor c = reader.TokenType == JsonToken.StartConstructor ? new JConstructor((string) reader.Value) : throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      c.SetLineInfo(reader as IJsonLineInfo, settings);
      await c.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
      return c;
    }

    /// <summary>Gets the container's children tokens.</summary>
    /// <value>The container's children tokens.</value>
    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._values;

    internal override int IndexOfItem(JToken? item) => item == null ? -1 : this._values.IndexOfReference<JToken>(item);

    internal override void MergeItem(object content, JsonMergeSettings? settings)
    {
      if (!(content is JConstructor jconstructor))
        return;
      if (jconstructor.Name != null)
        this.Name = jconstructor.Name;
      JContainer.MergeEnumerableContent((JContainer) this, (IEnumerable) jconstructor, settings);
    }

    /// <summary>Gets or sets the name of this constructor.</summary>
    /// <value>The constructor name.</value>
    public string? Name
    {
      get => this._name;
      set => this._name = value;
    }

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public override JTokenType Type => JTokenType.Constructor;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class.
    /// </summary>
    public JConstructor()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class from another <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> object to copy from.</param>
    public JConstructor(JConstructor other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name and content.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    /// <param name="content">The contents of the constructor.</param>
    public JConstructor(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name and content.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    /// <param name="content">The contents of the constructor.</param>
    public JConstructor(string name, object content)
      : this(name)
    {
      this.Add(content);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> class with the specified name.
    /// </summary>
    /// <param name="name">The constructor name.</param>
    public JConstructor(string name)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException("Constructor name cannot be empty.", nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          this._name = name;
          break;
      }
    }

    internal override bool DeepEquals(JToken node) => node is JConstructor jconstructor && this._name == jconstructor.Name && this.ContentsEqual((JContainer) jconstructor);

    internal override JToken CloneToken() => (JToken) new JConstructor(this);

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartConstructor(this._name);
      int count = this._values.Count;
      for (int index = 0; index < count; ++index)
        this._values[index].WriteTo(writer, converters);
      writer.WriteEndConstructor();
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
        return key is int index ? this.GetItem(index) : throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is int index))
          throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    internal override int GetDeepHashCode()
    {
      string name = this._name;
      return (name != null ? name.GetHashCode() : 0) ^ this.ContentsHashCode();
    }

    /// <summary>
    /// Loads a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JConstructor Load(JsonReader reader) => JConstructor.Load(reader, (JsonLoadSettings) null);

    /// <summary>
    /// Loads a <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />.
    /// </summary>
    /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JConstructor" />.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
    /// If this is <c>null</c>, default load settings will be used.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JConstructor" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
    public static JConstructor Load(JsonReader reader, JsonLoadSettings? settings)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      reader.MoveToContent();
      JConstructor jconstructor = reader.TokenType == JsonToken.StartConstructor ? new JConstructor((string) reader.Value) : throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
      jconstructor.ReadTokenFrom(reader, settings);
      return jconstructor;
    }
  }
}
