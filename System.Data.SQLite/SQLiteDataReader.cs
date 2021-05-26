// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDataReader
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbDataReader.</summary>
  public sealed class SQLiteDataReader : DbDataReader
  {
    /// <summary>Underlying command this reader is attached to</summary>
    private SQLiteCommand _command;
    /// <summary>
    /// The flags pertaining to the associated connection (via the command).
    /// </summary>
    private SQLiteConnectionFlags _flags;
    /// <summary>
    /// Index of the current statement in the command being processed
    /// </summary>
    private int _activeStatementIndex;
    /// <summary>Current statement being Read()</summary>
    private SQLiteStatement _activeStatement;
    /// <summary>
    /// State of the current statement being processed.
    /// -1 = First Step() executed, so the first Read() will be ignored
    ///  0 = Actively reading
    ///  1 = Finished reading
    ///  2 = Non-row-returning statement, no records
    /// </summary>
    private int _readingState;
    /// <summary>
    /// Number of records affected by the insert/update statements executed on the command
    /// </summary>
    private int _rowsAffected;
    /// <summary>
    /// Count of fields (columns) in the row-returning statement currently being processed
    /// </summary>
    private int _fieldCount;
    /// <summary>
    /// The number of calls to Step() that have returned true (i.e. the number of rows that
    /// have been read in the current result set).
    /// </summary>
    private int _stepCount;
    /// <summary>
    /// Maps the field (column) names to their corresponding indexes within the results.
    /// </summary>
    private Dictionary<string, int> _fieldIndexes;
    /// <summary>
    /// Datatypes of active fields (columns) in the current statement, used for type-restricting data
    /// </summary>
    private SQLiteType[] _fieldTypeArray;
    /// <summary>The behavior of the datareader</summary>
    private CommandBehavior _commandBehavior;
    /// <summary>
    /// If set, then dispose of the command object when the reader is finished
    /// </summary>
    internal bool _disposeCommand;
    /// <summary>
    /// If set, then raise an exception when the object is accessed after being disposed.
    /// </summary>
    internal bool _throwOnDisposed;
    /// <summary>
    /// An array of rowid's for the active statement if CommandBehavior.KeyInfo is specified
    /// </summary>
    private SQLiteKeyReader _keyInfo;
    /// <summary>Matches the version of the connection.</summary>
    internal int _version;
    /// <summary>
    /// The "stub" (i.e. placeholder) base schema name to use when returning
    /// column schema information.  Matches the base schema name used by the
    /// associated connection.
    /// </summary>
    private string _baseSchemaName;
    private bool disposed;

    /// <summary>
    /// Internal constructor, initializes the datareader and sets up to begin executing statements
    /// </summary>
    /// <param name="cmd">The SQLiteCommand this data reader is for</param>
    /// <param name="behave">The expected behavior of the data reader</param>
    internal SQLiteDataReader(SQLiteCommand cmd, CommandBehavior behave)
    {
      this._throwOnDisposed = true;
      this._command = cmd;
      this._version = this._command.Connection._version;
      this._baseSchemaName = this._command.Connection._baseSchemaName;
      this._commandBehavior = behave;
      this._activeStatementIndex = -1;
      this._rowsAffected = -1;
      this.RefreshFlags();
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.NewDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[1]
      {
        (object) behave
      }));
      if (this._command == null)
        return;
      this.NextResult();
    }

    private void CheckDisposed()
    {
      if (this.disposed && this._throwOnDisposed)
        throw new ObjectDisposedException(typeof (SQLiteDataReader).Name);
    }

    /// <summary>Dispose of all resources used by this datareader.</summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.DisposingDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[9]
      {
        (object) disposing,
        (object) this.disposed,
        (object) this._commandBehavior,
        (object) this._readingState,
        (object) this._rowsAffected,
        (object) this._stepCount,
        (object) this._fieldCount,
        (object) this._disposeCommand,
        (object) this._throwOnDisposed
      }));
      try
      {
        if (this.disposed)
          return;
        this._throwOnDisposed = false;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    internal void Cancel() => this._version = 0;

    /// <summary>
    /// Closes the datareader, potentially closing the connection as well if CommandBehavior.CloseConnection was specified.
    /// </summary>
    public override void Close()
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.ClosingDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[7]
      {
        (object) this._commandBehavior,
        (object) this._readingState,
        (object) this._rowsAffected,
        (object) this._stepCount,
        (object) this._fieldCount,
        (object) this._disposeCommand,
        (object) this._throwOnDisposed
      }));
      try
      {
        if (this._command != null)
        {
          try
          {
            try
            {
              if (this._version != 0)
              {
                try
                {
                  do
                    ;
                  while (this.NextResult());
                }
                catch (SQLiteException ex)
                {
                }
              }
              this._command.ResetDataReader();
            }
            finally
            {
              if ((this._commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.Default && this._command.Connection != null)
                this._command.Connection.Close();
            }
          }
          finally
          {
            if (this._disposeCommand)
              this._command.Dispose();
          }
        }
        this._command = (SQLiteCommand) null;
        this._activeStatement = (SQLiteStatement) null;
        this._fieldIndexes = (Dictionary<string, int>) null;
        this._fieldTypeArray = (SQLiteType[]) null;
      }
      finally
      {
        if (this._keyInfo != null)
        {
          this._keyInfo.Dispose();
          this._keyInfo = (SQLiteKeyReader) null;
        }
      }
    }

    /// <summary>Throw an error if the datareader is closed</summary>
    private void CheckClosed()
    {
      if (!this._throwOnDisposed)
        return;
      if (this._command == null)
        throw new InvalidOperationException("DataReader has been closed");
      if (this._version == 0)
        throw new SQLiteException("Execution was aborted by the user");
      SQLiteConnection connection = this._command.Connection;
      if (connection._version != this._version || connection.State != ConnectionState.Open)
        throw new InvalidOperationException("Connection was closed, statement was terminated");
    }

    /// <summary>Throw an error if a row is not loaded</summary>
    private void CheckValidRow()
    {
      if (this._readingState != 0)
        throw new InvalidOperationException("No current row");
    }

    /// <summary>Enumerator support</summary>
    /// <returns>Returns a DbEnumerator object.</returns>
    public override IEnumerator GetEnumerator()
    {
      this.CheckDisposed();
      return (IEnumerator) new DbEnumerator((IDataReader) this, (this._commandBehavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection);
    }

    /// <summary>Not implemented.  Returns 0</summary>
    public override int Depth
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return 0;
      }
    }

    /// <summary>
    /// Returns the number of columns in the current resultset
    /// </summary>
    public override int FieldCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this._keyInfo == null ? this._fieldCount : this._fieldCount + this._keyInfo.Count;
      }
    }

    /// <summary>
    /// Forces the connection flags cached by this data reader to be refreshed
    /// from the underlying connection.
    /// </summary>
    public void RefreshFlags()
    {
      this.CheckDisposed();
      this._flags = SQLiteCommand.GetFlags(this._command);
    }

    /// <summary>
    /// Returns the number of rows seen so far in the current result set.
    /// </summary>
    public int StepCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this._stepCount;
      }
    }

    private int PrivateVisibleFieldCount => this._fieldCount;

    /// <summary>
    /// Returns the number of visible fields in the current resultset
    /// </summary>
    public override int VisibleFieldCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.PrivateVisibleFieldCount;
      }
    }

    /// <summary>
    /// This method is used to make sure the result set is open and a row is currently available.
    /// </summary>
    private void VerifyForGet()
    {
      this.CheckClosed();
      this.CheckValidRow();
    }

    /// <summary>
    /// SQLite is inherently un-typed.  All datatypes in SQLite are natively strings.  The definition of the columns of a table
    /// and the affinity of returned types are all we have to go on to type-restrict data in the reader.
    /// 
    /// This function attempts to verify that the type of data being requested of a column matches the datatype of the column.  In
    /// the case of columns that are not backed into a table definition, we attempt to match up the affinity of a column (int, double, string or blob)
    /// to a set of known types that closely match that affinity.  It's not an exact science, but its the best we can do.
    /// </summary>
    /// <returns>
    /// This function throws an InvalidTypeCast() exception if the requested type doesn't match the column's definition or affinity.
    /// </returns>
    /// <param name="i">The index of the column to type-check</param>
    /// <param name="typ">The type we want to get out of the column</param>
    private TypeAffinity VerifyType(int i, DbType typ)
    {
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTypeAffinity))
        return TypeAffinity.None;
      TypeAffinity affinity = this.GetSQLiteType(this._flags, i).Affinity;
      switch (affinity)
      {
        case TypeAffinity.Int64:
          if (typ == DbType.Int64 || typ == DbType.Int32 || (typ == DbType.Int16 || typ == DbType.Byte) || (typ == DbType.SByte || typ == DbType.Boolean || (typ == DbType.DateTime || typ == DbType.Double)) || (typ == DbType.Single || typ == DbType.Decimal))
            return affinity;
          break;
        case TypeAffinity.Double:
          if (typ == DbType.Double || typ == DbType.Single || (typ == DbType.Decimal || typ == DbType.DateTime))
            return affinity;
          break;
        case TypeAffinity.Text:
          if (typ == DbType.String || typ == DbType.Guid || (typ == DbType.DateTime || typ == DbType.Decimal))
            return affinity;
          break;
        case TypeAffinity.Blob:
          if (typ == DbType.Guid || typ == DbType.Binary || typ == DbType.String)
            return affinity;
          break;
      }
      throw new InvalidCastException();
    }

    /// <summary>
    /// Invokes the data reader value callback configured for the database
    /// type name associated with the specified column.  If no data reader
    /// value callback is available for the database type name, do nothing.
    /// </summary>
    /// <param name="index">The index of the column being read.</param>
    /// <param name="eventArgs">
    /// The extra event data to pass into the callback.
    /// </param>
    /// <param name="complete">
    /// Non-zero if the default handling for the data reader call should be
    /// skipped.  If this is set to non-zero and the necessary return value
    /// is unavailable or unsuitable, an exception will be thrown.
    /// </param>
    private void InvokeReadValueCallback(
      int index,
      SQLiteReadEventArgs eventArgs,
      out bool complete)
    {
      complete = false;
      SQLiteConnectionFlags flags = this._flags;
      this._flags &= ~SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
      try
      {
        string dataTypeName = this.GetDataTypeName(index);
        if (dataTypeName == null)
          return;
        SQLiteConnection connection = SQLiteDataReader.GetConnection(this);
        SQLiteTypeCallbacks callbacks;
        if (connection == null || !connection.TryGetTypeCallbacks(dataTypeName, out callbacks) || callbacks == null)
          return;
        SQLiteReadValueCallback readValueCallback = callbacks.ReadValueCallback;
        if (readValueCallback == null)
          return;
        object readValueUserData = callbacks.ReadValueUserData;
        readValueCallback((SQLiteConvert) this._activeStatement._sql, this, flags, eventArgs, dataTypeName, index, readValueUserData, out complete);
      }
      finally
      {
        this._flags |= SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
      }
    }

    /// <summary>
    /// Attempts to query the integer identifier for the current row.  This
    /// will not work for tables that were created WITHOUT ROWID -OR- if the
    /// query does not include the "rowid" column or one of its aliases -OR-
    /// if the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> was not created with the
    /// <see cref="F:System.Data.CommandBehavior.KeyInfo" /> flag.
    /// </summary>
    /// <param name="i">The index of the BLOB column.</param>
    /// <returns>
    /// The integer identifier for the current row -OR- null if it could not
    /// be determined.
    /// </returns>
    internal long? GetRowId(int i)
    {
      this.VerifyForGet();
      if (this._keyInfo == null)
        return new long?();
      string databaseName = this.GetDatabaseName(i);
      string tableName = this.GetTableName(i);
      int rowIdIndex = this._keyInfo.GetRowIdIndex(databaseName, tableName);
      return rowIdIndex != -1 ? new long?(this.GetInt64(rowIdIndex)) : this._keyInfo.GetRowId(databaseName, tableName);
    }

    /// <summary>
    /// Retrieves the column as a <see cref="T:System.Data.SQLite.SQLiteBlob" /> object.
    /// This will not work for tables that were created WITHOUT ROWID
    /// -OR- if the query does not include the "rowid" column or one
    /// of its aliases -OR- if the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> was
    /// not created with the <see cref="F:System.Data.CommandBehavior.KeyInfo" />
    /// flag.
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <param name="readOnly">
    /// Non-zero to open the blob object for read-only access.
    /// </param>
    /// <returns>A new <see cref="T:System.Data.SQLite.SQLiteBlob" /> object.</returns>
    public SQLiteBlob GetBlob(int i, bool readOnly)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBlob), (SQLiteReadEventArgs) new SQLiteReadBlobEventArgs(readOnly), liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.BlobValue;
      }
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetBlob(i - this.PrivateVisibleFieldCount, readOnly) : SQLiteBlob.Create(this, i, readOnly);
    }

    /// <summary>Retrieves the column as a boolean value</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>bool</returns>
    public override bool GetBoolean(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBoolean), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.BooleanValue.HasValue)
            throw new SQLiteException("missing boolean return value");
          return liteDataReaderValue.BooleanValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetBoolean(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Boolean);
      return Convert.ToBoolean(this.GetValue(i), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    /// <summary>Retrieves the column as a single byte value</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>byte</returns>
    public override byte GetByte(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetByte), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          byte? byteValue = liteDataReaderValue.ByteValue;
          if (!(byteValue.HasValue ? new int?((int) byteValue.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing byte return value");
          return liteDataReaderValue.ByteValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetByte(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Byte);
      return this._activeStatement._sql.GetByte(this._activeStatement, i);
    }

    /// <summary>Retrieves a column as an array of bytes (blob)</summary>
    /// <param name="i">The index of the column.</param>
    /// <param name="fieldOffset">The zero-based index of where to begin reading the data</param>
    /// <param name="buffer">The buffer to write the bytes into</param>
    /// <param name="bufferoffset">The zero-based index of where to begin writing into the array</param>
    /// <param name="length">The number of bytes to retrieve</param>
    /// <returns>The actual number of bytes written into the array</returns>
    /// <remarks>
    /// To determine the number of bytes in the column, pass a null value for the buffer.  The total length will be returned.
    /// </remarks>
    public override long GetBytes(
      int i,
      long fieldOffset,
      byte[] buffer,
      int bufferoffset,
      int length)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteReadArrayEventArgs readArrayEventArgs = new SQLiteReadArrayEventArgs(fieldOffset, buffer, bufferoffset, length);
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBytes), (SQLiteReadEventArgs) readArrayEventArgs, liteDataReaderValue), out complete);
        if (complete)
        {
          byte[] bytesValue = liteDataReaderValue.BytesValue;
          if (bytesValue == null)
            return -1;
          Array.Copy((Array) bytesValue, readArrayEventArgs.DataOffset, (Array) readArrayEventArgs.ByteBuffer, (long) readArrayEventArgs.BufferOffset, (long) readArrayEventArgs.Length);
          return (long) readArrayEventArgs.Length;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetBytes(i - this.PrivateVisibleFieldCount, fieldOffset, buffer, bufferoffset, length);
      int num = (int) this.VerifyType(i, DbType.Binary);
      return this._activeStatement._sql.GetBytes(this._activeStatement, i, (int) fieldOffset, buffer, bufferoffset, length);
    }

    /// <summary>Returns the column as a single character</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>char</returns>
    public override char GetChar(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetChar), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          char? charValue = liteDataReaderValue.CharValue;
          if (!(charValue.HasValue ? new int?((int) charValue.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing character return value");
          return liteDataReaderValue.CharValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetChar(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.SByte);
      return this._activeStatement._sql.GetChar(this._activeStatement, i);
    }

    /// <summary>Retrieves a column as an array of chars (blob)</summary>
    /// <param name="i">The index of the column.</param>
    /// <param name="fieldoffset">The zero-based index of where to begin reading the data</param>
    /// <param name="buffer">The buffer to write the characters into</param>
    /// <param name="bufferoffset">The zero-based index of where to begin writing into the array</param>
    /// <param name="length">The number of bytes to retrieve</param>
    /// <returns>The actual number of characters written into the array</returns>
    /// <remarks>
    /// To determine the number of characters in the column, pass a null value for the buffer.  The total length will be returned.
    /// </remarks>
    public override long GetChars(
      int i,
      long fieldoffset,
      char[] buffer,
      int bufferoffset,
      int length)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteReadArrayEventArgs readArrayEventArgs = new SQLiteReadArrayEventArgs(fieldoffset, buffer, bufferoffset, length);
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetChars), (SQLiteReadEventArgs) readArrayEventArgs, liteDataReaderValue), out complete);
        if (complete)
        {
          char[] charsValue = liteDataReaderValue.CharsValue;
          if (charsValue == null)
            return -1;
          Array.Copy((Array) charsValue, readArrayEventArgs.DataOffset, (Array) readArrayEventArgs.CharBuffer, (long) readArrayEventArgs.BufferOffset, (long) readArrayEventArgs.Length);
          return (long) readArrayEventArgs.Length;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetChars(i - this.PrivateVisibleFieldCount, fieldoffset, buffer, bufferoffset, length);
      if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTextAffinity))
      {
        int num = (int) this.VerifyType(i, DbType.String);
      }
      return this._activeStatement._sql.GetChars(this._activeStatement, i, (int) fieldoffset, buffer, bufferoffset, length);
    }

    /// <summary>
    /// Retrieves the name of the back-end datatype of the column
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public override string GetDataTypeName(int i)
    {
      this.CheckDisposed();
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDataTypeName(i - this.PrivateVisibleFieldCount);
      TypeAffinity nAffinity = TypeAffinity.Uninitialized;
      return this._activeStatement._sql.ColumnType(this._activeStatement, i, ref nAffinity);
    }

    /// <summary>Retrieve the column as a date/time value</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>DateTime</returns>
    public override DateTime GetDateTime(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDateTime), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DateTimeValue.HasValue)
            throw new SQLiteException("missing date/time return value");
          return liteDataReaderValue.DateTimeValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDateTime(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.DateTime);
      return this._activeStatement._sql.GetDateTime(this._activeStatement, i);
    }

    /// <summary>Retrieve the column as a decimal value</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>decimal</returns>
    public override Decimal GetDecimal(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDecimal), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DecimalValue.HasValue)
            throw new SQLiteException("missing decimal return value");
          return liteDataReaderValue.DecimalValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDecimal(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Decimal);
      CultureInfo cultureInfo = CultureInfo.CurrentCulture;
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.GetInvariantDecimal))
        cultureInfo = CultureInfo.InvariantCulture;
      return Decimal.Parse(this._activeStatement._sql.GetText(this._activeStatement, i), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, (IFormatProvider) cultureInfo);
    }

    /// <summary>Returns the column as a double</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>double</returns>
    public override double GetDouble(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDouble), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DoubleValue.HasValue)
            throw new SQLiteException("missing double return value");
          return liteDataReaderValue.DoubleValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDouble(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Double);
      return this._activeStatement._sql.GetDouble(this._activeStatement, i);
    }

    /// <summary>
    /// Determines and returns the <see cref="T:System.Data.SQLite.TypeAffinity" /> of the
    /// specified column.
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.TypeAffinity" /> associated with the specified
    /// column, if any.
    /// </returns>
    public TypeAffinity GetFieldAffinity(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetFieldAffinity(i - this.PrivateVisibleFieldCount) : this.GetSQLiteType(this._flags, i).Affinity;
    }

    /// <summary>Returns the .NET type of a given column</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>Type</returns>
    public override Type GetFieldType(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetFieldType(i - this.PrivateVisibleFieldCount) : SQLiteConvert.SQLiteTypeToType(this.GetSQLiteType(this._flags, i));
    }

    /// <summary>Returns a column as a float value</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>float</returns>
    public override float GetFloat(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetFloat), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.FloatValue.HasValue)
            throw new SQLiteException("missing float return value");
          return liteDataReaderValue.FloatValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetFloat(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Single);
      return Convert.ToSingle(this._activeStatement._sql.GetDouble(this._activeStatement, i));
    }

    /// <summary>Returns the column as a Guid</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>Guid</returns>
    public override Guid GetGuid(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetGuid), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.GuidValue.HasValue)
            throw new SQLiteException("missing guid return value");
          return liteDataReaderValue.GuidValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetGuid(i - this.PrivateVisibleFieldCount);
      if (this.VerifyType(i, DbType.Guid) != TypeAffinity.Blob)
        return new Guid(this._activeStatement._sql.GetText(this._activeStatement, i));
      byte[] numArray = new byte[16];
      this._activeStatement._sql.GetBytes(this._activeStatement, i, 0, numArray, 0, 16);
      return new Guid(numArray);
    }

    /// <summary>Returns the column as a short</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>Int16</returns>
    public override short GetInt16(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt16), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          short? int16Value = liteDataReaderValue.Int16Value;
          if (!(int16Value.HasValue ? new int?((int) int16Value.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing int16 return value");
          return liteDataReaderValue.Int16Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt16(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int16);
      return this._activeStatement._sql.GetInt16(this._activeStatement, i);
    }

    /// <summary>Retrieves the column as an int</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>Int32</returns>
    public override int GetInt32(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt32), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.Int32Value.HasValue)
            throw new SQLiteException("missing int32 return value");
          return liteDataReaderValue.Int32Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt32(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int32);
      return this._activeStatement._sql.GetInt32(this._activeStatement, i);
    }

    /// <summary>Retrieves the column as a long</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>Int64</returns>
    public override long GetInt64(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt64), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.Int64Value.HasValue)
            throw new SQLiteException("missing int64 return value");
          return liteDataReaderValue.Int64Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt64(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int64);
      return this._activeStatement._sql.GetInt64(this._activeStatement, i);
    }

    /// <summary>Retrieves the name of the column</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public override string GetName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnName(this._activeStatement, i);
    }

    /// <summary>
    /// Returns the name of the database associated with the specified column.
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public string GetDatabaseName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetDatabaseName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnDatabaseName(this._activeStatement, i);
    }

    /// <summary>
    /// Returns the name of the table associated with the specified column.
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public string GetTableName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetTableName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnTableName(this._activeStatement, i);
    }

    /// <summary>Returns the original name of the specified column.</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public string GetOriginalName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnOriginalName(this._activeStatement, i);
    }

    /// <summary>Retrieves the i of a column, given its name</summary>
    /// <param name="name">The name of the column to retrieve</param>
    /// <returns>The int i of the column</returns>
    public override int GetOrdinal(string name)
    {
      this.CheckDisposed();
      int num1 = this._throwOnDisposed ? 1 : 0;
      if (this._fieldIndexes == null)
        this._fieldIndexes = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      int num2;
      if (!this._fieldIndexes.TryGetValue(name, out num2))
      {
        num2 = this._activeStatement._sql.ColumnIndex(this._activeStatement, name);
        if (num2 == -1 && this._keyInfo != null)
        {
          num2 = this._keyInfo.GetOrdinal(name);
          if (num2 > -1)
            num2 += this.PrivateVisibleFieldCount;
        }
        this._fieldIndexes.Add(name, num2);
      }
      return num2 != -1 || !HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StrictConformance) ? num2 : throw new IndexOutOfRangeException();
    }

    /// <summary>
    /// Schema information in SQLite is difficult to map into .NET conventions, so a lot of work must be done
    /// to gather the necessary information so it can be represented in an ADO.NET manner.
    /// </summary>
    /// <returns>Returns a DataTable containing the schema information for the active SELECT statement being processed.</returns>
    public override DataTable GetSchemaTable()
    {
      this.CheckDisposed();
      return this.GetSchemaTable(true, false);
    }

    private static void GetStatementColumnParents(
      SQLiteBase sql,
      SQLiteStatement stmt,
      int fieldCount,
      ref Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns,
      ref Dictionary<int, SQLiteDataReader.ColumnParent> columnToParent)
    {
      if (parentToColumns == null)
        parentToColumns = new Dictionary<SQLiteDataReader.ColumnParent, List<int>>((IEqualityComparer<SQLiteDataReader.ColumnParent>) new SQLiteDataReader.ColumnParent());
      if (columnToParent == null)
        columnToParent = new Dictionary<int, SQLiteDataReader.ColumnParent>();
      for (int index = 0; index < fieldCount; ++index)
      {
        string databaseName = sql.ColumnDatabaseName(stmt, index);
        string tableName = sql.ColumnTableName(stmt, index);
        string columnName = sql.ColumnOriginalName(stmt, index);
        SQLiteDataReader.ColumnParent key = new SQLiteDataReader.ColumnParent(databaseName, tableName, (string) null);
        SQLiteDataReader.ColumnParent columnParent = new SQLiteDataReader.ColumnParent(databaseName, tableName, columnName);
        List<int> intList;
        if (!parentToColumns.TryGetValue(key, out intList))
          parentToColumns.Add(key, new List<int>((IEnumerable<int>) new int[1]
          {
            index
          }));
        else if (intList != null)
          intList.Add(index);
        else
          parentToColumns[key] = new List<int>((IEnumerable<int>) new int[1]
          {
            index
          });
        columnToParent.Add(index, columnParent);
      }
    }

    private static int CountParents(
      Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns)
    {
      int num = 0;
      if (parentToColumns != null)
      {
        foreach (SQLiteDataReader.ColumnParent key in parentToColumns.Keys)
        {
          if (key != null && !string.IsNullOrEmpty(key.TableName))
            ++num;
        }
      }
      return num;
    }

    internal DataTable GetSchemaTable(bool wantUniqueInfo, bool wantDefaultValue)
    {
      this.CheckClosed();
      int num = this._throwOnDisposed ? 1 : 0;
      Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns = (Dictionary<SQLiteDataReader.ColumnParent, List<int>>) null;
      Dictionary<int, SQLiteDataReader.ColumnParent> columnToParent = (Dictionary<int, SQLiteDataReader.ColumnParent>) null;
      SQLiteBase sql = this._command.Connection._sql;
      SQLiteDataReader.GetStatementColumnParents(sql, this._activeStatement, this._fieldCount, ref parentToColumns, ref columnToParent);
      DataTable tbl = new DataTable("SchemaTable");
      DataTable dataTable = (DataTable) null;
      string str1 = string.Empty;
      string str2 = string.Empty;
      string empty1 = string.Empty;
      tbl.Locale = CultureInfo.InvariantCulture;
      tbl.Columns.Add(SchemaTableColumn.ColumnName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.ColumnSize, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.NumericPrecision, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.NumericScale, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.IsUnique, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsKey, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof (string));
      tbl.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseColumnName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseTableName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.DataType, typeof (Type));
      tbl.Columns.Add(SchemaTableColumn.AllowDBNull, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.ProviderType, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.IsAliased, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsExpression, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsLong, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof (Type));
      tbl.Columns.Add(SchemaTableOptionalColumn.DefaultValue, typeof (object));
      tbl.Columns.Add("DataTypeName", typeof (string));
      tbl.Columns.Add("CollationType", typeof (string));
      tbl.BeginLoadData();
      for (int index = 0; index < this._fieldCount; ++index)
      {
        SQLiteType sqLiteType = this.GetSQLiteType(this._flags, index);
        DataRow row1 = tbl.NewRow();
        DbType type = sqLiteType.Type;
        row1[SchemaTableColumn.ColumnName] = (object) this.GetName(index);
        row1[SchemaTableColumn.ColumnOrdinal] = (object) index;
        row1[SchemaTableColumn.ColumnSize] = (object) SQLiteConvert.DbTypeToColumnSize(type);
        row1[SchemaTableColumn.NumericPrecision] = SQLiteConvert.DbTypeToNumericPrecision(type);
        row1[SchemaTableColumn.NumericScale] = SQLiteConvert.DbTypeToNumericScale(type);
        row1[SchemaTableColumn.ProviderType] = (object) sqLiteType.Type;
        row1[SchemaTableColumn.IsLong] = (object) false;
        row1[SchemaTableColumn.AllowDBNull] = (object) true;
        row1[SchemaTableOptionalColumn.IsReadOnly] = (object) false;
        row1[SchemaTableOptionalColumn.IsRowVersion] = (object) false;
        row1[SchemaTableColumn.IsUnique] = (object) false;
        row1[SchemaTableColumn.IsKey] = (object) false;
        row1[SchemaTableOptionalColumn.IsAutoIncrement] = (object) false;
        row1[SchemaTableColumn.DataType] = (object) this.GetFieldType(index);
        row1[SchemaTableOptionalColumn.IsHidden] = (object) false;
        row1[SchemaTableColumn.BaseSchemaName] = (object) this._baseSchemaName;
        string columnName = columnToParent[index].ColumnName;
        if (!string.IsNullOrEmpty(columnName))
          row1[SchemaTableColumn.BaseColumnName] = (object) columnName;
        row1[SchemaTableColumn.IsExpression] = (object) string.IsNullOrEmpty(columnName);
        row1[SchemaTableColumn.IsAliased] = (object) (string.Compare(this.GetName(index), columnName, StringComparison.OrdinalIgnoreCase) != 0);
        string tableName = columnToParent[index].TableName;
        if (!string.IsNullOrEmpty(tableName))
          row1[SchemaTableColumn.BaseTableName] = (object) tableName;
        string databaseName = columnToParent[index].DatabaseName;
        if (!string.IsNullOrEmpty(databaseName))
          row1[SchemaTableOptionalColumn.BaseCatalogName] = (object) databaseName;
        string dataType = (string) null;
        if (!string.IsNullOrEmpty(columnName))
        {
          string empty2 = string.Empty;
          if (row1[SchemaTableOptionalColumn.BaseCatalogName] != DBNull.Value)
            empty2 = (string) row1[SchemaTableOptionalColumn.BaseCatalogName];
          string empty3 = string.Empty;
          if (row1[SchemaTableColumn.BaseTableName] != DBNull.Value)
            empty3 = (string) row1[SchemaTableColumn.BaseTableName];
          if (sql.DoesTableExist(empty2, empty3))
          {
            string empty4 = string.Empty;
            if (row1[SchemaTableColumn.BaseColumnName] != DBNull.Value)
              empty4 = (string) row1[SchemaTableColumn.BaseColumnName];
            string collateSequence = (string) null;
            bool notNull = false;
            bool primaryKey = false;
            bool autoIncrement = false;
            this._command.Connection._sql.ColumnMetaData(empty2, empty3, columnName, true, ref dataType, ref collateSequence, ref notNull, ref primaryKey, ref autoIncrement);
            if (notNull || primaryKey)
              row1[SchemaTableColumn.AllowDBNull] = (object) false;
            bool flag = (bool) row1[SchemaTableColumn.AllowDBNull];
            row1[SchemaTableColumn.IsKey] = (object) (bool) (!primaryKey ? 0 : (SQLiteDataReader.CountParents(parentToColumns) <= 1 ? 1 : 0));
            row1[SchemaTableOptionalColumn.IsAutoIncrement] = (object) autoIncrement;
            row1["CollationType"] = (object) collateSequence;
            string[] strArray1 = dataType.Split('(');
            if (strArray1.Length > 1)
            {
              dataType = strArray1[0];
              string[] strArray2 = strArray1[1].Split(')');
              if (strArray2.Length > 1)
              {
                string[] strArray3 = strArray2[0].Split(',', '.');
                if (sqLiteType.Type == DbType.Binary || SQLiteConvert.IsStringDbType(sqLiteType.Type))
                {
                  row1[SchemaTableColumn.ColumnSize] = (object) Convert.ToInt32(strArray3[0], (IFormatProvider) CultureInfo.InvariantCulture);
                }
                else
                {
                  row1[SchemaTableColumn.NumericPrecision] = (object) Convert.ToInt32(strArray3[0], (IFormatProvider) CultureInfo.InvariantCulture);
                  if (strArray3.Length > 1)
                    row1[SchemaTableColumn.NumericScale] = (object) Convert.ToInt32(strArray3[1], (IFormatProvider) CultureInfo.InvariantCulture);
                }
              }
            }
            if (wantDefaultValue)
            {
              using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].TABLE_INFO([{1}])", (object) empty2, (object) empty3), this._command.Connection))
              {
                using (DbDataReader dbDataReader = (DbDataReader) sqLiteCommand.ExecuteReader())
                {
                  while (dbDataReader.Read())
                  {
                    if (string.Compare(empty4, dbDataReader.GetString(1), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                      if (!dbDataReader.IsDBNull(4))
                      {
                        row1[SchemaTableOptionalColumn.DefaultValue] = dbDataReader[4];
                        break;
                      }
                      break;
                    }
                  }
                }
              }
            }
            if (wantUniqueInfo)
            {
              if (empty2 != str1 || empty3 != str2)
              {
                str1 = empty2;
                str2 = empty3;
                dataTable = this._command.Connection.GetSchema("Indexes", new string[4]
                {
                  empty2,
                  null,
                  empty3,
                  null
                });
              }
              foreach (DataRow row2 in (InternalDataCollectionBase) dataTable.Rows)
              {
                DataTable schema = this._command.Connection.GetSchema("IndexColumns", new string[5]
                {
                  empty2,
                  null,
                  empty3,
                  (string) row2["INDEX_NAME"],
                  null
                });
                foreach (DataRow row3 in (InternalDataCollectionBase) schema.Rows)
                {
                  if (string.Compare(SQLiteConvert.GetStringOrNull(row3["COLUMN_NAME"]), columnName, StringComparison.OrdinalIgnoreCase) == 0)
                  {
                    if (parentToColumns.Count == 1)
                    {
                      if (schema.Rows.Count == 1)
                      {
                        if (!flag)
                        {
                          row1[SchemaTableColumn.IsUnique] = row2["UNIQUE"];
                          break;
                        }
                        break;
                      }
                      break;
                    }
                    break;
                  }
                }
              }
            }
          }
          if (string.IsNullOrEmpty(dataType))
          {
            TypeAffinity nAffinity = TypeAffinity.Uninitialized;
            dataType = this._activeStatement._sql.ColumnType(this._activeStatement, index, ref nAffinity);
          }
          if (!string.IsNullOrEmpty(dataType))
            row1["DataTypeName"] = (object) dataType;
        }
        tbl.Rows.Add(row1);
      }
      if (this._keyInfo != null)
        this._keyInfo.AppendSchemaTable(tbl);
      tbl.AcceptChanges();
      tbl.EndLoadData();
      return tbl;
    }

    /// <summary>Retrieves the column as a string</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>string</returns>
    public override string GetString(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetString), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.StringValue;
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetString(i - this.PrivateVisibleFieldCount);
      if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoVerifyTextAffinity))
      {
        int num = (int) this.VerifyType(i, DbType.String);
      }
      return this._activeStatement._sql.GetText(this._activeStatement, i);
    }

    /// <summary>
    /// Retrieves the column as an object corresponding to the underlying datatype of the column
    /// </summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>object</returns>
    public override object GetValue(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionReadValueCallbacks))
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetValue), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.Value;
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetValue(i - this.PrivateVisibleFieldCount);
      SQLiteType sqLiteType = this.GetSQLiteType(this._flags, i);
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DetectTextAffinity) && (sqLiteType == null || sqLiteType.Affinity == TypeAffinity.Text))
        sqLiteType = this.GetSQLiteType(sqLiteType, this._activeStatement._sql.GetText(this._activeStatement, i));
      else if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DetectStringType) && (sqLiteType == null || SQLiteConvert.IsStringDbType(sqLiteType.Type)))
        sqLiteType = this.GetSQLiteType(sqLiteType, this._activeStatement._sql.GetText(this._activeStatement, i));
      return this._activeStatement._sql.GetValue(this._activeStatement, this._flags, i, sqLiteType);
    }

    /// <summary>
    /// Retreives the values of multiple columns, up to the size of the supplied array
    /// </summary>
    /// <param name="values">The array to fill with values from the columns in the current resultset</param>
    /// <returns>The number of columns retrieved</returns>
    public override int GetValues(object[] values)
    {
      this.CheckDisposed();
      int num = this.FieldCount;
      if (values.Length < num)
        num = values.Length;
      for (int ordinal = 0; ordinal < num; ++ordinal)
        values[ordinal] = this.GetValue(ordinal);
      return num;
    }

    /// <summary>
    /// Returns a collection containing all the column names and values for the
    /// current row of data in the current resultset, if any.  If there is no
    /// current row or no current resultset, an exception may be thrown.
    /// </summary>
    /// <returns>
    /// The collection containing the column name and value information for the
    /// current row of data in the current resultset or null if this information
    /// cannot be obtained.
    /// </returns>
    public NameValueCollection GetValues()
    {
      this.CheckDisposed();
      if (this._activeStatement == null || this._activeStatement._sql == null)
        throw new InvalidOperationException();
      int visibleFieldCount = this.PrivateVisibleFieldCount;
      NameValueCollection nameValueCollection = new NameValueCollection(visibleFieldCount);
      for (int index = 0; index < visibleFieldCount; ++index)
      {
        string name = this._activeStatement._sql.ColumnName(this._activeStatement, index);
        string text = this._activeStatement._sql.GetText(this._activeStatement, index);
        nameValueCollection.Add(name, text);
      }
      return nameValueCollection;
    }

    /// <summary>
    /// Returns True if the resultset has rows that can be fetched
    /// </summary>
    public override bool HasRows
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StickyHasRows))
          return this._readingState != 1;
        return this._readingState != 1 || this._stepCount > 0;
      }
    }

    /// <summary>Returns True if the data reader is closed</summary>
    public override bool IsClosed
    {
      get
      {
        this.CheckDisposed();
        return this._command == null;
      }
    }

    /// <summary>Returns True if the specified column is null</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>True or False</returns>
    public override bool IsDBNull(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.IsDBNull(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.IsNull(this._activeStatement, i);
    }

    /// <summary>
    /// Moves to the next resultset in multiple row-returning SQL command.
    /// </summary>
    /// <returns>True if the command was successful and a new resultset is available, False otherwise.</returns>
    public override bool NextResult()
    {
      this.CheckDisposed();
      this.CheckClosed();
      int num1 = this._throwOnDisposed ? 1 : 0;
      SQLiteStatement stmt = (SQLiteStatement) null;
      bool flag = (this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default;
      int num2;
      while (true)
      {
        do
        {
          if (stmt == null && this._activeStatement != null && (this._activeStatement._sql != null && this._activeStatement._sql.IsOpen()))
          {
            if (!flag)
            {
              int num3 = (int) this._activeStatement._sql.Reset(this._activeStatement);
            }
            if ((this._commandBehavior & CommandBehavior.SingleResult) != CommandBehavior.Default)
            {
              while (true)
              {
                SQLiteStatement statement;
                do
                {
                  statement = this._command.GetStatement(this._activeStatementIndex + 1);
                  if (statement != null)
                  {
                    ++this._activeStatementIndex;
                    if (!flag && statement._sql.Step(statement))
                      ++this._stepCount;
                    if (statement._sql.ColumnCount(statement) == 0)
                    {
                      int changes = 0;
                      bool readOnly = false;
                      if (!statement.TryGetChanges(ref changes, ref readOnly))
                        return false;
                      if (!readOnly)
                      {
                        if (this._rowsAffected == -1)
                          this._rowsAffected = 0;
                        this._rowsAffected += changes;
                      }
                    }
                  }
                  else
                    goto label_17;
                }
                while (flag);
                int num4 = (int) statement._sql.Reset(statement);
              }
label_17:
              return false;
            }
          }
          stmt = this._command.GetStatement(this._activeStatementIndex + 1);
          if (stmt == null)
            return false;
          if (this._readingState < 1)
            this._readingState = 1;
          ++this._activeStatementIndex;
          num2 = stmt._sql.ColumnCount(stmt);
          if (!flag || num2 == 0)
          {
            if (!flag && stmt._sql.Step(stmt))
            {
              ++this._stepCount;
              this._readingState = -1;
              goto label_35;
            }
            else if (num2 == 0)
            {
              int changes = 0;
              bool readOnly = false;
              if (!stmt.TryGetChanges(ref changes, ref readOnly))
                return false;
              if (!readOnly)
              {
                if (this._rowsAffected == -1)
                  this._rowsAffected = 0;
                this._rowsAffected += changes;
              }
            }
            else
              goto label_34;
          }
          else
            goto label_35;
        }
        while (flag);
        int num5 = (int) stmt._sql.Reset(stmt);
      }
label_34:
      this._readingState = 1;
label_35:
      this._activeStatement = stmt;
      this._fieldCount = num2;
      this._fieldIndexes = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._fieldTypeArray = new SQLiteType[this.PrivateVisibleFieldCount];
      if ((this._commandBehavior & CommandBehavior.KeyInfo) != CommandBehavior.Default)
        this.LoadKeyInfo();
      return true;
    }

    /// <summary>
    /// This method attempts to query the database connection associated with
    /// the data reader in use.  If the underlying command or connection is
    /// unavailable, a null value will be returned.
    /// </summary>
    /// <returns>The connection object -OR- null if it is unavailable.</returns>
    internal static SQLiteConnection GetConnection(SQLiteDataReader dataReader)
    {
      try
      {
        if (dataReader != null)
        {
          SQLiteCommand command = dataReader._command;
          if (command != null)
          {
            SQLiteConnection connection = command.Connection;
            if (connection != null)
              return connection;
          }
        }
      }
      catch (ObjectDisposedException ex)
      {
      }
      return (SQLiteConnection) null;
    }

    /// <summary>
    /// Retrieves the SQLiteType for a given column and row value.
    /// </summary>
    /// <param name="oldType">
    /// The original SQLiteType structure, based only on the column.
    /// </param>
    /// <param name="text">
    /// The textual value of the column for a given row.
    /// </param>
    /// <returns>The SQLiteType structure.</returns>
    private SQLiteType GetSQLiteType(SQLiteType oldType, string text)
    {
      if (SQLiteConvert.LooksLikeNull(text))
        return new SQLiteType(TypeAffinity.Null, DbType.Object);
      if (SQLiteConvert.LooksLikeInt64(text))
        return new SQLiteType(TypeAffinity.Int64, DbType.Int64);
      if (SQLiteConvert.LooksLikeDouble(text))
        return new SQLiteType(TypeAffinity.Double, DbType.Double);
      return this._activeStatement != null && SQLiteConvert.LooksLikeDateTime((SQLiteConvert) this._activeStatement._sql, text) ? new SQLiteType(TypeAffinity.DateTime, DbType.DateTime) : oldType;
    }

    /// <summary>
    /// Retrieves the SQLiteType for a given column, and caches it to avoid repetetive interop calls.
    /// </summary>
    /// <param name="flags">The flags associated with the parent connection object.</param>
    /// <param name="i">The index of the column.</param>
    /// <returns>A SQLiteType structure</returns>
    private SQLiteType GetSQLiteType(SQLiteConnectionFlags flags, int i)
    {
      SQLiteType sqLiteType = this._fieldTypeArray[i] ?? (this._fieldTypeArray[i] = new SQLiteType());
      if (sqLiteType.Affinity == TypeAffinity.Uninitialized)
        sqLiteType.Type = SQLiteConvert.TypeNameToDbType(SQLiteDataReader.GetConnection(this), this._activeStatement._sql.ColumnType(this._activeStatement, i, ref sqLiteType.Affinity), flags);
      else
        sqLiteType.Affinity = this._activeStatement._sql.ColumnAffinity(this._activeStatement, i);
      return sqLiteType;
    }

    /// <summary>Reads the next row from the resultset</summary>
    /// <returns>True if a new row was successfully loaded and is ready for processing</returns>
    public override bool Read()
    {
      this.CheckDisposed();
      this.CheckClosed();
      int num = this._throwOnDisposed ? 1 : 0;
      if ((this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default)
        return false;
      if (this._readingState == -1)
      {
        this._readingState = 0;
        return true;
      }
      if (this._readingState == 0)
      {
        if ((this._commandBehavior & CommandBehavior.SingleRow) == CommandBehavior.Default && this._activeStatement._sql.Step(this._activeStatement))
        {
          ++this._stepCount;
          if (this._keyInfo != null)
            this._keyInfo.Reset();
          return true;
        }
        this._readingState = 1;
      }
      return false;
    }

    /// <summary>
    /// Returns the number of rows affected by the statement being executed.
    /// The value returned may not be accurate for DDL statements.  Also, it
    /// will be -1 for any statement that does not modify the database (e.g.
    /// SELECT).  If an otherwise read-only statement modifies the database
    /// indirectly (e.g. via a virtual table or user-defined function), the
    /// value returned is undefined.
    /// </summary>
    public override int RecordsAffected
    {
      get
      {
        this.CheckDisposed();
        return this._rowsAffected;
      }
    }

    /// <summary>Indexer to retrieve data from a column given its name</summary>
    /// <param name="name">The name of the column to retrieve data for</param>
    /// <returns>The value contained in the column</returns>
    public override object this[string name]
    {
      get
      {
        this.CheckDisposed();
        return this.GetValue(this.GetOrdinal(name));
      }
    }

    /// <summary>Indexer to retrieve data from a column given its i</summary>
    /// <param name="i">The index of the column.</param>
    /// <returns>The value contained in the column</returns>
    public override object this[int i]
    {
      get
      {
        this.CheckDisposed();
        return this.GetValue(i);
      }
    }

    private void LoadKeyInfo()
    {
      if (this._keyInfo != null)
      {
        this._keyInfo.Dispose();
        this._keyInfo = (SQLiteKeyReader) null;
      }
      this._keyInfo = new SQLiteKeyReader(this._command.Connection, this, this._activeStatement);
    }

    private sealed class ColumnParent : IEqualityComparer<SQLiteDataReader.ColumnParent>
    {
      public string DatabaseName;
      public string TableName;
      public string ColumnName;

      public ColumnParent()
      {
      }

      public ColumnParent(string databaseName, string tableName, string columnName)
        : this()
      {
        this.DatabaseName = databaseName;
        this.TableName = tableName;
        this.ColumnName = columnName;
      }

      public bool Equals(SQLiteDataReader.ColumnParent x, SQLiteDataReader.ColumnParent y) => x == null && y == null || x != null && y != null && (string.Equals(x.DatabaseName, y.DatabaseName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.TableName, y.TableName, StringComparison.OrdinalIgnoreCase)) && string.Equals(x.ColumnName, y.ColumnName, StringComparison.OrdinalIgnoreCase);

      public int GetHashCode(SQLiteDataReader.ColumnParent obj)
      {
        int num = 0;
        if (obj != null && obj.DatabaseName != null)
          num ^= obj.DatabaseName.GetHashCode();
        if (obj != null && obj.TableName != null)
          num ^= obj.TableName.GetHashCode();
        if (obj != null && obj.ColumnName != null)
          num ^= obj.ColumnName.GetHashCode();
        return num;
      }
    }
  }
}
