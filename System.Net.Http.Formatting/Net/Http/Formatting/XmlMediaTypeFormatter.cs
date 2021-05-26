// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.XmlMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Net.Http.Properties;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace System.Net.Http.Formatting
{
  /// <summary>
  /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> class to handle Xml. </summary>
  public class XmlMediaTypeFormatter : MediaTypeFormatter
  {
    private ConcurrentDictionary<Type, object> _serializerCache = new ConcurrentDictionary<Type, object>();
    private XmlDictionaryReaderQuotas _readerQuotas = FormattingUtilities.CreateDefaultReaderQuotas();

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> class.</summary>
    public XmlMediaTypeFormatter()
    {
      this.SupportedMediaTypes.Add(MediaTypeConstants.ApplicationXmlMediaType);
      this.SupportedMediaTypes.Add(MediaTypeConstants.TextXmlMediaType);
      this.SupportedEncodings.Add((Encoding) new UTF8Encoding(false, true));
      this.SupportedEncodings.Add((Encoding) new UnicodeEncoding(false, true, true));
      this.WriterSettings = new XmlWriterSettings()
      {
        OmitXmlDeclaration = true,
        CloseOutput = false,
        CheckCharacters = false
      };
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> instance to copy settings from.</param>
    protected XmlMediaTypeFormatter(XmlMediaTypeFormatter formatter)
      : base((MediaTypeFormatter) formatter)
    {
      this.UseXmlSerializer = formatter.UseXmlSerializer;
      this.WriterSettings = formatter.WriterSettings;
      this.MaxDepth = formatter.MaxDepth;
    }

    /// <summary>Gets the default media type for the XML formatter.</summary>
    /// <returns>The default media type, which is “application/xml”.</returns>
    public static MediaTypeHeaderValue DefaultMediaType => MediaTypeConstants.ApplicationXmlMediaType;

    /// <summary>Gets or sets a value indicating whether the XML formatter uses the <see cref="T:System.Xml.Serialization.XmlSerializer" /> as the default serializer, instead of  using the <see cref="T:System.Runtime.Serialization.DataContractSerializer" />.</summary>
    /// <returns>If true, the formatter uses the <see cref="T:System.Xml.Serialization.XmlSerializer" /> by default; otherwise, it uses the <see cref="T:System.Runtime.Serialization.DataContractSerializer" /> by default.</returns>
    [DefaultValue(false)]
    public bool UseXmlSerializer { get; set; }

    /// <summary>Gets or sets a value indicating whether to indent elements when writing data.</summary>
    /// <returns>true to indent elements; otherwise, false.</returns>
    public bool Indent
    {
      get => this.WriterSettings.Indent;
      set => this.WriterSettings.Indent = value;
    }

    /// <summary>Gets the settings to be used while writing.</summary>
    /// <returns>The settings to be used while writing.</returns>
    public XmlWriterSettings WriterSettings { get; private set; }

    /// <summary>Gets and sets the maximum nested node depth.</summary>
    /// <returns>The maximum nested node depth.</returns>
    public int MaxDepth
    {
      get => this._readerQuotas.MaxDepth;
      set => this._readerQuotas.MaxDepth = value >= 1 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 1);
    }

    /// <summary>Registers an <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> to read or write objects of a specified type.</summary>
    /// <param name="type">The type of object that will be serialized or deserialized with<paramref name="serializer" />.</param>
    /// <param name="serializer">The <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> instance.</param>
    public void SetSerializer(Type type, XmlObjectSerializer serializer) => this.VerifyAndSetSerializer(type, (object) serializer);

    /// <summary>Registers an <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> to read or write objects of a specified type.</summary>
    /// <param name="serializer">The <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> instance.</param>
    /// <typeparam name="T">The type of object that will be serialized or deserialized with<paramref name="serializer" />.</typeparam>
    public void SetSerializer<T>(XmlObjectSerializer serializer) => this.SetSerializer(typeof (T), serializer);

    /// <summary>Registers an <see cref="T:System.Xml.Serialization.XmlSerializer" /> to read or write objects of a specified type.</summary>
    /// <param name="type">The type of object that will be serialized or deserialized with<paramref name="serializer" />.</param>
    /// <param name="serializer">The <see cref="T:System.Xml.Serialization.XmlSerializer" /> instance.</param>
    public void SetSerializer(Type type, XmlSerializer serializer) => this.VerifyAndSetSerializer(type, (object) serializer);

    /// <summary>Registers an <see cref="T:System.Xml.Serialization.XmlSerializer" /> to read or write objects of a specified type.</summary>
    /// <param name="serializer">The <see cref="T:System.Xml.Serialization.XmlSerializer" /> instance.</param>
    /// <typeparam name="T">The type of object that will be serialized or deserialized with<paramref name="serializer" />.</typeparam>
    public void SetSerializer<T>(XmlSerializer serializer) => this.SetSerializer(typeof (T), serializer);

    /// <summary>Unregisters the serializer currently associated with the given type.</summary>
    /// <returns>true if a serializer was previously registered for the type; otherwise, false.</returns>
    /// <param name="type">The type of object whose serializer should be removed.</param>
    public bool RemoveSerializer(Type type) => !(type == (Type) null) ? this._serializerCache.TryRemove(type, out object _) : throw Error.ArgumentNull(nameof (type));

    /// <summary>Queries whether the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can deserializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can deserialize the type; otherwise, false.</returns>
    /// <param name="type">The type to deserialize.</param>
    public override bool CanReadType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return this.GetCachedSerializer(type, false) != null;
    }

    /// <summary>Queries whether the  <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can serializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can serialize the type; otherwise, false.</returns>
    /// <param name="type">The type to serialize.</param>
    public override bool CanWriteType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (this.UseXmlSerializer)
        MediaTypeFormatter.TryGetDelegatingTypeForIEnumerableGenericOrSame(ref type);
      else
        MediaTypeFormatter.TryGetDelegatingTypeForIQueryableGenericOrSame(ref type);
      return this.GetCachedSerializer(type, false) != null;
    }

    /// <summary> Called during deserialization to read an object of the specified type from the specified readStream. </summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> whose result will be the object instance that has been read.</returns>
    /// <param name="type">The type of object to read.</param>
    /// <param name="readStream">The <see cref="T:System.IO.Stream" /> from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being read.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    public override Task<object> ReadFromStreamAsync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (readStream == null)
        throw Error.ArgumentNull(nameof (readStream));
      try
      {
        return Task.FromResult<object>(this.ReadFromStream(type, readStream, content, formatterLogger));
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError<object>(ex);
      }
    }

    private object ReadFromStream(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger)
    {
      HttpContentHeaders httpContentHeaders = content == null ? (HttpContentHeaders) null : content.Headers;
      if (httpContentHeaders != null)
      {
        long? contentLength = httpContentHeaders.ContentLength;
        long num = 0;
        if (contentLength.GetValueOrDefault() == num & contentLength.HasValue)
          return MediaTypeFormatter.GetDefaultValueForType(type);
      }
      object deserializer = this.GetDeserializer(type, content);
      try
      {
        using (XmlReader xmlReader = this.CreateXmlReader(readStream, content))
        {
          switch (deserializer)
          {
            case XmlSerializer xmlSerializer3:
              return xmlSerializer3.Deserialize(xmlReader);
            case XmlObjectSerializer objectSerializer3:
label_8:
              return objectSerializer3.ReadObject(xmlReader);
            default:
              XmlMediaTypeFormatter.ThrowInvalidSerializerException(deserializer, "GetDeserializer");
              goto label_8;
          }
        }
      }
      catch (Exception ex)
      {
        if (formatterLogger == null)
        {
          throw;
        }
        else
        {
          formatterLogger.LogError(string.Empty, ex);
          return MediaTypeFormatter.GetDefaultValueForType(type);
        }
      }
    }

    /// <summary>Called during deserialization to get the XML serializer to use for deserializing objects.</summary>
    /// <returns>An instance of <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> or <see cref="T:System.Xml.Serialization.XmlSerializer" /> to use for deserializing the object.</returns>
    /// <param name="type">The type of object to deserialize.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being read.</param>
    protected internal virtual object GetDeserializer(Type type, HttpContent content) => this.GetSerializerForType(type);

    /// <summary>Called during deserialization to get the XML reader to use for reading objects from the stream.</summary>
    /// <returns>The <see cref="T:System.Xml.XmlReader" /> to use for reading objects.</returns>
    /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read from.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being read.</param>
    protected internal virtual XmlReader CreateXmlReader(
      Stream readStream,
      HttpContent content)
    {
      Encoding encoding = this.SelectCharacterEncoding(content == null ? (HttpContentHeaders) null : content.Headers);
      return (XmlReader) XmlDictionaryReader.CreateTextReader((Stream) new NonClosingDelegatingStream(readStream), encoding, this._readerQuotas, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>Called during serialization to write an object of the specified type to the specified writeStream.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.</returns>
    /// <param name="type">The type of object to write.</param>
    /// <param name="value">The object to write.</param>
    /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
    /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
    /// <param name="cancellationToken">The token to monitor cancellation.</param>
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
      if (cancellationToken.IsCancellationRequested)
        return TaskHelpers.Canceled();
      try
      {
        this.WriteToStream(type, value, writeStream, content);
        return TaskHelpers.Completed();
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError(ex);
      }
    }

    private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
    {
      if ((!this.UseXmlSerializer ? MediaTypeFormatter.TryGetDelegatingTypeForIQueryableGenericOrSame(ref type) : MediaTypeFormatter.TryGetDelegatingTypeForIEnumerableGenericOrSame(ref type)) && value != null)
        value = MediaTypeFormatter.GetTypeRemappingConstructor(type).Invoke(new object[1]
        {
          value
        });
      object serializer = this.GetSerializer(type, value, content);
      using (XmlWriter xmlWriter = this.CreateXmlWriter(writeStream, content))
      {
        switch (serializer)
        {
          case XmlSerializer xmlSerializer1:
            xmlSerializer1.Serialize(xmlWriter, value);
            break;
          case XmlObjectSerializer objectSerializer1:
label_6:
            objectSerializer1.WriteObject(xmlWriter, value);
            break;
          default:
            XmlMediaTypeFormatter.ThrowInvalidSerializerException(serializer, "GetSerializer");
            goto label_6;
        }
      }
    }

    /// <summary>Called during serialization to get the XML serializer to use for serializing objects.</summary>
    /// <returns>An instance of <see cref="T:System.Runtime.Serialization.XmlObjectSerializer" /> or <see cref="T:System.Xml.Serialization.XmlSerializer" /> to use for serializing the object.</returns>
    /// <param name="type">The type of object to serialize.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
    protected internal virtual object GetSerializer(Type type, object value, HttpContent content) => this.GetSerializerForType(type);

    /// <summary>Called during serialization to get the XML writer to use for writing objects to the stream.</summary>
    /// <returns>The <see cref="T:System.Xml.XmlWriter" /> to use for writing objects.</returns>
    /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
    protected internal virtual XmlWriter CreateXmlWriter(
      Stream writeStream,
      HttpContent content)
    {
      Encoding encoding = this.SelectCharacterEncoding(content?.Headers);
      XmlWriterSettings settings = this.WriterSettings.Clone();
      settings.Encoding = encoding;
      return XmlWriter.Create(writeStream, settings);
    }

    /// <summary>Called during deserialization to get the XML serializer.</summary>
    /// <returns>The object used for serialization.</returns>
    /// <param name="type">The type of object that will be serialized or deserialized.</param>
    public virtual XmlSerializer CreateXmlSerializer(Type type) => new XmlSerializer(type);

    /// <summary>Called during deserialization to get the DataContractSerializer serializer.</summary>
    /// <returns>The object used for serialization.</returns>
    /// <param name="type">The type of object that will be serialized or deserialized.</param>
    public virtual DataContractSerializer CreateDataContractSerializer(
      Type type)
    {
      return new DataContractSerializer(type);
    }

    /// <summary>This method is to support infrastructure and is not intended to be used directly from your code.</summary>
    /// <returns>Returns <see cref="T:System.Xml.XmlReader" />.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public XmlReader InvokeCreateXmlReader(Stream readStream, HttpContent content) => this.CreateXmlReader(readStream, content);

    /// <summary>This method is to support infrastructure and is not intended to be used directly from your code.</summary>
    /// <returns>Returns <see cref="T:System.Xml.XmlWriter" />.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public XmlWriter InvokeCreateXmlWriter(Stream writeStream, HttpContent content) => this.CreateXmlWriter(writeStream, content);

    /// <summary>This method is to support infrastructure and is not intended to be used directly from your code.</summary>
    /// <returns>Returns <see cref="T:System.Object" />.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object InvokeGetDeserializer(Type type, HttpContent content) => this.GetDeserializer(type, content);

    /// <summary>This method is to support infrastructure and is not intended to be used directly from your code.</summary>
    /// <returns>Returns <see cref="T:System.Object" />.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object InvokeGetSerializer(Type type, object value, HttpContent content) => this.GetSerializer(type, value, content);

    private object CreateDefaultSerializer(Type type, bool throwOnError)
    {
      Exception innerException = (Exception) null;
      object obj = (object) null;
      try
      {
        if (this.UseXmlSerializer)
        {
          obj = (object) this.CreateXmlSerializer(type);
        }
        else
        {
          FormattingUtilities.XsdDataContractExporter.GetRootElementName(type);
          obj = (object) this.CreateDataContractSerializer(type);
        }
      }
      catch (Exception ex)
      {
        innerException = ex;
      }
      if (!(obj == null & throwOnError))
        return obj;
      if (innerException != null)
        throw Error.InvalidOperation(innerException, Resources.SerializerCannotSerializeType, this.UseXmlSerializer ? (object) typeof (XmlSerializer).Name : (object) typeof (DataContractSerializer).Name, (object) type.Name);
      throw Error.InvalidOperation(Resources.SerializerCannotSerializeType, this.UseXmlSerializer ? (object) typeof (XmlSerializer).Name : (object) typeof (DataContractSerializer).Name, (object) type.Name);
    }

    private object GetCachedSerializer(Type type, bool throwOnError)
    {
      object defaultSerializer;
      if (!this._serializerCache.TryGetValue(type, out defaultSerializer))
      {
        defaultSerializer = this.CreateDefaultSerializer(type, throwOnError);
        this._serializerCache.TryAdd(type, defaultSerializer);
      }
      return defaultSerializer;
    }

    private void VerifyAndSetSerializer(Type type, object serializer)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (serializer == null)
        throw Error.ArgumentNull(nameof (serializer));
      this.SetSerializerInternal(type, serializer);
    }

    private void SetSerializerInternal(Type type, object serializer) => this._serializerCache.AddOrUpdate(type, serializer, (Func<Type, object, object>) ((key, value) => serializer));

    private object GetSerializerForType(Type type) => this.GetCachedSerializer(type, true) ?? throw Error.InvalidOperation(Resources.SerializerCannotSerializeType, this.UseXmlSerializer ? (object) typeof (XmlSerializer).Name : (object) typeof (DataContractSerializer).Name, (object) type.Name);

    private static void ThrowInvalidSerializerException(
      object serializer,
      string getSerializerMethodName)
    {
      if (serializer == null)
        throw Error.InvalidOperation(Resources.XmlMediaTypeFormatter_NullReturnedSerializer, (object) getSerializerMethodName);
      throw Error.InvalidOperation(Resources.XmlMediaTypeFormatter_InvalidSerializerType, (object) serializer.GetType().Name, (object) getSerializerMethodName);
    }
  }
}
