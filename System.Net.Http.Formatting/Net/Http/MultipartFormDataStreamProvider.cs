// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFormDataStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Specialized;
using System.IO;
using System.Net.Http.Formatting.Internal;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
  /// <summary>Represents an <see cref="T:System.Net.Http.IMultipartStreamProvider" /> suited for use with HTML file uploads for writing file  content to a <see cref="T:System.IO.FileStream" />.</summary>
  public class MultipartFormDataStreamProvider : MultipartFileStreamProvider
  {
    private CancellationToken _cancellationToken;

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFormDataStreamProvider" /> class. </summary>
    /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
    public MultipartFormDataStreamProvider(string rootPath)
      : base(rootPath)
    {
      this.FormData = (NameValueCollection) HttpValueCollection.Create();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFormDataStreamProvider" /> class.</summary>
    /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
    /// <param name="bufferSize">The number of bytes buffered for writes to the file.</param>
    public MultipartFormDataStreamProvider(string rootPath, int bufferSize)
      : base(rootPath, bufferSize)
    {
      this.FormData = (NameValueCollection) HttpValueCollection.Create();
    }

    /// <summary>Gets a <see cref="T:System.Collections.Specialized.NameValueCollection" /> of form data passed as part of the multipart form data.</summary>
    /// <returns>The <see cref="T:System.Collections.Specialized.NameValueCollection" /> of form data.</returns>
    public NameValueCollection FormData { get; private set; }

    /// <summary>Gets the streaming instance where the message body part is written.</summary>
    /// <returns>The <see cref="T:System.IO.Stream" /> instance where the message body part is written.</returns>
    /// <param name="parent">The HTTP content that contains this body part.</param>
    /// <param name="headers">Header fields describing the body part.</param>
    public override Stream GetStream(HttpContent parent, HttpContentHeaders headers) => MultipartFormDataStreamProviderHelper.IsFileContent(parent, headers) ? base.GetStream(parent, headers) : (Stream) new MemoryStream();

    /// <summary>Reads the non-file contents as form data.</summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override Task ExecutePostProcessingAsync() => MultipartFormDataStreamProviderHelper.ReadFormDataAsync(this.Contents, this.FormData, this._cancellationToken);

    public override Task ExecutePostProcessingAsync(CancellationToken cancellationToken)
    {
      this._cancellationToken = cancellationToken;
      return this.ExecutePostProcessingAsync();
    }
  }
}
