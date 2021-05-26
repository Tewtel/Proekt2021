// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Handlers.ProgressStream
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Handlers
{
  internal class ProgressStream : System.Net.Http.Internal.DelegatingStream
  {
    private readonly ProgressMessageHandler _handler;
    private readonly HttpRequestMessage _request;
    private long _bytesReceived;
    private long? _totalBytesToReceive;
    private long _bytesSent;
    private long? _totalBytesToSend;

    public ProgressStream(
      Stream innerStream,
      ProgressMessageHandler handler,
      HttpRequestMessage request,
      HttpResponseMessage response)
      : base(innerStream)
    {
      if (request.Content != null)
        this._totalBytesToSend = request.Content.Headers.ContentLength;
      if (response != null && response.Content != null)
        this._totalBytesToReceive = response.Content.Headers.ContentLength;
      this._handler = handler;
      this._request = request;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int bytesReceived = this.InnerStream.Read(buffer, offset, count);
      this.ReportBytesReceived(bytesReceived, (object) null);
      return bytesReceived;
    }

    public override int ReadByte()
    {
      int num = this.InnerStream.ReadByte();
      this.ReportBytesReceived(num == -1 ? 0 : 1, (object) null);
      return num;
    }

    public override async Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      ProgressStream progressStream = this;
      int bytesReceived = await progressStream.InnerStream.ReadAsync(buffer, offset, count, cancellationToken);
      progressStream.ReportBytesReceived(bytesReceived, (object) null);
      return bytesReceived;
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.InnerStream.BeginRead(buffer, offset, count, callback, state);
    }

    public override int EndRead(IAsyncResult asyncResult)
    {
      int bytesReceived = this.InnerStream.EndRead(asyncResult);
      this.ReportBytesReceived(bytesReceived, asyncResult.AsyncState);
      return bytesReceived;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.InnerStream.Write(buffer, offset, count);
      this.ReportBytesSent(count, (object) null);
    }

    public override void WriteByte(byte value)
    {
      this.InnerStream.WriteByte(value);
      this.ReportBytesSent(1, (object) null);
    }

    public override async Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      ProgressStream progressStream = this;
      await progressStream.InnerStream.WriteAsync(buffer, offset, count, cancellationToken);
      progressStream.ReportBytesSent(count, (object) null);
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return (IAsyncResult) new ProgressWriteAsyncResult(this.InnerStream, this, buffer, offset, count, callback, state);
    }

    public override void EndWrite(IAsyncResult asyncResult) => ProgressWriteAsyncResult.End(asyncResult);

    internal void ReportBytesSent(int bytesSent, object userState)
    {
      if (bytesSent <= 0)
        return;
      this._bytesSent += (long) bytesSent;
      int progressPercentage = 0;
      if (this._totalBytesToSend.HasValue)
      {
        long? totalBytesToSend1 = this._totalBytesToSend;
        long num1 = 0;
        if (!(totalBytesToSend1.GetValueOrDefault() == num1 & totalBytesToSend1.HasValue))
        {
          long num2 = 100L * this._bytesSent;
          long? totalBytesToSend2 = this._totalBytesToSend;
          progressPercentage = (int) (totalBytesToSend2.HasValue ? new long?(num2 / totalBytesToSend2.GetValueOrDefault()) : new long?()).Value;
        }
      }
      this._handler.OnHttpRequestProgress(this._request, new HttpProgressEventArgs(progressPercentage, userState, this._bytesSent, this._totalBytesToSend));
    }

    private void ReportBytesReceived(int bytesReceived, object userState)
    {
      if (bytesReceived <= 0)
        return;
      this._bytesReceived += (long) bytesReceived;
      int progressPercentage = 0;
      if (this._totalBytesToReceive.HasValue)
      {
        long? totalBytesToReceive1 = this._totalBytesToReceive;
        long num1 = 0;
        if (!(totalBytesToReceive1.GetValueOrDefault() == num1 & totalBytesToReceive1.HasValue))
        {
          long num2 = 100L * this._bytesReceived;
          long? totalBytesToReceive2 = this._totalBytesToReceive;
          progressPercentage = (int) (totalBytesToReceive2.HasValue ? new long?(num2 / totalBytesToReceive2.GetValueOrDefault()) : new long?()).Value;
        }
      }
      this._handler.OnHttpResponseProgress(this._request, new HttpProgressEventArgs(progressPercentage, userState, this._bytesReceived, this._totalBytesToReceive));
    }
  }
}
