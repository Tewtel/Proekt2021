// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleNoop
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class implements a virtual table module that does nothing by
  /// providing "empty" implementations for all of the
  /// <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface methods.  The result
  /// codes returned by these "empty" method implementations may be
  /// controlled on a per-method basis by using and/or overriding the
  /// <see cref="M:System.Data.SQLite.SQLiteModuleNoop.GetDefaultResultCode" />,
  /// <see cref="M:System.Data.SQLite.SQLiteModuleNoop.ResultCodeToEofResult(System.Data.SQLite.SQLiteErrorCode)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteModuleNoop.ResultCodeToFindFunctionResult(System.Data.SQLite.SQLiteErrorCode)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteModuleNoop.GetMethodResultCode(System.String)" />, and
  /// <see cref="M:System.Data.SQLite.SQLiteModuleNoop.SetMethodResultCode(System.String,System.Data.SQLite.SQLiteErrorCode)" /> methods from within derived classes.
  /// </summary>
  public class SQLiteModuleNoop : SQLiteModule
  {
    /// <summary>
    /// This field is used to store the <see cref="T:System.Data.SQLite.SQLiteErrorCode" />
    /// values to return, on a per-method basis, for all methods that are
    /// part of the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface.
    /// </summary>
    private Dictionary<string, SQLiteErrorCode> resultCodes;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    public SQLiteModuleNoop(string name)
      : base(name)
    {
      this.resultCodes = new Dictionary<string, SQLiteErrorCode>();
    }

    /// <summary>
    /// Determines the default <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value to be
    /// returned by methods of the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" />
    /// interface that lack an overridden implementation in all classes
    /// derived from the <see cref="T:System.Data.SQLite.SQLiteModuleNoop" /> class.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value that should be returned
    /// by all <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface methods unless
    /// a more specific result code has been set for that interface method.
    /// </returns>
    protected virtual SQLiteErrorCode GetDefaultResultCode() => SQLiteErrorCode.Ok;

    /// <summary>
    /// Converts a <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value into a boolean
    /// return value for use with the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Eof(System.Data.SQLite.SQLiteVirtualTableCursor)" /> method.
    /// </summary>
    /// <param name="resultCode">
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value to convert.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Boolean" /> value.
    /// </returns>
    protected virtual bool ResultCodeToEofResult(SQLiteErrorCode resultCode) => resultCode != SQLiteErrorCode.Ok;

    /// <summary>
    /// Converts a <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value into a boolean
    /// return value for use with the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="resultCode">
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value to convert.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Boolean" /> value.
    /// </returns>
    protected virtual bool ResultCodeToFindFunctionResult(SQLiteErrorCode resultCode) => resultCode == SQLiteErrorCode.Ok;

    /// <summary>
    /// Determines the <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value that should be
    /// returned by the specified <see cref="T:System.Data.SQLite.ISQLiteManagedModule" />
    /// interface method if it lack an overridden implementation.  If no
    /// specific <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value is available (or set)
    /// for the specified method, the <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value
    /// returned by the <see cref="M:System.Data.SQLite.SQLiteModuleNoop.GetDefaultResultCode" /> method will be
    /// returned instead.
    /// </summary>
    /// <param name="methodName">
    /// The name of the method.  Currently, this method must be part of
    /// the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value that should be returned
    /// by the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface method.
    /// </returns>
    protected virtual SQLiteErrorCode GetMethodResultCode(string methodName)
    {
      SQLiteErrorCode sqLiteErrorCode;
      return methodName == null || this.resultCodes == null || (this.resultCodes == null || !this.resultCodes.TryGetValue(methodName, out sqLiteErrorCode)) ? this.GetDefaultResultCode() : sqLiteErrorCode;
    }

    /// <summary>
    /// Sets the <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value that should be
    /// returned by the specified <see cref="T:System.Data.SQLite.ISQLiteManagedModule" />
    /// interface method if it lack an overridden implementation.
    /// </summary>
    /// <param name="methodName">
    /// The name of the method.  Currently, this method must be part of
    /// the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface.
    /// </param>
    /// <param name="resultCode">
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value that should be returned
    /// by the <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface method.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    protected virtual bool SetMethodResultCode(string methodName, SQLiteErrorCode resultCode)
    {
      if (methodName == null || this.resultCodes == null)
        return false;
      this.resultCodes[methodName] = resultCode;
      return true;
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
      return this.GetMethodResultCode(nameof (Create));
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
      return this.GetMethodResultCode(nameof (Connect));
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
      return this.GetMethodResultCode(nameof (BestIndex));
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
      return this.GetMethodResultCode(nameof (Disconnect));
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
      return this.GetMethodResultCode(nameof (Destroy));
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
      return this.GetMethodResultCode(nameof (Open));
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
      return this.GetMethodResultCode(nameof (Close));
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
      return this.GetMethodResultCode(nameof (Filter));
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
      return this.GetMethodResultCode(nameof (Next));
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
      return this.ResultCodeToEofResult(this.GetMethodResultCode(nameof (Eof)));
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
      return this.GetMethodResultCode(nameof (Column));
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
      return this.GetMethodResultCode(nameof (RowId));
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
      return this.GetMethodResultCode(nameof (Update));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Begin(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Begin(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Begin(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Begin(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Begin));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Sync(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Sync(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Sync(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Sync(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Sync));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Commit(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Commit(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Commit(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Commit(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Commit));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rollback(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rollback(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Rollback(System.Data.SQLite.SQLiteVirtualTable)" /> method.
    /// </returns>
    public override SQLiteErrorCode Rollback(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Rollback));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="argumentCount">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="name">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="function">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </param>
    /// <param name="pClientData">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.FindFunction(System.Data.SQLite.SQLiteVirtualTable,System.Int32,System.String,System.Data.SQLite.SQLiteFunction@,System.IntPtr@)" /> method.
    /// </returns>
    public override bool FindFunction(
      SQLiteVirtualTable table,
      int argumentCount,
      string name,
      ref SQLiteFunction function,
      ref IntPtr pClientData)
    {
      this.CheckDisposed();
      return this.ResultCodeToFindFunctionResult(this.GetMethodResultCode(nameof (FindFunction)));
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
      return this.GetMethodResultCode(nameof (Rename));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Savepoint(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Savepoint(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <param name="savepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Savepoint(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Savepoint(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </returns>
    public override SQLiteErrorCode Savepoint(
      SQLiteVirtualTable table,
      int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Savepoint));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Release(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Release(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <param name="savepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Release(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Release(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </returns>
    public override SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Release));
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RollbackTo(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RollbackTo(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <param name="savepoint">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RollbackTo(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.RollbackTo(System.Data.SQLite.SQLiteVirtualTable,System.Int32)" /> method.
    /// </returns>
    public override SQLiteErrorCode RollbackTo(
      SQLiteVirtualTable table,
      int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (RollbackTo));
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleNoop).Name);
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
