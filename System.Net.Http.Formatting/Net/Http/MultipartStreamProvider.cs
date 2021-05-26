// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
  /// <summary>Represents a stream provider that examines the headers provided by the MIME multipart parser as part of the MIME multipart extension methods (see <see cref="T:System.Net.Http.HttpContentMultipartExtensions" />) and decides what kind of stream to return for the body part to be written to.</summary>
  public abstract class MultipartStreamProvider
  {
    private Collection<HttpContent> _contents = new Collection<HttpContent>();

    /// <summary>Gets or sets the contents for this <see cref="T:System.Net.Http.MultipartStreamProvider" />.</summary>
    /// <returns>The contents for this <see cref="T:System.Net.Http.MultipartStreamProvider" />.</returns>
    public Collection<HttpContent> Contents => this._contents;

    /// <summary>Gets the stream where to write the body part to. This method is called when a MIME multipart body part has been parsed.</summary>
    /// <returns>The <see cref="T:System.IO.Stream" /> instance where the message body part is written to.</returns>
    /// <param name="parent">The content of the HTTP.</param>
    /// <param name="headers">The header fields describing the body part.</param>
    public abstract Stream GetStream(HttpContent parent, HttpContentHeaders headers);

    /// <summary>Executes the post processing operation for this <see cref="T:System.Net.Http.MultipartStreamProvider" />.</summary>
    /// <returns>The asynchronous task for this operation.</returns>
    public virtual Task ExecutePostProcessingAsync() => TaskHelpers.Completed();

    /// <summary>Executes the post processing operation for this <see cref="T:System.Net.Http.MultipartStreamProvider" />.</summary>
    /// <returns>The asynchronous task for this operation.</returns>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    public virtual Task ExecutePostProcessingAsync(CancellationToken cancellationToken) => this.ExecutePostProcessingAsync();
  }
}
