// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetConflictType
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This enumerated type represents a type of conflict seen when apply
  /// changes from a change set or patch set.
  /// </summary>
  public enum SQLiteChangeSetConflictType
  {
    /// <summary>
    /// This value is seen when processing a DELETE or UPDATE change if a
    /// row with the required PRIMARY KEY fields is present in the
    /// database, but one or more other (non primary-key) fields modified
    /// by the update do not contain the expected "before" values.
    /// </summary>
    Data = 1,
    /// <summary>
    /// This value is seen when processing a DELETE or UPDATE change if a
    /// row with the required PRIMARY KEY fields is not present in the
    /// database.  There is no conflicting row in this case.
    /// 
    /// The results of invoking the
    /// <see cref="M:System.Data.SQLite.ISQLiteChangeSetMetadataItem.GetConflictValue(System.Int32)" />
    /// method are undefined.
    /// </summary>
    NotFound = 2,
    /// <summary>
    /// This value is seen when processing an INSERT change if the
    /// operation would result in duplicate primary key values.
    /// The conflicting row in this case is the database row with the
    /// matching primary key.
    /// </summary>
    Conflict = 3,
    /// <summary>
    /// If a non-foreign key constraint violation occurs while applying a
    /// change (i.e. a UNIQUE, CHECK or NOT NULL constraint), the conflict
    /// callback will see this value.
    /// 
    /// There is no conflicting row in this case. The results of invoking
    /// the <see cref="M:System.Data.SQLite.ISQLiteChangeSetMetadataItem.GetConflictValue(System.Int32)" />
    /// method are undefined.
    /// </summary>
    Constraint = 4,
    /// <summary>
    /// If foreign key handling is enabled, and applying a changes leaves
    /// the database in a state containing foreign key violations, this
    /// value will be seen exactly once before the changes are committed.
    /// If the conflict handler
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictResult.Omit" />, the changes,
    /// including those that caused the foreign key constraint violation,
    /// are committed. Or, if it returns
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictResult.Abort" />, the changes are
    /// rolled back.
    /// 
    /// No current or conflicting row information is provided. The only
    /// method it is possible to call on the supplied
    /// <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" /> object is
    /// <see cref="P:System.Data.SQLite.ISQLiteChangeSetMetadataItem.NumberOfForeignKeyConflicts" />.
    /// </summary>
    ForeignKey = 5,
  }
}
