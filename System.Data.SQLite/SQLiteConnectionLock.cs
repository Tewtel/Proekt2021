// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionLock
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class is used to hold the native connection handle associated with
  /// a <see cref="T:System.Data.SQLite.SQLiteConnection" /> open until this subsystem is totally
  /// done with it.  This class is for internal use by this subsystem only.
  /// </summary>
  internal abstract class SQLiteConnectionLock : IDisposable
  {
    /// <summary>
    /// The SQL statement used when creating the native statement handle.
    /// There are no special requirements for this other than counting as
    /// an "open statement handle".
    /// </summary>
    private const string LockNopSql = "SELECT 1;";
    /// <summary>
    /// The format of the error message used when reporting, during object
    /// disposal, that the statement handle is still open (i.e. because
    /// this situation is considered a fairly serious programming error).
    /// </summary>
    private const string StatementMessageFormat = "Connection lock object was {0} with statement {1}";
    /// <summary>
    /// The wrapped native connection handle associated with this lock.
    /// </summary>
    private SQLiteConnectionHandle handle;
    /// <summary>
    /// The flags associated with the connection represented by the
    /// <see cref="F:System.Data.SQLite.SQLiteConnectionLock.handle" /> value.
    /// </summary>
    private SQLiteConnectionFlags flags;
    /// <summary>
    /// The native statement handle for this lock.  The garbage collector
    /// cannot cause this statement to be finalized; therefore, it will
    /// serve to hold the associated native connection open until it is
    /// freed manually using the <see cref="M:System.Data.SQLite.SQLiteConnectionLock.Unlock" /> method.
    /// </summary>
    private IntPtr statement;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified wrapped
    /// native connection handle and associated flags.
    /// </summary>
    /// <param name="handle">
    /// The wrapped native connection handle to be associated with this
    /// lock.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the connection represented by the
    /// <paramref name="handle" /> value.
    /// </param>
    /// <param name="autoLock">
    /// Non-zero if the <see cref="M:System.Data.SQLite.SQLiteConnectionLock.Lock" /> method should be called prior
    /// to returning from this constructor.
    /// </param>
    public SQLiteConnectionLock(
      SQLiteConnectionHandle handle,
      SQLiteConnectionFlags flags,
      bool autoLock)
    {
      this.handle = handle;
      this.flags = flags;
      if (!autoLock)
        return;
      this.Lock();
    }

    /// <summary>
    /// Queries and returns the wrapped native connection handle for this
    /// instance.
    /// </summary>
    /// <returns>
    /// The wrapped native connection handle for this instance -OR- null
    /// if it is unavailable.
    /// </returns>
    protected SQLiteConnectionHandle GetHandle() => this.handle;

    /// <summary>
    /// Queries and returns the flags associated with the connection for
    /// this instance.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteConnectionFlags" /> value.  There is no return
    /// value reserved to indicate an error.
    /// </returns>
    protected SQLiteConnectionFlags GetFlags() => this.flags;

    /// <summary>
    /// Queries and returns the native connection handle for this instance.
    /// </summary>
    /// <returns>
    /// The native connection handle for this instance.  If this value is
    /// unavailable or invalid an exception will be thrown.
    /// </returns>
    protected IntPtr GetIntPtr()
    {
      IntPtr num = this.handle != null ? (IntPtr) this.handle : throw new InvalidOperationException("Connection lock object has an invalid handle.");
      return !(num == IntPtr.Zero) ? num : throw new InvalidOperationException("Connection lock object has an invalid handle pointer.");
    }

    /// <summary>
    /// This method attempts to "lock" the associated native connection
    /// handle by preparing a SQL statement that will not be finalized
    /// until the <see cref="M:System.Data.SQLite.SQLiteConnectionLock.Unlock" /> method is called (i.e. and which
    /// cannot be done by the garbage collector).  If the statement is
    /// already prepared, nothing is done.  If the statement cannot be
    /// prepared for any reason, an exception will be thrown.
    /// </summary>
    public void Lock()
    {
      this.CheckDisposed();
      if (this.statement != IntPtr.Zero)
        return;
      IntPtr num = IntPtr.Zero;
      try
      {
        int length = 0;
        num = SQLiteString.Utf8IntPtrFromString("SELECT 1;", ref length);
        IntPtr zero = IntPtr.Zero;
        int nRemain = 0;
        string message = "sqlite3_prepare_interop";
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_prepare_interop(this.GetIntPtr(), num, length, ref this.statement, ref zero, ref nRemain);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, message);
      }
      finally
      {
        if (num != IntPtr.Zero)
        {
          SQLiteMemory.Free(num);
          IntPtr zero = IntPtr.Zero;
        }
      }
    }

    /// <summary>
    /// This method attempts to "unlock" the associated native connection
    /// handle by finalizing the previously prepared statement.  If the
    /// statement is already finalized, nothing is done.  If the statement
    /// cannot be finalized for any reason, an exception will be thrown.
    /// </summary>
    public void Unlock()
    {
      this.CheckDisposed();
      if (this.statement == IntPtr.Zero)
        return;
      string message = "sqlite3_finalize_interop";
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_finalize_interop(this.statement);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, message);
      this.statement = IntPtr.Zero;
    }

    /// <summary>Disposes of this object instance.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteConnectionLock).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed)
          return;
        if (!(this.statement != IntPtr.Zero))
          return;
        try
        {
          if (!HelperMethods.LogPrepare(this.GetFlags()))
            return;
          SQLiteLog.LogMessage(SQLiteErrorCode.Misuse, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Connection lock object was {0} with statement {1}", disposing ? (object) "disposed" : (object) "finalized", (object) this.statement));
        }
        catch
        {
        }
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteConnectionLock() => this.Dispose(false);
  }
}
