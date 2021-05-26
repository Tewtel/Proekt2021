// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Internal.ByteRangeStream
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http.Internal
{
  internal class ByteRangeStream : DelegatingStream
  {
    private readonly long _lowerbounds;
    private readonly long _totalCount;
    private long _currentCount;

    public ByteRangeStream(Stream innerStream, RangeItemHeaderValue range)
      : base(innerStream)
    {
      if (range == null)
        throw Error.ArgumentNull(nameof (range));
      if (!innerStream.CanSeek)
        throw Error.Argument(nameof (innerStream), Resources.ByteRangeStreamNotSeekable, (object) typeof (ByteRangeStream).Name);
      if (innerStream.Length < 1L)
        throw Error.ArgumentOutOfRange(nameof (innerStream), (object) innerStream.Length, Resources.ByteRangeStreamEmpty, (object) typeof (ByteRangeStream).Name);
      if (range.From.HasValue && range.From.Value > innerStream.Length)
        throw Error.ArgumentOutOfRange(nameof (range), (object) range.From, Resources.ByteRangeStreamInvalidFrom, (object) innerStream.Length);
      long val2 = innerStream.Length - 1L;
      long to;
      if (range.To.HasValue)
      {
        if (range.From.HasValue)
        {
          to = Math.Min(range.To.Value, val2);
          this._lowerbounds = range.From.Value;
        }
        else
        {
          to = val2;
          this._lowerbounds = Math.Max(innerStream.Length - range.To.Value, 0L);
        }
      }
      else if (range.From.HasValue)
      {
        to = val2;
        this._lowerbounds = range.From.Value;
      }
      else
      {
        to = val2;
        this._lowerbounds = 0L;
      }
      this._totalCount = to - this._lowerbounds + 1L;
      this.ContentRange = new ContentRangeHeaderValue(this._lowerbounds, to, innerStream.Length);
    }

    public ContentRangeHeaderValue ContentRange { get; private set; }

    public override long Length => this._totalCount;

    public override bool CanWrite => false;

    public override long Position
    {
      get => this._currentCount;
      set => this._currentCount = value >= 0L ? value : throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (value), (object) value, (object) 0L);
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return base.BeginRead(buffer, offset, this.PrepareStreamForRangeRead(count), callback, state);
    }

    public override int Read(byte[] buffer, int offset, int count) => base.Read(buffer, offset, this.PrepareStreamForRangeRead(count));

    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return base.ReadAsync(buffer, offset, this.PrepareStreamForRangeRead(count), cancellationToken);
    }

    public override int ReadByte() => this.PrepareStreamForRangeRead(1) <= 0 ? -1 : base.ReadByte();

    public override long Seek(long offset, SeekOrigin origin)
    {
      switch (origin)
      {
        case SeekOrigin.Begin:
          this._currentCount = offset;
          break;
        case SeekOrigin.Current:
          this._currentCount += offset;
          break;
        case SeekOrigin.End:
          this._currentCount = this._totalCount + offset;
          break;
        default:
          throw Error.InvalidEnumArgument(nameof (origin), (int) origin, typeof (SeekOrigin));
      }
      return this._currentCount >= 0L ? this._currentCount : throw new IOException(Resources.ByteRangeStreamInvalidOffset);
    }

    public override void SetLength(long value) => throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);

    public override void Write(byte[] buffer, int offset, int count) => throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);
    }

    public override void EndWrite(IAsyncResult asyncResult) => throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);

    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);
    }

    public override void WriteByte(byte value) => throw Error.NotSupported(Resources.ByteRangeStreamReadOnly);

    private int PrepareStreamForRangeRead(int count)
    {
      if (count <= 0)
        return count;
      if (this._currentCount >= this._totalCount)
        return 0;
      long num1 = Math.Min((long) count, this._totalCount - this._currentCount);
      long num2 = this._lowerbounds + this._currentCount;
      long position = this.InnerStream.Position;
      if (num2 != position)
        this.InnerStream.Position = num2;
      this._currentCount += num1;
      return (int) num1;
    }
  }
}
