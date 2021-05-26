// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLite3
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class implements SQLiteBase completely, and is the guts of the code that interop's SQLite with .NET
  /// </summary>
  internal class SQLite3 : SQLiteBase
  {
    internal const string PublicKey = "002400000480000094000000060200000024000052534131000400000100010005a288de5687c4e1b621ddff5d844727418956997f475eb829429e411aff3e93f97b70de698b972640925bdd44280df0a25a843266973704137cbb0e7441c1fe7cae4e2440ae91ab8cde3933febcb1ac48dd33b40e13c421d8215c18a4349a436dd499e3c385cc683015f886f6c10bd90115eb2bd61b67750839e3a19941dc9c";
    internal const string DesignerVersion = "1.0.112.0";
    private static object syncRoot = new object();
    /// <summary>
    /// This field is used to refer to memory allocated for the
    /// SQLITE_DBCONFIG_MAINDBNAME value used with the native
    /// "sqlite3_db_config" API.  If allocated, the associated
    /// memeory will be freed when the underlying connection is
    /// closed.
    /// </summary>
    private IntPtr dbName = IntPtr.Zero;
    /// <summary>
    /// The opaque pointer returned to us by the sqlite provider
    /// </summary>
    protected internal SQLiteConnectionHandle _sql;
    protected string _fileName;
    protected SQLiteConnectionFlags _flags;
    private bool _setLogCallback;
    protected bool _usePool;
    protected int _poolVersion;
    private int _cancelCount;
    private bool _buildingSchema;
    /// <summary>
    /// The user-defined functions registered on this connection
    /// </summary>
    protected Dictionary<SQLiteFunctionAttribute, SQLiteFunction> _functions;
    /// <summary>
    /// This is the name of the native library file that contains the
    /// "vtshim" extension [wrapper].
    /// </summary>
    protected string _shimExtensionFileName;
    /// <summary>
    /// This is the flag indicate whether the native library file that
    /// contains the "vtshim" extension must be dynamically loaded by
    /// this class prior to use.
    /// </summary>
    protected bool? _shimIsLoadNeeded = new bool?();
    /// <summary>
    /// This is the name of the native entry point for the "vtshim"
    /// extension [wrapper].
    /// </summary>
    protected string _shimExtensionProcName = "sqlite3_vtshim_init";
    /// <summary>The modules created using this connection.</summary>
    protected Dictionary<string, SQLiteModule> _modules;
    private bool disposed;
    /// <summary>
    /// Has the sqlite3_errstr() core library API been checked for yet?
    /// If so, is it present?
    /// </summary>
    private static bool? have_errstr = new bool?();
    /// <summary>
    /// Has the sqlite3_stmt_readonly() core library API been checked for yet?
    /// If so, is it present?
    /// </summary>
    private static bool? have_stmt_readonly = new bool?();
    /// <summary>
    /// This field is used to keep track of whether or not the
    /// "SQLite_ForceLogPrepare" environment variable has been queried.  If so,
    /// it will only be non-zero if the environment variable was present.
    /// </summary>
    private static bool? forceLogPrepare = new bool?();

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
    /// The fully qualified file name associated with <paramref name="db " />.
    /// </param>
    /// <param name="ownHandle">
    /// Non-zero if the newly created object instance will need to dispose
    /// of <paramref name="db" /> when it is no longer needed.
    /// </param>
    internal SQLite3(
      SQLiteDateFormats fmt,
      DateTimeKind kind,
      string fmtString,
      IntPtr db,
      string fileName,
      bool ownHandle)
      : base(fmt, kind, fmtString)
    {
      if (!(db != IntPtr.Zero))
        return;
      this._sql = new SQLiteConnectionHandle(db, ownHandle);
      this._fileName = fileName;
      SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, fileName, (object) new object[7]
      {
        (object) typeof (SQLite3),
        (object) fmt,
        (object) kind,
        (object) fmtString,
        (object) db,
        (object) fileName,
        (object) ownHandle
      }));
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLite3).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed)
          return;
        this.DisposeModules();
        this.Close(true);
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    /// <summary>
    /// This method attempts to dispose of all the <see cref="T:System.Data.SQLite.SQLiteModule" /> derived
    /// object instances currently associated with the native database connection.
    /// </summary>
    private void DisposeModules()
    {
      if (this._modules == null)
        return;
      foreach (KeyValuePair<string, SQLiteModule> module in this._modules)
        module.Value?.Dispose();
      this._modules.Clear();
    }

    internal override void Close(bool disposing)
    {
      if (this._sql == null)
        return;
      if (!this._sql.OwnHandle)
      {
        this._sql = (SQLiteConnectionHandle) null;
      }
      else
      {
        bool flag = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UnbindFunctionsOnClose);
        for (; this._usePool; this._usePool = false)
        {
          if (SQLiteBase.ResetConnection(this._sql, (IntPtr) this._sql, !disposing) && this.UnhookNativeCallbacks(true, !disposing))
          {
            if (flag)
              SQLiteFunction.UnbindAllFunctions((SQLiteBase) this, this._flags, false);
            this.DisposeModules();
            SQLiteConnectionPool.Add(this._fileName, this._sql, this._poolVersion);
            SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.ClosedToPool, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, this._fileName, (object) new object[4]
            {
              (object) typeof (SQLite3),
              (object) !disposing,
              (object) this._fileName,
              (object) this._poolVersion
            }));
            goto label_13;
          }
        }
        this.UnhookNativeCallbacks(disposing, !disposing);
        if (flag)
          SQLiteFunction.UnbindAllFunctions((SQLiteBase) this, this._flags, false);
        this._sql.Dispose();
        this.FreeDbName(!disposing);
