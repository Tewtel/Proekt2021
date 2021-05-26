// Decompiled with JetBrains decompiler
// Type: System.Net.Http.PushStreamContent
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Enables scenarios where a data producer wants to write directly (either synchronously or asynchronously) using a stream.</summary>
  public class PushStreamContent : HttpContent
  {
    private readonly Func<Stream, HttpContent, TransportContext, Task> _onStreamAvailable;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly. </param>
    public PushStreamContent(
      Action<Stream, HttpContent, TransportContext> onStreamAvailable)
      : this(PushStreamContent.Taskify(onStreamAvailable), (MediaTypeHeaderValue) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly.</param>
    public PushStreamContent(
      Func<Stream, HttpContent, TransportContext, Task> onStreamAvailable)
      : this(onStreamAvailable, (MediaTypeHeaderValue) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly.</param>
    /// <param name="mediaType">The media type.</param>
    public PushStreamContent(
      Action<Stream, HttpContent, TransportContext> onStreamAvailable,
      string mediaType)
      : this(PushStreamContent.Taskify(onStreamAvailable), new MediaTypeHeaderValue(mediaType))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly.</param>
    /// <param name="mediaType">The media type.</param>
    public PushStreamContent(
      Func<Stream, HttpContent, TransportContext, Task> onStreamAvailable,
      string mediaType)
      : this(onStreamAvailable, new MediaTypeHeaderValue(mediaType))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly.</param>
    /// <param name="mediaType">The media type.</param>
    public PushStreamContent(
      Action<Stream, HttpContent, TransportContext> onStreamAvailable,
      MediaTypeHeaderValue mediaType)
      : this(PushStreamContent.Taskify(onStreamAvailable), mediaType)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.PushStreamContent" /> class.</summary>
    /// <param name="onStreamAvailable">An action that is called when an output stream is available, allowing the action to write to it directly.</param>
    /// <param name="mediaType">The media type.</param>
    public PushStreamContent(
      Func<Stream, HttpContent, TransportContext, Task> onStreamAvailable,
      MediaTypeHeaderValue mediaType)
    {
      this._onStreamAvailable = onStreamAvailable != null ? onStreamAvailable : throw Error.ArgumentNull(nameof (onStreamAvailable));
      this.Headers.ContentType = mediaType ?? MediaTypeConstants.ApplicationOctetStreamMediaType;
    }

    private static Func<Stream, HttpContent, TransportContext, Task> Taskify(
      Action<Stream, HttpContent, TransportContext> onStreamAvailable)
    {
      if (onStreamAvailable == null)
        throw Error.ArgumentNull(nameof (onStreamAvailable));
      return (Func<Stream, HttpContent, TransportContext, Task>) ((stream, content, transportContext) =>
      {
        onStreamAvailable(stream, content, transportContext);
        return TaskHelpers.Completed();
      });
    }

    /// <summary>Asynchronously serializes the push content into stream.</summary>
    /// <returns>The serialized push content.</returns>
    /// <param name="stream">The stream where the push content will be serialized.</param>
    /// <param name="context">The context.</param>
    protected override async Task SerializeToStreamAsync(
      Stream stream,
      TransportContext context)
    {
      PushStreamContent pushStreamContent = this;
      TaskCompletionSource<bool> serializeToStreamTask = new TaskCompletionSource<bool>();
      Stream stream1 = (Stream) new PushStreamContent.CompleteTaskOnCloseStream(stream, serializeToStreamTask);
      await pushStreamContent._onStreamAvailable(stream1, (HttpContent) pushStreamContent, context);
      int num = await serializeToStreamTask.Task ? 1 : 0;
    }

    /// <summary>Determines whether the stream content has a valid length in bytes.</summary>
    /// <returns>true if length is a valid length; otherwise, false.</returns>
    /// <param name="length">The length in bytes of the stream content.</param>
    protected override bool TryComputeLength(out long length)
    {
      length = -1L;
      return false;
    }

    internal class CompleteTaskOnCloseStream : System.Net.Http.Internal.DelegatingStream
    {
      private TaskCompletionSource<bool> _serializeToStreamTask;

      public CompleteTaskOnCloseStream(
        Stream innerStream,
        TaskCompletionSource<bool> serializeToStreamTask)
        : base(innerStream)
      {
        this._serializeToStreamTask = serializeToStreamTask;
      }

      public override void Close() => this._serializeToStreamTask.TrySetResult(true);
    }
  }
}
