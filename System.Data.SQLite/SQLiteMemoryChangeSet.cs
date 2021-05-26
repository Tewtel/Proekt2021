// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMemoryChangeSet
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a set of changes contained entirely in memory.
  /// </summary>
  internal sealed class SQLiteMemoryChangeSet : 
    SQLiteChangeSetBase,
    ISQLiteChangeSet,
    IEnumerable<ISQLiteChangeSetMetadataItem>,
    IEnumerable,
    IDisposable
  {
    /// <summary>
    /// The raw byte data for this set of changes.  Since this data must
    /// be marshalled to a native memory buffer before being used, there
    /// must be enough memory available to store at least two times the
    /// amount of data contained within it.
    /// </summary>
    private byte[] rawData;
    /// <summary>The flags used to create the change set iterator.</summary>
    private SQLiteChangeSetStartFlags startFlags;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified raw byte
    /// data and wrapped native connection handle.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data for the specified change set (or patch set).
    /// </param>
    /// <param name="handle">
    /// The wrapped native connection handle to be associated with this
    /// set of changes.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the connection represented by the
    /// <paramref name="handle" /> value.
    /// </param>
    internal SQLiteMemoryChangeSet(
      byte[] rawData,
      SQLiteConnectionHandle handle,
      SQLiteConnectionFlags connectionFlags)
      : base(handle, connectionFlags)
    {
      this.rawData = rawData;
      this.startFlags = SQLiteChangeSetStartFlags.None;
    }

    /// <summary>
    /// Constructs an instance of this class using the specified raw byte
    /// data and wrapped native connection handle.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data for the specified change set (or patch set).
    /// </param>
    /// <param name="handle">
    /// The wrapped native connection handle to be associated with this
    /// set of changes.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the connection represented by the
    /// <paramref name="handle" /> value.
    /// </param>
    /// <param name="startFlags">
    /// The flags used to create the change set iterator.
    /// </param>
    internal SQLiteMemoryChangeSet(
      byte[] rawData,
      SQLiteConnectionHandle handle,
      SQLiteConnectionFlags connectionFlags,
      SQLiteChangeSetStartFlags startFlags)
      : base(handle, connectionFlags)
    {
      this.rawData = rawData;
      this.startFlags = startFlags;
    }

    /// <summary>
    /// This method "inverts" the set of changes within this instance.
    /// Applying an inverted set of changes to a database reverses the
    /// effects of applying the uninverted changes.  Specifically:
    /// <![CDATA[<ul>]]><![CDATA[<li>]]>
    /// Each DELETE change is changed to an INSERT, and
    /// <![CDATA[</li>]]><![CDATA[<li>]]>
    /// Each INSERT change is changed to a DELETE, and
    /// <![CDATA[</li>]]><![CDATA[<li>]]>
    /// For each UPDATE change, the old.* and new.* values are exchanged.
    /// <![CDATA[</li>]]><![CDATA[</ul>]]>
    /// This method does not change the order in which changes appear
    /// within the set of changes. It merely reverses the sense of each
    /// individual change.
    /// </summary>
    /// <returns>
    /// The new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> instance that represents
    /// the resulting set of changes.
    /// </returns>
    public ISQLiteChangeSet Invert()
    {
      this.CheckDisposed();
      SQLiteSessionHelpers.CheckRawData(this.rawData);
      IntPtr num = IntPtr.Zero;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int length = 0;
        num = SQLiteBytes.ToIntPtr(this.rawData, ref length);
        int nOut = 0;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_invert(length, num, ref nOut, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_invert");
        return (ISQLiteChangeSet) new SQLiteMemoryChangeSet(SQLiteBytes.FromIntPtr(zero1, nOut), this.GetHandle(), this.GetFlags());
      }
      finally
      {
        if (zero1 != IntPtr.Zero)
        {
          SQLiteMemory.FreeUntracked(zero1);
          IntPtr zero2 = IntPtr.Zero;
        }
        if (num != IntPtr.Zero)
        {
          SQLiteMemory.Free(num);
          IntPtr zero2 = IntPtr.Zero;
        }
      }
    }

    /// <summary>
    /// This method combines the specified set of changes with the ones
    /// contained in this instance.
    /// </summary>
    /// <param name="changeSet">
    /// The changes to be combined with those in this instance.
    /// </param>
    /// <returns>
    /// The new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> instance that represents
    /// the resulting set of changes.
    /// </returns>
    public ISQLiteChangeSet CombineWith(ISQLiteChangeSet changeSet)
    {
      this.CheckDisposed();
      SQLiteSessionHelpers.CheckRawData(this.rawData);
      if (!(changeSet is SQLiteMemoryChangeSet liteMemoryChangeSet))
        throw new ArgumentException("not a memory based change set", nameof (changeSet));
      SQLiteSessionHelpers.CheckRawData(liteMemoryChangeSet.rawData);
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int length1 = 0;
        num1 = SQLiteBytes.ToIntPtr(this.rawData, ref length1);
        int length2 = 0;
        num2 = SQLiteBytes.ToIntPtr(liteMemoryChangeSet.rawData, ref length2);
        int nOut = 0;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_concat(length1, num1, length2, num2, ref nOut, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_concat");
        return (ISQLiteChangeSet) new SQLiteMemoryChangeSet(SQLiteBytes.FromIntPtr(zero1, nOut), this.GetHandle(), this.GetFlags());
      }
      finally
      {
        if (zero1 != IntPtr.Zero)
        {
          SQLiteMemory.FreeUntracked(zero1);
          IntPtr zero2 = IntPtr.Zero;
        }
        if (num2 != IntPtr.Zero)
        {
          SQLiteMemory.Free(num2);
          IntPtr zero2 = IntPtr.Zero;
        }
        if (num1 != IntPtr.Zero)
        {
          SQLiteMemory.Free(num1);
          IntPtr zero2 = IntPtr.Zero;
        }
      }
    }

    /// <summary>
    /// Attempts to apply the set of changes in this instance to the
    /// associated database.
    /// </summary>
    /// <param name="conflictCallback">
    /// The <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate that will need
    /// to handle any conflicting changes that may arise.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    public void Apply(SessionConflictCallback conflictCallback, object clientData)
    {
      this.CheckDisposed();
      this.Apply(conflictCallback, (SessionTableFilterCallback) null, clientData);
    }

    /// <summary>
    /// Attempts to apply the set of changes in this instance to the
    /// associated database.
    /// </summary>
    /// <param name="conflictCallback">
    /// The <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate that will need
    /// to handle any conflicting changes that may arise.
    /// </param>
    /// <param name="tableFilterCallback">
    /// The optional <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> delegate
    /// that can be used to filter the list of tables impacted by the set
    /// of changes.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    public void Apply(
      SessionConflictCallback conflictCallback,
      SessionTableFilterCallback tableFilterCallback,
      object clientData)
    {
      this.CheckDisposed();
      SQLiteSessionHelpers.CheckRawData(this.rawData);
      if (conflictCallback == null)
        throw new ArgumentNullException(nameof (conflictCallback));
      UnsafeNativeMethods.xSessionFilter xFilter = this.GetDelegate(tableFilterCallback, clientData);
      UnsafeNativeMethods.xSessionConflict xConflict = this.GetDelegate(conflictCallback, clientData);
      IntPtr num = IntPtr.Zero;
      try
      {
        int length = 0;
        num = SQLiteBytes.ToIntPtr(this.rawData, ref length);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_apply(this.GetIntPtr(), length, num, xFilter, xConflict, IntPtr.Zero);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changeset_apply");
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
    /// Creates an <see cref="T:System.Collections.IEnumerator" /> capable of iterating over the
    /// items within this set of changes.
    /// </summary>
    /// <returns>
    /// The new <see cref="T:System.Collections.Generic.IEnumerator`1" />
    /// instance.
    /// </returns>
    public IEnumerator<ISQLiteChangeSetMetadataItem> GetEnumerator() => this.startFlags != SQLiteChangeSetStartFlags.None ? (IEnumerator<ISQLiteChangeSetMetadataItem>) new SQLiteMemoryChangeSetEnumerator(this.rawData, this.startFlags) : (IEnumerator<ISQLiteChangeSetMetadataItem>) new SQLiteMemoryChangeSetEnumerator(this.rawData);

    /// <summary>
    /// Creates an <see cref="T:System.Collections.IEnumerator" /> capable of iterating over the
    /// items within this set of changes.
    /// </summary>
    /// <returns>
    /// The new <see cref="T:System.Collections.IEnumerator" /> instance.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteMemoryChangeSet).Name);
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
        if (this.disposed || !disposing || this.rawData == null)
          return;
        this.rawData = (byte[]) null;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
