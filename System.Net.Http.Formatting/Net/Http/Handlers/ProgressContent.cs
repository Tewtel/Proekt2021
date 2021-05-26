// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Handlers.ProgressContent
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Threading.Tasks;

namespace System.Net.Http.Handlers
{
  internal class ProgressContent : HttpContent
  {
    private readonly HttpContent _innerContent;
    private readonly ProgressMessageHandler _handler;
    private readonly HttpRequestMessage _request;

    public ProgressContent(
      HttpContent innerContent,
      ProgressMessageHandler handler,
      HttpRequestMessage request)
    {
      this._innerContent = innerContent;
      this._handler = handler;
      this._request = request;
      innerContent.Headers.CopyTo(this.Headers);
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => this._innerContent.CopyToAsync((Stream) new ProgressStream(stream, this._handler, this._request, (HttpResponseMessage) null));

    protected override bool TryComputeLength(out long length)
    {
      long? contentLength = this._innerContent.Headers.ContentLength;
      if (contentLength.HasValue)
      {
        length = contentLength.Value;
        return true;
      }
      length = -1L;
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this._innerContent.Dispose();
    }
  }
}
