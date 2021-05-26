// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStreamChangeSetIterator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class manages the native change set iterator for a set of changes
  /// backed by a <see cref="T:System.IO.Stream" /> instance.
  /// </summary>
  internal sealed class SQLiteStreamChangeSetIterator : SQLiteChangeSetIterator
  {
    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance that is managing
    /// the underlying <see cref="T:System.IO.Stream" /> used as the backing store for
    /// the set of changes associated with this native change set iterator.
    /// </summary>
    private SQLiteStreamAdapter streamAdapter;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// iterator handle and <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" />.
    /// </summary>
    /// <param name="streamAdapter">
    /// The <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance to use.
    /// </param>
    /// <param name="iterator">The native iterator handle to use.</param>
    /// <param name="ownHandle">
    /// Non-zero if this instance is to take ownership of the native
    /// iterator handle specified by <paramref name="iterator" />.
    /// </param>
    private SQLiteStreamChangeSetIterator(
      SQLiteStreamAdapter streamAdapter,
      IntPtr iterator,
      bool ownHandle)
      : base(iterator, ownHandle)
    {
      this.streamAdapter = streamAdapter;
    }

    /// <summary>
    /// Attempts to create an instance of this class using the specified
    /// <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> where the raw byte data for the set of
    /// changes may be read.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the parent connection.
    /// </param>
    /// <returns>
    /// The new instance of this class -OR- null if it cannot be created.
    /// </returns>
    public static SQLiteStreamChangeSetIterator Create(
      Stream stream,
      SQLiteConnectionFlags connectionFlags)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      SQLiteStreamAdapter streamAdapter = (SQLiteStreamAdapter) null;
      SQLiteStreamChangeSetIterator changeSetIterator = (SQLiteStreamChangeSetIterator) null;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        streamAdapter = new SQLiteStreamAdapter(stream, connectionFlags);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_start_strm(ref zero1, streamAdapter.GetInputDelegate(), IntPtr.Zero);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_start_strm");
        changeSetIterator = new SQLiteStreamChangeSetIterator(streamAdapter, zero1, true);
        return changeSetIterator;
      }
      finally
      {
        if (changeSetIterator == null)
        {
          if (zero1 != IntPtr.Zero)
          {
            int num = (int) UnsafeNativeMethods.sqlite3changeset_finalize(zero1);
            IntPtr zero2 = IntPtr.Zero;
          }
          streamAdapter?.Dispose();
        }
      }
    }

    /// <summary>
    /// Attempts to create an instance of this class using the specified
    /// <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> where the raw byte data for the set of
    /// changes may be read.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the parent connection.
    /// </param>
    /// <param name="startFlags">
    /// The flags used to create the change set iterator.
    /// </param>
    /// <returns>
    /// The new instance of this class -OR- null if it cannot be created.
    /// </returns>
    public static SQLiteStreamChangeSetIterator Create(
      Stream stream,
      SQLiteConnectionFlags connectionFlags,
      SQLiteChangeSetStartFlags startFlags)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      SQLiteStreamAdapter streamAdapter = (SQLiteStreamAdapter) null;
      SQLiteStreamChangeSetIterator changeSetIterator = (SQLiteStreamChangeSetIterator) null;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        streamAdapter = new SQLiteStreamAdapter(stream, connectionFlags);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_start_v2_strm(ref zero1, streamAdapter.GetInputDelegate(), IntPtr.Zero, startFlags);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_start_v2_strm");
        changeSetIterator = new SQLiteStreamChangeSetIterator(streamAdapter, zero1, true);
        return changeSetIterator;
      }
      finally
      {
        if (changeSetIterator == null)
        {
          if (zero1 != IntPtr.Zero)
          {
            int num = (int) UnsafeNativeMethods.sqlite3changeset_finalize(zero1);
            IntPtr zero2 = IntPtr.Zero;
          }
          streamAdapter?.Dispose();
        }
      }
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteStreamChangeSetIterator).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
