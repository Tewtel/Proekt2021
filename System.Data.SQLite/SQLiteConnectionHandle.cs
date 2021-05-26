// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionHandle
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.SQLite
{
  internal sealed class SQLiteConnectionHandle : CriticalHandle
  {
    private bool ownHandle;

    public static implicit operator IntPtr(SQLiteConnectionHandle db) => db != null ? db.handle : IntPtr.Zero;

    internal SQLiteConnectionHandle(IntPtr db, bool ownHandle)
      : this(ownHandle)
    {
      this.ownHandle = ownHandle;
      this.SetHandle(db);
    }

    private SQLiteConnectionHandle(bool ownHandle)
      : base(IntPtr.Zero)
    {
    }

    protected override bool ReleaseHandle()
    {
      if (!this.ownHandle)
        return true;
      try
      {
        IntPtr db = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
        if (db != IntPtr.Zero)
          SQLiteBase.CloseConnection(this, db);
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

    public bool OwnHandle => this.ownHandle;

    public override bool IsInvalid => this.handle == IntPtr.Zero;
  }
}
