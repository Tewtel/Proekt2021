// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMetaDataCollectionNames
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>MetaDataCollections specific to SQLite</summary>
  public static class SQLiteMetaDataCollectionNames
  {
    /// <summary>
    /// Returns a list of databases attached to the connection
    /// </summary>
    public static readonly string Catalogs = nameof (Catalogs);
    /// <summary>Returns column information for the specified table</summary>
    public static readonly string Columns = nameof (Columns);
    /// <summary>
    /// Returns index information for the optionally-specified table
    /// </summary>
    public static readonly string Indexes = nameof (Indexes);
    /// <summary>Returns base columns for the given index</summary>
    public static readonly string IndexColumns = nameof (IndexColumns);
    /// <summary>Returns the tables in the given catalog</summary>
    public static readonly string Tables = nameof (Tables);
    /// <summary>Returns user-defined views in the given catalog</summary>
    public static readonly string Views = nameof (Views);
    /// <summary>
    /// Returns underlying column information on the given view
    /// </summary>
    public static readonly string ViewColumns = nameof (ViewColumns);
    /// <summary>Returns foreign key information for the given catalog</summary>
    public static readonly string ForeignKeys = nameof (ForeignKeys);
    /// <summary>Returns the triggers on the database</summary>
    public static readonly string Triggers = nameof (Triggers);
  }
}
