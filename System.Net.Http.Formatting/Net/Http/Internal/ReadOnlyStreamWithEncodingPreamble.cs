// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Internal.ReadOnlyStreamWithEncodingPreamble
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Internal
{
  internal class ReadOnlyStreamWithEncodingPreamble : Stream
  {
    private static Task<int> _cancelledTask = ReadOnlyStreamWithEncodingPreamble.GetCancelledTask();
    private Stream _innerStream;
    private ArraySegment<byte> _remainingBytes;

    public ReadOnlyStreamWithEncodingPreamble(Stream innerStream, Encoding encoding)
    {
      this._innerStream = innerStream;
      byte[] preamble = encoding.GetPreamble();
      int length1 = preamble.Length;
      if (length1 <= 0)
        return;
      int length2 = length1 * 2;
      byte[] array = new byte[length2];
      int count = length1;
      preamble.CopyTo((Array) array, 0);
      for (; count < length2; ++count)
      {
        int num = innerStream.ReadByte();
        if (num != -1)
          array[count] = (byte) num;
        else
          break;
      }
      if (count == length2)
      {
        bool flag = true;
        for (int index = 0; index < length1; ++index)
        {
          if ((int) array[index] != (int) array[index + length1])
          {
            flag = false;
            break;
          }
        }
        if (flag)
          count = length1;
      }
      this._remainingBytes = new ArraySegment<byte>(array, 0, count);
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => throw new NotImplementedException();

    public override long Position
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public override void Flush() => throw new NotImplementedException();

    private static Task<int> GetCancelledTask()
    {
      TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
      completionSource.SetCanceled();
      return completionSource.Task;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      byte[] array = this._remainingBytes.Array;
      if (array == null)
        return this._innerStream.Read(buffer, offset, count);
      int count1 = this._remainingBytes.Count;
      int offset1 = this._remainingBytes.Offset;
      int num = Math.Min(count, count1);
      for (int index = 0; index < num; ++index)
        buffer[offset + index] = array[offset1 + index];
      this._remainingBytes = num != count1 ? new ArraySegment<byte>(array, offset1 + num, count1 - num) : new ArraySegment<byte>();
      return num;
    }

    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      if (this._remainingBytes.Array == null)
        return this._innerStream.ReadAsync(buffer, offset, count, cancellationToken);
      return cancellationToken.IsCancellationRequested ? ReadOnlyStreamWithEncodingPreamble._cancelledTask : Task.FromResult<int>(this.Read(buffer, offset, count));
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
  }
}
