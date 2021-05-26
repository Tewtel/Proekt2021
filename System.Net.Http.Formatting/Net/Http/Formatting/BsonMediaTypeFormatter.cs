// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.BsonMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>Represents a media type formatter to handle Bson.</summary>
  public class BsonMediaTypeFormatter : BaseJsonMediaTypeFormatter
  {
    private static readonly Type OpenDictionaryType = typeof (Dictionary<,>);

    /// <summary>Initializes a new instance of the<see cref="T:System.Net.Http.Formatting.BsonMediaTypeFormatter" /> class.</summary>
    public BsonMediaTypeFormatter() => this.SupportedMediaTypes.Add(MediaTypeConstants.ApplicationBsonMediaType);

    /// <summary>Initializes a new instance of the<see cref="T:System.Net.Http.Formatting.BsonMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The formatter to copy settings from.</param>
    protected BsonMediaTypeFormatter(BsonMediaTypeFormatter formatter)
      : base((BaseJsonMediaTypeFormatter) formatter)
    {
    }

    /// <summary>Gets the default media type for Json, namely "application/bson".</summary>
    /// <returns>The default media type for Json, namely "application/bson".</returns>
    public static MediaTypeHeaderValue DefaultMediaType => MediaTypeConstants.ApplicationBsonMediaType;

    /// <summary>Gets or sets the maximum depth allowed by this formatter.</summary>
    /// <returns>The maximum depth allowed by this formatter.</returns>
    public override sealed int MaxDepth
    {
      get => base.MaxDepth;
      set => base.MaxDepth = value;
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
      if (type == typeof (DBNull) && content != null && content.Headers != null)
      {
        long? contentLength = content.Headers.ContentLength;
        long num = 0;
        if (contentLength.GetValueOrDefault() == num & contentLength.HasValue)
          return Task.FromResult<object>((object) DBNull.Value);
      }
      return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
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
      if (!BsonMediaTypeFormatter.IsSimpleType(type) && !(type == typeof (byte[])))
        return base.ReadFromStream(type, readStream, effectiveEncoding, formatterLogger);
      Type type1 = BsonMediaTypeFormatter.OpenDictionaryType.MakeGenericType(typeof (string), type);
      if (!(base.ReadFromStream(type1, readStream, effectiveEncoding, formatterLogger) is IDictionary dictionary))
        throw Error.InvalidOperation(Resources.MediaTypeFormatter_BsonParseError_MissingData, (object) type1.Name);
      string empty = string.Empty;
      IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
        {
          DictionaryEntry current = (DictionaryEntry) enumerator.Current;
          if (dictionary.Count == 1 && current.Key as string == "Value")
            return current.Value;
          if (current.Key != null)
            empty = current.Key.ToString();
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable2)
          disposable2.Dispose();
      }
      throw Error.InvalidOperation(Resources.MediaTypeFormatter_BsonParseError_UnexpectedData, (object) dictionary.Count, (object) empty);
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
      BsonReader bsonReader = effectiveEncoding != null ? new BsonReader(new BinaryReader(readStream, effectiveEncoding)) : throw Error.ArgumentNull(nameof (effectiveEncoding));
      try
      {
        bsonReader.ReadRootValueAsArray = typeof (IEnumerable).IsAssignableFrom(type) && !typeof (IDictionary).IsAssignableFrom(type);
      }
      catch
      {
        ((IDisposable) bsonReader).Dispose();
        throw;
      }
      return (JsonReader) bsonReader;
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
      if (value == null || value == DBNull.Value)
        return;
      Type type1 = value.GetType();
      if (BsonMediaTypeFormatter.IsSimpleType(type1) || type1 == typeof (byte[]))
        base.WriteToStream(typeof (Dictionary<string, object>), (object) new Dictionary<string, object>()
        {
          {
            "Value",
            value
          }
        }, writeStream, effectiveEncoding);
      else
        base.WriteToStream(type, value, writeStream, effectiveEncoding);
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
      return effectiveEncoding != null ? (JsonWriter) new BsonWriter(new BinaryWriter(writeStream, effectiveEncoding)) : throw Error.ArgumentNull(nameof (effectiveEncoding));
    }

    private static bool IsSimpleType(Type type) => TypeDescriptor.GetConverter(type).CanConvertFrom(typeof (string));
  }
}
