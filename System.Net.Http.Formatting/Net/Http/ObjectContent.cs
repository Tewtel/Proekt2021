// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ObjectContent
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary> Contains a value as well as an associated <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> that will be used to serialize the value when writing this content. </summary>
  public class ObjectContent : HttpContent
  {
    private object _value;
    private readonly MediaTypeFormatter _formatter;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ObjectContent" /> class.</summary>
    /// <param name="type">The type of object this instance will contain.</param>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    public ObjectContent(Type type, object value, MediaTypeFormatter formatter)
      : this(type, value, formatter, (MediaTypeHeaderValue) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ObjectContent" /> class.</summary>
    /// <param name="type">The type of object this instance will contain.</param>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    /// <param name="mediaType">The authoritative value of the Content-Type header.</param>
    public ObjectContent(Type type, object value, MediaTypeFormatter formatter, string mediaType)
      : this(type, value, formatter, ObjectContent.BuildHeaderValue(mediaType))
    {
    }

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.ObjectContent" /> class. </summary>
    /// <param name="type">The type of object this instance will contain.</param>
    /// <param name="value">The value of the object this instance will contain.</param>
    /// <param name="formatter">The formatter to use when serializing the value.</param>
    /// <param name="mediaType">The authoritative value of the Content-Type header. Can be null, in which case the default content type of the formatter will be used.</param>
    public ObjectContent(
      Type type,
      object value,
      MediaTypeFormatter formatter,
      MediaTypeHeaderValue mediaType)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (formatter == null)
        throw Error.ArgumentNull(nameof (formatter));
      this._formatter = formatter.CanWriteType(type) ? formatter : throw Error.InvalidOperation(Resources.ObjectContent_FormatterCannotWriteType, (object) formatter.GetType().FullName, (object) type.Name);
      this.ObjectType = type;
      this.VerifyAndSetObject(value);
      this._formatter.SetDefaultContentHeaders(type, this.Headers, mediaType);
    }

    /// <summary>Gets the type of object managed by this <see cref="T:System.Net.Http.ObjectContent" /> instance.</summary>
    /// <returns>The object type.</returns>
    public Type ObjectType { get; private set; }

    /// <summary>Gets the media-type formatter associated with this content instance.</summary>
    /// <returns>The media type formatter associated with this content instance.</returns>
    public MediaTypeFormatter Formatter => this._formatter;

    /// <summary>Gets or sets the value of the content.</summary>
    /// <returns>The content value.</returns>
    public object Value
    {
      get => this._value;
      set => this._value = value;
    }

    internal static MediaTypeHeaderValue BuildHeaderValue(string mediaType) => mediaType == null ? (MediaTypeHeaderValue) null : new MediaTypeHeaderValue(mediaType);

    /// <summary>Asynchronously serializes the object's content to the given stream.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="context">The associated <see cref="T:System.Net.TransportContext" />.</param>
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => this._formatter.WriteToStreamAsync(this.ObjectType, this.Value, stream, (HttpContent) this, context);

    /// <summary>Computes the length of the stream if possible.</summary>
    /// <returns>true if the length has been computed; otherwise, false.</returns>
    /// <param name="length">Receives the computed length of the stream.</param>
    protected override bool TryComputeLength(out long length)
    {
      length = -1L;
      return false;
    }

    private static bool IsTypeNullable(Type type)
    {
      if (!type.IsValueType())
        return true;
      return type.IsGenericType() && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    private void VerifyAndSetObject(object value)
    {
      if (value == null)
      {
        if (!ObjectContent.IsTypeNullable(this.ObjectType))
          throw Error.InvalidOperation(Resources.CannotUseNullValueType, (object) typeof (ObjectContent).Name, (object) this.ObjectType.Name);
      }
      else
      {
        Type type = value.GetType();
        if (!this.ObjectType.IsAssignableFrom(type))
          throw Error.Argument(nameof (value), Resources.ObjectAndTypeDisagree, (object) type.Name, (object) this.ObjectType.Name);
      }
      this._value = value;
    }
  }
}
