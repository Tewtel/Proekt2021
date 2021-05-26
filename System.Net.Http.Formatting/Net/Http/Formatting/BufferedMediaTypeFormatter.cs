// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.BufferedMediaTypeFormatter
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>Represents a helper class to allow a synchronous formatter on top of the asynchronous formatter infrastructure.</summary>
  public abstract class BufferedMediaTypeFormatter : MediaTypeFormatter
  {
    private const int MinBufferSize = 0;
    private const int DefaultBufferSize = 16384;
    private int _bufferSizeInBytes = 16384;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.BufferedMediaTypeFormatter" /> class.</summary>
    protected BufferedMediaTypeFormatter()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.BufferedMediaTypeFormatter" /> class.</summary>
    /// <param name="formatter">The <see cref="T:System.Net.Http.Formatting.BufferedMediaTypeFormatter" /> instance to copy settings from.</param>
    protected BufferedMediaTypeFormatter(BufferedMediaTypeFormatter formatter)
      : base((MediaTypeFormatter) formatter)
    {
      this.BufferSize = formatter.BufferSize;
    }

    /// <summary>Gets or sets the suggested size of buffer to use with streams in bytes.</summary>
    /// <returns>The suggested size of buffer to use with streams in bytes.</returns>
    public int BufferSize
    {
      get => this._bufferSizeInBytes;
      set => this._bufferSizeInBytes = value >= 0 ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 0);
    }

    /// <summary>Writes synchronously to the buffered stream.</summary>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="value">The object value to write. Can be null.</param>
    /// <param name="writeStream">The stream to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public virtual void WriteToStream(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      CancellationToken cancellationToken)
    {
      this.WriteToStream(type, value, writeStream, content);
    }

    /// <summary>Writes synchronously to the buffered stream.</summary>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="value">The object value to write. Can be null.</param>
    /// <param name="writeStream">The stream to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    public virtual void WriteToStream(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content)
    {
      throw Error.NotSupported(Resources.MediaTypeFormatterCannotWriteSync, (object) this.GetType().Name);
    }

    /// <summary>Reads synchronously from the buffered stream.</summary>
    /// <returns>An object of the given <paramref name="type" />.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public virtual object ReadFromStream(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      return this.ReadFromStream(type, readStream, content, formatterLogger);
    }

    /// <summary>Reads synchronously from the buffered stream.</summary>
    /// <returns>An object of the given <paramref name="type" />.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    public virtual object ReadFromStream(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger)
    {
      throw Error.NotSupported(Resources.MediaTypeFormatterCannotReadSync, (object) this.GetType().Name);
    }

    /// <summary>Writes asynchronously to the buffered stream.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="value">The object value to write.  It may be null.</param>
    /// <param name="writeStream">The stream to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="transportContext">The transport context.</param>
    public override sealed Task WriteToStreamAsync(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      TransportContext transportContext)
    {
      return this.WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
    }

    /// <summary>Writes asynchronously to the buffered stream.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="value">The object value to write.  It may be null.</param>
    /// <param name="writeStream">The stream to which to write.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="transportContext">The transport context.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public override sealed Task WriteToStreamAsync(
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
      try
      {
        this.WriteToStreamSync(type, value, writeStream, content, cancellationToken);
        return TaskHelpers.Completed();
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError(ex);
      }
    }

    private void WriteToStreamSync(
      Type type,
      object value,
      Stream writeStream,
      HttpContent content,
      CancellationToken cancellationToken)
    {
      using (Stream bufferStream = BufferedMediaTypeFormatter.GetBufferStream(writeStream, this._bufferSizeInBytes))
        this.WriteToStream(type, value, bufferStream, content, cancellationToken);
    }

    /// <summary>Reads asynchronously from the buffered stream.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    public override sealed Task<object> ReadFromStreamAsync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger)
    {
      return this.ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
    }

    /// <summary>Reads asynchronously from the buffered stream.</summary>
    /// <returns>A task object representing the asynchronous operation.</returns>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="readStream">The stream from which to read.</param>
    /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
    /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public override sealed Task<object> ReadFromStreamAsync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      if (readStream == null)
        throw Error.ArgumentNull(nameof (readStream));
      try
      {
        return Task.FromResult<object>(this.ReadFromStreamSync(type, readStream, content, formatterLogger, cancellationToken));
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError<object>(ex);
      }
    }

    private object ReadFromStreamSync(
      Type type,
      Stream readStream,
      HttpContent content,
      IFormatterLogger formatterLogger,
      CancellationToken cancellationToken)
    {
      HttpContentHeaders httpContentHeaders = content == null ? (HttpContentHeaders) null : content.Headers;
      if (httpContentHeaders != null)
      {
        long? contentLength = httpContentHeaders.ContentLength;
        long num = 0;
        if (contentLength.GetValueOrDefault() == num & contentLength.HasValue)
          return MediaTypeFormatter.GetDefaultValueForType(type);
      }
      using (Stream bufferStream = BufferedMediaTypeFormatter.GetBufferStream(readStream, this._bufferSizeInBytes))
        return this.ReadFromStream(type, bufferStream, content, formatterLogger, cancellationToken);
    }

    private static Stream GetBufferStream(Stream innerStream, int bufferSize) => (Stream) new BufferedStream((Stream) new NonClosingDelegatingStream(innerStream), bufferSize);
  }
}
