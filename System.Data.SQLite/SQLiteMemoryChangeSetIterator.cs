// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMemoryChangeSetIterator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class manages the native change set iterator for a set of changes
  /// contained entirely in memory.
  /// </summary>
  internal sealed class SQLiteMemoryChangeSetIterator : SQLiteChangeSetIterator
  {
    /// <summary>
    /// The native memory buffer allocated to contain the set of changes
    /// associated with this instance.  This will always be freed when this
    /// instance is disposed or finalized.
    /// </summary>
    private IntPtr pData;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// memory buffer and native iterator handle.
    /// </summary>
    /// <param name="pData">The native memory buffer to use.</param>
    /// <param name="iterator">The native iterator handle to use.</param>
    /// <param name="ownHandle">
    /// Non-zero if this instance is to take ownership of the native
    /// iterator handle specified by <paramref name="iterator" />.
    /// </param>
    private SQLiteMemoryChangeSetIterator(IntPtr pData, IntPtr iterator, bool ownHandle)
      : base(iterator, ownHandle)
    {
      this.pData = pData;
    }

    /// <summary>
    /// Attempts to create an instance of this class using the specified
    /// raw byte data.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data containing the set of changes for this native
    /// iterator.
    /// </param>
    /// <returns>
    /// The new instance of this class -OR- null if it cannot be created.
    /// </returns>
    public static SQLiteMemoryChangeSetIterator Create(byte[] rawData)
    {
      SQLiteSessionHelpers.CheckRawData(rawData);
      SQLiteMemoryChangeSetIterator changeSetIterator = (SQLiteMemoryChangeSetIterator) null;
      IntPtr num1 = IntPtr.Zero;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int length = 0;
        num1 = SQLiteBytes.ToIntPtr(rawData, ref length);
        if (num1 == IntPtr.Zero)
          throw new SQLiteException(SQLiteErrorCode.NoMem, (string) null);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_start(ref zero1, length, num1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_start");
        changeSetIterator = new SQLiteMemoryChangeSetIterator(num1, zero1, true);
        return changeSetIterator;
      }
      finally
      {
        if (changeSetIterator == null)
        {
          if (zero1 != IntPtr.Zero)
          {
            int num2 = (int) UnsafeNativeMethods.sqlite3changeset_finalize(zero1);
            IntPtr zero2 = IntPtr.Zero;
          }
          if (num1 != IntPtr.Zero)
          {
            SQLiteMemory.Free(num1);
            IntPtr zero2 = IntPtr.Zero;
          }
        }
      }
    }

    /// <summary>
    /// Attempts to create an instance of this class using the specified
    /// raw byte data.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data containing the set of changes for this native
    /// iterator.
    /// </param>
    /// <param name="flags">
    /// The flags used to create the change set iterator.
    /// </param>
    /// <returns>
    /// The new instance of this class -OR- null if it cannot be created.
    /// </returns>
    public static SQLiteMemoryChangeSetIterator Create(
      byte[] rawData,
      SQLiteChangeSetStartFlags flags)
    {
      SQLiteSessionHelpers.CheckRawData(rawData);
      SQLiteMemoryChangeSetIterator changeSetIterator = (SQLiteMemoryChangeSetIterator) null;
      IntPtr num1 = IntPtr.Zero;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int length = 0;
        num1 = SQLiteBytes.ToIntPtr(rawData, ref length);
        if (num1 == IntPtr.Zero)
          throw new SQLiteException(SQLiteErrorCode.NoMem, (string) null);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_start_v2(ref zero1, length, num1, flags);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_start_v2");
        changeSetIterator = new SQLiteMemoryChangeSetIterator(num1, zero1, true);
        return changeSetIterator;
      }
      finally
      {
        if (changeSetIterator == null)
        {
          if (zero1 != IntPtr.Zero)
          {
            int num2 = (int) UnsafeNativeMethods.sqlite3changeset_finalize(zero1);
            IntPtr zero2 = IntPtr.Zero;
          }
          if (num1 != IntPtr.Zero)
          {
            SQLiteMemory.Free(num1);
            IntPtr zero2 = IntPtr.Zero;
          }
        }
      }
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteMemoryChangeSetIterator).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      try
      {
        if (this.disposed || !(this.pData != IntPtr.Zero))
          return;
        SQLiteMemory.Free(this.pData);
        this.pData = IntPtr.Zero;
      }
      finally
      {
        this.disposed = true;
      }
    }
  }
}
