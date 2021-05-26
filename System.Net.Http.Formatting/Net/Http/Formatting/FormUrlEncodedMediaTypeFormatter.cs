// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting.Parsers;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>
  /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> class for handling HTML form URL-ended data, also known as application/x-www-form-urlencoded.  </summary>
  public class FormUrlEncodedMediaTypeFormatter : MediaTypeFormatter
  {
    private const int MinBufferSize = 256;
    private const int DefaultBufferSize = 32768;
    private int _readBufferSize = 32768;
    private int _maxDepth = 256;
    private readonly bool _isDerived;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> class.</summary>
    public FormUrlEncodedMediaTypeFormatter()
    {
      this.SupportedMediaTypes.Add(MediaTypeConstants.ApplicationFormUrlEncodedMediaType);
      this._isDerived = this.GetType() != typeof (FormUrlEncodedMediaTypeFormatter);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> instance to copy settings from.</param>
    protected FormUrlEncodedMediaTypeFormatter(FormUrlEncodedMediaTypeFormatter formatter)
      : base((MediaTypeFormatter) formatter)
    {
      this.MaxDepth = formatter.MaxDepth;
      this.ReadBufferSize = formatter.ReadBufferSize;
      this._isDerived = this.GetType() != typeof (FormUrlEncodedMediaTypeFormatter);
    }

    /// <summary>Gets the default media type for HTML form-URL-encoded data, which is application/x-www-form-urlencoded.</summary>
    /// <returns>The default media type for HTML form-URL-encoded data</returns>
    public static MediaTypeHeaderValue DefaultMediaType => MediaTypeConstants.ApplicationFormUrlEncodedMediaType;

    /// <summary>Gets or sets the maximum depth allowed by this formatter.</summary>
    /// <returns>The maximum depth.</returns>
    public int MaxDepth
    {
      get => this._maxDepth;
      set => this._maxDepth = value >= 1 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 1);
    }

    /// <summary>Gets or sets the size of the buffer when reading the incoming stream.</summary>
    /// <returns>The buffer size.</returns>
    public int ReadBufferSize
    {
      get => this._readBufferSize;
      set => this._readBufferSize = value >= 256 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 256);
    }

    internal override bool CanWriteAnyTypes => this._isDerived;

    /// <summary>Queries whether the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> can deserializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> can deserialize the type; otherwise, false.</returns>
    /// <param name="type">The type to deserialize.</param>
    public override bool CanReadType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return type == typeof (FormDataCollection) || FormattingUtilities.IsJTokenType(type);
    }

    /// <summary>Queries whether the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> can serializean object of the specified type.</summary>
    /// <returns>true if the <see cref="T:System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter" /> can serialize the type; otherwise, false.</returns>
    /// <param name="type">The type to serialize.</param>
    public override bool CanWriteType(Type type)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      return false;
    }

    /// <summary> Asynchronously deserializes an object of the specified type.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> whose result will be the object instance that has been read.</returns>
    /// <param name="type">The type of object to deserialize.</param>
    /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
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
        return Task.FromResult<object>(this.ReadFromStream(type, readStream));
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError<object>(ex);
      }
    }

    private object ReadFromStream(Type type, Stream readStream)
    {
      IEnumerable<KeyValuePair<string, string>> keyValuePairs = FormUrlEncodedMediaTypeFormatter.ReadFormUrlEncoded(readStream, this.ReadBufferSize);
      if (type == typeof (FormDataCollection))
        return (object) new FormDataCollection(keyValuePairs);
      if (FormattingUtilities.IsJTokenType(type))
        return (object) FormUrlEncodedJson.Parse(keyValuePairs, this._maxDepth);
      throw Error.InvalidOperation(Resources.SerializerCannotSerializeType, (object) this.GetType().Name, (object) type.Name);
    }

    private static IEnumerable<KeyValuePair<string, string>> ReadFormUrlEncoded(
      Stream input,
      int bufferSize)
    {
      byte[] buffer = new byte[bufferSize];
      bool isFinal = false;
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
      FormUrlEncodedParser urlEncodedParser = new FormUrlEncodedParser((ICollection<KeyValuePair<string, string>>) keyValuePairList, long.MaxValue);
      do
      {
        int bytesReady;
        try
        {
          bytesReady = input.Read(buffer, 0, buffer.Length);
          if (bytesReady == 0)
            isFinal = true;
        }
        catch (Exception ex)
        {
          string urlEncodedStream = Resources.ErrorReadingFormUrlEncodedStream;
          object[] objArray = new object[0];
          throw Error.InvalidOperation(ex, urlEncodedStream, objArray);
        }
        int bytesConsumed = 0;
        switch (urlEncodedParser.ParseBuffer(buffer, bytesReady, ref bytesConsumed, isFinal))
        {
          case ParserState.NeedMoreData:
          case ParserState.Done:
            continue;
          default:
            throw Error.InvalidOperation(Resources.FormUrlEncodedParseError, (object) bytesConsumed);
        }
      }
      while (!isFinal);
      return (IEnumerable<KeyValuePair<string, string>>) keyValuePairList;
    }
  }
}
