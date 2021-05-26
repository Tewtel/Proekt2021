// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeGroup
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a group of change sets (or patch sets).
  /// </summary>
  internal sealed class SQLiteChangeGroup : ISQLiteChangeGroup, IDisposable
  {
    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteSessionStreamManager" /> instance associated
    /// with this change group.
    /// </summary>
    private SQLiteSessionStreamManager streamManager;
    /// <summary>The flags associated with the connection.</summary>
    private SQLiteConnectionFlags flags;
    /// <summary>
    /// The native handle for this change group.  This will be deleted when
    /// this instance is disposed or finalized.
    /// </summary>
    private IntPtr changeGroup;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified
    /// connection flags.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection.
    /// </param>
    public SQLiteChangeGroup(SQLiteConnectionFlags flags)
    {
      this.flags = flags;
      this.InitializeHandle();
    }

    /// <summary>
    /// Throws an exception if the native change group handle is invalid.
    /// </summary>
    private void CheckHandle()
    {
      if (this.changeGroup == IntPtr.Zero)
        throw new InvalidOperationException("change group not open");
    }

    /// <summary>
    /// Makes sure the native change group handle is valid, creating it if
    /// necessary.
    /// </summary>
    private void InitializeHandle()
    {
      if (this.changeGroup != IntPtr.Zero)
        return;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changegroup_new(ref this.changeGroup);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changegroup_new");
    }

    /// <summary>
    /// Makes sure the <see cref="T:System.Data.SQLite.SQLiteSessionStreamManager" /> instance
    /// is available, creating it if necessary.
    /// </summary>
    private void InitializeStreamManager()
    {
      if (this.streamManager != null)
        return;
      this.streamManager = new SQLiteSessionStreamManager(this.flags);
    }

    /// <summary>
    /// Attempts to return a <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance
    /// suitable for the specified <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> instance.  If this value is null, a null
    /// value will be returned.
    /// </param>
    /// <returns>
    /// A <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance.  Typically, these
    /// are always freshly created; however, this method is designed to
    /// return the existing <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance
    /// associated with the specified stream, should one exist.
    /// </returns>
    private SQLiteStreamAdapter GetStreamAdapter(Stream stream)
    {
      this.InitializeStreamManager();
      return this.streamManager.GetAdapter(stream);
    }

    /// <summary>
    /// Attempts to add a change set (or patch set) to this change group
    /// instance.  The underlying data must be contained entirely within
    /// the <paramref name="rawData" /> byte array.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data for the specified change set (or patch set).
    /// </param>
    public void AddChangeSet(byte[] rawData)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteSessionHelpers.CheckRawData(rawData);
      IntPtr num = IntPtr.Zero;
      try
      {
        int length = 0;
        num = SQLiteBytes.ToIntPtr(rawData, ref length);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changegroup_add(this.changeGroup, length, num);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changegroup_add");
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
    /// Attempts to add a change set (or patch set) to this change group
    /// instance.  The underlying data will be read from the specified
    /// <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> instance containing the raw change set
    /// (or patch set) data to read.
    /// </param>
    public void AddChangeSet(Stream stream)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changegroup_add_strm(this.changeGroup, ((stream != null ? this.GetStreamAdapter(stream) : throw new ArgumentNullException(nameof (stream))) ?? throw new SQLiteException("could not get or create adapter for input stream")).GetInputDelegate(), IntPtr.Zero);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changegroup_add_strm");
    }

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// combined set of changes represented by this change group instance.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this change group instance.
    /// </param>
    public void CreateChangeSet(ref byte[] rawData)
    {
      this.CheckDisposed();
      this.CheckHandle();
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int nData = 0;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changegroup_output(this.changeGroup, ref nData, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3changegroup_output");
        rawData = SQLiteBytes.FromIntPtr(zero1, nData);
      }
      finally
      {
        if (zero1 != IntPtr.Zero)
        {
          SQLiteMemory.FreeUntracked(zero1);
          IntPtr zero2 = IntPtr.Zero;
        }
      }
    }

    /// <summary>
    /// Attempts to create and write, via <paramref name="stream" />, the
    /// combined set of changes represented by this change group instance.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this change
    /// group instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    public void CreateChangeSet(Stream stream)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changegroup_output_strm(this.changeGroup, ((stream != null ? this.GetStreamAdapter(stream) : throw new ArgumentNullException(nameof (stream))) ?? throw new SQLiteException("could not get or create adapter for output stream")).GetOutputDelegate(), IntPtr.Zero);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changegroup_output_strm");
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
        throw new ObjectDisposedException(typeof (SQLiteChangeGroup).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    private void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed)
          return;
        if (disposing && this.streamManager != null)
        {
          this.streamManager.Dispose();
          this.streamManager = (SQLiteSessionStreamManager) null;
        }
        if (!(this.changeGroup != IntPtr.Zero))
          return;
        UnsafeNativeMethods.sqlite3changegroup_delete(this.changeGroup);
        this.changeGroup = IntPtr.Zero;
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteChangeGroup() => this.Dispose(false);
  }
}