label_13:
        this._sql = (SQLiteConnectionHandle) null;
      }
    }

    /// <summary>
    /// Returns the number of times the <see cref="M:System.Data.SQLite.SQLite3.Cancel" /> method has been
    /// called.
    /// </summary>
    private int GetCancelCount() => Interlocked.CompareExchange(ref this._cancelCount, 0, 0);

    /// <summary>
    /// This method determines whether or not a <see cref="T:System.Data.SQLite.SQLiteException" />
    /// with a return code of <see cref="F:System.Data.SQLite.SQLiteErrorCode.Interrupt" /> should
    /// be thrown after making a call into the SQLite core library.
    /// </summary>
    /// <returns>
    /// Non-zero if a <see cref="T:System.Data.SQLite.SQLiteException" /> to be thrown.  This method
    /// will only return non-zero if the <see cref="M:System.Data.SQLite.SQLite3.Cancel" /> method was called
    /// one or more times during a call into the SQLite core library (e.g. when
    /// the sqlite3_prepare*() or sqlite3_step() APIs are used).
    /// </returns>
    private bool ShouldThrowForCancel() => this.GetCancelCount() > 0;

    /// <summary>
    /// Resets the value of the <see cref="F:System.Data.SQLite.SQLite3._cancelCount" /> field.
    /// </summary>
    private int ResetCancelCount() => Interlocked.CompareExchange(ref this._cancelCount, 0, this._cancelCount);

    /// <summary>
    /// Attempts to interrupt the query currently executing on the associated
    /// native database connection.
    /// </summary>
    internal override void Cancel()
    {
      try
      {
      }
      finally
      {
        Interlocked.Increment(ref this._cancelCount);
        UnsafeNativeMethods.sqlite3_interrupt((IntPtr) this._sql);
      }
    }

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
    internal override void BindFunction(
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags)
    {
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      SQLiteFunction.BindFunction((SQLiteBase) this, functionAttribute, function, flags);
      if (this._functions == null)
        this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
      this._functions[functionAttribute] = function;
    }

    /// <summary>
    /// This function binds a user-defined function to the connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be unbound.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>Non-zero if the function was unbound and removed.</returns>
    internal override bool UnbindFunction(
      SQLiteFunctionAttribute functionAttribute,
      SQLiteConnectionFlags flags)
    {
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      SQLiteFunction function;
      return this._functions != null && this._functions.TryGetValue(functionAttribute, out function) && (SQLiteFunction.UnbindFunction((SQLiteBase) this, functionAttribute, function, flags) && this._functions.Remove(functionAttribute));
    }

    internal override string Version => SQLite3.SQLiteVersion;

    internal override int VersionNumber => SQLite3.SQLiteVersionNumber;

    internal static string DefineConstants
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        IList<string> optionList = SQLiteDefineConstants.OptionList;
        if (optionList != null)
        {
          foreach (string str in (IEnumerable<string>) optionList)
          {
            if (str != null)
            {
              if (stringBuilder.Length > 0)
                stringBuilder.Append(' ');
              stringBuilder.Append(str);
            }
          }
        }
        return stringBuilder.ToString();
      }
    }

    internal static string SQLiteVersion => SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_libversion(), -1);

    internal static int SQLiteVersionNumber => UnsafeNativeMethods.sqlite3_libversion_number();

    internal static string SQLiteSourceId => SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_sourceid(), -1);

    internal static string SQLiteCompileOptions
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        int N = 0;
        int num = N + 1;
        for (IntPtr nativestring = UnsafeNativeMethods.sqlite3_compileoption_get(N); nativestring != IntPtr.Zero; nativestring = UnsafeNativeMethods.sqlite3_compileoption_get(num++))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(' ');
          stringBuilder.Append(SQLiteConvert.UTF8ToString(nativestring, -1));
        }
        return stringBuilder.ToString();
      }
    }

    internal static string InteropVersion => SQLiteConvert.UTF8ToString(UnsafeNativeMethods.interop_libversion(), -1);

    internal static string InteropSourceId => SQLiteConvert.UTF8ToString(UnsafeNativeMethods.interop_sourceid(), -1);

    internal static string InteropCompileOptions
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        int N = 0;
        int num = N + 1;
        for (IntPtr nativestring = UnsafeNativeMethods.interop_compileoption_get(N); nativestring != IntPtr.Zero; nativestring = UnsafeNativeMethods.interop_compileoption_get(num++))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(' ');
          stringBuilder.Append(SQLiteConvert.UTF8ToString(nativestring, -1));
        }
        return stringBuilder.ToString();
      }
    }

    internal override bool AutoCommit => SQLiteBase.IsAutocommit(this._sql, (IntPtr) this._sql);

    internal override bool IsReadOnly(string name)
    {
      IntPtr num1 = IntPtr.Zero;
      try
      {
        if (name != null)
          num1 = SQLiteString.Utf8IntPtrFromString(name);
        int num2;
        switch (UnsafeNativeMethods.sqlite3_db_readonly((IntPtr) this._sql, num1))
        {
          case -1:
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "database \"{0}\" not found", (object) name));
          case 0:
            num2 = 0;
            break;
          default:
            num2 = 1;
            break;
        }
        return num2 != 0;
      }
      finally
      {
        if (num1 != IntPtr.Zero)
        {
          SQLiteMemory.Free(num1);
          IntPtr zero = IntPtr.Zero;
        }
      }
    }

    internal override long LastInsertRowId => UnsafeNativeMethods.sqlite3_last_insert_rowid((IntPtr) this._sql);

    internal override int Changes => UnsafeNativeMethods.sqlite3_changes_interop((IntPtr) this._sql);

    internal override long MemoryUsed => SQLite3.StaticMemoryUsed;

    internal static long StaticMemoryUsed => UnsafeNativeMethods.sqlite3_memory_used();

    internal override long MemoryHighwater => SQLite3.StaticMemoryHighwater;

    internal static long StaticMemoryHighwater => UnsafeNativeMethods.sqlite3_memory_highwater(0);

    /// <summary>
    /// Returns non-zero if the underlying native connection handle is owned
    /// by this instance.
    /// </summary>
    internal override bool OwnHandle => this._sql != null ? this._sql.OwnHandle : throw new SQLiteException("no connection handle available");

    /// <summary>
    /// Returns the logical list of functions associated with this connection.
    /// </summary>
    internal override IDictionary<SQLiteFunctionAttribute, SQLiteFunction> Functions => (IDictionary<SQLiteFunctionAttribute, SQLiteFunction>) this._functions;

    internal override SQLiteErrorCode SetMemoryStatus(bool value) => SQLite3.StaticSetMemoryStatus(value);

    internal static SQLiteErrorCode StaticSetMemoryStatus(bool value) => UnsafeNativeMethods.sqlite3_config_int(SQLiteConfigOpsEnum.SQLITE_CONFIG_MEMSTATUS, value ? 1 : 0);

    /// <summary>
    /// Attempts to free as much heap memory as possible for the database connection.
    /// </summary>
    /// <returns>A standard SQLite return code (i.e. zero for success and non-zero for failure).</returns>
    internal override SQLiteErrorCode ReleaseMemory() => UnsafeNativeMethods.sqlite3_db_release_memory((IntPtr) this._sql);

    /// <summary>
    /// Attempts to free N bytes of heap memory by deallocating non-essential memory
    /// allocations held by the database library. Memory used to cache database pages
    /// to improve performance is an example of non-essential memory.  This is a no-op
    /// returning zero if the SQLite core library was not compiled with the compile-time
    /// option SQLITE_ENABLE_MEMORY_MANAGEMENT.  Optionally, attempts to reset and/or
    /// compact the Win32 native heap, if applicable.
    /// </summary>
    /// <param name="nBytes">The requested number of bytes to free.</param>
    /// <param name="reset">Non-zero to attempt a heap reset.</param>
    /// <param name="compact">Non-zero to attempt heap compaction.</param>
    /// <param name="nFree">
    /// The number of bytes actually freed.  This value may be zero.
    /// </param>
    /// <param name="resetOk">
    /// This value will be non-zero if the heap reset was successful.
    /// </param>
    /// <param name="nLargest">
    /// The size of the largest committed free block in the heap, in bytes.
    /// This value will be zero unless heap compaction is enabled.
    /// </param>
    /// <returns>
    /// A standard SQLite return code (i.e. zero for success and non-zero
    /// for failure).
    /// </returns>
    internal static SQLiteErrorCode StaticReleaseMemory(
      int nBytes,
      bool reset,
      bool compact,
      ref int nFree,
      ref bool resetOk,
      ref uint nLargest)
    {
      SQLiteErrorCode sqLiteErrorCode = SQLiteErrorCode.Ok;
      int num = UnsafeNativeMethods.sqlite3_release_memory(nBytes);
      uint largest = 0;
      bool flag = false;
      if (HelperMethods.IsWindows())
      {
        if (sqLiteErrorCode == SQLiteErrorCode.Ok && reset)
        {
          sqLiteErrorCode = UnsafeNativeMethods.sqlite3_win32_reset_heap();
          if (sqLiteErrorCode == SQLiteErrorCode.Ok)
            flag = true;
        }
        if (sqLiteErrorCode == SQLiteErrorCode.Ok && compact)
          sqLiteErrorCode = UnsafeNativeMethods.sqlite3_win32_compact_heap(ref largest);
      }
      else if (reset || compact)
        sqLiteErrorCode = SQLiteErrorCode.NotFound;
      nFree = num;
      nLargest = largest;
      resetOk = flag;
      return sqLiteErrorCode;
    }

    /// <summary>
    /// Shutdown the SQLite engine so that it can be restarted with different
    /// configuration options.  We depend on auto initialization to recover.
    /// </summary>
    /// <returns>Returns a standard SQLite result code.</returns>
    internal override SQLiteErrorCode Shutdown() => SQLite3.StaticShutdown(false);

    /// <summary>
    /// Shutdown the SQLite engine so that it can be restarted with different
    /// configuration options.  We depend on auto initialization to recover.
    /// </summary>
    /// <param name="directories">
    /// Non-zero to reset the database and temporary directories to their
    /// default values, which should be null for both.  This parameter has no
    /// effect on non-Windows operating systems.
    /// </param>
    /// <returns>Returns a standard SQLite result code.</returns>
    internal static SQLiteErrorCode StaticShutdown(bool directories)
    {
      SQLiteErrorCode sqLiteErrorCode = SQLiteErrorCode.Ok;
      if (directories && HelperMethods.IsWindows())
      {
        if (sqLiteErrorCode == SQLiteErrorCode.Ok)
          sqLiteErrorCode = UnsafeNativeMethods.sqlite3_win32_set_directory(1U, (string) null);
        if (sqLiteErrorCode == SQLiteErrorCode.Ok)
          sqLiteErrorCode = UnsafeNativeMethods.sqlite3_win32_set_directory(2U, (string) null);
      }
      if (sqLiteErrorCode == SQLiteErrorCode.Ok)
        sqLiteErrorCode = UnsafeNativeMethods.sqlite3_shutdown();
      return sqLiteErrorCode;
    }

    /// <summary>
    /// Determines if the associated native connection handle is open.
    /// </summary>
    /// <returns>
    /// Non-zero if the associated native connection handle is open.
    /// </returns>
    internal override bool IsOpen() => this._sql != null && !this._sql.IsInvalid && !this._sql.IsClosed;

    /// <summary>
    /// Returns the fully qualified path and file name for the currently open
    /// database, if any.
    /// </summary>
    /// <param name="dbName">The name of the attached database to query.</param>
    /// <returns>
    /// The fully qualified path and file name for the currently open database,
    /// if any.
    /// </returns>
    internal override string GetFileName(string dbName) => this._sql == null ? (string) null : SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_db_filename_bytes((IntPtr) this._sql, SQLiteConvert.ToUTF8(dbName)), -1);

    /// <summary>
    /// This method attempts to determine if a database connection opened
    /// with the specified <see cref="T:System.Data.SQLite.SQLiteOpenFlagsEnum" /> should be
    /// allowed into the connection pool.
    /// </summary>
    /// <param name="openFlags">
    /// The <see cref="T:System.Data.SQLite.SQLiteOpenFlagsEnum" /> that were specified when the
    /// connection was opened.
    /// </param>
    /// <returns>
    /// Non-zero if the connection should (eventually) be allowed into the
    /// connection pool; otherwise, zero.
    /// </returns>
    private static bool IsAllowedToUsePool(SQLiteOpenFlagsEnum openFlags) => openFlags == SQLiteOpenFlagsEnum.Default;

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
      if (this._usePool && !SQLite3.IsAllowedToUsePool(openFlags))
        this._usePool = false;
      this._fileName = strFilename;
      this._flags = connectionFlags;
      if (usePool)
      {
        this._sql = SQLiteConnectionPool.Remove(strFilename, maxPoolSize, out this._poolVersion);
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[8]
        {
          (object) typeof (SQLite3),
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
          SQLiteErrorCode errorCode = extFuncs == 0 ? UnsafeNativeMethods.sqlite3_open_v2(SQLiteConvert.ToUTF8(strFilename), ref zero, openFlags, SQLiteConvert.ToUTF8(vfsName)) : UnsafeNativeMethods.sqlite3_open_interop(SQLiteConvert.ToUTF8(strFilename), SQLiteConvert.ToUTF8(vfsName), openFlags, extFuncs, ref zero);
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, (string) null);
          this._sql = new SQLiteConnectionHandle(zero, true);
        }
        lock (this._sql)
          ;
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[7]
        {
          (object) typeof (SQLite3),
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

    internal override void ClearPool() => SQLiteConnectionPool.ClearPool(this._fileName);

    internal override int CountPool()
    {
      Dictionary<string, int> counts = (Dictionary<string, int>) null;
      int openCount = 0;
      int closeCount = 0;
      int totalCount = 0;
      SQLiteConnectionPool.GetCounts(this._fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
      return totalCount;
    }

    internal override void SetTimeout(int nTimeoutMS)
    {
      IntPtr sql = (IntPtr) this._sql;
      SQLiteErrorCode errorCode = !(sql == IntPtr.Zero) ? UnsafeNativeMethods.sqlite3_busy_timeout(sql, nTimeoutMS) : throw new SQLiteException("no connection handle available");
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override bool Step(SQLiteStatement stmt)
    {
      Random random = (Random) null;
      uint tickCount = (uint) Environment.TickCount;
      uint num = (uint) (stmt._command._commandTimeout * 1000);
      this.ResetCancelCount();
      SQLiteErrorCode errorCode1;
      while (true)
      {
        do
        {
          SQLiteErrorCode errorCode2;
          try
          {
          }
          finally
          {
            errorCode2 = UnsafeNativeMethods.sqlite3_step((IntPtr) stmt._sqlite_stmt);
          }
          if (this.ShouldThrowForCancel())
          {
            errorCode2 = errorCode2 == SQLiteErrorCode.Ok || errorCode2 == SQLiteErrorCode.Row || errorCode2 == SQLiteErrorCode.Done ? SQLiteErrorCode.Interrupt : throw new SQLiteException(errorCode2, (string) null);
          }
          else
          {
            switch (errorCode2)
            {
              case SQLiteErrorCode.Ok:
                continue;
              case SQLiteErrorCode.Interrupt:
                return false;
              case SQLiteErrorCode.Row:
                return true;
              case SQLiteErrorCode.Done:
                return false;
              default:
                errorCode1 = this.Reset(stmt);
                switch (errorCode1)
                {
                  case SQLiteErrorCode.Ok:
                    throw new SQLiteException(errorCode2, this.GetLastError());
                  case SQLiteErrorCode.Busy:
                  case SQLiteErrorCode.Locked:
                    continue;
                  default:
                    continue;
                }
            }
          }
        }
        while (stmt._command == null);
        if (random == null)
          random = new Random();
        if ((uint) Environment.TickCount - tickCount <= num)
          Thread.Sleep(random.Next(1, 150));
        else
          break;
      }
      throw new SQLiteException(errorCode1, this.GetLastError());
    }

    /// <summary>
    /// Returns the error message for the specified SQLite return code using
    /// the sqlite3_errstr() function, falling back to the internal lookup
    /// table if necessary.
    /// 
    /// WARNING: Do not remove this method, it is used via reflection.
    /// </summary>
    /// <param name="rc">The SQLite return code.</param>
    /// <returns>The error message or null if it cannot be found.</returns>
    internal static string GetErrorString(SQLiteErrorCode rc)
    {
      try
      {
        if (!SQLite3.have_errstr.HasValue)
          SQLite3.have_errstr = new bool?(SQLite3.SQLiteVersionNumber >= 3007015);
        if (SQLite3.have_errstr.Value)
        {
          IntPtr ptr = UnsafeNativeMethods.sqlite3_errstr(rc);
          if (ptr != IntPtr.Zero)
            return Marshal.PtrToStringAnsi(ptr);
        }
      }
      catch (EntryPointNotFoundException ex)
      {
      }
      return SQLiteBase.FallbackGetErrorString(rc);
    }

    /// <summary>
    /// Returns non-zero if the specified statement is read-only in nature.
    /// </summary>
    /// <param name="stmt">The statement to check.</param>
    /// <returns>True if the outer query is read-only.</returns>
    internal override bool IsReadOnly(SQLiteStatement stmt)
    {
      try
      {
        if (!SQLite3.have_stmt_readonly.HasValue)
          SQLite3.have_stmt_readonly = new bool?(SQLite3.SQLiteVersionNumber >= 3007004);
        if (SQLite3.have_stmt_readonly.Value)
          return UnsafeNativeMethods.sqlite3_stmt_readonly((IntPtr) stmt._sqlite_stmt) != 0;
      }
      catch (EntryPointNotFoundException ex)
      {
      }
      return false;
    }

    internal override SQLiteErrorCode Reset(SQLiteStatement stmt)
    {
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_reset_interop((IntPtr) stmt._sqlite_stmt);
      switch (errorCode)
      {
        case SQLiteErrorCode.Ok:
          return errorCode;
        case SQLiteErrorCode.Busy:
        case SQLiteErrorCode.Locked:
          return errorCode;
        case SQLiteErrorCode.Schema:
          string strRemain = (string) null;
          using (SQLiteStatement sqLiteStatement = this.Prepare((SQLiteConnection) null, stmt._sqlStatement, (SQLiteStatement) null, (uint) (stmt._command._commandTimeout * 1000), ref strRemain))
          {
            stmt._sqlite_stmt.Dispose();
            if (sqLiteStatement != null)
            {
              stmt._sqlite_stmt = sqLiteStatement._sqlite_stmt;
              sqLiteStatement._sqlite_stmt = (SQLiteStatementHandle) null;
            }
            stmt.BindParameters();
          }
          return SQLiteErrorCode.Unknown;
        default:
          throw new SQLiteException(errorCode, this.GetLastError());
      }
    }

    internal override string GetLastError() => this.GetLastError((string) null);

    internal override string GetLastError(string defValue)
    {
      string str = SQLiteBase.GetLastError(this._sql, (IntPtr) this._sql);
      if (string.IsNullOrEmpty(str))
        str = defValue;
      return str;
    }

    /// <summary>
    /// Determines if all calls to prepare a SQL query will be logged,
    /// regardless of the flags for the associated connection.
    /// </summary>
    /// <returns>Non-zero to log all calls to prepare a SQL query.</returns>
    internal static bool ForceLogPrepare()
    {
      lock (SQLite3.syncRoot)
      {
        if (!SQLite3.forceLogPrepare.HasValue)
          SQLite3.forceLogPrepare = UnsafeNativeMethods.GetSettingValue("SQLite_ForceLogPrepare", (string) null) == null ? new bool?(false) : new bool?(true);
        return SQLite3.forceLogPrepare.Value;
      }
    }

    internal override SQLiteStatement Prepare(
      SQLiteConnection cnn,
      string strSql,
      SQLiteStatement previous,
      uint timeoutMS,
      ref string strRemain)
    {
      if (!string.IsNullOrEmpty(strSql))
        strSql = strSql.Trim();
      if (!string.IsNullOrEmpty(strSql))
      {
        string baseSchemaName = cnn?._baseSchemaName;
        if (!string.IsNullOrEmpty(baseSchemaName))
        {
          strSql = strSql.Replace(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "[{0}].", (object) baseSchemaName), string.Empty);
          strSql = strSql.Replace(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}.", (object) baseSchemaName), string.Empty);
        }
      }
      SQLiteConnectionFlags flags = cnn != null ? cnn.Flags : SQLiteConnectionFlags.Default;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogPrepare(flags))
      {
        switch (strSql)
        {
          case "":
          case null:
            SQLiteLog.LogMessage("Preparing {<nothing>}...");
            break;
          default:
            if (strSql.Trim().Length != 0)
            {
              SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Preparing {{{0}}}...", (object) strSql));
              break;
            }
            goto case "";
        }
      }
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      int nRemain = 0;
      SQLiteErrorCode errorCode = SQLiteErrorCode.Schema;
      int num1 = 0;
      int num2 = cnn != null ? cnn._prepareRetries : 3;
      byte[] utF8 = SQLiteConvert.ToUTF8(strSql);
      SQLiteStatement sqLiteStatement = (SQLiteStatement) null;
      Random random = (Random) null;
      uint tickCount = (uint) Environment.TickCount;
      this.ResetCancelCount();
      GCHandle gcHandle = GCHandle.Alloc((object) utF8, GCHandleType.Pinned);
      IntPtr pSql = gcHandle.AddrOfPinnedObject();
      SQLiteStatementHandle stmt = (SQLiteStatementHandle) null;
      try
      {
        while (errorCode == SQLiteErrorCode.Schema || errorCode == SQLiteErrorCode.Locked || errorCode == SQLiteErrorCode.Busy)
        {
          if (num1 < num2)
          {
            try
            {
            }
            finally
            {
              IntPtr zero3 = IntPtr.Zero;
              zero2 = IntPtr.Zero;
              nRemain = 0;
              errorCode = UnsafeNativeMethods.sqlite3_prepare_interop((IntPtr) this._sql, pSql, utF8.Length - 1, ref zero3, ref zero2, ref nRemain);
              if (errorCode == SQLiteErrorCode.Ok && zero3 != IntPtr.Zero)
              {
                stmt?.Dispose();
                stmt = new SQLiteStatementHandle(this._sql, zero3);
              }
            }
            if (stmt != null)
              SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) stmt, strSql, (object) new object[5]
              {
                (object) typeof (SQLite3),
                (object) cnn,
                (object) strSql,
                (object) previous,
                (object) timeoutMS
              }));
            if (this.ShouldThrowForCancel())
            {
              errorCode = errorCode == SQLiteErrorCode.Ok || errorCode == SQLiteErrorCode.Row || errorCode == SQLiteErrorCode.Done ? SQLiteErrorCode.Interrupt : throw new SQLiteException(errorCode, (string) null);
            }
            else
            {
              switch (errorCode)
              {
                case SQLiteErrorCode.Error:
                  if (string.Compare(this.GetLastError(), "near \"TYPES\": syntax error", StringComparison.OrdinalIgnoreCase) == 0)
                  {
                    int num3 = strSql.IndexOf(';');
                    if (num3 == -1)
                      num3 = strSql.Length - 1;
                    string typedefs = strSql.Substring(0, num3 + 1);
                    strSql = strSql.Substring(num3 + 1);
                    strRemain = string.Empty;
                    for (; sqLiteStatement == null && strSql.Length > 0; strSql = strRemain)
                      sqLiteStatement = this.Prepare(cnn, strSql, previous, timeoutMS, ref strRemain);
                    sqLiteStatement?.SetTypes(typedefs);
                    return sqLiteStatement;
                  }
                  if (!this._buildingSchema && string.Compare(this.GetLastError(), 0, "no such table: TEMP.SCHEMA", 0, 26, StringComparison.OrdinalIgnoreCase) == 0)
                  {
                    strRemain = string.Empty;
                    this._buildingSchema = true;
                    try
                    {
                      if (((IServiceProvider) SQLiteFactory.Instance).GetService(typeof (ISQLiteSchemaExtensions)) is ISQLiteSchemaExtensions service11)
                        service11.BuildTempSchema(cnn);
                      for (; sqLiteStatement == null && strSql.Length > 0; strSql = strRemain)
                        sqLiteStatement = this.Prepare(cnn, strSql, previous, timeoutMS, ref strRemain);
                      return sqLiteStatement;
                    }
                    finally
                    {
                      this._buildingSchema = false;
                    }
                  }
                  else
                    continue;
                case SQLiteErrorCode.Interrupt:
                  goto label_51;
                case SQLiteErrorCode.Schema:
                  ++num1;
                  continue;
                default:
                  if (errorCode == SQLiteErrorCode.Locked || errorCode == SQLiteErrorCode.Busy)
                  {
                    if (random == null)
                      random = new Random();
                    if ((uint) Environment.TickCount - tickCount > timeoutMS)
                      throw new SQLiteException(errorCode, this.GetLastError());
                    Thread.Sleep(random.Next(1, 150));
                    continue;
                  }
                  continue;
              }
            }
          }
          else
            break;
        }
