// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteSession
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;
using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the change tracking session associated with a
  /// database.
  /// </summary>
  internal sealed class SQLiteSession : SQLiteConnectionLock, ISQLiteSession, IDisposable
  {
    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteSessionStreamManager" /> instance associated
    /// with this session.
    /// </summary>
    private SQLiteSessionStreamManager streamManager;
    /// <summary>
    /// The name of the database (e.g. "main") for this session.
    /// </summary>
    private string databaseName;
    /// <summary>
    /// The native handle for this session.  This will be deleted when
    /// this instance is disposed or finalized.
    /// </summary>
    private IntPtr session;
    /// <summary>
    /// The delegate used to provide table filtering to the native API.
    /// It will be null -OR- point to the <see cref="M:System.Data.SQLite.SQLiteSession.Filter(System.IntPtr,System.IntPtr)" /> method.
    /// </summary>
    private UnsafeNativeMethods.xSessionFilter xFilter;
    /// <summary>
    /// The managed callback used to filter tables for this session.  Set
    /// via the <see cref="M:System.Data.SQLite.SQLiteSession.SetTableFilter(System.Data.SQLite.SessionTableFilterCallback,System.Object)" /> method.
    /// </summary>
    private SessionTableFilterCallback tableFilterCallback;
    /// <summary>
    /// The optional application-defined context data that was passed to
    /// the <see cref="M:System.Data.SQLite.SQLiteSession.SetTableFilter(System.Data.SQLite.SessionTableFilterCallback,System.Object)" /> method.  This value may be null.
    /// </summary>
    private object tableFilterClientData;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified wrapped
    /// native connection handle and associated flags.
    /// </summary>
    /// <param name="handle">
    /// The wrapped native connection handle to be associated with this
    /// session.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the connection represented by the
    /// <paramref name="handle" /> value.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database (e.g. "main") for this session.
    /// </param>
    public SQLiteSession(
      SQLiteConnectionHandle handle,
      SQLiteConnectionFlags flags,
      string databaseName)
      : base(handle, flags, true)
    {
      this.databaseName = databaseName;
      this.InitializeHandle();
    }

    /// <summary>
    /// Throws an exception if the native session handle is invalid.
    /// </summary>
    private void CheckHandle()
    {
      if (this.session == IntPtr.Zero)
        throw new InvalidOperationException("session is not open");
    }

    /// <summary>
    /// Makes sure the native session handle is valid, creating it if
    /// necessary.
    /// </summary>
    private void InitializeHandle()
    {
      if (this.session != IntPtr.Zero)
        return;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_create(this.GetIntPtr(), SQLiteString.GetUtf8BytesFromString(this.databaseName), ref this.session);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3session_create");
    }

    /// <summary>
    /// This method sets up the internal table filtering associated state
    /// of this instance.
    /// </summary>
    /// <param name="callback">
    /// The table filter callback -OR- null to clear any existing table
    /// filter callback.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionFilter" /> native
    /// delegate -OR- null to clear any existing table filter.
    /// </returns>
    private UnsafeNativeMethods.xSessionFilter ApplyTableFilter(
      SessionTableFilterCallback callback,
      object clientData)
    {
      this.tableFilterCallback = callback;
      this.tableFilterClientData = clientData;
      if (callback == null)
      {
        if (this.xFilter != null)
          this.xFilter = (UnsafeNativeMethods.xSessionFilter) null;
        return (UnsafeNativeMethods.xSessionFilter) null;
      }
      if (this.xFilter == null)
        this.xFilter = new UnsafeNativeMethods.xSessionFilter(this.Filter);
      return this.xFilter;
    }

    /// <summary>
    /// Makes sure the <see cref="T:System.Data.SQLite.SQLiteSessionStreamManager" /> instance
    /// is available, creating it if necessary.
    /// </summary>
    private void InitializeStreamManager()
    {
      if (this.streamManager != null)
        return;
      this.streamManager = new SQLiteSessionStreamManager(this.GetFlags());
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
    /// This method is called when determining if a table needs to be
    /// included in the tracked changes for the associated database.
    /// </summary>
    /// <param name="context">
    /// Optional extra context information.  Currently, this will always
    /// have a value of <see cref="F:System.IntPtr.Zero" />.
    /// </param>
    /// <param name="pTblName">
    /// The native pointer to the name of the table.
    /// </param>
    /// <returns>
    /// Non-zero if changes to the specified table should be considered;
    /// otherwise, zero.
    /// </returns>
    private int Filter(IntPtr context, IntPtr pTblName)
    {
      try
      {
        return this.tableFilterCallback(this.tableFilterClientData, SQLiteString.StringFromUtf8IntPtr(pTblName)) ? 1 : 0;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "xSessionFilter", (object) ex));
        }
        catch
        {
        }
      }
      return 0;
    }

    /// <summary>
    /// Determines if this session is currently tracking changes to its
    /// associated database.
    /// </summary>
    /// <returns>
    /// Non-zero if changes to the associated database are being trakced;
    /// otherwise, zero.
    /// </returns>
    public bool IsEnabled()
    {
      this.CheckDisposed();
      this.CheckHandle();
      return UnsafeNativeMethods.sqlite3session_enable(this.session, -1) != 0;
    }

    /// <summary>
    /// Enables tracking of changes to the associated database.
    /// </summary>
    public void SetToEnabled()
    {
      this.CheckDisposed();
      this.CheckHandle();
      UnsafeNativeMethods.sqlite3session_enable(this.session, 1);
    }

    /// <summary>
    /// Disables tracking of changes to the associated database.
    /// </summary>
    public void SetToDisabled()
    {
      this.CheckDisposed();
      this.CheckHandle();
      UnsafeNativeMethods.sqlite3session_enable(this.session, 0);
    }

    /// <summary>
    /// Determines if this session is currently set to mark changes as
    /// indirect (i.e. as though they were made via a trigger or foreign
    /// key action).
    /// </summary>
    /// <returns>
    /// Non-zero if changes to the associated database are being marked as
    /// indirect; otherwise, zero.
    /// </returns>
    public bool IsIndirect()
    {
      this.CheckDisposed();
      this.CheckHandle();
      return UnsafeNativeMethods.sqlite3session_indirect(this.session, -1) != 0;
    }

    /// <summary>
    /// Sets the indirect flag for this session.  Subsequent changes will
    /// be marked as indirect until this flag is changed again.
    /// </summary>
    public void SetToIndirect()
    {
      this.CheckDisposed();
      this.CheckHandle();
      UnsafeNativeMethods.sqlite3session_indirect(this.session, 1);
    }

    /// <summary>
    /// Clears the indirect flag for this session.  Subsequent changes will
    /// be marked as direct until this flag is changed again.
    /// </summary>
    public void SetToDirect()
    {
      this.CheckDisposed();
      this.CheckHandle();
      UnsafeNativeMethods.sqlite3session_indirect(this.session, 0);
    }

    /// <summary>
    /// Determines if there are any tracked changes currently within the
    /// data for this session.
    /// </summary>
    /// <returns>
    /// Non-zero if there are no changes within the data for this session;
    /// otherwise, zero.
    /// </returns>
    public bool IsEmpty()
    {
      this.CheckDisposed();
      this.CheckHandle();
      return UnsafeNativeMethods.sqlite3session_isempty(this.session) != 0;
    }

    /// <summary>
    /// Upon success, causes changes to the specified table(s) to start
    /// being tracked.  Any tables impacted by calls to this method will
    /// not cause the <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> callback
    /// to be invoked.
    /// </summary>
    /// <param name="name">
    /// The name of the table to be tracked -OR- null to track all
    /// applicable tables within this database.
    /// </param>
    public void AttachTable(string name)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_attach(this.session, SQLiteString.GetUtf8BytesFromString(name));
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3session_attach");
    }

    /// <summary>
    /// This method is used to set the table filter for this instance.
    /// </summary>
    /// <param name="callback">
    /// The table filter callback -OR- null to clear any existing table
    /// filter callback.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    public void SetTableFilter(SessionTableFilterCallback callback, object clientData)
    {
      this.CheckDisposed();
      this.CheckHandle();
      UnsafeNativeMethods.sqlite3session_table_filter(this.session, this.ApplyTableFilter(callback, clientData), IntPtr.Zero);
    }

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// set of changes represented by this session instance.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this session instance.
    /// </param>
    public void CreateChangeSet(ref byte[] rawData)
    {
      this.CheckDisposed();
      this.CheckHandle();
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int nChangeSet = 0;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_changeset(this.session, ref nChangeSet, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3session_changeset");
        rawData = SQLiteBytes.FromIntPtr(zero1, nChangeSet);
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
    /// set of changes represented by this session instance.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this session
    /// instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    public void CreateChangeSet(Stream stream)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_changeset_strm(this.session, ((stream != null ? this.GetStreamAdapter(stream) : throw new ArgumentNullException(nameof (stream))) ?? throw new SQLiteException("could not get or create adapter for output stream")).GetOutputDelegate(), IntPtr.Zero);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3session_changeset_strm");
    }

    /// <summary>
    /// Attempts to create and return, via <paramref name="rawData" />, the
    /// set of changes represented by this session instance as a patch set.
    /// </summary>
    /// <param name="rawData">
    /// Upon success, this will contain the raw byte data for all the
    /// changes in this session instance.
    /// </param>
    public void CreatePatchSet(ref byte[] rawData)
    {
      this.CheckDisposed();
      this.CheckHandle();
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        int nPatchSet = 0;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_patchset(this.session, ref nPatchSet, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, "sqlite3session_patchset");
        rawData = SQLiteBytes.FromIntPtr(zero1, nPatchSet);
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
    /// set of changes represented by this session instance as a patch set.
    /// </summary>
    /// <param name="stream">
    /// Upon success, the raw byte data for all the changes in this session
    /// instance will be written to this <see cref="T:System.IO.Stream" />.
    /// </param>
    public void CreatePatchSet(Stream stream)
    {
      this.CheckDisposed();
      this.CheckHandle();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_patchset_strm(this.session, ((stream != null ? this.GetStreamAdapter(stream) : throw new ArgumentNullException(nameof (stream))) ?? throw new SQLiteException("could not get or create adapter for output stream")).GetOutputDelegate(), IntPtr.Zero);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3session_patchset_strm");
    }

    /// <summary>
    /// This method loads the differences between two tables [with the same
    /// name, set of columns, and primary key definition] into this session
    /// instance.
    /// </summary>
    /// <param name="fromDatabaseName">
    /// The name of the database containing the table with the original
    /// data (i.e. it will need updating in order to be identical to the
    /// one within the database associated with this session instance).
    /// </param>
    /// <param name="tableName">The name of the table.</param>
    public void LoadDifferencesFromTable(string fromDatabaseName, string tableName)
    {
      this.CheckDisposed();
      this.CheckHandle();
      if (fromDatabaseName == null)
        throw new ArgumentNullException(nameof (fromDatabaseName));
      if (tableName == null)
        throw new ArgumentNullException(nameof (tableName));
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3session_diff(this.session, SQLiteString.GetUtf8BytesFromString(fromDatabaseName), SQLiteString.GetUtf8BytesFromString(tableName), ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
        {
          string str = (string) null;
          str = zero1 != IntPtr.Zero ? SQLiteString.StringFromUtf8IntPtr(zero1) : throw new SQLiteException(errorCode, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0}{1}", (object) "sqlite3session_diff", (object) str));
          if (!string.IsNullOrEmpty(str))
            str = HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, ": {0}", (object) str);
        }
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
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteSession).Name);
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
        if (this.disposed)
          return;
        if (disposing)
        {
          if (this.xFilter != null)
            this.xFilter = (UnsafeNativeMethods.xSessionFilter) null;
          if (this.streamManager != null)
          {
            this.streamManager.Dispose();
            this.streamManager = (SQLiteSessionStreamManager) null;
          }
        }
        if (this.session != IntPtr.Zero)
        {
          UnsafeNativeMethods.sqlite3session_delete(this.session);
          this.session = IntPtr.Zero;
        }
        this.Unlock();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
