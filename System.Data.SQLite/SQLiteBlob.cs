// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBlob
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>Represents a single SQL blob in SQLite.</summary>
  public sealed class SQLiteBlob : IDisposable
  {
    /// <summary>The underlying SQLite object this blob is bound to.</summary>
    internal SQLiteBase _sql;
    /// <summary>The actual blob handle.</summary>
    internal SQLiteBlobHandle _sqlite_blob;
    private bool disposed;

    /// <summary>Initializes the blob.</summary>
    /// <param name="sqlbase">The base SQLite object.</param>
    /// <param name="blob">The blob handle.</param>
    private SQLiteBlob(SQLiteBase sqlbase, SQLiteBlobHandle blob)
    {
      this._sql = sqlbase;
      this._sqlite_blob = blob;
    }

    /// <summary>
    /// Creates a <see cref="T:System.Data.SQLite.SQLiteBlob" /> object.  This will not work
    /// for tables that were created WITHOUT ROWID -OR- if the query
    /// does not include the "rowid" column or one of its aliases -OR-
    /// if the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> was not created with the
    /// <see cref="F:System.Data.CommandBehavior.KeyInfo" /> flag.
    /// </summary>
    /// <param name="dataReader">
    /// The <see cref="T:System.Data.SQLite.SQLiteDataReader" /> instance with a result set
    /// containing the desired blob column.
    /// </param>
    /// <param name="i">The index of the blob column.</param>
    /// <param name="readOnly">
    /// Non-zero to open the blob object for read-only access.
    /// </param>
    /// <returns>
    /// The newly created <see cref="T:System.Data.SQLite.SQLiteBlob" /> instance -OR- null
    /// if an error occurs.
    /// </returns>
    public static SQLiteBlob Create(SQLiteDataReader dataReader, int i, bool readOnly) => SQLiteBlob.Create(SQLiteDataReader.GetConnection(dataReader), dataReader.GetDatabaseName(i), dataReader.GetTableName(i), dataReader.GetName(i), ((dataReader != null ? dataReader.GetRowId(i) : throw new ArgumentNullException(nameof (dataReader))) ?? throw new InvalidOperationException("No RowId is available")).Value, readOnly);

    /// <summary>
    /// Creates a <see cref="T:System.Data.SQLite.SQLiteBlob" /> object.  This will not work
    /// for tables that were created WITHOUT ROWID.
    /// </summary>
    /// <param name="connection">
    /// The connection to use when opening the blob object.
    /// </param>
    /// <param name="databaseName">
    /// The name of the database containing the blob object.
    /// </param>
    /// <param name="tableName">
    /// The name of the table containing the blob object.
    /// </param>
    /// <param name="columnName">
    /// The name of the column containing the blob object.
    /// </param>
    /// <param name="rowId">
    /// The integer identifier for the row associated with the desired
    /// blob object.
    /// </param>
    /// <param name="readOnly">
    /// Non-zero to open the blob object for read-only access.
    /// </param>
    /// <returns>
    /// The newly created <see cref="T:System.Data.SQLite.SQLiteBlob" /> instance -OR- null
    /// if an error occurs.
    /// </returns>
    public static SQLiteBlob Create(
      SQLiteConnection connection,
      string databaseName,
      string tableName,
      string columnName,
      long rowId,
      bool readOnly)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (!(connection._sql is SQLite3 sql1))
        throw new InvalidOperationException("Connection has no wrapper");
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new InvalidOperationException("Connection has an invalid handle.");
      SQLiteBlobHandle blob = (SQLiteBlobHandle) null;
      try
      {
      }
      finally
      {
        IntPtr zero = IntPtr.Zero;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_open((IntPtr) sql2, SQLiteConvert.ToUTF8(databaseName), SQLiteConvert.ToUTF8(tableName), SQLiteConvert.ToUTF8(columnName), rowId, readOnly ? 0 : 1, ref zero);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, (string) null);
        blob = new SQLiteBlobHandle(sql2, zero);
      }
      SQLiteConnection.OnChanged(connection, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) blob, (string) null, (object) new object[6]
      {
        (object) typeof (SQLiteBlob),
        (object) databaseName,
        (object) tableName,
        (object) columnName,
        (object) rowId,
        (object) readOnly
      }));
      return new SQLiteBlob((SQLiteBase) sql1, blob);
    }

    /// <summary>
    /// Throws an exception if the blob object does not appear to be open.
    /// </summary>
    private void CheckOpen()
    {
      if ((IntPtr) this._sqlite_blob == IntPtr.Zero)
        throw new InvalidOperationException("Blob is not open");
    }

    /// <summary>
    /// Throws an exception if an invalid read/write parameter is detected.
    /// </summary>
    /// <param name="buffer">
    /// When reading, this array will be populated with the bytes read from
    /// the underlying database blob.  When writing, this array contains new
    /// values for the specified portion of the underlying database blob.
    /// </param>
    /// <param name="count">The number of bytes to read or write.</param>
    /// <param name="offset">
    /// The byte offset, relative to the start of the underlying database
    /// blob, where the read or write operation will begin.
    /// </param>
    private void VerifyParameters(byte[] buffer, int count, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0)
        throw new ArgumentException("Negative offset not allowed.");
      if (count < 0)
        throw new ArgumentException("Negative count not allowed.");
      if (count > buffer.Length)
        throw new ArgumentException("Buffer is too small.");
    }

    /// <summary>
    /// Retargets this object to an underlying database blob for a
    /// different row; the database, table, and column remain exactly
    /// the same.  If this operation fails for any reason, this blob
    /// object is automatically disposed.
    /// </summary>
    /// <param name="rowId">The integer identifier for the new row.</param>
    public void Reopen(long rowId)
    {
      this.CheckDisposed();
      this.CheckOpen();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_reopen((IntPtr) this._sqlite_blob, rowId);
      if (errorCode != SQLiteErrorCode.Ok)
      {
        this.Dispose();
        throw new SQLiteException(errorCode, (string) null);
      }
    }

    /// <summary>
    /// Queries the total number of bytes for the underlying database blob.
    /// </summary>
    /// <returns>
    /// The total number of bytes for the underlying database blob.
    /// </returns>
    public int GetCount()
    {
      this.CheckDisposed();
      this.CheckOpen();
      return UnsafeNativeMethods.sqlite3_blob_bytes((IntPtr) this._sqlite_blob);
    }

    /// <summary>Reads data from the underlying database blob.</summary>
    /// <param name="buffer">
    /// This array will be populated with the bytes read from the
    /// underlying database blob.
    /// </param>
    /// <param name="count">The number of bytes to read.</param>
    /// <param name="offset">
    /// The byte offset, relative to the start of the underlying
    /// database blob, where the read operation will begin.
    /// </param>
    public void Read(byte[] buffer, int count, int offset)
    {
      this.CheckDisposed();
      this.CheckOpen();
      this.VerifyParameters(buffer, count, offset);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_read((IntPtr) this._sqlite_blob, buffer, count, offset);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
    }

    /// <summary>Writes data into the underlying database blob.</summary>
    /// <param name="buffer">
    /// This array contains the new values for the specified portion of
    /// the underlying database blob.
    /// </param>
    /// <param name="count">The number of bytes to write.</param>
    /// <param name="offset">
    /// The byte offset, relative to the start of the underlying
    /// database blob, where the write operation will begin.
    /// </param>
    public void Write(byte[] buffer, int count, int offset)
    {
      this.CheckDisposed();
      this.CheckOpen();
      this.VerifyParameters(buffer, count, offset);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_write((IntPtr) this._sqlite_blob, buffer, count, offset);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
    }

    /// <summary>Closes the blob, freeing the associated resources.</summary>
    public void Close() => this.Dispose();

    /// <summary>Disposes and finalizes the blob.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteBlob).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_blob != null)
        {
          this._sqlite_blob.Dispose();
          this._sqlite_blob = (SQLiteBlobHandle) null;
        }
        this._sql = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    /// <summary>The destructor.</summary>
    ~SQLiteBlob() => this.Dispose(false);
  }
}
