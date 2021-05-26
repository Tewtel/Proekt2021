// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCommand
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbCommand.</summary>
  [ToolboxItem(true)]
  [Designer("SQLite.Designer.SQLiteCommandDesigner, SQLite.Designer, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
  public sealed class SQLiteCommand : DbCommand, ICloneable
  {
    /// <summary>
    /// The default connection string to be used when creating a temporary
    /// connection to execute a command via the static
    /// <see cref="M:System.Data.SQLite.SQLiteCommand.Execute(System.String,System.Data.SQLite.SQLiteExecuteType,System.String,System.Object[])" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteCommand.Execute(System.String,System.Data.SQLite.SQLiteExecuteType,System.Data.CommandBehavior,System.String,System.Object[])" />
    /// methods.
    /// </summary>
    private static readonly string DefaultConnectionString = "Data Source=:memory:;";
    /// <summary>The command text this command is based on</summary>
    private string _commandText;
    /// <summary>The connection the command is associated with</summary>
    private SQLiteConnection _cnn;
    /// <summary>
    /// The version of the connection the command is associated with
    /// </summary>
    private int _version;
    /// <summary>
    /// Indicates whether or not a DataReader is active on the command.
    /// </summary>
    private WeakReference _activeReader;
    /// <summary>
    /// The timeout for the command, kludged because SQLite doesn't support per-command timeout values
    /// </summary>
    internal int _commandTimeout;
    /// <summary>Designer support</summary>
    private bool _designTimeVisible;
    /// <summary>Used by DbDataAdapter to determine updating behavior</summary>
    private UpdateRowSource _updateRowSource;
    /// <summary>The collection of parameters for the command</summary>
    private SQLiteParameterCollection _parameterCollection;
    /// <summary>
    /// The SQL command text, broken into individual SQL statements as they are executed
    /// </summary>
    internal List<SQLiteStatement> _statementList;
    /// <summary>Unprocessed SQL text that has not been executed</summary>
    internal string _remainingText;
    /// <summary>Transaction associated with this command</summary>
    private SQLiteTransaction _transaction;
    private bool disposed;

    /// <overloads>Constructs a new SQLiteCommand</overloads>
    /// <summary>Default constructor</summary>
    public SQLiteCommand()
      : this((string) null, (SQLiteConnection) null)
    {
    }

    /// <summary>Initializes the command with the given command text</summary>
    /// <param name="commandText">The SQL command text</param>
    public SQLiteCommand(string commandText)
      : this(commandText, (SQLiteConnection) null, (SQLiteTransaction) null)
    {
    }

    /// <summary>
    /// Initializes the command with the given SQL command text and attach the command to the specified
    /// connection.
    /// </summary>
    /// <param name="commandText">The SQL command text</param>
    /// <param name="connection">The connection to associate with the command</param>
    public SQLiteCommand(string commandText, SQLiteConnection connection)
      : this(commandText, connection, (SQLiteTransaction) null)
    {
    }

    /// <summary>
    /// Initializes the command and associates it with the specified connection.
    /// </summary>
    /// <param name="connection">The connection to associate with the command</param>
    public SQLiteCommand(SQLiteConnection connection)
      : this((string) null, connection, (SQLiteTransaction) null)
    {
    }

    private SQLiteCommand(SQLiteCommand source)
      : this(source.CommandText, source.Connection, source.Transaction)
    {
      this.CommandTimeout = source.CommandTimeout;
      this.DesignTimeVisible = source.DesignTimeVisible;
      this.UpdatedRowSource = source.UpdatedRowSource;
      foreach (SQLiteParameter parameter in (DbParameterCollection) source._parameterCollection)
        this.Parameters.Add(parameter.Clone());
    }

    /// <summary>
    /// Initializes a command with the given SQL, connection and transaction
    /// </summary>
    /// <param name="commandText">The SQL command text</param>
    /// <param name="connection">The connection to associate with the command</param>
    /// <param name="transaction">The transaction the command should be associated with</param>
    public SQLiteCommand(
      string commandText,
      SQLiteConnection connection,
      SQLiteTransaction transaction)
    {
      this._commandTimeout = 30;
      this._parameterCollection = new SQLiteParameterCollection(this);
      this._designTimeVisible = true;
      this._updateRowSource = UpdateRowSource.None;
      if (commandText != null)
        this.CommandText = commandText;
      if (connection != null)
      {
        this.DbConnection = (DbConnection) connection;
        this._commandTimeout = connection.DefaultTimeout;
      }
      if (transaction != null)
        this.Transaction = transaction;
      SQLiteConnection.OnChanged(connection, new ConnectionEventArgs(SQLiteConnectionEventType.NewCommand, (StateChangeEventArgs) null, (IDbTransaction) transaction, (IDbCommand) this, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
    }

    [Conditional("CHECK_STATE")]
    internal static void Check(SQLiteCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      command.CheckDisposed();
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteCommand).Name);
    }

    /// <summary>
    /// Disposes of the command and clears all member variables
    /// </summary>
    /// <param name="disposing">Whether or not the class is being explicitly or implicitly disposed</param>
    protected override void Dispose(bool disposing)
    {
      SQLiteConnection.OnChanged(this._cnn, new ConnectionEventArgs(SQLiteConnectionEventType.DisposingCommand, (StateChangeEventArgs) null, (IDbTransaction) this._transaction, (IDbCommand) this, (IDataReader) null, (CriticalHandle) null, (string) null, (object) new object[2]
      {
        (object) disposing,
        (object) this.disposed
      }));
      bool flag = false;
      try
      {
        if (this.disposed || !disposing)
          return;
        SQLiteDataReader sqLiteDataReader = (SQLiteDataReader) null;
        if (this._activeReader != null)
        {
          try
          {
            sqLiteDataReader = this._activeReader.Target as SQLiteDataReader;
          }
          catch (InvalidOperationException ex)
          {
          }
        }
        if (sqLiteDataReader != null)
        {
          sqLiteDataReader._disposeCommand = true;
          this._activeReader = (WeakReference) null;
          flag = true;
        }
        else
        {
          this.Connection = (SQLiteConnection) null;
          this._parameterCollection.Clear();
          this._commandText = (string) null;
        }
      }
      finally
      {
        if (!flag)
        {
          base.Dispose(disposing);
          this.disposed = true;
        }
      }
    }

    /// <summary>
    /// This method attempts to query the flags associated with the database
    /// connection in use.  If the database connection is disposed, the default
    /// flags will be returned.
    /// </summary>
    /// <param name="command">
    /// The command containing the databse connection to query the flags from.
    /// </param>
    /// <returns>The connection flags value.</returns>
    internal static SQLiteConnectionFlags GetFlags(SQLiteCommand command)
    {
      try
      {
        if (command != null)
        {
          SQLiteConnection cnn = command._cnn;
          if (cnn != null)
            return cnn.Flags;
        }
      }
      catch (ObjectDisposedException ex)
      {
      }
      return SQLiteConnectionFlags.Default;
    }

    private void DisposeStatements()
    {
      if (this._statementList == null)
        return;
      int count = this._statementList.Count;
      for (int index = 0; index < count; ++index)
        this._statementList[index]?.Dispose();
      this._statementList = (List<SQLiteStatement>) null;
    }

    private void ClearDataReader()
    {
      if (this._activeReader == null)
        return;
      SQLiteDataReader sqLiteDataReader = (SQLiteDataReader) null;
      try
      {
        sqLiteDataReader = this._activeReader.Target as SQLiteDataReader;
      }
      catch (InvalidOperationException ex)
      {
      }
      sqLiteDataReader?.Close();
      this._activeReader = (WeakReference) null;
    }

    /// <summary>Clears and destroys all statements currently prepared</summary>
    internal void ClearCommands()
    {
      this.ClearDataReader();
      this.DisposeStatements();
      this._parameterCollection.Unbind();
    }

    /// <summary>
    /// Builds an array of prepared statements for each complete SQL statement in the command text
    /// </summary>
    internal SQLiteStatement BuildNextCommand()
    {
      SQLiteStatement activeStatement = (SQLiteStatement) null;
      try
      {
        if (this._cnn != null && this._cnn._sql != null)
        {
          if (this._statementList == null)
            this._remainingText = this._commandText;
          activeStatement = this._cnn._sql.Prepare(this._cnn, this._remainingText, this._statementList == null ? (SQLiteStatement) null : this._statementList[this._statementList.Count - 1], (uint) (this._commandTimeout * 1000), ref this._remainingText);
          if (activeStatement != null)
          {
            activeStatement._command = this;
            if (this._statementList == null)
              this._statementList = new List<SQLiteStatement>();
            this._statementList.Add(activeStatement);
            this._parameterCollection.MapParameters(activeStatement);
            activeStatement.BindParameters();
          }
        }
        return activeStatement;
      }
      catch (Exception ex)
      {
        if (activeStatement != null)
        {
          if (this._statementList != null && this._statementList.Contains(activeStatement))
            this._statementList.Remove(activeStatement);
          activeStatement.Dispose();
        }
        this._remainingText = (string) null;
        throw;
      }
    }

    internal SQLiteStatement GetStatement(int index)
    {
      if (this._statementList == null)
        return this.BuildNextCommand();
      if (index == this._statementList.Count)
        return !string.IsNullOrEmpty(this._remainingText) ? this.BuildNextCommand() : (SQLiteStatement) null;
      SQLiteStatement statement = this._statementList[index];
      statement.BindParameters();
      return statement;
    }

    /// <summary>Not implemented</summary>
    public override void Cancel()
    {
      this.CheckDisposed();
      if (this._activeReader == null || !(this._activeReader.Target is SQLiteDataReader target))
        return;
      target.Cancel();
    }

    /// <summary>The SQL command text associated with the command</summary>
    [Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [RefreshProperties(RefreshProperties.All)]
    [DefaultValue("")]
    public override string CommandText
    {
      get
      {
        this.CheckDisposed();
        return this._commandText;
      }
      set
      {
        this.CheckDisposed();
        if (this._commandText == value)
          return;
        if (this._activeReader != null && this._activeReader.IsAlive)
          throw new InvalidOperationException("Cannot set CommandText while a DataReader is active");
        this.ClearCommands();
        this._commandText = value;
        SQLiteConnection cnn = this._cnn;
      }
    }

    /// <summary>
    /// The amount of time to wait for the connection to become available before erroring out
    /// </summary>
    [DefaultValue(30)]
    public override int CommandTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._commandTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._commandTimeout = value;
      }
    }

    /// <summary>
    /// The type of the command.  SQLite only supports CommandType.Text
    /// </summary>
    [DefaultValue(CommandType.Text)]
    [RefreshProperties(RefreshProperties.All)]
    public override CommandType CommandType
    {
      get
      {
        this.CheckDisposed();
        return CommandType.Text;
      }
      set
      {
        this.CheckDisposed();
        if (value != CommandType.Text)
          throw new NotSupportedException();
      }
    }

    /// <summary>Forwards to the local CreateParameter() function</summary>
    /// <returns></returns>
    protected override DbParameter CreateDbParameter() => (DbParameter) this.CreateParameter();

    /// <summary>Create a new parameter</summary>
    /// <returns></returns>
    public SQLiteParameter CreateParameter()
    {
      this.CheckDisposed();
      return new SQLiteParameter((IDbCommand) this);
    }

    /// <summary>The connection associated with this command</summary>
    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteConnection Connection
    {
      get
      {
        this.CheckDisposed();
        return this._cnn;
      }
      set
      {
        this.CheckDisposed();
        if (this._activeReader != null && this._activeReader.IsAlive)
          throw new InvalidOperationException("Cannot set Connection while a DataReader is active");
        if (this._cnn != null)
          this.ClearCommands();
        this._cnn = value;
        if (this._cnn == null)
          return;
        this._version = this._cnn._version;
      }
    }

    /// <summary>Forwards to the local Connection property</summary>
    protected override DbConnection DbConnection
    {
      get => (DbConnection) this.Connection;
      set => this.Connection = (SQLiteConnection) value;
    }

    /// <summary>
    /// Returns the SQLiteParameterCollection for the given command
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public SQLiteParameterCollection Parameters
    {
      get
      {
        this.CheckDisposed();
        return this._parameterCollection;
      }
    }

    /// <summary>Forwards to the local Parameters property</summary>
    protected override DbParameterCollection DbParameterCollection => (DbParameterCollection) this.Parameters;

    /// <summary>
    /// The transaction associated with this command.  SQLite only supports one transaction per connection, so this property forwards to the
    /// command's underlying connection.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public SQLiteTransaction Transaction
    {
      get
      {
        this.CheckDisposed();
        return this._transaction;
      }
      set
      {
        this.CheckDisposed();
        if (this._cnn != null)
        {
          if (this._activeReader != null && this._activeReader.IsAlive)
            throw new InvalidOperationException("Cannot set Transaction while a DataReader is active");
          if (value != null && value._cnn != this._cnn)
            throw new ArgumentException("Transaction is not associated with the command's connection");
          this._transaction = value;
        }
        else
        {
          if (value != null)
            this.Connection = value.Connection;
          this._transaction = value;
        }
      }
    }

    /// <summary>Forwards to the local Transaction property</summary>
    protected override DbTransaction DbTransaction
    {
      get => (DbTransaction) this.Transaction;
      set => this.Transaction = (SQLiteTransaction) value;
    }

    /// <summary>
    /// Verifies that all SQL queries associated with the current command text
    /// can be successfully compiled.  A <see cref="T:System.Data.SQLite.SQLiteException" /> will be
    /// raised if any errors occur.
    /// </summary>
    public void VerifyOnly()
    {
      this.CheckDisposed();
      SQLiteConnection cnn = this._cnn;
      SQLiteBase sql = cnn._sql;
      if (cnn == null || sql == null)
        throw new SQLiteException("invalid or unusable connection");
      List<SQLiteStatement> sqLiteStatementList = (List<SQLiteStatement>) null;
      SQLiteStatement sqLiteStatement1 = (SQLiteStatement) null;
      try
      {
        string strRemain = this._commandText;
        uint timeoutMS = (uint) (this._commandTimeout * 1000);
        SQLiteStatement previous = (SQLiteStatement) null;
        while (strRemain != null && strRemain.Length > 0)
        {
          sqLiteStatement1 = sql.Prepare(cnn, strRemain, previous, timeoutMS, ref strRemain);
          previous = sqLiteStatement1;
          if (sqLiteStatement1 != null)
          {
            if (sqLiteStatementList == null)
              sqLiteStatementList = new List<SQLiteStatement>();
            sqLiteStatementList.Add(sqLiteStatement1);
            sqLiteStatement1 = (SQLiteStatement) null;
          }
          if (strRemain != null)
            strRemain = strRemain.Trim();
        }
      }
      finally
      {
        sqLiteStatement1?.Dispose();
        if (sqLiteStatementList != null)
        {
          foreach (SQLiteStatement sqLiteStatement2 in sqLiteStatementList)
            sqLiteStatement2?.Dispose();
          sqLiteStatementList.Clear();
        }
      }
    }

    /// <summary>
    /// This function ensures there are no active readers, that we have a valid connection,
    /// that the connection is open, that all statements are prepared and all parameters are assigned
    /// in preparation for allocating a data reader.
    /// </summary>
    private void InitializeForReader()
    {
      if (this._activeReader != null && this._activeReader.IsAlive)
        throw new InvalidOperationException("DataReader already active on this command");
      if (this._cnn == null)
        throw new InvalidOperationException("No connection associated with this command");
      if (this._cnn.State != ConnectionState.Open)
        throw new InvalidOperationException("Database is not open");
      if (this._cnn._version != this._version)
      {
        this._version = this._cnn._version;
        this.ClearCommands();
      }
      this._parameterCollection.MapParameters((SQLiteStatement) null);
    }

    /// <summary>
    /// Creates a new SQLiteDataReader to execute/iterate the array of SQLite prepared statements
    /// </summary>
    /// <param name="behavior">The behavior the data reader should adopt</param>
    /// <returns>Returns a SQLiteDataReader object</returns>
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => (DbDataReader) this.ExecuteReader(behavior);

    /// <summary>
    /// This method creates a new connection, executes the query using the given
    /// execution type, closes the connection, and returns the results.  If the
    /// connection string is null, a temporary in-memory database connection will
    /// be used.
    /// </summary>
    /// <param name="commandText">
    /// The text of the command to be executed.
    /// </param>
    /// <param name="executeType">
    /// The execution type for the command.  This is used to determine which method
    /// of the command object to call, which then determines the type of results
    /// returned, if any.
    /// </param>
    /// <param name="connectionString">
    /// The connection string to the database to be opened, used, and closed.  If
    /// this parameter is null, a temporary in-memory databse will be used.
    /// </param>
    /// <param name="args">
    /// The SQL parameter values to be used when building the command object to be
    /// executed, if any.
    /// </param>
    /// <returns>
    /// The results of the query -OR- null if no results were produced from the
    /// given execution type.
    /// </returns>
    public static object Execute(
      string commandText,
      SQLiteExecuteType executeType,
      string connectionString,
      params object[] args)
    {
      return SQLiteCommand.Execute(commandText, executeType, CommandBehavior.Default, connectionString, args);
    }

    /// <summary>
    /// This method creates a new connection, executes the query using the given
    /// execution type and command behavior, closes the connection unless a data
    /// reader is created, and returns the results.  If the connection string is
    /// null, a temporary in-memory database connection will be used.
    /// </summary>
    /// <param name="commandText">
    /// The text of the command to be executed.
    /// </param>
    /// <param name="executeType">
    /// The execution type for the command.  This is used to determine which method
    /// of the command object to call, which then determines the type of results
    /// returned, if any.
    /// </param>
    /// <param name="commandBehavior">
    /// The command behavior flags for the command.
    /// </param>
    /// <param name="connectionString">
    /// The connection string to the database to be opened, used, and closed.  If
    /// this parameter is null, a temporary in-memory databse will be used.
    /// </param>
    /// <param name="args">
    /// The SQL parameter values to be used when building the command object to be
    /// executed, if any.
    /// </param>
    /// <returns>
    /// The results of the query -OR- null if no results were produced from the
    /// given execution type.
    /// </returns>
    public static object Execute(
      string commandText,
      SQLiteExecuteType executeType,
      CommandBehavior commandBehavior,
      string connectionString,
      params object[] args)
    {
      SQLiteConnection sqLiteConnection = (SQLiteConnection) null;
      try
      {
        if (connectionString == null)
          connectionString = SQLiteCommand.DefaultConnectionString;
        using (sqLiteConnection = new SQLiteConnection(connectionString))
        {
          sqLiteConnection.Open();
          using (SQLiteCommand command = sqLiteConnection.CreateCommand())
          {
            command.CommandText = commandText;
            if (args != null)
            {
              foreach (object obj in args)
              {
                if (!(obj is SQLiteParameter parameter9))
                {
                  parameter9 = command.CreateParameter();
                  parameter9.DbType = DbType.Object;
                  parameter9.Value = obj;
                }
                command.Parameters.Add(parameter9);
              }
            }
            switch (executeType)
            {
              case SQLiteExecuteType.NonQuery:
                return (object) command.ExecuteNonQuery(commandBehavior);
              case SQLiteExecuteType.Scalar:
                return command.ExecuteScalar(commandBehavior);
              case SQLiteExecuteType.Reader:
                bool flag = true;
                try
                {
                  return (object) command.ExecuteReader(commandBehavior | CommandBehavior.CloseConnection);
                }
                catch
                {
                  flag = false;
                  throw;
                }
                finally
                {
                  if (flag)
                    sqLiteConnection._noDispose = true;
                }
            }
          }
        }
      }
      finally
      {
        if (sqLiteConnection != null)
          sqLiteConnection._noDispose = false;
      }
      return (object) null;
    }

    /// <summary>
    /// Overrides the default behavior to return a SQLiteDataReader specialization class
    /// </summary>
    /// <param name="behavior">The flags to be associated with the reader.</param>
    /// <returns>A SQLiteDataReader</returns>
    public SQLiteDataReader ExecuteReader(CommandBehavior behavior)
    {
      this.CheckDisposed();
      this.InitializeForReader();
      SQLiteDataReader sqLiteDataReader = new SQLiteDataReader(this, behavior);
      this._activeReader = new WeakReference((object) sqLiteDataReader, false);
      return sqLiteDataReader;
    }

    /// <summary>
    /// Overrides the default behavior of DbDataReader to return a specialized SQLiteDataReader class
    /// </summary>
    /// <returns>A SQLiteDataReader</returns>
    public SQLiteDataReader ExecuteReader()
    {
      this.CheckDisposed();
      return this.ExecuteReader(CommandBehavior.Default);
    }

    /// <summary>
    /// Called by the SQLiteDataReader when the data reader is closed.
    /// </summary>
    internal void ResetDataReader() => this._activeReader = (WeakReference) null;

    /// <summary>
    /// Execute the command and return the number of rows inserted/updated affected by it.
    /// </summary>
    /// <returns>The number of rows inserted/updated affected by it.</returns>
    public override int ExecuteNonQuery()
    {
      this.CheckDisposed();
      return this.ExecuteNonQuery(CommandBehavior.Default);
    }

    /// <summary>
    /// Execute the command and return the number of rows inserted/updated affected by it.
    /// </summary>
    /// <param name="behavior">The flags to be associated with the reader.</param>
    /// <returns>The number of rows inserted/updated affected by it.</returns>
    public int ExecuteNonQuery(CommandBehavior behavior)
    {
      this.CheckDisposed();
      using (SQLiteDataReader sqLiteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
      {
        do
          ;
        while (sqLiteDataReader.NextResult());
        return sqLiteDataReader.RecordsAffected;
      }
    }

    /// <summary>
    /// Execute the command and return the first column of the first row of the resultset
    /// (if present), or null if no resultset was returned.
    /// </summary>
    /// <returns>The first column of the first row of the first resultset from the query.</returns>
    public override object ExecuteScalar()
    {
      this.CheckDisposed();
      return this.ExecuteScalar(CommandBehavior.Default);
    }

    /// <summary>
    /// Execute the command and return the first column of the first row of the resultset
    /// (if present), or null if no resultset was returned.
    /// </summary>
    /// <param name="behavior">The flags to be associated with the reader.</param>
    /// <returns>The first column of the first row of the first resultset from the query.</returns>
    public object ExecuteScalar(CommandBehavior behavior)
    {
      this.CheckDisposed();
      using (SQLiteDataReader sqLiteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
      {
        if (sqLiteDataReader.Read())
        {
          if (sqLiteDataReader.FieldCount > 0)
            return sqLiteDataReader[0];
        }
      }
      return (object) null;
    }

    /// <summary>
    /// This method resets all the prepared statements held by this instance
    /// back to their initial states, ready to be re-executed.
    /// </summary>
    public void Reset()
    {
      this.CheckDisposed();
      this.Reset(true, false);
    }

    /// <summary>
    /// This method resets all the prepared statements held by this instance
    /// back to their initial states, ready to be re-executed.
    /// </summary>
    /// <param name="clearBindings">
    /// Non-zero if the parameter bindings should be cleared as well.
    /// </param>
    /// <param name="ignoreErrors">
    /// If this is zero, a <see cref="T:System.Data.SQLite.SQLiteException" /> may be thrown for
    /// any unsuccessful return codes from the native library; otherwise, a
    /// <see cref="T:System.Data.SQLite.SQLiteException" /> will only be thrown if the connection
    /// or its state is invalid.
    /// </param>
    public void Reset(bool clearBindings, bool ignoreErrors)
    {
      this.CheckDisposed();
      if (clearBindings && this._parameterCollection != null)
        this._parameterCollection.Unbind();
      this.ClearDataReader();
      if (this._statementList == null)
        return;
      SQLiteBase sql = this._cnn._sql;
      foreach (SQLiteStatement statement in this._statementList)
      {
        if (statement != null)
        {
          SQLiteStatementHandle sqliteStmt = statement._sqlite_stmt;
          if (sqliteStmt != null)
          {
            SQLiteErrorCode errorCode = sql.Reset(statement);
            if (errorCode == SQLiteErrorCode.Ok && clearBindings && SQLite3.SQLiteVersionNumber >= 3003007)
              errorCode = UnsafeNativeMethods.sqlite3_clear_bindings((IntPtr) sqliteStmt);
            if (!ignoreErrors && errorCode != SQLiteErrorCode.Ok)
              throw new SQLiteException(errorCode, sql.GetLastError());
          }
        }
      }
    }

    /// <summary>
    /// Does nothing.  Commands are prepared as they are executed the first time, and kept in prepared state afterwards.
    /// </summary>
    public override void Prepare() => this.CheckDisposed();

    /// <summary>
    /// Sets the method the SQLiteCommandBuilder uses to determine how to update inserted or updated rows in a DataTable.
    /// </summary>
    [DefaultValue(UpdateRowSource.None)]
    public override UpdateRowSource UpdatedRowSource
    {
      get
      {
        this.CheckDisposed();
        return this._updateRowSource;
      }
      set
      {
        this.CheckDisposed();
        this._updateRowSource = value;
      }
    }

    /// <summary>
    /// Determines if the command is visible at design time.  Defaults to True.
    /// </summary>
    [Browsable(false)]
    [DesignOnly(true)]
    [DefaultValue(true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool DesignTimeVisible
    {
      get
      {
        this.CheckDisposed();
        return this._designTimeVisible;
      }
      set
      {
        this.CheckDisposed();
        this._designTimeVisible = value;
        TypeDescriptor.Refresh((object) this);
      }
    }

    /// <summary>Clones a command, including all its parameters</summary>
    /// <returns>A new SQLiteCommand with the same commandtext, connection and parameters</returns>
    public object Clone()
    {
      this.CheckDisposed();
      return (object) new SQLiteCommand(this);
    }
  }
}
