// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.BaseJsonMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>Abstract media type formatter class to support Bson and Json.</summary>
  public abstract class BaseJsonMediaTypeFormatter : MediaTypeFormatter
  {
    private int _maxDepth = 256;
    private readonly IContractResolver _defaultContractResolver;
    private JsonSerializerSettings _jsonSerializerSettings;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.BaseJsonMediaTypeFormatter" /> class.</summary>
    protected BaseJsonMediaTypeFormatter()
    {
      this._defaultContractResolver = (IContractResolver) new JsonContractResolver((MediaTypeFormatter) this);
      this._jsonSerializerSettings = this.CreateDefaultSerializerSettings();
      this.SupportedEncodings.Add((Encoding) new UTF8Encoding(false, true));
      this.SupportedEncodings.Add((Encoding) new UnicodeEncoding(false, true, true));
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.BaseJsonMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.BaseJsonMediaTypeFormatter" /> instance to copy settings from.</param>
    protected BaseJsonMediaTypeFormatter(BaseJsonMediaTypeFormatter formatter)
      : base((MediaTypeFormatter) formatter)
    {
      this.SerializerSettings = formatter.SerializerSettings;
      this.MaxDepth = formatter._maxDepth;
    }

    /// <summary>Gets or sets the JsonSerializerSettings used to configure the JsonSerializer.</summary>
    /// <returns>The JsonSerializerSettings used to configure the JsonSerializer.</returns>
    public JsonSerializerSettings SerializerSettings
    {
      get => this._jsonSerializerSettings;
      set => this._jsonSerializerSettings = value != null ? value : throw Error.ArgumentNull(nameof (value));
    }

    /// <summary>Gets or sets the maximum depth allowed by this formatter.</summary>
    /// <returns>The maximum depth allowed by this formatter.</returns>
    public virtual int MaxDepth
    {
      get => this._maxDepth;
      set => this._maxDepth = value >= 1 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 1);
    }

    /// <summary>Creates a <see cref="T:Newtonsoft.Json.JsonSerializerSettings" /> instance with the default settings used by the <see cref="T:System.Net.Http.Formatting.BaseJsonMediaTypeFormatter" />.</summary>
    /// <returns>Returns <see cref="T:Newtonsoft.Json.JsonSerializerSettings" />.</returns>
    public JsonSerializerSettings CreateDefaultSerializerSettings() => new JsonSerializerSettings()
    {
      ContractResolver = this._defaultContractResolver,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      TypeNameHandling = TypeNameHandling.None
    };

    /// <summary>Determines whether this formatter can read objects of the specified type.</summary>
    /// <returns>true if objects of this type can be read, otherwise false.</returns>
    /// <param name="type">The type of object that will be read.</param>
    public override bool CanReadType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return true;
    }

    /// <summary>Determines whether this formatter can write objects of the specified type.</summary>
    /// <returns>true if objects of this type can be written, otherwise false.</returns>
    /// <param name="type">The type of object to write.</param>
    public override bool CanWriteType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return true;
    }

    /// <summary>Called during deserialization to read an object of the specified type from the specified stream.</summary>
    /// <returns>A task whose result will be the object instance that has been read.</returns>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being read.</param>
    /// <param name="formatterLogger">The logger to log events to.</param>
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
      HttpContentHeaders contentHeaders = content == null ? (HttpContentHeaders) null : content.Headers;
      if (contentHeaders != null)
      {
        long? contentLength = contentHeaders.ContentLength;
        long num = 0;
        if (contentLength.GetValueOrDefault() == num & contentLength.HasValue)
          return MediaTypeFormatter.GetDefaultValueForType(type);
      }
      Encoding effectiveEncoding = this.SelectCharacterEncoding(contentHeaders);
      try
      {
        return this.ReadFromStream(type, readStream, effectiveEncoding, formatterLogger);
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

    /// <summary>Called during deserialization to read an object of the specified type from the specified stream.</summary>
    /// <returns>The object that has been read.</returns>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="effectiveEncoding">The encoding to use when reading.</param>
    /// <param name="formatterLogger">The logger to log events to.</param>
    public virtual object ReadFromStream(
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
      using (JsonReader jsonReaderInternal = this.CreateJsonReaderInternal(type, readStream, effectiveEncoding))
      {
        jsonReaderInternal.CloseInput = false;
        jsonReaderInternal.MaxDepth = new int?(this._maxDepth);
        JsonSerializer serializerInternal = this.CreateJsonSerializerInternal();
        EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> eventHandler = (EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>) null;
        if (formatterLogger != null)
        {
          eventHandler = (EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>) ((sender, e) =>
          {
            Exception error = e.ErrorContext.Error;
            formatterLogger.LogError(e.ErrorContext.Path, error);
            e.ErrorContext.Handled = true;
          });
          serializerInternal.Error += eventHandler;
        }
        try
        {
          return serializerInternal.Deserialize(jsonReaderInternal, type);
        }
        finally
        {
          if (eventHandler != null)
            serializerInternal.Error -= eventHandler;
        }
      }
    }

    private JsonReader CreateJsonReaderInternal(
      Type type,
      Stream readStream,
      Encoding effectiveEncoding)
    {
      return this.CreateJsonReader(type, readStream, effectiveEncoding) ?? throw Error.InvalidOperation(Resources.MediaTypeFormatter_JsonReaderFactoryReturnedNull, (object) "CreateJsonReader");
    }

    /// <summary>Called during deserialization to get the <see cref="T:Newtonsoft.Json.JsonReader" />.</summary>
    /// <returns>The reader to use during deserialization.</returns>
    /// <param name="type">The type of the object to read.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="effectiveEncoding">The encoding to use when reading.</param>
    public abstract JsonReader CreateJsonReader(
      Type type,
      Stream readStream,
      Encoding effectiveEncoding);

    private JsonSerializer CreateJsonSerializerInternal()
    {
      JsonSerializer jsonSerializer;
      try
      {
        jsonSerializer = this.CreateJsonSerializer();
      }
      catch (Exception ex)
      {
        string serializerFactoryThrew = Resources.JsonSerializerFactoryThrew;
        object[] objArray = new object[1]
        {
          (object) "CreateJsonSerializer"
        };
        throw Error.InvalidOperation(ex, serializerFactoryThrew, objArray);
      }
      return jsonSerializer != null ? jsonSerializer : throw Error.InvalidOperation(Resources.JsonSerializerFactoryReturnedNull, (object) "CreateJsonSerializer");
    }

    /// <summary>Called during serialization and deserialization to get the <see cref="T:Newtonsoft.Json.JsonSerializer" />.</summary>
    /// <returns>The JsonSerializer used during serialization and deserialization.</returns>
    public virtual JsonSerializer CreateJsonSerializer() => JsonSerializer.Create(this.SerializerSettings);

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
      Encoding effectiveEncoding = this.SelectCharacterEncoding(content == null ? (HttpContentHeaders) null : content.Headers);
      this.WriteToStream(type, value, writeStream, effectiveEncoding);
    }

    /// <summary>Called during serialization to write an object of the specified type to the specified stream.</summary>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="value">The object to write.</param>
    /// <param name="writeStream">The stream to write to.</param>
    /// <param name="effectiveEncoding">The encoding to use when writing.</param>
    public virtual void WriteToStream(
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
      using (JsonWriter jsonWriterInternal = this.CreateJsonWriterInternal(type, writeStream, effectiveEncoding))
      {
        jsonWriterInternal.CloseOutput = false;
        this.CreateJsonSerializerInternal().Serialize(jsonWriterInternal, value);
        jsonWriterInternal.Flush();
      }
    }

    private JsonWriter CreateJsonWriterInternal(
      Type type,
      Stream writeStream,
      Encoding effectiveEncoding)
    {
      return this.CreateJsonWriter(type, writeStream, effectiveEncoding) ?? throw Error.InvalidOperation(Resources.MediaTypeFormatter_JsonWriterFactoryReturnedNull, (object) "CreateJsonWriter");
    }

    /// <summary>Called during serialization to get the <see cref="T:Newtonsoft.Json.JsonWriter" />.</summary>
    /// <returns>The writer to use during serialization.</returns>
    /// <param name="type">The type of the object to write.</param>
    /// <param name="writeStream">The stream to write to.</param>
    /// <param name="effectiveEncoding">The encoding to use when writing.</param>
    public abstract JsonWriter CreateJsonWriter(
      Type type,
      Stream writeStream,
      Encoding effectiveEncoding);
  }
}
