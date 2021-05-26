// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MimeBodyPart
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting.Parsers;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  internal class MimeBodyPart : IDisposable
  {
    private static readonly Type _streamType = typeof (Stream);
    private Stream _outputStream;
    private MultipartStreamProvider _streamProvider;
    private HttpContent _parentContent;
    private HttpContent _content;
    private HttpContentHeaders _headers;

    public MimeBodyPart(
      MultipartStreamProvider streamProvider,
      int maxBodyPartHeaderSize,
      HttpContent parentContent)
    {
      this._streamProvider = streamProvider;
      this._parentContent = parentContent;
      this.Segments = new List<ArraySegment<byte>>(2);
      this._headers = FormattingUtilities.CreateEmptyContentHeaders();
      this.HeaderParser = new InternetMessageFormatHeaderParser((HttpHeaders) this._headers, maxBodyPartHeaderSize, true);
    }

    public InternetMessageFormatHeaderParser HeaderParser { get; private set; }

    public HttpContent GetCompletedHttpContent()
    {
      if (this._content == null)
        return (HttpContent) null;
      this._headers.CopyTo(this._content.Headers);
      return this._content;
    }

    public List<ArraySegment<byte>> Segments { get; private set; }

    public bool IsComplete { get; set; }

    public bool IsFinal { get; set; }

    public async Task WriteSegment(
      ArraySegment<byte> segment,
      CancellationToken cancellationToken)
    {
      await this.GetOutputStream().WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken);
    }

    private Stream GetOutputStream()
    {
      if (this._outputStream == null)
      {
        try
        {
          this._outputStream = this._streamProvider.GetStream(this._parentContent, this._headers);
        }
        catch (Exception ex)
        {
          string providerException = Resources.ReadAsMimeMultipartStreamProviderException;
          object[] objArray = new object[1]
          {
            (object) this._streamProvider.GetType().Name
          };
          throw Error.InvalidOperation(ex, providerException, objArray);
        }
        if (this._outputStream == null)
          throw Error.InvalidOperation(Resources.ReadAsMimeMultipartStreamProviderNull, (object) this._streamProvider.GetType().Name, (object) MimeBodyPart._streamType.Name);
        this._content = this._outputStream.CanWrite ? (HttpContent) new StreamContent(this._outputStream) : throw Error.InvalidOperation(Resources.ReadAsMimeMultipartStreamProviderReadOnly, (object) this._streamProvider.GetType().Name, (object) MimeBodyPart._streamType.Name);
      }
      return this._outputStream;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.CleanupOutputStream();
      this.CleanupHttpContent();
      this._parentContent = (HttpContent) null;
      this.HeaderParser = (InternetMessageFormatHeaderParser) null;
      this.Segments.Clear();
    }

    private void CleanupHttpContent()
    {
      if (!this.IsComplete && this._content != null)
        this._content.Dispose();
      this._content = (HttpContent) null;
    }

    private void CleanupOutputStream()
    {
      if (this._outputStream == null)
        return;
      if (this._outputStream is MemoryStream outputStream)
        outputStream.Position = 0L;
      else
        this._outputStream.Close();
      this._outputStream = (Stream) null;
    }
  }
}
