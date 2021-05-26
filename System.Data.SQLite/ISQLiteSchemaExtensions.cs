// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ISQLiteSchemaExtensions
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// 
  /// </summary>
  public interface ISQLiteSchemaExtensions
  {
    /// <summary>
    /// Creates temporary tables on the connection so schema information can be queried.
    /// </summary>
    /// <param name="connection">
    /// The connection upon which to build the schema tables.
    /// </param>
    void BuildTempSchema(SQLiteConnection connection);
  }
}
