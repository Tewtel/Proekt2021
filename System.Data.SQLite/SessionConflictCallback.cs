// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SessionConflictCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This callback is invoked when there is a conflict while apply changes
  /// to a database.
  /// </summary>
  /// <param name="clientData">
  /// The optional application-defined context data that was originally
  /// passed to the
  /// <see cref="M:System.Data.SQLite.ISQLiteChangeSet.Apply(System.Data.SQLite.SessionConflictCallback,System.Data.SQLite.SessionTableFilterCallback,System.Object)" />
  /// method.  This value may be null.
  /// </param>
  /// <param name="type">The type of this conflict.</param>
  /// <param name="item">
  /// The <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" /> object associated with
  /// this conflict.  This value may not be null; however, only properties
  /// that are applicable to the conflict type will be available.  Further
  /// information on this is available within the descriptions of the
  /// available <see cref="T:System.Data.SQLite.SQLiteChangeSetConflictType" /> values.
  /// </param>
  /// <returns>
  /// A <see cref="T:System.Data.SQLite.SQLiteChangeSetConflictResult" /> value that indicates the
  /// action to be taken in order to resolve the conflict.  Throwing an
  /// exception from this callback will result in undefined behavior.
  /// </returns>
  public delegate SQLiteChangeSetConflictResult SessionConflictCallback(
    object clientData,
    SQLiteChangeSetConflictType type,
    ISQLiteChangeSetMetadataItem item);
}
