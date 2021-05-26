// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Internal.DelegatingStream
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Internal
{
  internal abstract class DelegatingStream : Stream
  {
    private readonly Stream _innerStream;

    protected DelegatingStream(Stream innerStream) => this._innerStream = innerStream != null ? innerStream : throw Error.ArgumentNull(nameof (innerStream));

    protected Stream InnerStream => this._innerStream;

    public override bool CanRead => this._innerStream.CanRead;

    public override bool CanSeek => this._innerStream.CanSeek;

    public override bool CanWrite => this._innerStream.CanWrite;

    public override long Length => this._innerStream.Length;

    public override long Position
    {
      get => this._innerStream.Position;
      set => this._innerStream.Position = value;
    }

    public override int ReadTimeout
    {
      get => this._innerStream.ReadTimeout;
      set => this._innerStream.ReadTimeout = value;
    }

    public override bool CanTimeout => this._innerStream.CanTimeout;

    public override int WriteTimeout
    {
      get => this._innerStream.WriteTimeout;
      set => this._innerStream.WriteTimeout = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this._innerStream.Dispose();
      base.Dispose(disposing);
    }

    public override long Seek(long offset, SeekOrigin origin) => this._innerStream.Seek(offset, origin);

    public override int Read(byte[] buffer, int offset, int count) => this._innerStream.Read(buffer, offset, count);

    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this._innerStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this._innerStream.BeginRead(buffer, offset, count, callback, state);
    }

    public override int EndRead(IAsyncResult asyncResult) => this._innerStream.EndRead(asyncResult);

    public override int ReadByte() => this._innerStream.ReadByte();

    public override void Flush() => this._innerStream.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken) => this._innerStream.FlushAsync(cancellationToken);

    public override void SetLength(long value) => this._innerStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) => this._innerStream.Write(buffer, offset, count);

    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this._innerStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this._innerStream.BeginWrite(buffer, offset, count, callback, state);
    }

    public override void EndWrite(IAsyncResult asyncResult) => this._innerStream.EndWrite(asyncResult);

    public override void WriteByte(byte value) => this._innerStream.WriteByte(value);
  }
}
