// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBlobHandle
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
  internal sealed class SQLiteBlobHandle : CriticalHandle
  {
    private SQLiteConnectionHandle cnn;

    public static implicit operator IntPtr(SQLiteBlobHandle blob) => blob != null ? blob.handle : IntPtr.Zero;

    internal SQLiteBlobHandle(SQLiteConnectionHandle cnn, IntPtr blob)
      : this()
    {
      this.cnn = cnn;
      this.SetHandle(blob);
    }

    private SQLiteBlobHandle()
      : base(IntPtr.Zero)
    {
    }

    protected override bool ReleaseHandle()
    {
      try
      {
        IntPtr blob = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
        if (blob != IntPtr.Zero)
          SQLiteBase.CloseBlob(this.cnn, blob);
      }
      catch (SQLiteException ex)
      {
      }
      finally
      {
        this.SetHandleAsInvalid();
      }
      return true;
    }

    public override bool IsInvalid => this.handle == IntPtr.Zero;
  }
}
