// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleEnumerable
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class implements a virtual table module that exposes an
  /// <see cref="T:System.Collections.IEnumerable" /> object instance as a read-only virtual
  /// table.  It is not sealed and may be used as the base class for any
  /// user-defined virtual table class that wraps an
  /// <see cref="T:System.Collections.IEnumerable" /> object instance.  The following short
  /// example shows it being used to treat an array of strings as a table
  /// data source:
  /// <code>
  ///   public static class Sample
  ///   {
  ///     public static void Main()
  ///     {
  ///       using (SQLiteConnection connection = new SQLiteConnection(
  ///           "Data Source=:memory:;"))
  ///       {
  ///         connection.Open();
  /// 
  ///         connection.CreateModule(new SQLiteModuleEnumerable(
  ///           "sampleModule", new string[] { "one", "two", "three" }));
  /// 
  ///         using (SQLiteCommand command = connection.CreateCommand())
  ///         {
  ///           command.CommandText =
  ///               "CREATE VIRTUAL TABLE t1 USING sampleModule;";
  /// 
  ///           command.ExecuteNonQuery();
  ///         }
  /// 
  ///         using (SQLiteCommand command = connection.CreateCommand())
  ///         {
  ///           command.CommandText = "SELECT * FROM t1;";
  /// 
  ///           using (SQLiteDataReader dataReader = command.ExecuteReader())
  ///           {
  ///             while (dataReader.Read())
  ///               Console.WriteLine(dataReader[0].ToString());
  ///           }
  ///         }
  /// 
  ///         connection.Close();
  ///       }
  ///     }
  ///   }
  /// </code>
  /// </summary>
  public class SQLiteModuleEnumerable : SQLiteModuleCommon
  {
    /// <summary>
    /// The <see cref="T:System.Collections.IEnumerable" /> instance containing the backing data
    /// for the virtual table.
    /// </summary>
    private IEnumerable enumerable;
    /// <summary>
    /// Non-zero if different object instances with the same value should
    /// generate different row identifiers, where applicable.  This has no
    /// effect on the .NET Compact Framework.
    /// </summary>
    private bool objectIdentity;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    /// <param name="enumerable">
    /// The <see cref="T:System.Collections.IEnumerable" /> instance to expose as a virtual
    /// table.  This parameter cannot be null.
    /// </param>
    public SQLiteModuleEnumerable(string name, IEnumerable enumerable)
      : this(name, enumerable, false)
    {
    }

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    /// <param name="enumerable">
    /// The <see cref="T:System.Collections.IEnumerable" /> instance to expose as a virtual
    /// table.  This parameter cannot be null.
    /// </param>
    /// <param name="objectIdentity">
    /// Non-zero if different object instances with the same value should
    /// generate different row identifiers, where applicable.  This
    /// parameter has no effect on the .NET Compact Framework.
    /// </param>
    public SQLiteModuleEnumerable(string name, IEnumerable enumerable, bool objectIdentity)
      : base(name)
    {
      this.enumerable = enumerable != null ? enumerable : throw new ArgumentNullException(nameof (enumerable));
      this.objectIdentity = objectIdentity;
    }

    /// <summary>
    /// Sets the table error message to one that indicates the virtual
    /// table cursor has no current row.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance.
    /// </param>
    /// <returns>
    /// The value of <see cref="F:System.Data.SQLite.SQLiteErrorCode.Error" />.
    /// </returns>
    protected virtual SQLiteErrorCode CursorEndOfEnumeratorError(
      SQLiteVirtualTableCursor cursor)
    {
      this.SetCursorError(cursor, "already hit end of enumerator");
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </summary>
    /// <param name="connection">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="pClientData">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="arguments">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="error">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </returns>
    public override SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) != SQLiteErrorCode.Ok)
        return SQLiteErrorCode.Error;
      table = new SQLiteVirtualTable(arguments);
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </summary>
    /// <param name="connection">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="pClientData">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="arguments">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <param name="error">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> method.
    /// </returns>
    public override SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) != SQLiteErrorCode.Ok)
        return SQLiteErrorCode.Error;
      table = new SQLiteVirtualTable(arguments);
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
    /// </param>
    /// <param name="index">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
    /// </returns>
    public override SQLiteErrorCode BestIndex(
      SQLiteVirtualTable table,
      SQLiteIndex index)
    {
      this.CheckDisposed();
      if (table.BestIndex(index))
        return SQLiteErrorCode.Ok;
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "failed to select best index for virtual table \"{0}\"", (object) table.TableName));
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Disconnect(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Disconnect(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Disconnect(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Disconnect(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      table.Dispose();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Destroy(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Destroy(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Destroy(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Destroy(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      table.Dispose();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </param>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </returns>
    public override SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      cursor = (SQLiteVirtualTableCursor) new SQLiteVirtualTableCursorEnumerator(table, this.enumerable.GetEnumerator());
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Close(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Close(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Close(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </returns>
    public override SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      cursorEnumerator.Close();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </param>
    /// <param name="indexNumber">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </param>
    /// <param name="indexString">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </param>
    /// <param name="values">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </returns>
    public override SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      cursorEnumerator.Filter(indexNumber, indexString, values);
      cursorEnumerator.Reset();
      cursorEnumerator.MoveNext();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Next(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Next(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Next(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </returns>
    public override SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      cursorEnumerator.MoveNext();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Eof(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Eof(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Eof(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </returns>
    public override bool Eof(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return !(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator) ? this.ResultCodeToEofResult(this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator))) : cursorEnumerator.EndOfEnumerator;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <param name="context">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <param name="index">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </returns>
    public override SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      object current = cursorEnumerator.Current;
      if (current != null)
        context.SetString(this.GetStringFromObject(cursor, current));
      else
        context.SetNull();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RowId(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int64@)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RowId(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int64@)" /> method.
    /// </param>
    /// <param name="rowId">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RowId(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int64@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RowId(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int64@)" /> method.
    /// </returns>
    public override SQLiteErrorCode RowId(
      SQLiteVirtualTableCursor cursor,
      ref long rowId)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      object current = cursorEnumerator.Current;
      rowId = this.GetRowIdFromObject(cursor, current);
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Update(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteValue[],System.Int64@)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Update(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteValue[],System.Int64@)" /> method.
    /// </param>
    /// <param name="values">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Update(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteValue[],System.Int64@)" /> method.
    /// </param>
    /// <param name="rowId">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Update(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteValue[],System.Int64@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Update(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteValue[],System.Int64@)" /> method.
    /// </returns>
    public override SQLiteErrorCode Update(
      SQLiteVirtualTable table,
      SQLiteValue[] values,
      ref long rowId)
    {
      this.CheckDisposed();
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "virtual table \"{0}\" is read-only", (object) table.TableName));
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rename(System.Data.SQLite.SQLiteVirtualTable,System.String)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rename(System.Data.SQLite.SQLiteVirtualTable,System.String)" /> method.
    /// </param>
    /// <param name="newName">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rename(System.Data.SQLite.SQLiteVirtualTable,System.String)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rename(System.Data.SQLite.SQLiteVirtualTable,System.String)" /> method.
    /// </returns>
    public override SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName)
    {
      this.CheckDisposed();
      if (table.Rename(newName))
        return SQLiteErrorCode.Ok;
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "failed to rename virtual table from \"{0}\" to \"{1}\"", (object) table.TableName, (object) newName));
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleEnumerable).Name);
    }

    /// <summary>Disposes of this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this method is being called from the
    /// <see cref="M:System.IDisposable.Dispose" /> method.  Zero if this method is
    /// being called from the finalizer.
    /// </param>
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
  }
}
