// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetMetadataItem
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface implements properties and methods used to fetch metadata
  /// about one change within a set of changes for a database.
  /// </summary>
  internal sealed class SQLiteChangeSetMetadataItem : ISQLiteChangeSetMetadataItem, IDisposable
  {
    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteChangeSetIterator" /> instance to use.  This
    /// will NOT be owned by this class and will not be disposed upon this
    /// class being disposed or finalized.
    /// </summary>
    private SQLiteChangeSetIterator iterator;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.TableName" /> property. This value
    /// will be null if this field has not yet been populated via the
    /// underlying native API.
    /// </summary>
    private string tableName;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.NumberOfColumns" /> property. This
    /// value will be null if this field has not yet been populated via the
    /// underlying native API.
    /// </summary>
    private int? numberOfColumns;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.OperationCode" /> property.  This
    /// value will be null if this field has not yet been populated via the
    /// underlying native API.
    /// </summary>
    private SQLiteAuthorizerActionCode? operationCode;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.Indirect" /> property.  This value
    /// will be null if this field has not yet been populated via the
    /// underlying native API.
    /// </summary>
    private bool? indirect;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.PrimaryKeyColumns" /> property.
    /// This value will be null if this field has not yet been populated
    /// via the underlying native API.
    /// </summary>
    private bool[] primaryKeyColumns;
    /// <summary>
    /// Backing field for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.NumberOfForeignKeyConflicts" />
    /// property.  This value will be null if this field has not yet been
    /// populated via the underlying native API.
    /// </summary>
    private int? numberOfForeignKeyConflicts;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified iterator
    /// instance.
    /// </summary>
    /// <param name="iterator">The managed iterator instance to use.</param>
    public SQLiteChangeSetMetadataItem(SQLiteChangeSetIterator iterator) => this.iterator = iterator;

    /// <summary>
    /// Throws an exception if the managed iterator instance is invalid.
    /// </summary>
    private void CheckIterator()
    {
      if (this.iterator == null)
        throw new InvalidOperationException("iterator unavailable");
      this.iterator.CheckHandle();
    }

    /// <summary>
    /// Populates the underlying data for the <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.TableName" />,
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.NumberOfColumns" />, <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.OperationCode" />, and
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.Indirect" /> properties, using the appropriate native
    /// API.
    /// </summary>
    private void PopulateOperationMetadata()
    {
      if (this.tableName != null && this.numberOfColumns.HasValue && (this.operationCode.HasValue && this.indirect.HasValue))
        return;
      this.CheckIterator();
      IntPtr zero = IntPtr.Zero;
      SQLiteAuthorizerActionCode op = SQLiteAuthorizerActionCode.None;
      int bIndirect = 0;
      int nColumns = 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_op(this.iterator.GetIntPtr(), ref zero, ref nColumns, ref op, ref bIndirect);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changeset_op");
      this.tableName = SQLiteString.StringFromUtf8IntPtr(zero);
      this.numberOfColumns = new int?(nColumns);
      this.operationCode = new SQLiteAuthorizerActionCode?(op);
      this.indirect = new bool?(bIndirect != 0);
    }

    /// <summary>
    /// Populates the underlying data for the
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.PrimaryKeyColumns" /> property using the appropriate
    /// native API.
    /// </summary>
    private void PopulatePrimaryKeyColumns()
    {
      if (this.primaryKeyColumns != null)
        return;
      this.CheckIterator();
      IntPtr zero = IntPtr.Zero;
      int nColumns = 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_pk(this.iterator.GetIntPtr(), ref zero, ref nColumns);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changeset_pk");
      byte[] numArray = SQLiteBytes.FromIntPtr(zero, nColumns);
      if (numArray == null)
        return;
      this.primaryKeyColumns = new bool[nColumns];
      for (int index = 0; index < numArray.Length; ++index)
        this.primaryKeyColumns[index] = numArray[index] != (byte) 0;
    }

    /// <summary>
    /// Populates the underlying data for the
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.NumberOfForeignKeyConflicts" /> property using the
    /// appropriate native API.
    /// </summary>
    private void PopulateNumberOfForeignKeyConflicts()
    {
      if (this.numberOfForeignKeyConflicts.HasValue)
        return;
      this.CheckIterator();
      int conflicts = 0;
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3changeset_fk_conflicts(this.iterator.GetIntPtr(), ref conflicts);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, "sqlite3changeset_fk_conflicts");
      this.numberOfForeignKeyConflicts = new int?(conflicts);
    }

    /// <summary>The name of the table the change was made to.</summary>
    public string TableName
    {
      get
      {
        this.CheckDisposed();
        this.PopulateOperationMetadata();
        return this.tableName;
      }
    }

    /// <summary>
    /// The number of columns impacted by this change.  This value can be
    /// used to determine the highest valid column index that may be used
    /// with the <see cref="M:System.Data.SQLite.SQLiteChangeSetMetadataItem.GetOldValue(System.Int32)" />, <see cref="M:System.Data.SQLite.SQLiteChangeSetMetadataItem.GetNewValue(System.Int32)" />,
    /// and <see cref="M:System.Data.SQLite.SQLiteChangeSetMetadataItem.GetConflictValue(System.Int32)" /> methods of this interface.  It
    /// will be this value minus one.
    /// </summary>
    public int NumberOfColumns
    {
      get
      {
        this.CheckDisposed();
        this.PopulateOperationMetadata();
        return this.numberOfColumns.Value;
      }
    }

    /// <summary>
    /// This will contain the value
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Insert" />,
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" />, or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Delete" />, corresponding to
    /// the overall type of change this item represents.
    /// </summary>
    public SQLiteAuthorizerActionCode OperationCode
    {
      get
      {
        this.CheckDisposed();
        this.PopulateOperationMetadata();
        return this.operationCode.Value;
      }
    }

    /// <summary>
    /// Non-zero if this change is considered to be indirect (i.e. as
    /// though they were made via a trigger or foreign key action).
    /// </summary>
    public bool Indirect
    {
      get
      {
        this.CheckDisposed();
        this.PopulateOperationMetadata();
        return this.indirect.Value;
      }
    }

    /// <summary>
    /// This array contains a <see cref="T:System.Boolean" /> for each column in
    /// the table associated with this change.  The element will be zero
    /// if the column is not part of the primary key; otherwise, it will
    /// be non-zero.
    /// </summary>
    public bool[] PrimaryKeyColumns
    {
      get
      {
        this.CheckDisposed();
        this.PopulatePrimaryKeyColumns();
        return this.primaryKeyColumns;
      }
    }

    /// <summary>
    /// This method may only be called from within a
    /// <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate when the conflict
    /// type is <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.ForeignKey" />.  It
    /// returns the total number of known foreign key violations in the
    /// destination database.
    /// </summary>
    public int NumberOfForeignKeyConflicts
    {
      get
      {
        this.CheckDisposed();
        this.PopulateNumberOfForeignKeyConflicts();
        return this.numberOfForeignKeyConflicts.Value;
      }
    }

    /// <summary>
    /// Queries and returns the original value of a given column for this
    /// change.  This method may only be called when the
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.OperationCode" /> has a value of
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" /> or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Delete" />.
    /// </summary>
    /// <param name="columnIndex">
    /// The index for the column.  This value must be between zero and one
    /// less than the total number of columns for this table.
    /// </param>
    /// <returns>The original value of a given column for this change.</returns>
    public SQLiteValue GetOldValue(int columnIndex)
    {
      this.CheckDisposed();
      this.CheckIterator();
      IntPtr zero = IntPtr.Zero;
      int num = (int) UnsafeNativeMethods.sqlite3changeset_old(this.iterator.GetIntPtr(), columnIndex, ref zero);
      return SQLiteValue.FromIntPtr(zero);
    }

    /// <summary>
    /// Queries and returns the updated value of a given column for this
    /// change.  This method may only be called when the
    /// <see cref="P:System.Data.SQLite.SQLiteChangeSetMetadataItem.OperationCode" /> has a value of
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Insert" /> or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" />.
    /// </summary>
    /// <param name="columnIndex">
    /// The index for the column.  This value must be between zero and one
    /// less than the total number of columns for this table.
    /// </param>
    /// <returns>The updated value of a given column for this change.</returns>
    public SQLiteValue GetNewValue(int columnIndex)
    {
      this.CheckDisposed();
      this.CheckIterator();
      IntPtr zero = IntPtr.Zero;
      int num = (int) UnsafeNativeMethods.sqlite3changeset_new(this.iterator.GetIntPtr(), columnIndex, ref zero);
      return SQLiteValue.FromIntPtr(zero);
    }

    /// <summary>
    /// Queries and returns the conflicting value of a given column for
    /// this change.  This method may only be called from within a
    /// <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate when the conflict
    /// type is <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Data" /> or
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Conflict" />.
    /// </summary>
    /// <param name="columnIndex">
    /// The index for the column.  This value must be between zero and one
    /// less than the total number of columns for this table.
    /// </param>
    /// <returns>
    /// The conflicting value of a given column for this change.
    /// </returns>
    public SQLiteValue GetConflictValue(int columnIndex)
    {
      this.CheckDisposed();
      this.CheckIterator();
      IntPtr zero = IntPtr.Zero;
      int num = (int) UnsafeNativeMethods.sqlite3changeset_conflict(this.iterator.GetIntPtr(), columnIndex, ref zero);
      return SQLiteValue.FromIntPtr(zero);
    }

    /// <summary>Disposes of this object instance.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteChangeSetMetadataItem).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    private void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing || this.iterator == null)
          return;
        this.iterator = (SQLiteChangeSetIterator) null;
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteChangeSetMetadataItem() => this.Dispose(false);
  }
}
