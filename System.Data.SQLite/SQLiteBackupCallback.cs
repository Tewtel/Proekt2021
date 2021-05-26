// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBackupCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>Raised between each backup step.</summary>
  /// <param name="source">The source database connection.</param>
  /// <param name="sourceName">The source database name.</param>
  /// <param name="destination">The destination database connection.</param>
  /// <param name="destinationName">The destination database name.</param>
  /// <param name="pages">The number of pages copied with each step.</param>
  /// <param name="remainingPages">
  /// The number of pages remaining to be copied.
  /// </param>
  /// <param name="totalPages">
  /// The total number of pages in the source database.
  /// </param>
  /// <param name="retry">
  /// Set to true if the operation needs to be retried due to database
  /// locking issues; otherwise, set to false.
  /// </param>
  /// <returns>
  /// True to continue with the backup process or false to halt the backup
  /// process, rolling back any changes that have been made so far.
  /// </returns>
  public delegate bool SQLiteBackupCallback(
    SQLiteConnection source,
    string sourceName,
    SQLiteConnection destination,
    string destinationName,
    int pages,
    int remainingPages,
    int totalPages,
    bool retry);
}
