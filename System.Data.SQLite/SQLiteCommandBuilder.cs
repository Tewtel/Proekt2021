// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCommandBuilder
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbCommandBuilder.</summary>
  public sealed class SQLiteCommandBuilder : DbCommandBuilder
  {
    private bool disposed;

    /// <summary>Default constructor</summary>
    public SQLiteCommandBuilder()
      : this((SQLiteDataAdapter) null)
    {
    }

    /// <summary>
    /// Initializes the command builder and associates it with the specified data adapter.
    /// </summary>
    /// <param name="adp"></param>
    public SQLiteCommandBuilder(SQLiteDataAdapter adp)
    {
      this.QuotePrefix = "[";
      this.QuoteSuffix = "]";
      this.DataAdapter = adp;
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteCommandBuilder).Name);
    }

    /// <summary>
    /// Cleans up resources (native and managed) associated with the current instance.
    /// </summary>
    /// <param name="disposing">
    /// Zero when being disposed via garbage collection; otherwise, non-zero.
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

    /// <summary>
    /// Minimal amount of parameter processing.  Primarily sets the DbType for the parameter equal to the provider type in the schema
    /// </summary>
    /// <param name="parameter">The parameter to use in applying custom behaviors to a row</param>
    /// <param name="row">The row to apply the parameter to</param>
    /// <param name="statementType">The type of statement</param>
    /// <param name="whereClause">Whether the application of the parameter is part of a WHERE clause</param>
    protected override void ApplyParameterInfo(
      DbParameter parameter,
      DataRow row,
      StatementType statementType,
      bool whereClause)
    {
      parameter.DbType = (DbType) row[SchemaTableColumn.ProviderType];
    }

    /// <summary>Returns a valid named parameter</summary>
    /// <param name="parameterName">The name of the parameter</param>
    /// <returns>Error</returns>
    protected override string GetParameterName(string parameterName) => HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "@{0}", (object) parameterName);

    /// <summary>Returns a named parameter for the given ordinal</summary>
    /// <param name="parameterOrdinal">The i of the parameter</param>
    /// <returns>Error</returns>
    protected override string GetParameterName(int parameterOrdinal) => HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "@param{0}", (object) parameterOrdinal);

    /// <summary>
    /// Returns a placeholder character for the specified parameter i.
    /// </summary>
    /// <param name="parameterOrdinal">The index of the parameter to provide a placeholder for</param>
    /// <returns>Returns a named parameter</returns>
    protected override string GetParameterPlaceholder(int parameterOrdinal) => this.GetParameterName(parameterOrdinal);

    /// <summary>
    /// Sets the handler for receiving row updating events.  Used by the DbCommandBuilder to autogenerate SQL
    /// statements that may not have previously been generated.
    /// </summary>
    /// <param name="adapter">A data adapter to receive events on.</param>
    protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
    {
      if (adapter == base.DataAdapter)
        ((SQLiteDataAdapter) adapter).RowUpdating -= new EventHandler<RowUpdatingEventArgs>(this.RowUpdatingEventHandler);
      else
        ((SQLiteDataAdapter) adapter).RowUpdating += new EventHandler<RowUpdatingEventArgs>(this.RowUpdatingEventHandler);
    }

    private void RowUpdatingEventHandler(object sender, RowUpdatingEventArgs e) => this.RowUpdatingHandler(e);

    /// <summary>Gets/sets the DataAdapter for this CommandBuilder</summary>
    public SQLiteDataAdapter DataAdapter
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteDataAdapter) base.DataAdapter;
      }
      set
      {
        this.CheckDisposed();
        this.DataAdapter = (DbDataAdapter) value;
      }
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to delete rows from the database
    /// </summary>
    /// <returns></returns>
    public SQLiteCommand GetDeleteCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetDeleteCommand();
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to delete rows from the database
    /// </summary>
    /// <param name="useColumnsForParameterNames"></param>
    /// <returns></returns>
    public SQLiteCommand GetDeleteCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetDeleteCommand(useColumnsForParameterNames);
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to update rows in the database
    /// </summary>
    /// <returns></returns>
    public SQLiteCommand GetUpdateCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetUpdateCommand();
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to update rows in the database
    /// </summary>
    /// <param name="useColumnsForParameterNames"></param>
    /// <returns></returns>
    public SQLiteCommand GetUpdateCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetUpdateCommand(useColumnsForParameterNames);
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to insert rows into the database
    /// </summary>
    /// <returns></returns>
    public SQLiteCommand GetInsertCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetInsertCommand();
    }

    /// <summary>
    /// Returns the automatically-generated SQLite command to insert rows into the database
    /// </summary>
    /// <param name="useColumnsForParameterNames"></param>
    /// <returns></returns>
    public SQLiteCommand GetInsertCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetInsertCommand(useColumnsForParameterNames);
    }

    /// <summary>Overridden to hide its property from the designer</summary>
    [Browsable(false)]
    public override CatalogLocation CatalogLocation
    {
      get
      {
        this.CheckDisposed();
        return base.CatalogLocation;
      }
      set
      {
        this.CheckDisposed();
        base.CatalogLocation = value;
      }
    }

    /// <summary>Overridden to hide its property from the designer</summary>
    [Browsable(false)]
    public override string CatalogSeparator
    {
      get
      {
        this.CheckDisposed();
        return base.CatalogSeparator;
      }
      set
      {
        this.CheckDisposed();
        base.CatalogSeparator = value;
      }
    }

    /// <summary>Overridden to hide its property from the designer</summary>
    [Browsable(false)]
    [DefaultValue("[")]
    public override string QuotePrefix
    {
      get
      {
        this.CheckDisposed();
        return base.QuotePrefix;
      }
      set
      {
        this.CheckDisposed();
        base.QuotePrefix = value;
      }
    }

    /// <summary>Overridden to hide its property from the designer</summary>
    [Browsable(false)]
    public override string QuoteSuffix
    {
      get
      {
        this.CheckDisposed();
        return base.QuoteSuffix;
      }
      set
      {
        this.CheckDisposed();
        base.QuoteSuffix = value;
      }
    }

    /// <summary>Places brackets around an identifier</summary>
    /// <param name="unquotedIdentifier">The identifier to quote</param>
    /// <returns>The bracketed identifier</returns>
    public override string QuoteIdentifier(string unquotedIdentifier)
    {
      this.CheckDisposed();
      return string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || string.IsNullOrEmpty(unquotedIdentifier) ? unquotedIdentifier : this.QuotePrefix + unquotedIdentifier.Replace(this.QuoteSuffix, this.QuoteSuffix + this.QuoteSuffix) + this.QuoteSuffix;
    }

    /// <summary>Removes brackets around an identifier</summary>
    /// <param name="quotedIdentifier">The quoted (bracketed) identifier</param>
    /// <returns>The undecorated identifier</returns>
    public override string UnquoteIdentifier(string quotedIdentifier)
    {
      this.CheckDisposed();
      return string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || (string.IsNullOrEmpty(quotedIdentifier) || !quotedIdentifier.StartsWith(this.QuotePrefix, StringComparison.OrdinalIgnoreCase)) || !quotedIdentifier.EndsWith(this.QuoteSuffix, StringComparison.OrdinalIgnoreCase) ? quotedIdentifier : quotedIdentifier.Substring(this.QuotePrefix.Length, quotedIdentifier.Length - (this.QuotePrefix.Length + this.QuoteSuffix.Length)).Replace(this.QuoteSuffix + this.QuoteSuffix, this.QuoteSuffix);
    }

    /// <summary>Overridden to hide its property from the designer</summary>
    [Browsable(false)]
    public override string SchemaSeparator
    {
      get
      {
        this.CheckDisposed();
        return base.SchemaSeparator;
      }
      set
      {
        this.CheckDisposed();
        base.SchemaSeparator = value;
      }
    }

    /// <summary>
    /// Override helper, which can help the base command builder choose the right keys for the given query
    /// </summary>
    /// <param name="sourceCommand"></param>
    /// <returns></returns>
    protected override DataTable GetSchemaTable(DbCommand sourceCommand)
    {
      using (IDataReader dataReader = (IDataReader) sourceCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
      {
        DataTable schemaTable = dataReader.GetSchemaTable();
        if (this.HasSchemaPrimaryKey(schemaTable))
          this.ResetIsUniqueSchemaColumn(schemaTable);
        return schemaTable;
      }
    }

    private bool HasSchemaPrimaryKey(DataTable schema)
    {
      DataColumn column = schema.Columns[SchemaTableColumn.IsKey];
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if ((bool) row[column])
          return true;
      }
      return false;
    }

    private void ResetIsUniqueSchemaColumn(DataTable schema)
    {
      DataColumn column1 = schema.Columns[SchemaTableColumn.IsUnique];
      DataColumn column2 = schema.Columns[SchemaTableColumn.IsKey];
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if (!(bool) row[column2])
          row[column1] = (object) false;
      }
      schema.AcceptChanges();
    }
  }
}
