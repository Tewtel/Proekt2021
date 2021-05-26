// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModule
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a managed virtual table module implementation.
  /// It is not sealed and must be used as the base class for any
  /// user-defined virtual table module classes implemented in managed code.
  /// </summary>
  public abstract class SQLiteModule : ISQLiteManagedModule, IDisposable
  {
    /// <summary>
    /// The default version of the native sqlite3_module structure in use.
    /// </summary>
    private static readonly int DefaultModuleVersion = 2;
    /// <summary>
    /// This field is used to store the native sqlite3_module structure
    /// associated with this object instance.
    /// </summary>
    private UnsafeNativeMethods.sqlite3_module nativeModule;
    /// <summary>
    /// This field is used to store the destructor delegate to be passed to
    /// the SQLite core library via the sqlite3_create_disposable_module()
    /// function.
    /// </summary>
    private UnsafeNativeMethods.xDestroyModule destroyModule;
    /// <summary>
    /// This field is used to store a pointer to the native sqlite3_module
    /// structure returned by the sqlite3_create_disposable_module
    /// function.
    /// </summary>
    private IntPtr disposableModule;
    /// <summary>
    /// This field is used to store the virtual table instances associated
    /// with this module.  The native pointer to the sqlite3_vtab derived
    /// structure is used to key into this collection.
    /// </summary>
    private Dictionary<IntPtr, SQLiteVirtualTable> tables;
    /// <summary>
    /// This field is used to store the virtual table cursor instances
    /// associated with this module.  The native pointer to the
    /// sqlite3_vtab_cursor derived structure is used to key into this
    /// collection.
    /// </summary>
    private Dictionary<IntPtr, SQLiteVirtualTableCursor> cursors;
    /// <summary>
    /// This field is used to store the virtual table function instances
    /// associated with this module.  The case-insensitive function name
    /// and the number of arguments (with -1 meaning "any") are used to
    /// construct the string that is used to key into this collection.
    /// </summary>
    private Dictionary<string, SQLiteFunction> functions;
    private bool logErrors;
    private bool logExceptions;
    private bool declared;
    private string name;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    public SQLiteModule(string name)
    {
      this.name = name != null ? name : throw new ArgumentNullException(nameof (name));
      this.tables = new Dictionary<IntPtr, SQLiteVirtualTable>();
      this.cursors = new Dictionary<IntPtr, SQLiteVirtualTableCursor>();
      this.functions = new Dictionary<string, SQLiteFunction>();
    }

    /// <summary>
    /// Calls the native SQLite core library in order to create a new
    /// disposable module containing the implementation of a virtual table.
    /// </summary>
    /// <param name="pDb">The native database connection pointer to use.</param>
    /// <returns>Non-zero upon success.</returns>
    internal bool CreateDisposableModule(IntPtr pDb)
    {
      if (this.disposableModule != IntPtr.Zero)
        return true;
      IntPtr num = IntPtr.Zero;
      try
      {
        num = SQLiteString.Utf8IntPtrFromString(this.name);
        UnsafeNativeMethods.sqlite3_module module = this.AllocateNativeModule();
        this.destroyModule = new UnsafeNativeMethods.xDestroyModule(this.xDestroyModule);
        this.disposableModule = UnsafeNativeMethods.sqlite3_create_disposable_module(pDb, num, ref module, IntPtr.Zero, this.destroyModule);
        return this.disposableModule != IntPtr.Zero;
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
    /// This method is called by the SQLite core library when the native
    /// module associated with this object instance is being destroyed due
    /// to its parent connection being closed.  It may also be called by
    /// the "vtshim" module if/when the sqlite3_dispose_module() function
    /// is called.
    /// </summary>
    /// <param name="pClientData">
    /// The native user-data pointer associated with this module, as it was
    /// provided to the SQLite core library when the native module instance
    /// was created.
    /// </param>
    private void xDestroyModule(IntPtr pClientData) => this.disposableModule = IntPtr.Zero;

    /// <summary>
    /// Creates and returns the native sqlite_module structure using the
    /// configured (or default) <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
    /// interface implementation.
    /// </summary>
    /// <returns>
    /// The native sqlite_module structure using the configured (or
    /// default) <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface
    /// implementation.
    /// </returns>
    private UnsafeNativeMethods.sqlite3_module AllocateNativeModule() => this.AllocateNativeModule(this.GetNativeModuleImpl());

    /// <summary>
    /// Creates and returns the native sqlite_module structure using the
    /// specified <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface
    /// implementation.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface implementation to
    /// use.
    /// </param>
    /// <returns>
    /// The native sqlite_module structure using the specified
    /// <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface implementation.
    /// </returns>
    private UnsafeNativeMethods.sqlite3_module AllocateNativeModule(
      ISQLiteNativeModule module)
    {
      this.nativeModule = new UnsafeNativeMethods.sqlite3_module();
      this.nativeModule.iVersion = SQLiteModule.DefaultModuleVersion;
      if (module != null)
      {
        this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(module.xCreate);
        this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(module.xConnect);
        this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(module.xBestIndex);
        this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(module.xDisconnect);
        this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(module.xDestroy);
        this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(module.xOpen);
        this.nativeModule.xClose = new UnsafeNativeMethods.xClose(module.xClose);
        this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(module.xFilter);
        this.nativeModule.xNext = new UnsafeNativeMethods.xNext(module.xNext);
        this.nativeModule.xEof = new UnsafeNativeMethods.xEof(module.xEof);
        this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(module.xColumn);
        this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(module.xRowId);
        this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(module.xUpdate);
        this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(module.xBegin);
        this.nativeModule.xSync = new UnsafeNativeMethods.xSync(module.xSync);
        this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(module.xCommit);
        this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(module.xRollback);
        this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(module.xFindFunction);
        this.nativeModule.xRename = new UnsafeNativeMethods.xRename(module.xRename);
        this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(module.xSavepoint);
        this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(module.xRelease);
        this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(module.xRollbackTo);
      }
      else
      {
        this.nativeModule.xCreate = new UnsafeNativeMethods.xCreate(this.xCreate);
        this.nativeModule.xConnect = new UnsafeNativeMethods.xConnect(this.xConnect);
        this.nativeModule.xBestIndex = new UnsafeNativeMethods.xBestIndex(this.xBestIndex);
        this.nativeModule.xDisconnect = new UnsafeNativeMethods.xDisconnect(this.xDisconnect);
        this.nativeModule.xDestroy = new UnsafeNativeMethods.xDestroy(this.xDestroy);
        this.nativeModule.xOpen = new UnsafeNativeMethods.xOpen(this.xOpen);
        this.nativeModule.xClose = new UnsafeNativeMethods.xClose(this.xClose);
        this.nativeModule.xFilter = new UnsafeNativeMethods.xFilter(this.xFilter);
        this.nativeModule.xNext = new UnsafeNativeMethods.xNext(this.xNext);
        this.nativeModule.xEof = new UnsafeNativeMethods.xEof(this.xEof);
        this.nativeModule.xColumn = new UnsafeNativeMethods.xColumn(this.xColumn);
        this.nativeModule.xRowId = new UnsafeNativeMethods.xRowId(this.xRowId);
        this.nativeModule.xUpdate = new UnsafeNativeMethods.xUpdate(this.xUpdate);
        this.nativeModule.xBegin = new UnsafeNativeMethods.xBegin(this.xBegin);
        this.nativeModule.xSync = new UnsafeNativeMethods.xSync(this.xSync);
        this.nativeModule.xCommit = new UnsafeNativeMethods.xCommit(this.xCommit);
        this.nativeModule.xRollback = new UnsafeNativeMethods.xRollback(this.xRollback);
        this.nativeModule.xFindFunction = new UnsafeNativeMethods.xFindFunction(this.xFindFunction);
        this.nativeModule.xRename = new UnsafeNativeMethods.xRename(this.xRename);
        this.nativeModule.xSavepoint = new UnsafeNativeMethods.xSavepoint(this.xSavepoint);
        this.nativeModule.xRelease = new UnsafeNativeMethods.xRelease(this.xRelease);
        this.nativeModule.xRollbackTo = new UnsafeNativeMethods.xRollbackTo(this.xRollbackTo);
      }
      return this.nativeModule;
    }

    /// <summary>
    /// Creates a copy of the specified
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_module" /> object instance,
    /// using default implementations for the contained delegates when
    /// necessary.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_module" /> object
    /// instance to copy.
    /// </param>
    /// <returns>
    /// The new <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_module" /> object
    /// instance.
    /// </returns>
    private UnsafeNativeMethods.sqlite3_module CopyNativeModule(
      UnsafeNativeMethods.sqlite3_module module)
    {
      return new UnsafeNativeMethods.sqlite3_module()
      {
        iVersion = module.iVersion,
        xCreate = new UnsafeNativeMethods.xCreate((module.xCreate != null ? module.xCreate : new UnsafeNativeMethods.xCreate(this.xCreate)).Invoke),
        xConnect = new UnsafeNativeMethods.xConnect((module.xConnect != null ? module.xConnect : new UnsafeNativeMethods.xConnect(this.xConnect)).Invoke),
        xBestIndex = new UnsafeNativeMethods.xBestIndex((module.xBestIndex != null ? module.xBestIndex : new UnsafeNativeMethods.xBestIndex(this.xBestIndex)).Invoke),
        xDisconnect = new UnsafeNativeMethods.xDisconnect((module.xDisconnect != null ? module.xDisconnect : new UnsafeNativeMethods.xDisconnect(this.xDisconnect)).Invoke),
        xDestroy = new UnsafeNativeMethods.xDestroy((module.xDestroy != null ? module.xDestroy : new UnsafeNativeMethods.xDestroy(this.xDestroy)).Invoke),
        xOpen = new UnsafeNativeMethods.xOpen((module.xOpen != null ? module.xOpen : new UnsafeNativeMethods.xOpen(this.xOpen)).Invoke),
        xClose = new UnsafeNativeMethods.xClose((module.xClose != null ? module.xClose : new UnsafeNativeMethods.xClose(this.xClose)).Invoke),
        xFilter = new UnsafeNativeMethods.xFilter((module.xFilter != null ? module.xFilter : new UnsafeNativeMethods.xFilter(this.xFilter)).Invoke),
        xNext = new UnsafeNativeMethods.xNext((module.xNext != null ? module.xNext : new UnsafeNativeMethods.xNext(this.xNext)).Invoke),
        xEof = new UnsafeNativeMethods.xEof((module.xEof != null ? module.xEof : new UnsafeNativeMethods.xEof(this.xEof)).Invoke),
        xColumn = new UnsafeNativeMethods.xColumn((module.xColumn != null ? module.xColumn : new UnsafeNativeMethods.xColumn(this.xColumn)).Invoke),
        xRowId = new UnsafeNativeMethods.xRowId((module.xRowId != null ? module.xRowId : new UnsafeNativeMethods.xRowId(this.xRowId)).Invoke),
        xUpdate = new UnsafeNativeMethods.xUpdate((module.xUpdate != null ? module.xUpdate : new UnsafeNativeMethods.xUpdate(this.xUpdate)).Invoke),
        xBegin = new UnsafeNativeMethods.xBegin((module.xBegin != null ? module.xBegin : new UnsafeNativeMethods.xBegin(this.xBegin)).Invoke),
        xSync = new UnsafeNativeMethods.xSync((module.xSync != null ? module.xSync : new UnsafeNativeMethods.xSync(this.xSync)).Invoke),
        xCommit = new UnsafeNativeMethods.xCommit((module.xCommit != null ? module.xCommit : new UnsafeNativeMethods.xCommit(this.xCommit)).Invoke),
        xRollback = new UnsafeNativeMethods.xRollback((module.xRollback != null ? module.xRollback : new UnsafeNativeMethods.xRollback(this.xRollback)).Invoke),
        xFindFunction = new UnsafeNativeMethods.xFindFunction((module.xFindFunction != null ? module.xFindFunction : new UnsafeNativeMethods.xFindFunction(this.xFindFunction)).Invoke),
        xRename = new UnsafeNativeMethods.xRename((module.xRename != null ? module.xRename : new UnsafeNativeMethods.xRename(this.xRename)).Invoke),
        xSavepoint = new UnsafeNativeMethods.xSavepoint((module.xSavepoint != null ? module.xSavepoint : new UnsafeNativeMethods.xSavepoint(this.xSavepoint)).Invoke),
        xRelease = new UnsafeNativeMethods.xRelease((module.xRelease != null ? module.xRelease : new UnsafeNativeMethods.xRelease(this.xRelease)).Invoke),
        xRollbackTo = new UnsafeNativeMethods.xRollbackTo((module.xRollbackTo != null ? module.xRollbackTo : new UnsafeNativeMethods.xRollbackTo(this.xRollbackTo)).Invoke)
      };
    }

    /// <summary>
    /// Calls one of the virtual table initialization methods.
    /// </summary>
    /// <param name="create">
    /// Non-zero to call the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" />
    /// method; otherwise, the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" />
    /// method will be called.
    /// </param>
    /// <param name="pDb">The native database connection handle.</param>
    /// <param name="pAux">
    /// The original native pointer value that was provided to the
    /// sqlite3_create_module(), sqlite3_create_module_v2() or
    /// sqlite3_create_disposable_module() functions.
    /// </param>
    /// <param name="argc">
    /// The number of arguments from the CREATE VIRTUAL TABLE statement.
    /// </param>
    /// <param name="argv">
    /// The array of string arguments from the CREATE VIRTUAL TABLE
    /// statement.
    /// </param>
    /// <param name="pVtab">
    /// Upon success, this parameter must be modified to point to the newly
    /// created native sqlite3_vtab derived structure.
    /// </param>
    /// <param name="pError">
    /// Upon failure, this parameter must be modified to point to the error
    /// message, with the underlying memory having been obtained from the
    /// sqlite3_malloc() function.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    private SQLiteErrorCode CreateOrConnect(
      bool create,
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      try
      {
        string fileName = SQLiteString.StringFromUtf8IntPtr(UnsafeNativeMethods.sqlite3_db_filename(pDb, IntPtr.Zero));
        using (SQLiteConnection connection = new SQLiteConnection(pDb, fileName, false))
        {
          SQLiteVirtualTable table = (SQLiteVirtualTable) null;
          string error = (string) null;
          if (create && this.Create(connection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref table, ref error) == SQLiteErrorCode.Ok || !create && this.Connect(connection, pAux, SQLiteString.StringArrayFromUtf8SizeAndIntPtr(argc, argv), ref table, ref error) == SQLiteErrorCode.Ok)
          {
            if (table != null)
            {
              pVtab = this.TableToIntPtr(table);
              return SQLiteErrorCode.Ok;
            }
            pError = SQLiteString.Utf8IntPtrFromString("no table was created");
          }
          else
            pError = SQLiteString.Utf8IntPtrFromString(error);
        }
      }
      catch (Exception ex)
      {
        pError = SQLiteString.Utf8IntPtrFromString(ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>Calls one of the virtual table finalization methods.</summary>
    /// <param name="destroy">
    /// Non-zero to call the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Destroy(System.Data.SQLite.SQLiteVirtualTable)" />
    /// method; otherwise, the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Disconnect(System.Data.SQLite.SQLiteVirtualTable)" /> method will be
    /// called.
    /// </param>
    /// <param name="pVtab">
    /// The native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    private SQLiteErrorCode DestroyOrDisconnect(bool destroy, IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          if (!destroy || this.Destroy(table) != SQLiteErrorCode.Ok)
          {
            if (!destroy)
            {
              if (this.Disconnect(table) != SQLiteErrorCode.Ok)
                goto label_12;
            }
            else
              goto label_12;
          }
          if (this.tables != null)
            this.tables.Remove(pVtab);
          return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (this.LogExceptionsNoThrow)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", destroy ? (object) "xDestroy" : (object) "xDisconnect", (object) ex));
        }
        catch
        {
        }
      }
      finally
      {
        this.FreeTable(pVtab);
      }
label_12:
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance to be used.
    /// </param>
    /// <param name="pVtab">
    /// The native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="logErrors">
    /// Non-zero if this error message should also be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="logExceptions">
    /// Non-zero if caught exceptions should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    private static bool SetTableError(
      SQLiteModule module,
      IntPtr pVtab,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      try
      {
        if (logErrors)
        {
          if (error != null)
            SQLiteLog.LogMessage(SQLiteErrorCode.Error, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Virtual table error: {0}", (object) error));
        }
      }
      catch
      {
      }
      bool flag = false;
      IntPtr pMemory1 = IntPtr.Zero;
      try
      {
        if (pVtab == IntPtr.Zero)
          return false;
        int offset = SQLiteMarshal.NextOffsetOf(SQLiteMarshal.NextOffsetOf(0, IntPtr.Size, 4), 4, IntPtr.Size);
        IntPtr pMemory2 = SQLiteMarshal.ReadIntPtr(pVtab, offset);
        if (pMemory2 != IntPtr.Zero)
        {
          SQLiteMemory.Free(pMemory2);
          IntPtr zero = IntPtr.Zero;
          SQLiteMarshal.WriteIntPtr(pVtab, offset, zero);
        }
        if (error == null)
          return true;
        pMemory1 = SQLiteString.Utf8IntPtrFromString(error);
        SQLiteMarshal.WriteIntPtr(pVtab, offset, pMemory1);
        flag = true;
      }
      catch (Exception ex)
      {
        try
        {
          if (logExceptions)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) nameof (SetTableError), (object) ex));
        }
        catch
        {
        }
      }
      finally
      {
        if (!flag && pMemory1 != IntPtr.Zero)
        {
          SQLiteMemory.Free(pMemory1);
          IntPtr zero = IntPtr.Zero;
        }
      }
      return flag;
    }

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance to be used.
    /// </param>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance used to
    /// lookup the native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="logErrors">
    /// Non-zero if this error message should also be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="logExceptions">
    /// Non-zero if caught exceptions should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    private static bool SetTableError(
      SQLiteModule module,
      SQLiteVirtualTable table,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (table == null)
        return false;
      IntPtr nativeHandle = table.NativeHandle;
      return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetTableError(module, nativeHandle, logErrors, logExceptions, error);
    }

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance to be used.
    /// </param>
    /// <param name="pCursor">
    /// The native pointer to the sqlite3_vtab_cursor derived structure
    /// used to get the native pointer to the sqlite3_vtab derived
    /// structure.
    /// </param>
    /// <param name="logErrors">
    /// Non-zero if this error message should also be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="logExceptions">
    /// Non-zero if caught exceptions should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    private static bool SetCursorError(
      SQLiteModule module,
      IntPtr pCursor,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (pCursor == IntPtr.Zero)
        return false;
      IntPtr pVtab = SQLiteModule.TableFromCursor(module, pCursor);
      return !(pVtab == IntPtr.Zero) && SQLiteModule.SetTableError(module, pVtab, logErrors, logExceptions, error);
    }

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance to be used.
    /// </param>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance used to
    /// lookup the native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="logErrors">
    /// Non-zero if this error message should also be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="logExceptions">
    /// Non-zero if caught exceptions should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    private static bool SetCursorError(
      SQLiteModule module,
      SQLiteVirtualTableCursor cursor,
      bool logErrors,
      bool logExceptions,
      string error)
    {
      if (cursor == null)
        return false;
      IntPtr nativeHandle = cursor.NativeHandle;
      return !(nativeHandle == IntPtr.Zero) && SQLiteModule.SetCursorError(module, nativeHandle, logErrors, logExceptions, error);
    }

    /// <summary>
    /// Gets and returns the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface
    /// implementation to be used when creating the native sqlite3_module
    /// structure.  Derived classes may override this method to supply an
    /// alternate implementation for the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
    /// interface.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface implementation to
    /// be used when populating the native sqlite3_module structure.  If
    /// the returned value is null, the private methods provided by the
    /// <see cref="T:System.Data.SQLite.SQLiteModule" /> class and relating to the
    /// <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface  will be used to
    /// create the necessary delegates.
    /// </returns>
    protected virtual ISQLiteNativeModule GetNativeModuleImpl() => (ISQLiteNativeModule) null;

    /// <summary>
    /// Creates and returns the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
    /// interface implementation corresponding to the current
    /// <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> interface implementation
    /// corresponding to the current <see cref="T:System.Data.SQLite.SQLiteModule" /> object
    /// instance.
    /// </returns>
    protected virtual ISQLiteNativeModule CreateNativeModuleImpl() => (ISQLiteNativeModule) new SQLiteModule.SQLiteNativeModule(this);

    /// <summary>
    /// Allocates a native sqlite3_vtab derived structure and returns a
    /// native pointer to it.
    /// </summary>
    /// <returns>
    /// A native pointer to a native sqlite3_vtab derived structure.
    /// </returns>
    protected virtual IntPtr AllocateTable() => SQLiteMemory.Allocate(Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_vtab)));

    /// <summary>
    /// Zeros out the fields of a native sqlite3_vtab derived structure.
    /// </summary>
    /// <param name="pVtab">
    /// The native pointer to the native sqlite3_vtab derived structure to
    /// zero.
    /// </param>
    protected virtual void ZeroTable(IntPtr pVtab)
    {
      if (pVtab == IntPtr.Zero)
        return;
      int offset1 = 0;
      SQLiteMarshal.WriteIntPtr(pVtab, offset1, IntPtr.Zero);
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, IntPtr.Size, 4);
      SQLiteMarshal.WriteInt32(pVtab, offset2, 0);
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, 4, IntPtr.Size);
      SQLiteMarshal.WriteIntPtr(pVtab, offset3, IntPtr.Zero);
    }

    /// <summary>
    /// Frees a native sqlite3_vtab structure using the provided native
    /// pointer to it.
    /// </summary>
    /// <param name="pVtab">
    /// A native pointer to a native sqlite3_vtab derived structure.
    /// </param>
    protected virtual void FreeTable(IntPtr pVtab)
    {
      this.SetTableError(pVtab, (string) null);
      SQLiteMemory.Free(pVtab);
    }

    /// <summary>
    /// Allocates a native sqlite3_vtab_cursor derived structure and
    /// returns a native pointer to it.
    /// </summary>
    /// <returns>
    /// A native pointer to a native sqlite3_vtab_cursor derived structure.
    /// </returns>
    protected virtual IntPtr AllocateCursor() => SQLiteMemory.Allocate(Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_vtab_cursor)));

    /// <summary>
    /// Frees a native sqlite3_vtab_cursor structure using the provided
    /// native pointer to it.
    /// </summary>
    /// <param name="pCursor">
    /// A native pointer to a native sqlite3_vtab_cursor derived structure.
    /// </param>
    protected virtual void FreeCursor(IntPtr pCursor) => SQLiteMemory.Free(pCursor);

    /// <summary>
    /// Reads and returns the native pointer to the sqlite3_vtab derived
    /// structure based on the native pointer to the sqlite3_vtab_cursor
    /// derived structure.
    /// </summary>
    /// <param name="module">
    /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance to be used.
    /// </param>
    /// <param name="pCursor">
    /// The native pointer to the sqlite3_vtab_cursor derived structure
    /// from which to read the native pointer to the sqlite3_vtab derived
    /// structure.
    /// </param>
    /// <returns>
    /// The native pointer to the sqlite3_vtab derived structure -OR-
    /// <see cref="F:System.IntPtr.Zero" /> if it cannot be determined.
    /// </returns>
    private static IntPtr TableFromCursor(SQLiteModule module, IntPtr pCursor) => pCursor == IntPtr.Zero ? IntPtr.Zero : Marshal.ReadIntPtr(pCursor);

    /// <summary>
    /// Reads and returns the native pointer to the sqlite3_vtab derived
    /// structure based on the native pointer to the sqlite3_vtab_cursor
    /// derived structure.
    /// </summary>
    /// <param name="pCursor">
    /// The native pointer to the sqlite3_vtab_cursor derived structure
    /// from which to read the native pointer to the sqlite3_vtab derived
    /// structure.
    /// </param>
    /// <returns>
    /// The native pointer to the sqlite3_vtab derived structure -OR-
    /// <see cref="F:System.IntPtr.Zero" /> if it cannot be determined.
    /// </returns>
    protected virtual IntPtr TableFromCursor(IntPtr pCursor) => SQLiteModule.TableFromCursor(this, pCursor);

    /// <summary>
    /// Looks up and returns the <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object
    /// instance based on the native pointer to the sqlite3_vtab derived
    /// structure.
    /// </summary>
    /// <param name="pVtab">
    /// The native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance or null if
    /// the corresponding one cannot be found.
    /// </returns>
    protected virtual SQLiteVirtualTable TableFromIntPtr(IntPtr pVtab)
    {
      if (pVtab == IntPtr.Zero)
      {
        this.SetTableError(pVtab, "invalid native table");
        return (SQLiteVirtualTable) null;
      }
      SQLiteVirtualTable liteVirtualTable;
      if (this.tables != null && this.tables.TryGetValue(pVtab, out liteVirtualTable))
        return liteVirtualTable;
      this.SetTableError(pVtab, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "managed table for {0} not found", (object) pVtab));
      return (SQLiteVirtualTable) null;
    }

    /// <summary>
    /// Allocates and returns a native pointer to a sqlite3_vtab derived
    /// structure and creates an association between it and the specified
    /// <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance to be used
    /// when creating the association.
    /// </param>
    /// <returns>
    /// The native pointer to a sqlite3_vtab derived structure or
    /// <see cref="F:System.IntPtr.Zero" /> if the method fails for any reason.
    /// </returns>
    protected virtual IntPtr TableToIntPtr(SQLiteVirtualTable table)
    {
      if (table == null || this.tables == null)
        return IntPtr.Zero;
      IntPtr num = IntPtr.Zero;
      bool flag = false;
      try
      {
        num = this.AllocateTable();
        if (num != IntPtr.Zero)
        {
          this.ZeroTable(num);
          table.NativeHandle = num;
          this.tables.Add(num, table);
          flag = true;
        }
      }
      finally
      {
        if (!flag && num != IntPtr.Zero)
        {
          this.FreeTable(num);
          num = IntPtr.Zero;
        }
      }
      return num;
    }

    /// <summary>
    /// Looks up and returns the <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" />
    /// object instance based on the native pointer to the
    /// sqlite3_vtab_cursor derived structure.
    /// </summary>
    /// <param name="pVtab">
    /// The native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="pCursor">
    /// The native pointer to the sqlite3_vtab_cursor derived structure.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance or null
    /// if the corresponding one cannot be found.
    /// </returns>
    protected virtual SQLiteVirtualTableCursor CursorFromIntPtr(
      IntPtr pVtab,
      IntPtr pCursor)
    {
      if (pCursor == IntPtr.Zero)
      {
        this.SetTableError(pVtab, "invalid native cursor");
        return (SQLiteVirtualTableCursor) null;
      }
      SQLiteVirtualTableCursor virtualTableCursor;
      if (this.cursors != null && this.cursors.TryGetValue(pCursor, out virtualTableCursor))
        return virtualTableCursor;
      this.SetTableError(pVtab, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "managed cursor for {0} not found", (object) pCursor));
      return (SQLiteVirtualTableCursor) null;
    }

    /// <summary>
    /// Allocates and returns a native pointer to a sqlite3_vtab_cursor
    /// derived structure and creates an association between it and the
    /// specified <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance to be
    /// used when creating the association.
    /// </param>
    /// <returns>
    /// The native pointer to a sqlite3_vtab_cursor derived structure or
    /// <see cref="F:System.IntPtr.Zero" /> if the method fails for any reason.
    /// </returns>
    protected virtual IntPtr CursorToIntPtr(SQLiteVirtualTableCursor cursor)
    {
      if (cursor == null || this.cursors == null)
        return IntPtr.Zero;
      IntPtr num = IntPtr.Zero;
      bool flag = false;
      try
      {
        num = this.AllocateCursor();
        if (num != IntPtr.Zero)
        {
          cursor.NativeHandle = num;
          this.cursors.Add(num, cursor);
          flag = true;
        }
      }
      finally
      {
        if (!flag && num != IntPtr.Zero)
        {
          this.FreeCursor(num);
          num = IntPtr.Zero;
        }
      }
      return num;
    }

    /// <summary>
    /// Deterimines the key that should be used to identify and store the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance for the virtual table
    /// (i.e. to be returned via the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method).
    /// </summary>
    /// <param name="argumentCount">
    /// The number of arguments to the virtual table function.
    /// </param>
    /// <param name="name">The name of the virtual table function.</param>
    /// <param name="function">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance associated with
    /// this virtual table function.
    /// </param>
    /// <returns>
    /// The string that should be used to identify and store the virtual
    /// table function instance.  This method cannot return null.  If null
    /// is returned from this method, the behavior is undefined.
    /// </returns>
    protected virtual string GetFunctionKey(
      int argumentCount,
      string name,
      SQLiteFunction function)
    {
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}:{1}", (object) argumentCount, (object) name);
    }

    /// <summary>
    /// Attempts to declare the schema for the virtual table using the
    /// specified database connection.
    /// </summary>
    /// <param name="connection">
    /// The <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance to use when
    /// declaring the schema of the virtual table.  This parameter may not
    /// be null.
    /// </param>
    /// <param name="sql">
    /// The string containing the CREATE TABLE statement that completely
    /// describes the schema for the virtual table.  This parameter may not
    /// be null.
    /// </param>
    /// <param name="error">
    /// Upon failure, this parameter must be modified to contain an error
    /// message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    protected virtual SQLiteErrorCode DeclareTable(
      SQLiteConnection connection,
      string sql,
      ref string error)
    {
      if (connection == null)
      {
        error = "invalid connection";
        return SQLiteErrorCode.Error;
      }
      SQLiteBase sql1 = connection._sql;
      if (sql1 == null)
      {
        error = "connection has invalid handle";
        return SQLiteErrorCode.Error;
      }
      if (sql != null)
        return sql1.DeclareVirtualTable(this, sql, ref error);
      error = "invalid SQL statement";
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Calls the native SQLite core library in order to declare a virtual
    /// table function in response to a call into the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" />
    /// or <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> virtual table
    /// methods.
    /// </summary>
    /// <param name="connection">
    /// The <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance to use when
    /// declaring the schema of the virtual table.
    /// </param>
    /// <param name="argumentCount">
    /// The number of arguments to the function being declared.
    /// </param>
    /// <param name="name">The name of the function being declared.</param>
    /// <param name="error">
    /// Upon success, the contents of this parameter are undefined.  Upon
    /// failure, it should contain an appropriate error message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    protected virtual SQLiteErrorCode DeclareFunction(
      SQLiteConnection connection,
      int argumentCount,
      string name,
      ref string error)
    {
      if (connection == null)
      {
        error = "invalid connection";
        return SQLiteErrorCode.Error;
      }
      SQLiteBase sql = connection._sql;
      if (sql != null)
        return sql.DeclareVirtualFunction(this, argumentCount, name, ref error);
      error = "connection has invalid handle";
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Returns or sets a boolean value indicating whether virtual table
    /// errors should be logged using the <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </summary>
    protected virtual bool LogErrorsNoThrow
    {
      get => this.logErrors;
      set => this.logErrors = value;
    }

    /// <summary>
    /// Returns or sets a boolean value indicating whether exceptions
    /// caught in the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method,
    /// the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method,
    /// the <see cref="M:System.Data.SQLite.SQLiteModule.SetTableError(System.IntPtr,System.String)" /> method,
    /// the <see cref="M:System.Data.SQLite.SQLiteModule.SetTableError(System.Data.SQLite.SQLiteVirtualTable,System.String)" /> method,
    /// and the <see cref="M:System.Data.SQLite.SQLiteModule.Dispose" /> method should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </summary>
    protected virtual bool LogExceptionsNoThrow
    {
      get => this.logExceptions;
      set => this.logExceptions = value;
    }

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="pVtab">
    /// The native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetTableError(IntPtr pVtab, string error) => SQLiteModule.SetTableError(this, pVtab, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance used to
    /// lookup the native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetTableError(SQLiteVirtualTable table, string error) => SQLiteModule.SetTableError(this, table, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);

    /// <summary>
    /// Arranges for the specified error message to be placed into the
    /// zErrMsg field of a sqlite3_vtab derived structure, freeing the
    /// existing error message, if any.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance used to
    /// lookup the native pointer to the sqlite3_vtab derived structure.
    /// </param>
    /// <param name="error">The error message.</param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetCursorError(SQLiteVirtualTableCursor cursor, string error) => SQLiteModule.SetCursorError(this, cursor, this.LogErrorsNoThrow, this.LogExceptionsNoThrow, error);

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the specified estimated cost.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <param name="estimatedCost">
    /// The estimated cost value to use.  Using a null value means that the
    /// default value provided by the SQLite core library should be used.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetEstimatedCost(SQLiteIndex index, double? estimatedCost)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.EstimatedCost = estimatedCost;
      return true;
    }

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the default estimated cost.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetEstimatedCost(SQLiteIndex index) => this.SetEstimatedCost(index, new double?());

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the specified estimated rows.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <param name="estimatedRows">
    /// The estimated rows value to use.  Using a null value means that the
    /// default value provided by the SQLite core library should be used.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetEstimatedRows(SQLiteIndex index, long? estimatedRows)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.EstimatedRows = estimatedRows;
      return true;
    }

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the default estimated rows.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetEstimatedRows(SQLiteIndex index) => this.SetEstimatedRows(index, new long?());

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the specified flags.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <param name="indexFlags">
    /// The index flags value to use.  Using a null value means that the
    /// default value provided by the SQLite core library should be used.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetIndexFlags(SQLiteIndex index, SQLiteIndexFlags? indexFlags)
    {
      if (index == null || index.Outputs == null)
        return false;
      index.Outputs.IndexFlags = indexFlags;
      return true;
    }

    /// <summary>
    /// Modifies the specified <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance
    /// to contain the default index flags.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance to modify.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetIndexFlags(SQLiteIndex index) => this.SetIndexFlags(index, new SQLiteIndexFlags?());

    /// <summary>
    /// Returns or sets a boolean value indicating whether virtual table
    /// errors should be logged using the <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </summary>
    public virtual bool LogErrors
    {
      get
      {
        this.CheckDisposed();
        return this.LogErrorsNoThrow;
      }
      set
      {
        this.CheckDisposed();
        this.LogErrorsNoThrow = value;
      }
    }

    /// <summary>
    /// Returns or sets a boolean value indicating whether exceptions
    /// caught in the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method,
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method, and the
    /// <see cref="M:System.Data.SQLite.SQLiteModule.Dispose" /> method should be logged using the
    /// <see cref="T:System.Data.SQLite.SQLiteLog" /> class.
    /// </summary>
    public virtual bool LogExceptions
    {
      get
      {
        this.CheckDisposed();
        return this.LogExceptionsNoThrow;
      }
      set
      {
        this.CheckDisposed();
        this.LogExceptionsNoThrow = value;
      }
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="pDb">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pAux">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="argc">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="argv">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pError">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </returns>
    private SQLiteErrorCode xCreate(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      return this.CreateOrConnect(true, pDb, pAux, argc, argv, ref pVtab, ref pError);
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="pDb">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pAux">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="argc">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="argv">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pError">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </returns>
    private SQLiteErrorCode xConnect(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError)
    {
      return this.CreateOrConnect(false, pDb, pAux, argc, argv, ref pVtab, ref pError);
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
    /// </param>
    /// <param name="pIndex">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          SQLiteIndex index = (SQLiteIndex) null;
          SQLiteIndex.FromIntPtr(pIndex, true, ref index);
          if (this.BestIndex(table, index) == SQLiteErrorCode.Ok)
          {
            SQLiteIndex.ToIntPtr(index, pIndex, true);
            return SQLiteErrorCode.Ok;
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xDisconnect(IntPtr pVtab) => this.DestroyOrDisconnect(false, pVtab);

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xDestroy(IntPtr pVtab) => this.DestroyOrDisconnect(true, pVtab);

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
    /// </returns>
    private SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          SQLiteVirtualTableCursor cursor = (SQLiteVirtualTableCursor) null;
          if (this.Open(table, ref cursor) == SQLiteErrorCode.Ok)
          {
            if (cursor != null)
            {
              pCursor = this.CursorToIntPtr(cursor);
              if (pCursor != IntPtr.Zero)
                return SQLiteErrorCode.Ok;
              this.SetTableError(pVtab, "no native cursor was created");
            }
            else
              this.SetTableError(pVtab, "no managed cursor was created");
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xClose(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Close(cursor) == SQLiteErrorCode.Ok)
          {
            if (this.cursors != null)
              this.cursors.Remove(pCursor);
            return SQLiteErrorCode.Ok;
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      finally
      {
        this.FreeCursor(pCursor);
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </param>
    /// <param name="idxNum">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </param>
    /// <param name="idxStr">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </param>
    /// <param name="argc">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </param>
    /// <param name="argv">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xFilter(
      IntPtr pCursor,
      int idxNum,
      IntPtr idxStr,
      int argc,
      IntPtr argv)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Filter(cursor, idxNum, SQLiteString.StringFromUtf8IntPtr(idxStr), SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv)) == SQLiteErrorCode.Ok)
            return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xNext(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          if (this.Next(cursor) == SQLiteErrorCode.Ok)
            return SQLiteErrorCode.Ok;
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
    /// </returns>
    private int xEof(IntPtr pCursor)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
          return this.Eof(cursor) ? 1 : 0;
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return 1;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <param name="pContext">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <param name="index">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </returns>
    private SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
        {
          SQLiteContext context = new SQLiteContext(pContext);
          return this.Column(cursor, context, index);
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
    /// </summary>
    /// <param name="pCursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <param name="rowId">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
    /// </returns>
    private SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId)
    {
      IntPtr pVtab = IntPtr.Zero;
      try
      {
        pVtab = this.TableFromCursor(pCursor);
        SQLiteVirtualTableCursor cursor = this.CursorFromIntPtr(pVtab, pCursor);
        if (cursor != null)
          return this.RowId(cursor, ref rowId);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <param name="argc">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <param name="argv">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <param name="rowId">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </returns>
    private SQLiteErrorCode xUpdate(
      IntPtr pVtab,
      int argc,
      IntPtr argv,
      ref long rowId)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Update(table, SQLiteValue.ArrayFromSizeAndIntPtr(argc, argv), ref rowId);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xBegin(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Begin(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xSync(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Sync(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xCommit(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Commit(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xRollback(IntPtr pVtab)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Rollback(table);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="nArg">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="zName">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="callback">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pClientData">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </returns>
    private int xFindFunction(
      IntPtr pVtab,
      int nArg,
      IntPtr zName,
      ref SQLiteCallback callback,
      ref IntPtr pClientData)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
        {
          string name = SQLiteString.StringFromUtf8IntPtr(zName);
          SQLiteFunction function = (SQLiteFunction) null;
          if (this.FindFunction(table, nArg, name, ref function, ref pClientData))
          {
            if (function != null)
            {
              this.functions[this.GetFunctionKey(nArg, name, function)] = function;
              callback = new SQLiteCallback(function.ScalarCallback);
              return 1;
            }
            this.SetTableError(pVtab, "no function was created");
          }
        }
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return 0;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
    /// </param>
    /// <param name="zNew">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
    /// </returns>
    private SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Rename(table, SQLiteString.StringFromUtf8IntPtr(zNew));
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <param name="iSavepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
    /// </returns>
    private SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Savepoint(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <param name="iSavepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
    /// </returns>
    private SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.Release(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="pVtab">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <param name="iSavepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
    /// </returns>
    private SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint)
    {
      try
      {
        SQLiteVirtualTable table = this.TableFromIntPtr(pVtab);
        if (table != null)
          return this.RollbackTo(table, iSavepoint);
      }
      catch (Exception ex)
      {
        this.SetTableError(pVtab, ex.ToString());
      }
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Returns non-zero if the schema for the virtual table has been
    /// declared.
    /// </summary>
    public virtual bool Declared
    {
      get
      {
        this.CheckDisposed();
        return this.declared;
      }
      internal set => this.declared = value;
    }

    /// <summary>
    /// Returns the name of the module as it was registered with the SQLite
    /// core library.
    /// </summary>
    public virtual string Name
    {
      get
      {
        this.CheckDisposed();
        return this.name;
      }
    }

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="connection">
    /// The <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance associated with
    /// the virtual table.
    /// </param>
    /// <param name="pClientData">
    /// The native user-data pointer associated with this module, as it was
    /// provided to the SQLite core library when the native module instance
    /// was created.
    /// </param>
    /// <param name="arguments">
    /// The module name, database name, virtual table name, and all other
    /// arguments passed to the CREATE VIRTUAL TABLE statement.
    /// </param>
    /// <param name="table">
    /// Upon success, this parameter must be modified to contain the
    /// <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated with
    /// the virtual table.
    /// </param>
    /// <param name="error">
    /// Upon failure, this parameter must be modified to contain an error
    /// message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="connection">
    /// The <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance associated with
    /// the virtual table.
    /// </param>
    /// <param name="pClientData">
    /// The native user-data pointer associated with this module, as it was
    /// provided to the SQLite core library when the native module instance
    /// was created.
    /// </param>
    /// <param name="arguments">
    /// The module name, database name, virtual table name, and all other
    /// arguments passed to the CREATE VIRTUAL TABLE statement.
    /// </param>
    /// <param name="table">
    /// Upon success, this parameter must be modified to contain the
    /// <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated with
    /// the virtual table.
    /// </param>
    /// <param name="error">
    /// Upon failure, this parameter must be modified to contain an error
    /// message.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance containing all the
    /// data for the inputs and outputs relating to index selection.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode BestIndex(
      SQLiteVirtualTable table,
      SQLiteIndex index);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Destroy(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="cursor">
    /// Upon success, this parameter must be modified to contain the
    /// <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance associated
    /// with the newly opened virtual table cursor.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <param name="indexNumber">
    /// Number used to help identify the selected index.
    /// </param>
    /// <param name="indexString">
    /// String used to help identify the selected index.
    /// </param>
    /// <param name="values">
    /// The values corresponding to each column in the selected index.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <returns>
    /// Non-zero if no more rows are available; zero otherwise.
    /// </returns>
    public abstract bool Eof(SQLiteVirtualTableCursor cursor);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <param name="context">
    /// The <see cref="T:System.Data.SQLite.SQLiteContext" /> object instance to be used for
    /// returning the specified column value to the SQLite core library.
    /// </param>
    /// <param name="index">
    /// The zero-based index corresponding to the column containing the
    /// value to be returned.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <param name="rowId">
    /// Upon success, this parameter must be modified to contain the unique
    /// integer row identifier for the current row for the specified cursor.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode RowId(
      SQLiteVirtualTableCursor cursor,
      ref long rowId);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="values">
    /// The array of <see cref="T:System.Data.SQLite.SQLiteValue" /> object instances containing
    /// the new or modified column values, if any.
    /// </param>
    /// <param name="rowId">
    /// Upon success, this parameter must be modified to contain the unique
    /// integer row identifier for the row that was inserted, if any.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Update(
      SQLiteVirtualTable table,
      SQLiteValue[] values,
      ref long rowId);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Begin(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Sync(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Commit(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Rollback(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="argumentCount">
    /// The number of arguments to the function being sought.
    /// </param>
    /// <param name="name">The name of the function being sought.</param>
    /// <param name="function">
    /// Upon success, this parameter must be modified to contain the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance responsible for
    /// implementing the specified function.
    /// </param>
    /// <param name="pClientData">
    /// Upon success, this parameter must be modified to contain the
    /// native user-data pointer associated with
    /// <paramref name="function" />.
    /// </param>
    /// <returns>
    /// Non-zero if the specified function was found; zero otherwise.
    /// </returns>
    public abstract bool FindFunction(
      SQLiteVirtualTable table,
      int argumentCount,
      string name,
      ref SQLiteFunction function,
      ref IntPtr pClientData);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="newName">The new name for the virtual table.</param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="savepoint">
    /// This is an integer identifier under which the the current state of
    /// the virtual table should be saved.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Savepoint(
      SQLiteVirtualTable table,
      int savepoint);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="savepoint">
    /// This is an integer used to indicate that any saved states with an
    /// identifier greater than or equal to this should be deleted by the
    /// virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <param name="savepoint">
    /// This is an integer identifier used to specify a specific saved
    /// state for the virtual table for it to restore itself back to, which
    /// should also have the effect of deleting all saved states with an
    /// integer identifier greater than this one.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    public abstract SQLiteErrorCode RollbackTo(
      SQLiteVirtualTable table,
      int savepoint);

    /// <summary>Disposes of this object instance.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModule).Name);
    }

    /// <summary>Disposes of this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this method is being called from the
    /// <see cref="M:System.Data.SQLite.SQLiteModule.Dispose" /> method.  Zero if this method is being
    /// called from the finalizer.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this.functions != null)
          this.functions.Clear();
      }
      try
      {
        if (this.disposableModule != IntPtr.Zero)
        {
          UnsafeNativeMethods.sqlite3_dispose_module(this.disposableModule);
          this.disposableModule = IntPtr.Zero;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (this.LogExceptionsNoThrow)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) nameof (Dispose), (object) ex));
        }
        catch
        {
        }
      }
      this.disposed = true;
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteModule() => this.Dispose(false);

    /// <summary>
    /// This class implements the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
    /// interface by forwarding those method calls to the
    /// <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance it contains.  If the
    /// contained <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance is null, all
    /// the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" /> methods simply generate an
    /// error.
    /// </summary>
    private sealed class SQLiteNativeModule : ISQLiteNativeModule, IDisposable
    {
      /// <summary>
      /// This is the value that is always used for the "logErrors"
      /// parameter to the various static error handling methods provided
      /// by the <see cref="T:System.Data.SQLite.SQLiteModule" /> class.
      /// </summary>
      private const bool DefaultLogErrors = true;
      /// <summary>
      /// This is the value that is always used for the "logExceptions"
      /// parameter to the various static error handling methods provided
      /// by the <see cref="T:System.Data.SQLite.SQLiteModule" /> class.
      /// </summary>
      private const bool DefaultLogExceptions = true;
      /// <summary>
      /// This is the error message text used when the contained
      /// <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance is not available
      /// for any reason.
      /// </summary>
      private const string ModuleNotAvailableErrorMessage = "native module implementation not available";
      /// <summary>
      /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance used to provide
      /// an implementation of the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
      /// interface.
      /// </summary>
      private SQLiteModule module;
      private bool disposed;

      /// <summary>Constructs an instance of this class.</summary>
      /// <param name="module">
      /// The <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance used to provide
      /// an implementation of the <see cref="T:System.Data.SQLite.ISQLiteNativeModule" />
      /// interface.
      /// </param>
      public SQLiteNativeModule(SQLiteModule module) => this.module = module;

      /// <summary>
      /// Sets the table error message to one that indicates the native
      /// module implementation is not available.
      /// </summary>
      /// <param name="pVtab">
      /// The native pointer to the sqlite3_vtab derived structure.
      /// </param>
      /// <returns>
      /// The value of <see cref="F:System.Data.SQLite.SQLiteErrorCode.Error" />.
      /// </returns>
      private static SQLiteErrorCode ModuleNotAvailableTableError(IntPtr pVtab)
      {
        SQLiteModule.SetTableError((SQLiteModule) null, pVtab, true, true, "native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      /// <summary>
      /// Sets the table error message to one that indicates the native
      /// module implementation is not available.
      /// </summary>
      /// <param name="pCursor">
      /// The native pointer to the sqlite3_vtab_cursor derived
      /// structure.
      /// </param>
      /// <returns>
      /// The value of <see cref="F:System.Data.SQLite.SQLiteErrorCode.Error" />.
      /// </returns>
      private static SQLiteErrorCode ModuleNotAvailableCursorError(IntPtr pCursor)
      {
        SQLiteModule.SetCursorError((SQLiteModule) null, pCursor, true, true, "native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </summary>
      /// <param name="pDb">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pAux">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="argc">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="argv">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pError">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCreate(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </returns>
      public SQLiteErrorCode xCreate(
        IntPtr pDb,
        IntPtr pAux,
        int argc,
        IntPtr argv,
        ref IntPtr pVtab,
        ref IntPtr pError)
      {
        if (this.module != null)
          return this.module.xCreate(pDb, pAux, argc, argv, ref pVtab, ref pError);
        pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </summary>
      /// <param name="pDb">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pAux">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="argc">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="argv">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pError">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xConnect(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
      /// </returns>
      public SQLiteErrorCode xConnect(
        IntPtr pDb,
        IntPtr pAux,
        int argc,
        IntPtr argv,
        ref IntPtr pVtab,
        ref IntPtr pError)
      {
        if (this.module != null)
          return this.module.xConnect(pDb, pAux, argc, argv, ref pVtab, ref pError);
        pError = SQLiteString.Utf8IntPtrFromString("native module implementation not available");
        return SQLiteErrorCode.Error;
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
      /// </param>
      /// <param name="pIndex">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBestIndex(System.IntPtr,System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xBestIndex(pVtab, pIndex);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xDisconnect(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xDisconnect(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xDestroy(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xDestroy(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xOpen(System.IntPtr,System.IntPtr@)" /> method.
      /// </returns>
      public SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xOpen(pVtab, ref pCursor);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xClose(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xClose(IntPtr pCursor) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xClose(pCursor);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </param>
      /// <param name="idxNum">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </param>
      /// <param name="idxStr">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </param>
      /// <param name="argc">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </param>
      /// <param name="argv">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFilter(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xFilter(
        IntPtr pCursor,
        int idxNum,
        IntPtr idxStr,
        int argc,
        IntPtr argv)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xFilter(pCursor, idxNum, idxStr, argc, argv);
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xNext(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xNext(IntPtr pCursor) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xNext(pCursor);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xEof(System.IntPtr)" /> method.
      /// </returns>
      public int xEof(IntPtr pCursor)
      {
        if (this.module != null)
          return this.module.xEof(pCursor);
        int num = (int) SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor);
        return 1;
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <param name="pContext">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <param name="index">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xColumn(System.IntPtr,System.IntPtr,System.Int32)" /> method.
      /// </returns>
      public SQLiteErrorCode xColumn(IntPtr pCursor, IntPtr pContext, int index) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xColumn(pCursor, pContext, index);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
      /// </summary>
      /// <param name="pCursor">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <param name="rowId">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRowId(System.IntPtr,System.Int64@)" /> method.
      /// </returns>
      public SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableCursorError(pCursor) : this.module.xRowId(pCursor, ref rowId);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <param name="argc">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <param name="argv">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <param name="rowId">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xUpdate(System.IntPtr,System.Int32,System.IntPtr,System.Int64@)" /> method.
      /// </returns>
      public SQLiteErrorCode xUpdate(
        IntPtr pVtab,
        int argc,
        IntPtr argv,
        ref long rowId)
      {
        return this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xUpdate(pVtab, argc, argv, ref rowId);
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xBegin(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xBegin(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xBegin(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xSync(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xSync(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xCommit(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xCommit(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xRollback(IntPtr pVtab) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRollback(pVtab);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="nArg">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="zName">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="callback">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </param>
      /// <param name="pClientData">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xFindFunction(System.IntPtr,System.Int32,System.IntPtr,System.Data.SQLite.SQLiteCallback@,System.IntPtr@)" /> method.
      /// </returns>
      public int xFindFunction(
        IntPtr pVtab,
        int nArg,
        IntPtr zName,
        ref SQLiteCallback callback,
        ref IntPtr pClientData)
      {
        if (this.module != null)
          return this.module.xFindFunction(pVtab, nArg, zName, ref callback, ref pClientData);
        int num = (int) SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab);
        return 0;
      }

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
      /// </param>
      /// <param name="zNew">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRename(System.IntPtr,System.IntPtr)" /> method.
      /// </returns>
      public SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRename(pVtab, zNew);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <param name="iSavepoint">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSavepoint(System.IntPtr,System.Int32)" /> method.
      /// </returns>
      public SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xSavepoint(pVtab, iSavepoint);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <param name="iSavepoint">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRelease(System.IntPtr,System.Int32)" /> method.
      /// </returns>
      public SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRelease(pVtab, iSavepoint);

      /// <summary>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
      /// </summary>
      /// <param name="pVtab">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <param name="iSavepoint">
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
      /// </param>
      /// <returns>
      /// See the <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollbackTo(System.IntPtr,System.Int32)" /> method.
      /// </returns>
      public SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint) => this.module == null ? SQLiteModule.SQLiteNativeModule.ModuleNotAvailableTableError(pVtab) : this.module.xRollbackTo(pVtab, iSavepoint);

      /// <summary>Disposes of this object instance.</summary>
      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      /// <summary>
      /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
      /// instance has been disposed.
      /// </summary>
      private void CheckDisposed()
      {
        if (this.disposed)
          throw new ObjectDisposedException(typeof (SQLiteModule.SQLiteNativeModule).Name);
      }

      /// <summary>Disposes of this object instance.</summary>
      /// <param name="disposing">
      /// Non-zero if this method is being called from the
      /// <see cref="M:System.Data.SQLite.SQLiteModule.SQLiteNativeModule.Dispose" /> method.  Zero if this method is being
      /// called from the finalizer.
      /// </param>
      private void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        if (this.module != null)
          this.module = (SQLiteModule) null;
        this.disposed = true;
      }

      /// <summary>Finalizes this object instance.</summary>
      ~SQLiteNativeModule() => this.Dispose(false);
    }
  }
}
