// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteChangeSetMetadataItem
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface contains properties and methods used to fetch metadata
  /// about one change within a set of changes for a database.
  /// </summary>
  public interface ISQLiteChangeSetMetadataItem : IDisposable
  {
    /// <summary>The name of the table the change was made to.</summary>
    string TableName { get; }

    /// <summary>
    /// The number of columns impacted by this change.  This value can be
    /// used to determine the highest valid column index that may be used
    /// with the <see cref="M:System.Data.SQLite.ISQLiteChangeSetMetadataItem.GetOldValue(System.Int32)" />, <see cref="M:System.Data.SQLite.ISQLiteChangeSetMetadataItem.GetNewValue(System.Int32)" />,
    /// and <see cref="M:System.Data.SQLite.ISQLiteChangeSetMetadataItem.GetConflictValue(System.Int32)" /> methods of this interface.  It
    /// will be this value minus one.
    /// </summary>
    int NumberOfColumns { get; }

    /// <summary>
    /// This will contain the value
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Insert" />,
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" />, or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Delete" />, corresponding to
    /// the overall type of change this item represents.
    /// </summary>
    SQLiteAuthorizerActionCode OperationCode { get; }

    /// <summary>
    /// Non-zero if this change is considered to be indirect (i.e. as
    /// though they were made via a trigger or foreign key action).
    /// </summary>
    bool Indirect { get; }

    /// <summary>
    /// This array contains a <see cref="T:System.Boolean" /> for each column in
    /// the table associated with this change.  The element will be zero
    /// if the column is not part of the primary key; otherwise, it will
    /// be non-zero.
    /// </summary>
    bool[] PrimaryKeyColumns { get; }

    /// <summary>
    /// This method may only be called from within a
    /// <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate when the conflict
    /// type is <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.ForeignKey" />.  It
    /// returns the total number of known foreign key violations in the
    /// destination database.
    /// </summary>
    int NumberOfForeignKeyConflicts { get; }

    /// <summary>
    /// Queries and returns the original value of a given column for this
    /// change.  This method may only be called when the
    /// <see cref="P:System.Data.SQLite.ISQLiteChangeSetMetadataItem.OperationCode" /> has a value of
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" /> or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Delete" />.
    /// </summary>
    /// <param name="columnIndex">
    /// The index for the column.  This value must be between zero and one
    /// less than the total number of columns for this table.
    /// </param>
    /// <returns>The original value of a given column for this change.</returns>
    SQLiteValue GetOldValue(int columnIndex);

    /// <summary>
    /// Queries and returns the updated value of a given column for this
    /// change.  This method may only be called when the
    /// <see cref="P:System.Data.SQLite.ISQLiteChangeSetMetadataItem.OperationCode" /> has a value of
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Insert" /> or
    /// <see cref="F:System.Data.SQLite.SQLiteAuthorizerActionCode.Update" />.
    /// </summary>
    /// <param name="columnIndex">
    /// The index for the column.  This value must be between zero and one
    /// less than the total number of columns for this table.
    /// </param>
    /// <returns>The updated value of a given column for this change.</returns>
    SQLiteValue GetNewValue(int columnIndex);

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
    SQLiteValue GetConflictValue(int columnIndex);
  }
}
