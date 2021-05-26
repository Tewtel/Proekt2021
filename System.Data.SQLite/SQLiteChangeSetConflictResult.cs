// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetConflictResult
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This enumerated type represents the result of a user-defined conflict
  /// resolution callback.
  /// </summary>
  public enum SQLiteChangeSetConflictResult
  {
    /// <summary>
    /// If a conflict callback returns this value no special action is
    /// taken. The change that caused the conflict is not applied. The
    /// application of changes continues with the next change.
    /// </summary>
    Omit,
    /// <summary>
    /// This value may only be returned from a conflict callback if the
    /// type of conflict was <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Data" />
    /// or <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Conflict" />. If this is
    /// not the case, any changes applied so far are rolled back and the
    /// call to
    /// <see cref="M:System.Data.SQLite.ISQLiteChangeSet.Apply(System.Data.SQLite.SessionConflictCallback,System.Data.SQLite.SessionTableFilterCallback,System.Object)" />
    /// will raise a <see cref="T:System.Data.SQLite.SQLiteException" /> with an error code of
    /// <see cref="F:System.Data.SQLite.SQLiteErrorCode.Misuse" />.
    /// 
    /// If this value is returned for a
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Data" /> conflict, then the
    /// conflicting row is either updated or deleted, depending on the type
    /// of change.
    /// 
    /// If this value is returned for a
    /// <see cref="F:System.Data.SQLite.SQLiteChangeSetConflictType.Conflict" /> conflict, then
    /// the conflicting row is removed from the database and a second
    /// attempt to apply the change is made. If this second attempt fails,
    /// the original row is restored to the database before continuing.
    /// </summary>
    Replace,
    /// <summary>
    /// If this value is returned, any changes applied so far are rolled
    /// back and the call to
    /// <see cref="M:System.Data.SQLite.ISQLiteChangeSet.Apply(System.Data.SQLite.SessionConflictCallback,System.Data.SQLite.SessionTableFilterCallback,System.Object)" />
    /// will raise a <see cref="T:System.Data.SQLite.SQLiteException" /> with an error code of
    /// <see cref="F:System.Data.SQLite.SQLiteErrorCode.Abort" />.
    /// </summary>
    Abort,
  }
}
