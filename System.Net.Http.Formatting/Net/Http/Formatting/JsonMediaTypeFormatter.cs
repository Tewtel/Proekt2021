// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.JsonMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Net.Http.Properties;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace System.Net.Http.Formatting
{
  /// <summary>Represents the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> class to handle JSON. </summary>
  public class JsonMediaTypeFormatter : BaseJsonMediaTypeFormatter
  {
    private ConcurrentDictionary<Type, DataContractJsonSerializer> _dataContractSerializerCache = new ConcurrentDictionary<Type, DataContractJsonSerializer>();
    private XmlDictionaryReaderQuotas _readerQuotas = FormattingUtilities.CreateDefaultReaderQuotas();
    private RequestHeaderMapping _requestHeaderMapping;

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> class. </summary>
    public JsonMediaTypeFormatter()
    {
      this.SupportedMediaTypes.Add(MediaTypeConstants.ApplicationJsonMediaType);
      this.SupportedMediaTypes.Add(MediaTypeConstants.TextJsonMediaType);
      this._requestHeaderMapping = (RequestHeaderMapping) new XmlHttpRequestHeaderMapping();
      this.MediaTypeMappings.Add((MediaTypeMapping) this._requestHeaderMapping);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> instance to copy settings from.</param>
    protected JsonMediaTypeFormatter(JsonMediaTypeFormatter formatter)
      : base((BaseJsonMediaTypeFormatter) formatter)
    {
      this.UseDataContractJsonSerializer = formatter.UseDataContractJsonSerializer;
      this.Indent = formatter.Indent;
    }

    /// <summary>Gets the default media type for JSON, namely "application/json".</summary>
    /// <returns>The <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> for JSON.</returns>
    public static MediaTypeHeaderValue DefaultMediaType => MediaTypeConstants.ApplicationJsonMediaType;

    /// <summary> Gets or sets a value indicating whether to use <see cref="T:System.Runtime.Serialization.Json.DataContractJsonSerializer" /> by default. </summary>
    /// <returns>true if to <see cref="T:System.Runtime.Serialization.Json.DataContractJsonSerializer" /> by default; otherwise, false.</returns>
    public bool UseDataContractJsonSerializer { get; set; }

    /// <summary> Gets or sets a value indicating whether to indent elements when writing data.  </summary>
    /// <returns>true if to indent elements when writing data; otherwise, false.</returns>
    public bool Indent { get; set; }

    /// <summary>Gets or sets the maximum depth allowed by this formatter.</summary>
    /// <returns>The maximum depth allowed by this formatter.</returns>
    public override sealed int MaxDepth
    {
      get => base.MaxDepth;
      set
      {
        base.MaxDepth = value;
        this._readerQuotas.MaxDepth = value;
      }
    }

    /// <summary>Called during deserialization to get the <see cref="T:Newtonsoft.Json.JsonReader" />.</summary>
    /// <returns>The reader to use during deserialization.</returns>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="effectiveEncoding">The encoding to use when reading.</param>
    public override JsonReader CreateJsonReader(
      Type type,
      Stream readStream,
      Encoding effectiveEncoding)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (readStream == null)
        throw Error.ArgumentNull(nameof (readStream));
      return effectiveEncoding != null ? (JsonReader) new JsonTextReader((TextReader) new StreamReader(readStream, effectiveEncoding)) : throw Error.ArgumentNull(nameof (effectiveEncoding));
    }

    /// <summary>Called during serialization to get the <see cref="T:Newtonsoft.Json.JsonWriter" />.</summary>
    /// <returns>The writer to use during serialization.</returns>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="writeStream">The stream to write to.</param>
    /// <param name="effectiveEncoding">The encoding to use when writing.</param>
    public override JsonWriter CreateJsonWriter(
      Type type,
      Stream writeStream,
      Encoding effectiveEncoding)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (writeStream == null)
        throw Error.ArgumentNull(nameof (writeStream));
      JsonWriter jsonWriter = effectiveEncoding != null ? (JsonWriter) new JsonTextWriter((TextWriter) new StreamWriter(writeStream, effectiveEncoding)) : throw Error.ArgumentNull(nameof (effectiveEncoding));
      if (this.Indent)
        jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
      return jsonWriter;
    }

    /// <summary>Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can read objects of the specified <paramref name="type" />.</summary>
    /// <returns>true if objects of this <paramref name="type" /> can be read, otherwise false.</returns>
    /// <param name="type">The type of object that will be read.</param>
    public override bool CanReadType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return this.UseDataContractJsonSerializer ? this._dataContractSerializerCache.GetOrAdd(type, (Func<Type, DataContractJsonSerializer>) (t => this.CreateDataContractSerializer(t, false))) != null : base.CanReadType(type);
    }

    /// <summary>Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can write objects of the specified <paramref name="type" />.</summary>
    /// <returns>true if objects of this <paramref name="type" /> can be written, otherwise false.</returns>
    /// <param name="type">The type of object that will be written.</param>
    public override bool CanWriteType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (!this.UseDataContractJsonSerializer)
        return base.CanWriteType(type);
      MediaTypeFormatter.TryGetDelegatingTypeForIQueryableGenericOrSame(ref type);
      return this._dataContractSerializerCache.GetOrAdd(type, (Func<Type, DataContractJsonSerializer>) (t => this.CreateDataContractSerializer(t, false))) != null;
    }

    /// <summary>Called during deserialization to read an object of the specified type from the specified stream.</summary>
    /// <returns>The object that has been read.</returns>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="effectiveEncoding">The encoding to use when reading.</param>
    /// <param name="formatterLogger">The logger to log events to.</param>
    public override object ReadFromStream(
      Type type,
      Stream readStream,
      Encoding effectiveEncoding,
      IFormatterLogger formatterLogger)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (readStream == null)
        throw Error.ArgumentNull(nameof (readStream));
      if (effectiveEncoding == null)
        throw Error.ArgumentNull(nameof (effectiveEncoding));
      if (!this.UseDataContractJsonSerializer)
        return base.ReadFromStream(type, readStream, effectiveEncoding, formatterLogger);
      using (XmlReader jsonReader = (XmlReader) JsonReaderWriterFactory.CreateJsonReader((Stream) new NonClosingDelegatingStream(readStream), effectiveEncoding, this._readerQuotas, (OnXmlDictionaryReaderClose) null))
        return this.GetDataContractSerializer(type).ReadObject(jsonReader);
    }

    /// <summary>Called during serialization to write an object of the specified type to the specified stream.</summary>
    /// <returns>Returns <see cref="T:System.Threading.Tasks.Task" />.</returns>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="value">The object to write.</param>
    /// <param name="writeStream">The stream to write to.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
    /// <param name="transportContext">The transport context.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation.</param>
    public override Task WriteToStreamAsync(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      TransportContext transportContext,
      CancellationToken cancellationToken)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (writeStream == null)
        throw Error.ArgumentNull(nameof (writeStream));
      if (this.UseDataContractJsonSerializer && this.Indent)
        throw Error.NotSupported(Resources.UnsupportedIndent, (object) typeof (DataContractJsonSerializer));
      return base.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
    }

    /// <summary>Called during serialization to write an object of the specified type to the specified stream.</summary>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="value">The object to write.</param>
    /// <param name="writeStream">The stream to write to.</param>
    /// <param name="effectiveEncoding">The encoding to use when writing.</param>
    public override void WriteToStream(
      Type type,
      object value,
      Stream writeStream,
      Encoding effectiveEncoding)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (writeStream == null)
        throw Error.ArgumentNull(nameof (writeStream));
      if (effectiveEncoding == null)
        throw Error.ArgumentNull(nameof (effectiveEncoding));
      if (this.UseDataContractJsonSerializer)
      {
        if (MediaTypeFormatter.TryGetDelegatingTypeForIQueryableGenericOrSame(ref type) && value != null)
          value = MediaTypeFormatter.GetTypeRemappingConstructor(type).Invoke(new object[1]
          {
            value
          });
        using (XmlWriter jsonWriter = (XmlWriter) JsonReaderWriterFactory.CreateJsonWriter(writeStream, effectiveEncoding, false))
          this.GetDataContractSerializer(type).WriteObject(jsonWriter, value);
      }
      else
        base.WriteToStream(type, value, writeStream, effectiveEncoding);
    }

    private DataContractJsonSerializer CreateDataContractSerializer(
      Type type,
      bool throwOnError)
    {
      DataContractJsonSerializer contractJsonSerializer = (DataContractJsonSerializer) null;
      Exception innerException = (Exception) null;
      try
      {
        FormattingUtilities.XsdDataContractExporter.GetRootElementName(type);
        contractJsonSerializer = this.CreateDataContractSerializer(type);
      }
      catch (Exception ex)
      {
        innerException = ex;
      }
      if (!(contractJsonSerializer == null & throwOnError))
        return contractJsonSerializer;
      if (innerException != null)
        throw Error.InvalidOperation(innerException, Resources.SerializerCannotSerializeType, (object) typeof (DataContractJsonSerializer).Name, (object) type.Name);
      throw Error.InvalidOperation(Resources.SerializerCannotSerializeType, (object) typeof (DataContractJsonSerializer).Name, (object) type.Name);
    }

    /// <summary>Called during deserialization to get the <see cref="T:System.Runtime.Serialization.Json.DataContractJsonSerializer" />.</summary>
    /// <returns>The object used for serialization.</returns>
    /// <param name="type">The type of object that will be serialized or deserialized.</param>
    public virtual DataContractJsonSerializer CreateDataContractSerializer(
      Type type)
    {
      return !(type == (Type) null) ? new DataContractJsonSerializer(type) : throw Error.ArgumentNull(nameof (type));
    }

    private DataContractJsonSerializer GetDataContractSerializer(Type type) => this._dataContractSerializerCache.GetOrAdd(type, (Func<Type, DataContractJsonSerializer>) (t => this.CreateDataContractSerializer(type, true))) ?? throw Error.InvalidOperation(Resources.SerializerCannotSerializeType, (object) typeof (DataContractJsonSerializer).Name, (object) type.Name);
  }
}
