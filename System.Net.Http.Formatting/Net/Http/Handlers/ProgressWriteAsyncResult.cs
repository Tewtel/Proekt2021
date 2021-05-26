// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Handlers.ProgressWriteAsyncResult
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Internal;

namespace System.Net.Http.Handlers
{
  internal class ProgressWriteAsyncResult : AsyncResult
  {
    private static readonly AsyncCallback _writeCompletedCallback = new AsyncCallback(ProgressWriteAsyncResult.WriteCompletedCallback);
    private readonly Stream _innerStream;
    private readonly ProgressStream _progressStream;
    private readonly int _count;

    public ProgressWriteAsyncResult(
      Stream innerStream,
      ProgressStream progressStream,
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
      : base(callback, state)
    {
      this._innerStream = innerStream;
      this._progressStream = progressStream;
      this._count = count;
      try
      {
        IAsyncResult result = innerStream.BeginWrite(buffer, offset, count, ProgressWriteAsyncResult._writeCompletedCallback, (object) this);
        if (!result.CompletedSynchronously)
          return;
        this.WriteCompleted(result);
      }
      catch (Exception ex)
      {
        this.Complete(true, ex);
      }
    }

    private static void WriteCompletedCallback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      ProgressWriteAsyncResult asyncState = (ProgressWriteAsyncResult) result.AsyncState;
      try
      {
        asyncState.WriteCompleted(result);
      }
      catch (Exception ex)
      {
        asyncState.Complete(false, ex);
      }
    }

    private void WriteCompleted(IAsyncResult result)
    {
      this._innerStream.EndWrite(result);
      this._progressStream.ReportBytesSent(this._count, this.AsyncState);
      this.Complete(result.CompletedSynchronously);
    }

    public static void End(IAsyncResult result) => AsyncResult.End<ProgressWriteAsyncResult>(result);
  }
}
