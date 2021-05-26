// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetIterator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class manages the native change set iterator.  It is used as the
  /// base class for the <see cref="T:System.Data.SQLite.SQLiteMemoryChangeSetIterator" /> and
  /// <see cref="T:System.Data.SQLite.SQLiteStreamChangeSetIterator" /> classes.  It knows how to
  /// advance the native iterator handle as well as finalize it.
  /// </summary>
  internal class SQLiteChangeSetIterator : IDisposable
  {
    /// <summary>The native change set (a.k.a. iterator) handle.</summary>
    private IntPtr iterator;
    /// <summary>
    /// Non-zero if this instance owns the native iterator handle in the
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetIterator.iterator" /> field.  In that case, this instance will
    /// finalize the native iterator handle upon being disposed or
    /// finalized.
    /// </summary>
    private bool ownHandle;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified native
    /// iterator handle.
    /// </summary>
    /// <param name="iterator">The native iterator handle to use.</param>
    /// <param name="ownHandle">
    /// Non-zero if this instance is to take ownership of the native
    /// iterator handle specified by <paramref name="iterator" />.
    /// </param>
    protected SQLiteChangeSetIterator(IntPtr iterator, bool ownHandle)
    {
      this.iterator = iterator;
      this.ownHandle = ownHandle;
    }

    /// <summary>
    /// Throws an exception if the native iterator handle is invalid.
    /// </summary>
    internal void CheckHandle()
    {
      if (this.iterator == IntPtr.Zero)
        throw new InvalidOperationException("iterator is not open");
    }

    /// <summary>
    /// Used to query the native iterator handle.  This method is only used
    /// by the <see cref="T:System.Data.SQLite.SQLiteChangeSetMetadataItem" /> class.
    /// </summary>
    /// <returns>
    /// The native iterator handle -OR- <see cref="F:System.IntPtr.Zero" /> if it
    /// is not available.
    /// </returns>
    internal IntPtr GetIntPtr() => this.iterator;

    /// <summary>
    /// Attempts to advance the native iterator handle to its next item.
    /// </summary>
    /// <returns>
    /// Non-zero if the native iterator handle was advanced and contains
    /// more data; otherwise, zero.  If the underlying native API returns
    /// an unexpected value then an exception will be thrown.
    /// </returns>
    public bool Next()
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_next(this.iterator);
      switch (errorCode)
      {
        case SQLiteErrorCode.Ok:
          throw new SQLiteException(SQLiteErrorCode.Ok, "sqlite3changeset_next: unexpected result Ok");
        case SQLiteErrorCode.Row:
          return true;
        case SQLiteErrorCode.Done:
          return false;
        default:
          throw new SQLiteException(errorCode, "sqlite3changeset_next");
      }
    }

    /// <summary>
    /// Attempts to create an instance of this class that is associated
    /// with the specified native iterator handle.  Ownership of the
    /// native iterator handle is NOT transferred to the new instance of
    /// this class.
    /// </summary>
    /// <param name="iterator">The native iterator handle to use.</param>
    /// <returns>
    /// The new instance of this class.  No return value is reserved to
    /// indicate an error; however, if the native iterator handle is not
    /// valid, any subsequent attempt to make use of it via the returned
    /// instance of this class may throw exceptions.
    /// </returns>
    public static SQLiteChangeSetIterator Attach(IntPtr iterator) => new SQLiteChangeSetIterator(iterator, false);

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
        throw new ObjectDisposedException(typeof (SQLiteChangeSetIterator).Name);
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
        if (this.disposed || !(this.iterator != IntPtr.Zero))
          return;
        if (this.ownHandle)
        {
          int num = (int) UnsafeNativeMethods.sqlite3changeset_finalize(this.iterator);
        }
        this.iterator = IntPtr.Zero;
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteChangeSetIterator() => this.Dispose(false);
  }
}
