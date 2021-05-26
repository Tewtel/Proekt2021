// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteKeyReader
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class provides key info for a given SQLite statement.
  /// <remarks>
  /// Providing key information for a given statement is non-trivial :(
  /// </remarks>
  /// </summary>
  internal sealed class SQLiteKeyReader : IDisposable
  {
    private SQLiteKeyReader.KeyInfo[] _keyInfo;
    private SQLiteStatement _stmt;
    private bool _isValid;
    private SQLiteKeyReader.RowIdInfo[] _rowIdInfo;
    private bool disposed;

    /// <summary>
    /// This function does all the nasty work at determining what keys need to be returned for
    /// a given statement.
    /// </summary>
    /// <param name="cnn"></param>
    /// <param name="reader"></param>
    /// <param name="stmt"></param>
    internal SQLiteKeyReader(SQLiteConnection cnn, SQLiteDataReader reader, SQLiteStatement stmt)
    {
      Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
      Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
      List<SQLiteKeyReader.KeyInfo> keyInfoList = new List<SQLiteKeyReader.KeyInfo>();
      List<SQLiteKeyReader.RowIdInfo> rowIdInfoList = new List<SQLiteKeyReader.RowIdInfo>();
      this._stmt = stmt;
      using (DataTable schema = cnn.GetSchema("Catalogs"))
      {
        foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
          dictionary1.Add((string) row["CATALOG_NAME"], Convert.ToInt32(row["ID"], (IFormatProvider) CultureInfo.InvariantCulture));
      }
      using (DataTable schemaTable = reader.GetSchemaTable(false, false))
      {
        foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
        {
          if (row[SchemaTableOptionalColumn.BaseCatalogName] != DBNull.Value)
          {
            string key = (string) row[SchemaTableOptionalColumn.BaseCatalogName];
            string str = (string) row[SchemaTableColumn.BaseTableName];
            List<string> stringList;
            if (!dictionary2.ContainsKey(key))
            {
              stringList = new List<string>();
              dictionary2.Add(key, stringList);
            }
            else
              stringList = dictionary2[key];
            if (!stringList.Contains(str))
              stringList.Add(str);
          }
        }
        foreach (KeyValuePair<string, List<string>> keyValuePair in dictionary2)
        {
          for (int index1 = 0; index1 < keyValuePair.Value.Count; ++index1)
          {
            string table = keyValuePair.Value[index1];
            DataRow dataRow = (DataRow) null;
            using (DataTable schema1 = cnn.GetSchema("Indexes", new string[3]
            {
              keyValuePair.Key,
              null,
              table
            }))
            {
              for (int index2 = 0; index2 < 2 && dataRow == null; ++index2)
              {
                foreach (DataRow row in (InternalDataCollectionBase) schema1.Rows)
                {
                  if (index2 == 0 && (bool) row["PRIMARY_KEY"])
                  {
                    dataRow = row;
                    break;
                  }
                  if (index2 == 1 && (bool) row["UNIQUE"])
                  {
                    dataRow = row;
                    break;
                  }
                }
              }
              if (dataRow == null)
              {
                keyValuePair.Value.RemoveAt(index1);
                --index1;
              }
              else
              {
                using (DataTable schema2 = cnn.GetSchema("Tables", new string[3]
                {
                  keyValuePair.Key,
                  null,
                  table
                }))
                {
                  int database = dictionary1[keyValuePair.Key];
                  int int32 = Convert.ToInt32(schema2.Rows[0]["TABLE_ROOTPAGE"], (IFormatProvider) CultureInfo.InvariantCulture);
                  int cursorForTable = stmt._sql.GetCursorForTable(stmt, database, int32);
                  using (DataTable schema3 = cnn.GetSchema("IndexColumns", new string[4]
                  {
                    keyValuePair.Key,
                    null,
                    table,
                    (string) dataRow["INDEX_NAME"]
                  }))
                  {
                    bool flag1 = (string) dataRow["INDEX_NAME"] == "sqlite_master_PK_" + table;
                    SQLiteKeyReader.KeyQuery keyQuery = (SQLiteKeyReader.KeyQuery) null;
                    List<string> stringList = new List<string>();
                    for (int index2 = 0; index2 < schema3.Rows.Count; ++index2)
                    {
                      string stringOrNull = SQLiteConvert.GetStringOrNull(schema3.Rows[index2]["COLUMN_NAME"]);
                      bool flag2 = true;
                      foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
                      {
                        if (!row.IsNull(SchemaTableColumn.BaseColumnName) && (string) row[SchemaTableColumn.BaseColumnName] == stringOrNull && ((string) row[SchemaTableColumn.BaseTableName] == table && (string) row[SchemaTableOptionalColumn.BaseCatalogName] == keyValuePair.Key))
                        {
                          if (flag1)
                            rowIdInfoList.Add(new SQLiteKeyReader.RowIdInfo()
                            {
                              databaseName = keyValuePair.Key,
                              tableName = table,
                              column = (int) row[SchemaTableColumn.ColumnOrdinal]
                            });
                          schema3.Rows.RemoveAt(index2);
                          --index2;
                          flag2 = false;
                          break;
                        }
                      }
                      if (flag2)
                        stringList.Add(stringOrNull);
                    }
                    if (!flag1 && stringList.Count > 0)
                    {
                      string[] array = new string[stringList.Count];
                      stringList.CopyTo(array);
                      keyQuery = new SQLiteKeyReader.KeyQuery(cnn, keyValuePair.Key, table, array);
                    }
                    for (int index2 = 0; index2 < schema3.Rows.Count; ++index2)
                    {
                      string stringOrNull = SQLiteConvert.GetStringOrNull(schema3.Rows[index2]["COLUMN_NAME"]);
                      keyInfoList.Add(new SQLiteKeyReader.KeyInfo()
                      {
                        rootPage = int32,
                        cursor = cursorForTable,
                        database = database,
                        databaseName = keyValuePair.Key,
                        tableName = table,
                        columnName = stringOrNull,
                        query = keyQuery,
                        column = index2
                      });
                    }
                  }
                }
              }
            }
          }
        }
      }
      this._keyInfo = new SQLiteKeyReader.KeyInfo[keyInfoList.Count];
      keyInfoList.CopyTo(this._keyInfo);
      this._rowIdInfo = new SQLiteKeyReader.RowIdInfo[rowIdInfoList.Count];
      rowIdInfoList.CopyTo(this._rowIdInfo);
    }

    internal int GetRowIdIndex(string databaseName, string tableName)
    {
      if (this._rowIdInfo != null && databaseName != null && tableName != null)
      {
        for (int index = 0; index < this._rowIdInfo.Length; ++index)
        {
          if (this._rowIdInfo[index].databaseName == databaseName && this._rowIdInfo[index].tableName == tableName)
            return this._rowIdInfo[index].column;
        }
      }
      return -1;
    }

    internal long? GetRowId(string databaseName, string tableName)
    {
      if (this._keyInfo != null && databaseName != null && tableName != null)
      {
        for (int index = 0; index < this._keyInfo.Length; ++index)
        {
          if (this._keyInfo[index].databaseName == databaseName && this._keyInfo[index].tableName == tableName)
          {
            long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[index].cursor);
            if (rowIdForCursor != 0L)
              return new long?(rowIdForCursor);
          }
        }
      }
      return new long?();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteKeyReader).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this._stmt = (SQLiteStatement) null;
        if (this._keyInfo != null)
        {
          for (int index = 0; index < this._keyInfo.Length; ++index)
          {
            if (this._keyInfo[index].query != null)
              this._keyInfo[index].query.Dispose();
          }
          this._keyInfo = (SQLiteKeyReader.KeyInfo[]) null;
        }
      }
      this.disposed = true;
    }

    ~SQLiteKeyReader() => this.Dispose(false);

    /// <summary>How many additional columns of keyinfo we're holding</summary>
    internal int Count => this._keyInfo != null ? this._keyInfo.Length : 0;

    private void Sync(int i)
    {
      this.Sync();
      if (this._keyInfo[i].cursor == -1)
        throw new InvalidCastException();
    }

    /// <summary>
    /// Make sure all the subqueries are open and ready and sync'd with the current rowid
    /// of the table they're supporting
    /// </summary>
    private void Sync()
    {
      if (this._isValid)
        return;
      SQLiteKeyReader.KeyQuery keyQuery = (SQLiteKeyReader.KeyQuery) null;
      for (int index = 0; index < this._keyInfo.Length; ++index)
      {
        if (this._keyInfo[index].query == null || this._keyInfo[index].query != keyQuery)
        {
          keyQuery = this._keyInfo[index].query;
          keyQuery?.Sync(this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[index].cursor));
        }
      }
      this._isValid = true;
    }

    /// <summary>Release any readers on any subqueries</summary>
    internal void Reset()
    {
      this._isValid = false;
      if (this._keyInfo == null)
        return;
      for (int index = 0; index < this._keyInfo.Length; ++index)
      {
        if (this._keyInfo[index].query != null)
          this._keyInfo[index].query.IsValid = false;
      }
    }

    internal string GetDataTypeName(int i)
    {
      this.Sync();
      return this._keyInfo[i].query != null ? this._keyInfo[i].query._reader.GetDataTypeName(this._keyInfo[i].column) : "integer";
    }

    internal TypeAffinity GetFieldAffinity(int i)
    {
      this.Sync();
      return this._keyInfo[i].query != null ? this._keyInfo[i].query._reader.GetFieldAffinity(this._keyInfo[i].column) : TypeAffinity.Uninitialized;
    }

    internal Type GetFieldType(int i)
    {
      this.Sync();
      return this._keyInfo[i].query != null ? this._keyInfo[i].query._reader.GetFieldType(this._keyInfo[i].column) : typeof (long);
    }

    internal string GetDatabaseName(int i) => this._keyInfo[i].databaseName;

    internal string GetTableName(int i) => this._keyInfo[i].tableName;

    internal string GetName(int i) => this._keyInfo[i].columnName;

    internal int GetOrdinal(string name)
    {
      for (int index = 0; index < this._keyInfo.Length; ++index)
      {
        if (string.Compare(name, this._keyInfo[index].columnName, StringComparison.OrdinalIgnoreCase) == 0)
          return index;
      }
      return -1;
    }

    internal SQLiteBlob GetBlob(int i, bool readOnly)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetBlob(this._keyInfo[i].column, readOnly);
      throw new InvalidCastException();
    }

    internal bool GetBoolean(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetBoolean(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal byte GetByte(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetByte(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetBytes(this._keyInfo[i].column, fieldOffset, buffer, bufferoffset, length);
      throw new InvalidCastException();
    }

    internal char GetChar(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetChar(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetChars(this._keyInfo[i].column, fieldOffset, buffer, bufferoffset, length);
      throw new InvalidCastException();
    }

    internal DateTime GetDateTime(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetDateTime(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal Decimal GetDecimal(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetDecimal(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal double GetDouble(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetDouble(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal float GetFloat(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetFloat(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal Guid GetGuid(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetGuid(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal short GetInt16(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetInt16(this._keyInfo[i].column);
      long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
      return rowIdForCursor != 0L ? Convert.ToInt16(rowIdForCursor) : throw new InvalidCastException();
    }

    internal int GetInt32(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetInt32(this._keyInfo[i].column);
      long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
      return rowIdForCursor != 0L ? Convert.ToInt32(rowIdForCursor) : throw new InvalidCastException();
    }

    internal long GetInt64(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetInt64(this._keyInfo[i].column);
      long rowIdForCursor = this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor);
      return rowIdForCursor != 0L ? rowIdForCursor : throw new InvalidCastException();
    }

    internal string GetString(int i)
    {
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetString(this._keyInfo[i].column);
      throw new InvalidCastException();
    }

    internal object GetValue(int i)
    {
      if (this._keyInfo[i].cursor == -1)
        return (object) DBNull.Value;
      this.Sync(i);
      if (this._keyInfo[i].query != null)
        return this._keyInfo[i].query._reader.GetValue(this._keyInfo[i].column);
      return this.IsDBNull(i) ? (object) DBNull.Value : (object) this.GetInt64(i);
    }

    internal bool IsDBNull(int i)
    {
      if (this._keyInfo[i].cursor == -1)
        return true;
      this.Sync(i);
      return this._keyInfo[i].query != null ? this._keyInfo[i].query._reader.IsDBNull(this._keyInfo[i].column) : this._stmt._sql.GetRowIdForCursor(this._stmt, this._keyInfo[i].cursor) == 0L;
    }

    /// <summary>
    /// Append all the columns we've added to the original query to the schema
    /// </summary>
    /// <param name="tbl"></param>
    internal void AppendSchemaTable(DataTable tbl)
    {
      SQLiteKeyReader.KeyQuery keyQuery = (SQLiteKeyReader.KeyQuery) null;
      for (int index = 0; index < this._keyInfo.Length; ++index)
      {
        if (this._keyInfo[index].query == null || this._keyInfo[index].query != keyQuery)
        {
          keyQuery = this._keyInfo[index].query;
          if (keyQuery == null)
          {
            DataRow row = tbl.NewRow();
            row[SchemaTableColumn.ColumnName] = (object) this._keyInfo[index].columnName;
            row[SchemaTableColumn.ColumnOrdinal] = (object) tbl.Rows.Count;
            row[SchemaTableColumn.ColumnSize] = (object) 8;
            row[SchemaTableColumn.NumericPrecision] = (object) (int) byte.MaxValue;
            row[SchemaTableColumn.NumericScale] = (object) (int) byte.MaxValue;
            row[SchemaTableColumn.ProviderType] = (object) DbType.Int64;
            row[SchemaTableColumn.IsLong] = (object) false;
            row[SchemaTableColumn.AllowDBNull] = (object) false;
            row[SchemaTableOptionalColumn.IsReadOnly] = (object) false;
            row[SchemaTableOptionalColumn.IsRowVersion] = (object) false;
            row[SchemaTableColumn.IsUnique] = (object) false;
            row[SchemaTableColumn.IsKey] = (object) true;
            row[SchemaTableColumn.DataType] = (object) typeof (long);
            row[SchemaTableOptionalColumn.IsHidden] = (object) true;
            row[SchemaTableColumn.BaseColumnName] = (object) this._keyInfo[index].columnName;
            row[SchemaTableColumn.IsExpression] = (object) false;
            row[SchemaTableColumn.IsAliased] = (object) false;
            row[SchemaTableColumn.BaseTableName] = (object) this._keyInfo[index].tableName;
            row[SchemaTableOptionalColumn.BaseCatalogName] = (object) this._keyInfo[index].databaseName;
            row[SchemaTableOptionalColumn.IsAutoIncrement] = (object) true;
            row["DataTypeName"] = (object) "integer";
            tbl.Rows.Add(row);
          }
          else
          {
            keyQuery.Sync(0L);
            using (DataTable schemaTable = keyQuery._reader.GetSchemaTable())
            {
              foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
              {
                object[] itemArray = row.ItemArray;
                DataRow dataRow = tbl.Rows.Add(itemArray);
                dataRow[SchemaTableOptionalColumn.IsHidden] = (object) true;
                dataRow[SchemaTableColumn.ColumnOrdinal] = (object) (tbl.Rows.Count - 1);
              }
            }
          }
        }
      }
    }

    /// <summary>Used to support CommandBehavior.KeyInfo</summary>
    private struct KeyInfo
    {
      internal string databaseName;
      internal string tableName;
      internal string columnName;
      internal int database;
      internal int rootPage;
      internal int cursor;
      internal SQLiteKeyReader.KeyQuery query;
      internal int column;
    }

    /// <summary>
    /// Used to keep track of the per-table RowId column metadata.
    /// </summary>
    private struct RowIdInfo
    {
      internal string databaseName;
      internal string tableName;
      internal int column;
    }

    /// <summary>A single sub-query for a given table/database.</summary>
    private sealed class KeyQuery : IDisposable
    {
      private SQLiteCommand _command;
      internal SQLiteDataReader _reader;
      private bool disposed;

      internal KeyQuery(
        SQLiteConnection cnn,
        string database,
        string table,
        params string[] columns)
      {
        using (SQLiteCommandBuilder liteCommandBuilder = new SQLiteCommandBuilder())
        {
          this._command = cnn.CreateCommand();
          for (int index = 0; index < columns.Length; ++index)
            columns[index] = liteCommandBuilder.QuoteIdentifier(columns[index]);
        }
        this._command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT {0} FROM [{1}].[{2}] WHERE ROWID = ?", (object) string.Join(",", columns), (object) database, (object) table);
        this._command.Parameters.AddWithValue((string) null, (object) 0L);
      }

      internal bool IsValid
      {
        set
        {
          if (value)
            throw new ArgumentException();
          if (this._reader == null)
            return;
          this._reader.Dispose();
          this._reader = (SQLiteDataReader) null;
        }
      }

      internal void Sync(long rowid)
      {
        this.IsValid = false;
        this._command.Parameters[0].Value = (object) rowid;
        this._reader = this._command.ExecuteReader();
        this._reader.Read();
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      private void CheckDisposed()
      {
        if (this.disposed)
          throw new ObjectDisposedException(typeof (SQLiteKeyReader.KeyQuery).Name);
      }

      private void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        if (disposing)
        {
          this.IsValid = false;
          if (this._command != null)
            this._command.Dispose();
          this._command = (SQLiteCommand) null;
        }
        this.disposed = true;
      }

      ~KeyQuery() => this.Dispose(false);
    }
  }
}
