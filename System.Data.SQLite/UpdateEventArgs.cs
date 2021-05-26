// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UpdateEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Passed during an Update callback, these event arguments detail the type of update operation being performed
  /// on the given connection.
  /// </summary>
  public class UpdateEventArgs : EventArgs
  {
    /// <summary>
    /// The name of the database being updated (usually "main" but can be any attached or temporary database)
    /// </summary>
    public readonly string Database;
    /// <summary>The name of the table being updated</summary>
    public readonly string Table;
    /// <summary>
    /// The type of update being performed (insert/update/delete)
    /// </summary>
    public readonly UpdateEventType Event;
    /// <summary>The RowId affected by this update.</summary>
    public readonly long RowId;

    internal UpdateEventArgs(string database, string table, UpdateEventType eventType, long rowid)
    {
      this.Database = database;
      this.Table = table;
      this.Event = eventType;
      this.RowId = rowid;
    }
  }
}
