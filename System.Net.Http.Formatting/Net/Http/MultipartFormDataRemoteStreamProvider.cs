// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFormDataRemoteStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http.Formatting.Internal;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>A <see cref="T:System.Net.Http.MultipartStreamProvider" /> implementation suited for use with HTML file uploads for writing file content to a remote storage <see cref="T:System.IO.Stream" />. The stream provider looks at the Content-Disposition header field and determines an output remote <see cref="T:System.IO.Stream" /> based on the presence of a filename parameter. If a filename parameter is present in the Content-Disposition header field, then the body part is written to a remote <see cref="T:System.IO.Stream" /> provided by <see cref="M:System.Net.Http.MultipartFormDataRemoteStreamProvider.GetRemoteStream(System.Net.Http.HttpContent,System.Net.Http.Headers.HttpContentHeaders)" />. Otherwise it is written to a <see cref="T:System.IO.MemoryStream" />.</summary>
  public abstract class MultipartFormDataRemoteStreamProvider : MultipartStreamProvider
  {
    private CancellationToken _cancellationToken = CancellationToken.None;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFormDataRemoteStreamProvider" /> class.</summary>
    protected MultipartFormDataRemoteStreamProvider()
    {
      this.FormData = (NameValueCollection) HttpValueCollection.Create();
      this.FileData = new Collection<MultipartRemoteFileData>();
    }

    /// <summary>Gets a collection of file data passed as part of the multipart form data.</summary>
    public Collection<MultipartRemoteFileData> FileData { get; private set; }

    /// <summary>Gets a <see cref="T:System.Collections.Specialized.NameValueCollection" /> of form data passed as part of the multipart form data.</summary>
    public NameValueCollection FormData { get; private set; }

    /// <summary>Provides a <see cref="T:System.Net.Http.RemoteStreamInfo" /> for <see cref="M:System.Net.Http.MultipartFormDataRemoteStreamProvider.GetStream(System.Net.Http.HttpContent,System.Net.Http.Headers.HttpContentHeaders)" />. Override this method to provide a remote stream to which the data should be written.</summary>
    /// <returns>A result specifying a remote stream where the file will be written to and a location where the file can be accessed. It cannot be null and the stream must be writable.</returns>
    /// <param name="parent">The parent <see cref="T:System.Net.Http.HttpContent" /> MIME multipart instance.</param>
    /// <param name="headers">The header fields describing the body part's content.</param>
    public abstract RemoteStreamInfo GetRemoteStream(
      HttpContent parent,
      HttpContentHeaders headers);

    public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
    {
      if (!MultipartFormDataStreamProviderHelper.IsFileContent(parent, headers))
        return (Stream) new MemoryStream();
      RemoteStreamInfo remoteStream = this.GetRemoteStream(parent, headers);
      if (remoteStream == null)
        throw Error.InvalidOperation(Resources.RemoteStreamInfoCannotBeNull, (object) "GetRemoteStream", (object) this.GetType().Name);
      this.FileData.Add(new MultipartRemoteFileData(headers, remoteStream.Location, remoteStream.FileName));
      return remoteStream.RemoteStream;
    }

    /// <summary>Read the non-file contents as form data.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing the post processing.</returns>
    public override Task ExecutePostProcessingAsync() => MultipartFormDataStreamProviderHelper.ReadFormDataAsync(this.Contents, this.FormData, this._cancellationToken);

    /// <summary>Read the non-file contents as form data.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing the post processing.</returns>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    public override Task ExecutePostProcessingAsync(CancellationToken cancellationToken)
    {
      this._cancellationToken = cancellationToken;
      return this.ExecutePostProcessingAsync();
    }
  }
}
