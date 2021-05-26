// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStatement
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>Represents a single SQL statement in SQLite.</summary>
  internal sealed class SQLiteStatement : IDisposable
  {
    /// <summary>
    /// The underlying SQLite object this statement is bound to
    /// </summary>
    internal SQLiteBase _sql;
    /// <summary>The command text of this SQL statement</summary>
    internal string _sqlStatement;
    /// <summary>The actual statement pointer</summary>
    internal SQLiteStatementHandle _sqlite_stmt;
    /// <summary>An index from which unnamed parameters begin</summary>
    internal int _unnamedParameters;
    /// <summary>
    /// Names of the parameters as SQLite understands them to be
    /// </summary>
    internal string[] _paramNames;
    /// <summary>Parameters for this statement</summary>
    internal SQLiteParameter[] _paramValues;
    /// <summary>Command this statement belongs to (if any)</summary>
    internal SQLiteCommand _command;
    /// <summary>
    /// The flags associated with the parent connection object.
    /// </summary>
    private SQLiteConnectionFlags _flags;
    private string[] _types;
    private bool disposed;

    /// <summary>
    /// Initializes the statement and attempts to get all information about parameters in the statement
    /// </summary>
    /// <param name="sqlbase">The base SQLite object</param>
    /// <param name="flags">The flags associated with the parent connection object</param>
    /// <param name="stmt">The statement</param>
    /// <param name="strCommand">The command text for this statement</param>
    /// <param name="previous">The previous command in a multi-statement command</param>
    internal SQLiteStatement(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags,
      SQLiteStatementHandle stmt,
      string strCommand,
      SQLiteStatement previous)
    {
      this._sql = sqlbase;
      this._sqlite_stmt = stmt;
      this._sqlStatement = strCommand;
      this._flags = flags;
      int num = 0;
      int length = this._sql.Bind_ParamCount(this, this._flags);
      if (length <= 0)
        return;
      if (previous != null)
        num = previous._unnamedParameters;
      this._paramNames = new string[length];
      this._paramValues = new SQLiteParameter[length];
      for (int index = 0; index < length; ++index)
      {
        string str = this._sql.Bind_ParamName(this, this._flags, index + 1);
        if (string.IsNullOrEmpty(str))
        {
          str = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num);
          ++num;
          ++this._unnamedParameters;
        }
        this._paramNames[index] = str;
        this._paramValues[index] = (SQLiteParameter) null;
      }
    }

    /// <summary>Disposes and finalizes the statement</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteStatement).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_stmt != null)
        {
          this._sqlite_stmt.Dispose();
          this._sqlite_stmt = (SQLiteStatementHandle) null;
        }
        this._paramNames = (string[]) null;
        this._paramValues = (SQLiteParameter[]) null;
        this._sql = (SQLiteBase) null;
        this._sqlStatement = (string) null;
      }
      this.disposed = true;
    }

    ~SQLiteStatement() => this.Dispose(false);

    /// <summary>
    /// If the underlying database connection is open, fetches the number of changed rows
    /// resulting from the most recent query; otherwise, does nothing.
    /// </summary>
    /// <param name="changes">
    /// The number of changes when true is returned.
    /// Undefined if false is returned.
    /// </param>
    /// <param name="readOnly">
    /// The read-only flag when true is returned.
    /// Undefined if false is returned.
    /// </param>
    /// <returns>Non-zero if the number of changed rows was fetched.</returns>
    internal bool TryGetChanges(ref int changes, ref bool readOnly)
    {
      if (this._sql == null || !this._sql.IsOpen())
        return false;
      changes = this._sql.Changes;
      readOnly = this._sql.IsReadOnly(this);
      return true;
    }

    /// <summary>
    /// Called by SQLiteParameterCollection, this function determines if the specified parameter name belongs to
    /// this statement, and if so, keeps a reference to the parameter so it can be bound later.
    /// </summary>
    /// <param name="s">The parameter name to map</param>
    /// <param name="p">The parameter to assign it</param>
    internal bool MapParameter(string s, SQLiteParameter p)
    {
      if (this._paramNames == null)
        return false;
      int indexA = 0;
      if (s.Length > 0 && ":$@;".IndexOf(s[0]) == -1)
        indexA = 1;
      int length = this._paramNames.Length;
      for (int index = 0; index < length; ++index)
      {
        if (string.Compare(this._paramNames[index], indexA, s, 0, Math.Max(this._paramNames[index].Length - indexA, s.Length), StringComparison.OrdinalIgnoreCase) == 0)
        {
          this._paramValues[index] = p;
          return true;
        }
      }
      return false;
    }

    /// <summary>
    ///  Bind all parameters, making sure the caller didn't miss any
    /// </summary>
    internal void BindParameters()
    {
      if (this._paramNames == null)
        return;
      int length = this._paramNames.Length;
      for (int index = 0; index < length; ++index)
        this.BindParameter(index + 1, this._paramValues[index]);
    }

    /// <summary>
    /// This method attempts to query the database connection associated with
    /// the statement in use.  If the underlying command or connection is
    /// unavailable, a null value will be returned.
    /// </summary>
    /// <returns>The connection object -OR- null if it is unavailable.</returns>
    private static SQLiteConnection GetConnection(SQLiteStatement statement)
    {
      try
      {
        if (statement != null)
        {
          SQLiteCommand command = statement._command;
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
    /// Invokes the parameter binding callback configured for the database
    /// type name associated with the specified column.  If no parameter
    /// binding callback is available for the database type name, do
    /// nothing.
    /// </summary>
    /// <param name="index">The index of the column being read.</param>
    /// <param name="parameter">
    /// The <see cref="T:System.Data.SQLite.SQLiteParameter" /> instance being bound to the
    /// command.
    /// </param>
    /// <param name="complete">
    /// Non-zero if the default handling for the parameter binding call
    /// should be skipped (i.e. the parameter should not be bound at all).
    /// Great care should be used when setting this to non-zero.
    /// </param>
    private void InvokeBindValueCallback(int index, SQLiteParameter parameter, out bool complete)
    {
      complete = false;
      SQLiteConnectionFlags flags = this._flags;
      this._flags &= ~SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
      try
      {
        if (parameter == null)
          return;
        SQLiteConnection connection = SQLiteStatement.GetConnection(this);
        if (connection == null)
          return;
        string typeName = parameter.TypeName;
        if (typeName == null && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseParameterNameForTypeName))
          typeName = parameter.ParameterName;
        if (typeName == null && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseParameterDbTypeForTypeName))
          typeName = SQLiteConvert.DbTypeToTypeName(connection, parameter.DbType, this._flags);
        SQLiteTypeCallbacks callbacks;
        if (typeName == null || !connection.TryGetTypeCallbacks(typeName, out callbacks) || callbacks == null)
          return;
        SQLiteBindValueCallback bindValueCallback = callbacks.BindValueCallback;
        if (bindValueCallback == null)
          return;
        object bindValueUserData = callbacks.BindValueUserData;
        bindValueCallback((SQLiteConvert) this._sql, this._command, flags, parameter, typeName, index, bindValueUserData, out complete);
      }
      finally
      {
        this._flags |= SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
      }
    }

    /// <summary>
    /// Perform the bind operation for an individual parameter
    /// </summary>
    /// <param name="index">The index of the parameter to bind</param>
    /// <param name="param">The parameter we're binding</param>
    private void BindParameter(int index, SQLiteParameter param)
    {
      if (param == null)
        throw new SQLiteException("Insufficient parameters supplied to the command");
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionBindValueCallbacks))
      {
        bool complete;
        this.InvokeBindValueCallback(index, param, out complete);
        if (complete)
          return;
      }
      object obj = param.Value;
      DbType dbType = param.DbType;
      if (obj != null && dbType == DbType.Object)
        dbType = SQLiteConvert.TypeToDbType(obj.GetType());
      if (SQLite3.ForceLogPrepare() || HelperMethods.LogPreBind(this._flags))
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} with database type {2} and raw value {{{3}}}...", (object) (IntPtr) this._sqlite_stmt, (object) index, (object) dbType, obj));
      if (obj == null || Convert.IsDBNull(obj))
      {
        this._sql.Bind_Null(this, this._flags, index);
      }
      else
      {
        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        bool flag1 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindInvariantText);
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.ConvertInvariantText))
          cultureInfo = invariantCulture;
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindAllAsText))
        {
          if (obj is DateTime dt4)
            this._sql.Bind_DateTime(this, this._flags, index, dt4);
          else
            this._sql.Bind_Text(this, this._flags, index, flag1 ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) cultureInfo));
        }
        else
        {
          bool flag2 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindInvariantDecimal);
          if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.BindDecimalAsText) && obj is Decimal)
          {
            this._sql.Bind_Text(this, this._flags, index, flag1 || flag2 ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) cultureInfo));
          }
          else
          {
            switch (dbType)
            {
              case DbType.Binary:
                this._sql.Bind_Blob(this, this._flags, index, (byte[]) obj);
                break;
              case DbType.Byte:
                this._sql.Bind_UInt32(this, this._flags, index, (uint) Convert.ToByte(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Boolean:
                this._sql.Bind_Boolean(this, this._flags, index, SQLiteConvert.ToBoolean(obj, (IFormatProvider) cultureInfo, true));
                break;
              case DbType.Currency:
              case DbType.Double:
              case DbType.Single:
                this._sql.Bind_Double(this, this._flags, index, Convert.ToDouble(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Date:
              case DbType.DateTime:
              case DbType.Time:
                this._sql.Bind_DateTime(this, this._flags, index, obj is string ? this._sql.ToDateTime((string) obj) : Convert.ToDateTime(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Decimal:
                this._sql.Bind_Text(this, this._flags, index, flag1 || flag2 ? SQLiteConvert.ToStringWithProvider((object) Convert.ToDecimal(obj, (IFormatProvider) cultureInfo), (IFormatProvider) invariantCulture) : SQLiteConvert.ToStringWithProvider((object) Convert.ToDecimal(obj, (IFormatProvider) cultureInfo), (IFormatProvider) cultureInfo));
                break;
              case DbType.Guid:
                if (this._command.Connection._binaryGuid)
                {
                  this._sql.Bind_Blob(this, this._flags, index, ((Guid) obj).ToByteArray());
                  break;
                }
                this._sql.Bind_Text(this, this._flags, index, flag1 ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Int16:
                this._sql.Bind_Int32(this, this._flags, index, (int) Convert.ToInt16(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Int32:
                this._sql.Bind_Int32(this, this._flags, index, Convert.ToInt32(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.Int64:
                this._sql.Bind_Int64(this, this._flags, index, Convert.ToInt64(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.SByte:
                this._sql.Bind_Int32(this, this._flags, index, (int) Convert.ToSByte(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.UInt16:
                this._sql.Bind_UInt32(this, this._flags, index, (uint) Convert.ToUInt16(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.UInt32:
                this._sql.Bind_UInt32(this, this._flags, index, Convert.ToUInt32(obj, (IFormatProvider) cultureInfo));
                break;
              case DbType.UInt64:
                this._sql.Bind_UInt64(this, this._flags, index, Convert.ToUInt64(obj, (IFormatProvider) cultureInfo));
                break;
              default:
                this._sql.Bind_Text(this, this._flags, index, flag1 ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) cultureInfo));
                break;
            }
          }
        }
      }
    }

    internal string[] TypeDefinitions => this._types;

    internal void SetTypes(string typedefs)
    {
      int num = typedefs.IndexOf("TYPES", 0, StringComparison.OrdinalIgnoreCase);
      if (num == -1)
        throw new ArgumentOutOfRangeException();
      string[] strArray = typedefs.Substring(num + 6).Replace(" ", string.Empty).Replace(";", string.Empty).Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("`", string.Empty).Split(',', '\r', '\n', '\t');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (string.IsNullOrEmpty(strArray[index]))
          strArray[index] = (string) null;
      }
      this._types = strArray;
    }
  }
}
