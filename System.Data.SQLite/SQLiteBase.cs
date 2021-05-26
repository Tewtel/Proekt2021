// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBase
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This internal class provides the foundation of SQLite support.  It defines all the abstract members needed to implement
  /// a SQLite data provider, and inherits from SQLiteConvert which allows for simple translations of string to and from SQLite.
  /// </summary>
  internal abstract class SQLiteBase : SQLiteConvert, IDisposable
  {
    /// <summary>
    /// The error code used for logging exceptions caught in user-provided
    /// code.
    /// </summary>
    internal const int COR_E_EXCEPTION = -2146233088;
    private bool disposed;
    private static string[] _errorMessages = new string[29]
    {
      "not an error",
      "SQL logic error",
      "internal logic error",
      "access permission denied",
      "query aborted",
      "database is locked",
      "database table is locked",
      "out of memory",
      "attempt to write a readonly database",
      "interrupted",
      "disk I/O error",
      "database disk image is malformed",
      "unknown operation",
      "database or disk is full",
      "unable to open database file",
      "locking protocol",
      "table contains no data",
      "database schema has changed",
      "string or blob too big",
      "constraint failed",
      "datatype mismatch",
      "bad parameter or other API misuse",
      "large file support is disabled",
      "authorization denied",
      "auxiliary database format error",
      "column index out of range",
      "file is not a database",
      "notification message",
      "warning message"
    };

    internal SQLiteBase(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString)
      : base(fmt, kind, fmtString)
    {
    }

    /// <summary>
    /// Returns a string representing the active version of SQLite
    /// </summary>
    internal abstract string Version { get; }

    /// <summary>
    /// Returns an integer representing the active version of SQLite
    /// </summary>
    internal abstract int VersionNumber { get; }

    /// <summary>
    /// Returns non-zero if this connection to the database is read-only.
    /// </summary>
    internal abstract bool IsReadOnly(string name);

    /// <summary>
    /// Returns the rowid of the most recent successful INSERT into the database from this connection.
    /// </summary>
    internal abstract long LastInsertRowId { get; }

    /// <summary>
    /// Returns the number of changes the last executing insert/update caused.
    /// </summary>
    internal abstract int Changes { get; }

    /// <summary>
    /// Returns the amount of memory (in bytes) currently in use by the SQLite core library.  This is not really a per-connection
    /// value, it is global to the process.
    /// </summary>
    internal abstract long MemoryUsed { get; }

    /// <summary>
    /// Returns the maximum amount of memory (in bytes) used by the SQLite core library since the high-water mark was last reset.
    /// This is not really a per-connection value, it is global to the process.
    /// </summary>
    internal abstract long MemoryHighwater { get; }

    /// <summary>
    /// Returns non-zero if the underlying native connection handle is owned by this instance.
    /// </summary>
    internal abstract bool OwnHandle { get; }

    /// <summary>
    /// Returns the logical list of functions associated with this connection.
    /// </summary>
    internal abstract IDictionary<SQLiteFunctionAttribute, SQLiteFunction> Functions { get; }

    /// <summary>
    /// Sets the status of the memory usage tracking subsystem in the SQLite core library.  By default, this is enabled.
    /// If this is disabled, memory usage tracking will not be performed.  This is not really a per-connection value, it is
    /// global to the process.
    /// </summary>
    /// <param name="value">Non-zero to enable memory usage tracking, zero otherwise.</param>
    /// <returns>A standard SQLite return code (i.e. zero for success and non-zero for failure).</returns>
    internal abstract SQLiteErrorCode SetMemoryStatus(bool value);

    /// <summary>
    /// Attempts to free as much heap memory as possible for the database connection.
    /// </summary>
    /// <returns>A standard SQLite return code (i.e. zero for success and non-zero for failure).</returns>
    internal abstract SQLiteErrorCode ReleaseMemory();

    /// <summary>
    /// Shutdown the SQLite engine so that it can be restarted with different config options.
    /// We depend on auto initialization to recover.
    /// </summary>
    internal abstract SQLiteErrorCode Shutdown();

    /// <summary>
    /// Determines if the associated native connection handle is open.
    /// </summary>
    /// <returns>Non-zero if a database connection is open.</returns>
    internal abstract bool IsOpen();

    /// <summary>
    /// Returns the fully qualified path and file name for the currently open
    /// database, if any.
    /// </summary>
    /// <param name="dbName">The name of the attached database to query.</param>
    /// <returns>
    /// The fully qualified path and file name for the currently open database,
    /// if any.
    /// </returns>
    internal abstract string GetFileName(string dbName);

    /// <summary>Opens a database.</summary>
    /// <remarks>
    /// Implementers should call SQLiteFunction.BindFunctions() and save the array after opening a connection
    /// to bind all attributed user-defined functions and collating sequences to the new connection.
    /// </remarks>
    /// <param name="strFilename">The filename of the database to open.  SQLite automatically creates it if it doesn't exist.</param>
    /// <param name="vfsName">The name of the VFS to use -OR- null to use the default VFS.</param>
    /// <param name="connectionFlags">The flags associated with the parent connection object</param>
    /// <param name="openFlags">The open flags to use when creating the connection</param>
    /// <param name="maxPoolSize">The maximum size of the pool for the given filename</param>
    /// <param name="usePool">If true, the connection can be pulled from the connection pool</param>
    internal abstract void Open(
      string strFilename,
      string vfsName,
      SQLiteConnectionFlags connectionFlags,
      SQLiteOpenFlagsEnum openFlags,
      int maxPoolSize,
      bool usePool);

    /// <summary>Closes the currently-open database.</summary>
    /// <remarks>
    /// After the database has been closed implemeters should call SQLiteFunction.UnbindFunctions() to deallocate all interop allocated
    /// memory associated with the user-defined functions and collating sequences tied to the closed connection.
    /// </remarks>
    /// <param name="disposing">Non-zero if connection is being disposed, zero otherwise.</param>
    internal abstract void Close(bool disposing);

    /// <summary>
    /// Sets the busy timeout on the connection.  SQLiteCommand will call this before executing any command.
    /// </summary>
    /// <param name="nTimeoutMS">The number of milliseconds to wait before returning SQLITE_BUSY</param>
    internal abstract void SetTimeout(int nTimeoutMS);

    /// <summary>Returns the text of the last error issued by SQLite</summary>
    /// <returns></returns>
    internal abstract string GetLastError();

    /// <summary>
    /// Returns the text of the last error issued by SQLite -OR- the specified default error text if
    /// none is available from the SQLite core library.
    /// </summary>
    /// <param name="defValue">
    /// The error text to return in the event that one is not available from the SQLite core library.
    /// </param>
    /// <returns>The error text.</returns>
    internal abstract string GetLastError(string defValue);

    /// <summary>
    /// When pooling is enabled, force this connection to be disposed rather than returned to the pool
    /// </summary>
    internal abstract void ClearPool();

    /// <summary>
    /// When pooling is enabled, returns the number of pool entries matching the current file name.
    /// </summary>
    /// <returns>The number of pool entries matching the current file name.</returns>
    internal abstract int CountPool();

    /// <summary>Prepares a SQL statement for execution.</summary>
    /// <param name="cnn">The source connection preparing the command.  Can be null for any caller except LINQ</param>
    /// <param name="strSql">The SQL command text to prepare</param>
    /// <param name="previous">The previous statement in a multi-statement command, or null if no previous statement exists</param>
    /// <param name="timeoutMS">The timeout to wait before aborting the prepare</param>
    /// <param name="strRemain">The remainder of the statement that was not processed.  Each call to prepare parses the
    /// SQL up to to either the end of the text or to the first semi-colon delimiter.  The remaining text is returned
    /// here for a subsequent call to Prepare() until all the text has been processed.</param>
    /// <returns>Returns an initialized SQLiteStatement.</returns>
    internal abstract SQLiteStatement Prepare(
      SQLiteConnection cnn,
      string strSql,
      SQLiteStatement previous,
      uint timeoutMS,
      ref string strRemain);

    /// <summary>Steps through a prepared statement.</summary>
    /// <param name="stmt">The SQLiteStatement to step through</param>
    /// <returns>True if a row was returned, False if not.</returns>
    internal abstract bool Step(SQLiteStatement stmt);

    /// <summary>
    /// Returns non-zero if the specified statement is read-only in nature.
    /// </summary>
    /// <param name="stmt">The statement to check.</param>
    /// <returns>True if the outer query is read-only.</returns>
    internal abstract bool IsReadOnly(SQLiteStatement stmt);

    /// <summary>
    /// Resets a prepared statement so it can be executed again.  If the error returned is SQLITE_SCHEMA,
    /// transparently attempt to rebuild the SQL statement and throw an error if that was not possible.
    /// </summary>
    /// <param name="stmt">The statement to reset</param>
    /// <returns>Returns -1 if the schema changed while resetting, 0 if the reset was sucessful or 6 (SQLITE_LOCKED) if the reset failed due to a lock</returns>
    internal abstract SQLiteErrorCode Reset(SQLiteStatement stmt);

    /// <summary>
    /// Attempts to interrupt the query currently executing on the associated
    /// native database connection.
    /// </summary>
    internal abstract void Cancel();

    /// <summary>
    /// This function binds a user-defined function to the connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be bound.
    /// </param>
    /// <param name="function">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance that implements the
    /// function to be bound.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    internal abstract void BindFunction(
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags);

    /// <summary>
    /// This function unbinds a user-defined function from the connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be unbound.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>Non-zero if the function was unbound.</returns>
    internal abstract bool UnbindFunction(
      SQLiteFunctionAttribute functionAttribute,
      SQLiteConnectionFlags flags);

    internal abstract void Bind_Double(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      double value);

    internal abstract void Bind_Int32(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      int value);

    internal abstract void Bind_UInt32(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      uint value);

    internal abstract void Bind_Int64(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      long value);

    internal abstract void Bind_UInt64(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      ulong value);

    internal abstract void Bind_Boolean(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      bool value);

    internal abstract void Bind_Text(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      string value);

    internal abstract void Bind_Blob(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      byte[] blobData);

    internal abstract void Bind_DateTime(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      DateTime dt);

    internal abstract void Bind_Null(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index);

    internal abstract int Bind_ParamCount(SQLiteStatement stmt, SQLiteConnectionFlags flags);

    internal abstract string Bind_ParamName(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index);

    internal abstract int Bind_ParamIndex(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      string paramName);

    internal abstract int ColumnCount(SQLiteStatement stmt);

    internal abstract string ColumnName(SQLiteStatement stmt, int index);

    internal abstract TypeAffinity ColumnAffinity(SQLiteStatement stmt, int index);

    internal abstract string ColumnType(
      SQLiteStatement stmt,
      int index,
      ref TypeAffinity nAffinity);

    internal abstract int ColumnIndex(SQLiteStatement stmt, string columnName);

    internal abstract string ColumnOriginalName(SQLiteStatement stmt, int index);

    internal abstract string ColumnDatabaseName(SQLiteStatement stmt, int index);

    internal abstract string ColumnTableName(SQLiteStatement stmt, int index);

    internal abstract bool DoesTableExist(string dataBase, string table);

    internal abstract bool ColumnMetaData(
      string dataBase,
      string table,
      string column,
      bool canThrow,
      ref string dataType,
      ref string collateSequence,
      ref bool notNull,
      ref bool primaryKey,
      ref bool autoIncrement);

    internal abstract void GetIndexColumnExtendedInfo(
      string database,
      string index,
      string column,
      ref int sortMode,
      ref int onError,
      ref string collationSequence);

    internal abstract object GetObject(SQLiteStatement stmt, int index);

    internal abstract double GetDouble(SQLiteStatement stmt, int index);

    internal abstract bool GetBoolean(SQLiteStatement stmt, int index);

    internal abstract sbyte GetSByte(SQLiteStatement stmt, int index);

    internal abstract byte GetByte(SQLiteStatement stmt, int index);

    internal abstract short GetInt16(SQLiteStatement stmt, int index);

    internal abstract ushort GetUInt16(SQLiteStatement stmt, int index);

    internal abstract int GetInt32(SQLiteStatement stmt, int index);

    internal abstract uint GetUInt32(SQLiteStatement stmt, int index);

    internal abstract long GetInt64(SQLiteStatement stmt, int index);

    internal abstract ulong GetUInt64(SQLiteStatement stmt, int index);

    internal abstract string GetText(SQLiteStatement stmt, int index);

    internal abstract long GetBytes(
      SQLiteStatement stmt,
      int index,
      int nDataoffset,
      byte[] bDest,
      int nStart,
      int nLength);

    internal abstract char GetChar(SQLiteStatement stmt, int index);

    internal abstract long GetChars(
      SQLiteStatement stmt,
      int index,
      int nDataoffset,
      char[] bDest,
      int nStart,
      int nLength);

    internal abstract DateTime GetDateTime(SQLiteStatement stmt, int index);

    internal abstract bool IsNull(SQLiteStatement stmt, int index);

    internal abstract SQLiteErrorCode CreateCollation(
      string strCollation,
      SQLiteCollation func,
      SQLiteCollation func16,
      bool @throw);

    internal abstract SQLiteErrorCode CreateFunction(
      string strFunction,
      int nArgs,
      bool needCollSeq,
      SQLiteCallback func,
      SQLiteCallback funcstep,
      SQLiteFinalCallback funcfinal,
      bool @throw);

    internal abstract CollationSequence GetCollationSequence(
      SQLiteFunction func,
      IntPtr context);

    internal abstract int ContextCollateCompare(
      CollationEncodingEnum enc,
      IntPtr context,
      string s1,
      string s2);

    internal abstract int ContextCollateCompare(
      CollationEncodingEnum enc,
      IntPtr context,
      char[] c1,
      char[] c2);

    internal abstract int AggregateCount(IntPtr context);

    internal abstract IntPtr AggregateContext(IntPtr context);

    internal abstract long GetParamValueBytes(
      IntPtr ptr,
      int nDataOffset,
      byte[] bDest,
      int nStart,
      int nLength);

    internal abstract double GetParamValueDouble(IntPtr ptr);

    internal abstract int GetParamValueInt32(IntPtr ptr);

    internal abstract long GetParamValueInt64(IntPtr ptr);

    internal abstract string GetParamValueText(IntPtr ptr);

    internal abstract TypeAffinity GetParamValueType(IntPtr ptr);

    internal abstract void ReturnBlob(IntPtr context, byte[] value);

    internal abstract void ReturnDouble(IntPtr context, double value);

    internal abstract void ReturnError(IntPtr context, string value);

    internal abstract void ReturnInt32(IntPtr context, int value);

    internal abstract void ReturnInt64(IntPtr context, long value);

    internal abstract void ReturnNull(IntPtr context);

    internal abstract void ReturnText(IntPtr context, string value);

    /// <summary>
    /// Calls the native SQLite core library in order to create a disposable
    /// module containing the implementation of a virtual table.
    /// </summary>
    /// <param name="module">
    /// The module object to be used when creating the native disposable module.
    /// </param>
    /// <param name="flags">
    /// The flags for the associated <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance.
    /// </param>
    internal abstract void CreateModule(SQLiteModule module, SQLiteConnectionFlags flags);

    /// <summary>
    /// Calls the native SQLite core library in order to cleanup the resources
    /// associated with a module containing the implementation of a virtual table.
    /// </summary>
    /// <param name="module">
    /// The module object previously passed to the <see cref="M:System.Data.SQLite.SQLiteBase.CreateModule(System.Data.SQLite.SQLiteModule,System.Data.SQLite.SQLiteConnectionFlags)" />
    /// method.
    /// </param>
    /// <param name="flags">
    /// The flags for the associated <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance.
    /// </param>
    internal abstract void DisposeModule(SQLiteModule module, SQLiteConnectionFlags flags);

    /// <summary>
    /// Calls the native SQLite core library in order to declare a virtual table
    /// in response to a call into the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" />
    /// or <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> virtual table methods.
    /// </summary>
    /// <param name="module">
    /// The virtual table module that is to be responsible for the virtual table
    /// being declared.
    /// </param>
    /// <param name="strSql">
    /// The string containing the SQL statement describing the virtual table to
    /// be declared.
    /// </param>
    /// <param name="error">
    /// Upon success, the contents of this parameter are undefined.  Upon failure,
    /// it should contain an appropriate error message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    internal abstract SQLiteErrorCode DeclareVirtualTable(
      SQLiteModule module,
      string strSql,
      ref string error);

    /// <summary>
    /// Calls the native SQLite core library in order to declare a virtual table
    /// function in response to a call into the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" />
    /// or <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> virtual table methods.
    /// </summary>
    /// <param name="module">
    /// The virtual table module that is to be responsible for the virtual table
    /// function being declared.
    /// </param>
    /// <param name="argumentCount">
    /// The number of arguments to the function being declared.
    /// </param>
    /// <param name="name">The name of the function being declared.</param>
    /// <param name="error">
    /// Upon success, the contents of this parameter are undefined.  Upon failure,
    /// it should contain an appropriate error message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    internal abstract SQLiteErrorCode DeclareVirtualFunction(
      SQLiteModule module,
      int argumentCount,
      string name,
      ref string error);

    /// <summary>
    /// Returns the current and/or highwater values for the specified database status parameter.
    /// </summary>
    /// <param name="option">The database status parameter to query.</param>
    /// <param name="reset">
    /// Non-zero to reset the highwater value to the current value.
    /// </param>
    /// <param name="current">If applicable, receives the current value.</param>
    /// <param name="highwater">
    /// If applicable, receives the highwater value.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    internal abstract SQLiteErrorCode GetStatusParameter(
      SQLiteStatusOpsEnum option,
      bool reset,
      ref int current,
      ref int highwater);

    /// <summary>Change a limit value for the database.</summary>
    /// <param name="option">The database limit to change.</param>
    /// <param name="value">The new value for the specified limit.</param>
    /// <returns>
    /// The old value for the specified limit -OR- negative one if an error
    /// occurs.
    /// </returns>
    internal abstract int SetLimitOption(SQLiteLimitOpsEnum option, int value);

    /// <summary>Change a configuration option value for the database.</summary>
    /// <param name="option">
    /// The database configuration option to change.
    /// </param>
    /// <param name="value">
    /// The new value for the specified configuration option.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    internal abstract SQLiteErrorCode SetConfigurationOption(
      SQLiteConfigDbOpsEnum option,
      object value);

    /// <summary>Enables or disables extension loading by SQLite.</summary>
    /// <param name="bOnOff">
    /// True to enable loading of extensions, false to disable.
    /// </param>
    internal abstract void SetLoadExtension(bool bOnOff);

    /// <summary>Loads a SQLite extension library from the named file.</summary>
    /// <param name="fileName">
    /// The name of the dynamic link library file containing the extension.
    /// </param>
    /// <param name="procName">
    /// The name of the exported function used to initialize the extension.
    /// If null, the default "sqlite3_extension_init" will be used.
    /// </param>
    internal abstract void LoadExtension(string fileName, string procName);

    /// <summary>
    /// Enables or disables extened result codes returned by SQLite
    /// </summary>
    /// <param name="bOnOff">true to enable extended result codes, false to disable.</param>
    /// <returns></returns>
    internal abstract void SetExtendedResultCodes(bool bOnOff);

    /// <summary>
    /// Returns the numeric result code for the most recent failed SQLite API call
    /// associated with the database connection.
    /// </summary>
    /// <returns>Result code</returns>
    internal abstract SQLiteErrorCode ResultCode();

    /// <summary>
    /// Returns the extended numeric result code for the most recent failed SQLite API call
    /// associated with the database connection.
    /// </summary>
    /// <returns>Extended result code</returns>
    internal abstract SQLiteErrorCode ExtendedResultCode();

    /// <summary>
    /// Add a log message via the SQLite sqlite3_log interface.
    /// </summary>
    /// <param name="iErrCode">Error code to be logged with the message.</param>
    /// <param name="zMessage">String to be logged.  Unlike the SQLite sqlite3_log()
    /// interface, this should be pre-formatted.  Consider using the
    /// String.Format() function.</param>
    /// <returns></returns>
    internal abstract void LogMessage(SQLiteErrorCode iErrCode, string zMessage);

    internal abstract void SetPassword(byte[] passwordBytes);

    internal abstract void ChangePassword(byte[] newPasswordBytes);

    internal abstract void SetProgressHook(int nOps, SQLiteProgressCallback func);

    internal abstract void SetAuthorizerHook(SQLiteAuthorizerCallback func);

    internal abstract void SetUpdateHook(SQLiteUpdateCallback func);

    internal abstract void SetCommitHook(SQLiteCommitCallback func);

    internal abstract void SetTraceCallback(SQLiteTraceCallback func);

    internal abstract void SetTraceCallback2(SQLiteTraceFlags mask, SQLiteTraceCallback2 func);

    internal abstract void SetRollbackHook(SQLiteRollbackCallback func);

    internal abstract SQLiteErrorCode SetLogCallback(SQLiteLogCallback func);

    /// <summary>
    /// Checks if the SQLite core library has been initialized in the current process.
    /// </summary>
    /// <returns>
    /// Non-zero if the SQLite core library has been initialized in the current process,
    /// zero otherwise.
    /// </returns>
    internal abstract bool IsInitialized();

    internal abstract int GetCursorForTable(SQLiteStatement stmt, int database, int rootPage);

    internal abstract long GetRowIdForCursor(SQLiteStatement stmt, int cursor);

    internal abstract object GetValue(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      SQLiteType typ);

    /// <summary>
    /// Returns non-zero if the given database connection is in autocommit mode.
    /// Autocommit mode is on by default.  Autocommit mode is disabled by a BEGIN
    /// statement.  Autocommit mode is re-enabled by a COMMIT or ROLLBACK.
    /// </summary>
    internal abstract bool AutoCommit { get; }

    internal abstract SQLiteErrorCode FileControl(
      string zDbName,
      int op,
      IntPtr pArg);

    /// <summary>
    /// Creates a new SQLite backup object based on the provided destination
    /// database connection.  The source database connection is the one
    /// associated with this object.  The source and destination database
    /// connections cannot be the same.
    /// </summary>
    /// <param name="destCnn">The destination database connection.</param>
    /// <param name="destName">The destination database name.</param>
    /// <param name="sourceName">The source database name.</param>
    /// <returns>The newly created backup object.</returns>
    internal abstract SQLiteBackup InitializeBackup(
      SQLiteConnection destCnn,
      string destName,
      string sourceName);

    /// <summary>
    /// Copies up to N pages from the source database to the destination
    /// database associated with the specified backup object.
    /// </summary>
    /// <param name="backup">The backup object to use.</param>
    /// <param name="nPage">
    /// The number of pages to copy or negative to copy all remaining pages.
    /// </param>
    /// <param name="retry">
    /// Set to true if the operation needs to be retried due to database
    /// locking issues.
    /// </param>
    /// <returns>
    /// True if there are more pages to be copied, false otherwise.
    /// </returns>
    internal abstract bool StepBackup(SQLiteBackup backup, int nPage, ref bool retry);

    /// <summary>
    /// Returns the number of pages remaining to be copied from the source
    /// database to the destination database associated with the specified
    /// backup object.
    /// </summary>
    /// <param name="backup">The backup object to check.</param>
    /// <returns>The number of pages remaining to be copied.</returns>
    internal abstract int RemainingBackup(SQLiteBackup backup);

    /// <summary>
    /// Returns the total number of pages in the source database associated
    /// with the specified backup object.
    /// </summary>
    /// <param name="backup">The backup object to check.</param>
    /// <returns>The total number of pages in the source database.</returns>
    internal abstract int PageCountBackup(SQLiteBackup backup);

    /// <summary>
    /// Destroys the backup object, rolling back any backup that may be in
    /// progess.
    /// </summary>
    /// <param name="backup">The backup object to destroy.</param>
    internal abstract void FinishBackup(SQLiteBackup backup);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteBase).Name);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    ~SQLiteBase() => this.Dispose(false);

    /// <summary>
    /// Returns the error message for the specified SQLite return code using
    /// the internal static lookup table.
    /// </summary>
    /// <param name="rc">The SQLite return code.</param>
    /// <returns>The error message or null if it cannot be found.</returns>
    protected static string FallbackGetErrorString(SQLiteErrorCode rc)
    {
      switch (rc)
      {
        case SQLiteErrorCode.Row:
          return "another row available";
        case SQLiteErrorCode.Done:
          return "no more rows available";
        case SQLiteErrorCode.Abort_Rollback:
          return "abort due to ROLLBACK";
        default:
          if (SQLiteBase._errorMessages == null)
            return (string) null;
          int index = (int) (rc & SQLiteErrorCode.NonExtendedMask);
          if (index < 0 || index >= SQLiteBase._errorMessages.Length)
            index = 1;
          return SQLiteBase._errorMessages[index];
      }
    }

    internal static string GetLastError(SQLiteConnectionHandle hdl, IntPtr db)
    {
      if (hdl == null || db == IntPtr.Zero)
        return "null connection or database handle";
      string str = (string) null;
      try
      {
      }
      finally
      {
        lock (hdl)
        {
          if (!hdl.IsInvalid && !hdl.IsClosed)
          {
            int len = 0;
            str = SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_errmsg_interop(db, ref len), len);
          }
          else
            str = "closed or invalid connection handle";
        }
      }
      GC.KeepAlive((object) hdl);
      return str;
    }

    internal static void FinishBackup(SQLiteConnectionHandle hdl, IntPtr backup)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static void CloseBlob(SQLiteConnectionHandle hdl, IntPtr blob)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static void FinalizeStatement(SQLiteConnectionHandle hdl, IntPtr stmt)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static void CloseConnection(SQLiteConnectionHandle hdl, IntPtr db)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static void CloseConnectionV2(SQLiteConnectionHandle hdl, IntPtr db)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static bool ResetConnection(SQLiteConnectionHandle hdl, IntPtr db, bool canThrow)
    {
      if (hdl == null || db == IntPtr.Zero)
        return false;
      bool flag = false;
      try
      {
      }
      finally
      {
        lock (hdl)
        {
          if (canThrow && hdl.IsInvalid)
            throw new InvalidOperationException("The connection handle is invalid.");
          if (canThrow && hdl.IsClosed)
            throw new InvalidOperationException("The connection handle is closed.");
          if (!hdl.IsInvalid)
          {
            if (!hdl.IsClosed)
            {
              IntPtr errMsg = IntPtr.Zero;
              do
              {
                errMsg = UnsafeNativeMethods.sqlite3_next_stmt(db, errMsg);
                if (errMsg != IntPtr.Zero)
                  UnsafeNativeMethods.sqlite3_reset_interop(errMsg);
              }
              while (errMsg != IntPtr.Zero);
              if (SQLiteBase.IsAutocommit(hdl, db))
              {
                flag = true;
              }
              else
              {
                SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_exec(db, SQLiteConvert.ToUTF8("ROLLBACK"), IntPtr.Zero, IntPtr.Zero, ref errMsg);
                if (errorCode == SQLiteErrorCode.Ok)
                  flag = true;
                else if (canThrow)
                  throw new SQLiteException(errorCode, SQLiteBase.GetLastError(hdl, db));
              }
            }
          }
        }
      }
      GC.KeepAlive((object) hdl);
      return flag;
    }

    internal static bool IsAutocommit(SQLiteConnectionHandle hdl, IntPtr db)
    {
      if (hdl == null || db == IntPtr.Zero)
        return false;
      bool flag = false;
      try
      {
      }
      finally
      {
        lock (hdl)
        {
          if (!hdl.IsInvalid)
          {
            if (!hdl.IsClosed)
              flag = UnsafeNativeMethods.sqlite3_get_autocommit(db) == 1;
          }
        }
      }
      GC.KeepAlive((object) hdl);
      return flag;
    }
  }
}
