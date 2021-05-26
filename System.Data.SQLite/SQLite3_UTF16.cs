// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLite3_UTF16
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// Alternate SQLite3 object, overriding many text behaviors to support UTF-16 (Unicode)
  /// </summary>
  internal sealed class SQLite3_UTF16 : SQLite3
  {
    private bool disposed;

    /// <summary>
    /// Constructs the object used to interact with the SQLite core library
    /// using the UTF-8 text encoding.
    /// </summary>
    /// <param name="fmt">
    /// The DateTime format to be used when converting string values to a
    /// DateTime and binding DateTime parameters.
    /// </param>
    /// <param name="kind">
    /// The <see cref="T:System.DateTimeKind" /> to be used when creating DateTime
    /// values.
    /// </param>
    /// <param name="fmtString">
    /// The format string to be used when parsing and formatting DateTime
    /// values.
    /// </param>
    /// <param name="db">
    /// The native handle to be associated with the database connection.
    /// </param>
    /// <param name="fileName">
    /// The fully qualified file name associated with <paramref name="db" />.
    /// </param>
    /// <param name="ownHandle">
    /// Non-zero if the newly created object instance will need to dispose
    /// of <paramref name="db" /> when it is no longer needed.
    /// </param>
    internal SQLite3_UTF16(
      SQLiteDateFormats fmt,
      DateTimeKind kind,
      string fmtString,
      IntPtr db,
      string fileName,
      bool ownHandle)
      : base(fmt, kind, fmtString, db, fileName, ownHandle)
    {
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLite3_UTF16).Name);
    }

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

    /// <summary>
    /// Overrides SQLiteConvert.ToString() to marshal UTF-16 strings instead of UTF-8
    /// </summary>
    /// <param name="b">A pointer to a UTF-16 string</param>
    /// <param name="nbytelen">The length (IN BYTES) of the string</param>
    /// <returns>A .NET string</returns>
    public override string ToString(IntPtr b, int nbytelen)
    {
      this.CheckDisposed();
      return SQLite3_UTF16.UTF16ToString(b, nbytelen);
    }

    public static string UTF16ToString(IntPtr b, int nbytelen)
    {
      if (nbytelen == 0 || b == IntPtr.Zero)
        return string.Empty;
      return nbytelen == -1 ? Marshal.PtrToStringUni(b) : Marshal.PtrToStringUni(b, nbytelen / 2);
    }

    internal override void Open(
      string strFilename,
      string vfsName,
      SQLiteConnectionFlags connectionFlags,
      SQLiteOpenFlagsEnum openFlags,
      int maxPoolSize,
      bool usePool)
    {
      if (this._sql != null)
        this.Close(false);
      if (this._sql != null)
        throw new SQLiteException("connection handle is still active");
      this._usePool = usePool;
      this._fileName = strFilename;
      this._flags = connectionFlags;
      if (usePool)
      {
        this._sql = SQLiteConnectionPool.Remove(strFilename, maxPoolSize, out this._poolVersion);
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[8]
        {
          (object) typeof (SQLite3_UTF16),
          (object) strFilename,
          (object) vfsName,
          (object) connectionFlags,
          (object) openFlags,
          (object) maxPoolSize,
          (object) usePool,
          (object) this._poolVersion
        }));
      }
      if (this._sql == null)
      {
        try
        {
        }
        finally
        {
          IntPtr zero = IntPtr.Zero;
          int extFuncs = HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoExtensionFunctions) ? 0 : 1;
          SQLiteErrorCode errorCode;
          if (vfsName != null || extFuncs != 0)
          {
            errorCode = UnsafeNativeMethods.sqlite3_open16_interop(SQLiteConvert.ToUTF8(strFilename), SQLiteConvert.ToUTF8(vfsName), openFlags, extFuncs, ref zero);
          }
          else
          {
            if ((openFlags & SQLiteOpenFlagsEnum.Create) != SQLiteOpenFlagsEnum.Create && !File.Exists(strFilename))
              throw new SQLiteException(SQLiteErrorCode.CantOpen, strFilename);
            if (vfsName != null)
              throw new SQLiteException(SQLiteErrorCode.CantOpen, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "cannot open using UTF-16 and VFS \"{0}\": need interop assembly", (object) vfsName));
            errorCode = UnsafeNativeMethods.sqlite3_open16(strFilename, ref zero);
          }
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, (string) null);
          this._sql = new SQLiteConnectionHandle(zero, true);
        }
        lock (this._sql)
          ;
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[7]
        {
          (object) typeof (SQLite3_UTF16),
          (object) strFilename,
          (object) vfsName,
          (object) connectionFlags,
          (object) openFlags,
          (object) maxPoolSize,
          (object) usePool
        }));
      }
      if (!HelperMethods.HasFlags(connectionFlags, SQLiteConnectionFlags.NoBindFunctions))
      {
        if (this._functions == null)
          this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
        foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> bindFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction>>) SQLiteFunction.BindFunctions((SQLiteBase) this, connectionFlags))
          this._functions[bindFunction.Key] = bindFunction.Value;
      }
      this.SetTimeout(0);
      GC.KeepAlive((object) this._sql);
    }

    internal override void Bind_DateTime(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      DateTime dt)
    {
      switch (this._datetimeFormat)
      {
        case SQLiteDateFormats.Ticks:
        case SQLiteDateFormats.JulianDay:
        case SQLiteDateFormats.UnixEpoch:
          base.Bind_DateTime(stmt, flags, index, dt);
          break;
        default:
          if (HelperMethods.LogBind(flags))
            SQLite3.LogBind(stmt?._sqlite_stmt, index, dt);
          this.Bind_Text(stmt, flags, index, this.ToString(dt));
          break;
      }
    }

    internal override void Bind_Text(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      string value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_text16((IntPtr) sqliteStmt, index, value, value.Length * 2, (IntPtr) -1);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override DateTime GetDateTime(SQLiteStatement stmt, int index)
    {
      if (this._datetimeFormat == SQLiteDateFormats.Ticks)
        return SQLiteConvert.TicksToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
      if (this._datetimeFormat == SQLiteDateFormats.JulianDay)
        return SQLiteConvert.ToDateTime(this.GetDouble(stmt, index), this._datetimeKind);
      return this._datetimeFormat == SQLiteDateFormats.UnixEpoch ? SQLiteConvert.UnixEpochToDateTime(this.GetInt64(stmt, index), this._datetimeKind) : this.ToDateTime(this.GetText(stmt, index));
    }

    internal override string ColumnName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      IntPtr b = UnsafeNativeMethods.sqlite3_column_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len);
      return !(b == IntPtr.Zero) ? SQLite3_UTF16.UTF16ToString(b, len) : throw new SQLiteException(SQLiteErrorCode.NoMem, this.GetLastError());
    }

    internal override string GetText(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_text16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnOriginalName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_origin_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnDatabaseName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_database_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnTableName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_table_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string GetParamValueText(IntPtr ptr)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_value_text16_interop(ptr, ref len), len);
    }

    internal override void ReturnError(IntPtr context, string value) => UnsafeNativeMethods.sqlite3_result_error16(context, value, value.Length * 2);

    internal override void ReturnText(IntPtr context, string value) => UnsafeNativeMethods.sqlite3_result_text16(context, value, value.Length * 2, (IntPtr) -1);
  }
}
