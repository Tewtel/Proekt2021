// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SessionTableFilterCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This callback is invoked when a determination must be made about
  /// whether changes to a specific table should be tracked -OR- applied.
  /// It will not be called for tables that are already attached to a
  /// <see cref="T:System.Data.SQLite.ISQLiteSession" />.
  /// </summary>
  /// <param name="clientData">
  /// The optional application-defined context data that was originally
  /// passed to the <see cref="M:System.Data.SQLite.ISQLiteSession.SetTableFilter(System.Data.SQLite.SessionTableFilterCallback,System.Object)" /> or
  /// <see cref="M:System.Data.SQLite.ISQLiteChangeSet.Apply(System.Data.SQLite.SessionConflictCallback,System.Data.SQLite.SessionTableFilterCallback,System.Object)" />
  /// methods.  This value may be null.
  /// </param>
  /// <param name="name">The name of the table.</param>
  /// <returns>
  /// Non-zero if changes to the table should be considered; otherwise,
  /// zero.  Throwing an exception from this callback will result in
  /// undefined behavior.
  /// </returns>
  public delegate bool SessionTableFilterCallback(object clientData, string name);
}
