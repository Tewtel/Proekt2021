// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteConnectionPool
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This interface represents a custom connection pool implementation
  /// usable by System.Data.SQLite.
  /// </summary>
  public interface ISQLiteConnectionPool
  {
    /// <summary>
    /// Counts the number of pool entries matching the specified file name.
    /// </summary>
    /// <param name="fileName">
    /// The file name to match or null to match all files.
    /// </param>
    /// <param name="counts">
    /// The pool entry counts for each matching file.
    /// </param>
    /// <param name="openCount">
    /// The total number of connections successfully opened from any pool.
    /// </param>
    /// <param name="closeCount">
    /// The total number of connections successfully closed from any pool.
    /// </param>
    /// <param name="totalCount">
    /// The total number of pool entries for all matching files.
    /// </param>
    void GetCounts(
      string fileName,
      ref Dictionary<string, int> counts,
      ref int openCount,
      ref int closeCount,
      ref int totalCount);

    /// <summary>
    /// Disposes of all pooled connections associated with the specified
    /// database file name.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    void ClearPool(string fileName);

    /// <summary>Disposes of all pooled connections.</summary>
    void ClearAllPools();

    /// <summary>
    /// Adds a connection to the pool of those associated with the
    /// specified database file name.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    /// <param name="handle">The database connection handle.</param>
    /// <param name="version">
    /// The connection pool version at the point the database connection
    /// handle was received from the connection pool.  This is also the
    /// connection pool version that the database connection handle was
    /// created under.
    /// </param>
    void Add(string fileName, object handle, int version);

    /// <summary>
    /// Removes a connection from the pool of those associated with the
    /// specified database file name with the intent of using it to
    /// interact with the database.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    /// <param name="maxPoolSize">
    /// The new maximum size of the connection pool for the specified
    /// database file name.
    /// </param>
    /// <param name="version">
    /// The connection pool version associated with the returned database
    /// connection handle, if any.
    /// </param>
    /// <returns>
    /// The database connection handle associated with the specified
    /// database file name or null if it cannot be obtained.
    /// </returns>
    object Remove(string fileName, int maxPoolSize, out int version);
  }
}