label_51:
        if (this.ShouldThrowForCancel())
        {
          errorCode = errorCode == SQLiteErrorCode.Ok || errorCode == SQLiteErrorCode.Row || errorCode == SQLiteErrorCode.Done ? SQLiteErrorCode.Interrupt : throw new SQLiteException(errorCode, (string) null);
        }
        else
        {
          if (errorCode == SQLiteErrorCode.Interrupt)
            return (SQLiteStatement) null;
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, this.GetLastError());
          strRemain = SQLiteConvert.UTF8ToString(zero2, nRemain);
          if (stmt != null)
            sqLiteStatement = new SQLiteStatement((SQLiteBase) this, flags, stmt, strSql.Substring(0, strSql.Length - strRemain.Length), previous);
          return sqLiteStatement;
        }
      }
      finally
      {
        gcHandle.Free();
      }
    }

    protected static void LogBind(SQLiteStatementHandle handle, int index) => SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as NULL...", (object) (IntPtr) handle, (object) index));

    protected static void LogBind(SQLiteStatementHandle handle, int index, System.ValueType value) => SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", (object) (IntPtr) handle, (object) index, (object) value.GetType(), (object) value));

    private static string FormatDateTime(DateTime value)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK"));
      stringBuilder.Append(' ');
      stringBuilder.Append((object) value.Kind);
      stringBuilder.Append(' ');
      stringBuilder.Append(value.Ticks);
      return stringBuilder.ToString();
    }

    protected static void LogBind(SQLiteStatementHandle handle, int index, DateTime value) => SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", (object) (IntPtr) handle, (object) index, (object) typeof (DateTime), (object) SQLite3.FormatDateTime(value)));

    protected static void LogBind(SQLiteStatementHandle handle, int index, string value) => SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", (object) (IntPtr) handle, (object) index, (object) typeof (string), value != null ? (object) value : (object) "<null>"));

    private static string ToHexadecimalString(byte[] array)
    {
      if (array == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        stringBuilder.Append(array[index].ToString("x2"));
      return stringBuilder.ToString();
    }

    protected static void LogBind(SQLiteStatementHandle handle, int index, byte[] value) => SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} as type {2} with value {{{3}}}...", (object) (IntPtr) handle, (object) index, (object) typeof (byte[]), value != null ? (object) SQLite3.ToHexadecimalString(value) : (object) "<null>"));

    internal override void Bind_Double(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      double value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_double((IntPtr) sqliteStmt, index, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_Int32(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      int value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_int((IntPtr) sqliteStmt, index, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_UInt32(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      uint value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      SQLiteErrorCode errorCode;
      if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.BindUInt32AsInt64))
      {
        long num = (long) value;
        errorCode = UnsafeNativeMethods.sqlite3_bind_int64((IntPtr) sqliteStmt, index, num);
      }
      else
        errorCode = UnsafeNativeMethods.sqlite3_bind_uint((IntPtr) sqliteStmt, index, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_Int64(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      long value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_int64((IntPtr) sqliteStmt, index, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_UInt64(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      ulong value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_uint64((IntPtr) sqliteStmt, index, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_Boolean(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      bool value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, (System.ValueType) value);
      int num = value ? 1 : 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_int((IntPtr) sqliteStmt, index, num);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_Text(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      string value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, value);
      byte[] utF8 = SQLiteConvert.ToUTF8(value);
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, utF8);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_text((IntPtr) sqliteStmt, index, utF8, utF8.Length - 1, (IntPtr) -1);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_DateTime(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      DateTime dt)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, dt);
      if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.BindDateTimeWithKind) && this._datetimeKind != DateTimeKind.Unspecified && (dt.Kind != DateTimeKind.Unspecified && dt.Kind != this._datetimeKind))
      {
        if (this._datetimeKind == DateTimeKind.Utc)
          dt = dt.ToUniversalTime();
        else if (this._datetimeKind == DateTimeKind.Local)
          dt = dt.ToLocalTime();
      }
      switch (this._datetimeFormat)
      {
        case SQLiteDateFormats.Ticks:
          long ticks = dt.Ticks;
          if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
            SQLite3.LogBind(sqliteStmt, index, (System.ValueType) ticks);
          SQLiteErrorCode errorCode1 = UnsafeNativeMethods.sqlite3_bind_int64((IntPtr) sqliteStmt, index, ticks);
          if (errorCode1 == SQLiteErrorCode.Ok)
            break;
          throw new SQLiteException(errorCode1, this.GetLastError());
        case SQLiteDateFormats.JulianDay:
          double julianDay = SQLiteConvert.ToJulianDay(dt);
          if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
            SQLite3.LogBind(sqliteStmt, index, (System.ValueType) julianDay);
          SQLiteErrorCode errorCode2 = UnsafeNativeMethods.sqlite3_bind_double((IntPtr) sqliteStmt, index, julianDay);
          if (errorCode2 == SQLiteErrorCode.Ok)
            break;
          throw new SQLiteException(errorCode2, this.GetLastError());
        case SQLiteDateFormats.UnixEpoch:
          long int64 = Convert.ToInt64(dt.Subtract(SQLiteConvert.UnixEpoch).TotalSeconds);
          if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
            SQLite3.LogBind(sqliteStmt, index, (System.ValueType) int64);
          SQLiteErrorCode errorCode3 = UnsafeNativeMethods.sqlite3_bind_int64((IntPtr) sqliteStmt, index, int64);
          if (errorCode3 == SQLiteErrorCode.Ok)
            break;
          throw new SQLiteException(errorCode3, this.GetLastError());
        default:
          byte[] utF8 = this.ToUTF8(dt);
          if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
            SQLite3.LogBind(sqliteStmt, index, utF8);
          SQLiteErrorCode errorCode4 = UnsafeNativeMethods.sqlite3_bind_text((IntPtr) sqliteStmt, index, utF8, utF8.Length - 1, (IntPtr) -1);
          if (errorCode4 == SQLiteErrorCode.Ok)
            break;
          throw new SQLiteException(errorCode4, this.GetLastError());
      }
    }

    internal override void Bind_Blob(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      byte[] blobData)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index, blobData);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_blob((IntPtr) sqliteStmt, index, blobData, blobData.Length, (IntPtr) -1);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override void Bind_Null(SQLiteStatement stmt, SQLiteConnectionFlags flags, int index)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLite3.LogBind(sqliteStmt, index);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_null((IntPtr) sqliteStmt, index);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override int Bind_ParamCount(SQLiteStatement stmt, SQLiteConnectionFlags flags)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      int num = UnsafeNativeMethods.sqlite3_bind_parameter_count((IntPtr) sqliteStmt);
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Statement {0} paramter count is {1}.", (object) (IntPtr) sqliteStmt, (object) num));
      return num;
    }

    internal override string Bind_ParamName(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      int len = 0;
      string str = SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_bind_parameter_name_interop((IntPtr) sqliteStmt, index, ref len), len);
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Statement {0} paramter #{1} name is {{{2}}}.", (object) (IntPtr) sqliteStmt, (object) index, (object) str));
      return str;
    }

    internal override int Bind_ParamIndex(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      string paramName)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      int num = UnsafeNativeMethods.sqlite3_bind_parameter_index((IntPtr) sqliteStmt, SQLiteConvert.ToUTF8(paramName));
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogBind(flags))
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Statement {0} paramter index of name {{{1}}} is #{2}.", (object) (IntPtr) sqliteStmt, (object) paramName, (object) num));
      return num;
    }

    internal override int ColumnCount(SQLiteStatement stmt) => UnsafeNativeMethods.sqlite3_column_count((IntPtr) stmt._sqlite_stmt);

    internal override string ColumnName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      IntPtr nativestring = UnsafeNativeMethods.sqlite3_column_name_interop((IntPtr) stmt._sqlite_stmt, index, ref len);
      return !(nativestring == IntPtr.Zero) ? SQLiteConvert.UTF8ToString(nativestring, len) : throw new SQLiteException(SQLiteErrorCode.NoMem, this.GetLastError());
    }

    internal override TypeAffinity ColumnAffinity(SQLiteStatement stmt, int index) => UnsafeNativeMethods.sqlite3_column_type((IntPtr) stmt._sqlite_stmt, index);

    internal override string ColumnType(
      SQLiteStatement stmt,
      int index,
      ref TypeAffinity nAffinity)
    {
      int len = 0;
      IntPtr nativestring = UnsafeNativeMethods.sqlite3_column_decltype_interop((IntPtr) stmt._sqlite_stmt, index, ref len);
      nAffinity = this.ColumnAffinity(stmt, index);
      if (nativestring != IntPtr.Zero && (len > 0 || len == -1))
      {
        string str = SQLiteConvert.UTF8ToString(nativestring, len);
        if (!string.IsNullOrEmpty(str))
          return str;
      }
      string[] typeDefinitions = stmt.TypeDefinitions;
      return typeDefinitions != null && index < typeDefinitions.Length && typeDefinitions[index] != null ? typeDefinitions[index] : string.Empty;
    }

    internal override int ColumnIndex(SQLiteStatement stmt, string columnName)
    {
      int num = this.ColumnCount(stmt);
      for (int index = 0; index < num; ++index)
      {
        if (string.Compare(columnName, this.ColumnName(stmt, index), StringComparison.OrdinalIgnoreCase) == 0)
          return index;
      }
      return -1;
    }

    internal override string ColumnOriginalName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_origin_name_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnDatabaseName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_database_name_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnTableName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_table_name_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override bool DoesTableExist(string dataBase, string table)
    {
      string dataType = (string) null;
      string collateSequence = (string) null;
      bool notNull = false;
      bool primaryKey = false;
      bool autoIncrement = false;
      return this.ColumnMetaData(dataBase, table, (string) null, false, ref dataType, ref collateSequence, ref notNull, ref primaryKey, ref autoIncrement);
    }

    internal override bool ColumnMetaData(
      string dataBase,
      string table,
      string column,
      bool canThrow,
      ref string dataType,
      ref string collateSequence,
      ref bool notNull,
      ref bool primaryKey,
      ref bool autoIncrement)
    {
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      int notNull1 = 0;
      int primaryKey1 = 0;
      int autoInc = 0;
      int dtLen = 0;
      int csLen = 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_table_column_metadata_interop((IntPtr) this._sql, SQLiteConvert.ToUTF8(dataBase), SQLiteConvert.ToUTF8(table), SQLiteConvert.ToUTF8(column), ref zero1, ref zero2, ref notNull1, ref primaryKey1, ref autoInc, ref dtLen, ref csLen);
      if (canThrow && errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
      dataType = SQLiteConvert.UTF8ToString(zero1, dtLen);
      collateSequence = SQLiteConvert.UTF8ToString(zero2, csLen);
      notNull = notNull1 == 1;
      primaryKey = primaryKey1 == 1;
      autoIncrement = autoInc == 1;
      return errorCode == SQLiteErrorCode.Ok;
    }

    internal override object GetObject(SQLiteStatement stmt, int index)
    {
      switch (this.ColumnAffinity(stmt, index))
      {
        case TypeAffinity.Int64:
          return (object) this.GetInt64(stmt, index);
        case TypeAffinity.Double:
          return (object) this.GetDouble(stmt, index);
        case TypeAffinity.Text:
          return (object) this.GetText(stmt, index);
        case TypeAffinity.Blob:
          long bytes = this.GetBytes(stmt, index, 0, (byte[]) null, 0, 0);
          if (bytes > 0L && bytes <= (long) int.MaxValue)
          {
            byte[] bDest = new byte[(int) bytes];
            this.GetBytes(stmt, index, 0, bDest, 0, (int) bytes);
            return (object) bDest;
          }
          break;
        case TypeAffinity.Null:
          return (object) DBNull.Value;
      }
      throw new NotImplementedException();
    }

    internal override double GetDouble(SQLiteStatement stmt, int index) => UnsafeNativeMethods.sqlite3_column_double((IntPtr) stmt._sqlite_stmt, index);

    internal override bool GetBoolean(SQLiteStatement stmt, int index) => SQLiteConvert.ToBoolean(this.GetObject(stmt, index), (IFormatProvider) CultureInfo.InvariantCulture, false);

    internal override sbyte GetSByte(SQLiteStatement stmt, int index) => (sbyte) (this.GetInt32(stmt, index) & (int) byte.MaxValue);

    internal override byte GetByte(SQLiteStatement stmt, int index) => (byte) (this.GetInt32(stmt, index) & (int) byte.MaxValue);

    internal override short GetInt16(SQLiteStatement stmt, int index) => (short) (this.GetInt32(stmt, index) & (int) ushort.MaxValue);

    internal override ushort GetUInt16(SQLiteStatement stmt, int index) => (ushort) (this.GetInt32(stmt, index) & (int) ushort.MaxValue);

    internal override int GetInt32(SQLiteStatement stmt, int index) => UnsafeNativeMethods.sqlite3_column_int((IntPtr) stmt._sqlite_stmt, index);

    internal override uint GetUInt32(SQLiteStatement stmt, int index) => (uint) this.GetInt32(stmt, index);

    internal override long GetInt64(SQLiteStatement stmt, int index) => UnsafeNativeMethods.sqlite3_column_int64((IntPtr) stmt._sqlite_stmt, index);

    internal override ulong GetUInt64(SQLiteStatement stmt, int index) => (ulong) this.GetInt64(stmt, index);

    internal override string GetText(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_column_text_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override DateTime GetDateTime(SQLiteStatement stmt, int index)
    {
      if (this._datetimeFormat == SQLiteDateFormats.Ticks)
        return SQLiteConvert.TicksToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
      if (this._datetimeFormat == SQLiteDateFormats.JulianDay)
        return SQLiteConvert.ToDateTime(this.GetDouble(stmt, index), this._datetimeKind);
      if (this._datetimeFormat == SQLiteDateFormats.UnixEpoch)
        return SQLiteConvert.UnixEpochToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
      int len = 0;
      return this.ToDateTime(UnsafeNativeMethods.sqlite3_column_text_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override long GetBytes(
      SQLiteStatement stmt,
      int index,
      int nDataOffset,
      byte[] bDest,
      int nStart,
      int nLength)
    {
      int num = UnsafeNativeMethods.sqlite3_column_bytes((IntPtr) stmt._sqlite_stmt, index);
      if (bDest == null)
        return (long) num;
      int length = nLength;
      if (length + nStart > bDest.Length)
        length = bDest.Length - nStart;
      if (length + nDataOffset > num)
        length = num - nDataOffset;
      if (length > 0)
        Marshal.Copy((IntPtr) (UnsafeNativeMethods.sqlite3_column_blob((IntPtr) stmt._sqlite_stmt, index).ToInt64() + (long) nDataOffset), bDest, nStart, length);
      else
        length = 0;
      return (long) length;
    }

    internal override char GetChar(SQLiteStatement stmt, int index) => Convert.ToChar(this.GetUInt16(stmt, index));

    internal override long GetChars(
      SQLiteStatement stmt,
      int index,
      int nDataOffset,
      char[] bDest,
      int nStart,
      int nLength)
    {
      int count = nLength;
      string text = this.GetText(stmt, index);
      int length = text.Length;
      if (bDest == null)
        return (long) length;
      if (count + nStart > bDest.Length)
        count = bDest.Length - nStart;
      if (count + nDataOffset > length)
        count = length - nDataOffset;
      if (count > 0)
        text.CopyTo(nDataOffset, bDest, nStart, count);
      else
        count = 0;
      return (long) count;
    }

    internal override bool IsNull(SQLiteStatement stmt, int index) => this.ColumnAffinity(stmt, index) == TypeAffinity.Null;

    internal override int AggregateCount(IntPtr context) => UnsafeNativeMethods.sqlite3_aggregate_count(context);

    internal override SQLiteErrorCode CreateFunction(
      string strFunction,
      int nArgs,
      bool needCollSeq,
      SQLiteCallback func,
      SQLiteCallback funcstep,
      SQLiteFinalCallback funcfinal,
      bool canThrow)
    {
      SQLiteErrorCode functionInterop = UnsafeNativeMethods.sqlite3_create_function_interop((IntPtr) this._sql, SQLiteConvert.ToUTF8(strFunction), nArgs, 4, IntPtr.Zero, func, funcstep, funcfinal, needCollSeq ? 1 : 0);
      if (functionInterop == SQLiteErrorCode.Ok)
        functionInterop = UnsafeNativeMethods.sqlite3_create_function_interop((IntPtr) this._sql, SQLiteConvert.ToUTF8(strFunction), nArgs, 1, IntPtr.Zero, func, funcstep, funcfinal, needCollSeq ? 1 : 0);
      return !canThrow || functionInterop == SQLiteErrorCode.Ok ? functionInterop : throw new SQLiteException(functionInterop, this.GetLastError());
    }

    internal override SQLiteErrorCode CreateCollation(
      string strCollation,
      SQLiteCollation func,
      SQLiteCollation func16,
      bool canThrow)
    {
      SQLiteErrorCode collation = UnsafeNativeMethods.sqlite3_create_collation((IntPtr) this._sql, SQLiteConvert.ToUTF8(strCollation), 2, IntPtr.Zero, func16);
      if (collation == SQLiteErrorCode.Ok)
        collation = UnsafeNativeMethods.sqlite3_create_collation((IntPtr) this._sql, SQLiteConvert.ToUTF8(strCollation), 1, IntPtr.Zero, func);
      return !canThrow || collation == SQLiteErrorCode.Ok ? collation : throw new SQLiteException(collation, this.GetLastError());
    }

    internal override int ContextCollateCompare(
      CollationEncodingEnum enc,
      IntPtr context,
      string s1,
      string s2)
    {
      Encoding encoding = (Encoding) null;
      switch (enc)
      {
        case CollationEncodingEnum.UTF8:
          encoding = Encoding.UTF8;
          break;
        case CollationEncodingEnum.UTF16LE:
          encoding = Encoding.Unicode;
          break;
        case CollationEncodingEnum.UTF16BE:
          encoding = Encoding.BigEndianUnicode;
          break;
      }
      byte[] bytes1 = encoding.GetBytes(s1);
      byte[] bytes2 = encoding.GetBytes(s2);
      return UnsafeNativeMethods.sqlite3_context_collcompare_interop(context, bytes1, bytes1.Length, bytes2, bytes2.Length);
    }

    internal override int ContextCollateCompare(
      CollationEncodingEnum enc,
      IntPtr context,
      char[] c1,
      char[] c2)
    {
      Encoding encoding = (Encoding) null;
      switch (enc)
      {
        case CollationEncodingEnum.UTF8:
          encoding = Encoding.UTF8;
          break;
        case CollationEncodingEnum.UTF16LE:
          encoding = Encoding.Unicode;
          break;
        case CollationEncodingEnum.UTF16BE:
          encoding = Encoding.BigEndianUnicode;
          break;
      }
      byte[] bytes1 = encoding.GetBytes(c1);
      byte[] bytes2 = encoding.GetBytes(c2);
      return UnsafeNativeMethods.sqlite3_context_collcompare_interop(context, bytes1, bytes1.Length, bytes2, bytes2.Length);
    }

    internal override CollationSequence GetCollationSequence(
      SQLiteFunction func,
      IntPtr context)
    {
      CollationSequence collationSequence = new CollationSequence();
      int len = 0;
      int type = 0;
      int enc = 0;
      IntPtr nativestring = UnsafeNativeMethods.sqlite3_context_collseq_interop(context, ref type, ref enc, ref len);
      collationSequence.Name = SQLiteConvert.UTF8ToString(nativestring, len);
      collationSequence.Type = (CollationTypeEnum) type;
      collationSequence._func = func;
      collationSequence.Encoding = (CollationEncodingEnum) enc;
      return collationSequence;
    }

    internal override long GetParamValueBytes(
      IntPtr p,
      int nDataOffset,
      byte[] bDest,
      int nStart,
      int nLength)
    {
      int num = UnsafeNativeMethods.sqlite3_value_bytes(p);
      if (bDest == null)
        return (long) num;
      int length = nLength;
      if (length + nStart > bDest.Length)
        length = bDest.Length - nStart;
      if (length + nDataOffset > num)
        length = num - nDataOffset;
      if (length > 0)
        Marshal.Copy((IntPtr) (UnsafeNativeMethods.sqlite3_value_blob(p).ToInt64() + (long) nDataOffset), bDest, nStart, length);
      else
        length = 0;
      return (long) length;
    }

    internal override double GetParamValueDouble(IntPtr ptr) => UnsafeNativeMethods.sqlite3_value_double(ptr);

    internal override int GetParamValueInt32(IntPtr ptr) => UnsafeNativeMethods.sqlite3_value_int(ptr);

    internal override long GetParamValueInt64(IntPtr ptr) => UnsafeNativeMethods.sqlite3_value_int64(ptr);

    internal override string GetParamValueText(IntPtr ptr)
    {
      int len = 0;
      return SQLiteConvert.UTF8ToString(UnsafeNativeMethods.sqlite3_value_text_interop(ptr, ref len), len);
    }

    internal override TypeAffinity GetParamValueType(IntPtr ptr) => UnsafeNativeMethods.sqlite3_value_type(ptr);

    internal override void ReturnBlob(IntPtr context, byte[] value) => UnsafeNativeMethods.sqlite3_result_blob(context, value, value.Length, (IntPtr) -1);

    internal override void ReturnDouble(IntPtr context, double value) => UnsafeNativeMethods.sqlite3_result_double(context, value);

    internal override void ReturnError(IntPtr context, string value) => UnsafeNativeMethods.sqlite3_result_error(context, SQLiteConvert.ToUTF8(value), value.Length);

    internal override void ReturnInt32(IntPtr context, int value) => UnsafeNativeMethods.sqlite3_result_int(context, value);

    internal override void ReturnInt64(IntPtr context, long value) => UnsafeNativeMethods.sqlite3_result_int64(context, value);

    internal override void ReturnNull(IntPtr context) => UnsafeNativeMethods.sqlite3_result_null(context);

    internal override void ReturnText(IntPtr context, string value)
    {
      byte[] utF8 = SQLiteConvert.ToUTF8(value);
      UnsafeNativeMethods.sqlite3_result_text(context, SQLiteConvert.ToUTF8(value), utF8.Length - 1, (IntPtr) -1);
    }

    /// <summary>
    /// Determines the file name of the native library containing the native
    /// "vtshim" extension -AND- whether it should be dynamically loaded by
    /// this class.
    /// </summary>
    /// <param name="isLoadNeeded">
    /// This output parameter will be set to non-zero if the returned native
    /// library file name should be dynamically loaded prior to attempting
    /// the creation of native disposable extension modules.
    /// </param>
    /// <returns>
    /// The file name of the native library containing the native "vtshim"
    /// extension -OR- null if it cannot be determined.
    /// </returns>
    private string GetShimExtensionFileName(ref bool isLoadNeeded)
    {
      isLoadNeeded = !this._shimIsLoadNeeded.HasValue ? HelperMethods.IsWindows() : this._shimIsLoadNeeded.Value;
      return this._shimExtensionFileName ?? UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
    }

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
    internal override void CreateModule(SQLiteModule module, SQLiteConnectionFlags flags)
    {
      if (module == null)
        throw new ArgumentNullException(nameof (module));
      if (HelperMethods.NoLogModule(flags))
      {
        module.LogErrors = HelperMethods.LogModuleError(flags);
        module.LogExceptions = HelperMethods.LogModuleException(flags);
      }
      if (this._sql == null)
        throw new SQLiteException("connection has an invalid handle");
      bool isLoadNeeded = false;
      string extensionFileName = this.GetShimExtensionFileName(ref isLoadNeeded);
      if (isLoadNeeded)
      {
        if (extensionFileName == null)
          throw new SQLiteException("the file name for the \"vtshim\" extension is unknown");
        if (this._shimExtensionProcName == null)
          throw new SQLiteException("the entry point for the \"vtshim\" extension is unknown");
        this.SetLoadExtension(true);
        this.LoadExtension(extensionFileName, this._shimExtensionProcName);
      }
      if (!module.CreateDisposableModule((IntPtr) this._sql))
        throw new SQLiteException(this.GetLastError());
      if (this._modules == null)
        this._modules = new Dictionary<string, SQLiteModule>();
      this._modules.Add(module.Name, module);
      if (!this._usePool)
        return;
      this._usePool = false;
    }

    /// <summary>
    /// Calls the native SQLite core library in order to cleanup the resources
    /// associated with a module containing the implementation of a virtual table.
    /// </summary>
    /// <param name="module">
    /// The module object previously passed to the <see cref="M:System.Data.SQLite.SQLite3.CreateModule(System.Data.SQLite.SQLiteModule,System.Data.SQLite.SQLiteConnectionFlags)" />
    /// method.
    /// </param>
    /// <param name="flags">
    /// The flags for the associated <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance.
    /// </param>
    internal override void DisposeModule(SQLiteModule module, SQLiteConnectionFlags flags)
    {
      if (module == null)
        throw new ArgumentNullException(nameof (module));
      module.Dispose();
    }

    internal override IntPtr AggregateContext(IntPtr context) => UnsafeNativeMethods.sqlite3_aggregate_context(context, 1);

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
    internal override SQLiteErrorCode DeclareVirtualTable(
      SQLiteModule module,
      string strSql,
      ref string error)
    {
      if (this._sql == null)
      {
        error = "connection has an invalid handle";
        return SQLiteErrorCode.Error;
      }
      IntPtr num = IntPtr.Zero;
      try
      {
        num = SQLiteString.Utf8IntPtrFromString(strSql);
        SQLiteErrorCode sqLiteErrorCode = UnsafeNativeMethods.sqlite3_declare_vtab((IntPtr) this._sql, num);
        if (sqLiteErrorCode == SQLiteErrorCode.Ok && module != null)
          module.Declared = true;
        if (sqLiteErrorCode != SQLiteErrorCode.Ok)
          error = this.GetLastError();
        return sqLiteErrorCode;
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
    internal override SQLiteErrorCode DeclareVirtualFunction(
      SQLiteModule module,
      int argumentCount,
      string name,
      ref string error)
    {
      if (this._sql == null)
      {
        error = "connection has an invalid handle";
        return SQLiteErrorCode.Error;
      }
      IntPtr num = IntPtr.Zero;
      try
      {
        num = SQLiteString.Utf8IntPtrFromString(name);
        SQLiteErrorCode sqLiteErrorCode = UnsafeNativeMethods.sqlite3_overload_function((IntPtr) this._sql, num, argumentCount);
        if (sqLiteErrorCode != SQLiteErrorCode.Ok)
          error = this.GetLastError();
        return sqLiteErrorCode;
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
    /// Builds an error message string fragment containing the
    /// defined values of the <see cref="T:System.Data.SQLite.SQLiteStatusOpsEnum" />
    /// enumeration.
    /// </summary>
    /// <returns>The built string fragment.</returns>
    private static string GetStatusDbOpsNames()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string name in Enum.GetNames(typeof (SQLiteStatusOpsEnum)))
      {
        if (!string.IsNullOrEmpty(name))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(name);
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Builds an error message string fragment containing the
    /// defined values of the <see cref="T:System.Data.SQLite.SQLiteLimitOpsEnum" />
    /// enumeration.
    /// </summary>
    /// <returns>The built string fragment.</returns>
    private static string GetLimitOpsNames()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string name in Enum.GetNames(typeof (SQLiteLimitOpsEnum)))
      {
        if (!string.IsNullOrEmpty(name))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(name);
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Builds an error message string fragment containing the
    /// defined values of the <see cref="T:System.Data.SQLite.SQLiteConfigDbOpsEnum" />
    /// enumeration.
    /// </summary>
    /// <returns>The built string fragment.</returns>
    private static string GetConfigDbOpsNames()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string name in Enum.GetNames(typeof (SQLiteConfigDbOpsEnum)))
      {
        if (!string.IsNullOrEmpty(name))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(name);
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns the current and/or highwater values for the specified
    /// database status parameter.
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
    internal override SQLiteErrorCode GetStatusParameter(
      SQLiteStatusOpsEnum option,
      bool reset,
      ref int current,
      ref int highwater)
    {
      if (!Enum.IsDefined(typeof (SQLiteStatusOpsEnum), (object) option))
        throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "unrecognized status option, must be: {0}", (object) SQLite3.GetStatusDbOpsNames()));
      return UnsafeNativeMethods.sqlite3_db_status((IntPtr) this._sql, option, ref current, ref highwater, reset ? 1 : 0);
    }

    /// <summary>Change a limit value for the database.</summary>
    /// <param name="option">The database limit to change.</param>
    /// <param name="value">The new value for the specified limit.</param>
    /// <returns>
    /// The old value for the specified limit -OR- negative one if an error
    /// occurs.
    /// </returns>
    internal override int SetLimitOption(SQLiteLimitOpsEnum option, int value)
    {
      if (!Enum.IsDefined(typeof (SQLiteLimitOpsEnum), (object) option))
        throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "unrecognized limit option, must be: {0}", (object) SQLite3.GetLimitOpsNames()));
      return UnsafeNativeMethods.sqlite3_limit((IntPtr) this._sql, option, value);
    }

    /// <summary>Change a configuration option value for the database.</summary>
    /// <param name="option">
    /// The database configuration option to change.
    /// </param>
    /// <param name="value">
    /// The new value for the specified configuration option.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    internal override SQLiteErrorCode SetConfigurationOption(
      SQLiteConfigDbOpsEnum option,
      object value)
    {
      if (!Enum.IsDefined(typeof (SQLiteConfigDbOpsEnum), (object) option))
        throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "unrecognized configuration option, must be: {0}", (object) SQLite3.GetConfigDbOpsNames()));
      switch (option)
      {
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_NONE:
          return SQLiteErrorCode.Ok;
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_MAINDBNAME:
          if (value == null)
            throw new ArgumentNullException(nameof (value));
          if (!(value is string))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", (object) typeof (string)));
          SQLiteErrorCode sqLiteErrorCode = SQLiteErrorCode.Error;
          IntPtr num = IntPtr.Zero;
          try
          {
            num = SQLiteString.Utf8IntPtrFromString((string) value);
            if (num == IntPtr.Zero)
              throw new SQLiteException(SQLiteErrorCode.NoMem, "cannot allocate database name");
            sqLiteErrorCode = UnsafeNativeMethods.sqlite3_db_config_charptr((IntPtr) this._sql, option, num);
            if (sqLiteErrorCode == SQLiteErrorCode.Ok)
            {
              this.FreeDbName(true);
              this.dbName = num;
              num = IntPtr.Zero;
            }
          }
          finally
          {
            if (sqLiteErrorCode != SQLiteErrorCode.Ok && num != IntPtr.Zero)
            {
              SQLiteMemory.Free(num);
              IntPtr zero = IntPtr.Zero;
            }
          }
          return sqLiteErrorCode;
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_LOOKASIDE:
          if (!(value is object[] objArray2))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", (object) typeof (object[])));
          if (!(objArray2[0] is IntPtr))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration element zero (0) type mismatch, must be of type {0}", (object) typeof (IntPtr)));
          if (!(objArray2[1] is int))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration element one (1) type mismatch, must be of type {0}", (object) typeof (int)));
          if (!(objArray2[2] is int))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration element two (2) type mismatch, must be of type {0}", (object) typeof (int)));
          return UnsafeNativeMethods.sqlite3_db_config_intptr_two_ints((IntPtr) this._sql, option, (IntPtr) objArray2[0], (int) objArray2[1], (int) objArray2[2]);
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_FKEY:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_TRIGGER:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_FTS3_TOKENIZER:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_NO_CKPT_ON_CLOSE:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_QPSG:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_TRIGGER_EQP:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_RESET_DATABASE:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DEFENSIVE:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_WRITABLE_SCHEMA:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_LEGACY_ALTER_TABLE:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DQS_DML:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_DQS_DDL:
        case SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_VIEW:
          if (!(value is bool flag2))
            throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "configuration value type mismatch, must be of type {0}", (object) typeof (bool)));
          int result = 0;
          return UnsafeNativeMethods.sqlite3_db_config_int_refint((IntPtr) this._sql, option, flag2 ? 1 : 0, ref result);
        default:
          throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "unsupported configuration option {0}", (object) option));
      }
    }

    /// <summary>Enables or disables extension loading by SQLite.</summary>
    /// <param name="bOnOff">
    /// True to enable loading of extensions, false to disable.
    /// </param>
    internal override void SetLoadExtension(bool bOnOff)
    {
      SQLiteErrorCode errorCode = SQLite3.SQLiteVersionNumber < 3013000 ? UnsafeNativeMethods.sqlite3_enable_load_extension((IntPtr) this._sql, bOnOff ? -1 : 0) : this.SetConfigurationOption(SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION, (object) bOnOff);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    /// <summary>Loads a SQLite extension library from the named file.</summary>
    /// <param name="fileName">
    /// The name of the dynamic link library file containing the extension.
    /// </param>
    /// <param name="procName">
    /// The name of the exported function used to initialize the extension.
    /// If null, the default "sqlite3_extension_init" will be used.
    /// </param>
    internal override void LoadExtension(string fileName, string procName)
    {
      if (fileName == null)
        throw new ArgumentNullException(nameof (fileName));
      IntPtr zero1 = IntPtr.Zero;
      try
      {
        byte[] bytes = Encoding.UTF8.GetBytes(fileName + (object) char.MinValue);
        byte[] procName1 = (byte[]) null;
        if (procName != null)
          procName1 = Encoding.UTF8.GetBytes(procName + (object) char.MinValue);
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_load_extension((IntPtr) this._sql, bytes, procName1, ref zero1);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, SQLiteConvert.UTF8ToString(zero1, -1));
      }
      finally
      {
        if (zero1 != IntPtr.Zero)
        {
          UnsafeNativeMethods.sqlite3_free(zero1);
          IntPtr zero2 = IntPtr.Zero;
        }
      }
    }

    /// Enables or disables extended result codes returned by SQLite
    internal override void SetExtendedResultCodes(bool bOnOff)
    {
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_extended_result_codes((IntPtr) this._sql, bOnOff ? -1 : 0);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    /// Gets the last SQLite error code
    internal override SQLiteErrorCode ResultCode() => UnsafeNativeMethods.sqlite3_errcode((IntPtr) this._sql);

    /// Gets the last SQLite extended error code
    internal override SQLiteErrorCode ExtendedResultCode() => UnsafeNativeMethods.sqlite3_extended_errcode((IntPtr) this._sql);

    /// Add a log message via the SQLite sqlite3_log interface.
    internal override void LogMessage(SQLiteErrorCode iErrCode, string zMessage) => SQLite3.StaticLogMessage(iErrCode, zMessage);

    /// Add a log message via the SQLite sqlite3_log interface.
    internal static void StaticLogMessage(SQLiteErrorCode iErrCode, string zMessage) => UnsafeNativeMethods.sqlite3_log(iErrCode, SQLiteConvert.ToUTF8(zMessage));

    private static void ZeroPassword(byte[] passwordBytes)
    {
      if (passwordBytes == null)
        return;
      for (int index = 0; index < passwordBytes.Length; ++index)
      {
        byte num = (byte) ((index + 1) % (int) byte.MaxValue);
        passwordBytes[index] = num;
        passwordBytes[index] ^= num;
      }
    }

    internal override void SetPassword(byte[] passwordBytes)
    {
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_key((IntPtr) this._sql, passwordBytes, passwordBytes.Length);
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword))
        SQLite3.ZeroPassword(passwordBytes);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
      if (!this._usePool)
        return;
      this._usePool = false;
    }

    internal override void ChangePassword(byte[] newPasswordBytes)
    {
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_rekey((IntPtr) this._sql, newPasswordBytes, newPasswordBytes == null ? 0 : newPasswordBytes.Length);
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword))
        SQLite3.ZeroPassword(newPasswordBytes);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
      if (!this._usePool)
        return;
      this._usePool = false;
    }

    internal override void SetProgressHook(int nOps, SQLiteProgressCallback func) => UnsafeNativeMethods.sqlite3_progress_handler((IntPtr) this._sql, nOps, func, IntPtr.Zero);

    internal override void SetAuthorizerHook(SQLiteAuthorizerCallback func) => UnsafeNativeMethods.sqlite3_set_authorizer((IntPtr) this._sql, func, IntPtr.Zero);

    internal override void SetUpdateHook(SQLiteUpdateCallback func) => UnsafeNativeMethods.sqlite3_update_hook((IntPtr) this._sql, func, IntPtr.Zero);

    internal override void SetCommitHook(SQLiteCommitCallback func) => UnsafeNativeMethods.sqlite3_commit_hook((IntPtr) this._sql, func, IntPtr.Zero);

    internal override void SetTraceCallback(SQLiteTraceCallback func) => UnsafeNativeMethods.sqlite3_trace((IntPtr) this._sql, func, IntPtr.Zero);

    internal override void SetTraceCallback2(SQLiteTraceFlags mask, SQLiteTraceCallback2 func) => UnsafeNativeMethods.sqlite3_trace_v2((IntPtr) this._sql, mask, func, IntPtr.Zero);

    internal override void SetRollbackHook(SQLiteRollbackCallback func) => UnsafeNativeMethods.sqlite3_rollback_hook((IntPtr) this._sql, func, IntPtr.Zero);

    /// <summary>
    /// Allows the setting of a logging callback invoked by SQLite when a
    /// log event occurs.  Only one callback may be set.  If NULL is passed,
    /// the logging callback is unregistered.
    /// </summary>
    /// <param name="func">The callback function to invoke.</param>
    /// <returns>Returns a result code</returns>
    internal override SQLiteErrorCode SetLogCallback(SQLiteLogCallback func)
    {
      SQLiteErrorCode sqLiteErrorCode = UnsafeNativeMethods.sqlite3_config_log(SQLiteConfigOpsEnum.SQLITE_CONFIG_LOG, func, IntPtr.Zero);
      if (sqLiteErrorCode == SQLiteErrorCode.Ok)
        this._setLogCallback = func != null;
      return sqLiteErrorCode;
    }

    /// <summary>
    /// Appends an error message and an appropriate line-ending to a <see cref="T:System.Text.StringBuilder" />
    /// instance.  This is useful because the .NET Compact Framework has a slightly different set
    /// of supported methods for the <see cref="T:System.Text.StringBuilder" /> class.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="T:System.Text.StringBuilder" /> instance to append to.
    /// </param>
    /// <param name="message">
    /// The message to append.  It will be followed by an appropriate line-ending.
    /// </param>
    private static void AppendError(StringBuilder builder, string message) => builder?.AppendLine(message);

    /// <summary>
    /// This method attempts to cause the SQLite native library to invalidate
    /// its function pointers that refer to this instance.  This is necessary
    /// to prevent calls from native code into delegates that may have been
    /// garbage collected.  Normally, these types of issues can only arise for
    /// connections that are added to the pool; howver, it is good practice to
    /// unconditionally invalidate function pointers that may refer to objects
    /// being disposed.
    /// </summary>
    /// <param name="includeGlobal">
    /// Non-zero to also invalidate global function pointers (i.e. those that
    /// are not directly associated with this connection on the native side).
    /// </param>
    /// <param name="canThrow">
    /// Non-zero if this method is being executed within a context where it can
    /// throw an exception in the event of failure; otherwise, zero.
    /// </param>
    /// <returns>
    /// Non-zero if this method was successful; otherwise, zero.
    /// </returns>
    private bool UnhookNativeCallbacks(bool includeGlobal, bool canThrow)
    {
      bool flag = true;
      SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;
      StringBuilder builder = new StringBuilder();
      try
      {
        this.SetRollbackHook((SQLiteRollbackCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset rollback hook");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      try
      {
        if (UnsafeNativeMethods.sqlite3_libversion_number() >= 3014000)
          this.SetTraceCallback2(SQLiteTraceFlags.SQLITE_TRACE_NONE, (SQLiteTraceCallback2) null);
        else
          this.SetTraceCallback((SQLiteTraceCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset trace callback");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      try
      {
        this.SetCommitHook((SQLiteCommitCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset commit hook");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      try
      {
        this.SetUpdateHook((SQLiteUpdateCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset update hook");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      try
      {
        this.SetAuthorizerHook((SQLiteAuthorizerCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset authorizer hook");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      try
      {
        this.SetProgressHook(0, (SQLiteProgressCallback) null);
      }
      catch (Exception ex)
      {
        SQLite3.AppendError(builder, "failed to unset progress hook");
        errorCode = SQLiteErrorCode.Error;
        flag = false;
      }
      if (includeGlobal)
      {
        if (this._setLogCallback)
        {
          try
          {
            SQLiteErrorCode sqLiteErrorCode = this.SetLogCallback((SQLiteLogCallback) null);
            if (sqLiteErrorCode != SQLiteErrorCode.Ok)
            {
              SQLite3.AppendError(builder, "could not unset log callback");
              errorCode = sqLiteErrorCode;
              flag = false;
            }
          }
          catch (Exception ex)
          {
            SQLite3.AppendError(builder, "failed to unset log callback");
            errorCode = SQLiteErrorCode.Error;
            flag = false;
          }
        }
      }
      return flag || !canThrow ? flag : throw new SQLiteException(errorCode, builder.ToString());
    }

    /// <summary>
    /// This method attempts to free the cached database name used with the
    /// <see cref="M:System.Data.SQLite.SQLite3.SetConfigurationOption(System.Data.SQLite.SQLiteConfigDbOpsEnum,System.Object)" /> method.
    /// </summary>
    /// <param name="canThrow">
    /// Non-zero if this method is being executed within a context where it can
    /// throw an exception in the event of failure; otherwise, zero.
    /// </param>
    /// <returns>
    /// Non-zero if this method was successful; otherwise, zero.
    /// </returns>
    private bool FreeDbName(bool canThrow)
    {
      try
      {
        if (this.dbName != IntPtr.Zero)
        {
          SQLiteMemory.Free(this.dbName);
          this.dbName = IntPtr.Zero;
        }
        return true;
      }
      catch (Exception ex)
      {
        if (canThrow)
          throw;
      }
      return false;
    }

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
    internal override SQLiteBackup InitializeBackup(
      SQLiteConnection destCnn,
      string destName,
      string sourceName)
    {
      if (destCnn == null)
        throw new ArgumentNullException(nameof (destCnn));
      if (destName == null)
        throw new ArgumentNullException(nameof (destName));
      if (sourceName == null)
        throw new ArgumentNullException(nameof (sourceName));
      if (!(destCnn._sql is SQLite3 sql1))
        throw new ArgumentException("Destination connection has no wrapper.", nameof (destCnn));
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new ArgumentException("Destination connection has an invalid handle.", nameof (destCnn));
      SQLiteConnectionHandle sql3 = this._sql;
      if (sql3 == null)
        throw new InvalidOperationException("Source connection has an invalid handle.");
      byte[] utF8_1 = SQLiteConvert.ToUTF8(destName);
      byte[] utF8_2 = SQLiteConvert.ToUTF8(sourceName);
      SQLiteBackupHandle backup1 = (SQLiteBackupHandle) null;
      try
      {
      }
      finally
      {
        IntPtr backup2 = UnsafeNativeMethods.sqlite3_backup_init((IntPtr) sql2, utF8_1, (IntPtr) sql3, utF8_2);
        if (backup2 == IntPtr.Zero)
        {
          SQLiteErrorCode errorCode = this.ResultCode();
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, this.GetLastError());
          throw new SQLiteException("failed to initialize backup");
        }
        backup1 = new SQLiteBackupHandle(sql2, backup2);
      }
      SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) backup1, (string) null, (object) new object[4]
      {
        (object) typeof (SQLite3),
        (object) destCnn,
        (object) destName,
        (object) sourceName
      }));
      return new SQLiteBackup((SQLiteBase) this, backup1, (IntPtr) sql2, utF8_1, (IntPtr) sql3, utF8_2);
    }

    /// <summary>
    /// Copies up to N pages from the source database to the destination
    /// database associated with the specified backup object.
    /// </summary>
    /// <param name="backup">The backup object to use.</param>
    /// <param name="nPage">
    /// The number of pages to copy, negative to copy all remaining pages.
    /// </param>
    /// <param name="retry">
    /// Set to true if the operation needs to be retried due to database
    /// locking issues; otherwise, set to false.
    /// </param>
    /// <returns>
    /// True if there are more pages to be copied, false otherwise.
    /// </returns>
    internal override bool StepBackup(SQLiteBackup backup, int nPage, ref bool retry)
    {
      retry = false;
      IntPtr backup1 = (IntPtr) ((backup != null ? backup._sqlite_backup : throw new ArgumentNullException(nameof (backup))) ?? throw new InvalidOperationException("Backup object has an invalid handle."));
      SQLiteErrorCode errorCode = !(backup1 == IntPtr.Zero) ? UnsafeNativeMethods.sqlite3_backup_step(backup1, nPage) : throw new InvalidOperationException("Backup object has an invalid handle pointer.");
      backup._stepResult = errorCode;
      switch (errorCode)
      {
        case SQLiteErrorCode.Ok:
          return true;
        case SQLiteErrorCode.Busy:
          retry = true;
          return true;
        case SQLiteErrorCode.Locked:
          retry = true;
          return true;
        case SQLiteErrorCode.Done:
          return false;
        default:
          throw new SQLiteException(errorCode, this.GetLastError());
      }
    }

    /// <summary>
    /// Returns the number of pages remaining to be copied from the source
    /// database to the destination database associated with the specified
    /// backup object.
    /// </summary>
    /// <param name="backup">The backup object to check.</param>
    /// <returns>The number of pages remaining to be copied.</returns>
    internal override int RemainingBackup(SQLiteBackup backup)
    {
      IntPtr backup1 = (IntPtr) ((backup != null ? backup._sqlite_backup : throw new ArgumentNullException(nameof (backup))) ?? throw new InvalidOperationException("Backup object has an invalid handle."));
      return !(backup1 == IntPtr.Zero) ? UnsafeNativeMethods.sqlite3_backup_remaining(backup1) : throw new InvalidOperationException("Backup object has an invalid handle pointer.");
    }

    /// <summary>
    /// Returns the total number of pages in the source database associated
    /// with the specified backup object.
    /// </summary>
    /// <param name="backup">The backup object to check.</param>
    /// <returns>The total number of pages in the source database.</returns>
    internal override int PageCountBackup(SQLiteBackup backup)
    {
      IntPtr backup1 = (IntPtr) ((backup != null ? backup._sqlite_backup : throw new ArgumentNullException(nameof (backup))) ?? throw new InvalidOperationException("Backup object has an invalid handle."));
      return !(backup1 == IntPtr.Zero) ? UnsafeNativeMethods.sqlite3_backup_pagecount(backup1) : throw new InvalidOperationException("Backup object has an invalid handle pointer.");
    }

    /// <summary>
    /// Destroys the backup object, rolling back any backup that may be in
    /// progess.
    /// </summary>
    /// <param name="backup">The backup object to destroy.</param>
    internal override void FinishBackup(SQLiteBackup backup)
    {
      SQLiteBackupHandle liteBackupHandle = backup != null ? backup._sqlite_backup : throw new ArgumentNullException(nameof (backup));
      IntPtr backup1 = liteBackupHandle != null ? (IntPtr) liteBackupHandle : throw new InvalidOperationException("Backup object has an invalid handle.");
      SQLiteErrorCode errorCode = !(backup1 == IntPtr.Zero) ? UnsafeNativeMethods.sqlite3_backup_finish_interop(backup1) : throw new InvalidOperationException("Backup object has an invalid handle pointer.");
      liteBackupHandle.SetHandleAsInvalid();
      if (errorCode != SQLiteErrorCode.Ok && errorCode != backup._stepResult)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    /// <summary>
    /// Determines if the SQLite core library has been initialized for the
    /// current process.
    /// </summary>
    /// <returns>
    /// A boolean indicating whether or not the SQLite core library has been
    /// initialized for the current process.
    /// </returns>
    internal override bool IsInitialized() => SQLite3.StaticIsInitialized();

    /// <summary>
    /// Determines if the SQLite core library has been initialized for the
    /// current process.
    /// </summary>
    /// <returns>
    /// A boolean indicating whether or not the SQLite core library has been
    /// initialized for the current process.
    /// </returns>
    internal static bool StaticIsInitialized()
    {
      lock (SQLite3.syncRoot)
      {
        bool enabled = SQLiteLog.Enabled;
        SQLiteLog.Enabled = false;
        try
        {
          return UnsafeNativeMethods.sqlite3_config_none(SQLiteConfigOpsEnum.SQLITE_CONFIG_NONE) == SQLiteErrorCode.Misuse;
        }
        finally
        {
          SQLiteLog.Enabled = enabled;
        }
      }
    }

    /// <summary>
    /// Helper function to retrieve a column of data from an active statement.
    /// </summary>
    /// <param name="stmt">The statement being step()'d through</param>
    /// <param name="flags">The flags associated with the connection.</param>
    /// <param name="index">The column index to retrieve</param>
    /// <param name="typ">The type of data contained in the column.  If Uninitialized, this function will retrieve the datatype information.</param>
    /// <returns>Returns the data in the column</returns>
    internal override object GetValue(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      SQLiteType typ)
    {
      TypeAffinity affinity = typ.Affinity;
      if (affinity == TypeAffinity.Null)
        return (object) DBNull.Value;
      Type type = (Type) null;
      if (typ.Type != DbType.Object)
      {
        type = SQLiteConvert.SQLiteTypeToType(typ);
        affinity = SQLiteConvert.TypeToAffinity(type, flags);
      }
      if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetAllAsText))
        return (object) this.GetText(stmt, index);
      switch (affinity)
      {
        case TypeAffinity.Int64:
          if (type == (Type) null)
            return (object) this.GetInt64(stmt, index);
          if (type == typeof (bool))
            return (object) this.GetBoolean(stmt, index);
          if (type == typeof (sbyte))
            return (object) this.GetSByte(stmt, index);
          if (type == typeof (byte))
            return (object) this.GetByte(stmt, index);
          if (type == typeof (short))
            return (object) this.GetInt16(stmt, index);
          if (type == typeof (ushort))
            return (object) this.GetUInt16(stmt, index);
          if (type == typeof (int))
            return (object) this.GetInt32(stmt, index);
          if (type == typeof (uint))
            return (object) this.GetUInt32(stmt, index);
          if (type == typeof (long))
            return (object) this.GetInt64(stmt, index);
          return type == typeof (ulong) ? (object) this.GetUInt64(stmt, index) : Convert.ChangeType((object) this.GetInt64(stmt, index), type, HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetInvariantInt64) ? (IFormatProvider) CultureInfo.InvariantCulture : (IFormatProvider) CultureInfo.CurrentCulture);
        case TypeAffinity.Double:
          return type == (Type) null ? (object) this.GetDouble(stmt, index) : Convert.ChangeType((object) this.GetDouble(stmt, index), type, HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetInvariantDouble) ? (IFormatProvider) CultureInfo.InvariantCulture : (IFormatProvider) CultureInfo.CurrentCulture);
        case TypeAffinity.Blob:
          if (typ.Type == DbType.Guid && typ.Affinity == TypeAffinity.Text)
            return (object) new Guid(this.GetText(stmt, index));
          int bytes = (int) this.GetBytes(stmt, index, 0, (byte[]) null, 0, 0);
          byte[] numArray = new byte[bytes];
          this.GetBytes(stmt, index, 0, numArray, 0, bytes);
          return typ.Type == DbType.Guid && bytes == 16 ? (object) new Guid(numArray) : (object) numArray;
        case TypeAffinity.DateTime:
          return (object) this.GetDateTime(stmt, index);
        default:
          return (object) this.GetText(stmt, index);
      }
    }

    internal override int GetCursorForTable(SQLiteStatement stmt, int db, int rootPage) => UnsafeNativeMethods.sqlite3_table_cursor_interop((IntPtr) stmt._sqlite_stmt, db, rootPage);

    internal override long GetRowIdForCursor(SQLiteStatement stmt, int cursor)
    {
      long rowid = 0;
      return UnsafeNativeMethods.sqlite3_cursor_rowid_interop((IntPtr) stmt._sqlite_stmt, cursor, ref rowid) == SQLiteErrorCode.Ok ? rowid : 0L;
    }

    internal override void GetIndexColumnExtendedInfo(
      string database,
      string index,
      string column,
      ref int sortMode,
      ref int onError,
      ref string collationSequence)
    {
      IntPtr zero = IntPtr.Zero;
      int colllen = 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_index_column_info_interop((IntPtr) this._sql, SQLiteConvert.ToUTF8(database), SQLiteConvert.ToUTF8(index), SQLiteConvert.ToUTF8(column), ref sortMode, ref onError, ref zero, ref colllen);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
      collationSequence = SQLiteConvert.UTF8ToString(zero, colllen);
    }

    internal override SQLiteErrorCode FileControl(
      string zDbName,
      int op,
      IntPtr pArg)
    {
      return UnsafeNativeMethods.sqlite3_file_control((IntPtr) this._sql, zDbName != null ? SQLiteConvert.ToUTF8(zDbName) : (byte[]) null, op, pArg);
    }
  }
}
