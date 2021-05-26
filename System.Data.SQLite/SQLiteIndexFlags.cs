// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexFlags
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These are the allowed values for the index flags from the
  /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
  /// </summary>
  [Flags]
  public enum SQLiteIndexFlags
  {
    /// <summary>No special handling.  This is the default.</summary>
    None = 0,
    /// <summary>
    /// This value indicates that the scan of the index will visit at
    /// most one row.
    /// </summary>
    ScanUnique = 1,
  }
}
