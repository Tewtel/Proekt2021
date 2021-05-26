// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteManagedModule
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface represents a virtual table implementation written in
  /// managed code.
  /// </summary>
  public interface ISQLiteManagedModule
  {
    /// <summary>
    /// Returns non-zero if the schema for the virtual table has been
    /// declared.
    /// </summary>
    bool Declared { get; }

    /// <summary>
    /// Returns the name of the module as it was registered with the SQLite
    /// core library.
    /// </summary>
    string Name { get; }

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
    SQLiteErrorCode Create(
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
    SQLiteErrorCode Connect(
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
    SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDisconnect(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    SQLiteErrorCode Disconnect(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xDestroy(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    SQLiteErrorCode Destroy(SQLiteVirtualTable table);

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
    SQLiteErrorCode Open(
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
    SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor);

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
    SQLiteErrorCode Filter(
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
    SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor);

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
    bool Eof(SQLiteVirtualTableCursor cursor);

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
    SQLiteErrorCode Column(
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
    SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId);

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
    SQLiteErrorCode Update(
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
    SQLiteErrorCode Begin(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xSync(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    SQLiteErrorCode Sync(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xCommit(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    SQLiteErrorCode Commit(SQLiteVirtualTable table);

    /// <summary>
    /// This method is called in response to the
    /// <see cref="M:System.Data.SQLite.ISQLiteNativeModule.xRollback(System.IntPtr)" /> method.
    /// </summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this virtual table.
    /// </param>
    /// <returns>A standard SQLite return code.</returns>
    SQLiteErrorCode Rollback(SQLiteVirtualTable table);

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
    bool FindFunction(
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
    SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName);

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
    SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint);

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
    SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint);

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
    SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint);
  }
}
